using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace QueemAI
{
    internal class MovesGenParams
    {
        public MovesProvider Provider { get; set; }
        public ChessPlayerBase Player { get; set; }
        public ChessPlayerBase OpponentPlayer { get; set; }
        public CoordsTable<CoordsTable<int>> HistoryTable { get; set; }
    }
}
