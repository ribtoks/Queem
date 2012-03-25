using System;
using Queem.CoreInterface.Interface;
using QueemCore.ChessBoard;
using System.Collections.Generic;

namespace Queem.CoreInterface.Adapters
{
	public class MovesProviderAdapter : MovesProvider
	{
		protected ChessBoard board;
		protected GameProvider innerProvider;
		protected PlayerAdapter player1;
		protected PlayerAdapter player2;
	
		public MovesProviderAdapter (GameProvider provider) 
		{
			this.board = new ChessBoardAdapter(provider);
			this.innerProvider = provider;
			
			this.player1 = new PlayerAdapter(provider.PlayerBoard1);
			this.player2 = new PlayerAdapter(provider.PlayerBoard2);
		}
		
		public override ChessBoard ChessBoard 
		{
			get 
			{
				return this.board;
			}
		}
		
		public override Player Player1 
		{
			get 
			{
				return this.player1;
			}
		}
		
		public override Player Player2 
		{
			get 
			{
				return this.player2;
			}
		}
	}
}

