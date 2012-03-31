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

namespace ChessBoardVisualLib.ViewModel
{
    public class ChessBoardViewModel : ObservableObject
    {
        private GameProvider provider;
        private ObservableCollection<SquareItem> squareItems;

        private Square moveStart;
        private Square moveEnd;

        private List<Square> lastHighlightedSquares;

        public ChessBoardViewModel(GameProvider gameProvider)
        {
            this.provider = gameProvider;
            this.InitItems();
            this.lastHighlightedSquares = new List<Square>();

            // TODO remove this in future
            this.CurrentPlayerColor = Color.White;
        }

        private void InitItems()
        {
            this.squareItems = new ObservableCollection<SquareItem>();
            this.provider.ForEachFigureReal((square, figure, color) =>
                {
                    this.squareItems.Add(
                        new SquareItem(square, figure, color));
                });
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

        private MoveAnimationState moveAnimationState;
        public MoveAnimationState MoveAnimationState
        {
            get { return this.moveAnimationState; }
            set
            {
                if (this.moveAnimationState != value)
                {
                    this.moveAnimationState = value;
                    OnPropertyChanged("MoveAnimationState");
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

        public MouseClickResults MouseClick(SquareItem item)
        {
            if (this.IsFigureMoving)
            {
                this.StopFigureMoving();

                if (this.TryFinishMove(item))
                    return MouseClickResults.MoveFinished;

                if (item.FigureType == Figure.Nobody)
                    return MouseClickResults.MoveCanceled;

                if (item.FigureColor == this.CurrentPlayerColor)
                    this.InitFigureMoveBegin(item);
            }
            else
            {
                if (item.FigureType == Figure.Nobody)
                    return MouseClickResults.MoveCanceled;

                this.InitFigureMoveBegin(item);
            }

            return MouseClickResults.NewMove;
        }

        private bool TryFinishMove(SquareItem item)
        {
            if (!this.IsLegalMoveEnd(item.Square))
                return false;

            this.MoveEnd = item.Square;
            Move move = new Move(this.moveStart, item.Square);
            this.provider.ProcessMove(move, this.CurrentPlayerColor);
            this.moveAnimationState = MoveAnimationState.Start;
            return true;
        }

        private void StopFigureMoving()
        {
            this.IsFigureMoving = false;
            this.ClearHighligtedSquares();
        }

        private bool IsLegalMoveEnd(Square moveEnd)
        {
            return this.lastHighlightedSquares.Contains(moveEnd);
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
            foreach (var item in this.lastHighlightedSquares)
                this.squareItems[item.GetRealIndex()].IsHighlighted = false;
            this.lastHighlightedSquares.Clear();
        }

        private void SetHighlightedSquares(Square square, Color color)
        {
            this.lastHighlightedSquares = this.provider.GetTargetSquares(
                            square, color);

            foreach (var item in this.lastHighlightedSquares)
                this.squareItems[item.GetRealIndex()].IsHighlighted = true;
        }

        public void ChangeCurrentPlayer()
        {
            if (this.CurrentPlayerColor == Color.White)
                this.CurrentPlayerColor = Color.Black;
            else
                this.CurrentPlayerColor = Color.White;
        }
    }
}
