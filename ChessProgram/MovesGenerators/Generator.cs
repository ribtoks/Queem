using System;
using System.Linq;
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
			return string.Join("\n", BitConverter.GetBytes(board)
						.Reverse()
						.Select(b => 
					        new string(Convert.ToString(b, 2).LJust(8, '0').Reverse().ToArray())).ToArray());
		}
		
		
	}
}

