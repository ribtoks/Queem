using System;
using System.Collections.Generic;

namespace BasicChessClasses
{
	public class ChessBoard : ICloneable
	{
		private GeneralFigure[,] board = new GeneralFigure[8, 8];
		private ulong currentHashCode = 0;

		private ChessBoard ()
		{
		}
		
		public ChessBoard (FigureStartPosition myStartPos, FigureColor myStartColor)
		{
			InitializeBoard ();
			
			if (myStartPos == FigureStartPosition.Down) 
			{
				SetFiguresDown (myStartColor);
				SetFiguresUp ((FigureColor)(1 - (int)myStartColor));
			} 
			else 
			{
				SetFiguresUp (myStartColor);
				SetFiguresDown ((FigureColor)(1 - (int)myStartColor));
			}
			
			// now calculate initial hash
			for (int i = 0; i < 8; ++i)
			{
				for (int j = 0; j < 8; ++j)
				{
					currentHashCode ^= Constants.FigureHashes[i, j][ board[i, j] ];
				}
			}
		}
		
		#region ICloneable implementation
		
		public object Clone ()
		{
			ChessBoard cb = new ChessBoard ();
			
			for (int i = 0; i < 8; ++i)
			{
				for (int j = 0; j < 8; ++j)
				{
					cb.board[i, j] = new GeneralFigure(board[i, j]);
				}
			}
			
			return cb;
		}
		
		object ICloneable.Clone ()
		{
			return Clone ();
		}
		
		#endregion
		
		#region Some board Stuff

		public void InitializeBoard ()
		{
			for (int i = 0; i < 8; ++i)
				for (int j = 0; j < 8; ++j)
					board[i, j] = new GeneralFigure ();
		}
		
		public GeneralFigure this[Coordinates coords]
		{
			get { return board[coords.Y, coords.X]; }
			set { board[coords.Y, coords.X].ReAssign (value); }
		}
		
		public GeneralFigure this[FieldLetter letter, int y]
		{
			get { return board[y, (int)letter]; }
			set { board[y, (int)letter].ReAssign (value); }
		}
		
		public GeneralFigure this[int x, int y]
		{
			get 
			{
				if (((x & 7) != x) || ((y & 7) != y))
					return GeneralFigure.NullFigure;
				
				return board[y, x];
			}
		}
		
		private void SwapRows (int row1, int row2)
		{
			GeneralFigure temp = new GeneralFigure ();
			for (int i = 0; i < 8; ++i)
			{
				temp.ReAssign (board[row1, i]);
				board[row1, i].ReAssign (board[row2, i]);
				board[row2, i].ReAssign (temp);
			}
		}
		
		private void SwapColumns (int col1, int col2)
		{
			GeneralFigure temp = new GeneralFigure ();
			for (int k = 0; k < 8; ++k)
            {
				temp.ReAssign (board[k, col1]);
				board[k, col1].ReAssign (board[k, col2]);
				board[k, col2].ReAssign (temp);
			}
		}
		
		public void ReflectBoard (bool withColors)
		{
			int i = 0;

			for (i = 0; i < 4; ++i)
				SwapRows (i, 8 - i - 1);

            for (i = 0; i < 4; ++i)
				SwapColumns (i, 8 - i - 1);

            if (withColors)
				for (i = 0; i < 8; ++i)
					for (int j = 0; j < 8; ++j)
						board[i, j].ReflectColor ();
		}
		
		private void SetFiguresDown (FigureColor color)
		{
			for (int i = 0; i < 8; ++i)
				board[8 - 2, i] = new GeneralFigure (FigureType.Pawn, color);
			
			board[8 - 1, 0] = new GeneralFigure (FigureType.Rook, color);
			board[8 - 1, 7] = new GeneralFigure (FigureType.Rook, color);
			
			board[8 - 1, 1] = new GeneralFigure (FigureType.Horse, color);
			board[8 - 1, 6] = new GeneralFigure (FigureType.Horse, color);
			
			board[8 - 1, 2] = new GeneralFigure (FigureType.Bishop, color);
			board[8 - 1, 5] = new GeneralFigure (FigureType.Bishop, color);

            if (color == FigureColor.White)
            {
				board[8 - 1, 3] = new GeneralFigure (FigureType.Queen, color);
				board[8 - 1, 4] = new GeneralFigure (FigureType.King, color);
			}
            else
            {
				board[8 - 1, 4] = new GeneralFigure (FigureType.Queen, color);
				board[8 - 1, 3] = new GeneralFigure (FigureType.King, color);
			}
		}
		
