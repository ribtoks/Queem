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
					
			var pawnGenerator = (PawnMovesGenerator)this.moveGenerators[(int)Figure.Pawn];
			pawnGenerator.PlayerPos = this.position;
		}
		
		protected ulong GetPawnAttacks()
		{
			if (this.position == PlayerPosition.Down)
				return this.Pawns.AnyUpAttacks();
			else
				return this.Pawns.AnyDownAttacks();
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
		
		protected ulong GetBishopsQueens()
		{
			return this.bitboards[(int)Figure.Bishop].GetInnerValue() | 
				this.bitboards[(int)Figure.Queen].GetInnerValue();
		}
		
		protected ulong GetRooksQueens()
		{
			return this.bitboards[(int)Figure.Rook].GetInnerValue() | 
				this.bitboards[(int)Figure.Queen].GetInnerValue();
		}
		
		#region GetMoves
				
		public List<Move[]> GetMoves(Figure figure, ulong opponentFigures)
		{
			var otherFigures = opponentFigures | this.allFigures;
			var mask = ~this.allFigures;
			return this.moveGenerators[(int)figure].GetMoves(otherFigures, mask);
		}
		
		public List<Move[]> GetAttacks(Figure figure, ulong opponentFigures)
		{
			var otherFigures = opponentFigures | this.allFigures;
			var mask = opponentFigures;
			return this.moveGenerators[(int)figure].GetMoves(otherFigures, mask);
		}
		
		public List<Move[]> GetChecks(Figure figure, ulong opponentKing)
		{
			var otherFigures = this.allFigures;
			var mask = opponentKing;
			return this.moveGenerators[(int)figure].GetMoves(otherFigures, mask);
		}
		
		public List<Move[]> GetKingMoves(PlayerBoard opponent)
		{
			var otherFigures = opponent.allFigures | this.allFigures;
			var mask = ~this.allFigures;
			var list = this.moveGenerators[(int)Figure.King].GetMoves(otherFigures, mask);
			
			// ---------------------------------------------
			
			// castlings stuff
			var allMasks = KingBitBoardHelper.CastlingMasks;
			ulong[] castlingMasks = allMasks[(int)this.color][(int)this.position];
			
			bool leftIsNotEmpty = (castlingMasks[0] & otherFigures) != 0;
			bool rightIsNotEmpty = (castlingMasks[1] & otherFigures) != 0;
			
			// check other figures
			if (leftIsNotEmpty && rightIsNotEmpty)
				return list;
				
			// check king can move
			if (this.King.AlreadyMoved != 0)
				return list;
				
			var rooks = this.Rooks;
			// if rooks cannot move
			if (rooks.GetInnerProperty() == 0)
				return list;
			
			bool noCheck;
			var squares = KingBitBoardHelper.CastlingSquares[(int)this.color][(int)this.position];
			int resultIndex = -1;
			// check checks
			if (leftIsNotEmpty && (rooks.LeftNotMoved != 0))
			{				
				noCheck = true;
				for (int i = 0; i < squares[0].Length; ++i)
				{
					if (this.IsUnderAttack(squares[0][i], opponent))
					{
						noCheck = false;
						break;
					}
				}
				if (noCheck)
					resultIndex = 0;									
			}
			
			if (rightIsNotEmpty && (rooks.RightNotMoved != 0))
			{				
				noCheck = true;
				for (int i = 0; i < squares[1].Length; ++i)
				{
					if (this.IsUnderAttack(squares[1][i], opponent))
					{
						noCheck = false;
						break;
					}
				}
				if (noCheck)
					resultIndex += 2;
			}
			
			if (resultIndex != -1)
				list.Add(KingBitBoardHelper.CastlingMoves[(int)this.color][(int)this.position][resultIndex]);
			
			return list;
		}
		
		#endregion
		
		protected bool IsUnderAttack(Square sq, PlayerBoard opponentBoard)
		{
			ulong occupied = 1UL << (int)sq;
			
			ulong pawnAttacks = opponentBoard.GetPawnAttacks();						
			if ((occupied & pawnAttacks) != 0) 
				return true;				
			
			ulong knights = opponentBoard.bitboards[(int)Figure.Knight].GetInnerValue();
			ulong king = opponentBoard.bitboards[(int)Figure.King].GetInnerValue();
			ulong opponentAllFigures = opponentBoard.GetAllFigures();			
			
			var knightAttackGenerator = this.attacksGenerators[(int)Figure.Knight];
			ulong occupiedKnightMoves = knightAttackGenerator.GetAttacks(sq, opponentAllFigures);
			if ((occupiedKnightMoves & knights) != 0)
				return true;
				
			var kingAttackGenerator = this.attacksGenerators[(int)Figure.King];
			ulong occupiedKingMoves = kingAttackGenerator.GetAttacks(sq, opponentAllFigures);
			if ((occupiedKingMoves & king) != 0)
				return true;
			
			ulong rooksQueens = opponentBoard.GetRooksQueens();
			ulong bishopsQueens = opponentBoard.GetBishopsQueens();
									
			var rookAttackGenerator = this.attacksGenerators[(int)Figure.Rook];			
			ulong occupiedRookMoves = rookAttackGenerator.GetAttacks(sq, opponentAllFigures);
			if ((occupiedRookMoves & rooksQueens) != 0)
				return true;
				
			var bishopAttackGenerator = this.attacksGenerators[(int)Figure.Bishop];
			ulong occupiedBishopMoves = bishopAttackGenerator.GetAttacks(sq, opponentAllFigures);
			if ((occupiedBishopMoves & bishopsQueens) != 0)
				return true;
			return false;
		}
		
		protected bool IsUnderCheck(PlayerBoard opponent)
		{
			return IsUnderAttack(this.King.GetSquare(), opponent);
		}
	}
}

