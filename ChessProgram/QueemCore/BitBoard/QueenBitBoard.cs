using System;
using System.Collections.Generic;

namespace QueemCore.BitBoard
{	
	public class QueenBitBoard : BitBoard
	{
		public QueenBitBoard ()
			:base()
		{
		}
		
		public QueenBitBoard (ulong val)
			:base(val)
		{
		}
		
		public override IEnumerable<ulong> GetAttacks ()
		{
			throw new NotImplementedException ();
		}
	}
}

