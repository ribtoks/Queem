using System;
using QueemCore;
using QueemCore.BitBoards;
using QueemCore.AttacksGenerators;

namespace QueemCore.ChessBoard
{
	public class PlayerBoard
	{
		protected PlayerPosition position;	
		protected Color color;
		protected BitBoard[] bitboards;
		
		#region Boards
		
		public PawnBitBoard Pawns
		{
			get { return (PawnBitBoard)this.bitboards[(int)Figure.Pawn]; }
		}
		
		public KnightBitBoard Knights
		{
			get { return (KnightBitBoard)this.bitboards[(int)Figure.Knight]; }
		}
		
		public BishopBitBoard Bishops
		{
			get { return (BishopBitBoard)this.bitboards[(int)Figure.Bishop]; }
		}
		
		public RookBitBoard Rooks
		{
			get { return (RookBitBoard)this.bitboards[(int)Figure.Rook]; }
		}
		
		public QueenBitBoard Queens
		{
			get { return (QueenBitBoard)this.bitboards[(int)Figure.Queen]; }
		}
		
		public KingBitBoard King
		{
			get { return (KingBitBoard)this.bitboards[(int)Figure.King]; }
		}
		
		#endregion
					
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

