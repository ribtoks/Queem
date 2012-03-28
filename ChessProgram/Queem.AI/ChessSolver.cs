using System;
using Queem.Core;
using Queem.Core.ChessBoard;
using DebutMovesHolder;

namespace Queem.AI
{
	public class ChessSolver
	{
		protected DebutGraph debutGraph;
	
		public ChessSolver (DebutGraph graph)
		{
			this.debutGraph = graph;
		}
		
		protected bool TryFindDebutMove(out Move debutMove)
		{
			debutGraph = null;
		}
		
		public Move SolveProblem(GameProvider provider, Color color, int maxdepth)
		{
			Move bestMove;
			
			if (this.TryFindDebutMove(out bestMove))
				return bestMove;			
		}
	}
}

