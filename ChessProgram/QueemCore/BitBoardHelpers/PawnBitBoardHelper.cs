using System;
using System.Collections.Generic;

namespace QueemCore.BitBoards.Helpers
{
	public static class PawnBitBoardHelper
	{
		/*
		 * first own list 
		 * second direction
		 * third rank
		 * forth byte
		 * 
		 * each array of array of... represents
		 * list of moves of how we got there
		*/		
		public static readonly Move[][][][] QuietMoves;
		public static readonly Move[][][][] AttacksLeftMoves;
		public static readonly Move[][][][] AttacksRightMoves;
		public static readonly Move[][][][] DoublePushes;
		
		static PawnBitBoardHelper()
		{
			QuietMoves = GenerateQuietMoves();
			AttacksLeftMoves = GenerateLeftMoves();
			AttacksRightMoves = GenerateRightMoves();
			DoublePushes = GenerateDoublePushes();
		}
		
		/*
		 * +7 +8 +9
		 * -1  0 +1
		 * -9 -8 -7
		*/
		private static Move[] GetPawnMoves(int rank, byte b, int delta)
		{
			var squares = BitBoardSerializer.Squares[rank][b];
			var moves = new List<Move>();
			
			foreach (var sq in squares)
			{
				Square start = sq;
				start = (Square)((int)start + delta);
				moves.Add(new Move(start, sq));
			}
			
			return moves.ToArray();
		}
		
		#region Quiet Moves
		
		private static Move[][][][] GenerateQuietMoves()
		{
			Move[][][][] moves = new Move[2][][][];
			moves[0] = new Move[8][][];
			moves[1] = new Move[8][][];
			
			for (int rank = 0; rank < 8; ++rank)
			{
				moves[0][rank] = new Move[256][];
				moves[1][rank] = new Move[256][];
				
				for (int b = 0; b < 256; ++b)
				{
					moves[0][rank][b] = GetQuietUpMoves(rank, (byte)b);
					moves[1][rank][b] = GetQuietDownMoves(rank, (byte)b);
				}
			}
			
			return moves;
		}
		
		private static Move[] GetQuietUpMoves(int rank, byte b)
		{
			if (rank == 0)
				return new Move[0];
				
			return GetPawnMoves(rank, b, -8);
		}
		
		private static Move[] GetQuietDownMoves(int rank, byte b)
		{
			if (rank == 7)
				return new Move[0];
				
			return GetPawnMoves(rank, b, 8);
		}
				
		#endregion
		
		#region Left Moves
		
		private static Move[][][][] GenerateLeftMoves()
		{
			Move[][][][] moves = new Move[2][][][];
			moves[0] = new Move[8][][];
			moves[1] = new Move[8][][];
			
			for (int rank = 0; rank < 8; ++rank)
			{
				moves[0][rank] = new Move[256][];
				moves[1][rank] = new Move[256][];
				
				// 127 - don't use H file (8th bit 01111111 reversed)
				for (int b = 0; b < 128; ++b)
				{
					moves[0][rank][b] = GetLeftUpMoves(rank, (byte)b);
					moves[1][rank][b] = GetLeftDownMoves(rank, (byte)b);
				}
				
				for (int b = 128; b < 256; ++b)
				{
					moves[0][rank][b] = new Move[0];
					moves[1][rank][b] = new Move[0];
				}
			}
			
			return moves;
		}
		
		private static Move[] GetLeftUpMoves(int rank, byte b)
		{
			if (rank == 0)
				return new Move[0];
				
			return GetPawnMoves(rank, b, -7);
		}
		
		private static Move[] GetLeftDownMoves(int rank, byte b)
		{
			if (rank == 7)
				return new Move[0];
				
			return GetPawnMoves(rank, b, 9);
		}
		
		#endregion		
		
		#region Right moves
		
		private static Move[][][][] GenerateRightMoves()
		{
			Move[][][][] moves = new Move[2][][][];
			moves[0] = new Move[8][][];
			moves[1] = new Move[8][][];
			
			for (int rank = 0; rank < 8; ++rank)
			{
				moves[0][rank] = new Move[256][];
				moves[1][rank] = new Move[256][];
				
				// don't use A file (8th bit 11111110 reversed)
				for (int b = 0; b < 256; ++b)
				{
					if (b % 2 == 0)
					{
						moves[0][rank][b] = GetRightUpMoves(rank, (byte)b);
						moves[1][rank][b] = GetRightDownMoves(rank, (byte)b);
					}
					else
					{
						moves[0][rank][b] = new Move[0];
						moves[1][rank][b] = new Move[0];
					}
				}
			}
			
			return moves;
		}
		
		private static Move[] GetRightUpMoves(int rank, byte b)
		{
			if (rank == 0)
				return new Move[0];
				
			return GetPawnMoves(rank, b, -9);
		}
		
		private static Move[] GetRightDownMoves(int rank, byte b)
		{
			if (rank == 7)
				return new Move[0];
				
			return GetPawnMoves(rank, b, 7);
		}
		
		#endregion
		
		#region Double pushes
		
		private static Move[][][][] GenerateDoublePushes()
		{
			Move[][][][] moves = new Move[2][][][];
			moves[0] = new Move[8][][];
			moves[1] = new Move[8][][];
			
			moves[0][3] = new Move[256][];
			moves[1][4] = new Move[256][];
			for (int b = 0; b < 256; ++b)
			{
				moves[0][3][b] = GetPawnMoves(3, (byte)b, -16);
				moves[1][4][b] = GetPawnMoves(4, (byte)b, 16);
			}
			
			for (int rank = 0; rank < 8; ++rank)
			{
				if ((rank == 3) ||
					(rank == 4))
					continue;
			
				moves[0][rank] = new Move[256][];
				moves[1][rank] = new Move[256][];
				
				for (int b = 0; b < 256; ++b)
				{
					moves[0][rank][b] = new Move[0];
					moves[1][rank][b] = new Move[0];
				}
			}
			
			return moves;
		}
		
		#endregion
	
		public static ulong NorthPawnsFill(ulong gen)
		{
			gen |= (gen <<  8);
		   	gen |= (gen << 16);
		   	gen |= (gen << 32);
		   	return gen;
		}
		
		public static ulong SouthPawnsFill(ulong gen)
		{
			gen |= (gen >>  8);
		   	gen |= (gen >> 16);
		   	gen |= (gen >> 32);
		   	return gen;
		}
		
		public static ulong FilePawnFill(ulong gen)
		{
			return (NorthPawnsFill(gen) | SouthPawnsFill(gen));
		}
	}
}

