using System;
using System.Collections;
using System.Collections.Generic;

namespace BasicChessClasses
{
	public class FigureCollectionBase<T> : ICloneable, IEnumerable<T> where T: Figure, new()
	{
		protected CoordinatesMap<T> figures;
		protected List<T> figureList;
		protected Dictionary<T, bool> figureAccess;
		protected CoordinatesMap<T> killedFigures;
		protected int figuresCount = 0;
		
		public FigureCollectionBase ()
		{
			figures = new CoordinatesMap<T> ();
			killedFigures = new CoordinatesMap<T> ();
			
			//FillDictionaryCoords (figures);
			//FillDictionaryCoords (killedFigures);
			
			
			figureList = new List<T> ();
			figureAccess = new Dictionary<T, bool> ();
		}
		
		protected FigureCollectionBase (FigureCollectionBase<T> copy)
		{
			// this method make deep copying of Figures
			
			#region Creation 
			
			// copying dictionary
			figures = new CoordinatesMap<T> ();
			//FillDictionaryCoords (figures);
			
			figureList = new List<T> (copy.figureList.Count);
			
			killedFigures = new CoordinatesMap<T> ();
			//FillDictionaryCoords (killedFigures);
			
			figureAccess = new Dictionary<T, bool> (copy.figureAccess.Count);
			
			figuresCount = copy.figuresCount;
			
			#endregion
			
			// copying itself
			
			for (int i = 0; i < 8; ++i)
			{
				for (int j = 0; j < 8; ++j)					
				{
					var figure = copy.figures[i, j];
					
					T nextCopy = new T ();
					
					nextCopy.Coordinates = figure.Coordinates;
					nextCopy.Color = figure.Color;
					nextCopy.Type = figure.Type;
					nextCopy.hashcode = figure.hashcode;
				
					// paste copy into dictionary
					figures[i, j] = nextCopy;
					
					// paste copy into list
					figureList.Add (nextCopy);
					
					figureAccess.Add (nextCopy, true);
				}
			}
			
			for (int i = 0; i < 8; ++i)
			{
				for (int j = 0; j < 8; ++j)					
				{
					var figure = copy.killedFigures[i, j];
					
					T nextCopy = new T ();
					
					nextCopy.Coordinates = figure.Coordinates;
					nextCopy.Color = figure.Color;
					nextCopy.Type = figure.Type;
					nextCopy.hashcode = figure.hashcode;
				
					// paste copy into dictionary
					killedFigures[i, j] = nextCopy;
					
					figureAccess.Add (nextCopy, false);
				}
			}
		}
		
		protected void FillDictionaryCoords (Dictionary<Coordinates, T> figureDictionary)
		{
			figureDictionary.Clear ();
			
			List<FieldLetter> letters = new List<FieldLetter> {FieldLetter.A, FieldLetter.B, FieldLetter.C, 
				FieldLetter.D, FieldLetter.E, FieldLetter.F, FieldLetter.G, FieldLetter.H};
			
			List<int> vertical = new List<int> {0, 1, 2, 3, 4, 5, 6, 7};
			
			foreach (FieldLetter letter in letters)
			{
				foreach (int y in vertical)
				{
					figureDictionary.Add(new Coordinates(letter, y), null);
				}
			}
		}
		
		public void AddFigure (Coordinates addWhere,FigureType type, FigureColor color)
		{
            T figure = new T ();
			figure.Color = color;
			figure.Type = type;
			figure.Coordinates = addWhere;
				
			figures[addWhere] = figure;
			figureList.Add (figure);
			figureAccess[figure] = true;
			
			++figuresCount;
		}
		
		public void AddFigure (Figure addWhat)
		{
			T figure = new T ();
			figure.Color = addWhat.Color;
			figure.Type = addWhat.Type;
			figure.Coordinates = addWhat.Coordinates;
			
			figures[addWhat.Coordinates] = figure;
			figureList.Add (figure);
			figureAccess.Add (figure, true);
			
			++figuresCount;
		}
		
		public void MoveFigure (Coordinates moveFrom, Coordinates moveWhere)
		{
			T figure = figures[moveFrom];
			figures[moveFrom] = null;
			figures[moveWhere] = figure;
			
			figure.Coordinates = moveWhere;
		}
		
		public void RemoveFigure (Coordinates coords)
		{
			T figure = figures[coords];
			
			figureAccess[figure] = false;
			figures[coords] = null;
			
			killedFigures[coords] = figure;
			
			--figuresCount;
		}
		
