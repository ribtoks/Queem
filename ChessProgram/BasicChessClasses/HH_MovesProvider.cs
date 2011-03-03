using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace BasicChessClasses
{
    public class HH_MovesProvider : MovesProvider
    {
        public HH_MovesProvider(FigureColor myColor, FigureStartPosition myStartPos)
            : base(myColor, myStartPos)
        {
        }

        public MoveResult ProvideMyMove(ChessMove move)
        {
            return this.ProvidePlayerMove(move, player1, player2);
        }

        public MoveResult ProvideOpponenMove(ChessMove move)
        {
            return this.ProvidePlayerMove(move, player2, player1);
        }
    }
}
