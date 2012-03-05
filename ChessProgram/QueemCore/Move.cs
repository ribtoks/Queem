using System;

namespace QueemCore
{
	public class Move
	{
		public Move (Square start, Square end)
		{
			this.From = start;
			this.To = end;
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

