using System;
using QueemCore.BitBoards.Helpers;
using System.Collections.Generic;
using QueemCore.AttacksGenerators;

namespace QueemCore.BitBoards
{
	public class KingBitBoard : BitBoard
	{
		protected Square sq;
	
		public KingBitBoard()
			:base()
		{			
		}
	
		public KingBitBoard (AttacksGenerator generator)
			:base(generator)
		{
		}
		
		public KingBitBoard(ulong val, AttacksGenerator generator)
			:base(val, generator)
		{
		}
		
		public override void DoMove (Move move)
		{
			this.sq = move.To;
			base.DoMove (move);
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
		
		public override IEnumerable<ulong> GetAttacks (ulong otherFigures)
		{
			if (this.sq != Square.NoSquare)
				yield return this.attacksGenerator.GetAttacks(this.sq, otherFigures);
		}
	}
}
