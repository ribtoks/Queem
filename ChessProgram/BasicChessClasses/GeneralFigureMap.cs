using System;
using System.Collections.Generic;

namespace BasicChessClasses
{
	public class GeneralFigureMap<T> where T : new()
	{
		private T[][] arr;
		
		public GeneralFigureMap ()
		{
			int colorsCount = 2;
			int figureTypesCount = Enum.GetValues(typeof(FigureType)).Length;
			
			arr = new T[colorsCount][];
			
			for (int i = 0; i < 2; ++i)
				arr[i] = new T[figureTypesCount];
		}
		
		public T this[FigureColor color, FigureType type]
		{
			get { return arr[(int)color][(int)type]; }
			set { arr[(int)color][(int)type] = value; }
		}
		
		public T this[GeneralFigure figure]
		{
			get { return arr[(int)figure.Color][(int)figure.Type]; }
			set { arr[(int)figure.Color][(int)figure.Type] = value; }
		}
	}
	
	public class CoordinatesMap<T> where T : class, new()
	{
		private T[,] arr;
		
		public CoordinatesMap ()
		{
			arr = new T[8, 8];
		}
		
		public T this[int i, int j]
		{
			get { return arr[i, j]; }
			set { arr[i, j] = value; }
		}
		
		public T this[Coordinates coords]
		{
			get { return arr[coords.Y, coords.X]; }
			set { arr[coords.Y, coords.X] = value; }
		}
	}
}

