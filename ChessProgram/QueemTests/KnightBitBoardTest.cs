using System;
using NUnit.Framework;
using QueemCore.BitBoard.Helpers;
using QueemCore;

namespace Queem.Tests
{
	[TestFixture()]
	public class KnightBitBoardTest
	{
		[Test()]
		public void KnightTest1()
		{				
			string knightAttacksBoard = 
				"01010000" + 
				"10001000" + 
				"00000000" + 
				"10001000" + 
				"01010000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			ulong attacks = BitBoardHelper.FromString(knightAttacksBoard);
			Assert.AreEqual(attacks, KnightsBoardHelper.GetKnightAttacks(Square.C6));
		}
		
		[Test()]
		public void KnightTest2()
		{				
			string knightAttacksBoard = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"01000000" + 
				"00100000" + 
				"00000000" + 
				"00100000";
			ulong attacks = BitBoardHelper.FromString(knightAttacksBoard);
			Assert.AreEqual(attacks, KnightsBoardHelper.GetKnightAttacks(Square.A2));
		}
		
		[Test()]
		public void KnightTest3()
		{
			string knightAttacksBoard = 
				"00000000" + 
				"00000100" + 
				"00000010" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			ulong attacks = BitBoardHelper.FromString(knightAttacksBoard);
			Assert.AreEqual(attacks, KnightsBoardHelper.GetKnightAttacks(Square.H8));
		}
		
		[Test()]
		public void KnightTest4()
		{							
			string knightAttacksBoard = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"01010000" + 
				"10001000" + 
				"00000000";
			ulong attacks = BitBoardHelper.FromString(knightAttacksBoard);
			Assert.AreEqual(attacks, KnightsBoardHelper.GetKnightAttacks(Square.C1));
		}
		
		[Test()]
		public void KnightTest5()
		{				
			string knightAttacksBoard = 
				"00000000" + 
				"00000101" + 
				"00001000" + 
				"00000000" + 
				"00001000" + 
				"00000101" + 
				"00000000" + 
				"00000000";
			ulong attacks = BitBoardHelper.FromString(knightAttacksBoard);
			Assert.AreEqual(attacks, KnightsBoardHelper.GetKnightAttacks(Square.G5));
		}
	}
}

