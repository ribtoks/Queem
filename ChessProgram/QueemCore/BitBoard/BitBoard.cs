using System;
using System.Linq;
using QueemCore.Extensions;
using QueemCore.BitBoard.Helpers;

namespace QueemCore.BitBoard
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
		
		public BitBoard ToggleBit (Square square)
		{
			if (square != Square.NoSquare) 
			{
				ulong oneBitNumber = 1UL << (int)square;
				this.board = this.board ^ oneBitNumber;
			}
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
			this.ToggleBit(move.From);
			this.ToggleBit(move.To);
		}
		
		public BitBoard FlipVertical()
		{
			this.board = BitBoardHelper.FlipVertical(this.board);			
			return this;
		}
		
		public BitBoard FlipHorizontal()
		{
			this.board = BitBoardHelper.FlipHorizontal(this.board);			
			return this;
		}
		
		public BitBoard FlipDiagonal_A1H8()
		{
		   	this.board = BitBoardHelper.FlipDiagonalA1H8(this.board);			
			return this;
		}
		
		public BitBoard FlipDiagonal_A8H1()
		{
			this.board = BitBoardHelper.FlipDiagonalA8H1(this.board);			
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

