using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace QueemAI
{
    public static class PositionEvaluator
    {
        public static readonly int PawnValue = 100;
        public static readonly int BishopValue = 400;
        public static readonly int HorseValue = 400;
        public static readonly int RookValue = 600;
        public static readonly int QueenValue = 1200;
        public static readonly int KingValue = 30300;

        public static readonly int MateValue = KingValue;

        #region Position values

        public static readonly int[,] PawnPositionValues = new int[,] {
            {0,  0,  0,  0,  0,  0,  0,  0},
            {50, 50, 50, 50, 50, 50, 50, 50},
            {10, 10, 20, 30, 30, 20, 10, 10},
            {5,  5, 10, 25, 25, 10,  5,  5},
            {0,  0,  0, 20, 20,  0,  0,  0},
            {5, -5,-10,  0,  0,-10, -5,  5},
            {5, 10, 10,-20,-20, 10, 10,  5},
            {0,  0,  0,  0,  0,  0,  0,  0}
        };

        public static readonly int[,] HorsePositionValues = new int[,] {
            {-50,-40,-30,-30,-30,-30,-40,-50},
            {-40,-20,  0,  0,  0,  0,-20,-40},
            {-30,  0, 10, 15, 15, 10,  0,-30},
            {-30,  5, 15, 20, 20, 15,  5,-30},
            {-30,  0, 15, 20, 20, 15,  0,-30},
            {-30,  5, 10, 15, 15, 10,  5,-30},
            {-40,-20,  0,  5,  5,  0,-20,-40},
            {-50,-40,-30,-30,-30,-30,-40,-50}
        };

        public static readonly int[,] BishopPositionValues = new int[,] {
            {-20,-10,-10,-10,-10,-10,-10,-20},
            {-10,  0,  0,  0,  0,  0,  0,-10},
            {-10,  0,  5, 10, 10,  5,  0,-10},
            {-10,  5,  5, 10, 10,  5,  5,-10},
            {-10,  0, 10, 10, 10, 10,  0,-10},
            {-10, 10, 10, 10, 10, 10, 10,-10},
            {-10,  5,  0,  0,  0,  0,  5,-10},
            {-20,-10,-10,-10,-10,-10,-10,-20}
        };

        public static readonly int[,] RookPositionValues = new int[,] {
            { 0,  0,  0,  0,  0,  0,  0,  0},
            {5, 10, 10, 10, 10, 10, 10,  5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {-5,  0,  0,  0,  0,  0,  0, -5},
            {0,  0,  0,  5,  5,  0,  0,  0}
        };

        public static readonly int[,] QueenPositionValues = new int[,] {
            {-20,-10,-10, -5, -5,-10,-10,-20},
            {-10,  0,  0,  0,  0,  0,  0,-10},
            {-10,  0,  5,  5,  5,  5,  0,-10},
            {-5,  0,  5,  5,  5,  5,  0, -5},
            {0,  0,  5,  5,  5,  5,  0, -5},
            {-10,  5,  5,  5,  5,  5,  0,-10},
            {-10,  0,  5,  0,  0,  0,  0,-10},
            {-20,-10,-10, -5, -5,-10,-10,-20}
        };

        public static readonly int[,] KingMiddleGamePositionValues = new int[,] {
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-30,-40,-40,-50,-50,-40,-40,-30},
            {-20,-30,-30,-40,-40,-30,-30,-20},
            {-10,-20,-20,-20,-20,-20,-20,-10},
            {20, 20,  0,  0,  0,  0, 20, 20},
            {20, 30, 10,  0,  0, 10, 30, 20}
        };

        public static readonly int[,] KingEndGamePositionValues = new int[,] {
            {-50,-40,-30,-20,-20,-30,-40,-50},
            {-30,-20,-10,  0,  0,-10,-20,-30},
            {-30,-10, 20, 30, 30, 20,-10,-30},
            {-30,-10, 30, 40, 40, 30,-10,-30},
            {-30,-10, 30, 40, 40, 30,-10,-30},
            {-30,-10, 20, 30, 30, 20,-10,-30},
            {-30,-30,  0,  0,  0,  0,-30,-30},
            {-50,-30,-30,-30,-30,-30,-30,-50}
        };

        #endregion
        
        internal static int SimpleEvaluatePosition(ChessPlayerBase player, ChessPlayerBase opponentPlayer)
        {
            int result = 0;
            FiguresManager fm = player.FiguresManager;

            // add actual figures
            result += fm.PawnCount * PositionEvaluator.PawnValue;
            result += fm.BishopCount * PositionEvaluator.BishopValue;
            result += fm.HorseCount * PositionEvaluator.HorseValue;
            result += fm.RookCount * PositionEvaluator.RookValue;
            result += fm.QueenCount * PositionEvaluator.QueenValue;

            // position values
            result += fm.Pawns.GetPositionValue(PawnPositionValues, player.StartPos);
            result += fm.Horses.GetPositionValue(HorsePositionValues, player.StartPos);
            result += fm.Bishops.GetPositionValue(BishopPositionValues, player.StartPos);
            result += fm.Rooks.GetPositionValue(RookPositionValues, player.StartPos);
            result += fm.Queens.GetPositionValue(QueenPositionValues, player.StartPos);

            if (fm.Count < 5)
                result += fm.Kings.GetPositionValue(KingEndGamePositionValues,
                    player.StartPos);
            else
                result += fm.Kings.GetPositionValue(KingMiddleGamePositionValues,
                    player.StartPos);

            // two bishops control disjoint sets of cells
            if (player.FiguresManager.Bishops.Count == 2)
                result += PositionEvaluator.PawnValue / 3;

            return result;
        }

        internal static int SophisticatedEvaluatePosition(ChessPlayerBase player, ChessPlayerBase opponentPlayer)
        {
            int result = 0;
            FiguresManager fm = player.FiguresManager;

            // add actual figures
            result += fm.PawnCount * PositionEvaluator.PawnValue;
            result += fm.BishopCount * PositionEvaluator.BishopValue;
            result += fm.HorseCount * PositionEvaluator.HorseValue;
            result += fm.RookCount * PositionEvaluator.RookValue;
            result += fm.QueenCount * PositionEvaluator.QueenValue;

            // position values
            result += fm.Pawns.GetPositionValue(PawnPositionValues, player.StartPos);
            result += fm.Horses.GetPositionValue(HorsePositionValues, player.StartPos);
            result += fm.Bishops.GetPositionValue(BishopPositionValues, player.StartPos);
            result += fm.Rooks.GetPositionValue(RookPositionValues, player.StartPos);
            result += fm.Queens.GetPositionValue(QueenPositionValues, player.StartPos);

            if (fm.Count < 5)
                result += fm.Kings.GetPositionValue(KingEndGamePositionValues,
                    player.StartPos);
            else
                result += fm.Kings.GetPositionValue(KingMiddleGamePositionValues,
                    player.StartPos);
            
            if (player.FiguresManager.Bishops.Count == 2)
                result += PositionEvaluator.PawnValue / 3;

            // TODO write lots of other conditions

            return result;
        }
    }
}
