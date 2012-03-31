using System;
using Queem.Core.BitBoards;

namespace Queem.Core.ChessBoard
{	
	public abstract class InitialFigureShuffler
	{	
		public void Init(Action<Square> action, PlayerPosition position, Color color)
		{
			if (position == PlayerPosition.Up)
				this.ShuffleUp(action, color);
			else
				this.ShuffleDown(action, color);
		}
		
		protected abstract void ShuffleUp(Action<Square> action, Color color);
		protected abstract void ShuffleDown(Action<Square> action, Color color);
	}
	
	public class PawnFigureShuffler : InitialFigureShuffler
	{	
		protected override void ShuffleUp (Action<Square> action, Color color)
		{			
			action(Square.A7);
			action(Square.B7);
			action(Square.C7);
			action(Square.D7);
			action(Square.E7);
			action(Square.F7);
			action(Square.G7);
			action(Square.H7);
		}
		
		protected override void ShuffleDown (Action<Square> action, Color color)
		{
			action(Square.A2);
			action(Square.B2);
			action(Square.C2);
			action(Square.D2);
			action(Square.E2);
			action(Square.F2);
			action(Square.G2);
			action(Square.H2);
		}
	}
	
	public class KnightFigureShuffler : InitialFigureShuffler
	{	
		protected override void ShuffleDown (Action<Square> action, Color color)
		{
			action(Square.B1);
			action(Square.G1);
		}
		
		protected override void ShuffleUp (Action<Square> action, Color color)
		{
			action(Square.B8);
			action(Square.G8);
		}
	}
	
	public class BishopFigureShuffler : InitialFigureShuffler
	{	
		protected override void ShuffleDown (Action<Square> action, Color color)
		{
			action(Square.C1);
			action(Square.F1);
		}
		
		protected override void ShuffleUp (Action<Square> action, Color color)
		{
			action(Square.C8);
			action(Square.F8);
		}
	}
	
	public class RookFigureShuffler : InitialFigureShuffler
	{	
		protected override void ShuffleDown (Action<Square> action, Color color)
		{
			action(Square.A1);
			action(Square.H1);
		}
		
		protected override void ShuffleUp (Action<Square> action, Color color)
		{
			action(Square.A8);
			action(Square.H8);
		}
	}
	
	public class QueenFigureShuffler : InitialFigureShuffler
	{	
		protected override void ShuffleDown (Action<Square> action, Color color)
		{
			if (color == Color.White)
				action(Square.D1);
			else
				action(Square.E1);
		}
		
		protected override void ShuffleUp (Action<Square> action, Color color)
		{
			if (color == Color.White)
				action(Square.E8);
			else
				action(Square.D8);
		}
	}
	
	public class KingFigureShuffler : InitialFigureShuffler
	{	
		protected override void ShuffleDown (Action<Square> action, Color color)
		{
			if (color == Color.White)
				action(Square.E1);
			else
				action(Square.D1);
		}
		
		protected override void ShuffleUp (Action<Square> action, Color color)
		{
			if (color == Color.White)
				action(Square.D8);
			else
				action(Square.E8);
		}
	}
	
	public static class FigureShufflerFactory
	{
		public static InitialFigureShuffler CreateShuffler(Figure figureType)
		{
			switch (figureType)
			{
			case Figure.Pawn:
				return new PawnFigureShuffler();
			case Figure.Knight:
				return new KnightFigureShuffler();
			case Figure.Bishop:
				return new BishopFigureShuffler();
			case Figure.Rook:
				return new RookFigureShuffler();
			case Figure.Queen:
				return new QueenFigureShuffler();
			case Figure.King:
				return new KingFigureShuffler();
			default:
				return null;
			}
		}
	}
}

