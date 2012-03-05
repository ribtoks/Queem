using System;
using NUnit.Framework;
using QueemCore.BitBoard.Helpers;
using QueemCore;
using QueemCore.MovesProviders;

namespace Queem.Tests
{
	[TestFixture()]
	public class KnightBitBoardTest
	{
		protected KnightMovesProvider provider = new KnightMovesProvider();
	
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
			Assert.AreEqual(attacks, provider.GetAttacks(Square.C6, 0));
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
			Assert.AreEqual(attacks, provider.GetAttacks(Square.A2, 0));
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
			Assert.AreEqual(attacks, provider.GetAttacks(Square.H8, 0));
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
			Assert.AreEqual(attacks, provider.GetAttacks(Square.C1, 0));
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
			Assert.AreEqual(attacks, provider.GetAttacks(Square.G5, 0));
		}
	}
}

