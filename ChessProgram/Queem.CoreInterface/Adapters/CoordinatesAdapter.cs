using System;
using Queem.Core;
using Queem.CoreInterface.Interface;

namespace Queem.CoreInterface.Adapters
{
	public class CoordinatesAdapter : Coordinates
	{
		protected Square square;
		
		public CoordinatesAdapter (Square sq)
		{
			this.square = sq;
		}
		
		public override int Y 
		{
			get 
			{
				return 7 - ((int)this.square >> 3);
			}
		}
		
		public override FieldLetter Letter 
		{
			get 
			{
				return (FieldLetter)this.X;
			}
		}
		
		public override int X 
		{
			get 
			{
				return ((int)this.square & 7);
			}
		}
		
		public override void Set (int x, int y)
		{
			this.square = (Square)(8*(7 - y) + x);
		}
	}
}