		public void DestroyFigure (Coordinates coords)
		{
			// completely destroy figure from everywhere
			
			T figure = figures[coords];
			
			figureAccess[figure] = false;
			figures[coords] = null;
			
			// in most cases figure would be
			// the last figure in list
			
			int i = figureList.Count - 1;
			
			while (i > 0)
			{
				if (figureList[i] == figure)
					break;
				
				--i;
			}
			
			if (i == -1)
				return;
			
			figureList.RemoveAt (i);
			--figuresCount;
		}
		
		public void RestoreFigure (Coordinates coords)
		{
			T figure = killedFigures[coords];
			killedFigures[coords] = null;
			
			figureAccess[figure] = true;
			figures[coords] = figure;
			
			++figuresCount;
		}
		
		public void ReflectCoordinates(bool withColor)
		{
			for (int i = 0; i < figureList.Count; ++i)
			{
				figureList[i].ReflectCoordinates ();
				
				if (withColor)
					figureList[i].Color = 
						(figureList[i].Color == FigureColor.Black) ? 
						FigureColor.White : FigureColor.Black;
			}
		}
		
		public T this[Coordinates coords]
		{
			get { return figures[coords]; }
			internal set { figures[coords] = value; }
		}
		
		public virtual bool GetBoolProperty (Coordinates coordinates)
		{
			return false;
		}
		
		public virtual void SetBoolProperty (Coordinates coordinate, bool data)
		{
		}
		
		
		public int Count
		{
			get { return figuresCount; }
		}
		
		
		#region IEnumerable<T> implementation
		
		public IEnumerator<T> GetEnumerator ()
		{
			#if DEBUG
			if (figureList.Count != figureAccess.Count)
				throw new ApplicationException ("Collections are not equal");
#endif
			
			for (int i = 0; i < figureList.Count; ++i)
			{
				if (figureAccess[ figureList[i] ])
					yield return figureList[i];
			}

		}
		
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
		
		#endregion
		
		#region IClonable implementation
		
		public object Clone ()
		{
			return new FigureCollectionBase<T> (this);
		}
		
		object ICloneable.Clone ()
		{
			return Clone ();
		}
		
		#endregion
	}
	
	public class FigureCollection<T> : FigureCollectionBase<T>, ICloneable where T: Figure, new()
	{
		public FigureCollection ()
			: base()
		{
		}
		
		protected FigureCollection (FigureCollection<T> copy)
			:base(copy)
		{
		}
		
		#region IClonable implementation
		
		new public object Clone ()
		{
			return new FigureCollection<T> (this);
		}
		
		object ICloneable.Clone ()
		{
			return Clone ();
		}
		
		#endregion
	}
	
	public class RookCollection : FigureCollectionBase<Rook>, ICloneable
	{
		public RookCollection ()
			: base()
		{
		}
		
		protected RookCollection (RookCollection copy)
			: base (copy)
		{
		}
		
		public override void SetBoolProperty (Coordinates coordinate, bool data)
		{
			figures[coordinate].CanDoCastling = data;
		}
		
		public override bool GetBoolProperty (Coordinates coordinates)
		{
			return figures[coordinates].CanDoCastling;
		}
		
		#region IClonable implementation
		
		new public object Clone ()
		{
			return new RookCollection (this);
		}
		
		object ICloneable.Clone ()
		{
			return Clone ();
		}
		
		#endregion
	}
	
	public class PawnCollection : FigureCollectionBase<Pawn>, ICloneable
	{
		public PawnCollection ()
			: base()
		{
		}
		
		protected PawnCollection (PawnCollection copy)
			:base (copy)
		{
		}
		
		public override void SetBoolProperty (Coordinates coordinate, bool data)
		{
			figures[coordinate].IsInPassing = data;
		}
		
		public override bool GetBoolProperty (Coordinates coordinates)
		{
			return figures[coordinates].IsInPassing;
		}
		
		#region IClonable implementation
		
		new public object Clone ()
		{
			return new PawnCollection (this);
		}
		
		object ICloneable.Clone ()
		{
			return Clone ();
		}
		
		#endregion
	}
	
	public class KingCollection : FigureCollectionBase<King>, ICloneable
	{
		public KingCollection ()
			: base()
		{
		}
		
		protected KingCollection (KingCollection copy)
			:base (copy)
		{
		}
		
		public override void SetBoolProperty (Coordinates coordinate, bool data)
		{
			figures[coordinate].CanDoCastling = data;
		}
		
		public override bool GetBoolProperty (Coordinates coordinates)
		{
			return figures[coordinates].CanDoCastling;
		}
		
		public King King
		{
			get { return figureList[0]; }
		}
		
		#region IClonable implementation
		
		new public object Clone ()
		{
			return new KingCollection (this);
		}
		
		object ICloneable.Clone ()
		{
			return Clone ();
		}
		
		#endregion
	}
}

