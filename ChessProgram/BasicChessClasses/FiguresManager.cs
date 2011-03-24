using System;
using System.Collections;
using System.Collections.Generic;

namespace BasicChessClasses
{
	public delegate void AddFigureDelegate (Coordinates addWhere, FigureType type, FigureColor color);
	public delegate void MoveFigureDelegate (Coordinates moveFrom, Coordinates moveWhere);
	public delegate void RemoveFigureDelegate (Coordinates coords);
	public delegate void RestoreFigureDelegate (Coordinates coords);
	public delegate void DestroyFigureDelegate (Coordinates coords);
	public delegate void SetBoolPropertyDelegate (Coordinates coordinates, bool property);
	public delegate bool GetBoolPropertyDelegate (Coordinates coordinates);
		
	public class FiguresManagementDelegates
	{
		protected AddFigureDelegate addFigure;
		protected MoveFigureDelegate moveFigure;
		protected RemoveFigureDelegate removeFigure;
		protected RestoreFigureDelegate restoreFigure;
		protected DestroyFigureDelegate destroyFigure;
		protected SetBoolPropertyDelegate setBoolProperty;
		protected GetBoolPropertyDelegate getBoolProperty;
		
		public FiguresManagementDelegates (AddFigureDelegate addFigureDelegate,
		                                   MoveFigureDelegate moveFigureDelegate,
		                                   RemoveFigureDelegate removeFigureDelegate,
		                                   RestoreFigureDelegate restoreFigureDelegate,
		                                   DestroyFigureDelegate destroyFigureDelegate,
		                                   SetBoolPropertyDelegate setBoolPropertyDelegate,
		                                   GetBoolPropertyDelegate getBoolPropertyDelegate)
		{
			addFigure = addFigureDelegate;
			moveFigure = moveFigureDelegate;
			removeFigure = removeFigureDelegate;
			restoreFigure = restoreFigureDelegate;
			destroyFigure = destroyFigureDelegate;
			setBoolProperty = setBoolPropertyDelegate;
			getBoolProperty = getBoolPropertyDelegate;
		}
		
		public AddFigureDelegate AddFigure 
		{
			get { return this.addFigure; }
			set { addFigure = value; }
		}

		public MoveFigureDelegate MoveFigure 
		{
			get { return this.moveFigure; }
			set { moveFigure = value; }
		}

		public RemoveFigureDelegate RemoveFigure 
		{
			get { return this.removeFigure; }
			set { removeFigure = value; }
		}
		
		public RestoreFigureDelegate RestoreFigure 
		{
			get { return this.restoreFigure; }
			set { restoreFigure = value; }
		}
		
		public DestroyFigureDelegate DestroyFigure 
		{
			get { return this.destroyFigure; }
			set { destroyFigure = value; }
		}
		
		public SetBoolPropertyDelegate SetBoolProperty 
		{
			get { return this.setBoolProperty; }
			set { setBoolProperty = value; }
		}
		
		public GetBoolPropertyDelegate GetBoolProperty 
		{
			get { return this.getBoolProperty; }
			set { getBoolProperty = value; }
		}
	}
		
		
	public class FiguresManager : IEnumerable<Figure>
	{
		#region Data
		
		protected PawnCollection pawns;
		protected FigureCollection<Bishop> bishops;
		protected FigureCollection<Horse> horses;
		protected RookCollection rooks;
		protected FigureCollection<Queen> queens;
		protected KingCollection kings;
		
		// dictionary is much more slower, than usual switch for ints, 
		// that's why i'll delete it
		//protected Dictionary<FigureType, FiguresManagementDelegates> handlers;
		FiguresManagementDelegates pawnDeletages;
		FiguresManagementDelegates rookDeletages;
		FiguresManagementDelegates bishopDeletages;
		FiguresManagementDelegates horseDeletages;
		FiguresManagementDelegates queenDeletages;
		FiguresManagementDelegates kingDeletages;
		
