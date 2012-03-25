using System;

namespace Queem.Core.ChessBoard
{
	public class FixedArray
	{
		public Move[] InnerArray { get; set; }
		public int Size { get; set; }
	}

	public static class MovesArray
	{
		private static int CurrentIndex;
		private static readonly FixedArray[] AllocatedArrays;
		private static readonly int HistorySize = 10000;
		// (8*7 max figure moves)*8 (max count of figures with max moves) == 448
		private static readonly int MaxMovesSize = 448; 
		
		static MovesArray ()
		{
			AllocatedArrays = new FixedArray[HistorySize];
			for (int i = 0; i < HistorySize; ++i)
			{
				AllocatedArrays[i] = new FixedArray();
				var innerArray = new Move[MaxMovesSize];
				for (int j = 0; j < MaxMovesSize; ++j)
					innerArray[j] = new Move(Square.NoSquare, Square.NoSquare);
					
				AllocatedArrays[i].InnerArray = innerArray;
			}
			
			CurrentIndex = -1;
		}
		
		public static FixedArray New()
		{
			CurrentIndex += 1;
			AllocatedArrays[CurrentIndex].Size = 0;
			return AllocatedArrays[CurrentIndex];
		}
		
		public static void ReleaseLast()
		{
			CurrentIndex -= 1;
		}
	}
}

