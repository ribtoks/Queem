using System;
using NUnit.Framework;
using QueemCore.BitBoard;
using QueemCore.BitBoard.Helpers;
using QueemCore;

namespace Queem.Tests
{
	[TestFixture()]
	public class RookBitBoardTest
	{		
		[Test()]
		public void ClearBytePosition1()
		{		
			byte rookPos = Convert.ToByte("00001000", 2);
			byte otherFigures = Convert.ToByte("00000000", 2);
			byte result = Convert.ToByte("11110111", 2);
			
			Assert.AreEqual(result, RookBitBoardHelper.GetByteRankMovesMask(rookPos, otherFigures));
		}
		
		[Test()]
		public void ClearBytePosition2()
		{		
			byte rookPos = Convert.ToByte("00000001", 2);
			byte otherFigures = Convert.ToByte("00000000", 2);
			byte result = Convert.ToByte("11111110", 2);
			
			Assert.AreEqual(result, RookBitBoardHelper.GetByteRankMovesMask(rookPos, otherFigures));
		}
		
		[Test()]
		public void ClearBytePosition3()
		{		
			byte rookPos = Convert.ToByte("10000000", 2);
			byte otherFigures = Convert.ToByte("00000000", 2);
			byte result = Convert.ToByte("01111111", 2);
			
			Assert.AreEqual(result, RookBitBoardHelper.GetByteRankMovesMask(rookPos, otherFigures));
		}
		
		[Test()]
		public void BytePosition1()
		{		
			byte rookPos = Convert.ToByte("00001000", 2);
			byte otherFigures = Convert.ToByte("10000001", 2);
			byte result = Convert.ToByte("11110111", 2);
			
			Assert.AreEqual(result, RookBitBoardHelper.GetByteRankMovesMask(rookPos, otherFigures));
		}
		
		[Test()]
		public void BytePosition2()
		{		
			byte rookPos = Convert.ToByte("00001000", 2);
			byte otherFigures = Convert.ToByte("01000010", 2);
			byte result = Convert.ToByte("01110110", 2);
			
			Assert.AreEqual(result, RookBitBoardHelper.GetByteRankMovesMask(rookPos, otherFigures));
		}
		
		[Test()]
		public void BytePosition3()
		{		
			byte rookPos = Convert.ToByte("00001000", 2);
			byte otherFigures = Convert.ToByte("00010100", 2);
			byte result = Convert.ToByte("00010100", 2);
			
			Assert.AreEqual(result, RookBitBoardHelper.GetByteRankMovesMask(rookPos, otherFigures));
		}
		
		[Test()]
		public void BytePosition4()
		{		
			byte rookPos = Convert.ToByte("01000000", 2);
			byte otherFigures = Convert.ToByte("00010100", 2);
			byte result = Convert.ToByte("10110000", 2);
			
			Assert.AreEqual(result, RookBitBoardHelper.GetByteRankMovesMask(rookPos, otherFigures));
		}
		
		[Test()]
		public void BytePosition5()
		{		
			byte rookPos = Convert.ToByte("00000010", 2);
			byte otherFigures = Convert.ToByte("00010100", 2);
			byte result = Convert.ToByte("00000101", 2);
			
			Assert.AreEqual(result, RookBitBoardHelper.GetByteRankMovesMask(rookPos, otherFigures));
		}
		
		[Test()]
		public void BytePosition6()
		{		
			byte rookPos = Convert.ToByte("10000000", 2);
			byte otherFigures = Convert.ToByte("00010100", 2);
			byte result = Convert.ToByte("01110000", 2);
			
			Assert.AreEqual(result, RookBitBoardHelper.GetByteRankMovesMask(rookPos, otherFigures));
		}
		
		[Test()]
		public void BytePosition7()
		{		
			byte rookPos = Convert.ToByte("00000001", 2);
			byte otherFigures = Convert.ToByte("00010100", 2);
			byte result = Convert.ToByte("00000110", 2);
			
			Assert.AreEqual(result, RookBitBoardHelper.GetByteRankMovesMask(rookPos, otherFigures));
		}
		
		[Test()]
		public void ClearPosition1()
		{
			string rookBoard = 
				"00000000" + 
				"00000000" + 
				"00100000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			
			File file = File.C;
			int rank = 5;
			
			string otherFiguresBoard = BitBoardHelper.GetEmptyBoardString();
			
			string resultBoard = 
				"00100000" + 
				"00100000" + 
				"11011111" + 
				"00100000" + 
				"00100000" + 
				"00100000" + 
				"00100000" + 
				"00100000";
				
			ulong otherFigures = BitBoardHelper.FromString(otherFiguresBoard);
			ulong result = BitBoardHelper.FromString(resultBoard);
			
			Assert.AreEqual(result, 
				RookBitBoardHelper.GetRookAttacks(rank, file, otherFigures));
		}
		
