using System;

namespace Queem.Core
{
	public class Move
	{
		public Move (Square start, Square end)
			:this(start, end, MoveType.Quiet)
		{
		}
		
		public Move (Square start, Square end, MoveType type)
		{
			this.From = start;
			this.To = end;
			this.Type = MoveType.Quiet;
		}

        public Move(string str)
        {
            throw new NotImplementedException();
        }

        public Move(Move from)
        {
            this.From = from.From;
            this.To = from.To;
            this.Type = from.Type;
        }
		
		public Square From
		{
			get; set;
		}
		
		public Square To
		{
			get; set;
		}
		
		public MoveType Type 
		{ 
			get; set;
		}
	}
}

