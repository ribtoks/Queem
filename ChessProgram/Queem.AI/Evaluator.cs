using System;
using Queem.Core.ChessBoard;
using Queem.Core;

namespace Queem.AI
{
	public static class Evaluator
    {
        public static readonly int PawnValue = 100;
        public static readonly int BishopValue = 400;
        public static readonly int KnightValue = 400;
        public static readonly int RookValue = 600;
        public static readonly int QueenValue = 1200;
        public static readonly int KingValue = 10000;

        public static readonly int MateValue = KingValue;
        
        public static int[] FigureValues;
        public static Figure[] EvaluatedFigures = new Figure[] { Figure.Pawn, Figure.Bishop, 
        	Figure.Rook, Figure.Knight, Figure.Queen };
        	
        // position - figure - square
        private static int[][][] PositionValues;
        
        private static int[][] KingSquareStartValues;
		private static int[][] KingSquareEndValues;
		
		static Evaluator()
		{
			FigureValues = GetValuesArray();
			FillSquareValues();
		}
		
		private static void FillSquareValues()
		{
			KingSquareStartValues = new int[2][];
			KingSquareStartValues[(int)PlayerPosition.Down] = MakeSquareLikeArray(KingMiddleGamePositionValues);
			KingSquareStartValues[(int)PlayerPosition.Up] = MakeSquareUpPositionArray(KingMiddleGamePositionValues);
			
			KingSquareEndValues = new int[2][];
			KingSquareEndValues[(int)PlayerPosition.Down] = MakeSquareLikeArray(KingEndGamePositionValues);
			KingSquareEndValues[(int)PlayerPosition.Up] = MakeSquareUpPositionArray(KingEndGamePositionValues);
			
			PositionValues = new int[2][][];
			PositionValues[(int)PlayerPosition.Down] = GetDownPositionValues();
			PositionValues[(int)PlayerPosition.Up] = GetUpPositionValues();
		}
		
		private static int[][] GetDownPositionValues()
		{
			int[][] positionValues = new int[7][];
			positionValues[(int)Figure.Pawn] = MakeSquareLikeArray(PawnPositionValues);
			positionValues[(int)Figure.Knight] = MakeSquareLikeArray(KnightPositionValues);
			positionValues[(int)Figure.Bishop] = MakeSquareLikeArray(BishopPositionValues);
			positionValues[(int)Figure.Rook] = MakeSquareLikeArray(RookPositionValues);
			positionValues[(int)Figure.Queen] = MakeSquareLikeArray(QueenPositionValues);			
			positionValues[(int)Figure.Nobody] = new int[64];
			positionValues[(int)Figure.King] = new int[64];
			
			return positionValues;
		}
		
		private static int[][] GetUpPositionValues()
		{
			int[][] positionValues = new int[7][];
			positionValues[(int)Figure.Pawn] = MakeSquareUpPositionArray(PawnPositionValues);
			positionValues[(int)Figure.Knight] = MakeSquareUpPositionArray(KnightPositionValues);
			positionValues[(int)Figure.Bishop] = MakeSquareUpPositionArray(BishopPositionValues);
			positionValues[(int)Figure.Rook] = MakeSquareUpPositionArray(RookPositionValues);
			positionValues[(int)Figure.Queen] = MakeSquareUpPositionArray(QueenPositionValues);			
			positionValues[(int)Figure.Nobody] = new int[64];
			positionValues[(int)Figure.King] = new int[64];
			
			return positionValues;
		}
		
		private static int[] GetValuesArray()
		{
			int[] array = new int[7];
			array[(int)Figure.Pawn] = PawnValue;
			array[(int)Figure.Bishop] = BishopValue;
			array[(int)Figure.Knight] = KnightValue;
			array[(int)Figure.Rook] = RookValue;
			array[(int)Figure.Queen] = QueenValue;
			array[(int)Figure.King] = KingValue;
			array[(int)Figure.Nobody] = 0;
			return array;
		}
		
		private static int[] MakeSquareLikeArray(int[,] usualArray)
		{
			int[] resultArray = new int[64];
			
			for (int i = 0; i < 8; ++i)
			{
				for (int j = 0; j < 8; ++j)
				{
					int index = 8 * (7 - i) + j;
					resultArray[index] = usualArray[i, j];
				}
			}
			
			return resultArray;
		}
		
		private static int[] MakeSquareUpPositionArray(int[,] usualArray)
		{
			int[] resultArray = new int[64];
			
			for (int i = 0; i < 8; ++i)
			{
				for (int j = 0; j < 8; ++j)
				{
					int index = 8 * i + j;
					resultArray[index] = usualArray[i, j];
				}
			}
			
			return resultArray;
		}
		
        #region Position values

