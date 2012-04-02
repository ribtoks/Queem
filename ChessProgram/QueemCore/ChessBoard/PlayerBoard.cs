using System;
using Queem.Core;
using Queem.Core.BitBoards;
using Queem.Core.AttacksGenerators;
using Queem.Core.MovesGenerators;
using System.Collections.Generic;
using Queem.Core.BitBoards.Helpers;

namespace Queem.Core.ChessBoard
{
	public enum MovesMask { AllMoves, Attacks}
	
	public class PlayerBoard
	{
		protected PlayerPosition position;	
		protected Color color;
		
		protected BitBoard[] bitboards;
		protected AttacksGenerator[] attacksGenerators;
		protected MovesGenerator[] moveGenerators;
		
		protected Figure[] figures;
		protected ulong allFigures;
		
		public static readonly Figure[] KnightBishopRookQueen = 
			new Figure[] {Figure.Knight, Figure.Bishop, Figure.Rook, Figure.Queen};
		
		public Figure[] Figures
		{
			get { return this.figures; }
		}
		
		public Color FigureColor
		{
			get { return this.color; }
		}
		
		public PlayerPosition Position
		{
			get { return this.position; }
		}
		
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
					
		#region Initializations
		
		public PlayerBoard (PlayerPosition playerPosition, Color playerColor)
		{
			this.position = playerPosition;
			this.color = playerColor;

            this.ResetAll();
		}

        public void ResetAll()
        {
            this.CreateBitBoards();
            this.FillBitBoards();
            this.RefreshAllFiguresBoard();

            this.CreateFiguresArray();
            this.FillFiguresArray();

            this.CreateAttacksGenerators();
            this.CreateMovesGenerators();
        }
		
		protected void CreateFiguresArray()
		{
			this.figures = new Figure[64];
			for (int i = 0; i < this.figures.Length; ++i)
				this.figures[i] = Figure.Nobody;
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
				BoardInitializer.Shufflers[i].Init((sq) => this.bitboards[i].SetBit(sq), 
												   this.position,
												   this.color);
		}
		
		protected void FillFiguresArray()
		{
			for (int i = 0; i < 6; ++i)
				BoardInitializer.Shufflers[i].Init((sq) => this.figures[(int)sq] = (Figure)i, 
												   this.position,
												   this.color);
		}
		
