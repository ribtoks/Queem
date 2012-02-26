using System;
using System.Linq;
using QueemCore.BitBoard.Helpers;
using QueemCore.Extensions;

namespace MovesGenerators
{
	public abstract class Generator
	{
		protected object data;
		
		public abstract void Run();
		public virtual object GetResults() { return this.data; }
		public abstract void WriteResults(System.IO.TextWriter tw);
		
		protected string GetBoardString(ulong board)
		{
			return BitBoardHelper.ToString(board, "\n");
		}
	}
}

