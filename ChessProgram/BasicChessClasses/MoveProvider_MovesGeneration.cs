using System;
using System.Collections.Generic;

namespace BasicChessClasses
{
	/*
	public static class ListExtensions
	{
		public static List<T> CombineWith<T> (this List<T> list, List<T> otherList)
		{
			list.AddRange (otherList);
			return list;
		}
	}
	*/
	
	public partial class MovesProvider
	{
		public virtual List<Coordinates> GetPawnMoves (Coordinates coords)
		{
			FigureColor color = board[coords].Color;
			
			PawnProcessor pp = pawnProcessor1;
			if (color == this.player2.FiguresColor)
				pp = pawnProcessor2;
			
			GeneralFigureMap<byte> map = figureMapper.GetFiguresMap (color);
            GeneralFigureMap<byte> attackingMap = figureMapper.GetAttackingFiguresMap(color);

            int x = coords.X;
			int y = coords.Y;

            byte firstMove = 0;
            bool canTakePassing = false;

			// if startpos == Down
			int dir = -1;

            if (pp.StartPos == FigureStartPosition.Up)
            {
                dir *= -1;
                firstMove = Convert.ToByte(y == 1);
                canTakePassing = (y == 4);
            }
            else
            {
                firstMove = Convert.ToByte(y == 6);
                canTakePassing = (y == 3);
            }

            byte leftPassing = 0;
            byte rightPassing = 0;

            if (canTakePassing)
            {
                leftPassing = GetPassingPawnState(coords, -1);
                rightPassing = GetPassingPawnState(coords, +1);
            }

            byte oneStepUp = Convert.ToByte(board[x, y + dir].Type == FigureType.Nobody);
            byte twoStepsUp = Convert.ToByte(board[x, y + 2*dir].Type == FigureType.Nobody);
            // can go if only first cell in empty
            twoStepsUp = (byte)(twoStepsUp & oneStepUp);
			
			return pp.GetMoves (coords, 
                (byte)(attackingMap[ board[x - 1, y + dir] ] | leftPassing), 
                oneStepUp, 
			    (byte)(twoStepsUp & firstMove), 
                (byte)(attackingMap[ board[x + 1, y + dir] ] | rightPassing));
		}

        protected virtual byte GetPassingPawnState(Coordinates coords, int direction)
        {
            byte state = 0;

            int x = coords.X;
            int y = coords.Y;

            // neighbour figure
            GeneralFigure nf = board[x + direction, y];

            if ((nf.Type == FigureType.Pawn) && 
                (nf.Color != board[x, y].Color))
            {
                var lastMove = this.history.LastMove;
                if (Math.Abs(lastMove.Start.Y - lastMove.End.Y) == 2)
                    if (lastMove.End.EqualCoords(x + direction, y))
                    {
                        state = 1;
                    }
            }

            return state;
        }
		
		public virtual List<Coordinates> GetKingMoves (Coordinates coords)
		{
			FigureColor color = board[coords].Color;
			
			GeneralFigureMap<byte> map = figureMapper.GetFiguresMap (color);
			
			int initX = coords.X;
			int initY = coords.Y;
			
			int x = initX;
			int y = initY;
			
			List<Coordinates> moveCoords = kingProcessor.GetMoves (coords, 
			                    map[ board[x - 1, y + 1] ], map[ board[x - 1, y] ],
			                    map[ board[x - 1, y - 1] ], map[ board[x, y - 1] ],
			                    map[ board[x + 1, y - 1] ], map[ board[x + 1, y] ],
			                    map[ board[x + 1, y + 1] ], map[ board[x, y + 1] ]);
			
			FiguresManager fm = player1.FiguresManager;
			
			ChessPlayerBase opponentPlayer = player2;
			
			if (color != player1.FiguresColor)
			{
				fm = player2.FiguresManager;
				opponentPlayer = player1;
			}
			
			if (!fm.Kings.King.CanDoCastling)
				return moveCoords;

            bool isClear = true;
			
			Coordinates leftRookCoord = new Coordinates (FieldLetter.A, y);
            if ((object)fm.Rooks[leftRookCoord] != null)
			{
 				if (fm.Rooks[leftRookCoord].CanDoCastling)
				{
					for (x = 1; x < initX; ++x)
					{
						if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
						{
							isClear = false;
							break;
						}
					}
					
					if (isClear)
					{
						bool noEnemyControl = true;
						
						for (x = 1; x <= initX; ++x)
						{
							if (IsUnderPlayerControl (x, y, opponentPlayer))
							{
								noEnemyControl = false;
								break;
							}
						}

                        if (noEnemyControl)
                        {
                            leftRookCoord.X += 2;
                            moveCoords.Add(leftRookCoord);
                        }
					}
				}
			}
			
			Coordinates rightRookCoord = new Coordinates (FieldLetter.H, y);
            if ((object)fm.Rooks[rightRookCoord] != null)
			{
				if (fm.Rooks[rightRookCoord].CanDoCastling)
				{
                    isClear = true;

					for (x = initX + 1; x < 7; ++x)
					{
						if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
						{
							isClear = false;
							break;
						}
					}
					
					if (isClear)
					{
						bool noEnemyControl = true;
                        // rook can be controlled by opponents figure
                        // but king cannot
						for (x = initX; x < 7; ++x)
						{
							if (IsUnderPlayerControl (x, y, opponentPlayer))
							{
								noEnemyControl = false;
								break;
							}
						}

                        if (noEnemyControl)
                        {
                            rightRookCoord.X -= 1;
                            moveCoords.Add(rightRookCoord);
                        }
					}
				}
			}
			
			return moveCoords;
		}
		
