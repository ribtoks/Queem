using System;
using System.Linq;

namespace KingMovesGenerator
{
	public enum Square
	{		
		A1=0, B1, C1, D1, E1, F1, G1, H1,
	   	A2, B2, C2, D2, E2, F2, G2, H2,
	   	A3, B3, C3, D3, E3, F3, G3, H3,
	   	A4, B4, C4, D4, E4, F4, G4, H4,
	   	A5, B5, C5, D5, E5, F5, G5, H5,
	   	A6, B6, C6, D6, E6, F6, G6, H6,
	   	A7, B7, C7, D7, E7, F7, G7, H7,
	   	A8, B8, C8, D8, E8, F8, G8, H8,
	   	NoSquare
	}
	
	class MainClass
	{
		public static readonly ulong NotAFile = 0xfefefefefefefefeUL;
		public static readonly ulong NotHFile = 0x7f7f7f7f7f7f7f7fUL;
		public static readonly ulong NotABFile = 0xfcfcfcfcfcfcfcfcUL;
		public static readonly ulong NotGHFile = 0x3f3f3f3f3f3f3f3fUL;
		
		public static ulong ShiftNorth(ulong b) { return b << 8; }
		public static ulong ShiftSouth(ulong b) { return b >> 8; }
		public static ulong ShiftEastOne (ulong b) {return (b & NotHFile) << 1;}
		public static ulong ShiftNorthEastOne (ulong b) {return (b & NotHFile) << 9;}
		public static ulong ShiftSouthEastOne (ulong b) {return (b & NotHFile) >> 7;}
		public static ulong ShiftWestOne (ulong b) {return (b & NotAFile) >> 1;}
		public static ulong ShiftSouthWestOne (ulong b) {return (b & NotAFile) >> 9;}
		public static ulong ShiftNorhtWestOne (ulong b) {return (b & NotAFile) << 7;}
		
		public static ulong GetKingMoves(int i, int j)
		{
			int index = i*8+j;
			ulong initialPos = 1UL << index;
			
			ulong board = initialPos;
			
			ulong attacks = ShiftEastOne(board) | ShiftWestOne(board);
			board = initialPos | attacks;
			attacks |= ShiftNorth(board) | ShiftSouth(board);
			return attacks;
		}
		
		public static string GetBoardString(ulong board)
		{
			return string.Join("\n", BitConverter.GetBytes(board)
						.Reverse()
						.Select(b => 
					        new string(Convert.ToString(b, 2).LJust(8, '0').Reverse().ToArray())).ToArray());
			/*
			 for (int i = 0; i < 8; ++i)
				str = str.Insert(i+i*8, "\n");
			
			*/
		}
		
		public static ulong GetOneBitNumber(int rank, int file)
		{
			int squareIndex = rank*8 + file;
			return 1UL << squareIndex;
		}
		
		public static void Main (string[] args)
		{
			ulong[] kingMoves = new ulong[64];
			
			
			for (int i = 0; i < 8; i++) 
			{
				for (int j = 0; j < 8; ++j)
				{
					kingMoves[i*8 + j] = GetKingMoves(i, j);
				}
			}
			
			//Console.WriteLine (GetBoardString(kingMoves[(int)Square.H1]));
			
			Console.WriteLine (string.Join(", ", 
			                               kingMoves.Select(u => "0x" + u.ToString("X") + "UL").ToArray())
			                   );
		}
	}
	
	public static class MyStringExtensions
	{
		public static string LJust(this string str, int length, char ch)
		{
			if (str.Length < length)
				return (new string(ch, length - str.Length)) + str;
			return str;
		}
	}
}
