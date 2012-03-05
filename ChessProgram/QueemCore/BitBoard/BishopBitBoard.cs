using System;
using System.Collections.Generic;

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
		
		public override IEnumerable<ulong> GetAttacks()
		{
			yield return 0;
		}
	}
}

