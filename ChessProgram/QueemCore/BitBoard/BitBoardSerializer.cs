using System;
using System.Collections.Generic;
using QueemCore.BitBoard.Helpers;

namespace QueemCore
{
	public static class BitBoardSerializer
	{
		public static readonly List<Square>[][] Squares;
		
		static BitBoardSerializer()
		{
			Squares = GenerateSquares();
		}
		
		public static List<Square>[][] GenerateSquares()
		{
			List<Square>[][] squares = new List<Square>[8][];
			for (int rank = 0; rank < 8; ++rank)
			{
				squares[rank] = new List<Square>[256];
				
				for (byte b = 0; b < 256; ++b)
				{
					squares[rank][b] = GetList(rank, b);
				}
			}
			
			return squares;
		}
		
		private static List<Square> GetList(int rank, byte b)
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
			
			return squares;
		}		
		
		public static List<Square> Serialize(ulong board, int rank)
		{
			int rankByte = (int)((board >> (rank*8)) & 0xff);
			return Squares[rank][rankByte];
		}
	}
}

