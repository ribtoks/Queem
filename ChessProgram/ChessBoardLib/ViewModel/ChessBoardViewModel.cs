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

namespace ChessBoardVisualLib.ViewModel
{
    public class ChessBoardViewModel : ObservableObject
    {
        private GameProvider provider;
        private ObservableCollection<SquareItem> squareItems;

        private Square moveStart;

        private List<Square> lastHighlightedSquares;

        public ChessBoardViewModel(GameProvider gameProvider)
        {
            this.provider = gameProvider;
            this.InitItems();
            this.lastHighlightedSquares = new List<Square>();
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

        public ObservableCollection<SquareItem> Squares
        {
            get { return this.squareItems; }
        }

        public Color CurrentPlayerColor
        {
            get;
            set;
        }

        public void MouseClick(SquareItem item)
        {
            if (this.IsFigureMoving)
            {
                this.StopFigureMoving();

                if (this.TryFinishMove(item))
                    return;

                if (item.FigureType == Figure.Nobody)
                    return;

                if (item.FigureColor == this.CurrentPlayerColor)
                    this.InitFigureMoveBegin(item);
            }
            else
            {
                if (item.FigureType == Figure.Nobody)
                    return;

                this.InitFigureMoveBegin(item);
            }
        }

        private bool TryFinishMove(SquareItem item)
        {
            if (!this.IsLegalMoveEnd(item.Square))
                return false;

            Move move = new Move(this.moveStart, item.Square);
            this.provider.ProcessMove(move, this.CurrentPlayerColor);
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
            this.moveStart = item.Square;
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

        private void GetPossibleMoves(SquareItem item)
        {            
        }
    }
}