        public static readonly int[,] PawnPositionValues = new int[,] {
            {0, 0, 0, 0, 0, 0, 0, 0},
            {50, 50, 50, 50, 50, 50, 50, 50},
            {10, 10, 20, 30, 30, 20, 10, 10},
            {5, 5, 10, 25, 25, 10, 5, 5},
            {0, 0, 0, 20, 20, 0, 0, 0},
            {5, -5,-10, 0, 0,-10, -5, 5},
            {5, 10, 10,-20,-20, 10, 10, 5},
            {0, 0, 0, 0, 0, 0, 0, 0}
        };

        public static readonly int[,] KnightPositionValues = new int[,] {
            {-50,-40,-30,-30,-30,-30,-40,-50},
            {-40,-20, 0, 0, 0, 0,-20,-40},
            {-30, 0, 10, 15, 15, 10, 0,-30},
            {-30, 5, 15, 20, 20, 15, 5,-30},
            {-30, 0, 15, 20, 20, 15, 0,-30},
            {-30, 5, 10, 15, 15, 10, 5,-30},
            {-40,-20, 0, 5, 5, 0,-20,-40},
            {-50,-40,-30,-30,-30,-30,-40,-50}
        };

        public static readonly int[,] BishopPositionValues = new int[,] {
            {-20,-10,-10,-10,-10,-10,-10,-20},
            {-10, 0, 0, 0, 0, 0, 0,-10},
            {-10, 0, 5, 10, 10, 5, 0,-10},
            {-10, 5, 5, 10, 10, 5, 5,-10},
            {-10, 0, 10, 10, 10, 10, 0,-10},
            {-10, 10, 10, 10, 10, 10, 10,-10},
            {-10, 5, 0, 0, 0, 0, 5,-10},
            {-20,-10,-10,-10,-10,-10,-10,-20}
        };

        public static readonly int[,] RookPositionValues = new int[,] {
            { 0, 0, 0, 0, 0, 0, 0, 0},
            {5, 10, 10, 10, 10, 10, 10, 5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {-5, 0, 0, 0, 0, 0, 0, -5},
            {0, 0, 0, 5, 5, 0, 0, 0}
        };

        public static readonly int[,] QueenPositionValues = new int[,] {
            {-20,-10,-10, -5, -5,-10,-10,-20},
            {-10, 0, 0, 0, 0, 0, 0,-10},
            {-10, 0, 5, 5, 5, 5, 0,-10},
            {-5, 0, 5, 5, 5, 5, 0, -5},
            {0, 0, 5, 5, 5, 5, 0, -5},
            {-10, 5, 5, 5, 5, 5, 0,-10},
            {-10, 0, 5, 0, 0, 0, 0,-10},
            {-20,-10,-10, -5, -5,-10,-10,-20}
        };

        public static readonly int[,] KingMiddleGamePositionValues = new int[,] {
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-20,-30,-30,-40,-40,-30,-30,-20},
            {-10,-20,-20,-20,-20,-20,-20,-10},
            {20, 20, 0, 0, 0, 0, 20, 20},
            {20, 30, 10, 0, 0, 10, 30, 20}
        };

        public static readonly int[,] KingEndGamePositionValues = new int[,] {
            {-50,-40,-30,-20,-20,-30,-40,-50},
            {-30,-20,-10, 0, 0,-10,-20,-30},
            {-30,-10, 20, 30, 30, 20,-10,-30},
            {-30,-10, 30, 40, 40, 30,-10,-30},
            {-30,-10, 30, 40, 40, 30,-10,-30},
            {-30,-10, 20, 30, 30, 20,-10,-30},
            {-30,-30, 0, 0, 0, 0,-30,-30},
            {-50,-30,-30,-30,-30,-30,-30,-50}
        };

        #endregion
        
        public static int Evaluate(PlayerBoard player, PlayerBoard opponent)
		{
			int value1 = 0;
			int value2 = 0;
			
			foreach (var figure in EvaluatedFigures)
			{
				int figureValue = FigureValues[(int)figure];
				value1 += player.BitBoards[(int)figure].GetBitsCount() * figureValue;
				value2 += opponent.BitBoards[(int)figure].GetBitsCount() * figureValue;
			}
			
			var positionValues1 = PositionValues[(int)player.Position];
			var positionValues2 = PositionValues[(int)opponent.Position];
			
			for (int i = 0; i < 64; ++i)
			{
				value1 += positionValues1[ (int)player.Figures[i] ][i];
				value2 += positionValues2[ (int)opponent.Figures[i] ][i];
			}
			
			if (player.GetAllFiguresCount() < 6)
				value1 += KingSquareEndValues[(int)player.Position][(int)player.King.GetSquare()];
			else
				value1 += KingSquareStartValues[(int)player.Position][(int)player.King.GetSquare()];
		
			if (opponent.GetAllFiguresCount() < 6)
				value2 += KingSquareEndValues[(int)opponent.Position][(int)opponent.King.GetSquare()];
			else
				value2 += KingSquareStartValues[(int)opponent.Position][(int)opponent.King.GetSquare()];		
					
			return value1 - value2;		
		}
	}
}