		protected void CreateAttacksGenerators()
		{
			this.attacksGenerators = new AttacksGenerator[6];
			for (int i = 0; i < 6; ++i)
				this.attacksGenerators[i] = AttacksGeneratorFactory.CreateGenerator((Figure)i, this.position);
			
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
		
		#endregion
		
		protected ulong GetPawnAttacks()
		{
			if (this.position == PlayerPosition.Down)
				return this.Pawns.AnyUpAttacks();
			else
				return this.Pawns.AnyDownAttacks();
		}
		
		public void ProcessMove(Move move, Figure figure)
		{
			this.bitboards[(int)figure].DoMove(move);
			
			this.allFigures |= (1UL << (int)move.To);
			this.allFigures &= (~(1UL << (int)move.From));
			
			this.figures[(int)move.From] = Figure.Nobody;
			this.figures[(int)move.To] = figure;
		}
		
		public void CancelMove(int sqFrom, int sqTo)
		{
			var figure = this.figures[sqTo];
			this.bitboards[(int)figure].UndoMove(sqFrom, sqTo);
			
			this.allFigures |= (1UL << sqFrom);
			this.allFigures &= (~(1UL << sqTo));
			
			this.figures[sqFrom] = figure;
			this.figures[sqTo] = Figure.Nobody;
		}
		
		public void AddFigure(Square sq, Figure figure)
		{
			this.bitboards[(int)figure].SetBit(sq);
			this.allFigures |= (1UL << (int)sq);
			
			this.figures[(int)sq] = figure;
		}
		
		public void RemoveFigure(Square sq, Figure figure)
		{			
			this.bitboards[(int)figure].UnsetBit(sq);
			this.allFigures &= (~(1UL << (int)sq));
			
			this.figures[(int)sq] = Figure.Nobody;
		}
		
		public int GetBoardProperty(Figure figure)
		{
			return this.bitboards[(int)figure].GetInnerProperty();
		}
		
		public void SetProperty(Figure figure, int property)
		{
			this.bitboards[(int)figure].SetInnerProperty(property);
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
		
		public FixedArray GetMoves(PlayerBoard opponent, Move lastMove, MovesMask movesMask)
		{
			FixedArray moves = MovesArray.New();
			var innerArray = moves.InnerArray;
			int index = 0;
			
			ulong mask = 0;
			if (movesMask == MovesMask.AllMoves)
				mask = ~this.allFigures;
			else
				mask = opponent.allFigures;
			
			var opponentFigures = opponent.allFigures;
			var otherFigures = opponentFigures | this.allFigures;
			
			// add knight, bishop, rook, queen moves
			for (int i = 0; i < PlayerBoard.KnightBishopRookQueen.Length; ++i)
			{
				var figure = PlayerBoard.KnightBishopRookQueen[i];
				var newMovesList = this.moveGenerators[(int)figure].GetMoves(otherFigures, mask);
				
				for (int j = 0; j < newMovesList.Count; ++j)
				{
					var newMovesArray = newMovesList[j];	
					
					for (int k = 0; k < newMovesArray.Length; ++k)
					{
                        var item = newMovesArray[k];
						innerArray[index].From = item.From;
						innerArray[index].To = item.To;

                        int destinationFigure = (int)opponent.figures[(int)item.To];
						innerArray[index].Type = 
							BitBoardHelper.MoveTypes[(int)figure][destinationFigure][(int)item.From][(int)item.To];
						
						index++;
					}
				}
			}
			
			// add king moves
			var kingMoves = this.GetKingMoves(opponent, mask);
			
			for (int j = 0; j < kingMoves.Count; ++j)
			{
				var newMovesArray = kingMoves[j];
				
				for (int k = 0; k < newMovesArray.Length; ++k)
				{
					var item = newMovesArray[k];
					innerArray[index].From = item.From;
					innerArray[index].To = item.To;
                    int destinationFigure = (int)opponent.figures[(int)item.To];
					
					if (Math.Abs((int)item.From - (int)item.To) == 2)
						innerArray[index].Type = MoveType.KingCastle;
					else
						innerArray[index].Type = 
							BitBoardHelper.MoveTypes[(int)Figure.King][destinationFigure][(int)item.From][(int)item.To];
					
					index++;
				}
			}
			
			// add to mask value 
            mask = opponent.allFigures;
			// pawn in passing state bit
			int lastFrom = (int)lastMove.From;
			int lastTo = (int)lastMove.To;
			int middle = (lastFrom + lastTo) >> 1;
			bool wasLastMovePassing = false;
			
			if (Math.Abs(lastFrom - lastTo) == 16)
				if (opponent.Pawns.IsBitSet(lastMove.To))
				{
					mask |= 1UL << middle;
                    otherFigures |= mask;
					wasLastMovePassing = true;
				}
			
			// add pawns moves            
			var pawnMoves = this.moveGenerators[(int)Figure.Pawn].GetMoves(otherFigures, mask);
			int moveTo;
			for (int j = 0; j < pawnMoves.Count; ++j)
			{
				var newMovesArray = pawnMoves[j];
				
				for (int k = 0; k < newMovesArray.Length; ++k)
				{
					var item = innerArray[index];
					
					item.From = newMovesArray[k].From;
					item.To = newMovesArray[k].To;
					
					moveTo = (int)item.To;
					int destinationFigure = (int)opponent.figures[(int)item.To];
					item.Type = BitBoardHelper.MoveTypes[(int)Figure.Pawn][destinationFigure][(int)item.From][moveTo];
					
					if (wasLastMovePassing)
						if (item.To == (Square)middle)
							item.Type = MoveType.EpCapture;
					
					if ((moveTo < 8) ||
						(moveTo >= 56))
					{
						// add 4 moves
						for (int m = 1; m < 4; ++m)
						{
							innerArray[index + m].From = item.From;
							innerArray[index + m].To = item.To;
						}
						
						this.AddPromotionMoves(innerArray, index, (Figure)j);
						index += 3;
					}
															
					index++;
				}
			}

            moves.Size = index;
			return moves;
		}
		
		protected void AddPromotionMoves(Move[] innerArray, int index, Figure destinationFigure)
		{			
			if (destinationFigure != Figure.Nobody)
			{
				innerArray[index].Type = MoveType.KnightPromoCapture;
				innerArray[index + 1].Type = MoveType.BishopPromoCapture;
				innerArray[index + 2].Type = MoveType.RookPromoCapture;
				innerArray[index + 3].Type = MoveType.QueenPromoCapture;
			}
			else
			{
				innerArray[index].Type = MoveType.KnightPromotion;
				innerArray[index + 1].Type = MoveType.BishopPromotion;
				innerArray[index + 2].Type = MoveType.RookPromotion;
				innerArray[index + 3].Type = MoveType.QueenPromotion;
			}			
		}
				
		public List<Move[]> GetKingMoves(PlayerBoard opponent, ulong mask)
		{
			var otherFigures = opponent.allFigures | this.allFigures;
			var list = this.moveGenerators[(int)Figure.King].GetMoves(otherFigures, mask);
			
			// ---------------------------------------------
			
			// castlings stuff
			
			// check king can move
			if (this.King.AlreadyMoved != 0)
				return list;
				
			var rooks = this.Rooks;
			// if rooks cannot move
			if (rooks.GetInnerProperty() == 0)
				return list;
						
			var allMasks = KingBitBoardHelper.CastlingMasks;
			ulong[] castlingMasks = allMasks[(int)this.color][(int)this.position];
			
			bool leftIsNotEmpty = (castlingMasks[0] & otherFigures) != 0;
			bool rightIsNotEmpty = (castlingMasks[1] & otherFigures) != 0;
			
			// check other figures
			if (leftIsNotEmpty && rightIsNotEmpty)
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
		
		public void FilterMoves(PlayerBoard opponent, FixedArray moves)
		{
			var kingSquare = this.King.GetSquare();
			int index = 0;
			int squeezed_index = 0;
			var innerArray = moves.InnerArray;
			int size = moves.Size;
			
			while (index < size)
			{
				var move = innerArray[index];
				var figure = this.figures[(int)move.From];
				
				if (move.Type == MoveType.KingCastle)
				{
					index++;
					continue;
				}
				
				this.ProcessMove(move, figure);
				
				// TODO write "from direction" method
				if (!this.IsUnderAttack(kingSquare, opponent))
				{
					innerArray[squeezed_index].From = move.From;
					innerArray[squeezed_index].To = move.To;
                    innerArray[squeezed_index].Type = move.Type;
					++squeezed_index;
				}
				
				this.CancelMove((int)move.From, (int)move.To);
				
				++index;
			}

            moves.Size = squeezed_index;
		}
		
		protected bool IsUnderAttack(Square sq, PlayerBoard opponentBoard)
		{
			ulong occupied = 1UL << (int)sq;
			
			ulong pawnAttacks = opponentBoard.GetPawnAttacks();						
			if ((occupied & pawnAttacks) != 0) 
				return true;
			
			ulong knights = opponentBoard.bitboards[(int)Figure.Knight].GetInnerValue();
			ulong king = opponentBoard.bitboards[(int)Figure.King].GetInnerValue();
			ulong otherFigures = opponentBoard.allFigures | this.allFigures;
			
			var knightAttackGenerator = this.attacksGenerators[(int)Figure.Knight];
			ulong occupiedKnightMoves = knightAttackGenerator.GetAttacks(sq, otherFigures);
			if ((occupiedKnightMoves & knights) != 0)
				return true;
				
			var kingAttackGenerator = this.attacksGenerators[(int)Figure.King];
			ulong occupiedKingMoves = kingAttackGenerator.GetAttacks(sq, otherFigures);
			if ((occupiedKingMoves & king) != 0)
				return true;
			
			ulong rooksQueens = opponentBoard.GetRooksQueens();
			ulong bishopsQueens = opponentBoard.GetBishopsQueens();
									
			var rookAttackGenerator = this.attacksGenerators[(int)Figure.Rook];			
			ulong occupiedRookMoves = rookAttackGenerator.GetAttacks(sq, otherFigures);
			if ((occupiedRookMoves & rooksQueens) != 0)
				return true;
				
			var bishopAttackGenerator = this.attacksGenerators[(int)Figure.Bishop];
			ulong occupiedBishopMoves = bishopAttackGenerator.GetAttacks(sq, otherFigures);
			if ((occupiedBishopMoves & bishopsQueens) != 0)
				return true;
				
			return false;
		}
		
		public bool IsUnderCheck(PlayerBoard opponent)
		{
			return IsUnderAttack(this.King.GetSquare(), opponent);
		}
	}
}

