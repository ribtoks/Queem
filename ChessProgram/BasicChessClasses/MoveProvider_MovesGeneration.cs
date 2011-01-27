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
			
			// if startpos == Down
			int dir = -1;
			
			if (pp.StartPos == FigureStartPosition.Up)
				dir *= -1;
			
			int x = coords.X;
			int y = coords.Y;
			
			return pp.GetMoves (coords, map[ board[x - 1, y + dir] ], map[ board[x, y + dir] ], 
			                    map[ board[x, y + dir * 2] ], map[ board[x + 1, y + dir] ]);
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
			if (board[FieldLetter.A, y].Type == FigureType.Rook)
			{
				if (fm.Rooks[leftRookCoord].CanDoCastling)
				{
					for (x = 0; x < initX; ++x)
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
						
						for (x = 0; x < initX; ++x)
						{
							if (IsUnderPlayerControl (x, y, opponentPlayer))
							{
								noEnemyControl = false;
								break;
							}
						}
						
						if (noEnemyControl)							
							moveCoords.Add (leftRookCoord);
					}
				}
			}
			
			Coordinates rightRookCoord = new Coordinates (FieldLetter.A, y);
			if (board[FieldLetter.H, y].Type == FigureType.Rook)
			{
				if (fm.Rooks[rightRookCoord].CanDoCastling)
				{
					for (x = initX + 1; x < 8; ++x)
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
						
						for (x = initX + 1; x < 8; ++x)
						{
							if (IsUnderPlayerControl (x, y, opponentPlayer))
							{
								noEnemyControl = false;
								break;
							}
						}
						
						if (noEnemyControl)
							moveCoords.Add (rightRookCoord);
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
			
			GeneralFigureMap<byte> map = figureMapper.GetAttackingFiguresMap (color);
			
			// if startpos == Down
			int dir = -1;
			
			if (pp.StartPos == FigureStartPosition.Up)
				dir *= -1;
			
			int x = coords.X;
			int y = coords.Y;
			
			return pp.GetMoves (coords, map[ board[x - 1, y + dir] ], map[ board[x, y + dir] ], 
			                    map[ board[x, y + dir * 2] ], map[ board[x + 1, y + dir] ]);
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
			FigureType figureType = board[coords.X, coords.Y].Type;
			
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
		
		public bool ArePossibleCells (Coordinates coords)
		{
			return (this.GetFilteredCells (coords).Count > 0);
		}
	}
}