		#endregion

        #region Properties

        public int PawnCount
        {
            get { return pawns.Count; }
        }

        public int BishopCount
        {
            get { return bishops.Count; }
        }

        public int HorseCount
        {
            get { return horses.Count; }
        }

        public int RookCount
        {
            get { return rooks.Count; }
        }

        public int QueenCount
        {
            get { return queens.Count; }
        }

        #endregion

        public FiguresManager (FigureStartPosition startPos, FigureColor figuresColor)
		{
			pawns = new PawnCollection ();
			rooks = new RookCollection ();
			bishops = new FigureCollection<Bishop> ();
			horses = new FigureCollection<Horse> ();
			queens = new FigureCollection<Queen> ();	
			kings = new KingCollection ();
			
			pawnDeletages = 
				new FiguresManagementDelegates(pawns.AddFigure,
				                               pawns.MoveFigure,
				                               pawns.RemoveFigure,
				                               pawns.RestoreFigure,
				                               pawns.DestroyFigure,
				                               pawns.SetBoolProperty,
				                               pawns.GetBoolProperty);
			
			rookDeletages = 
				new FiguresManagementDelegates(rooks.AddFigure,
				                               rooks.MoveFigure,
				                               rooks.RemoveFigure,
				                               rooks.RestoreFigure,
				                               rooks.DestroyFigure,
				                               rooks.SetBoolProperty,
				                               rooks.GetBoolProperty);
			
			bishopDeletages = 
				new FiguresManagementDelegates(bishops.AddFigure,
				                               bishops.MoveFigure,
				                               bishops.RemoveFigure,
				                               bishops.RestoreFigure,
				                               bishops.DestroyFigure,
				                               bishops.SetBoolProperty,
				                               bishops.GetBoolProperty);
			
			horseDeletages = 
				new FiguresManagementDelegates(horses.AddFigure,
				                               horses.MoveFigure,
				                               horses.RemoveFigure,
				                               horses.RestoreFigure,
				                               horses.DestroyFigure,
				                               horses.SetBoolProperty,
				                               horses.GetBoolProperty);
			
			queenDeletages = 
				new FiguresManagementDelegates (queens.AddFigure,
				                               queens.MoveFigure,
				                               queens.RemoveFigure,
				                               queens.RestoreFigure,
				                               queens.DestroyFigure,
				                               queens.SetBoolProperty,
				                               queens.GetBoolProperty);
			
			kingDeletages = 
				new FiguresManagementDelegates (kings.AddFigure,
				                               kings.MoveFigure,
				                               kings.RemoveFigure,
				                               kings.RestoreFigure,
				                               kings.DestroyFigure,
				                               kings.SetBoolProperty,
				                               kings.GetBoolProperty);
			/*
			handlers = new Dictionary<FigureType, FiguresManagementDelegates> ();
			handlers.Add (FigureType.Pawn, pawnDeletages);
			handlers.Add (FigureType.Horse, horseDeletages);
			handlers.Add (FigureType.Rook, rookDeletages);
			handlers.Add (FigureType.Bishop, bishopDeletages);
			handlers.Add (FigureType.Queen, queenDeletages);
			handlers.Add (FigureType.King, kingDeletages);
			*/
			FillFigures (startPos, figuresColor);
		}
		
		public FiguresManager (FiguresManager copy)
		{
			pawns = (PawnCollection) copy.pawns.Clone ();
			bishops = (FigureCollection<Bishop>) copy.bishops.Clone ();
			horses = (FigureCollection<Horse>) copy.horses.Clone ();
			rooks = (RookCollection) copy.rooks.Clone ();
			queens = (FigureCollection<Queen>) copy.queens.Clone ();
			kings = (KingCollection) copy.kings.Clone ();
			
			// just copy "function pointers"
			//handlers = copy.handlers;
			
			pawnDeletages = copy.pawnDeletages;
			horseDeletages = copy.horseDeletages;
			rookDeletages = copy.rookDeletages;
			bishopDeletages = copy.bishopDeletages;
			queenDeletages = copy.queenDeletages;
			kingDeletages = copy.kingDeletages;
		}
		
