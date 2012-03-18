using System;
using QueemCore.History;

namespace QueemCore.ChessBoard
{
	public class GameProvider
	{
		PlayerBoard playerBoard1;
		PlayerBoard playerBoard2;
		
		MovesHistory history;
	
		public GameProvider ()
		{
			this.playerBoard1 = new PlayerBoard(PlayerPosition.Down, Color.White);
			this.playerBoard2 = new PlayerBoard(PlayerPosition.Up, Color.Black);
			
			this.history = new MovesHistory();
		}
		
		public MoveType MakeMove(Move move)
		{
			
		}
	}
}

