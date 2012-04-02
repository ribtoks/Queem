using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChessBoardVisualLib.ViewModel;
using Queem.Core.ChessBoard;
using Queem.Core.History;
using Queem.Core;
using ChessBoardVisualLib.Helpers;
using System.Windows.Controls.Primitives;
using ChessBoardVisualLib.Extensions;

namespace ChessBoardVisualLib.View
{
    /// <summary>
    /// Interaction logic for ChessBoardControl.xaml
    /// </summary>
    public partial class ChessBoardControl : UserControl
    {
        private ChessBoardViewModel viewModel;

        private int animationsDone;
        private int desiredAnimationsCount;

        public ChessBoardControl()
        {
            InitializeComponent();
        }

        public void SetupGameProvider(GameProvider provider)
        {
            this.viewModel = new ChessBoardViewModel(provider);
            this.DataContext = this.viewModel;
        }

        public void ChangeCurrentPlayer()
        {
            this.viewModel.ChangeCurrentPlayer();
        }

        public Queem.Core.Color CurrentPlayerColor
        {
            get { return this.viewModel.CurrentPlayerColor; }
        }

        public event EventHandler MoveAnimationPreview;

        private void OnMoveAnimationPreview()
        {
            this.IsHitTestVisible = false;

            if (this.MoveAnimationPreview != null)
                this.MoveAnimationPreview(this, EventArgs.Empty);
        }

        public event EventHandler MoveAnimationFinished;

        private void OnMoveAnimationFinished()
        {
            this.IsHitTestVisible = true;

            if (this.MoveAnimationFinished != null)
                this.MoveAnimationFinished(this, EventArgs.Empty);
        }

        public event EventHandler MoveFinished;

        private void OnMoveFinished()
        {
            if (this.MoveFinished != null)
                this.MoveFinished(this, EventArgs.Empty);
        }

        public event EventHandler PawnPromoted;

        private void OnPawnPromoted()
        {
            if (this.PawnPromoted != null)
                this.PawnPromoted(this, EventArgs.Empty);
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as Grid;
            var squareItem = element.DataContext as SquareItem;

            var result = this.viewModel.MouseClick(squareItem);

            if (result == Enums.MouseOperationResults.MoveFinished)
                this.OnMoveFinished();
        }

        private void squareGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var element = sender as Grid;
            var squareItem = element.DataContext as SquareItem;

            var result = this.viewModel.MouseUp(squareItem);

            if (result == Enums.MouseOperationResults.MoveFinished)
                this.OnMoveFinished();
        }

        public void AnimateLast()
        {
            var history = this.viewModel.GameProvider.History;
            this.AnimateMove(history.GetLastDeltaChange(), history.GetLastMove());
        }
        
        public void AnimateMove(DeltaChange dc, Move move)
        {
            this.OnMoveAnimationPreview();
            this.SetupAnimationCount(move);

            while (dc.HasItems())
            {
                var change = dc.PopLast();

                switch(change.Action)
                {
                    case MoveAction.Move:
                        this.InnerAnimateMove(new Move(change.Square, change.AdditionalSquare), Queem.Core.Figure.Nobody);
                        break;

                    case MoveAction.Deletion:
                        if (change.Square != move.To)
                            // if not equal - passing capture
                            this.viewModel.RemoveFigure(change.Square);
                        break;

                    case MoveAction.Creation:
                        this.viewModel.UpdateFigure(change.Square,
                            change.FigureType, change.FigureColor);
                        break;
                }
            }
        }

        private void InnerAnimateMove(Move move, Queem.Core.Figure figureDied)
        {
            var uniformGrid = mainGrid.FindChild<UniformGrid>();
            var moveGrid = uniformGrid.Children.OfType<ContentPresenter>()
                .Where((child) =>
                    (child.DataContext as SquareItem).Square == move.From)
                .First();


            int zIndex = Panel.GetZIndex(moveGrid);
            // set zIndex over 9000
            Panel.SetZIndex(moveGrid, 9001);

            this.viewModel.AnimateMove(move,
                moveGrid.ActualWidth,
                (sourceItem) =>
                {
                    if (figureDied != Queem.Core.Figure.Nobody)
                        sourceItem.UpdateChessFigure(figureDied,
                            this.viewModel.CurrentPlayerColor.GetOpposite());

                    Panel.SetZIndex(moveGrid, zIndex);
                    this.animationsDone += 1;

                    if (this.animationsDone == this.desiredAnimationsCount)
                        this.OnMoveAnimationFinished();
                });
        }

        private void SetupAnimationCount(Move move)
        {
            if (move.Type == MoveType.KingCastle)
                this.desiredAnimationsCount = 2;
            else
                this.desiredAnimationsCount = 1;

            this.animationsDone = 0;
        }

        public void AnimateCancelMove(DeltaChange dc, Move move)
        {
            this.OnMoveAnimationPreview();
            this.SetupAnimationCount(move);

            Queem.Core.Figure figureDied = dc.Filter((ch) =>
                (ch.Action == MoveAction.Deletion) &&
                (ch.FigureColor == this.viewModel.CurrentPlayerColor) &&
                (ch.Square == move.To)).First().FigureType;

            while (dc.HasItems())
            {
                var change = dc.PopLast();

                switch (change.Action)
                {
                    case MoveAction.Move:
                        this.InnerAnimateMove(new Move(change.Square, change.AdditionalSquare), figureDied);
                        break;

                    case MoveAction.Creation:
                        break;

                    case MoveAction.Deletion:
                        if (move.To != change.Square)
                            this.viewModel.UpdateFigure(change.Square, 
                                change.FigureType, change.FigureColor);
                        break;
                }
            }
        }

        private void PromotePawn_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var grid = e.OriginalSource as Grid;
            var promotionItem = grid.DataContext as PawnPromotionItem;

            var square = this.viewModel.MoveEnd;

            this.viewModel.ShowOverlayState = Enums.ShowOverlayState.Hide;
            this.viewModel.PromotePawn(promotionItem.FigureColor, square, promotionItem.FigureType);

            this.OnPawnPromoted();
        }

        public void PromotePawn(Queem.Core.Color color, Square square, Queem.Core.Figure figure)
        {
            this.viewModel.PromotePawn(color, square, figure);
        }

        public void UserPromotePawn()
        {
            this.viewModel.ShowOverlayState = Enums.ShowOverlayState.Show;
        }
    }
}