		#region Properties
		
		public FigureCollection<Bishop> Bishops 
		{
			get { return this.bishops; }
		}

		public FigureCollection<Horse> Horses 
		{
			get { return this.horses; }
		}

		public KingCollection Kings 
		{
			get { return this.kings; }
		}

		public PawnCollection Pawns 
		{
			get { return this.pawns; }
		}

		public FigureCollection<Queen> Queens 
		{
			get { return this.queens; }
		}

		public RookCollection Rooks 
		{
			get { return this.rooks; }
		}
		
		#endregion

        public int GetPositionValue(int[,] positionValues)
        {
            int value = 0;

            value += pawns.GetPositionValue(positionValues);
            value += bishops.GetPositionValue(positionValues);
            value += horses.GetPositionValue(positionValues);
            value += rooks.GetPositionValue(positionValues);
            value += queens.GetPositionValue(positionValues);

            return value;
        }
		
		public IEnumerator<Figure> GetEnumerator ()
		{
			foreach (Pawn pawn in pawns)
				yield return pawn;
			
			foreach (Rook rook in rooks)
				yield return rook;
			
			foreach (Bishop bishop in bishops)
				yield return bishop;
			
			foreach (Horse horse in horses)
				yield return horse;
			
			foreach (Queen queen in queens)
				yield return queen;
			
			foreach (King king in kings)
				yield return king;
		}
		
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
		
		protected void FillFigures (FigureStartPosition startPos, FigureColor figuresColor)
		{
			int i = 0;
            Coordinates coords = new Coordinates();

            coords.Y = ((int)startPos) - 1;
            for (i = 0; i < 8; ++i)
            {
                coords.Letter = (FieldLetter)i;                
                pawns.AddFigure(new Pawn(coords, figuresColor));
            }

            //add to list all rooks
            coords.Y = (startPos == FigureStartPosition.Up) ? (0) : (8 - 1);
            coords.Letter = FieldLetter.A;
            rooks.AddFigure(new Rook(coords, figuresColor));
            coords.Letter = FieldLetter.H;
            rooks.AddFigure(new Rook(coords, figuresColor));

            //add all horses
            coords.Y = (startPos == FigureStartPosition.Up) ? (0) : (8 - 1);
            coords.Letter = FieldLetter.B;
            horses.AddFigure(new Horse(coords, figuresColor));
            coords.Letter = FieldLetter.G;
            horses.AddFigure(new Horse(coords, figuresColor));

            //add to list all bishops
            coords.Y = (startPos == FigureStartPosition.Up) ? (0) : (8 - 1);
            coords.Letter = FieldLetter.C;
            bishops.AddFigure(new Bishop(coords, figuresColor));
            coords.Letter = FieldLetter.F;
            bishops.AddFigure(new Bishop(coords, figuresColor));

            //add queen
            coords.Y = (startPos == FigureStartPosition.Up) ? (0) : (8 - 1);
            if (startPos == FigureStartPosition.Down)
            {
                if (figuresColor == FigureColor.Black)
                    coords.Letter = FieldLetter.E;
                else
                    coords.Letter = FieldLetter.D;
            }
            else
            {
                if (figuresColor == FigureColor.Black)
                    coords.Letter = FieldLetter.D;
                else
                    coords.Letter = FieldLetter.E;
            }
            queens.AddFigure(new Queen(coords, figuresColor));

            //add KING
            coords.Y = (startPos == FigureStartPosition.Up) ? (0) : (8 - 1);
            if (startPos == FigureStartPosition.Up)
            {
                if (figuresColor == FigureColor.Black)
                    coords.Letter = FieldLetter.E;
                else
                    coords.Letter = FieldLetter.D;
            }
            else
            {
                if (figuresColor == FigureColor.Black)
                    coords.Letter = FieldLetter.D;
                else
                    coords.Letter = FieldLetter.E;
            }
			
			kings.AddFigure(coords, 
			                FigureType.King, figuresColor);
		}
		
