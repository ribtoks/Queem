using System;
using QueemCore.MovesGenerators;
using QueemCore.BitBoards;
using QueemCore.AttacksGenerators;
using System.Collections.Generic;
using QueemCore.BitBoards.Helpers;

namespace QueemCore
{
	public class PawnMovesGenerator : MovesGenerator
	{
		protected ulong[] pawnMoves;
	
		public PawnMovesGenerator (BitBoard board, AttacksGenerator generator)
			:base(board, generator)
		{
			this.pawnMoves = new ulong[4];
			this.PlayerPos = PlayerPosition.Up;
		}		
																																																																																																																																																																																																																			
		public PlayerPosition PlayerPos { get; set; }
		
		public override List<Move[]> GetMoves (ulong otherFigures, ulong mask)
		{
			var list = new List<Move[]>(8);	
			var pawns = (PawnBitBoard) this.board;
			ulong emptySquares = ~otherFigures;
			int dir = (this.PlayerPos == PlayerPosition.Up) ? 1 : 0;																																				
			
			if (this.PlayerPos == PlayerPosition.Down)
			{
				this.pawnMoves[0] = pawns.SingleUpPushTargets(emptySquares);
				// left
				this.pawnMoves[1] = pawns.NorthWestAttacks();
				// right
				this.pawnMoves[2] = pawns.NorthEastAttacks();
				this.pawnMoves[3] = pawns.DoubleUpPushTargets(emptySquares);
			}
			else
			{
				this.pawnMoves[0] = pawns.SingleDownPushTargets(emptySquares);
				// left
				this.pawnMoves[1] = pawns.SouthWestAttacks();
				// right
				this.pawnMoves[2] = pawns.SouthEastAttacks();
				this.pawnMoves[3] = pawns.DoubleDownPushTargets(emptySquares);
			}
			
			this.AddMoves(this.pawnMoves[0], list, PawnBitBoardHelper.QuietMoves[dir]);
			this.AddMoves(this.pawnMoves[1], list, PawnBitBoardHelper.AttacksLeftMoves[dir]);
			this.AddMoves(this.pawnMoves[2], list, PawnBitBoardHelper.AttacksRightMoves[dir]);
			this.AddMoves(this.pawnMoves[3], list, PawnBitBoardHelper.DoublePushes[dir]);
			
			return list;
		}
		
		private void AddMoves(ulong board, List<Move[]> list, Move[][][] hardcodedMoves)
		{
			int rankIndex = 0, rank;
			while (board != 0)
			{
				rank = (int)(board & 0xff);
				
				if (rank != 0)
					list.Add(hardcodedMoves[rankIndex][rank]);
			
				rankIndex++;
				board >>= 8;
			}
		}
	}
}

