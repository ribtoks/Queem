using System;
using System.Collections.Generic;

namespace BasicChessClasses
{
	public class GeneralFigure
	{
		public static GeneralFigure NullFigure = 
			new GeneralFigure (FigureType.Null, FigureColor.Black);
		
		private static List<GeneralFigure> allPossibleFigures;
		
		static GeneralFigure ()
		{
			allPossibleFigures = new List<GeneralFigure> ();
			
			foreach (var color in Enum.GetValues (typeof(FigureColor)))
			{
				foreach (var type in Enum.GetValues (typeof(FigureType)))
				{
					allPossibleFigures.Add (new GeneralFigure ((FigureType)type, (FigureColor)color));
				}
			}
		}
		
		protected FigureType type;
		protected FigureColor color;
		
		public GeneralFigure ()
			: this(FigureType.Nobody, FigureColor.Black)
		{
		}
		
		public GeneralFigure (FigureType fType, FigureColor fColor)
		{
			type = fType;
			color = fColor;
		}
		
		public GeneralFigure (GeneralFigure copy)
			: this(copy.type, copy.color)
		{
		}
		
		public void ReAssign (GeneralFigure copy)
		{
			type = copy.type;
			color = copy.color;
		}
		
		public void ReflectColor ()
		{
			if (color == FigureColor.Black)
				color = FigureColor.White;
			color = FigureColor.Black;
		}
		
		public override string ToString ()
		{
			return string.Format ("{0} {1}", color.ToString (), type.ToString ());
		}
		
		public static bool operator == (GeneralFigure gf1, GeneralFigure gf2)
		{
			return ((gf1.type == gf2.type) && (gf1.color == gf2.color));
		}
		
		public static bool operator != (GeneralFigure gf1, GeneralFigure gf2)
		{
			return !(gf1 == gf2);
		}
		
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			
			GeneralFigure gf = (GeneralFigure)obj;
			if ((object)gf == null)
				return false;
			
			return ((gf.type == type) && (gf.color == color));
		}
		
		public override int GetHashCode ()
		{
			return (int)type * ((int)color + 20);
		}

		public FigureType Type 
		{
			get { return type; }
			set { type = value; }
		}

		public FigureColor Color 
		{
			get { return color; }
			set { color = value; }
		}
		
