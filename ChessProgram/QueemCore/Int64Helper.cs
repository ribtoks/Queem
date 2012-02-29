using System;

namespace QueemCore
{
	public static class Int64Helper
	{
		public static readonly ushort[] ReversedUShorts;
		
		static Int64Helper()
		{
			ReversedUShorts = GenerateReversedUShorts();
		}
		
		public static ushort[] GenerateReversedUShorts()
		{
			var arr = new ushort[ushort.MaxValue + 1];
			for (int i = 0; i < arr.Length; ++i)
				arr[i] = GetUshortReversed((ushort)i);
			return arr;
		}
		
		public static ushort GetUshortReversed(ushort input)
		{
			int lower_byte = input & 0xff;
			int upper_byte = (input >> 8) & 0xff;
			
			byte lower_reversed = ReverseByte((byte)lower_byte);
			byte upper_reversed = ReverseByte((byte)upper_byte);
			
			return (ushort)((lower_reversed << 8) | upper_reversed);
		}
		
		public static byte ReverseByte(byte b)
		{
			return (byte)((b * 0x0202020202UL & 0x010884422010UL) % 1023);
		}
	}
}

