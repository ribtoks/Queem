using System;
using System.Collections.Generic;

namespace BasicChessClasses
{
	/// <summary>
	/// Class, that maps on which cells i can move my figure
	/// First FigureColor parameter in dictionary is a color
	/// of my own figures
	/// </summary>
	public class FigureMapper
	{
		protected GeneralFigureMap<byte> whiteFiguresMap;
		protected GeneralFigureMap<byte> blackFiguresMap;
		protected GeneralFigureMap<byte> whiteAttackingFiguresMap;
		protected GeneralFigureMap<byte> blackAttackingFiguresMap;
		
		public FigureMapper ()
		{
			//TODO run in parallel thread...
			Initialize ();
		}
		
		protected void Initialize ()
		{
			whiteFiguresMap = new GeneralFigureMap<byte> ();
			blackFiguresMap = new GeneralFigureMap<byte> ();
			
			InitializeUsualMapForColor (whiteFiguresMap, FigureColor.White);
			InitializeUsualMapForColor (blackFiguresMap, FigureColor.Black);
			
			// -----------------------------------------------------------------			
			
			whiteAttackingFiguresMap = new GeneralFigureMap<byte> ();
			blackAttackingFiguresMap = new GeneralFigureMap<byte> ();
			
			InitializeAttackingMapForColor (whiteAttackingFiguresMap, FigureColor.White);
			InitializeAttackingMapForColor (blackAttackingFiguresMap, FigureColor.Black);
		}
		
		protected void InitializeUsualMapForColor (GeneralFigureMap<byte> map, 
		                                           FigureColor color)
		{
			foreach (GeneralFigure figure in GeneralFigure.AllPossibleFigures)
			{
				// cannot go outside the board
				if (figure.Type == FigureType.Null)
				{
					map[figure] = 0;
					continue;
				}
				
				// can go to empty cell
				if (figure.Type == FigureType.Nobody)
				{
					map[figure] = 1;
					continue;
				}
				
				// cannot go to cells, where stands
				// figure with the same color
				if (figure.Color == color)
					map[figure] = 0;
				else
					// and can capture the opponent
					map[figure] = 1;
			}
		}
		
		protected void InitializeAttackingMapForColor (GeneralFigureMap<byte> map, 
		                                           FigureColor color)
		{
			foreach (GeneralFigure figure in GeneralFigure.AllPossibleFigures) 
			{
				// cannot go outside the board
				if (figure.Type == FigureType.Null) 
				{
					map[figure] = 0;
					continue;
				}
				
				// can go to empty cell
				if (figure.Type == FigureType.Nobody) 
				{
					map[figure] = 0;
					continue;
				}
				
				// cannot go to cells, where stands
				// figure with the same color
				if (figure.Color == color)
					map[figure] = 0;
				else
					// just can capture the opponent
					map[figure] = 1;
			}
		}
		
		public GeneralFigureMap<byte> GetFiguresMap (FigureColor color)
		{
			switch (color)
			{
			case FigureColor.White:
				return whiteFiguresMap;
			case FigureColor.Black:
				return blackFiguresMap;
			default:
				throw new NotImplementedException ();
			}
		}
		
		public GeneralFigureMap<byte> GetAttackingFiguresMap (FigureColor color)
		{
			switch (color)
			{
			case FigureColor.White:
				return whiteAttackingFiguresMap;
			case FigureColor.Black:
				return blackAttackingFiguresMap;
			default:
				throw new NotImplementedException ();
			}
		}
	}
	
	/// <summary>
	/// Class, that maps which figures can control my cell
	/// - on diagonals (on distance > 1) queens and bishops can
	/// - on verticals rooks and queens can
	/// First FigureColor in dictionary is color of my figures
	/// </summary>
	public class CellControlFiguresMap
	{
		protected GeneralFigureMap<byte> whiteDiagonalsMap;
		protected GeneralFigureMap<byte> blackDiagonalsMap;
		
		// vertical and horisontal
		protected GeneralFigureMap<byte> whiteVHMap;
		protected GeneralFigureMap<byte> blackVHMap;
		
		// obsolete
		protected GeneralFigureMap<byte> whiteHorseMap;
		protected GeneralFigureMap<byte> blackHorseMap;
		
		public CellControlFiguresMap ()
		{
			Initialize ();
		}
		
