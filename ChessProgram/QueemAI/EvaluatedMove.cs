using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace QueemAI
{
    public class EvaluatedMove
    {
        public EvaluatedMove()
        {
            //Move = new ChessMove();
            Move = null;
            Value = 0;
        }

        public ChessMove Move { get; set; }
        public int Value { get; set; }
    }
}
