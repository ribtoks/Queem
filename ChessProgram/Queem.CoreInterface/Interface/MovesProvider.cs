using System;
using System.Collections.Generic;

namespace Queem.CoreInterface.Interface
{
	public abstract class MovesProvider
	{
		public abstract ChessBoard ChessBoard { get; }
		
		public abstract Player Player1 { get; }
		public abstract Player Player2 { get; }
	}
}

