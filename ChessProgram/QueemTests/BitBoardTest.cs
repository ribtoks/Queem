using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using QueemCore;
using QueemCore.BitBoard.Helpers;
using QueemCore.BitBoard;
using QueemCore.Extensions;
using System.Text;

namespace Queem.Tests
{
	[TestFixture()]
	public class BitBoardTests
	{
		protected string Get64CharsString()
		{
			var rand = new Random(DateTime.Now.Millisecond);
			int a1 = rand.Next(), a2 = rand.Next();
			return Convert.ToString(a1, 2) + Convert.ToString(a2, 2);
		}
		
		[Test()]
		public void SimpleStringTest1()
		{
			string boardString = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000100";
				
			var board = new BitBoard();
			board.ToggleBit(Square.F1);
			
			Assert.AreEqual(boardString, 
				BitBoardHelper.ToString(board.GetInnerValue(), string.Empty));
		}
		
		[Test()]
		public void SimpleStringTest2()
		{
			string boardString = 
				"00000000" + 
				"10000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
				
			var board = new BitBoard();
			board.ToggleBit(Square.A7);
			
			Assert.AreEqual(boardString, 
				BitBoardHelper.ToString(board.GetInnerValue(), string.Empty));
		}
		
		[Test()]
		public void SimpleStringTest3()
		{
			string boardString = 
				"00000000" + 
				"10000010" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"10011000";
				
			var board = new BitBoard();
			board.ToggleBit(0, 0);
			board.ToggleBit(0, 3);
			board.ToggleBit(0, 4);
			board.ToggleBit(6, 0);
			board.ToggleBit(6, 6);
			
			Assert.AreEqual(boardString, 
				BitBoardHelper.ToString(board.GetInnerValue(), string.Empty));
		}
		
		[Test()]
		public void BitsCountTest ()
		{
			int count = 100000;
			
			while (count-- != 0) {
				string binaryString = this.Get64CharsString ();
				int onesCount = binaryString.Count ((c) => c == '1');
				
				ulong val = Convert.ToUInt64 (binaryString, 2);
				BitBoard bb = new BitBoard (val);
				Assert.AreEqual (onesCount, bb.GetBitsCount ());
			}
		}
		
		[Test()]
		public void FromStringTest ()
		{
			string boardString = 
				"00000000" + 
				"00101110" + 
				"00001000" + 
				"00010000" + 
				"00010000" + 
				"01000000" + 
				"00100010" + 
				"00100010";
			
			var board = BitBoardHelper.FromString(boardString);			
			var newString = BitBoardHelper.ToString(board, string.Empty);
			
			Assert.AreEqual(boardString, newString);
		}
		
		private List<Tuple<int, int>> GetRandomBits(int count)
		{
			Random rand = new Random(DateTime.Now.Millisecond);
			var indices = new List<Tuple<int, int>>();
			while ((count--) > 0)
			{
				int x = rand.Next(8);
				int y = rand.Next(8);
				
				indices.Add(Tuple.Create(x, y));
			}
			return indices;
		}
		
		[Test()]
		public void ToStringComplexTest()
		{
			int count = 100;
			var rand = new Random(DateTime.Now.Millisecond);
						
			while ((count--) > 0)
			{
				var board = new BitBoard();
				var str = new StringBuilder(64);
				str.Append('0', 64);
				
				var indices = GetRandomBits(rand.Next(64));
				foreach (var point in indices)
				{
					board.SetBit(point.Item1, point.Item2);
					
					int index = (7 - point.Item1)*8 + point.Item2;
					str[index] = '1';
				}
				
				Assert.AreEqual(str.ToString(), 
					BitBoardHelper.ToString(board.GetInnerValue(), string.Empty));								
			}			
		}
		
		[Test()]
		public void FromStringComplexTest()
		{
			int count = 100;
			var rand = new Random(DateTime.Now.Millisecond);
						
			while ((count--) > 0)
			{
				var board = new BitBoard();
				var str = new StringBuilder(64);
				str.Append('0', 64);
				
				var indices = GetRandomBits(rand.Next(64));
				foreach (var point in indices)
				{
					board.SetBit(point.Item1, point.Item2);
					
					int index = (7 - point.Item1)*8 + point.Item2;
					str[index] = '1';
				}
								
				Assert.AreEqual(
					board.GetInnerValue(), 
					BitBoardHelper.FromString(str.ToString()));
			}
		}
		
		[Test()]
		public void TestByteMirror1()
		{
			byte b  = Convert.ToByte("00001111", 2);
			byte r  = Convert.ToByte("11110000", 2);
			Assert.AreEqual(r, BitBoardHelper.GetMirroredByte(b));
		}
		
		[Test()]
		public void TestByteMirror2()
		{
			byte b  = Convert.ToByte("00001101", 2);
			byte r  = Convert.ToByte("10110000", 2);
			Assert.AreEqual(r, BitBoardHelper.GetMirroredByte(b));
		}
		
		[Test()]
		public void TestByteMirror3()
		{
			byte b  = Convert.ToByte("01001101", 2);
			byte r  = Convert.ToByte("10110010", 2);
			Assert.AreEqual(r, BitBoardHelper.GetMirroredByte(b));
		}
		
		[Test()]
		public void TestByteMirror4()
		{
			byte b  = Convert.ToByte("10000000", 2);
			byte r  = Convert.ToByte("00000001", 2);
			Assert.AreEqual(r, BitBoardHelper.GetMirroredByte(b));
		}
		
		[Test()]
		public void TestFileToRank1()
		{
			string boardString = 
				"10000000" + 
				"10000000" + 
				"00000000" + 
				"10000000" + 
				"00000000" + 
				"00000000" + 
				"10000000" + 
				"10000000";
			byte b = Convert.ToByte("11001011", 2);
			ulong board = BitBoardHelper.FromString(boardString);
			
			Assert.AreEqual(b, BitBoardHelper.GetRankFromAFile(board));
		}
		
		[Test()]
		public void TestFileToRank2()
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
			byte b = Convert.ToByte("10000000", 2);
			ulong board = BitBoardHelper.FromString(boardString);
			
			Assert.AreEqual(b, BitBoardHelper.GetRankFromAFile(board));
		}
		
		[Test()]
		public void TestFileToRank3()
		{
			string boardString = 
				"10000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			byte b = Convert.ToByte("00000001", 2);
			ulong board = BitBoardHelper.FromString(boardString);
			
			Assert.AreEqual(b, BitBoardHelper.GetRankFromAFile(board));
		}
	}
}