		private void SetFiguresUp (FigureColor color)
		{
			for (int i = 0; i < 8; ++i)
				board[1, i] = new GeneralFigure (FigureType.Pawn, color);
			
			board[0, 0] = new GeneralFigure (FigureType.Rook, color);
			board[0, 7] = new GeneralFigure (FigureType.Rook, color);
			
			board[0, 1] = new GeneralFigure (FigureType.Horse, color);
			board[0, 6] = new GeneralFigure (FigureType.Horse, color);
			
			board[0, 2] = new GeneralFigure (FigureType.Bishop, color);
			board[0, 5] = new GeneralFigure (FigureType.Bishop, color);

            if (color == FigureColor.Black)
            {
				board[0, 3] = new GeneralFigure (FigureType.Queen, color);
				board[0, 4] = new GeneralFigure (FigureType.King, color);
			}
            else
            {
				board[0, 4] = new GeneralFigure (FigureType.Queen, color);
				board[0, 3] = new GeneralFigure (FigureType.King, color);
			}
		}
		
		public bool HitTest (Coordinates coords)
		{
			if ((coords.Y & 7) != coords.Y)
				return false;
			
			int x = coords.X;
			
			if ((x & 7) != x)
				return false;
			
			return true;
		}
		
		public bool HitTest (int x, int y)
		{
			if ((y & 7) != y)
				return false;
			
			if ((x & 7) != x)
				return false;
			
			return true;
		}
		
		public bool IsAFriendOn (Coordinates coords, FigureColor myColor)
		{
			return (board[coords.Y, coords.X].Color == myColor);
		}
		
		public bool IsAFriendOn (FieldLetter letter, int y, FigureColor myColor)
		{
			return (board[y, (int)letter].Color == myColor);
		}
		
		public bool IsAFriendOn (int x, int y, FigureColor myColor)
		{
			return (board[y, x].Color == myColor);
		}
		
		public bool IsAnEnemyOn (Coordinates coords, FigureColor myColor)
		{
			return (board[coords.Y, coords.X].Color != myColor);
		}
		
		public bool IsAnEnemyOn (FieldLetter letter, int y, FigureColor myColor)
		{
			return (board[y, (int)letter].Color != myColor);
		}
		
		public bool IsAnEnemyOn (int x, int y, FigureColor myColor)
		{
			return (board[y, x].Color != myColor);
		}
		
		public bool IsAnyFigureAt (int x, int y, FigureColor color)
		{
			if (board[y, x].Type != FigureType.Nobody)
				if (board[y, x].Color == color)
					return true;
			return false;
		}
		
		public bool IsAnyFigureAt (FieldLetter letter, int y, FigureColor color)
		{
			int x = (int) letter;
			
			if (board[y, x].Type != FigureType.Nobody)
				if (board[y, x].Color == color)
					return true;
			return false;
		}
		
		public bool IsAnyFigureAt (Coordinates coords, FigureColor color)
		{
			int x = coords.X;
			int y = coords.Y;
			
			if (board[y, x].Type != FigureType.Nobody)
				if (board[y, x].Color == color)
					return true;
			return false;
		}
		
		#endregion
		
