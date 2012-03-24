using System;
using QueemCore;
using System.Collections.Generic;

namespace QueemCore.History
{
	public class MovesHistory
	{
		protected readonly int maxlength = 10000;
		protected List<Move> moves;
		protected List<DeltaChange> deltaChanges;
		protected int lastIndex;
	
		public MovesHistory ()
		{
			this.moves = new List<Move>(maxlength);
			this.deltaChanges = new List<DeltaChange>(maxlength);			
			this.lastIndex = -1;
			this.FillCache();
		}
		
		protected void FillCache()
		{
			int N = this.maxlength;
			
			while ((--N) > 0)
			{
				moves.Add(new Move(Square.A1, Square.A1));
				deltaChanges.Add(new DeltaChange());
			}
		}
		
		public DeltaChange PopLastDeltaChange()
		{
			return this.deltaChanges[lastIndex--];
		}
		
		public DeltaChange GetLastDeltaChange()
		{
			return this.deltaChanges[lastIndex];
		}
		
		public DeltaChange GetNextDeltaChange()
		{
			lastIndex++;
			return this.deltaChanges[lastIndex];
		}
		
		public Move GetLastMove()
		{
			return this.moves[lastIndex];
		}
		
		public Move GetNextMove()
		{
			lastIndex++;
			return this.moves[lastIndex];
		}
		
		public void AddItem(Move move)
		{
			lastIndex++;
			
			this.moves[lastIndex].From = move.From;
			this.moves[lastIndex].To = move.To;
			this.moves[lastIndex].Type = move.Type;
		}
		
		public void RemoveLastItem()
		{
#if DEBUG
			if (lastIndex == 0)
				throw new InvalidOperationException();
#endif
			lastIndex--;
		}
	}
}

