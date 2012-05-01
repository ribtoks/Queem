using System;
using Queem.Core.BitBoards.Helpers;
using System.Collections.Generic;
using Queem.Core.AttacksGenerators;

namespace Queem.Core.BitBoards
{
	public class RookBitBoard : BitBoard
	{
        protected const ulong rank1 = 0x00000000000000FFUL;
        protected const ulong rank8 = 0xFF00000000000000UL;
        protected ulong rankMask;

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

        public void SetupOrientation(PlayerPosition pp)
        {
            if (pp == PlayerPosition.Up)
                this.rankMask = rank8;
            else
                if (pp == PlayerPosition.Down)
                    this.rankMask = rank1;
        }
		
		public int LeftNotMoved { get; set; }
		public int RightNotMoved { get; set; }
		
		public override void DoMove (Move move)
		{
			var leftMask = RookBitBoardHelper.LeftRookMask & this.board & this.rankMask;
			var rightMask = RookBitBoardHelper.RightRookMask & this.board & this.rankMask;
			
			base.DoMove (move);
			
			if (this.LeftNotMoved != 0)
			{
				var newLeftMask = RookBitBoardHelper.LeftRookMask & this.board & this.rankMask;
				this.LeftNotMoved = (newLeftMask == leftMask) ? 1 : 0;
			}
			
			if (this.RightNotMoved != 0)
			{
				var newRightMask = RookBitBoardHelper.RightRookMask & this.board & this.rankMask;
				this.RightNotMoved = (newRightMask == rightMask) ? 1 : 0;
			}
		}
        
        public override BitBoard UnsetBit(Square sq)
        {
            var leftMask = RookBitBoardHelper.LeftRookMask & this.board & this.rankMask;
            var rightMask = RookBitBoardHelper.RightRookMask & this.board & this.rankMask;

            base.UnsetBit(sq);

            if (this.LeftNotMoved != 0)
            {
                var newLeftMask = RookBitBoardHelper.LeftRookMask & this.board & this.rankMask;
                this.LeftNotMoved = (newLeftMask == leftMask) ? 1 : 0;
            }

            if (this.RightNotMoved != 0)
            {
                var newRightMask = RookBitBoardHelper.RightRookMask & this.board & this.rankMask;
                this.RightNotMoved = (newRightMask == rightMask) ? 1 : 0;
            }

            return this;
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

