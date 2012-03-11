using System;
using QueemCore.BitBoard;

namespace QueemCore.ChessBoard
{	
	public abstract class InitialFigureShuffler
	{
		public void Init(BitBoard board, PlayerPosition pos, Color playerColor)
		{
			if (pos == PlayerPosition.Up)
				this.ShuffleUp(board);
			else
				this.ShuffleDown(board);
		}
		
		protected abstract void ShuffleUp(BitBoard board, Color color);
		protected abstract void ShuffleDown(BitBoard board, Color color);
	}
	
	public class PawnFigureShuffler : InitialFigureShuffler
	{
		protected override void ShuffleUp (BitBoard board, Color color)
		{
			board.SetBit(Square.A7);
			board.SetBit(Square.B7);
			board.SetBit(Square.C7);
			board.SetBit(Square.D7);
			board.SetBit(Square.E7);
			board.SetBit(Square.F7);
			board.SetBit(Square.G7);
			board.SetBit(Square.H7);
		}
		
		protected override void ShuffleDown (BitBoard board, Color color)
		{
			board.SetBit(Square.A2);
			board.SetBit(Square.B2);
			board.SetBit(Square.C2);
			board.SetBit(Square.D2);
			board.SetBit(Square.E2);
			board.SetBit(Square.F2);
			board.SetBit(Square.G2);
			board.SetBit(Square.H2);
		}
	}
	
	public class KnightFigureShuffler : InitialFigureShuffler
	{
		protected override void ShuffleDown (BitBoard board, Color color)
		{
			board.SetBit(Square.B1);
			board.SetBit(Square.G1);
		}
		
		protected override void ShuffleUp (BitBoard board, Color color)
		{
			board.SetBit(Square.B8);
			board.SetBit(Square.G8);
		}
	}
	
	public class BishopFigureShuffler : InitialFigureShuffler
	{
		protected override void ShuffleDown (BitBoard board, Color color)
		{
			board.SetBit(Square.C1);
			board.SetBit(Square.F1);
		}
		
		protected override void ShuffleUp (BitBoard board, Color color)
		{
			board.SetBit(Square.C8);
			board.SetBit(Square.F8);
		}
	}
	
	public class RookFigureShuffler : InitialFigureShuffler
	{
		protected override void ShuffleDown (BitBoard board, Color color)
		{
			board.SetBit(Square.A1);
			board.SetBit(Square.H1);
		}
		
		protected override void ShuffleUp (BitBoard board, Color color)
		{
			board.SetBit(Square.A8);
			board.SetBit(Square.H8);
		}
	}
	
	public class QueenFigureShuffler : InitialFigureShuffler
	{
		protected override void ShuffleDown (BitBoard board, Color color)
		{
			if (color == Color.White)
				board.SetBit(Square.D1);
			else
				board.SetBit(Square.E1);
		}
		
		protected override void ShuffleUp (BitBoard board, Color color)
		{
			if (color == Color.White)
				board.SetBit(Square.E1);
			else
				board.SetBit(Square.D1);
		}
	}
	
	public class KingFigureShuffler : InitialFigureShuffler
	{
		protected override void ShuffleDown (BitBoard board, Color color)
		{
			if (color == Color.White)
				board.SetBit(Square.E1);
			else
				board.SetBit(Square.D1);
		}
		
		protected override void ShuffleUp (BitBoard board, Color color)
		{
			if (color == Color.White)
				board.SetBit(Square.D1);
			else
				board.SetBit(Square.E1);
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

