using System;
using QueemCore;
using QueemCore.BitBoard;
using QueemCore.AttacksGenerators;

namespace QueemCore.ChessBoard
{
	public class PlayerBoard
	{
		protected PlayerPosition position;	
		protected Color color;
		protected BitBoard[] bitboards;
			
		public PlayerBoard (PlayerPosition playerPosition, Color playerColor)
		{
			this.position = playerPosition;
			this.color = playerColor;
			
			this.CreateBitBoards();	
			this.FillBitBoards();
		}
		
		protected void CreateBitBoards()
		{
			this.bitboards = new BitBoard[6];
			
			for (int i = 0; i < 6; ++i)
				this.bitboards[i] = BitBoardFactory.CreateBitBoard((Figure)i);
		}
		
		protected void FillBitBoards()
		{
			for (int i = 0; i < 6; ++i)
				BoardInitializer.Shufflers[i].Init(this.bitboards[i], 
												   this.position,
												   this.color);
		}		
	}
}

