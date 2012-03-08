using System;
using NUnit.Framework;
using QueemCore.BitBoard;
using QueemCore.BitBoard.Helpers;
using QueemCore;
using QueemCore.MovesProviders;

namespace Queem.Tests
{
	[TestFixture]
	public class QueenTests
	{
		protected QueenAttacksGenerator provider = new QueenAttacksGenerator();
	
		[Test]
		public void AttacksTest1()
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
				"10000001" + 
				"01000001" + 
				"00100001" + 
				"00010001" + 
				"00001001" + 
				"00000101" + 
				"00000011" + 
				"11111110";
			
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var attacks = provider.GetAttacks(coords, board);
			Assert.AreEqual(movesBoard, attacks);
		}
		
		[Test]
		public void AttacksTest2()
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
				"10000001" + 
				"10000010" + 
				"10000100" + 
				"10001000" + 
				"10010000" + 
				"10100000" + 
				"11000000" + 
				"01111111";
			
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var attacks = provider.GetAttacks(coords, board);		
			Assert.AreEqual(movesBoard, attacks);
		}
		
		[Test]
		public void AttacksTest3()
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
			var coords = Square.C4;
			
			string moves = 
				"00100010" + 
				"00100100" + 
				"10101000" + 
				"01110000" + 
				"11011111" + 
				"01110000" + 
				"10101000" + 
				"00100100";
			
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var attacks = provider.GetAttacks(coords, board);			
			Assert.AreEqual(movesBoard, attacks);
		}
		
		[Test]
		public void AttacksTest4()
		{
			string boardString = 
				"00000000" + 
				"01100100" + 
				"00001000" + 
				"00000000" + 
				"00000110" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			
			ulong board = BitBoardHelper.FromString(boardString);
			var coords = Square.C4;
			
			string moves = 
				"00000000" + 
				"00100000" + 
				"10101000" + 
				"01110000" + 
				"11011100" + 
				"01110000" + 
				"10101000" + 
				"00100100";
			
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var attacks = provider.GetAttacks(coords, board);			
			Assert.AreEqual(movesBoard, attacks);
		}
		
		[Test]
		public void AttacksTest5()
		{
			string boardString = 
				"00000000" + 
				"11100000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00100001" + 
				"01011001" + 
				"10111000";
			
			ulong board = BitBoardHelper.FromString(boardString);
			var coords = Square.H8;
			
			string moves = 
				"11111110" + 
				"00000011" + 
				"00000101" + 
				"00001001" + 
				"00010001" + 
				"00100001" + 
				"00000000" + 
				"00000000";
			
			ulong movesBoard = BitBoardHelper.FromString(moves);
			var attacks = provider.GetAttacks(coords, board);			
			Assert.AreEqual(movesBoard, attacks);
		}
	}
}
