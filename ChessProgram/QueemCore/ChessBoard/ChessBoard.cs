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
		
		protected PawnBitBoard pawns;
		protected KnightBitBoard knights;
		protected BishopBitBoard bishops;
		protected RookBitBoard rooks;
		protected QueenBitBoard queens;
		protected KingBitBoard king;
	
		public PlayerBoard (PlayerPosition playerPosition, Color playerColor)
		{
			this.position = playerPosition;
			this.color = playerColor;
		
			this.CreateBitBoards();
			this.FillBitBoards();
		}
		
		protected void CreateBitBoards()
		{
			this.pawns = new PawnBitBoard();
			this.knights = new KnightBitBoard();
			this.bishops = new BishopBitBoard();
			this.rooks = new RookBitBoard();
			this.queens = new QueenBitBoard();
			this.king = new KingBitBoard();
		}
		
		protected void FillBitBoards()
		{
		}
		
		protected void FillPawns()
		{
			var initialShuffler = new PawnFigureShuffler();
			initialShuffler.Init(this.pawns, this.position, this.color);
		}
	}
}

