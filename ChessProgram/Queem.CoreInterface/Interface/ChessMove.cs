using System;

namespace Queem.CoreInterface.Interface
{
	public abstract class ChessMove
    {
        public abstract Coordinates Start { get; }

        public abstract Coordinates End { get; }        
    }
}

