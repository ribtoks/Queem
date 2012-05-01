using System;
using Queem.Core;

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
        public int PlayerIndex { get; set; }
        public bool WasNullMove { get; set; }
	
		public virtual ChessTreeNode GetNext()
		{
			var nextNode = new ChessTreeNode();
			nextNode.Alpha = -this.Beta;
			nextNode.Beta = -this.Alpha;
			nextNode.Depth = this.Depth - 1;
            nextNode.PlayerIndex = 1 - this.PlayerIndex;
            nextNode.WasNullMove = this.WasNullMove;
			
			return nextNode;
		}

        public virtual ChessTreeNode GetNextZW()
        {
            var nextNode = new ChessTreeNode();
            nextNode.Beta = 1 - this.Beta;
            nextNode.Depth = this.Depth - 1;
            nextNode.PlayerIndex = 1 - this.PlayerIndex;
            nextNode.WasNullMove = false;
            return nextNode;
        }

        public virtual ChessTreeNode GetNextQuiescenceZW()
        {
            var nextNode = new ChessTreeNode();
            nextNode.Alpha = this.Beta - 1;
            nextNode.Beta = this.Beta;
            nextNode.Depth = this.Depth - 1;
            nextNode.PlayerIndex = this.PlayerIndex;
            nextNode.WasNullMove = this.WasNullMove;
            return nextNode;
        }

        public virtual ChessTreeNode GetNextNullMoveZW(int r)
        {
            var nextNode = new ChessTreeNode();
            nextNode.Beta = 1 - this.Beta;
            nextNode.Depth = this.Depth - r - 1;
            nextNode.PlayerIndex = 1 - this.PlayerIndex;
            nextNode.WasNullMove = true;
            return nextNode;
        }

        public bool IsZeroDepth()
        {
            return this.Depth <= 0;
        }
	}
}

