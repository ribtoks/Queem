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
		
		public static ulong GetReversedUlong(ulong u)
		{
			ushort bytes3 = (ushort)((u >> (16*3)) & 0xffff);
			ushort bytes2 = (ushort)((u >> (16*2)) & 0xffff);
			ushort bytes1 = (ushort)((u >> 16) & 0xffff);
			ushort bytes0 = (ushort)(u & 0xffff);
			
			ulong reversed3 = ReversedUShorts[bytes3];
			ulong reversed2 = ReversedUShorts[bytes2];
			ulong reversed1 = ReversedUShorts[bytes1];
			ulong reversed0 = ReversedUShorts[bytes0];
			
			return (reversed0 << 16*3) | (reversed1 << 16*2) | (reversed2 << 16) | reversed3;
		}
	}
}