		public virtual List<Coordinates> GetHorseMoves (Coordinates coords)
		{
			FigureColor color = board[coords].Color;
			
			GeneralFigureMap<byte> map = figureMapper.GetFiguresMap (color);
			
			int x = coords.X;
			int y = coords.Y;
			
			return horseProcessor.GetMoves (coords, 
			                    map[ board[x - 2, y - 1] ], map[ board[x - 1, y - 2] ],
			                    map[ board[x + 1, y - 2] ], map[ board[x + 2, y - 1] ],
			                    map[ board[x + 2, y + 1] ], map[ board[x + 1, y + 2] ],
			                    map[ board[x - 1, y + 2] ], map[ board[x - 2, y + 1] ]);
		}
		
		public virtual List<Coordinates> GetBishopMoves (Coordinates coords)
		{
			FigureColor color = board[coords].Color;
			
			int initX = coords.X;
			int initY = coords.Y;
			
			int x = initX;
			int y = initY;
			
			List<Coordinates> destinationCells = new List<Coordinates> (8);
			
			// up left
			y = initY - 1;
			x = initX - 1;
			while ((y >= 0) && (x >= 0))
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				
				destinationCells.Add (new Coordinates(x, y));
				--y;
				--x;
			}
			
			// up right
			y = initY - 1;
			x = initX + 1;
			while ((y >= 0) && (x < 8))
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				
				destinationCells.Add (new Coordinates(x, y));
				--y;
				++x;
			}
			
