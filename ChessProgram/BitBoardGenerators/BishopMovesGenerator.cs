using System;
using QueemCore.BitBoard.Helpers;

namespace MovesGenerators
{
	public class BishopMovesGenerator : Generator
	{
		public override void Run ()
		{
			this.data = BishopBitBoardHelper.AntiDiagonalsMasks;
		}
		
		public override void WriteResults (System.IO.TextWriter tw)
		{
			var arr = this.data as ulong[];
			
			foreach (var item in arr)
			{
				tw.WriteLine(BitBoardHelper.ToString(item, "\n"));
				tw.WriteLine();
			}
		}
	}
}

