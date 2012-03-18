using System;

namespace QueemCore.History
{
	public class DeltaChange
	{
		protected Change[] changes;
		protected int lastIndex;
		
		public DeltaChange()
		{
			this.changes = new Change[10];
			this.lastIndex = 0;
		}
		
		public Change GetNext()
		{
#if DEBUG
			if (lastIndex == (changes.Length - 1))
				throw new InvalidOperationException();
#endif		
			return this.changes[++lastIndex];
		}
		
		public Change PickLast()
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
	}
}

