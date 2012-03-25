using System;
using Queem.CoreInterface.Interface;
using Queem.Core.ChessBoard;
using System.Collections.Generic;
using Queem.Core;

namespace Queem.CoreInterface.Adapters
{
	public class PlayerAdapter : Player
	{
		protected PlayerBoard innerBoard;
		protected Color color;
	
		public PlayerAdapter(PlayerBoard board)
		{
			this.innerBoard = board;
			this.color = board.FigureColor;
		}
		
		public override IEnumerable<Queem.CoreInterface.Interface.Figure> FiguresManager 
		{
			get 
			{
				var figures = this.innerBoard.Figures;
				
				for (int i = 0; i < figures.Length; ++i)
				{
				
					if (figures[i] != Queem.Core.Figure.Nobody)
						yield return new FigureAdapter(
							new GeneralFigureAdapter(this.innerBoard.FigureColor, figures[i]),
							new CoordinatesAdapter((Square)i));
				}
			}
		}
		
		public override FigureColor FiguresColor 
		{
			get 
			{
				if (this.color == Color.White)
					return FigureColor.White;
				return FigureColor.Black;
			}
		}
	}
}

