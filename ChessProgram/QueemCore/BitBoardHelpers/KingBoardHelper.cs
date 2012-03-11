using System;

namespace QueemCore.BitBoards.Helpers
{
	public static class KingBoardHelper
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
		
		public static ulong KingAttacks(Square square)
		{
			return KingMoves[(int)square];
		}
		
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
	}
}

