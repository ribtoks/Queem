using System;
using System.Collections.Generic;

namespace BasicChessClasses
{
	public enum MoveAction { Creation, Move, Deletion, PawnChange }
	
	public class Change
	{
		protected MoveAction action;
		protected Coordinates coords;
		protected Coordinates additionalCoords = null;
		protected bool data = false;
		protected FigureType figureType = FigureType.Nobody;
		protected FigureColor figureColor = FigureColor.Black;

		public Change (MoveAction moveAction, Coordinates coordinates, 
		               FigureType fType, FigureColor color)
		{
			action = moveAction;
			coords = new Coordinates (coordinates);
			figureType = fType;
			figureColor = color;
		}
		
		public Change (MoveAction moveAction, ChessMove move, FigureType fType, FigureColor color)
			:this(moveAction, move.Start, fType, color)
		{
			// specify end of move
			additionalCoords = new Coordinates (move.End);
		}
		
		public Change (MoveAction moveAction, ChessMove move, 
		               FigureType fType, FigureColor color, bool moveData)
			:this(moveAction, move, fType, color)
		{
			data = moveData;
		}
		
		public Change (Change copy)
		{
			action = copy.action;
			coords = new Coordinates (copy.coords);
			
			if ((object)copy.additionalCoords != null)
				additionalCoords = new Coordinates (copy.additionalCoords);
			
			data = copy.data;
			figureType = copy.figureType;
			figureColor = copy.figureColor;
		}

		public Coordinates AdditionalCoords 
		{
			get { return this.additionalCoords; }
			set { additionalCoords = value; }
		}

		public MoveAction Action
		{
			get { return this.action; }
			set { action = value; }
		}

		public Coordinates Coords
		{
			get { return this.coords; }
			set 
			{
				coords.Letter = value.Letter;
				coords.Y = value.Y;
			}
		}
		
		public bool Data 
		{
			get { return this.data; }
			set { data = value; }
		}
		
		public FigureType FigureType 
		{
			get { return this.figureType; }
			set { figureType = value; }
		}

		public FigureColor FigureColor 
		{
			get { return this.figureColor; }
			set { figureColor = value; }
		}
	}
	
	public class DeltaChanges
	{
		protected Stack<Change> changes;		

		public DeltaChanges ()
		{
			changes = new Stack<Change> ();
		}
		
		public DeltaChanges (DeltaChanges copy)
		{
			changes = new Stack<Change> ();
			Change[] arr = copy.changes.ToArray ();
			
			for (int i = arr.Length - 1; i >= 0; --i)
				changes.Push (new Change(arr[i]));
		}
		
		public void Add (Change change)
		{
			changes.Push (change);
		}
		
		public Stack<Change> Changes 
		{
			get { return this.changes; }
			set { changes = value; }
		}
		
		public void Reflect ()
		{
			Change[] arr = changes.ToArray ();
			changes.Clear ();
			
			for (int i = arr.Length - 1; i >= 0; --i)
			{
				arr[i].Coords.Reflect ();
				
				if ((object)arr[i].AdditionalCoords != null)
					arr[i].AdditionalCoords.Reflect ();
				
				changes.Push (arr[i]);
			}
		}
	}
	
	public class MovesHistory
	{
		protected List<ChessMove> moves;
		protected List<DeltaChanges> deltaChanges;
		protected List<MoveResult> moveResults;
		
		public MovesHistory ()
		{
			moves = new List<ChessMove> ();
			deltaChanges = new List<DeltaChanges> ();
			moveResults = new List<MoveResult> ();
		}
		
		public MovesHistory (MovesHistory copy)
		{
			moves = new List<ChessMove> (copy.moves.Count);
			deltaChanges = new List<DeltaChanges> (copy.deltaChanges.Count);
			moveResults = new List<MoveResult> (copy.moveResults);
			
			foreach (var move in copy.moves)
				moves.Add (new ChessMove (move));
			
			foreach (var deltaChange in copy.deltaChanges)
				deltaChanges.Add (new DeltaChanges (deltaChange));
		}
		
		public void Add (ChessMove move, MoveResult moveResult, DeltaChanges changes)
		{
			moves.Add ((ChessMove)move.Clone());
			deltaChanges.Add (changes);
			moveResults.Add (moveResult);
		}
		
		public void UndoLast ()
		{
			if (moves.Count == 0)
				return;
			
			moves.RemoveAt (moves.Count - 1);
			deltaChanges.RemoveAt (deltaChanges.Count - 1);
			moveResults.RemoveAt (moveResults.Count - 1);
		}

		public List<DeltaChanges> DeltaChanges 
		{
			get { return this.deltaChanges; }
		}

		public List<ChessMove> Moves 
		{
			get { return this.moves; }
		}
		
		public List<MoveResult> MoveResults
		{
			get { return this.moveResults; }
		}
		
		// unsafe methods... will crash if 
		// collections have no elements
		
		public ChessMove LastMove
		{
			get { return moves[moves.Count - 1]; }
		}
		
		public DeltaChanges LastChanges
		{
			get { return deltaChanges[deltaChanges.Count - 1]; }
		}
		
		public MoveResult LastMoveResult
		{
			get { return moveResults[moveResults.Count - 1]; }
		}
		
		public void Reflect ()
		{
			foreach (var move in moves)
				move.InnerReflect ();
			
			foreach (var changes in deltaChanges)
				changes.Reflect ();
		}
		
		public void Clear ()
		{
			deltaChanges.Clear ();
			moves.Clear ();
			moveResults.Clear ();
		}

        public int Count
        {
            get { return moves.Count; }
        }
	}
}