		public void ReflectFigures(bool withColor)
        {
			pawns.ReflectCoordinates (withColor);
			rooks.ReflectCoordinates (withColor);
			bishops.ReflectCoordinates (withColor);
			horses.ReflectCoordinates (withColor);
			queens.ReflectCoordinates (withColor);
			kings.ReflectCoordinates (withColor);
        }
		
		public void AddFigure (Coordinates addWhere, FigureType figureType, FigureColor color)
		{
			switch (figureType)
			{
			case FigureType.Bishop:
				bishopDeletages.AddFigure (addWhere, figureType, color);
				break;
			case FigureType.Horse:
				horseDeletages.AddFigure (addWhere, figureType, color);
				break;
			case FigureType.King:
				kingDeletages.AddFigure (addWhere, figureType, color);
				break;
			case FigureType.Pawn:
				pawnDeletages.AddFigure (addWhere, figureType, color);
				break;
			case FigureType.Queen:
				queenDeletages.AddFigure (addWhere, figureType, color);
				break;
			case FigureType.Rook:
				rookDeletages.AddFigure (addWhere, figureType, color);
				break;
			default:
				throw new NotSupportedException ();
			}
			
			//handlers[figureType].AddFigure (addWhere, figureType, color);
		}
		
		public void MoveFigure (FigureType figureType, 
		                        Coordinates moveFrom, Coordinates moveWhere)
		{
			switch (figureType)
			{
			case FigureType.Bishop:
				bishopDeletages.MoveFigure (moveFrom, moveWhere);
				break;
			case FigureType.Horse:
				horseDeletages.MoveFigure (moveFrom, moveWhere);
				break;
			case FigureType.King:
				kingDeletages.MoveFigure (moveFrom, moveWhere);
				break;
			case FigureType.Pawn:
				pawnDeletages.MoveFigure (moveFrom, moveWhere);
				break;
			case FigureType.Queen:
				queenDeletages.MoveFigure (moveFrom, moveWhere);
				break;
			case FigureType.Rook:
				rookDeletages.MoveFigure (moveFrom, moveWhere);
				break;
			default:
				throw new NotSupportedException ();
			}
			
			//handlers[figureType].MoveFigure (moveFrom, moveWhere);
		}
		
		public void RemoveFigure (FigureType figureType, Coordinates coords)
		{
			switch (figureType)
			{
			case FigureType.Bishop:
				bishopDeletages.RemoveFigure (coords);
				break;
			case FigureType.Horse:
				horseDeletages.RemoveFigure (coords);
				break;
			case FigureType.King:
				kingDeletages.RemoveFigure (coords);
				break;
			case FigureType.Pawn:
				pawnDeletages.RemoveFigure (coords);
				break;
			case FigureType.Queen:
				queenDeletages.RemoveFigure (coords);
				break;
			case FigureType.Rook:
				rookDeletages.RemoveFigure (coords);
				break;
			default:
				throw new NotSupportedException ();
			}
			
			//handlers[figureType].RemoveFigure (coords);
		}
		
		public void RestoreFigure (FigureType figureType, Coordinates coords)
		{
			switch (figureType)
			{
			case FigureType.Bishop:
				bishopDeletages.RestoreFigure (coords);
				break;
			case FigureType.Horse:
				horseDeletages.RestoreFigure (coords);
				break;
			case FigureType.King:
				kingDeletages.RestoreFigure (coords);
				break;
			case FigureType.Pawn:
				pawnDeletages.RestoreFigure (coords);
				break;
			case FigureType.Queen:
				queenDeletages.RestoreFigure (coords);
				break;
			case FigureType.Rook:
				rookDeletages.RestoreFigure (coords);
				break;
			default:
				throw new NotSupportedException ();
			}
			
			//handlers[figureType].RestoreFigure (coords);
		}
		
