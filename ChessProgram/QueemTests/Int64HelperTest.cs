using System;
using System.Text;
using QueemCore;
using QueemCore.Extensions;
using NUnit.Framework;

namespace Queem.Tests
{
	[TestFixture]
	public class Int64HelperTest
	{
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
	}
}

