using System;
using QueemCore.BitBoard;

namespace QueemCore.BitBoard
{
	public class RookBitBoard : BitBoard
	{
		public RookBitBoard ()
			:base()
		{
		}
		
		public RookBitBoard(ulong val)
			:base(val)
		{
		}
		
		public ulong GetAttacks()
		{
			return 0;
		}
	}
}

