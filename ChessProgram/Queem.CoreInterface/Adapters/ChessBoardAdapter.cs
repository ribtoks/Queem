using System;
using QueemCore.ChessBoard;
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
				int square = j*8 + i;
				var board = this.innerProvider.PlayerBoard1;
				
				if (board.Figures[square] == QueemCore.Figure.Nobody)
					board = this.innerProvider.PlayerBoard2;
				
				return new GeneralFigureAdapter(board.FigureColor, board.Figures[square]);
			}
		}
	}
}

