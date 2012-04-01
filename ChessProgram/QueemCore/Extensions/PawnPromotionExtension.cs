using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Queem.Core;

namespace Queem.Core.Extensions
{
    public static class PawnPromotionExtension
    {
        private static Dictionary<MoveType, Figure> promotions;

        static PawnPromotionExtension()
        {
            promotions = new Dictionary<MoveType, Figure>()
            {
                { MoveType.KnightPromotion, Figure.Knight },
                { MoveType.BishopPromotion, Figure.Bishop },
                { MoveType.RookPromotion, Figure.Rook },
                { MoveType.QueenPromotion, Figure.Queen },
                { MoveType.KnightPromoCapture, Figure.Knight },
                { MoveType.BishopPromoCapture, Figure.Bishop },
                { MoveType.RookPromoCapture, Figure.Rook },
                { MoveType.QueenPromoCapture, Figure.Queen }
            };
        }

        public static Figure GetPromotionFigure(this MoveType type)
        {
            if (promotions.ContainsKey(type))
                return promotions[type];
            return Figure.Nobody;
        }
    }
}
