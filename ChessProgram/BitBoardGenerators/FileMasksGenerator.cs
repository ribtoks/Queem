using System;
using System.Linq;
using QueemCore.BitBoard;

namespace MovesGenerators
{
	public class FileMasksGenerator : Generator
	{
		protected ulong GetFileMask(int file)
		{
			ulong board = 1UL << file;
			int count = 8;
			while(--count > 0) 
				board |= BitBoardHelper.ShiftNorthOne(board);
			
			return board;
		}
		
		public override void Run ()
		{
			ulong[] masks = new ulong[8];
			for (int i = 0; i < 8; ++i)
			{
				masks[i] = GetFileMask(i);
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

