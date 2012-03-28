using System;
using Queem.Core.ChessBoard;
using Queem.CoreInterface.Interface;

namespace Queem.CoreInterface.Adapters
{
	public class ChessBoardAdapter : ChessBoard
	{
		protected GameProvider innerProvider;
	
		public ChessBoardAdapter (GameProvider provider)
		{
			this.innerProvider = provider;
		}
		
		public override GeneralFigure this[int i, int j] 
		{
			get 
			{
				int square = (7 - j)*8 + i;
				var board = this.innerProvider.PlayerBoard1;
				
				if (board.Figures[square] == Queem.Core.Figure.Nobody)
					board = this.innerProvider.PlayerBoard2;
				
				return new GeneralFigureAdapter(board.FigureColor, board.Figures[square]);
			}
		}

        public override GeneralFigure this[Coordinates coords]
        {
            get 
            {
                return this[coords.X, coords.Y];
            }
        }
	}
}

