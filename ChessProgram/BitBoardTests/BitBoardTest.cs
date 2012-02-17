using System;
using NUnit.Framework;
using System.Linq;
using QueemCore;
using QueemCore.BitBoard.Helpers;
using QueemCore.BitBoard;

namespace BitBoardTests
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
			//TODO test if bit is set
		}
	}
}
