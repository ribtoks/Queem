using System;
using Queem.Core.ChessBoard;
using Queem.Core;

namespace Queem.AI
{
	public static class MovesSorter
	{
		private static Move[] tempBuffer = new Move[448];
							
		public static void Sort(FixedArray moves, PlayerBoard opponentBoard)
		{
			// approximate moves size - 40 moves
			// best approach to split on 20+20
			// sort each part with insert sort
			// and then merge them
			
			int size = moves.Size;
			int left = 0, right = size - 1;
			var array = moves.InnerArray;
			int coef = 10000;
			int split = (left + right) / 2;
			int i, j;
			
			for (i = 0; i < split; ++i)
			{
				int x = (int)array[i].Type * coef + 
					(int)opponentBoard.Figures[(int)array[i].To];
				Move move = array[i];
				
				for (j = i - 1; j >= 0; j--)
				{
					int y = (int)array[j].Type * coef + 
						(int)opponentBoard.Figures[(int)array[j].To];
					if (y <= x)
						 break;
					
					array[j + 1] = array[j];					
				}
				
				array[j + 1] = move;
			}
			
			for (i = split; i < size; ++i)
			{
				int x = (int)array[i].Type * coef + 
					(int)opponentBoard.Figures[(int)array[i].To];
					
				Move move = array[i];
				
				for (j = i - 1; j >= split; j--)
				{
					int y = (int)array[j].Type * coef + 
						(int)opponentBoard.Figures[(int)array[j].To];
					if (y <= x)
						 break;
					
					array[j + 1] = array[j];
				}
				
				array[j + 1] = move;
			}
			
			// merge
			int pos1 = 0;
   			int pos2 = split + 1;
   			int index = 0;

   			while ((pos1 <= split) && (pos2 <= right))
   			{
   				int x = (int)array[pos1].Type * coef + 
					(int)opponentBoard.Figures[(int)array[pos1].To];
					
				int y = (int)array[pos2].Type * coef + 
					(int)opponentBoard.Figures[(int)array[pos2].To];
      			// stable sort when <= 0
      			// and unstable when < 0
      			if (x <= y)
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
	}
}

