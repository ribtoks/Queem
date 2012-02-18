using System;
using System.Collections;
using System.Collections.Generic;
using QueemCore.Extensions;
using System.Linq;

namespace QueemCore.BitBoard.Helpers
{
	public static class BitBoardHelper
	{
		public static int BitsCount(ulong board)
		{
			ulong k1 = 0x5555555555555555UL;
			ulong k2 = 0x3333333333333333UL;
			ulong k4 = 0x0f0f0f0f0f0f0f0fUL;
			
			ulong x = board;
			
			x =  x - ((x >> 1)  & k1); /* put count of each 2 bits into those 2 bits */
    		x = (x & k2) + ((x >> 2)  & k2); /* put count of each 4 bits into those 4 bits */
    		x = (x + (x >> 4)) & k4 ; /* put count of each 8 bits into those 8 bits */
    		x += x >>  8;  /* put count of each 16 bits into their lowest 8 bits */
    		x += x >> 16;  /* put count of each 32 bits into their lowest 8 bits */
    		x += x >> 32;  /* put count of the final 64 bits into the lowest 8 bits */
    		return (int) x & 255;
		}
	
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
		
		public static readonly ulong NotAFile = 0xfefefefefefefefeUL;
		public static readonly ulong NotHFile = 0x7f7f7f7f7f7f7f7fUL;
		public static readonly ulong NotABFile = 0xfcfcfcfcfcfcfcfcUL;
		public static readonly ulong NotGHFile = 0x3f3f3f3f3f3f3f3fUL;
		
		public static ulong ShiftSouthOne(ulong b) { return b >> 8;	}		
		public static ulong ShiftNorthOne(ulong b) { return b << 8;	}
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
		
		public static ulong FlipDiagonalA8H1(ulong board)
		{
			ulong k1 = 0xaa00aa00aa00aa00UL;
		   	ulong k2 = 0xcccc0000cccc0000UL;
		   	ulong k4 = 0xf0f0f0f00f0f0f0fUL;
			ulong x = board;
		   	ulong t =  x ^ (x << 36) ;
		   	x ^= k4 & (t ^ (x >> 36));
		   	t = k2 & (x ^ (x << 18));
		   	x ^= t ^ (t >> 18) ;
		   	t = k1 & (x ^ (x <<  9));
		   	x ^= t ^ (t >>  9) ;
		   	return x;
		}
		
		public static ulong FlipVertical(ulong board)
		{
			ulong k1 = 0x00FF00FF00FF00FFUL;
			ulong k2 = 0x0000FFFF0000FFFFUL;
			ulong x = board;
			x = ((x >>  8) & k1) | ((x & k1) <<  8);
   			x = ((x >> 16) & k2) | ((x & k2) << 16);
   			x = ( x >> 32) | ( x << 32);
			return x;
		}
		
		public static ulong FlipHorizontal(ulong board)
		{
			ulong k1 = 0x5555555555555555UL;
   			ulong k2 = 0x3333333333333333UL;
   			ulong k4 = 0x0f0f0f0f0f0f0f0fUL;
			ulong x = board;
		   	x = ((x >> 1) & k1) +  2*(x & k1);
		   	x = ((x >> 2) & k2) +  4*(x & k2);
		   	x = ((x >> 4) & k4) + 16*(x & k4);
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
		
		public static readonly int[] lsb_64_table = 
		{  63, 30,  3, 32, 59, 14, 11, 33,
		   60, 24, 50,  9, 55, 19, 21, 34,
		   61, 29,  2, 53, 51, 23, 41, 18,
		   56, 28,  1, 43, 46, 27,  0, 35,
		   62, 31, 58,  4,  5, 49, 54,  6,
		   15, 52, 12, 40,  7, 42, 45, 16,
		   25, 57, 48, 13, 10, 39,  8, 44,
		   20, 47, 38, 22, 17, 37, 36, 26
		};
		
		public static int BitScan(ulong bb)
		{
			int folded = 0;
			folded  = (int)((bb ^ (bb-1)) >> 32);
   			folded ^= (int)( bb ^ (bb-1)); // lea
   			return lsb_64_table[folded * 0x78291ACF >> 26];			
		}
		
		public static IEnumerable<string> SplitString(string str, int chunkSize)
    	{
        	return Enumerable.Range(0, str.Length / chunkSize)
            	.Select(i => str.Substring(i * chunkSize, chunkSize));
		}
		
		public static string ToString(ulong board, string separator)
		{
			return string.Join(separator, 
						BitConverter.GetBytes(board)
						.Reverse()
						.Select(b => 
					        Convert.ToString(b, 2)
					        .LJust(8, '0')
					        .MyReverse()).ToArray());
		}
		
		public static ulong FromString(string s)
		{
			/*
			 * string indices
			*  0  1  2  3  4  5  6  7    56 57 58 59 60 61 62 63
		 	*  8  9  10 11 12 13 14 15   48 49 50 51 52 53 54 55
		 	*  16 17 18 19 20 21 22 23   40 41 42 43 44 45 46 47
		 	*  24 25 26 27 28 29 30 31   32 33 34 35 36 37 38 39
		 	*  32 33 34 35 36 37 38 39   24 25 26 27 28 29 30 31
		 	*  40 41 42 43 44 45 46 47   16 17 18 19 20 21 22 23
		 	*  48 49 50 51 52 53 54 55   8  9  10 11 12 13 14 15
		 	*  56 57 58 59 60 61 62 63   0  1  2  3  4  5  6  7 
			*/
			var chunks8 = SplitString(s, 8)
				.Select(t => t.MyReverse());
			
			/*
			 * string indices
			 * 7  6  5  4  3  2  1  0
			 * 15 14 13 12 11 10 9  8
			 * 23 22 21 20 19 18 17 16
			 * 31 30 29 28 27 26 25 24
			 * 39 38 37 36 35 34 33 32
			 * 47 46 45 44 43 42 41 40
			 * 55 54 53 52 51 50 49 48
			 * 63 62 61 60 59 58 57 56
			*/
			
			/*
			 * real indices in integer
			 * 63 62 61 60 59 58 57 56
			 * 55 54 53 52 51 50 49 48
			 * 47 46 45 44 43 42 41 40
			 * 39 38 37 36 35 34 33 32
			 * 31 30 29 28 27 26 25 24
			 * 23 22 21 20 19 18 17 16
			 * 15 14 13 12 11 10 9  8
			 * 7  6  5  4  3  2  1  0
			*/
			
			var joined = string.Join(string.Empty, chunks8.ToArray());
			var chunks32 = SplitString(joined, 32).ToArray();
			
			uint firstPart = Convert.ToUInt32(chunks32[0], 2);
			uint secondPart = Convert.ToUInt32(chunks32[1], 2);
			
			ulong first = ((ulong)firstPart) << 32;
			ulong second = (ulong)secondPart;
			
			return first | second;
		}
	}
}

