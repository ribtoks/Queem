using System;
using QueemCore;

namespace QueemCore.BitBoards.Helpers
{

	public static class KingBitBoardHelper
	{
		public static readonly ulong[] KingMoves = 
		{
			0x302UL, 0x705UL, 0xE0AUL, 0x1C14UL, 0x3828UL, 0x7050UL, 0xE0A0UL, 0xC040UL, 0x30203UL, 
			0x70507UL, 0xE0A0EUL, 0x1C141CUL, 0x382838UL, 0x705070UL, 0xE0A0E0UL, 0xC040C0UL, 0x3020300UL, 
			0x7050700UL, 0xE0A0E00UL, 0x1C141C00UL, 0x38283800UL, 0x70507000UL, 0xE0A0E000UL, 0xC040C000UL, 
			0x302030000UL, 0x705070000UL, 0xE0A0E0000UL, 0x1C141C0000UL, 0x3828380000UL, 0x7050700000UL, 
			0xE0A0E00000UL, 0xC040C00000UL, 0x30203000000UL, 0x70507000000UL, 0xE0A0E000000UL, 0x1C141C000000UL, 
			0x382838000000UL, 0x705070000000UL, 0xE0A0E0000000UL, 0xC040C0000000UL, 0x3020300000000UL, 0x7050700000000UL, 
			0xE0A0E00000000UL, 0x1C141C00000000UL, 0x38283800000000UL, 0x70507000000000UL, 0xE0A0E000000000UL, 
			0xC040C000000000UL, 0x302030000000000UL, 0x705070000000000UL, 0xE0A0E0000000000UL, 0x1C141C0000000000UL, 
			0x3828380000000000UL, 0x7050700000000000UL, 0xE0A0E00000000000UL, 0xC040C00000000000UL, 0x203000000000000UL, 
			0x507000000000000UL, 0xA0E000000000000UL, 0x141C000000000000UL, 0x2838000000000000UL, 0x5070000000000000UL, 
			0xA0E0000000000000UL, 0x40C0000000000000UL
		};
				
		// Returns true if a path of set bits in 'path' exists that 8-way connect
		// any set bit in sq1 to any set bit of sq2		 
		public static bool SquaresAreConnected(ulong sq1, ulong sq2, ulong path)
		{
		   	// With bitboard sq1, do an 8-way flood fill, masking off bits not in
		   	// path at every step. Stop when fill reaches any set bit in sq2, or
		   	// fill cannot progress any further
			 
		   	if ((~(sq1 &= path) | ~(sq2 &= path)) != 0) 
					return false;
		                      
			// Drop bits not in path
		    // Early exit if sq1 or sq2 not on any path
		 
		   	while((sq1 & sq2) != 0)
		   	{
		      	ulong temp = sq1;
				// Set all 8 neighbours
		      	sq1 |= BitBoardHelper.ShiftEastOne(sq1) | BitBoardHelper.ShiftWestOne(sq1);    
		      	sq1 |= BitBoardHelper.ShiftSouthOne(sq1) | BitBoardHelper.ShiftNorthOne(sq1);
				// Drop bits not in path
		      	sq1 &= path;
		      	if (sq1 == temp)
					// Fill has stopped
					return false;
		   }
			// Found a good path
		   	return true;
		}
		
		public static readonly ulong[][][] CastlingMasks;
		public static readonly Square[][][][] CastlingSquares;
		public static readonly Move[][][][] CastlingMoves;
		
		static KingBitBoardHelper()
		{
			CastlingMasks = CreateCastlingMasks();
			CastlingSquares = CreateCastlingSquares();
			CastlingMoves = CreateCastlingMoves();
		}		
		
		public static Move[][][][] CreateCastlingMoves()
		{
			var moves = new Move[2][][][];
			
			for (int pos = 0; pos < 2; ++pos)
			{
				moves[pos] = new Move[2][][];
				moves[pos][0] = new Move[3][];
				moves[pos][1] = new Move[3][];
			}
			
			moves[(int)Color.White][(int)PlayerPosition.Down][0] = 
				new Move[] {new Move(Square.E1, Square.C1)};
			moves[(int)Color.White][(int)PlayerPosition.Down][1] = 
				new Move[] {new Move(Square.E1, Square.G1)};
			moves[(int)Color.White][(int)PlayerPosition.Down][2] = 
				new Move[] {new Move(Square.E1, Square.C1), new Move(Square.E1, Square.G1)};
				
			moves[(int)Color.Black][(int)PlayerPosition.Down][0] = 
				new Move[] {new Move(Square.D1, Square.B1)};
			moves[(int)Color.Black][(int)PlayerPosition.Down][1] = 
				new Move[] {new Move(Square.D1, Square.F1)};
			moves[(int)Color.Black][(int)PlayerPosition.Down][2] = 
				new Move[] {new Move(Square.D1, Square.B1), new Move(Square.D1, Square.F1)};
				
			moves[(int)Color.White][(int)PlayerPosition.Up][0] = 
				new Move[] {new Move(Square.E8, Square.C8)};
			moves[(int)Color.White][(int)PlayerPosition.Up][1] = 
				new Move[] {new Move(Square.E8, Square.G8)};
			moves[(int)Color.White][(int)PlayerPosition.Up][2] = 
				new Move[] {new Move(Square.E8, Square.C8), new Move(Square.E8, Square.G8)};
				
			moves[(int)Color.Black][(int)PlayerPosition.Up][0] = 
				new Move[] {new Move(Square.D8, Square.B8)};
			moves[(int)Color.Black][(int)PlayerPosition.Up][1] = 
				new Move[] {new Move(Square.D8, Square.F8)};
			moves[(int)Color.Black][(int)PlayerPosition.Up][2] = 
				new Move[] {new Move(Square.D8, Square.B8), new Move(Square.D8, Square.F8)};
			
			return moves;
		}
		
