using System;
using QueemCore.Extensions;
using System.Linq;
using System.Diagnostics;
using QueemCore.BitBoard;
using QueemCore;

namespace MovesGenerators
{
	public class RookMovesGenerator : Generator
	{
		public override void Run ()
		{
			this.data = RookBitBoardHelper.GenerateRotatedRanks();
		}
		
		public override void WriteResults (System.IO.TextWriter tw)
		{
			tw.WriteLine (string.Join("\n\n", 
			                               ((ulong[])this.data).Select(u => this.GetBoardString(u)).ToArray()));
		}
	}
}