		public void DestroyFigure (FigureType figureType, Coordinates coords)
		{
			switch (figureType)
			{
			case FigureType.Bishop:
				bishopDeletages.DestroyFigure (coords);
				break;
			case FigureType.Horse:
				horseDeletages.DestroyFigure (coords);
				break;
			case FigureType.King:
				kingDeletages.DestroyFigure (coords);
				break;
			case FigureType.Pawn:
				pawnDeletages.DestroyFigure (coords);
				break;
			case FigureType.Queen:
				queenDeletages.DestroyFigure (coords);
				break;
			case FigureType.Rook:
				rookDeletages.DestroyFigure (coords);
				break;
			default:
				throw new NotSupportedException ();
			}
			
			//handlers[figureType].DestroyFigure (coords);
		}
		
		public void SetBoolPropertyOfFigure (FigureType figureType, 
		                                      Coordinates coords, bool data)
		{
			switch (figureType)
			{
			case FigureType.Bishop:
				//bishopDeletages.SetBoolProperty (coords, data);
				break;
			case FigureType.Horse:
				//horseDeletages.SetBoolProperty (coords, data);
				break;
			case FigureType.King:
				kingDeletages.SetBoolProperty (coords, data);
				break;
			case FigureType.Pawn:
				//pawnDeletages.SetBoolProperty (coords, data);
				break;
			case FigureType.Queen:
				//queenDeletages.SetBoolProperty (coords, data);
				break;
			case FigureType.Rook:
				rookDeletages.SetBoolProperty (coords, data);
				break;
			default:
				throw new NotSupportedException ();
			}
			
			//handlers[figureType].SetBoolProperty (coords, data);
		}
		
		public bool GetBoolPropertyOfFigure (FigureType figureType, 
		                                      Coordinates coords)
		{
			switch (figureType)
			{
			case FigureType.Bishop:
				return false;
			case FigureType.Horse:
				return false;
			case FigureType.King:
				return kingDeletages.GetBoolProperty (coords);
			case FigureType.Pawn:
                return false;
			case FigureType.Queen:
				return false;
			case FigureType.Rook:
				return rookDeletages.GetBoolProperty (coords);
			default:
				throw new NotSupportedException ();
			}
			
			//return handlers[figureType].GetBoolProperty (coords);
		}
		
		public void Reflect (bool withColors)
		{
            /*
             * An optimization can be provided here.
             * We can check once is withColor variable
             * true, but not in every foreach statement.
             * But this method won't be called so many
             * times, that this spike can live
            */
			foreach (var figure in pawns)
			{
				figure.ReflectCoordinates ();
				if (withColors)
					figure.Color = figure.Color.GetOppositeColor ();
			}
			
			foreach (var figure in rooks)
			{
				figure.ReflectCoordinates ();
				if (withColors)
					figure.Color = figure.Color.GetOppositeColor ();
			}
			
			foreach (var figure in bishops)
			{
				figure.ReflectCoordinates ();
				if (withColors)
					figure.Color = figure.Color.GetOppositeColor ();
			}
			
			foreach (var figure in horses)
			{
				figure.ReflectCoordinates ();
				if (withColors)
					figure.Color = figure.Color.GetOppositeColor ();
			}
			
			foreach (var figure in queens)
			{
				figure.ReflectCoordinates ();
				if (withColors)
					figure.Color = figure.Color.GetOppositeColor ();
			}
			
			foreach (var figure in kings)
			{
				figure.ReflectCoordinates ();
				if (withColors)
					figure.Color = figure.Color.GetOppositeColor ();
			}
		}
	}
}
