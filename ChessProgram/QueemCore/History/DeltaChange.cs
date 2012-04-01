using System;
using System.Collections.Generic;

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

        public DeltaChange(DeltaChange from)
        {
            int changesCount = from.changes.Length;
            this.changes = new Change[changesCount];
            for (int i = 0; i < changesCount; ++i)
                this.changes[i] = new Change(from.changes[i]);

            this.lastIndex = from.lastIndex;
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

        public DeltaChange GetCopy()
        {
            return new DeltaChange(this);
        }

        public IEnumerable<Change> Filter(Func<Change, bool> predicate)
        {
            for (int i = 0; i <= this.lastIndex; ++i)
            {
                if (predicate(this.changes[i]))
                    yield return this.changes[i];
            }
        }
	}
}

