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
			ulong knights = this.board;
		   	ulong l1 = (knights >> 1) & 0x7f7f7f7f7f7f7f7fUL;
		   	ulong l2 = (knights >> 2) & 0x3f3f3f3f3f3f3f3fUL;
		   	ulong r1 = (knights << 1) & 0xfefefefefefefefeUL;
		   	ulong r2 = (knights << 2) & 0xfcfcfcfcfcfcfcfcUL;
		   	ulong h1 = l1 | r1;
		   	ulong h2 = l2 | r2;
		   	return (h1<<16) | (h1>>16) | (h2<<8) | (h2>>8);
		}
	}
}

