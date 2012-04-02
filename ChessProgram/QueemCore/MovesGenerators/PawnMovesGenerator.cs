using System;
using Queem.Core.MovesGenerators;
using Queem.Core.BitBoards;
using Queem.Core.AttacksGenerators;
using System.Collections.Generic;
using Queem.Core.BitBoards.Helpers;

namespace Queem.Core
{
	public class PawnMovesGenerator : MovesGenerator
	{
		protected Move[][][][][] movesReferences;
	
		public PawnMovesGenerator (BitBoard board, AttacksGenerator generator)
			:base(board, generator)
		{
			this.movesReferences = new Move[4][][][][];
			this.movesReferences[(int)PawnTarget.SinglePush] = PawnBitBoardHelper.QuietMoves;
			this.movesReferences[(int)PawnTarget.LeftAttack] = PawnBitBoardHelper.AttacksLeftMoves;
			this.movesReferences[(int)PawnTarget.RightAttack] = PawnBitBoardHelper.AttacksRightMoves;
			this.movesReferences[(int)PawnTarget.DoublePush] = PawnBitBoardHelper.DoublePushes;
		}
		
        // mask - opponent figures | 1 bit of passing move
		public override List<Move[]> GetMoves (ulong otherFigures, ulong mask)
		{
			var list = new List<Move[]>(8);				
			var pawnGenerator = (PawnAttacksGenerator) this.generator;
			int dir = pawnGenerator.Index;
			
			ulong[] attacks = pawnGenerator.GetAttacks(this.board.GetInnerValue(), otherFigures);
            ulong notMineFigures = ~(otherFigures & (~mask));
            
            attacks[(int)PawnTarget.LeftAttack] &= mask & notMineFigures;
            attacks[(int)PawnTarget.RightAttack] &= mask & notMineFigures;
									
			int rankIndex = 0, rank;
			for (int i = 0; i < 4; ++i)
			{
				var board = attacks[i];
                rankIndex = 0;
				while (board != 0)
				{
					rank = (int)(board & 0xff);
					
					if (rank != 0)
						list.Add(this.movesReferences[i][dir][rankIndex][rank]);
				
					rankIndex++;
					board >>= 8;
				}
			}
			
			return list;
		}		
	}
}