		public static ulong[] GetCastlingMask(PlayerPosition playerPosition, Color playerColor)
		{
			return CastlingMasks[(int)playerColor][(int)playerPosition];
		}
		
		private static Square[][][][] CreateCastlingSquares()
		{
			Square[][][][] squares = new Square[2][][][];
			for (int pos = 0; pos < 2; ++pos)
			{
				squares[pos] = new Square[2][][];
				for (int leftright = 0; leftright < 2; ++leftright)
					squares[pos][leftright] = new Square[2][];
			}
				
			squares[(int)Color.White][(int)PlayerPosition.Down][0] = new Square[] {Square.C1, Square.D1};
			squares[(int)Color.White][(int)PlayerPosition.Down][1] = new Square[] {Square.F1, Square.G1};
			
			squares[(int)Color.White][(int)PlayerPosition.Up][0] = new Square[] { Square.B8, Square.C8 };
			squares[(int)Color.White][(int)PlayerPosition.Up][1] = new Square[] { Square.E8, Square.F8 };
			
			squares[(int)Color.Black][(int)PlayerPosition.Down][0] = new Square[] { Square.B1, Square.C1 };
			squares[(int)Color.Black][(int)PlayerPosition.Down][1] = new Square[] { Square.E1, Square.F1 };
			
			squares[(int)Color.Black][(int)PlayerPosition.Up][0] = new Square[] {Square.C8, Square.D8};
			squares[(int)Color.Black][(int)PlayerPosition.Up][1] = new Square[] {Square.F8, Square.G8};
			
			return squares;
		}
		
		private static ulong[][][] CreateCastlingMasks()
		{
			ulong[][][] masks = new ulong[2][][];
			
			for (int pos = 0; pos < 2; ++pos)
				masks[pos] = new ulong[2][];
			
			masks[(int)Color.White][(int)PlayerPosition.Down] = GetLowerLongShortMasks();
			masks[(int)Color.White][(int)PlayerPosition.Up] = GetUpperShortLongMasks();
			
			masks[(int)Color.Black][(int)PlayerPosition.Down] = GetLowerShortLongMasks();
			masks[(int)Color.Black][(int)PlayerPosition.Up] = GetUpperLongShortMasks();
			
			return masks;
		}
		
		private static ulong[] GetLowerLongShortMasks()
		{
			var leftLongBoard = new BitBoard();
			leftLongBoard.SetBit(Square.C1);
			leftLongBoard.SetBit(Square.D1);
			
			var rightShortBoard = new BitBoard();
			rightShortBoard.SetBit(Square.F1);
			rightShortBoard.SetBit(Square.G1);
			
			return new ulong[] {leftLongBoard.GetInnerValue(), 
					rightShortBoard.GetInnerValue()};
		}
		
		private static ulong[] GetLowerShortLongMasks()
		{
			var leftShortBoard = new BitBoard();
			leftShortBoard.SetBit(Square.B1);
			leftShortBoard.SetBit(Square.C1);
			
			var rightLongBoard = new BitBoard();
			rightLongBoard.SetBit(Square.E1);
			rightLongBoard.SetBit(Square.F1);
			
			return new ulong[] {leftShortBoard.GetInnerValue(), 
					rightLongBoard.GetInnerValue()};
		}
		
		private static ulong[] GetUpperLongShortMasks()
		{
			var leftLongBoard = new BitBoard();
			leftLongBoard.SetBit(Square.C8);
			leftLongBoard.SetBit(Square.D8);
			
			var rightShortBoard = new BitBoard();
			rightShortBoard.SetBit(Square.F8);
			rightShortBoard.SetBit(Square.G8);
			
			return new ulong[] {leftLongBoard.GetInnerValue(), 
					rightShortBoard.GetInnerValue()};
		}
		
		private static ulong[] GetUpperShortLongMasks()
		{
			var leftShortBoard = new BitBoard();
			leftShortBoard.SetBit(Square.B8);
			leftShortBoard.SetBit(Square.C8);
			
			var rightLongBoard = new BitBoard();
			rightLongBoard.SetBit(Square.E8);
			rightLongBoard.SetBit(Square.F8);
			
			return new ulong[] {leftShortBoard.GetInnerValue(), 
					rightLongBoard.GetInnerValue()};
		}
	}
}

