using System;
using System.Linq;

namespace KnightMovesGenerator
{
	class MainClass
	{
		public static ulong notAFile = 0xfefefefefefefefeUL;
		public static ulong notHFile = 0x7f7f7f7f7f7f7f7fUL;
		public static ulong notABFile = 0xfcfcfcfcfcfcfcfcUL;
		public static ulong notGHFile = 0x3f3f3f3f3f3f3f3fUL;
		
		public static ulong noNoEa(ulong b) {return (b << 17) & notAFile ;}
		public static ulong noEaEa(ulong b) {return (b << 10) & notABFile;}
		public static ulong soEaEa(ulong b) {return (b >>  6) & notABFile;}
		public static ulong soSoEa(ulong b) {return (b >> 15) & notAFile ;}
		public static ulong noNoWe(ulong b) {return (b << 15) & notHFile ;}
		public static ulong noWeWe(ulong b) {return (b <<  6) & notGHFile;}
		public static ulong soWeWe(ulong b) {return (b >> 10) & notGHFile;}
		public static ulong soSoWe(ulong b) {return (b >> 17) & notHFile ;}
		 
		
		public static ulong GetKnightMoves(int i, int j)
		{
			/*
			 *   noNoWe    noNoEa
		            +15  +17
		             |     |
		noWeWe  +6 __|     |__+10  noEaEa
		              \   /
		               >0<
		           __ /   \ __
		soWeWe -10   |     |   -6  soEaEa
		             |     |
		            -17  -15
		        soSoWe    soSoEa
			 */
			int index = i*8+j;
			ulong initialPos = 1UL << index;
			
			ulong board = 0;
			
			board |= noNoWe(initialPos);
			board |= noNoEa(initialPos);
			board |= noWeWe(initialPos);
			board |= noEaEa(initialPos);
			board |= soWeWe(initialPos);
			board |= soEaEa(initialPos);
			board |= soSoWe(initialPos);
			board |= soSoEa(initialPos);
			
			return board;
		}
		
		public static string Get8LengthString(string str)
		{
			if (str.Length < 8)
				return (new string('0', 8 - str.Length)) + str;
			else
				return str;
		}
		
		public static string ReverseString(string str)
		{
			var arr = str.ToCharArray();
			Array.Reverse(arr);
			return new string(arr);
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
			ulong[] knightMoves = new ulong[64];
			
			
			for (int i = 0; i < 8; i++) 
			{
				for (int j = 0; j < 8; ++j)
				{
					knightMoves[i*8 + j] = GetKnightMoves(i, j);
				}
			}
			
			Console.WriteLine (string.Join(", ", 
			                               knightMoves.Select(u => "0x" + u.ToString("X") + "UL").ToArray())
			                   );
			
			//Console.WriteLine (GetBoardString(GetKnightMoves(1, 1)));
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
