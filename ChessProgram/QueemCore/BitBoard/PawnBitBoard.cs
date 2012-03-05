using System;
using QueemCore.BitBoard.Helpers;
using System.Collections.Generic;
using QueemCore.MovesProviders;

namespace QueemCore.BitBoard
{
	public class PawnBitBoard : BitBoard
	{
		public PawnBitBoard (MovesProvider provider)
			:base(provider)
		{
		}
		
		public PawnBitBoard(ulong val, MovesProvider provider)
			:base(val, provider)
		{
		}
		
		public ulong SingleUpPushTargets(ulong emptySquares)
		{
			return BitBoardHelper.ShiftNorthOne(this.board) & emptySquares;
		}
		
		public ulong SingleDownPushTargets(ulong emptySquares)
		{
			return BitBoardHelper.ShiftSouthOne(this.board) & emptySquares;
		}
		
		public ulong DoubleUpPushTargets(ulong emptySquares)
		{
			ulong rank4 = 0x00000000FF000000UL;
   			ulong singlePushs = this.SingleUpPushTargets(emptySquares);
   			return BitBoardHelper.ShiftNorthOne(singlePushs) & emptySquares & rank4;
		}
		
		public ulong DoubleDownPushTargets(ulong emptySquares)
		{
			ulong rank5 = 0x000000FF00000000UL;
   			ulong singlePushs = this.SingleDownPushTargets(emptySquares);
   			return BitBoardHelper.ShiftSouthOne(singlePushs) & emptySquares & rank5;
		}
		
		public ulong PawnsAbleToPushUp(ulong emptySquares)
		{
			return BitBoardHelper.ShiftSouthOne(emptySquares) & this.board;
		}
		
		public ulong PawnsAbleToDoublePushUp(ulong emptySquares)
		{
			ulong rank4 = 0x00000000FF000000UL;
   			ulong emptyRank3 = BitBoardHelper.ShiftSouthOne(emptySquares & rank4) & emptySquares;
   			return this.PawnsAbleToPushUp(emptyRank3);
		}
		
		public ulong PawnsAbleToPushDown(ulong emptySquares)
		{
			return BitBoardHelper.ShiftNorthOne(emptySquares) & this.board;
		}
		
		public ulong PawnsAbleToDoublePushDown(ulong emptySquares)
		{
			ulong rank5 = 0x000000FF00000000UL;
   			ulong emptyRank4 = BitBoardHelper.ShiftNorthOne(emptySquares & rank5) & emptySquares;
   			return this.PawnsAbleToPushUp(emptyRank4);
		}
		
		public ulong NorthEastAttacks() { return BitBoardHelper.ShiftNorthEastOne(this.board); }
		public ulong SouthEastAttacks() { return BitBoardHelper.ShiftSouthEastOne(this.board); }
		public ulong NorthWestAttacks() { return BitBoardHelper.ShiftNorhtWestOne(this.board); }
		public ulong SouthWestAttacks() { return BitBoardHelper.ShiftSouthWestOne(this.board); }
		
		public ulong AnyUpAttacks() 
		{
			return (this.NorthEastAttacks() | this.NorthWestAttacks());
		}
		
		public ulong UpDoubleAttacks()
		{
			return (this.NorthEastAttacks() & this.NorthWestAttacks());
		}
		
		public ulong UpSingleAttacks()
		{
			return (this.NorthEastAttacks() ^ this.NorthWestAttacks());
		}
		
		public ulong AnyDownAttacks() 
		{
			return (this.SouthEastAttacks() | this.SouthWestAttacks());
		}
		
		public ulong DownDoubleAttacks()
		{
			return (this.SouthEastAttacks() & this.SouthWestAttacks());
		}
		
		public ulong DownSingleAttacks()
		{
			return (this.SouthEastAttacks() ^ this.SouthWestAttacks());
		}
		
		// all pawns that are blocked by the opponent's pawns
		public ulong UpRam(PawnBitBoard opponentPawns)
		{
			return BitBoardHelper.ShiftSouthOne(opponentPawns.board) & this.board;
		}
		
		// all pawns that are blocked by the opponent's pawns
		public ulong DownRam(PawnBitBoard opponentPawns)
		{
			return BitBoardHelper.ShiftNorthOne(this.board) & opponentPawns.board;
		}
		
		public ulong PawnsWithEastNeighbour()
		{
			return (this.board & BitBoardHelper.ShiftWestOne(this.board));
		}
		
		public ulong PawnsWithWestNeighbour()
		{
			return this.PawnsWithEastNeighbour() << 1;
		}
		
		public ulong DefendedUpFromWest ()
		{
			return this.board & this.NorthEastAttacks ();
		}
		
		public ulong DefendedUpFromEast ()
		{
			return this.board & this.NorthWestAttacks ();
		}
				
		public ulong DefendedDownFromEast ()
		{
			return this.board & this.SouthWestAttacks ();
		}
		
		public ulong DefendedDownFromWest ()
		{
			return this.board & this.SouthEastAttacks ();
		}
		
		public override IEnumerable<ulong> GetAttacks (ulong otherFigures)
		{
			throw new NotImplementedException ();
		}
	}
}

