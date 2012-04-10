using System;

namespace Queem.Core.ChessBoard
{
	public class FixedArray
	{
		public Move[] InnerArray { get; set; }
		public int Size { get; set; }
	}

	public class MovesArrayAllocator
	{
        private object syncRoot = new object();
		private int CurrentIndex;
		private readonly FixedArray[] AllocatedArrays;
		private readonly int HistorySize = 1000;
		// (8*7 max figure moves)*8 (max count of figures with max moves) == 448
		private readonly int MaxMovesSize = 448; 
		
		public MovesArrayAllocator ()
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
		
		public FixedArray CreateNewArray()
		{
            lock (syncRoot)
            {
                CurrentIndex += 1;
                AllocatedArrays[CurrentIndex].Size = 0;
                return AllocatedArrays[CurrentIndex];
            }
		}
		
		public void ReleaseLast()
		{
            lock (syncRoot)
            {
                CurrentIndex -= 1;
            }
		}
	}
}

