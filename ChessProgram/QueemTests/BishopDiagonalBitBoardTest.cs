using System;
using NUnit.Framework;
using Queem.Core.BitBoards;
using Queem.Core.BitBoards.Helpers;
using Queem.Core;
using Queem.Core.AttacksGenerators;

namespace Queem.Tests
{
	[TestFixture]
	public class BishopDiagonalBitBoardTest
	{
		protected BishopAttacksGenerator provider = new BishopAttacksGenerator();
	
		[Test]
		public void TestDiagonalMoves1()
		{
			var boardString = BitBoardHelper.GetEmptyBoardString();
			ulong board = BitBoardHelper.FromString(boardString);
			var coords = Square.A1;
			
			string moves = 
				"00000001" + 
				"00000010" + 
				"00000100" + 
				"00001000" + 
				"00010000" + 
				"00100000" + 
				"01000000" + 
				"00000000";
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var diagonalAttacks = provider.DiagonalAttacks(coords, board);
			Assert.AreEqual(movesBoard, diagonalAttacks);
		}
		
		[Test]
		public void TestDiagonalMoves2()
		{
			string boardString = 
				"00000001" + 
				"10000000" + 
				"00000000" + 
				"00001000" + 
				"00010000" + 
				"00100000" + 
				"10000100" + 
				"00010000";
			
			ulong board = BitBoardHelper.FromString(boardString);
			var coords = Square.A1;
			
			string moves = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00100000" + 
				"01000000" + 
				"00000000";
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var diagonalAttacks = provider.DiagonalAttacks(coords, board); 
			Assert.AreEqual(movesBoard, diagonalAttacks);
		}
		
		[Test]
		public void TestDiagonalMoves3()
		{
			string boardString = 
				"00000001" + 
				"10000000" + 
				"00000000" + 
				"00001000" + 
				"00010100" + 
				"00100000" + 
				"11100100" + 
				"00010000";
			
			ulong board = BitBoardHelper.FromString(boardString);
			var coords = Square.B1;
			
			string moves = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00100000" + 
				"00000000";
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var diagonalAttacks = provider.DiagonalAttacks(coords, board);
			Assert.AreEqual(movesBoard, diagonalAttacks);
		}
		
		[Test]
		public void TestDiagonalMoves4()
		{
			string boardString = 
				"00000000" + 
				"00001000" + 
				"00000001" + 
				"00010010" + 
				"10100100" + 
				"00000001" + 
				"00011000" + 
				"10100011";
			
			ulong board = BitBoardHelper.FromString(boardString);
			var coords = Square.E3;
			
			string moves = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000100" + 
				"00000000" + 
				"00010000" + 
				"00000000";
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var diagonalAttacks = provider.DiagonalAttacks(coords, board);
			Assert.AreEqual(movesBoard, diagonalAttacks);
		}
		
		[Test]
		public void TestDiagonalMoves5()
		{
			string boardString = 
				"01000000" + 
				"00000000" + 
				"00000010" + 
				"01111110" + 
				"00000010" + 
				"00000010" + 
				"00000010" + 
				"00000000";
			
			ulong board = BitBoardHelper.FromString(boardString);
			var coords = Square.A7;
			
			string moves = 
				"01000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var diagonalAttacks = provider.DiagonalAttacks(coords, board);
			Assert.AreEqual(movesBoard, diagonalAttacks);
		}
		
		[Test]
		public void TestDiagonalMoves6()
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
			var coords = Square.A8;
			
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
			var diagonalAttacks = provider.DiagonalAttacks(coords, board);
			Assert.AreEqual(movesBoard, diagonalAttacks);
		}
		
		[Test]
		public void TestDiagonalMoves7()
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
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var diagonalAttacks = provider.DiagonalAttacks(coords, board);
			Assert.AreEqual(movesBoard, diagonalAttacks);
		}
		
		[Test]
		public void TestDiagonalMoves8()
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
			var coords = Square.H7;
			
			string moves = 
				"00000000" + 
				"00000000" + 
				"00000010" + 
				"00000100" + 
				"00001000" + 
				"00010000" + 
				"00100000" + 
				"01000000";
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var diagonalAttacks = provider.DiagonalAttacks(coords, board);
			Assert.AreEqual(movesBoard, diagonalAttacks);
		}
		
		[Test]
		public void TestDiagonalMoves9()
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
			var coords = Square.F6;
			
			string moves = 
				"00000001" + 
				"00000010" + 
				"00000000" + 
				"00001000" + 
				"00010000" + 
				"00100000" + 
				"01000000" + 
				"10000000";
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var diagonalAttacks = provider.DiagonalAttacks(coords, board);
			Assert.AreEqual(movesBoard, diagonalAttacks);
		}
	}
}
