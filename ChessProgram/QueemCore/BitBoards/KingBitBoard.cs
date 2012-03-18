using System;
using QueemCore.BitBoards.Helpers;
using System.Collections.Generic;
using QueemCore.AttacksGenerators;

namespace QueemCore.BitBoards
{
	public class KingBitBoard : BitBoard
	{
		protected Square sq;
		
		public int AlreadyMoved { get; set; }
	
		public KingBitBoard()
			:base()
		{			
		}
			
		public KingBitBoard(ulong val)
			:base(val)
		{
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
		
		public override void UndoMove (Move move)
		{
			this.sq = move.From;
			base.UndoMove (move);
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
