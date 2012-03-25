using System;
using QueemCore;
using Queem.CoreInterface;
using Queem.CoreInterface.Interface;

namespace Queem.CoreInterface.Adapters
{
	public class ChessMoveAdapter : ChessMove
	{
		protected Coordinates start;
		protected Coordinates end;
		
		public ChessMoveAdapter (Move move)
		{
			this.start = new CoordinatesAdapter(move.From);
			this.end = new CoordinatesAdapter(move.To);
		}
		
		public override Coordinates Start 
		{
			get 
			{
				return start;
			}
		}
		
		public override Coordinates End 
		{
			get 
			{
				return end;
			}
		}
	}
}

