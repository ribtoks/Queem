using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
