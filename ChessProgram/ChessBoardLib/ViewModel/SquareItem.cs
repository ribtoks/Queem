using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Queem.Core;
using Queem.Core.BitBoards.Helpers;
using ChessBoardVisualLib.Enums;

namespace ChessBoardVisualLib.ViewModel
{
    public static class SquareExtension
    {
        public static int GetRealIndex(this Square sq)
        {
            int file = (int)BitBoardHelper.GetFileFromSquare(sq);
            int rank = BitBoardHelper.GetRankFromSquare(sq);

            return (7 - rank) * 8 + file;
        }
    }

    public class SquareItem : DependencyObject
    {
        private Square innerSquare;

        public SquareItem(Square square, Figure figure, Color color)
        {
            this.innerSquare = square;
            this.SetBackgroundColor(square);
            this.FigureType = figure;
            this.FigureColor = color;
            this.ColoredFigure = ColoredFigureHelper.Create(color, figure);
        }

        public Square Square
        {
            get { return this.innerSquare; }           
        }

        public void SetBackgroundColor(Square square)
        {
            int file = (int)BitBoardHelper.GetFileFromSquare(square);
            int rank = BitBoardHelper.GetRankFromSquare(square);

            if (0 == ((7 - file) + rank) % 2)
                this.SquareColor = Color.White;
            else
                this.SquareColor = Color.Black;
        }

        public static readonly DependencyProperty SquareColorProperty =
            DependencyProperty.Register("SquareColor", typeof(Color), typeof(SquareItem));

        public Color SquareColor
        {
            get { return (Color)GetValue(SquareColorProperty); }
            set { SetValue(SquareColorProperty, value); }
        }

        public static readonly DependencyProperty FigureTypeProperty =
            DependencyProperty.Register("FigureType", typeof(Figure), typeof(SquareItem));

        public Figure FigureType
        {
            get { return (Figure)GetValue(FigureTypeProperty); }
            set { SetValue(FigureTypeProperty, value); }
        }

        public static readonly DependencyProperty ColoredFigureProperty =
            DependencyProperty.Register("ColoredFigure", typeof(ColoredFigure), typeof(SquareItem));

        public ColoredFigure ColoredFigure
        {
            get { return (ColoredFigure)GetValue(ColoredFigureProperty); }
            set { SetValue(ColoredFigureProperty, value); }
        }

        public static readonly DependencyProperty IsHighlightedProperty =
            DependencyProperty.Register("IsHighlighted", typeof(bool), typeof(SquareItem));

        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        public static readonly DependencyProperty FigureColorProperty =
            DependencyProperty.Register("FigureColor", typeof(Color), typeof(SquareItem));

        public Color FigureColor
        {
            get { return (Color)GetValue(FigureColorProperty); }
            set { SetValue(FigureColorProperty, value); }
        }
    }
}
