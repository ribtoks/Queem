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
                this.IsFigureMoving = false;
                this.UnHighlightSquares();

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
            return true;
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
            this.UnHighlightSquares();
            this.lastHighlightedSquares.Clear();
        }

        private void UnHighlightSquares()
        {
            foreach (var item in this.lastHighlightedSquares)
                this.squareItems[item.GetRealIndex()].IsHighlighted = false;
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

        public int GetLastMoveDeltaX()
        {
            int fileStart = (int)BitBoardHelper.GetFileFromSquare(this.MoveStart);
            int fileEnd = (int)BitBoardHelper.GetFileFromSquare(this.MoveEnd);

            return fileEnd - fileStart;
        }

        public int GetLastMoveDeltaY()
        {
            int rankStart = BitBoardHelper.GetRankFromSquare(this.MoveStart);
            int rankEnd = BitBoardHelper.GetRankFromSquare(this.MoveEnd);

            return rankStart - rankEnd;
        }

        public void ReplaceLastMoveFigure()
        {
            Figure figureMoved = this.squareItems[this.moveStart.GetRealIndex()].FigureType;

            this.SetFigure(this.moveEnd, figureMoved, this.CurrentPlayerColor);
            this.SetFigure(this.moveStart, Figure.Nobody, this.CurrentPlayerColor);
            
            this.squareItems[this.moveStart.GetRealIndex()].ResetTransform();
        }

        private void SetFigure(Square square, Figure figure, Color color)
        {
            int index = square.GetRealIndex();
            this.squareItems[index].FigureType = figure;
            this.squareItems[index].FigureColor = color;
        }
    }
}
