using System;
using System.Linq;
using QueemCore.BitBoard;
using QueemCore.BitBoard.Helpers;

namespace MovesGenerators
{
	public class KingMovesGenerator : Generator
	{
		private ulong GetKingMoves(int i, int j)
		{
			int index = i*8+j;
			ulong initialPos = 1UL << index;
			
			ulong board = initialPos;
			
			ulong attacks = BitBoardHelper.ShiftEastOne(board) | BitBoardHelper.ShiftWestOne(board);
			board = initialPos | attacks;
			attacks |= BitBoardHelper.ShiftNorthOne(board) | BitBoardHelper.ShiftSouthOne(board);
			return attacks;
		}
		
		public override void Run ()
		{
			ulong[] kingMoves = new ulong[64];
			
			for (int i = 0; i < 8; i++) 
			{
				for (int j = 0; j < 8; ++j)
				{
					kingMoves[i*8 + j] = GetKingMoves(i, j);
				}
			}
			
			this.data = kingMoves;
		}
		
		public override void WriteResults (System.IO.TextWriter tw)
		{
			tw.WriteLine (string.Join(", ", 
			                               ((ulong[])this.data).Select(u => "0x" + u.ToString("X") + "UL").ToArray()));
		}
	}
}

