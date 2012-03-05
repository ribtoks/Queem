using System;
using System.Linq;
using System.Diagnostics;
using QueemCore.BitBoard;

namespace QueemCore.BitBoard.Helpers
{
	public static class RookBitBoardHelper
	{
		public static readonly byte[,] FirstRankAttacks;
		public static readonly ulong[] RotatedBytes;
		
		static RookBitBoardHelper()
		{
			FirstRankAttacks = GenerateFirstRankAttacks();
			RotatedBytes = GenerateRotatedRanks();
		}
		
		public static byte[,] GenerateFirstRankAttacks()
		{
			byte[,] situations = new byte[8, 256];
			
			for (int i = 0; i < 8; ++i)
			{
				int rookPos = 0x1 << i;
				int otherFiguresMask = ~rookPos;
				
				byte[] arr = Enumerable.
					Range(0, 256).
					// remove bit with current rook position
					Select(n => (byte)(n & otherFiguresMask)).
					Distinct().	
					ToArray();
				
				Debug.Assert(arr.Length == 128, "Wrong count");
								
				Array.ForEach(arr, 
				              (otherFigures) => 
				              situations[7 - i, otherFigures] = 
				              	GetByteRankMovesMask((byte)rookPos, otherFigures));
			}
			return situations;
		}
				
		public static byte GetByteRankMovesMask(byte rookPosition, byte otherFigures)			
		{
			/*
			 * Test input 
			 *  rook position - 00001000
			 *  other figures - 01000010
			 * Output           01110110
			*/
			
			byte moves = 0;
			
			byte left = (byte)(rookPosition << 1);
			while (left != 0)
			{
				moves |= left;
				// set bit means other figure there
				if ((left & otherFigures) == left)
					break;
				left = (byte)(left << 1);
			}
			
			byte right = (byte)(rookPosition >> 1);
			while (right != 0)
			{
				moves |= right;
				// set bit means other figure there
				if ((right & otherFigures) == right)
					break;
				right = (byte)(right >> 1);
			}
			
			return moves;
		}
		
		public static ulong[] GenerateRotatedRanks()
		{
			return Enumerable.Range(0, 256) 
						.Select(b => BitBoardHelper.FlipDiagonalA1H8((ulong)b))
						.ToArray();
		}		
		
		public static ulong GetRookAttacks(ulong otherFigures, Square sq)
		{
			int file = (int)sq & 7;
			int rank = (int)sq >> 3;
		
			ulong otherFiguresFile = otherFigures >> file;
			ulong clearAFile = otherFiguresFile & (~BitBoardHelper.NotAFile);
			byte rotatedFile = BitBoardHelper.GetRankFromAFile(clearAFile);
			byte fileAttacks = FirstRankAttacks[rank, rotatedFile];
			ulong verticalAttacks = BitBoardHelper.GetFileFromRank(fileAttacks) << file;
			
			ulong otherFiguresRank = otherFigures >> (8*rank);
			otherFiguresRank &= 255;
			byte rankAttacks = FirstRankAttacks[7 - file, otherFiguresRank];
			// in real int64 lower byte has mirrored bits
			// 7 6 5 4 3 2 1 0 and in this 0 1 2 3 4 5 6 7
			ulong horizontalAttacks = (ulong)(rankAttacks) << (8*rank);
			
			return verticalAttacks | horizontalAttacks;
		}
	}
}

