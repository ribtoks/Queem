using System;
using Queem.Core.BitBoards.Helpers;
using System.Collections.Generic;
using Queem.Core.AttacksGenerators;

namespace Queem.Core.BitBoards
{
	public class RookBitBoard : BitBoard
	{
		public RookBitBoard()
			:base()
		{
            this.LeftNotMoved = this.RightNotMoved = 1;
		}
				
		public RookBitBoard(ulong val)
			:base(val)
		{
            this.LeftNotMoved = this.RightNotMoved = 1;
		}
		
		public int LeftNotMoved { get; set; }
		public int RightNotMoved { get; set; }
		
		public override void DoMove (Move move)
		{
			var leftMask = RookBitBoardHelper.LeftRookMask & this.board;
			var rightMask = RookBitBoardHelper.RightRookMask & this.board;
			
			base.DoMove (move);
			
			if (this.LeftNotMoved != 0)
			{
				var newLeftMask = RookBitBoardHelper.LeftRookMask & this.board;
				this.LeftNotMoved = (newLeftMask == leftMask) ? 1 : 0;
			}
			
			if (this.RightNotMoved != 0)
			{
				var newRightMask = RookBitBoardHelper.RightRookMask & this.board;
				this.RightNotMoved = (newRightMask == rightMask) ? 1 : 0;
			}
		}
		
		public override int GetInnerProperty ()
		{
			return (LeftNotMoved << 8) | (RightNotMoved);
		}
		
		public override void SetInnerProperty (int property)
		{
			RightNotMoved = property & 0xff;
			property >>= 8;
			LeftNotMoved = property & 0xff;
		}
	}
}

