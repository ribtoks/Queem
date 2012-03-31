using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Queem.Core;
using Queem.Core.BitBoards.Helpers;

namespace ChessBoardVisualLib.ViewModel
{
    public class SquareItem : DependencyObject
    {
        public SquareItem(Square square, Figure figure)
        {
            this.SetColor(square);
            this.FigureType = figure;
        }

        public void SetColor(Square square)
        {
            int file = (int)BitBoardHelper.GetFileFromSquare(square);
            int rank = (int)BitBoardHelper.GetRankFromSquare(square);

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
    }
}
