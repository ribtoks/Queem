using System;

namespace QueemCore.BitBoard
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
		public static readonly ulong NotABFile = 0xfcfcfcfcfcfcfcfcUL;
		public static readonly ulong NotGHFile = 0x3f3f3f3f3f3f3f3fUL;
		
		public static ulong ShiftEastOne (ulong b) {return (b & NotHFile) << 1;}
		public static ulong ShiftNorthEastOne (ulong b) {return (b & NotHFile) << 9;}
		public static ulong ShiftSouthEastOne (ulong b) {return (b & NotHFile) >> 7;}
		public static ulong ShiftWestOne (ulong b) {return (b & NotAFile) >> 1;}
		public static ulong ShiftSouthWestOne (ulong b) {return (b & NotAFile) >> 9;}
		public static ulong ShiftNorhtWestOne (ulong b) {return (b & NotAFile) << 7;}
		
		public static ulong ShiftNorthNorthEast(ulong b) {return (b << 17) & NotAFile ;}
		public static ulong ShiftNorthEastEast(ulong b) {return (b << 10) & NotABFile;}
		public static ulong ShiftSouthEastEast(ulong b) {return (b >>  6) & NotABFile;}
		public static ulong ShiftSouthSouthEast(ulong b) {return (b >> 15) & NotAFile ;}
		public static ulong ShiftNorthNorthWest(ulong b) {return (b << 15) & NotHFile ;}
		public static ulong ShiftNorthWestWest(ulong b) {return (b <<  6) & NotGHFile;}
		public static ulong ShiftSouthWestWest(ulong b) {return (b >> 10) & NotGHFile;}
		public static ulong ShiftSouthSouthWest(ulong b) {return (b >> 17) & NotHFile ;}
		
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
		
		public static ulong FlipDiagonalA1H8(ulong board)
		{
			ulong k1 = 0x5500550055005500UL;
		   	ulong k2 = 0x3333000033330000UL;
		   	ulong k4 = 0x0f0f0f0f00000000UL;
			ulong x = board;
		   	ulong t  = k4 & (x ^ (x << 28));
		   	x ^= t ^ (t >> 28) ;
		   	t = k2 & (x ^ (x << 14));
		   	x ^= t ^ (t >> 14) ;
		   	t = k1 & (x ^ (x <<  7));
		   	x ^= t ^ (t >>  7) ;
		   	return x;
		}
		
		public static readonly ulong[] FilesMasks = 
			{0x101010101010101UL, 0x202020202020202UL, 0x404040404040404UL, 0x808080808080808UL, 
			0x1010101010101010UL, 0x2020202020202020UL, 0x4040404040404040UL, 0x8080808080808080UL};
		public static readonly ulong[] RanksMasks = 
			{0xFFUL, 0xFF00UL, 0xFF0000UL, 0xFF000000UL, 
			0xFF00000000UL, 0xFF0000000000UL, 0xFF000000000000UL, 0xFF00000000000000UL};
		
		public static ulong FileMask(File file)
		{
			return FilesMasks[(int)file];
		}
		
		public static ulong RankMask(int rank)
		{
			return RanksMasks[rank];
		}
	}
}

