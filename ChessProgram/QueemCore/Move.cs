using System;

namespace QueemCore
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

