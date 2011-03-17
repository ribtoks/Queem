using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace ChessBoardTest
{
    public class MoveRedoInfo
    {
        public ChessMove Move { get; set; }
        public FigureType PawnDecision { get; set; }
    }
}
