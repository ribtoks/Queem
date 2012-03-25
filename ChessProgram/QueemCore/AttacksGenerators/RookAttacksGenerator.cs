using System;
using System.Collections.Generic;
using Queem.Core.BitBoards.Helpers;
using Queem.Core;

namespace Queem.Core.AttacksGenerators
{
	public class RookAttacksGenerator : AttacksGenerator
	{
		public override ulong GetAttacks (Square figureSquare, ulong otherFigures)
		{
			int file = (int)figureSquare & 7;
			int rank = (int)figureSquare >> 3;
		
			ulong otherFiguresFile = otherFigures >> file;
			ulong clearAFile = otherFiguresFile & (~BitBoardHelper.NotAFile);
			byte rotatedFile = BitBoardHelper.GetRankFromAFile(clearAFile);
			byte fileAttacks = RookBitBoardHelper.FirstRankAttacks[rank, rotatedFile];
			ulong verticalAttacks = BitBoardHelper.GetFileFromRank(fileAttacks) << file;
			
			ulong otherFiguresRank = otherFigures >> (8*rank);
			otherFiguresRank &= 255;
			byte rankAttacks = RookBitBoardHelper.FirstRankAttacks[7 - file, otherFiguresRank];
			// in real int64 lower byte has mirrored bits
			// 7 6 5 4 3 2 1 0 and in this 0 1 2 3 4 5 6 7
			ulong horizontalAttacks = (ulong)(rankAttacks) << (8*rank);
			
			return (verticalAttacks | horizontalAttacks);
		}
	}
}

