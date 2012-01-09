using System;

namespace QueemCore
{
	public static class BitBoardHelper
	{
		public static ulong RotateLeft(ulong x, int s)
		{
			return (x << s) | (x >> (64-s));
		}
		
		public static ulong RotateRight(ulong x, int s)
		{
			return (x >> s) | (x << (64-s));
		}
		
		/*
		 * Shifting
		*/
		
		/*
		 *   northwest    north   northeast
		 *           +7    +8    +9
		 *               \  |  /
		 *   west    -1 <-  0 -> +1    east
		 *               /  |  \
		 *           -9    -8    -7
		 *   southwest    south   southeast
		 * 
		*/
		
		public static ulong ShiftSouthOne(ulong b)
		{
			return b >> 8;
		}
		
		public static ulong ShiftNorthOne(ulong b)
		{
			return b << 8;
		}
		
		public static readonly ulong NotAFile = 0xfefefefefefefefeUL;
		public static readonly ulong NotHFile = 0x7f7f7f7f7f7f7f7fUL;
		
		public static ulong ShiftEastOne (ulong b) {return (b & NotHFile) << 1;}
		public static ulong ShiftNorthEastOne (ulong b) {return (b & NotHFile) << 9;}
		public static ulong ShiftSouthEastOne (ulong b) {return (b & NotHFile) >> 7;}
		public static ulong ShiftWestOne (ulong b) {return (b & NotAFile) >> 1;}
		public static ulong ShiftSouthWestOne (ulong b) {return (b & NotAFile) >> 9;}
		public static ulong ShiftNorhtWestOne (ulong b) {return (b & NotAFile) << 7;}
		
		/**
		 * swap n none overlapping bits of bit-index i with j
		 * @param b any bitboard
		 * @param i,j positions of bit sequences to swap
		 * @param n number of consecutive bits to swap
		 * @return bitboard b with swapped bit-sequences
		 */
		public static ulong SwapNBits(ulong b, int i, int j, int n) 
		{
		   ulong m = (1UL << n) - 1;
		   ulong x = ((b >> i) ^ (b >> j)) & m;
		   return  b ^  (x << i) ^ (x << j);
		}
		
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

