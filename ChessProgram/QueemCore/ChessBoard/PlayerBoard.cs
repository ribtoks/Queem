using System;
using QueemCore;
using QueemCore.BitBoards;
using QueemCore.AttacksGenerators;
using QueemCore.MovesGenerators;
using System.Collections.Generic;
using QueemCore.BitBoards.Helpers;

namespace QueemCore.ChessBoard
{
	public class PlayerBoard
	{
		protected PlayerPosition position;	
		protected Color color;
		
		protected BitBoard[] bitboards;
		protected AttacksGenerator[] attacksGenerators;
		protected MovesGenerator[] moveGenerators;
		
		protected ulong allFigures;
		
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
			this.RefreshAllFiguresBoard();
			
			this.CreateAttacksGenerators();
			this.CreateMovesGenerators();
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
		
		protected void CreateAttacksGenerators()
		{
			this.attacksGenerators = new AttacksGenerator[6];
			for (int i = 0; i < 6; ++i)
				this.attacksGenerators[i] = AttacksGeneratorFactory.CreateGenerator((Figure)i);
		}
		
		protected void CreateMovesGenerators()
		{
			this.moveGenerators = new MovesGenerator[6];
			for (int i = 0; i < 6; ++i)
				this.moveGenerators[i] = MovesGeneratorFactory.CreateGenerator(
					(Figure)i,
					this.bitboards[i],
					this.attacksGenerators[i]);
		}
		
		public void DoMove(Move move, Figure figure)
		{
			this.bitboards[(int)figure].DoMove(move);
			
			this.allFigures |= (1UL << (int)move.To);
			this.allFigures &= (~(1UL << (int)move.From));
		}
		
		public void AddFigure(Square sq, Figure figure)
		{
			this.bitboards[(int)figure].SetBit(sq);
			this.allFigures |= (1UL << (int)sq);
		}
		
		public void RemoveFigure(Square sq, Figure figure)
		{			
			this.bitboards[(int)figure].UnsetBit(sq);
			this.allFigures &= (~(1UL << (int)sq));
		}
		
		public void RefreshAllFiguresBoard()
		{
			ulong result = 0;
			for (int i = 0; i < 6; ++i)
				result |= this.bitboards[i].GetInnerValue();
				
			this.allFigures = result;
		}
		
		public ulong GetAllFigures()
		{
			return this.allFigures;
		}
		
		protected List<List<Move>> GetKnightMoves(ulong opponentFigures)
		{
			var otherFigures = opponentFigures | this.allFigures;
			var mask = ~this.allFigures;
			return this.moveGenerators[(int)Figure.Knight].GetMoves(otherFigures, mask);
		}
	}
}

