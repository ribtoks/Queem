using System;
using NUnit.Framework;
using QueemCore.BitBoards.Helpers;
using QueemCore;
using QueemCore.BitBoards;

namespace Queem.Tests
{
	[TestFixture]
	public class PawnTests
	{
		[Test]
		public void TestDoubleUpPush()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"10000010" + 
				"00101000" + 
				"00000000";
			
			var board = new PawnBitBoard(BitBoardHelper.FromString(boardString));
			
			string moves = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00101000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			
			ulong movesBoard = BitBoardHelper.FromString(moves);
			ulong emptySquares = 0;
			emptySquares = ~emptySquares;
			
			var attacks = board.DoubleUpPushTargets(emptySquares);
			Assert.AreEqual(movesBoard, attacks);
		}
		
		[Test]
		public void TestDownLeftAttacks()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"10000010" + 
				"00101000" + 
				"00000000";
			
			var board = new PawnBitBoard(BitBoardHelper.FromString(boardString));
			
			string moves = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000100" + 
				"01010000";
				
			ulong movesBoard = BitBoardHelper.FromString(moves);
			ulong emptySquares = 0;
			emptySquares = ~emptySquares;
			
			var attacks = board.SouthWestAttacks();
			Assert.AreEqual(movesBoard, attacks);
		}
		
		[Test]
		public void TestUpRightAttacks()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"10000010" + 
				"00101000" + 
				"00000000";
			
			var board = new PawnBitBoard(BitBoardHelper.FromString(boardString));
			
			string moves = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"01000001" + 
				"00010100" + 
				"00000000" + 
				"00000000";
				
			ulong movesBoard = BitBoardHelper.FromString(moves);
			ulong emptySquares = 0;
			emptySquares = ~emptySquares;
			
			var attacks = board.NorthEastAttacks();
			Assert.AreEqual(movesBoard, attacks);
		}
	}
}