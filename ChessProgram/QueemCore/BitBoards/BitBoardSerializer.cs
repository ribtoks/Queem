using System;
using System.Collections.Generic;
using Queem.Core.BitBoards.Helpers;

namespace Queem.Core
{
	public static class BitBoardSerializer
	{
		public static readonly Square[][][] Squares;
		//  first dimension - move-from 
		// second dimension - rank
		//  third dimension - rank byte
		public static readonly Move[][][][] Moves;
		
		static BitBoardSerializer()
		{
			Squares = GenerateSquares();
			Moves = GenerateMoves();
		}
		
		private static Square[][][] GenerateSquares()
		{
			Square[][][] squares = new Square[8][][];
			for (int rank = 0; rank < 8; ++rank)
			{
				squares[rank] = new Square[256][];
				
				for (int b = 0; b < 256; ++b)
				{
					squares[rank][b] = GetSquaresArray(rank, (byte)b);
				}
			}
			
			return squares;
		}
				
		private static Square[] GetSquaresArray(int rank, byte b)
		{
			var squares = new List<Square>();
			
			for (int i = 0; i < 8; ++i)
			{
				// 00010000
				int bit = 1 << i;
				// 11101111
				bit = ~bit;
				
				// if bit is set
				if ((b & bit) != b)
					squares.Add(BitBoardHelper.GetSquare(rank, i));
			}
			
			return squares.ToArray();
		}
		
		public static IEnumerable<Square> GetSquares(ulong board)
		{
			int rankIndex = 0;
			while (board != 0)
			{
				int rank = (int)(board & 0xff);
				
				if (rank != 0)				
					foreach (var sq in Squares[rankIndex][rank])
						yield return sq;
				
				board >>= 8;
				rankIndex++;
			}
		}
		
		public static IEnumerable<T> MapSquares<T>(ulong board, Func<Square, T> func)
		{
			int rankIndex = 0;
			while (board != 0)
			{
				int rank = (int)(board & 0xff);
				
				if (rank != 0)				
					foreach (var sq in Squares[rankIndex][rank])
						yield return func(sq);
				
				board >>= 8;
				rankIndex++;
			}
		}
		
		public static IEnumerable<Move> GetMoves(Square start, ulong board)
		{
			int rankIndex = 0;
			while (board != 0)
			{
				int rank = (int)(board & 0xff);
				
				if (rank != 0)
					foreach (var move in Moves[(int)start][rankIndex][rank])
						yield return move;
				
				board >>= 8;
				rankIndex++;
			}
		}
		
		private static Move[][][][] GenerateMoves()
		{
			Move[][][][] moves = new Move[64][][][];
			for (int sq = 0; sq < 64; ++sq)
			{
				moves[sq] = new Move[8][][];
				
				for (int rank = 0; rank < 8; ++rank)
				{
					moves[sq][rank] = new Move[256][];
					
					for (int b = 0; b < 256; ++b)
					{
						moves[sq][rank][b] = GetMovesArray((Square)sq, rank, (byte)b);
					}
				}
			}
			
			return moves;
		}
		
		private static Move[] GetMovesArray(Square start, int rank, byte b)
		{
			var moves = new List<Move>();
			
			foreach (var sq in Squares[rank][b])
				moves.Add(new Move(start, sq));
			
			return moves.ToArray();
		}
	}
}

