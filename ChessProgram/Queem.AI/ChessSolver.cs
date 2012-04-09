using System;
using Queem.Core;
using Queem.Core.ChessBoard;
using DebutsLib;

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
			debutMove = null;
            return false;
		}		
		
		public Move SolveProblem(GameProvider provider, Color color, int maxdepth)
		{
			Move bestMove;
			
			if (this.TryFindDebutMove(out bestMove))
				return bestMove;
				
			FixedArray moves;

            if (provider.PlayerBoard1.FigureColor == color)
            {
                moves = provider.PlayerBoard1.GetMoves(provider.PlayerBoard2,
                    provider.History.GetLastMove(),
                    MovesMask.AllMoves);
                provider.FilterMoves(moves, color);
            }
            else
            {
                moves = provider.PlayerBoard2.GetMoves(provider.PlayerBoard1,
                    provider.History.GetLastMove(),
                    MovesMask.AllMoves);
                provider.FilterMoves(moves, provider.PlayerBoard2.FigureColor);
            }
				
			Random rand = new Random(DateTime.Now.Millisecond);
            var move = new Move(moves.InnerArray[rand.Next(moves.Size)]);
            MovesArray.ReleaseLast();
            return move;
		}
	}
}

