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
        private static readonly int DefaultHistorySize = 1000;
        // (8*7 max figure moves)*8 (max count of figures with max moves) == 448
        private static readonly int DefaultMaxMovesSize = 448;

        private object syncRoot = new object();
		private int CurrentIndex;
		private readonly FixedArray[] AllocatedArrays;
        private int HistorySize;
		private int MaxMovesSize; 
		
		public MovesArrayAllocator ()
            :this(DefaultHistorySize, DefaultMaxMovesSize)
		{	
		}

        public MovesArrayAllocator(int historySize, int maxMovesSize)
        {
            this.HistorySize = historySize;
            this.MaxMovesSize = maxMovesSize;

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

