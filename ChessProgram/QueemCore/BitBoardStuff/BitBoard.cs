using System;
using System.Linq;
using QueemCore.Extensions;

namespace QueemCore
{
	public class BitBoard
	{
		/*
		 * Indices structure
		 * 
		 * 56 57 58 59 60 61 62 63
		 * 48 49 50 51 52 53 54 55
		 * 40 41 42 43 44 45 46 47
		 * 32 33 34 35 36 37 38 39
		 * 24 25 26 27 28 29 30 31
		 * 16 17 18 19 20 21 22 23
		 * 8  9  10 11 12 13 14 15
		 * 0  1  2  3  4  5  6  7
		*/
		protected ulong board;
		
		public BitBoard()
		{
			this.board = 0;
		}
		
		public BitBoard(ulong value)
		{
			this.board = value;
		}
		
		public int GetBitsCount()
		{
			ulong k1 = 0x5555555555555555UL;
			ulong k2 = 0x3333333333333333UL;
			ulong k4 = 0x0f0f0f0f0f0f0f0fUL;
			
			ulong x = this.board;
			
			x =  x - ((x >> 1)  & k1); /* put count of each 2 bits into those 2 bits */
    		x = (x & k2) + ((x >> 2)  & k2); /* put count of each 4 bits into those 4 bits */
    		x = (x + (x >> 4)) & k4 ; /* put count of each 8 bits into those 8 bits */
    		x += x >>  8;  /* put count of each 16 bits into their lowest 8 bits */
    		x += x >> 16;  /* put count of each 32 bits into their lowest 8 bits */
    		x += x >> 32;  /* put count of the final 64 bits into the lowest 8 bits */
    		return (int) x & 255;
		}
		
		public bool IsSubsetOf(BitBoard b)
		{
			return ((this.board & b.board) == this.board);
		}
		
		public bool IsDisjointWith(BitBoard b)
		{
			return (this.board & b.board) == 0;
		}
		
		public BitBoard GetComplement()
		{
			return new BitBoard( ~(this.board) );
		}
		
		public bool IsBitZero(int rank, int file)
		{
			var oneBitNumber = this.GetOneBitNumber(rank, file);
			ulong negativeOneBit = (~oneBitNumber);
			return (this.board & negativeOneBit) == this.board;
		}
		
		public bool IsBitOne(int rank, int file)
		{
			return !(this.IsBitZero(rank, file));
		}
		
		public BitBoard ToggleBit(int rank, int file)
		{
			var oneBitNumber = this.GetOneBitNumber(rank, file);	
			this.board = this.board ^ oneBitNumber;
			
			return this;
		}
		
		public BitBoard ToggleBit(int squareIndex)
		{
			ulong oneBitNumber = 1UL << squareIndex;
			this.board = this.board ^ oneBitNumber;
			
			return this;
		}
		
		protected ulong GetOneBitNumber(int rank, int file)
		{
			// rank and file in 0..7
			int squareIndex = rank*8 + file;
			return 1UL << squareIndex;
		}
		
		public ulong GetInnerValue()
		{
			return this.board;
		}
		
		public void DoMove(Move move)
		{
			this.ToggleBit((int)move.From);
			this.ToggleBit((int)move.To);
		}
		
		public BitBoard FlipVertical()
		{
			ulong k1 = 0x00FF00FF00FF00FFUL;
			ulong k2 = 0x0000FFFF0000FFFFUL;
			ulong x = this.board;
			x = ((x >>  8) & k1) | ((x & k1) <<  8);
   			x = ((x >> 16) & k2) | ((x & k2) << 16);
   			x = ( x >> 32) | ( x << 32);
			this.board = x;
			
			return this;
		}
		
		public BitBoard FlipHorisontal()
		{
			ulong k1 = 0x5555555555555555UL;
   			ulong k2 = 0x3333333333333333UL;
   			ulong k4 = 0x0f0f0f0f0f0f0f0fUL;
			ulong x = this.board;
		   	x = ((x >> 1) & k1) +  2*(x & k1);
		   	x = ((x >> 2) & k2) +  4*(x & k2);
		   	x = ((x >> 4) & k4) + 16*(x & k4);
		   	this.board = x;
			
			return this;
		}
		
		public BitBoard FlipDiagonal_A1H8()
		{
		   	ulong k1 = 0x5500550055005500UL;
		   	ulong k2 = 0x3333000033330000UL;
		   	ulong k4 = 0x0f0f0f0f00000000UL;
			ulong x = this.board;
		   	ulong t  = k4 & (x ^ (x << 28));
		   	x ^= t ^ (t >> 28) ;
		   	t = k2 & (x ^ (x << 14));
		   	x ^= t ^ (t >> 14) ;
		   	t = k1 & (x ^ (x <<  7));
		   	x ^= t ^ (t >>  7) ;
		   	this.board = x;
			
			return this;
		}
		
		public BitBoard FlipDiagonal_A8H1()
		{
			ulong k1 = 0xaa00aa00aa00aa00UL;
		   	ulong k2 = 0xcccc0000cccc0000UL;
		   	ulong k4 = 0xf0f0f0f00f0f0f0fUL;
			ulong x = this.board;
		   	ulong t =  x ^ (x << 36) ;
		   	x ^= k4 & (t ^ (x >> 36));
		   	t = k2 & (x ^ (x << 18));
		   	x ^= t ^ (t >> 18) ;
		   	t = k1 & (x ^ (x <<  9));
		   	x ^= t ^ (t >>  9) ;
		   	this.board = x;
			
			return this;
		}
		
		public BitBoard Rotate180()
		{
			return this.FlipHorisontal().FlipVertical();
		}
		
		public BitBoard Rotate90Clockwise()
		{
			return this.FlipVertical().FlipDiagonal_A1H8();
		}
		
		public BitBoard Rotate90CounterClockwise()
		{
			return this.FlipDiagonal_A1H8().FlipVertical();
		}
		
		public override string ToString ()
		{
			return string.Join ("\n", BitConverter.GetBytes (this.board)
						.Reverse ()
						.Select (b => 
					        Convert.ToString (b, 2)
	           					.LJust (8, '0')
	           					.MyReverse ())
			            .ToArray ());
		}
	}
}

