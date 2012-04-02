using System;
using Queem.Core.BitBoards.Helpers;
using System.Collections.Generic;
using Queem.Core.AttacksGenerators;

namespace Queem.Core.BitBoards
{
	public class KingBitBoard : BitBoard
	{
		protected Square sq;
		
		public int AlreadyMoved { get; set; }
	
		public KingBitBoard()
			:base()
		{
            this.AlreadyMoved = 0;
		}
			
		public KingBitBoard(ulong val)
			:base(val)
		{
            this.AlreadyMoved = 0;
		}
		
		public override int GetInnerProperty ()
		{
			return this.AlreadyMoved;
		}
		
		public override void SetInnerProperty (int property)
		{
			 this.AlreadyMoved = property;
		}
		
		public override void DoMove (Move move)
		{
			this.sq = move.To;
			base.DoMove (move);
			AlreadyMoved = 1;
		}
		
		public override void UndoMove (int sqFrom, int sqTo)
		{
			this.sq = (Square)sqFrom;
			base.UndoMove (sqFrom, sqTo);
		}
		
		public override BitBoard SetBit (Square square)
		{
			this.sq = square;
			return base.SetBit (square);
		}
		
		public override BitBoard UnsetBit (Square square)
		{
			this.sq = Square.NoSquare;
			return base.UnsetBit (square);
		}
		
		public Square GetSquare()
		{
			return this.sq;
		}		
	}
}
