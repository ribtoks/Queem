using System;
using Queem.Core.ChessBoard;
using Queem.Core;
using System.Collections;
using System.Collections.Generic;

namespace Queem.AI
{
	internal delegate void QuickSortDelegate(Move[] items, int first, int last);

    public class MovesComparer : IComparer<Move>
    {
        public int Compare(Move x, Move y)
        {
            return y.Value - x.Value;
        }
    }

	public static class MovesSorter
	{
		public static void Sort2(FixedArray moves)
		{
            if (moves.Size == 0)
                return;

            Move[] tempBuffer = new Move[moves.Size];
            // approximate moves size - 40 moves
			// best approach to split on 20+20
			// sort each part with insert sort
			// and then merge them
			
			int size = moves.Size;
			int left = 0, right = size - 1;
			var array = moves.InnerArray;
			int split = (left + right) / 2;
			int i, j;
			
			for (i = 0; i < split; ++i)
			{
				Move move = array[i];
				
				for (j = i - 1; (j >= 0) && (array[j].Value > move.Value); j--)
					array[j + 1] = array[j];
				
				array[j + 1] = move;
			}
			
			for (i = split; i < size; ++i)
			{		
				Move move = array[i];
				
				for (j = i - 1; (j >= split) && (array[j].Value > move.Value); j--)
					array[j + 1] = array[j];
				
				array[j + 1] = move;
			}
			
			// merge
			int pos1 = 0;
   			int pos2 = split + 1;
   			int index = 0;

   			while ((pos1 <= split) && (pos2 <= right))
   			{
      			// stable sort when <= 0
      			// and unstable when < 0
      			if (array[pos1].Value <= array[pos2].Value)
         			tempBuffer[index++] = array[pos1++];
		      	else
		        	tempBuffer[index++] = array[pos2++];
		   }
		
		   while (pos1 <= split)
		      tempBuffer[index++] = array[pos1++];
		
		   while (pos2 <= right)
		      tempBuffer[index++] = array[pos2++];
		
		   for (i = left; i <= right; ++i)
		      array[i] = tempBuffer[i - left];
		}
		
		public static void Sort(FixedArray moves)
		{
            if (moves.Size == 0)
                return;

			QuickSortDelegate quicksort = null;
            quicksort =
                (items, first, last) =>
                {
                    int left = first;
                    int right = last;
                    int mid = items[(left + right) >> 1].Value;

                    while (left <= right)
                    {
                        while (items[left].Value > mid)
                            ++left;

                        while (items[right].Value < mid)
                            --right;

                        if (left <= right)
                        {
                            var tempItem = items[left];
                            items[left] = items[right];
                            items[right] = tempItem;

                            ++left;
                            --right;
                        }
                    }

                    if (first < right)
                        quicksort(items, first, right);

                    if (left < last)
                        quicksort(items, left, last);
                };

            quicksort(moves.InnerArray, 0, moves.Size - 1);
		}
	}
}