		[Test()]
		public void ClearPosition2()
		{
			string rookBoard = 
				"10000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			
			File file = File.A;
			int rank = 7;
			
			string otherFiguresBoard = BitBoardHelper.GetEmptyBoardString();
			
			string resultBoard = 
				"01111111" + 
				"10000000" + 
				"10000000" + 
				"10000000" + 
				"10000000" + 
				"10000000" + 
				"10000000" + 
				"10000000";
						
			ulong otherFigures = BitBoardHelper.FromString(otherFiguresBoard);
			ulong result = BitBoardHelper.FromString(resultBoard);
			
			Assert.AreEqual(result, 
				RookBitBoardHelper.GetRookAttacks(rank, file, otherFigures));
		}
		
		[Test()]
		public void ClearPosition3()
		{
			string rookBoard = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000001" + 
				"00000000" + 
				"00000000";
			
			File file = File.H;
			int rank = 2;
			
			string otherFiguresBoard = BitBoardHelper.GetEmptyBoardString();
			
			string resultBoard = 
				"00000001" + 
				"00000001" + 
				"00000001" + 
				"00000001" + 
				"00000001" + 
				"11111110" + 
				"00000001" + 
				"00000001";

			ulong otherFigures = BitBoardHelper.FromString(otherFiguresBoard);
			ulong result = BitBoardHelper.FromString(resultBoard);
			
			Assert.AreEqual(result, 
				RookBitBoardHelper.GetRookAttacks(rank, file, otherFigures));
		}
		
		[Test()]
		public void RookPosition1()
		{
			string rookBoard = 
				"00000000" + 
				"00000000" + 
				"00100000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			
			File file = File.C;
			int rank = 5;
			
			string otherFiguresBoard = 
				"00000000" + 
				"00100000" + 
				"00000100" + 
				"00100000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			
			string resultBoard = 
				"00000000" + 
				"00100000" + 
				"11011100" + 
				"00100000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
				
			ulong otherFigures = BitBoardHelper.FromString(otherFiguresBoard);
			ulong result = BitBoardHelper.FromString(resultBoard);
			
			Assert.AreEqual(result, 
				RookBitBoardHelper.GetRookAttacks(rank, file, otherFigures));
		}
		
		[Test()]
		public void RookPosition2()
		{
			string rookBoard = 
				"00000000" + 
				"00000000" + 
				"00100000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			
			File file = File.C;
			int rank = 5;
			
			string otherFiguresBoard = 
				"01100000" + 
				"00000100" + 
				"10000001" + 
				"00000000" + 
				"00000000" + 
				"01000100" + 
				"00000000" + 
				"00100000";
			
			string resultBoard = 
				"00100000" + 
				"00100000" + 
				"11011111" + 
				"00100000" + 
				"00100000" + 
				"00100000" + 
				"00100000" + 
				"00100000";
								
			ulong otherFigures = BitBoardHelper.FromString(otherFiguresBoard);
			ulong result = BitBoardHelper.FromString(resultBoard);
			
			Assert.AreEqual(result, 
				RookBitBoardHelper.GetRookAttacks(rank, file, otherFigures));
		}
		
		[Test()]
		public void RookPosition3()
		{
			string rookBoard = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"10000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			
			File file = File.A;
			int rank = 4;
			
			string otherFiguresBoard = 
				"00100000" + 
				"10001000" + 
				"00000000" + 
				"00001010" + 
				"00000000" + 
				"10000000" + 
				"00010000" + 
				"10000000";
			
			string resultBoard = 
				"00000000" + 
				"10000000" + 
				"10000000" + 
				"01111000" + 
				"10000000" + 
				"10000000" + 
				"00000000" + 
				"00000000";
				
			ulong otherFigures = BitBoardHelper.FromString(otherFiguresBoard);
			ulong result = BitBoardHelper.FromString(resultBoard);
			
			Assert.AreEqual(result, 
				RookBitBoardHelper.GetRookAttacks(rank, file, otherFigures));
		}
		
		[Test()]
		public void RookPosition4()
		{
			string rookBoard = 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000001";
			
			File file = File.H;
			int rank = 0;
			
			string otherFiguresBoard = 
				"00100001" + 
				"10001001" + 
				"00000001" + 
				"00001010" + 
				"00000000" + 
				"10000000" + 
				"00010000" + 
				"10100000";
			
			string resultBoard = 
				"00000000" + 
				"00000000" + 
				"00000001" + 
				"00000001" + 
				"00000001" + 
				"00000001" + 
				"00000001" + 
				"00111110";
								
			ulong otherFigures = BitBoardHelper.FromString(otherFiguresBoard);
			ulong result = BitBoardHelper.FromString(resultBoard);
			
			Assert.AreEqual(result, 
				RookBitBoardHelper.GetRookAttacks(rank, file, otherFigures));
		}
	}
}

