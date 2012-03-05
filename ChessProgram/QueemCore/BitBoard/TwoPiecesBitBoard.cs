using System;
using System.Collections.Generic;
using QueemCore.MovesProviders;

namespace QueemCore.BitBoard
{
	public abstract class TwoPiecesBitBoard : BitBoard
	{
		protected Square sq1;
		protected Square sq2;
	
		public TwoPiecesBitBoard (MovesProvider provider) 
			: base(provider)
		{
		}
		
		public TwoPiecesBitBoard(ulong val, MovesProvider provider)
			: base(val, provider)
		{
		}
		
		public override void DoMove (Move move)
		{			
			base.DoMove (move);
			this.MoveInnerSquares(move);
		}
		
		protected void MoveInnerSquares(Move move)
		{
			if (move.From == this.sq1)
				this.sq1 = move.To;
			else
				if (move.From == this.sq2)
					this.sq2 = move.To;
		}
		
		public override BitBoard UnsetBit (Square sq)
		{
			this.UnsetInnerSquare(sq);
			return base.UnsetBit (sq);
		}
		
		protected void UnsetInnerSquare(Square sq)
		{
			if (sq == this.sq1)
				this.sq1 = Square.NoSquare;
			else
				if (sq == this.sq2)
					this.sq2 = Square.NoSquare;
		}
		
		protected void SetInnerSquare(Square sq)
		{
			if (this.sq1 == Square.NoSquare)
				this.sq1 = sq;
			else
				if (this.sq2 == Square.NoSquare)
					this.sq2 = sq;
		}
		
		public override IEnumerable<ulong> GetAttacks (ulong otherFigures)
		{
			if (this.sq1 != Square.NoSquare)
				yield return this.movesProvider.GetAttacks(this.sq1, otherFigures);
				
			if (this.sq2 != Square.NoSquare)
				yield return this.movesProvider.GetAttacks(this.sq2, otherFigures);
		}
	}
}

