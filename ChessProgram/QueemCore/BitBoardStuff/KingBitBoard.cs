using System;

namespace QueemCore
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
			ulong attacks = ShiftEastOne(this.board) | ShiftWestOne(this.board);
			ulong temp = this.board | attacks;
			attacks |= ShiftNorth(temp) | ShiftSouth(temp);
			return attacks;
		}
	}
}

