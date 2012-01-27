using System;

namespace QueemCore
{
	public class KnightBitBoard : BitBoard
	{
		public KnightBitBoard ()
			:base()
		{
		}
		
		public KnightBitBoard(ulong val)
			:base(val)
		{
		}
		
		public ulong GetAttacks() 
		{
			return this.GetKnightAttacks(this.board);
		}
		
		private ulong GetKnightAttacks(ulong board)
		{
			ulong knights = board;
		   	ulong l1 = (knights >> 1) & 0x7f7f7f7f7f7f7f7fUL;
		   	ulong l2 = (knights >> 2) & 0x3f3f3f3f3f3f3f3fUL;
		   	ulong r1 = (knights << 1) & 0xfefefefefefefefeUL;
		   	ulong r2 = (knights << 2) & 0xfcfcfcfcfcfcfcfcUL;
		   	ulong h1 = l1 | r1;
		   	ulong h2 = l2 | r2;
		   	return (h1<<16) | (h1>>16) | (h2<<8) | (h2>>8);
		}
		
		// no set should be empty -> assert (b1 != 0 && b2 != 0 )
		private int CalculateDistance(ulong b1, ulong b2)
		{
			int d = 0;
   			while ((b1 & b2) == 0) 
			{
				 // as long as sets are disjoint
      			b1 = this.GetKnightAttacks(b1);
				// increment distance
				++d;
   			}
   			return d;
		}
		
		public int KnightDistance(int a, int b)
		{
			return this.CalculateDistance(1UL << a, 1UL << b);
		}
	}
}

