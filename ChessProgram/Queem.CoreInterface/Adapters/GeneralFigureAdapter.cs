using System;
using QueemCore;
using Queem.CoreInterface.Interface;

namespace Queem.CoreInterface.Adapters
{
	public class GeneralFigureAdapter : GeneralFigure
	{
		static GeneralFigureAdapter()
		{
			FigureTypes = new FigureType[7];
			FigureTypes[(int)QueemCore.Figure.Bishop] = FigureType.Bishop;
			FigureTypes[(int)QueemCore.Figure.King] = FigureType.King;
			FigureTypes[(int)QueemCore.Figure.Knight] = FigureType.Knight;
			FigureTypes[(int)QueemCore.Figure.Nobody] = FigureType.Nobody;
			FigureTypes[(int)QueemCore.Figure.Pawn] = FigureType.Pawn;
			FigureTypes[(int)QueemCore.Figure.Queen] = FigureType.Queen;
			FigureTypes[(int)QueemCore.Figure.Rook] = FigureType.Rook;
		}
		
		public static FigureType[] FigureTypes;
	
		protected Color color;
		protected QueemCore.Figure figure;
		
		public GeneralFigureAdapter (Color figureColor, QueemCore.Figure figureType)
		{
			this.color = figureColor;
			this.figure = figureType;
		}
		
		public override FigureColor Color 
		{
			get 
			{
				if (this.color == QueemCore.Color.White)
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

