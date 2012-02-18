using System;
using NUnit.Framework;
using QueemCore.BitBoard;
using QueemCore.BitBoard.Helpers;
using QueemCore;

namespace BitBoardTests
{
	[TestFixture()]
	public class RookBitBoardTest
	{
		private string GetEmptyBoard()
		{
			return
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
		}
		
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
				"00000000" + 
				"00010000" + 
				"00000000" + 
				"00000000" + 
				"00000000" + 
				"00000000";
			
			File file = File.D;
			int rank = 4;
			
			string otherFiguresBoard = this.GetEmptyBoard();
			
			string resultBoard = 
				"00010000" + 
				"00010000" + 
				"00010000" + 
				"11101111" + 
				"00010000" + 
				"00010000" + 
				"00010000" + 
				"00010000";
			
			ulong rook = BitBoardHelper.FromString(rookBoard);
			ulong otherFigures = BitBoardHelper.FromString(otherFiguresBoard);
			ulong result = BitBoardHelper.FromString(resultBoard);
			
			Assert.AreEqual(result, 
				RookBitBoardHelper.GetRookAttacks(rank, file, otherFigures));
		}
	}
}

