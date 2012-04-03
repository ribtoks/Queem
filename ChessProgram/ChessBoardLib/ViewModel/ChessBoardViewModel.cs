using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Queem.Core.ChessBoard;
using Queem.Core;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Windows.Input;
using ChessBoardVisualLib.Commands;
using ChessBoardVisualLib.Enums;
using System.Windows.Data;
using ChessBoardVisualLib.Converters;
using Queem.Core.BitBoards.Helpers;
using ChessBoardVisualLib.Extensions;
using QueemCore;
using System.Windows.Threading;

namespace ChessBoardVisualLib.ViewModel
{
    public class ChessBoardViewModel : ObservableObject
    {
        private GameProvider provider;
        private ObservableCollection<SquareItem> squareItems;

        private Square moveStart;
        private Square moveEnd;

        private List<HighlightedSquare> lastHighlightedSquares;

        private PawnPromotionViewModel promotionViewModel;
        private ShowOverlayState showOverlayState;

        private Dispatcher dispatcher;

        public ChessBoardViewModel(GameProvider gameProvider)
        {
            this.provider = gameProvider;
            this.squareItems = new ObservableCollection<SquareItem>();

            this.InitItems();
            this.lastHighlightedSquares = new List<HighlightedSquare>();

            // TODO remove this in future
            this.CurrentPlayerColor = Color.White;

            this.promotionViewModel = new PawnPromotionViewModel();
            this.showOverlayState = Enums.ShowOverlayState.Nothing;

            this.dispatcher = Dispatcher.CurrentDispatcher;
        }

        private void InitItems()
        {
            this.squareItems.Clear();
            this.provider.ForEachFigureReal((square, figure, color) =>
                {
                    this.squareItems.Add(
                        new SquareItem(square, figure, color));
                });
        }

        public GameProvider GameProvider
        {
            get { return this.provider; }
        }

        public PawnPromotionViewModel PromotionContext
        {
            get { return this.promotionViewModel; }
        }

        private bool isFigureMoving;
        public bool IsFigureMoving
        {
            get { return this.isFigureMoving; }
            set
            {
                if (this.isFigureMoving != value)
                {
                    this.isFigureMoving = value;
                    OnPropertyChanged("IsFigureMoving");
                }
            }
        }

        public Square MoveStart
        {
            get { return this.moveStart; }
            set
            {
                if (this.moveStart != value)
                {
                    this.moveStart = value;
                    OnPropertyChanged("MoveStart");
                }
            }
        }

        public Square MoveEnd
        {
            get { return this.moveEnd; }
            set
            {
                if (this.moveEnd != value)
                {
                    this.moveEnd = value;
                    OnPropertyChanged("MoveEnd");
                }
            }
        }

        public ObservableCollection<SquareItem> Squares
        {
            get { return this.squareItems; }
        }

        public Color CurrentPlayerColor
        {
            get;
            set;
        }

        public ShowOverlayState ShowOverlayState
        {
            get { return this.showOverlayState; }
            set
            {
                if (this.showOverlayState != value)
                {
                    this.showOverlayState= value;
                    OnPropertyChanged("ShowOverlayState");
                }
            }
        }

        public MouseOperationResults MouseClick(SquareItem item)
        {
            if (this.IsFigureMoving)
            {
                this.IsFigureMoving = false;
                this.UnHighlightSquares();

                if (this.TryFinishMove(item))
                    return MouseOperationResults.MoveFinished;

                if (item.FigureType == Figure.Nobody)
                    return MouseOperationResults.MoveCanceled;

                if (item.FigureColor == this.CurrentPlayerColor)
                    this.InitFigureMoveBegin(item);
            }
            else
            {
                if (item.FigureType == Figure.Nobody)
                    return MouseOperationResults.MoveCanceled;

                this.InitFigureMoveBegin(item);
            }

            return MouseOperationResults.NewMove;
        }

