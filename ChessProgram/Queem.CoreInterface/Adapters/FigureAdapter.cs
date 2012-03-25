using System;
using Queem.CoreInterface.Interface;

namespace Queem.CoreInterface.Adapters
{
	public class FigureAdapter : Queem.CoreInterface.Interface.Figure
	{
		protected GeneralFigure generalFigure;
		protected Coordinates coords;
	
		public FigureAdapter (GeneralFigure figure, Coordinates coordinates)
		{
			this.generalFigure = figure;
			this.coords = coordinates;
		}
		
		public override FigureType Type 
		{
			get 
			{
				return this.generalFigure.Type;
			}
		}
		
		public override FigureColor Color 
		{
			get 
			{
				return this.generalFigure.Color;
			}
		}
		
		public override Coordinates Coordinates 
		{
			get 
			{
				return this.coords;
			}
		}
	}
}

