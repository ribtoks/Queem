using System;

namespace BasicChessClasses
{
	public interface IReflectable
	{
		void Reflect();
		IReflectable GetReflected();
	}
	
	public class Coordinates : IReflectable
	{
		protected int y;
		protected FieldLetter letter;
		
		#region Properties

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int X
        {
        	get { return (int)letter; }
        	set { letter = (FieldLetter)value; }
        }

        public FieldLetter Letter
        {
            get { return letter; }
            set { letter = value; }
        }

        #endregion
		
		#region Constructors
		
		public Coordinates ()
		{
			y = 0;
			letter = FieldLetter.A;
		}
		
		public Coordinates (FieldLetter _letter, int _y)
		{
			y = _y;
			letter = _letter;
		}
		
		public Coordinates (int _x, int _y)
		{
			letter = (FieldLetter)_x;
			y = _y;
		}
		
		public Coordinates (Coordinates copy)
			: this(copy.letter, copy.y)
		{
		}
		
		public Coordinates (string line)
		{
			if (line.Length != 2)
				throw new WrongFormatException ("Input line is not <Coordinate> string");
			
			try
			{
				letter = (FieldLetter)(char.ToLower (line[0]) - 'a');
				y = line[1] - '0';
			}
			catch
			{
				throw new WrongFormatException ("Input line is not <Coordinate> string");
			}
		}
		
		#endregion

        public void Set(int _x, int _y)
        {
            letter = (FieldLetter)_x;
            y = _y;
        }
		
		public void Reflect ()
		{
			y = 8 - 1 - y;
			letter = (FieldLetter)(FieldLetter.H - letter);
		}
		
		public IReflectable GetReflected ()
		{
			return new Coordinates (
				(FieldLetter)(FieldLetter.H - letter), 
				8 - 1 - y);
		}
		
		/// <summary>
		/// Calculates the real chess coordinates
		/// </summary>
		/// <param name="whiteStartPos">Current position of white figures</param>
		/// <returns>A new chess coordinate</returns>
		public Coordinates GetReal (FigureStartPosition whiteCurrPosition)
		{
			if (whiteCurrPosition == FigureStartPosition.Down)
				return new Coordinates (letter, 8 - y);
			else
				return new Coordinates (
					(FieldLetter)((int)FieldLetter.H - (int)this.letter), 
					y + 1);
		}
		
		public override string ToString ()
		{
			return string.Format ("{0}{1}", 
				letter.ToString().ToLower(), y.ToString ());
		}
		
		public static bool operator == (Coordinates coord1, Coordinates coord2)
		{
			return ((coord1.y == coord2.y) && (coord1.letter == coord2.letter));
		}
		
		public static bool operator != (Coordinates coord1, Coordinates coord2)
		{
			return !(coord1 == coord2);
		}
		
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;

            Coordinates coord = (Coordinates)obj;
			if ((object)coord == null)
				return false;

            return ((coord.y == y) && (coord.letter == letter));
		}
		
		public override int GetHashCode ()
		{
			return y * 8 + (int)letter + 1;
		}
	}
	
	public class ChessMove
	{
		protected Coordinates start;
		protected Coordinates end;
		
		#region Constructors
		
		public ChessMove ()
		{
			start = new Coordinates ();
			end = new Coordinates ();
		}
		
		public ChessMove (Coordinates copyStart, Coordinates copyEnd)
		{
			start = new Coordinates (copyStart);
			end = new Coordinates (copyEnd);
		}
		
		public ChessMove (ChessMove copy)
			: this(copy.start, copy.end)
		{
		}
		
		public ChessMove (string line)
		{
			if (line.Length != 5)
				throw new WrongFormatException ("<Move> string has wrong format");
			
			string[] parts = line.Split ('-');
			
			if (parts.Length != 2)
				throw new WrongFormatException ("<Move> string has wrong format");
			
			start = new Coordinates (parts[0]);
			end = new Coordinates (parts[1]);
		}
		
		public ChessMove (string line, FigureStartPosition whitePos)
			: this(line)
		{
			if (whitePos == FigureStartPosition.Down) 
			{
				start.Y = 8 - start.Y;
				end.Y = 8 - end.Y;
			} 
			else 
			{
				start.Letter = (FieldLetter)((int)FieldLetter.H - (int)start.Letter);
				start.Y--;
				
				end.Letter = (FieldLetter)((int)FieldLetter.H - (int)end.Letter);
				end.Y--;
			}
		}		

		#endregion
		
		public Coordinates Start 
		{
			get { return start; }
			set 
			{
				start.Letter = value.Letter;
				start.Y = value.Y;
			}
		}
		
		public Coordinates End 
		{
			get { return end; }
			set 
			{
				end.Letter = value.Letter;
				end.Y = value.Y;
			}
		}
		
		public override string ToString ()
		{
			return string.Format ("{0}-{1}", start, end);
		}
		
		public ChessMove GetBackMove ()
		{
			return new ChessMove (end, start);
		}
		
		public string GetRealMove (FigureStartPosition whitePos)
		{
			return string.Format ("{0}-{1}", 
				start.GetReal (whitePos), end.GetReal (whitePos));
		}
		
		public void InnerReflect ()
		{
			start.Reflect ();
			end.Reflect ();
		}
		
		public static bool operator == (ChessMove move1, ChessMove move2)
		{
			return ((move1.start == move2.start) && (move1.end == move2.end));
		}
		
		public static bool operator != (ChessMove move1, ChessMove move2)
		{
			return !(move1 == move2);
		}
		
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;

            ChessMove move = (ChessMove)obj;
			if ((object)move == null)
				return false;

            return ((move.start == start) && (move.end == end));
		}
		
		public override int GetHashCode ()
		{
			return (start.GetHashCode () - 1) * 64 + end.GetHashCode ();
		}
	}
	
	public class PromotionMove : ChessMove
	{
		private PromotionType promotion;
		
		public PromotionMove ()
			: base()
		{
		}
		
		public PromotionMove (Coordinates copyStart, 
			Coordinates copyEnd, PromotionType promotionType)
			: base(copyStart, copyEnd)
		{
			promotion = promotionType;
		}
		
		public PromotionMove (PromotionMove copy)
			: this(copy.start, copy.end, copy.promotion)
		{
		}
		
		public PromotionMove (string line)
			: base(line.Substring (0, line.Length - 1))
		{
			char c = line[line.Length - 1];
			switch (c)
			{
			case 'b':
				promotion = PromotionType.Bishop;
				break;
			
			case 'h':
				promotion = PromotionType.Horse;
				break;
			
			case 'r':
				promotion = PromotionType.Rook;
				break;
			
			case 'q':
				promotion = PromotionType.Queen;
				break;
			
			default:
				break;
			}
		}
		
		public PromotionType Promotion 
		{
			get { return promotion; }
			// set { promotion = value; }
		}
		
		public override string ToString ()
		{
			return base.ToString () + char.ToLower (promotion.ToString ()[0]);
		}
	}
}
