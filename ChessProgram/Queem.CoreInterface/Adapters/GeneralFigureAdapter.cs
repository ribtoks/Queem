using System;
using Queem.Core;
using Queem.CoreInterface.Interface;

namespace Queem.CoreInterface.Adapters
{
	public class GeneralFigureAdapter : GeneralFigure
	{
		static GeneralFigureAdapter()
		{
			FigureTypes = new FigureType[7];
			FigureTypes[(int)Queem.Core.Figure.Bishop] = FigureType.Bishop;
			FigureTypes[(int)Queem.Core.Figure.King] = FigureType.King;
			FigureTypes[(int)Queem.Core.Figure.Knight] = FigureType.Knight;
			FigureTypes[(int)Queem.Core.Figure.Nobody] = FigureType.Nobody;
			FigureTypes[(int)Queem.Core.Figure.Pawn] = FigureType.Pawn;
			FigureTypes[(int)Queem.Core.Figure.Queen] = FigureType.Queen;
			FigureTypes[(int)Queem.Core.Figure.Rook] = FigureType.Rook;
		}
		
		public static FigureType[] FigureTypes;
	
		protected Color color;
		protected Queem.Core.Figure figure;
		
		public GeneralFigureAdapter (Color figureColor, Queem.Core.Figure figureType)
		{
			this.color = figureColor;
			this.figure = figureType;
		}
		
		public override FigureColor Color 
		{
			get 
			{
				if (this.color == Queem.Core.Color.White)
					return FigureColor.White;
				else
					return FigureColor.Black;
			}
		}
		
		public override FigureType Type 
		{
			get 
			{
				return FigureTypes[(int)this.figure];
			}
		}
	}
}

