using System;
using System.Linq;

namespace MovesGenerators
{
	public class RankMasksGenerator : Generator
	{
		private ulong GetRankMask(int i)
		{
			ulong board = 0xffUL;
			board <<= i*8;
			return board;
		}
		
		public override void Run ()
		{
			ulong[] masks = new ulong[8];
			for (int i = 0; i < 8; ++i)
			{
				masks[i] = GetRankMask(i);
			}
			
			this.data = masks;
		}
		
		public override void WriteResults (System.IO.TextWriter tw)
		{			
			//tw.WriteLine (string.Join("\n\n", 
			//                               ((ulong[])this.data).Select(u => this.GetBoardString(u)).ToArray()));
			
			tw.WriteLine (string.Join(", ", 
			                               ((ulong[])this.data).Select(u => "0x" + u.ToString("X") + "UL").ToArray()));
		}
	}
}

