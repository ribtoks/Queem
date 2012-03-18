using System;
using System.Collections.Generic;
using QueemCore.BitBoards;
using QueemCore.AttacksGenerators;
using QueemCore.BitBoards.Helpers;

namespace QueemCore.MovesGenerators
{
	public class KingMovesGenerator : MovesGenerator
	{
		protected KingBitBoard kingBoard;
		
		public KingMovesGenerator (BitBoard bitboard, AttacksGenerator attacksgenerator)
			:base(bitboard, attacksgenerator)
		{
			this.kingBoard = (KingBitBoard)bitboard;
		}
		
		public override List<Move[]> GetMoves (ulong otherFigures, ulong mask)
		{
			var list = new List<Move[]>(4);
			Square figureSquare = this.kingBoard.GetSquare();
			ulong attacks = generator.GetAttacks(figureSquare, otherFigures);
			
			attacks &= mask;
						
			int rankIndex = 0;
			while (attacks != 0)
			{
				int rank = (int)(attacks & 0xff);
							
				if (rank != 0)
					list.Add(BitBoardSerializer.Moves[(int)figureSquare][rankIndex][rank]);
				
				rankIndex++;
				attacks >>= 8;
			}
									
			return list;
		}
	}
}

