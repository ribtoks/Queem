using System;

namespace Queem.Core.History
{
	public class DeltaChange
	{
		protected Change[] changes;
		protected int lastIndex;
		
		public DeltaChange()
		{
            int changesCount = 5;
			this.changes = new Change[changesCount];
            for (int i = 0; i < changesCount; ++i)
                this.changes[i] = new Change();

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
		
		public bool HasItems()
		{
			return this.lastIndex >= 0;
		}
	}
}

