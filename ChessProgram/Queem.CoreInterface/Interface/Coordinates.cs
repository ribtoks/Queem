using System;

namespace Queem.CoreInterface.Interface
{
	public enum FigureType { Pawn = 0, Rook = 1, Bishop = 2, Knight = 3, King = 4, Queen = 5,
Nobody = 6, Null = 7 };

    public enum FigureColor { Black = 0, White = 1 };

    public enum FieldLetter { A = 0, B = 1, C = 2, D = 3, E = 4, F = 5, G = 6, H = 7, Below = -1,
BelowBelow = -2, Above = 8, AboveAbove = 9 };

	public abstract class Coordinates
    {
        public abstract int Y { get; }
        
        public abstract int X { get; }
        
        public abstract FieldLetter Letter { get; }
        
        public abstract void Set(int x, int y);
    }
}

