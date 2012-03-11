using System;

namespace QueemCore.BitBoards.Helpers
{
	public static class PawnBitBoardHelper
	{
		public static ulong NorthPawnsFill(ulong gen)
		{
			gen |= (gen <<  8);
		   	gen |= (gen << 16);
		   	gen |= (gen << 32);
		   	return gen;
		}
		
		public static ulong SouthPawnsFill(ulong gen)
		{
			gen |= (gen >>  8);
		   	gen |= (gen >> 16);
		   	gen |= (gen >> 32);
		   	return gen;
		}
		
		public static ulong FilePawnFill(ulong gen)
		{
			return (NorthPawnsFill(gen) | SouthPawnsFill(gen));
		}
	}
}

