using System;
using NUnit.Framework;
using System.Linq;
using QueemCore;

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
			
			while (count-- != 0)
			{
				string binaryString = this.Get64CharsString();
				int onesCount = binaryString.Count( (c) => c == '1' );
				
				ulong val = Convert.ToUInt64(binaryString, 2);
				BitBoard bb = new BitBoard(val);
				Assert.AreEqual(onesCount, bb.GetBitsCount());
			}
		}
	}
}
