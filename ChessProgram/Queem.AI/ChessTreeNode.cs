using System;

namespace Queem.AI
{
	public class ChessTreeNode
	{
		public ChessTreeNode ()
		{
		}
		
		public int Alpha { get; set; }
		public int Beta { get; set; }
		public int Depth { get; set; }
	
		public virtual ChessTreeNode GetNext()
		{
			var nextNode = new ChessTreeNode();
			nextNode.Alpha = -this.Beta;
			nextNode.Beta = -this.Alpha;
			
			return nextNode;
		}	
	}
}

