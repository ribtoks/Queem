using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace QueemAI
{
    public class CH_MovesProvider : MovesProvider
    {
        public CH_MovesProvider(FigureColor myColor, FigureStartPosition myStartPos)
            : base(myColor, myStartPos)
        {
        }

        public MoveResult ProvideMyMove(ChessMove move)
        {
            return this.ProvidePlayerMove(move, player1, player2);
        }

        public MoveResult ProvideOpponetMove(ChessMove move)
        {
            return this.ProvidePlayerMove(move, player2, player1);
        }

        public void ReplacePawn(Coordinates pawnCoords, FigureType newType, FigureColor figureColor)
        {
            if (figureColor == player1.FiguresColor)
                this.ReplacePawnAtTheOtherSide(pawnCoords, newType, player1);
            else
                this.ReplacePawnAtTheOtherSide(pawnCoords, newType, player2);
        }
    }
}
