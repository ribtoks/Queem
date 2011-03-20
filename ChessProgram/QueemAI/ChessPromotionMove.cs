using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace QueemAI
{
    public class ChessPromotionMove : ChessMove
    {
        public ChessPromotionMove()
            : base()
        {
        }

        public ChessPromotionMove(ChessMove copy, FigureType promotion)
            : base(copy)
        {
            Promotion = promotion;
        }

        public ChessPromotionMove(ChessPromotionMove copy)
            : base(copy)
        {
            Promotion = copy.Promotion;
        }

        public FigureType Promotion { get; set; }
    }
}
