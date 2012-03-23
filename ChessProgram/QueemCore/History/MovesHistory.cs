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
		protected List<MoveType> moveResults;
		protected int lastIndex;
	
		public MovesHistory ()
		{
			this.moves = new List<Move>(maxlength);
			this.deltaChanges = new List<DeltaChange>(maxlength);
			this.moveResults = new List<MoveType>(maxlength);
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
				moveResults.Add(MoveType.Quiet);
			}
		}
		
		public DeltaChange GetCurrentDeltaChange()
		{
			return this.deltaChanges[lastIndex];
		}
		
		public DeltaChange GetNextDeltaChange()
		{
			lastIndex++;
			return this.deltaChanges[lastIndex];
		}
		
		public Move GetCurrentMove()
		{
			return this.moves[lastIndex];
		}
		
		public Move GetNextMove()
		{
			lastIndex++;
			return this.moves[lastIndex];
		}
		
		public MoveType GetCurrentMoveResult()
		{
			return this.moveResults[lastIndex];
		}
		
		public void SetMoveResult(MoveType result)
		{
			this.moveResults[lastIndex] = result;
		}
		
		public void AddItem()
		{
			lastIndex++;
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

