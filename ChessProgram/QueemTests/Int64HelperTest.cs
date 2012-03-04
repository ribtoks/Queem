using System;
using System.Text;
using QueemCore;
using QueemCore.Extensions;
using NUnit.Framework;
using QueemCore.BitBoard.Helpers;

namespace Queem.Tests
{
	[TestFixture]
	public class Int64HelperTest
	{
		protected string Get64CharsString()
		{
			var rand = new Random(DateTime.Now.Millisecond);
			int a1 = rand.Next(), a2 = rand.Next();
			return Convert.ToString(a1, 2).LJust(32, '0') + Convert.ToString(a2, 2).LJust(32, '0');
		}
				
		[Test]
		public void TestReverseByte()
		{						
			for (int i = 0; i < 256; ++i)
			{					
				byte b = (byte)i;
				byte b_reversed = Convert.ToByte(Convert.ToString(b, 2).LJust(8, '0').MyReverse(), 2);
					
				Assert.AreEqual(b_reversed, Int64Helper.ReverseByte(b));
			}
		}
		
		[Test]
		public void TestReverseUShort()
		{
			for (int i = 0; i <= ushort.MaxValue; ++i)
			{					
				ushort s = (ushort)i;
				ushort s_reversed = Convert.ToUInt16(Convert.ToString(s, 2).LJust(16, '0').MyReverse(), 2);
					
				Assert.AreEqual(s_reversed, Int64Helper.GetUshortReversed(s));
			}
		}
		
		[Test]
		public void TestReverseUlong()
		{
			int count = 10000;
			while ((count--) > 0)
			{
				string str = Get64CharsString();
				ulong board = BitBoardHelper.FromString(str);
				ulong reversed = Int64Helper.GetReversedUlong(board);
				ulong rereversed = Int64Helper.GetReversedUlong(reversed);
		
				Assert.AreEqual(str.MyReverse(), BitBoardHelper.ToString(reversed, string.Empty).LJust(64, '0'));
				Assert.AreEqual(board, rereversed);
			}
		}
	}
}

