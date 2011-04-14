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
        public static readonly int BishopValue = 3 * PawnValue;
        public static readonly int HorseValue = 3 * PawnValue;
        public static readonly int RookValue = 5 * PawnValue;
        public static readonly int QueenValue = 9 * PawnValue;
        public static readonly int KingValue = 100 * PawnValue;

        public static readonly int MateValue = KingValue;

        public static readonly int[,] PositionValue = new int[,] {
            {1, 1, 2, 3, 3, 2, 1, 1},
            {1, 2, 3, 4, 4, 3, 2, 1},
            {2, 3, 4, 5, 5, 4, 3, 2},
            {3, 4, 5, 7, 7, 5, 4, 3},
            {3, 4, 5, 7, 7, 5, 4, 3},
            {2, 3, 4, 5, 5, 4, 3, 2},
            {1, 2, 3, 4, 4, 3, 2, 1},
            {1, 1, 2, 3, 3, 2, 1, 1}
        };

        internal static int SimpleEvaluatePosition(ChessPlayerBase player, ChessPlayerBase opponentPlayer)
        {
            int result = 0;

            // add actual figures
            result += player.FiguresManager.PawnCount * PositionEvaluator.PawnValue;
            result += player.FiguresManager.BishopCount * PositionEvaluator.BishopValue;
            result += player.FiguresManager.HorseCount * PositionEvaluator.HorseValue;
            result += player.FiguresManager.RookCount * PositionEvaluator.RookValue;
            result += player.FiguresManager.QueenCount * PositionEvaluator.QueenValue;

            //foreach (var figure in player.FiguresManager)
            //{
            //    var coords = figure.Coordinates;
            //    result += PositionEvaluator.PositionValue[coords.Y, coords.X];
            //}
            result += player.FiguresManager.GetPositionValue(
                PositionEvaluator.PositionValue);

            if (player.FiguresManager.Bishops.Count == 2)
                result += PositionEvaluator.PawnValue / 3;

            // TODO write lots of other conditions

            return result;
        }

        internal static int SophisticatedEvaluatePosition(ChessPlayerBase player, ChessPlayerBase opponentPlayer)
        {
            int result = 0;

            // add actual figures
            result += player.FiguresManager.PawnCount * PositionEvaluator.PawnValue;
            result += player.FiguresManager.BishopCount * PositionEvaluator.BishopValue;
            result += player.FiguresManager.HorseCount * PositionEvaluator.HorseValue;
            result += player.FiguresManager.RookCount * PositionEvaluator.RookValue;
            result += player.FiguresManager.QueenCount * PositionEvaluator.QueenValue;

            //foreach (var figure in player.FiguresManager)
            //{
            //    var coords = figure.Coordinates;
            //    result += PositionEvaluator.PositionValue[coords.Y, coords.X];
            //}
            result += player.FiguresManager.GetPositionValue(
                PositionEvaluator.PositionValue);

            if (player.FiguresManager.Bishops.Count == 2)
                result += PositionEvaluator.PawnValue / 3;

            // TODO write lots of other conditions

            return result;
        }
    }
}
