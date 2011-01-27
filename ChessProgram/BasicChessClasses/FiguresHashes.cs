using System;
namespace BasicChessClasses
{
	public static class FiguresHashes
	{
		private static int i;
		
		static FiguresHashes ()
		{
			i = 0;
		}
		
		public static int GetNextHash ()
		{
			return i++;
		}
	}
}