        public MouseOperationResults MouseUp(SquareItem item)
        {
            if (this.IsFigureMoving)
            {
                if (item.Square == this.moveStart)
                    return MouseOperationResults.NewMove;

                this.IsFigureMoving = false;
                this.UnHighlightSquares();

                if (this.TryFinishMove(item))
                    return MouseOperationResults.MoveFinished;
            }

            return MouseOperationResults.MoveCanceled;
        }

        private bool TryFinishMove(SquareItem item)
        {
            MoveType type;
            if (!this.IsLegalMoveEnd(item.Square, out type))
                return false;

            this.MoveEnd = item.Square;
            Move move = new Move(this.moveStart, item.Square, type);
            this.provider.ProcessMove(move, this.CurrentPlayerColor);
            return true;
        }
        
        private bool IsLegalMoveEnd(Square moveEnd, out MoveType type)
        {
            var result = this.lastHighlightedSquares.Where((hs) => hs.Square == moveEnd);

            type = MoveType.Quiet;
            if (result.Count() > 0)
                type = result.First().MoveType;

            return result.Count() > 0;
        }

        public void InitFigureMoveBegin(SquareItem item)
        {
            this.MoveStart = item.Square;
            this.IsFigureMoving = true;
            this.UpdateHighlightedSquares(item.Square, item.FigureColor);
        }

        private void UpdateHighlightedSquares(Square square, Color color)
        {
            this.ClearHighligtedSquares();
            this.SetHighlightedSquares(square, color);
        }

        private void ClearHighligtedSquares()
        {
            this.UnHighlightSquares();
            this.lastHighlightedSquares.Clear();
        }

        private void UnHighlightSquares()
        {
            foreach (var item in this.lastHighlightedSquares)
                this.squareItems[item.Square.GetRealIndex()].IsHighlighted = false;
        }

        private void SetHighlightedSquares(Square square, Color color)
        {
            this.lastHighlightedSquares = this.provider.GetTargetSquares(
                            square, color);

            foreach (var item in this.lastHighlightedSquares)
                this.squareItems[item.Square.GetRealIndex()].IsHighlighted = true;
        }

        public void ChangeCurrentPlayer()
        {
            if (this.CurrentPlayerColor == Color.White)
                this.CurrentPlayerColor = Color.Black;
            else
                this.CurrentPlayerColor = Color.White;
        }
        
        public void AnimateMove(Move move, double width, Action<SquareItem> animationFinishedAction)
        {
            double deltaX = move.GetDeltaX() * width;
            double deltaY = -move.GetDeltaY() * width;

            var sourceItem = this.squareItems[move.From.GetRealIndex()];
            var targetItem = this.squareItems[move.To.GetRealIndex()];
            var figureMoving = sourceItem.FigureType;

            sourceItem.AnimateShift(deltaX, deltaY, (item) =>
                {
                    this.dispatcher.BeginInvoke(new Action(() =>
                        {
                            targetItem.UpdateChessFigure(figureMoving, sourceItem.FigureColor);
                            sourceItem.UpdateChessFigure(Figure.Nobody, sourceItem.FigureColor);
                            sourceItem.ResetTransform();

                            animationFinishedAction(item);
                        }), DispatcherPriority.Render);                    
                });
        }

        public void RemoveFigure(Square square)
        {
            var item = this.squareItems[square.GetRealIndex()];
            item.UpdateChessFigure(Figure.Nobody, this.CurrentPlayerColor);
        }

        public void UpdateFigure(Square square, Figure figure, Color color)
        {
            var item = this.squareItems[square.GetRealIndex()];
            item.UpdateChessFigure(figure, color);
        }

        public void UpdateLayout()
        {
            this.InitItems();
        }

        public void PromotePawn(Color color, Square square, Figure newFigure)
        {
            this.GameProvider.PromotePawn(color, square, newFigure);
            this.UpdateFigure(square, newFigure, color);
        }
    }
}
