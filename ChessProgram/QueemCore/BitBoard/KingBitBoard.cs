using System;
using QueemCore.BitBoard.Helpers;

namespace QueemCore.BitBoard
{
	public class KingBitBoard : BitBoard
	{
		public KingBitBoard ()
			:base()
		{
		}
		
		public KingBitBoard(ulong val)
			:base(val)
		{
		}
		
		public ulong GetAttacks()
		{
			ulong attacks = BitBoardHelper.ShiftEastOne(this.board) | BitBoardHelper.ShiftWestOne(this.board);
			ulong temp = this.board | attacks;
			attacks |= BitBoardHelper.ShiftNorthOne(temp) | BitBoardHelper.ShiftSouthOne(temp);
			return attacks;
		}
	}
}
