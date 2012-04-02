using System;
using Queem.Core.BitBoards;
using Queem.Core.AttacksGenerators;
using System.Collections.Generic;
using System.Linq;

namespace Queem.Core.MovesGenerators
{
	public class MovesGenerator
	{
		protected BitBoard board;
		protected AttacksGenerator generator;
	
		public MovesGenerator (BitBoard bitboard, AttacksGenerator attacksgenerator)
		{
			this.board = bitboard;
			this.generator = attacksgenerator;
		}
		
		public virtual List<Move[]> GetMoves(ulong otherFigures, ulong mask)
		{
			var list = new List<Move[]>(8);
			
			ulong attacks;
			
			ulong myboard = board.GetInnerValue();
			
			int myrankIndex = 0;
			while (myboard != 0)
			{
				int myrank = (int)(myboard & 0xff);
				
				if (myrank != 0)
				{
					var myfigures = BitBoardSerializer.Squares[myrankIndex][myrank];
					for (int i = 0; i < myfigures.Length; ++i)
					{
						var figureSquare = myfigures[i];
                        var notFigure = ~(1UL << (int)figureSquare);
						attacks = generator.GetAttacks(figureSquare, otherFigures & notFigure);
						
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
					}
				}
				
				myrankIndex++;
				myboard >>= 8;
			}
			
			return list;
		}
	}
}

