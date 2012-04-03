using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Queem.Core;

namespace Queem.Core.Extensions
{
    public static class PawnPromotionExtension
    {
        private static Dictionary<MoveType, Figure> reversePromotions;
        private static Dictionary<Figure, MoveType> promotions;

        static PawnPromotionExtension()
        {
            reversePromotions = new Dictionary<MoveType, Figure>()
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

            promotions = new Dictionary<Figure, MoveType>()
            {
                { Figure.Knight, MoveType.KnightPromotion  },
                { Figure.Bishop, MoveType.BishopPromotion  },
                { Figure.Rook, MoveType.RookPromotion },
                { Figure.Queen, MoveType.QueenPromotion}
            };
        }

        public static Figure GetPromotionFigure(this MoveType type)
        {
            if (reversePromotions.ContainsKey(type))
                return reversePromotions[type];
            return Figure.Nobody;
        }

        public static MoveType GetPromotionType(this Figure figure)
        {
            if (promotions.ContainsKey(figure))
                return promotions[figure];
            return MoveType.Quiet;
        }
    }
}
