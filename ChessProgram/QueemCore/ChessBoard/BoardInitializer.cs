using System;
using Queem.Core.ChessBoard;

namespace Queem.Core
{
	public static class BoardInitializer
	{
		public static readonly InitialFigureShuffler[] Shufflers;
		
		static BoardInitializer()
		{
			Shufflers = CreateShufflers();
		}
		
		private static InitialFigureShuffler[] CreateShufflers()
		{
			var shufflers = new InitialFigureShuffler[6];
			for (int i = 0; i < 6; ++i)
				shufflers[i] = FigureShufflerFactory.CreateShuffler((Figure)i);
			return shufflers;
		}
	}
}

