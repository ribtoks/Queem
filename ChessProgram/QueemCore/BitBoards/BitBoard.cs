using System;
using System.Linq;
using QueemCore.Extensions;
using QueemCore.BitBoards.Helpers;
using System.Collections.Generic;
using QueemCore.AttacksGenerators;

namespace QueemCore.BitBoards
{
	public class BitBoard
	{
		/*
		 * Indices structure ((7 - y)*8 + x)
		 * 
		 * 56 57 58 59 60 61 62 63   0  1  2  3  4  5  6  7 
		 * 48 49 50 51 52 53 54 55   8  9  10 11 12 13 14 15
		 * 40 41 42 43 44 45 46 47   16 17 18 19 20 21 22 23
		 * 32 33 34 35 36 37 38 39   24 25 26 27 28 29 30 31
		 * 24 25 26 27 28 29 30 31   32 33 34 35 36 37 38 39
		 * 16 17 18 19 20 21 22 23   40 41 42 43 44 45 46 47
		 * 8  9  10 11 12 13 14 15   48 49 50 51 52 53 54 55
		 * 0  1  2  3  4  5  6  7    56 57 58 59 60 61 62 63
		*/
		
		/*
		 * Indices structure (y*8 + (7 - x))
		 * 
		 * 56 57 58 59 60 61 62 63   63 62 61 60 59 58 57 56
		 * 48 49 50 51 52 53 54 55   55 54 53 52 51 50 49 48
		 * 40 41 42 43 44 45 46 47   47 46 45 44 43 42 41 40
		 * 32 33 34 35 36 37 38 39   39 38 37 36 35 34 33 32
		 * 24 25 26 27 28 29 30 31   31 30 29 28 27 26 25 24
		 * 16 17 18 19 20 21 22 23   23 22 21 20 19 18 17 16
		 * 8  9  10 11 12 13 14 15   15 14 13 12 11 10 9  8
		 * 0  1  2  3  4  5  6  7    7  6  5  4  3  2  1  0
		*/
		
		protected ulong board;
		
		public BitBoard()
			:this(0)
		{
		}
				
		public BitBoard(ulong value)
		{
			this.board = value;
		}
				
		public virtual IEnumerable<Square> GetOwnFigures()
		{
			return BitBoardSerializer.GetSquares(this.board);
		}
		
		public int GetBitsCount()
		{
			return BitBoardHelper.BitsCount(this.board);
		}
		
		public bool IsSubsetOf(BitBoard b)
		{
			return ((this.board & b.board) == this.board);
		}
		
		public bool IsDisjointWith(BitBoard b)
		{
			return (this.board & b.board) == 0;
		}
						
		public bool IsBitZero(int rank, int file)
		{
			var oneBitNumber = this.GetOneBitNumber(rank, file);
			ulong negativeOneBit = (~oneBitNumber);
			return (this.board & negativeOneBit) == this.board;
		}
		
		public bool IsBitSet(int rank, int file)
		{
			return !(this.IsBitZero(rank, file));
		}
		
		public bool IsBitSet(Square sq)
		{
			ulong oneBitNumber = 1UL << (int)sq;			
			return (this.board & oneBitNumber) == oneBitNumber;
		}
		
		public virtual BitBoard SetBit(Square sq)
		{
			ulong oneBitNumber = 1UL << (int)sq;
			this.board = this.board | oneBitNumber;
			
			return this;
		}
		
		public virtual BitBoard UnsetBit(Square sq)
		{
			ulong oneBitNumber = 1UL << (int)sq;
			this.board = this.board & (~oneBitNumber);
			
			return this;
		}
		
		protected BitBoard ToggleBit (Square square)
		{
			// unpredictable if square is NoSquare
			ulong oneBitNumber = 1UL << (int)square;
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
		
		public virtual int GetInnerProperty()
		{
			return 0;
		}
		
		public virtual void SetInnerProperty(int property)
		{
		}
		
		public virtual void DoMove(Move move)
		{
			ulong moveFrom = 1UL << (int)move.From;
			ulong moveTo = 1UL << (int)move.To;
			
			this.board &= (~moveFrom);
			this.board |= moveTo;
		}
		
		public virtual void UndoMove(Move move)
		{
			ulong moveFrom = 1UL << (int)move.From;
			ulong moveTo = 1UL << (int)move.To;
			
			this.board &= (~moveTo);
			this.board |= moveFrom;
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
			return this.FlipHorizontal().FlipVertical();
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
			return BitBoardHelper.ToString(this.board, "\n");
		}
	}
}

