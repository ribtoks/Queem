using System;

namespace Queem.CoreInterface.Interface
{
	public abstract class ChessBoard
	{
        public abstract GeneralFigure this[int i, int j] { get; }
        public abstract GeneralFigure this[Coordinates coords] { get; }
	}
}

