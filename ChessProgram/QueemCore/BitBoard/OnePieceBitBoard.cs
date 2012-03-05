using System;
using System.Collections.Generic;
using QueemCore.MovesProviders;

namespace QueemCore.BitBoard
{
	public class OnePieceBitBoard : BitBoard
	{
		protected Square sq;
		
		public OnePieceBitBoard (MovesProvider provider) 
			: base(provider)
		{
		}
		
		public OnePieceBitBoard (ulong val, MovesProvider provider)
			: base(val, provider)
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
				yield return this.movesProvider.GetAttacks(this.sq, otherFigures);
		}
	}
}

