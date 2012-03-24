using System;
using QueemCore.BitBoards.Helpers;

namespace QueemCore.AttacksGenerators
{
	public enum PawnTarget { LeftAttack, RightAttack, SinglePush, DoublePush }

	public class PawnAttacksGenerator : AttacksGenerator
	{
		protected int direction;
		protected int index;
		protected ulong[] cases;
		protected const ulong rank5 = 0x000000FF00000000UL;
		protected const ulong rank4 = 0x00000000FF000000UL;
		protected ulong[] attacks;
		
		public PawnAttacksGenerator (PlayerPosition position)
		{
			if (position == PlayerPosition.Down)
			{
				this.direction = 1;
				this.index = 0;
			}
			else
			{
				this.direction = -1;
				this.index = 1;
			}
			
			this.cases = new ulong[2];
		}
		
		public int Index
		{
			get { return this.index; }
		}
		
		public override ulong GetAttacks (Square figures, ulong otherFigures)
		{
			throw new NotImplementedException ();
		}
		
		public ulong[] GetAttacks (ulong figures, ulong otherFigures)
		{
			ulong emptySquares = ~otherFigures;

			// left attacks
			this.cases[0] = (figures & BitBoardHelper.NotAFile) << 7;
			this.cases[1] = (figures & BitBoardHelper.NotAFile) >> 9;
			this.attacks[(int)PawnTarget.LeftAttack] = this.cases[this.index];
			
			// right attacks
			this.cases[0] = (figures & BitBoardHelper.NotHFile) << 9;
			this.cases[1] = (figures & BitBoardHelper.NotHFile) >> 7;
			this.attacks[(int)PawnTarget.RightAttack] = this.cases[this.index];
			
			// ordinal move
			this.cases[0] = figures << 8;
			this.cases[1] = figures >> 8;
			this.attacks[(int)PawnTarget.SinglePush] = this.cases[this.index] & emptySquares;
			
			// double push
			this.cases[0] = (this.cases[0] << 8) & emptySquares & rank4;
			this.cases[1] = (this.cases[1] >> 8) & emptySquares & rank5;
			this.attacks[(int)PawnTarget.DoublePush] = this.cases[this.index];
			
			return this.attacks;
		}
	}
}

