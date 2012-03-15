using System;
using NUnit.Framework;
using QueemCore;
using QueemCore.BitBoards.Helpers;

namespace Queem.Tests
{
	[TestFixture]
	public class PawnMovesGeneratorTests
	{		
		private bool AreMovesEqual(Move[] moves1, Move[] moves2)
		{
			if (moves1.Length != moves2.Length)
				return false;
			
			for (int i = 0; i < moves1.Length; ++i)
			{
				if ((moves1[i].From != moves2[i].From) ||
					(moves1[i].To != moves2[i].To))
					return false;
			}
			
			return true;
		}
		
		[Test]
		public void TestSingleUpPushes1()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"01011100" + 
				"00000000" + 
				"00000000";
			//	 abcdefgh
			int rankIndex = 2;
			ulong board = BitBoardHelper.FromString(boardString);
			byte rank = (byte)(board >> (rankIndex * 8) & 0xff);
			var realMoves = new Move[] {
				new Move(Square.B2, Square.B3),
				new Move(Square.D2, Square.D3),
				new Move(Square.E2, Square.E3),
				new Move(Square.F2, Square.F3)};
				
			var moves = PawnBitBoardHelper.QuietMoves[0][rankIndex][rank];
			Assert.IsTrue(this.AreMovesEqual(realMoves, moves));
		}
		
		[Test]
		public void TestSingleUpPushes2()
		{
			string boardString = 
				"01000001" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			//	 abcdefgh
			int rankIndex = 7;
			ulong board = BitBoardHelper.FromString(boardString);
			byte rank = (byte)(board >> (rankIndex * 8) & 0xff);
			var realMoves = new Move[] {
				new Move(Square.B7, Square.B8),
				new Move(Square.H7, Square.H8)};
				
			var moves = PawnBitBoardHelper.QuietMoves[0][rankIndex][rank];
			Assert.IsTrue(this.AreMovesEqual(realMoves, moves));
		}
		
		[Test]
		public void TestSingleUpLeftAttacks1()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"01011100" + 
				"00000000" + 
				"00000000";
			//	 abcdefgh
			int rankIndex = 2;
			ulong board = BitBoardHelper.FromString(boardString);
			byte rank = (byte)(board >> (rankIndex * 8) & 0xff);
			var realMoves = new Move[] {
				new Move(Square.C2, Square.B3),
				new Move(Square.E2, Square.D3),
				new Move(Square.F2, Square.E3),
				new Move(Square.G2, Square.F3)};
				
			var moves = PawnBitBoardHelper.AttacksLeftMoves[0][rankIndex][rank];
			Assert.IsTrue(this.AreMovesEqual(realMoves, moves));
		}
		
		[Test]
		public void TestSingleUpRightAttacks1()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"01011100" + 
				"00000000" + 
				"00000000";
			//	 abcdefgh
			int rankIndex = 2;
			ulong board = BitBoardHelper.FromString(boardString);
			byte rank = (byte)(board >> (rankIndex * 8) & 0xff);
			var realMoves = new Move[] {
				new Move(Square.A2, Square.B3),
				new Move(Square.C2, Square.D3),
				new Move(Square.D2, Square.E3),
				new Move(Square.E2, Square.F3)};
				
			var moves = PawnBitBoardHelper.AttacksRightMoves[0][rankIndex][rank];
			Assert.IsTrue(this.AreMovesEqual(realMoves, moves));
		}
		
		[Test]
		public void TestSingleUpLeftAttacks2()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000001" + 
				"00000000" + 
				"00000000";
			//	 abcdefgh
			int rankIndex = 2;
			ulong board = BitBoardHelper.FromString(boardString);
			byte rank = (byte)(board >> (rankIndex * 8) & 0xff);
				
			var moves = PawnBitBoardHelper.AttacksLeftMoves[0][rankIndex][rank];
			Assert.IsTrue(moves.Length == 0);
		}
		
		[Test]
		public void TestSingleUpRightAttacks2()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"10000000";
			//	 abcdefgh
			int rankIndex = 0;
			ulong board = BitBoardHelper.FromString(boardString);
			byte rank = (byte)(board >> (rankIndex * 8) & 0xff);
				
			var moves = PawnBitBoardHelper.AttacksRightMoves[0][rankIndex][rank];
			Assert.IsTrue(moves.Length == 0);
		}
		
		[Test]
		public void TestSingleDownPushes1()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"01011100" + 
				"00000000" + 
				"00000000";
			//	 abcdefgh
			int rankIndex = 2;
			ulong board = BitBoardHelper.FromString(boardString);
			byte rank = (byte)(board >> (rankIndex * 8) & 0xff);
			var realMoves = new Move[] {
				new Move(Square.B4, Square.B3),
				new Move(Square.D4, Square.D3),
				new Move(Square.E4, Square.E3),
				new Move(Square.F4, Square.F3)};
				
			var moves = PawnBitBoardHelper.QuietMoves[1][rankIndex][rank];
			Assert.IsTrue(this.AreMovesEqual(realMoves, moves));
		}
		
		[Test]
		public void TestSingleDownPushes2()
		{
			string boardString = 
				"01000001" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			//	 abcdefgh
			int rankIndex = 7;
			ulong board = BitBoardHelper.FromString(boardString);
			byte rank = (byte)(board >> (rankIndex * 8) & 0xff);
							
			var moves = PawnBitBoardHelper.QuietMoves[1][rankIndex][rank];
			Assert.IsTrue(moves.Length == 0);
		}
		
		[Test]
		public void TestSingleDownLeftAttacks1()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"01011100" + 
				"00000000" + 
				"00000000";
			//	 abcdefgh
			int rankIndex = 2;
			ulong board = BitBoardHelper.FromString(boardString);
			byte rank = (byte)(board >> (rankIndex * 8) & 0xff);
			var realMoves = new Move[] {
				new Move(Square.C4, Square.B3),
				new Move(Square.E4, Square.D3),
				new Move(Square.F4, Square.E3),
				new Move(Square.G4, Square.F3)};
				
			var moves = PawnBitBoardHelper.AttacksLeftMoves[1][rankIndex][rank];
			Assert.IsTrue(this.AreMovesEqual(realMoves, moves));
		}
		
		[Test]
		public void TestSingleDownRightAttacks1()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"01011100" + 
				"00000000" + 
				"00000000";
			//	 abcdefgh
			int rankIndex = 2;
			ulong board = BitBoardHelper.FromString(boardString);
			byte rank = (byte)(board >> (rankIndex * 8) & 0xff);
			var realMoves = new Move[] {
				new Move(Square.A4, Square.B3),
				new Move(Square.C4, Square.D3),
				new Move(Square.D4, Square.E3),
				new Move(Square.E4, Square.F3)};
				
			var moves = PawnBitBoardHelper.AttacksRightMoves[1][rankIndex][rank];
			Assert.IsTrue(this.AreMovesEqual(realMoves, moves));
		}
		
		[Test]
		public void TestDoubleUpPushes()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"01011100" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			//	 abcdefgh
			int rankIndex = 3;
			ulong board = BitBoardHelper.FromString(boardString);
			byte rank = (byte)(board >> (rankIndex * 8) & 0xff);
			var realMoves = new Move[] {
				new Move(Square.B2, Square.B4),
				new Move(Square.D2, Square.D4),
				new Move(Square.E2, Square.E4),
				new Move(Square.F2, Square.F4)};
				
			var moves = PawnBitBoardHelper.DoublePushes[0][rankIndex][rank];
			Assert.IsTrue(this.AreMovesEqual(realMoves, moves));
		}
	}
}

