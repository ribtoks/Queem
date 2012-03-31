using System;
using Queem.Core;

namespace ChessBoardVisualLib.Enums
{
    public enum ColoredFigure
    {
        BlackPawn, BlackKnight, BlackBishop, BlackRook, BlackQueen, BlackKing, BlackNobody,
        WhitePawn, WhiteKnight, WhiteBishop, WhiteRook, WhiteQueen, WhiteKing, WhiteNobody
    }

    public static class ColoredFigureHelper
    {
        public static ColoredFigure Create(Color color, Figure figure)
        {
            string line = string.Format("{0}{1}", color, figure);
            object obj = Enum.Parse(typeof(ColoredFigure), line);

            if (obj is ColoredFigure)
                return (ColoredFigure)obj;

            throw new InvalidOperationException();
        }
    }
}