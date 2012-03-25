using System;

namespace Queem.Core.BitBoards.Helpers
{
	public static class BishopBitBoardHelper
	{
		/*
		 * 0 0 0 1 ...
		 * 0 0 1 0 ...
		 * 0 1 0 0 ...
		 * 1 0 0 0 ...
		 *   . . .
		*/
		// 7 + main + 7
		public static readonly ulong[] DiagonalsMasks;
	
		/*
		 * 1 0 0 0 ...
		 * 0 1 0 0 ...
		 * 0 0 1 0 ...
		 * 0 0 0 1 ...
		 *   . . .
		*/
		public static readonly ulong[] AntiDiagonalsMasks;
	
		static BishopBitBoardHelper()
		{
			DiagonalsMasks = GenerateDiagonals();
			AntiDiagonalsMasks = GenerateAntiDiagonals();
		}
		
		public static ulong[] GenerateDiagonals()
		{
			ulong[] arr = new ulong[15];
		
			for (int i = 0; i < 8; ++i)
			{
				ulong board = 0;
				ulong minusBoard = 0;
			
				for (int x = 0; x <= i; ++x)
				{
					int y = x + (7 - i);
					
					board |= BitBoardHelper.GetOneBitNumber(y, x);
					minusBoard |= BitBoardHelper.GetOneBitNumber(7 - y, 7 - x);
				}					
				
				arr[14 - i] = minusBoard;
				arr[i] = board;
			}
			
			return arr;
		}
		
		public static ulong[] GenerateAntiDiagonals()
		{
			ulong[] arr = new ulong[15];
		
			for (int i = 0; i < 8; ++i)
			{
				ulong board = 0;
				ulong minusBoard = 0;
			
				for (int x = 0; x <= i; ++x)
				{
					int y = i - x;
					
					board |= BitBoardHelper.GetOneBitNumber(x, y);
					minusBoard |= BitBoardHelper.GetOneBitNumber(7 - x, 7 - y);
				}
				
				arr[14 - i] = minusBoard;
				arr[i] = board;
			}
			
			return arr;
		}
		
		public static ulong GetDiagonalMask(Square sq)
		{
			int rank = (int)sq >> 3;
			int file = (int)sq & 7;
			
			return DiagonalsMasks[8 + file - rank];
		}		
	}
}

