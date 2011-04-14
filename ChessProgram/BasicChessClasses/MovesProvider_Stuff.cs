using System;
using System.Collections.Generic;

namespace BasicChessClasses
{
	public partial class MovesProvider
	{
		public FigureStartPosition WhitePos
		{
			get { return whitePos; }
			//set { whitePos = value; }
		}

        public ChessBoard ChessBoard
        {
            get { return board; }
        }

        public ChessPlayerBase Player1
        {
            get { return player1; }
        }

        public ChessPlayerBase Player2
        {
            get { return player2; }
        }

        public ChessPlayerBase GetPlayer(FigureColor color)
        {
            return players[(int)color];
        }

        public MovesHistory History
        {
            get { return history; }
        }
		
		public bool IsThreefoldRepetition ()
		{
			return (positionCounts[board.HashCode] == 3);
		}
		
		public bool IsStalemate (ChessPlayerBase player, ChessPlayerBase opponentPlayer)
		{
			FiguresManager fm = player.FiguresManager;
			List<Coordinates> coords;

            var king_coords = GetFilteredCells(fm.Kings.King.Coordinates);
            if (king_coords.Count != 0)
                return false;
			
			// horses usualy can make more moves, because
            // they can jump over other figures
            // that's why we must first check their cells
			foreach (var figure in fm.Horses)
			{
				coords = GetFilteredCells (figure.Coordinates);
				if (coords.Count != 0)
					return false;
			}
			
			// then check pawns, because, there're many of them
			foreach (var figure in fm.Pawns)
			{
				coords = GetFilteredCells (figure.Coordinates);
				if (coords.Count != 0)
					return false;
			}
			
			// then queen, because it can move
			// to many cells
			foreach (var figure in fm.Queens)
			{
				coords = GetFilteredCells (figure.Coordinates);
				if (coords.Count != 0)
					return false;
			}
			
			foreach (var figure in fm.Rooks)
			{
				coords = GetFilteredCells (figure.Coordinates);
				if (coords.Count != 0)
					return false;
			}
			
			foreach (var figure in fm.Bishops)
			{
				coords = GetFilteredCells (figure.Coordinates);
				if (coords.Count != 0)
					return false;
			}
			
			return true;
		}
		
		public bool IsCheckmate (ChessPlayerBase player, ChessPlayerBase opponentPlayer)
		{
			if (!IsInCheck (player, opponentPlayer))
				return false;
			
			return IsStalemate (player, opponentPlayer);
		}
		
		public bool IsCheckmate (bool isStalemate, ChessPlayerBase player, 
		                         ChessPlayerBase opponentPlayer)
		{
			if (!IsInCheck (player, opponentPlayer))
				return false;
			
			return isStalemate;
		}
		
		public virtual void ReflectAll (bool withColors)
		{
			history.Reflect ();
			
			whitePos = whitePos.GetOppositePosition ();
			
			board.ReflectBoard (withColors);
			
			player1.Reflect (withColors);
			player2.Reflect (withColors);
		}
		
		public virtual void ResetAll ()
		{
			history.Clear ();
			
			board = new ChessBoard (player1.StartPos, player1.FiguresColor);
			
			player1 = new ChessPlayerBase (player1.FiguresColor, player1.StartPos);
			player2 = new ChessPlayerBase (player2.FiguresColor, player2.StartPos);

            this.SetupInnerColorToObjectMaps();
		}

        public void CancelMove(FigureColor playerColor)
        {
            if (playerColor == player1.FiguresColor)
                CancelLastPlayerMove(player1, player2);
            else
                CancelLastPlayerMove(player2, player1);
        }
	}
}
