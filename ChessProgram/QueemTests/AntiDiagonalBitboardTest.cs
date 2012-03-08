using System;
using NUnit.Framework;
using QueemCore.BitBoard;
using QueemCore.BitBoard.Helpers;
using QueemCore;
using QueemCore.MovesProviders;

namespace Queem.Tests
{
	[TestFixture]
	public class BishopAntiDiagonalBitBoardTest
	{
		protected BishopAttacksGenerator provider = new BishopAttacksGenerator();
	
		[Test]
		public void TestAntiDiagonalMoves1()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			
			ulong board = BitBoardHelper.FromString(boardString);
			var coords = Square.H1;
			
			string moves = 
				"10000000" + 
				"01000000" + 
				"00100000" + 
				"00010000" + 
				"00001000" + 
				"00000100" + 
				"00000010" + 
				"00000000";
				
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var antiDiagonalAttacks = provider.AntiDiagonalAttacks(coords, board);
			Assert.AreEqual(movesBoard, antiDiagonalAttacks);
		}
		
		[Test]
		public void TestAntiDiagonalMoves2()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			
			ulong board = BitBoardHelper.FromString(boardString);
			var coords = Square.A1;
			
			string moves = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
				
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var antiDiagonalAttacks = provider.AntiDiagonalAttacks(coords, board);
			Assert.AreEqual(movesBoard, antiDiagonalAttacks);
		}
		
		[Test]
		public void TestAntiDiagonalMoves3()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			
			ulong board = BitBoardHelper.FromString(boardString);
			var coords = Square.H8;
			
			string moves = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
				
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var antiDiagonalAttacks = provider.AntiDiagonalAttacks(coords, board);
			Assert.AreEqual(movesBoard, antiDiagonalAttacks);
		}
		
		[Test]
		public void TestAntiDiagonalMoves4()
		{
			string boardString = 
				"00000011" + 
				"00000110" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"01000000" + 
				"00110000";
			
			ulong board = BitBoardHelper.FromString(boardString);
			var coords = Square.B8;
			
			string moves = 
				"00000000" + 
				"00100000" + 
				"00010000" + 
				"00001000" + 
				"00000100" + 
				"00000010" + 
				"00000001" + 
				"00000000";
				
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var antiDiagonalAttacks = provider.AntiDiagonalAttacks(coords, board);
			Assert.AreEqual(movesBoard, antiDiagonalAttacks);
		}
		
		[Test]
		public void TestAntiDiagonalMoves5()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"10000000" + 
				"01000000" + 
				"00000000" + 
				"00000000" + 
				"00001000";
			
			ulong board = BitBoardHelper.FromString(boardString);
			var coords = Square.C3;
			
			string moves = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"01000000" + 
				"00000000" + 
				"00010000" + 
				"00001000";
				
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var antiDiagonalAttacks = provider.AntiDiagonalAttacks(coords, board);
			Assert.AreEqual(movesBoard, antiDiagonalAttacks);
		}
	}
}