		public void ProvideMove (ChessMove move, MoveResult moveResult)
		{
			int startX = move.Start.X;
			int startY = move.Start.Y;
			
			int endX = move.End.X;
			int endY = move.End.Y;
			
			GeneralFigure endFigure = board[endY, endX];
			GeneralFigure startFigure = board[startY, startX];
			
			// -----------------------------------------------------
			// xor out a figure, that is taken (or an emply cell)
			currentHashCode ^= Constants.FigureHashes[move.End][endFigure];
			
			// xor out a figure, that is moving
			currentHashCode ^= Constants.FigureHashes[move.Start][startFigure];
			// -----------------------------------------------------
			
			// check if some in-passing pawn was killed
			if (moveResult == MoveResult.CapturedInPassing)
			{
				// remove figure, that was there
				currentHashCode ^= Constants.FigureHashes[startY, endX][ board[startY, endX] ];
				
				board[startY, endX].Type = FigureType.Nobody;
				
				// add nobody there
				currentHashCode ^= Constants.FigureHashes[startY, endX][ board[startY, endX] ];
			}
			
			// making a move
			
			endFigure.Color = startFigure.Color;
			endFigure.Type = startFigure.Type;
			
			startFigure.Type = FigureType.Nobody;
			
			// --------------------------------------
			// now xor other figures
			// --------------------------------------
			
			// xor FigureType.Nobody
			currentHashCode ^= Constants.FigureHashes[move.Start][startFigure];
			
			// add a figure in move.end
			currentHashCode ^= Constants.FigureHashes[move.End][endFigure];
		}
		
		public void CancelMove (Change moveChange)
		{
			int startX = moveChange.Coords.X;
			int startY = moveChange.Coords.Y;
			
			int endX = moveChange.AdditionalCoords.X;
			int endY = moveChange.AdditionalCoords.Y;
			
			GeneralFigure endFigure = board[endY, endX];
			GeneralFigure startFigure = board[startY, startX];
			
			// remove old figures
			currentHashCode ^= Constants.FigureHashes[moveChange.AdditionalCoords][endFigure];
			currentHashCode ^= Constants.FigureHashes[moveChange.Coords][startFigure];
			
			startFigure.Color = endFigure.Color;
			startFigure.Type = endFigure.Type;
				
			endFigure.Type = FigureType.Nobody;
			
			// add new Figures
			currentHashCode ^= Constants.FigureHashes[moveChange.AdditionalCoords][endFigure];
			currentHashCode ^= Constants.FigureHashes[moveChange.Coords][startFigure];
		}
		
		public void CancelDeletion (Change deletionChange)
		{
			int x = deletionChange.Coords.X;
			int y = deletionChange.Coords.Y;
			
			// remove hash
			currentHashCode ^= Constants.FigureHashes[deletionChange.Coords][ board[y, x] ];
			
			board[y, x].Type = deletionChange.FigureType;
			board[y, x].Color = deletionChange.FigureColor;
			
			// add hash
			currentHashCode ^= Constants.FigureHashes[deletionChange.Coords][ board[y, x] ];
		}
		
		public void CancelCreation (Change creationChange)
		{
			int x = creationChange.Coords.X;
			int y = creationChange.Coords.Y;
			
			// remove figure, that was there...
			currentHashCode ^= Constants.FigureHashes[creationChange.Coords][ board[y, x] ];
			
			board[y, x].Type = FigureType.Nobody;
            			
			// add nobody...
			currentHashCode ^= Constants.FigureHashes[creationChange.Coords][ board[y, x] ];
		}
		
		public bool IsHashCorrect ()
		{
			ulong hash = 0;
			
			// now calculate initial hash
			for (int i = 0; i < 8; ++i)
			{
				for (int j = 0; j < 8; ++j)
				{
					hash ^= Constants.FigureHashes[i, j][ board[i, j] ];
				}
			}
			
			return (hash == currentHashCode);
		}
		
		public override int GetHashCode ()
		{
			return (int)currentHashCode;
		}
		
		public override bool Equals (object obj)
		{
			if (obj == null)
                return false;

            ChessBoard board = obj as ChessBoard;
            if ((object)board == null)
                return false;

            if (board.currentHashCode != this.currentHashCode)
				return false;
			
            return true;
		}
		
		public ulong HashCode 
		{
			get { return this.currentHashCode; }
			set { currentHashCode = value; }
		}
	}
}
