using System;

namespace QueemCore.History
{
	public class DeltaChange
	{
		protected Change[] changes;
		protected int lastIndex;
		
		public DeltaChange()
		{
			this.changes = new Change[5];
			this.lastIndex = -1;
		}
		
		public Change GetNext(MoveAction action)
		{
#if DEBUG
			if (lastIndex == (changes.Length - 1))
				throw new InvalidOperationException();
#endif		
			lastIndex++;
			this.changes[lastIndex].Action = action;
			return this.changes[lastIndex];
		}
		
		public Change GetCurrent()
		{
			return this.changes[this.lastIndex];
		}
		
		public Change PopLast()
		{
			return this.changes[this.lastIndex--];
		}
		
		public void RemoveLast()
		{
			this.lastIndex--;
		}
		
		public void AddItem()
		{
			this.lastIndex++;
		}
	}
}

