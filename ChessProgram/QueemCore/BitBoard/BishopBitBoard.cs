using System;

namespace QueemCore.BitBoard
{
	public class BishopBitBoard : BitBoard
	{
		public BishopBitBoard ()
			:base()
		{
		}
		
		public BishopBitBoard(ulong val)
			:base(val)
		{					
		}
		
		public ulong GetAttacks()
		{
			return 0;
		}
	}
}