		public static List<GeneralFigure> AllPossibleFigures
		{
			get { return allPossibleFigures; }
		}
	}
	
	public abstract class Figure
	{
		protected Coordinates coords;
		protected GeneralFigure figure;
		protected internal int hashcode;
		
		public Figure ()
		{
            figure = new GeneralFigure();
            coords = new Coordinates();
		}
		
		public Figure (Coordinates copyCoords, FigureType ftype, FigureColor fcolor)
		{
			coords = new Coordinates (copyCoords);
			figure = new GeneralFigure (ftype, fcolor);
			
			// save new hash code for this figure
			hashcode = FiguresHashes.GetNextHash ();
		}
		
		public Figure (Coordinates copyCoords, GeneralFigure copyFigure)
			: this(copyCoords, copyFigure.Type, copyFigure.Color)
		{
		}
		
		public Figure (Figure copyFigure)
			: this(copyFigure.coords, copyFigure.figure)
		{
			hashcode = copyFigure.hashcode;
		}
		
		public void ReflectCoordinates ()
		{
			coords.Reflect ();
		}
		
		public Coordinates Coordinates
		{
			get { return coords; }
			set 
			{
				coords.Y = value.Y;
				coords.Letter = value.Letter;
			}
		}
		
		public FigureColor Color
		{
			get { return figure.Color; }
			set { figure.Color = value; }
		}
		
		public FigureType Type
		{
			get { return figure.Type; }
			set { figure.Type = value; }
		}
		
		#region Operator overloading

        public static bool operator ==(Figure figure1, Figure figure2)
        {
            if (figure1.coords != figure2.coords)
                return false;
            if (figure1.Color != figure2.Color)
                return false;
			
            return true;
        }

        public static bool operator !=(Figure figure1, Figure figure2)
        {
            return !(figure1 == figure2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Figure figure = obj as Figure;
            if ((object)figure == null)
                return false;

            if (figure.coords != this.coords)
                return false;
            if (figure.Color != this.Color)
                return false;
			
            return true;
        }
		
		public override int GetHashCode ()
		{
			return hashcode;
		}
		
        #endregion
	}
	
	public class Pawn : Figure
	{
		protected bool isInPassing = false;
		
		public Pawn ()
			: base()
		{
		}
		
		public Pawn (Coordinates coords, FigureColor color)
			: base(coords, FigureType.Pawn, color)
		{
		}
		
		public Pawn (Pawn copy)
			: base(copy)
		{
			isInPassing = copy.isInPassing;
		}
		
		public bool IsInPassing 
		{
			get { return this.isInPassing; }
			set { isInPassing = value; }
		}
		
		#region Operator overloading

        public static bool operator ==(Pawn pawn1, Pawn pawn2)
        {
            if (pawn1.Coordinates != pawn2.Coordinates)
                return false;
            if (pawn1.Color != pawn2.Color)
                return false;
			if (pawn1.isInPassing != pawn2.isInPassing)
				return false;
			
            return true;
        }

        public static bool operator !=(Pawn pawn1, Pawn pawn2)
        {
            return !(pawn1 == pawn2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Pawn pawn = obj as Pawn;
            if ((object)pawn == null)
                return false;

            if (pawn.coords != this.coords)
                return false;
            if (pawn.Color != this.Color)
                return false;
			if (pawn.isInPassing != this.isInPassing)
				return false;
			
            return true;
        }
		
		public override int GetHashCode ()
		{
			return hashcode;
		}
		
        #endregion
	}
	
	public class Bishop : Figure
	{
		public Bishop ()
			: base()
		{
		}
		
		public Bishop (Coordinates coords, FigureColor color)
			: base(coords, FigureType.Bishop, color)
		{
		}
		
		public Bishop (Bishop copy)
			: base(copy)
		{			
		}
	}
	
	public class Horse : Figure
	{
		public Horse ()
			: base()
		{
		}
		
		public Horse (Coordinates coords, FigureColor color) : 
			base(coords, FigureType.Horse, color)
		{
		}

		public Horse (Horse copy) : 
			base(copy)
		{
		}
	}
	
	public class Rook : Figure
	{
		public Rook ()
			: base()
		{
		}
		
		protected bool canDoCastling = true;		

		public Rook (Coordinates coords, FigureColor color) : 
			base(coords, FigureType.Rook, color)
		{
		}

		public Rook (Rook copy) 
			: base(copy)
		{
		}
		
		public bool CanDoCastling 
		{
			get { return this.canDoCastling; }
			set { canDoCastling = value; }
		}
		
		#region Operator overloading

        public static bool operator ==(Rook rook1, Rook rook2)
        {
            if (rook1.Coordinates != rook2.Coordinates)
                return false;
            if (rook1.Color != rook2.Color)
                return false;
			if (rook1.canDoCastling != rook2.canDoCastling)
				return false;
			
            return true;
        }

        public static bool operator !=(Rook rook1, Rook rook2)
        {
            return !(rook1 == rook2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Rook rook = obj as Rook;
            if ((object)rook == null)
                return false;

            if (rook.coords != this.coords)
                return false;
            if (rook.Color != this.Color)
                return false;
			if (rook.canDoCastling != this.canDoCastling)
				return false;
			
            return true;
        }
		
		public override int GetHashCode ()
		{
			return hashcode;
		}
		
        #endregion
	}
	
	public class Queen : Figure
	{
		public Queen ()
			: base()
		{
		}
		
		public Queen (Coordinates coords, FigureColor color) : 
			base(coords, FigureType.Queen, color)
		{
		}

		public Queen (Queen copy) : base(copy)
		{
		}
	}
	
	public class King : Figure
	{
		public King ()
			: base()
		{
		}
		
		protected bool canDoCastling = true;
		
		public King (Coordinates coords, FigureColor color) :
			base(coords, FigureType.King, color)
		{
		}

		public King (King copy) 
			: base(copy)
		{
		}
		
		public bool CanDoCastling 
		{
			get { return this.canDoCastling; }
			set { canDoCastling = value; }
		}
		
		#region Operator overloading

        public static bool operator ==(King king1, King king2)
        {
            if (king1.Coordinates != king2.Coordinates)
                return false;
            if (king1.Color != king2.Color)
                return false;
			if (king1.canDoCastling != king2.canDoCastling)
				return false;
			
            return true;
        }

        public static bool operator !=(King king1, King king2)
        {
            return !(king1 == king2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            King king = obj as King;
            if ((object)king == null)
                return false;

            if (king.coords != this.coords)
                return false;
            if (king.Color != this.Color)
                return false;
			if (king.canDoCastling != this.canDoCastling)
				return false;
			
            return true;
        }
		
		public override int GetHashCode ()
		{
			return hashcode;
		}
		
        #endregion
	}
}