		protected void InitializeDiagonalMap (GeneralFigureMap<byte> map, 
		                                           FigureColor color)
		{
			foreach (GeneralFigure figure in GeneralFigure.AllPossibleFigures)
			{
				// cannot go outside the board
				if (figure.Type == FigureType.Null)
				{
					map[figure] = 0;
					continue;
				}
				
				// nobody cannot control my cell
				if (figure.Type == FigureType.Nobody)
				{
					map[figure] = 0;
					continue;
				}
				
				// figures with other color
				// cannot control my cell
				if (figure.Color != color)
					map[figure] = 0;
				else
				{
					// only queens and bishops can (from large distance)
					if ((figure.Type == FigureType.Queen) || (figure.Type == FigureType.Bishop))
						map[figure] = 1;
					else
						map[figure] = 0;
				}
			}
		}
		
		protected void InitializeVHMap (GeneralFigureMap<byte> map, 
		                                           FigureColor color)
		{
			foreach (GeneralFigure figure in GeneralFigure.AllPossibleFigures)
			{
				// cannot go outside the board
				if (figure.Type == FigureType.Null) 
				{
					map[figure] = 0;
					continue;
				}
				
				// nobody cannot contorol my cell
				if (figure.Type == FigureType.Nobody) 
				{
					map[figure] = 0;
					continue;
				}
				
				// figures with other color
				// cannot control my cell
				if (figure.Color != color)
					map[figure] = 0;
				else
				{
					// only queens and bishops can (from large distance)
					if ((figure.Type == FigureType.Queen) || (figure.Type == FigureType.Rook))
						map[figure] = 1;
					else
						map[figure] = 0;
				}
			}
		}
		
		protected void InitializeHorseMap (GeneralFigureMap<byte> map, 
		                                           FigureColor color)
		{
			foreach (GeneralFigure figure in GeneralFigure.AllPossibleFigures) 
			{
				// cannot go outside the board
				if (figure.Type == FigureType.Null) 
				{
					map[figure] = 0;
					continue;
				}
				
				// nobody cannot contorol my cell
				if (figure.Type == FigureType.Nobody) 
				{
					map[figure] = 0;
					continue;
				}
				
				// figures with other color
				// cannot control my cell
				if (figure.Color != color)
					map[figure] = 0;
				else
				{
					// only horses can
					if (figure.Type == FigureType.Horse)
						map[figure] = 1;
					else
						map[figure] = 0;
				}
			}
		}
		
		protected void Initialize ()
		{
			whiteDiagonalsMap = new GeneralFigureMap<byte> ();
			blackDiagonalsMap = new GeneralFigureMap<byte> ();
			
			InitializeDiagonalMap (whiteDiagonalsMap, FigureColor.White);
			InitializeDiagonalMap (blackDiagonalsMap, FigureColor.Black);
			
			// -----------------------------------------------------------
			
			whiteVHMap = new GeneralFigureMap<byte> ();
			blackVHMap = new GeneralFigureMap<byte> ();
			
			InitializeVHMap (whiteVHMap, FigureColor.White);
			InitializeVHMap (blackVHMap, FigureColor.Black);
			
			// -----------------------------------------------------------
			
			whiteHorseMap = new GeneralFigureMap<byte> ();
			blackHorseMap = new GeneralFigureMap<byte> ();
			
			InitializeHorseMap (whiteHorseMap, FigureColor.White);
			InitializeHorseMap (blackHorseMap, FigureColor.Black);
		}
		
		public GeneralFigureMap<byte> GetDiagonalsMap (FigureColor color)
		{
			switch (color)
			{
			case FigureColor.White:
				return whiteDiagonalsMap;
			case FigureColor.Black:
				return blackDiagonalsMap;
			default:
				throw new NotImplementedException ();
			}
		}
		
		public GeneralFigureMap<byte> GetVHMap (FigureColor color)
		{
			switch (color)
			{
			case FigureColor.White:
				return whiteVHMap;
			case FigureColor.Black:
				return blackVHMap;
			default:
				throw new NotImplementedException ();
			}
		}
		
		public GeneralFigureMap<byte> GetHorseMap (FigureColor color)
		{
			switch (color)
			{
			case FigureColor.White:
				return whiteHorseMap;
			case FigureColor.Black:
				return blackHorseMap;
			default:
				throw new NotImplementedException ();
			}
		}
	}
}
