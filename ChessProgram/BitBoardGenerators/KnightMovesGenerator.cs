using System;
using System.Linq;
using Queem.Core.BitBoard;
using Queem.Core.BitBoard.Helpers;

namespace MovesGenerators
{
	public class KnightMovesGenerator : Generator
	{
		private ulong GetKnightMoves(int i, int j)
		{
			/*
			 *   noNoWe    noNoEa
		            +15  +17
		             |     |
		noWeWe  +6 __|     |__+10  noEaEa
		              \   /
		               >0<
		           __ /   \ __
		soWeWe -10   |     |   -6  soEaEa
		             |     |
		            -17  -15
		        soSoWe    soSoEa
			 */
			int index = i*8+j;
			ulong initialPos = 1UL << index;
			
			ulong board = 0;
			
			board |= BitBoardHelper.ShiftNorthNorthWest(initialPos);
			board |= BitBoardHelper.ShiftNorthNorthEast(initialPos);
			board |= BitBoardHelper.ShiftNorthWestWest(initialPos);
			board |= BitBoardHelper.ShiftNorthEastEast(initialPos);
			board |= BitBoardHelper.ShiftSouthWestWest(initialPos);
			board |= BitBoardHelper.ShiftSouthEastEast(initialPos);
			board |= BitBoardHelper.ShiftSouthSouthWest(initialPos);
			board |= BitBoardHelper.ShiftSouthSouthEast(initialPos);
			
			return board;
		}
		
		public override void Run ()
		{
			ulong[] knightMoves = new ulong[64];
			
			
			for (int i = 0; i < 8; i++) 
			{
				for (int j = 0; j < 8; ++j)
				{
					knightMoves[i*8 + j] = GetKnightMoves(i, j);
				}
			}
			
			this.data = knightMoves;
		}
		
		public override void WriteResults (System.IO.TextWriter tw)
		{
			tw.WriteLine (string.Join(", ", 
			                               ((ulong[])this.data).Select(u => "0x" + u.ToString("X") + "UL").ToArray()));
		}
	}
}