			// down left
			y = initY + 1;
			x = initX - 1;
			while ((y < 8) && (x >= 0))
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				
				destinationCells.Add (new Coordinates(x, y));
				++y;
				--x;
			}
			
			// down right
			y = initY + 1;
			x = initX + 1;
			while ((y < 8) && (x < 8))
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				
				destinationCells.Add (new Coordinates(x, y));
				++y;
				++x;
			}
			
			return destinationCells;
		}
		
		public virtual List<Coordinates> GetRookMoves (Coordinates coords)
		{
			FigureColor color = board[coords].Color;
			
			int initX = coords.X;
			int initY = coords.Y;
			
			int x = initX;
			int y = initY;
			
			List<Coordinates> destinationCells = new List<Coordinates> (8);
			
			// left
			y = initY;
			
			x = initX - 1;
			while (x >= 0)
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				
				destinationCells.Add (new Coordinates(x, y));
				--x;
			}
			
			// right
			x = initX + 1;
			while (x < 8)
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				
				destinationCells.Add (new Coordinates(x, y));
				++x;
			}
			
			
			x = initX;
			
			// down
			y = initY - 1;
			while (y >= 0)
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				
				destinationCells.Add (new Coordinates(x, y));
				--y;
			}
			
			// up
			y = initY + 1;
			while (y < 8)
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				
				destinationCells.Add (new Coordinates(x, y));
				++y;
			}
			
			return destinationCells;
		}
		
		public virtual List<Coordinates> GetQueenMoves (Coordinates coords)
		{
			List<Coordinates> bishopMoves = GetBishopMoves (coords);
			List<Coordinates> rookMoves = GetRookMoves (coords);
			
			List<Coordinates> resultMoves = new List<Coordinates>(bishopMoves.Count + 
			                                                      rookMoves.Count);
			
			resultMoves.AddRange (bishopMoves);
			resultMoves.AddRange (rookMoves);
			
			return resultMoves;
		}
		
		public virtual List<Coordinates> GetAttackingPawnMoves (Coordinates coords)
		{
			FigureColor color = board[coords].Color;
			
			PawnProcessor pp = pawnProcessor1;
			if (color == this.player2.FiguresColor)
                pp = pawnProcessor2;

            int x = coords.X;
            int y = coords.Y;

            GeneralFigureMap<byte> map = figureMapper.GetAttackingFiguresMap(color);
            bool canTakePassing = false;
			
			// if startpos == Down
			int dir = -1;

            if (pp.StartPos == FigureStartPosition.Up)
            {
                dir *= -1;
                canTakePassing = (y == 4);
            }
            else
            {
                canTakePassing = (y == 3);
            }

            byte leftPassing = 0;
            byte rightPassing = 0;

            if (canTakePassing)
            {
                leftPassing = GetPassingPawnState(coords, -1);
                rightPassing = GetPassingPawnState(coords, +1);
            }
			
			return pp.GetMoves (coords, 
                (byte)(map[ board[x - 1, y + dir] ] | leftPassing), 
                0/*map[ board[x, y + dir] ]*/, 
                0/*map[ board[x, y + dir * 2] ]*/, 
                (byte)(map[ board[x + 1, y + dir] ] | rightPassing));
		}
		
		public virtual List<Coordinates> GetAttackingKingMoves (Coordinates coords)
		{
			FigureColor color = board[coords].Color;
			
			GeneralFigureMap<byte> map = figureMapper.GetAttackingFiguresMap (color);
			
			int x = coords.X;
			int y = coords.Y;
			
			return kingProcessor.GetMoves (coords, 
			                    map[ board[x - 1, y + 1] ], map[ board[x - 1, y] ],
			                    map[ board[x - 1, y - 1] ], map[ board[x, y - 1] ],
			                    map[ board[x + 1, y - 1] ], map[ board[x + 1, y] ],
			                    map[ board[x + 1, y + 1] ], map[ board[x, y + 1] ]);
		}
		
		public virtual List<Coordinates> GetAttackingHorseMoves (Coordinates coords)
		{
			FigureColor color = board[coords].Color;
			
			GeneralFigureMap<byte> map = figureMapper.GetAttackingFiguresMap (color);
			
			int x = coords.X;
			int y = coords.Y;
			
			return horseProcessor.GetMoves (coords, 
			                    map[ board[x - 2, y - 1] ], map[ board[x - 1, y - 2] ],
			                    map[ board[x + 1, y - 2] ], map[ board[x + 2, y - 1] ],
			                    map[ board[x + 2, y + 1] ], map[ board[x + 1, y + 2] ],
			                    map[ board[x - 1, y + 2] ], map[ board[x - 2, y + 1] ]);
		}
		
		public virtual List<Coordinates> GetAttackingBishopMoves (Coordinates coords)
		{
			FigureColor color = board[coords].Color;
			
			int initX = coords.X;
			int initY = coords.Y;
			
			int x = initX;
			int y = initY;
			
			List<Coordinates> destinationCells = new List<Coordinates> (4);
			
			// up left
			y = initY - 1;
			x = initX - 1;
			while ((y >= 0) && (x >= 0))
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				--y;
				--x;
			}
			
			// up right
			y = initY - 1;
			x = initX + 1;
			while ((y >= 0) && (x < 8))
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				--y;
				++x;
			}
			
			// down left
			y = initY + 1;
			x = initX - 1;
			while ((y < 8) && (x >= 0))
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				++y;
				--x;
			}
			
			// down right
			y = initY + 1;
			x = initX + 1;
			while ((y < 8) && (x < 8))
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				++y;
				++x;
			}
			
			return destinationCells;
		}
		
		public virtual List<Coordinates> GetAttackingRookMoves (Coordinates coords)
		{
			FigureColor color = board[coords].Color;
			
			int initX = coords.X;
			int initY = coords.Y;
			
			int x = initX;
			int y = initY;
			
			List<Coordinates> destinationCells = new List<Coordinates> (4);
			
			// left
			y = initY;
			
			x = initX - 1;
			while (x >= 0)
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				--x;
			}
			
			// right
			x = initX + 1;
			while (x < 8)
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				++x;
			}
			
			
			x = initX;
			
			// down
			y = initY - 1;
			while (y >= 0)
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				--y;
			}
			
			// up
			y = initY + 1;
			while (y < 8)
			{
				if (board[(FieldLetter)x, y].Type != FigureType.Nobody)
				{
					if (board[(FieldLetter)x, y].Color != color)
						destinationCells.Add (new Coordinates(x, y));
					break;
				}
				++y;
			}
			
			return destinationCells;
		}
		
		public virtual List<Coordinates> GetAttackingQueenMoves (Coordinates coords)
		{
			List<Coordinates> bishopMoves = GetAttackingBishopMoves (coords);
			List<Coordinates> rookMoves = GetAttackingRookMoves (coords);
			
			List<Coordinates> resultMoves = new List<Coordinates>(bishopMoves.Count + 
			                                                      rookMoves.Count);
			
			resultMoves.AddRange (bishopMoves);
			resultMoves.AddRange (rookMoves);
			
			return resultMoves;
		}
		
        /// <summary>
        /// Generates all moves, that can do 
        /// a figure on specified coordinates
        /// </summary>
        /// <param name="coords">Coordinates of a figure</param>
        /// <returns>List of moves, that can be done in generic situation</returns>
		public List<Coordinates> GetMoves (Coordinates coords)
		{
			//return movesGenerators[ board[coords.X, coords.Y].Type ] (coords);
			FigureType figureType = board[coords].Type;
			
			switch (figureType)
			{
			case FigureType.Bishop:
				return this.GetBishopMoves (coords);
			case FigureType.Horse:
				return this.GetHorseMoves (coords);
			case FigureType.King:
				return this.GetKingMoves (coords);
			case FigureType.Pawn:
				return this.GetPawnMoves (coords);
			case FigureType.Queen:
				return this.GetQueenMoves (coords);
			case FigureType.Rook:
				return this.GetRookMoves (coords);
			default:
				return new List<Coordinates> ();
			}
		}

        /// <summary>
        /// Generates all moves, that can do 
        /// a figure on specified coordinates
        /// when taking some opponent's figure
        /// </summary>
        /// <param name="coords">Coordinates of a figure</param>
        /// <returns>List of moves, that can be done in generic situation</returns>
        public List<Coordinates> GetAttackingMoves(Coordinates coords)
        {
            //return movesGenerators[ board[coords.X, coords.Y].Type ] (coords);
            FigureType figureType = board[coords].Type;

            switch (figureType)
            {
                case FigureType.Bishop:
                    return this.GetAttackingBishopMoves(coords);
                case FigureType.Horse:
                    return this.GetAttackingHorseMoves(coords);
                case FigureType.King:
                    return this.GetAttackingKingMoves(coords);
                case FigureType.Pawn:
                    return this.GetAttackingPawnMoves(coords);
                case FigureType.Queen:
                    return this.GetAttackingQueenMoves(coords);
                case FigureType.Rook:
                    return this.GetAttackingRookMoves(coords);
                default:
                    return new List<Coordinates>();
            }
        }
		
        /// <summary>
        /// Generates all moves, that can be
        /// done in particular situation
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
		public List<Coordinates> GetFilteredCells (Coordinates coords)
		{
			List<Coordinates> allCoords = GetMoves (coords);
			
			if (board[coords].Color == player1.FiguresColor)
				return FilterMoves (coords, allCoords, player1, player2);
			else
				return FilterMoves (coords, allCoords, player2, player1);
		}

        /// <summary>
        /// Generates all moves, that can be
        /// done in particular situation
        /// when taking opponent's figure
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public List<Coordinates> GetFilteredAttackingCells(Coordinates coords)
        {
            List<Coordinates> allCoords = GetAttackingMoves(coords);

            if (board[coords].Color == player1.FiguresColor)
                return this.FilterMoves(coords, allCoords, player1, player2);
            else
                return this.FilterMoves(coords, allCoords, player2, player1);
        }

        public List<Coordinates> GetFilteredCells(Coordinates coords,
            ChessPlayerBase player, ChessPlayerBase opponentPlayer)
        {
            List<Coordinates> allCoords = GetMoves(coords);
            return this.FilterMoves(coords, allCoords, player, opponentPlayer);
        }

        public List<Coordinates> GetFilteredAttackingCells(Coordinates coords,
            ChessPlayerBase player, ChessPlayerBase opponentPlayer)
        {
            List<Coordinates> allCoords = this.GetAttackingMoves(coords);
            return this.FilterMoves(coords, allCoords, player, opponentPlayer);
        }
		
		public bool ArePossibleCells (Coordinates coords)
		{
			return (this.GetFilteredCells (coords).Count > 0);
		}
	}
}
