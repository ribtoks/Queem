using System;
using QueemCore.BitBoard.Helpers;
using System.Collections.Generic;

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
		
		public override IEnumerable<ulong> GetAttacks()
		{
			ulong attacks = BitBoardHelper.ShiftEastOne(this.board) | BitBoardHelper.ShiftWestOne(this.board);
			ulong temp = this.board | attacks;
			attacks |= BitBoardHelper.ShiftNorthOne(temp) | BitBoardHelper.ShiftSouthOne(temp);
			yield return attacks;
		}
	}
}
