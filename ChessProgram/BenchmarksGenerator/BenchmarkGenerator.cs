using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QueemAI;
using BasicChessClasses;

namespace BenchmarksGenerator
{
    public static class BenchmarkGenerator
    {
        public static List<ChessMove> GenerateSituation(int depth)
        {
            int curr_depth = 0;
            FigureColor myColor = FigureColor.White;
            FigureStartPosition myPosition = FigureStartPosition.Down;

            MovesProvider mp = new MovesProvider(myColor, myPosition);

            FigureColor currColor = myColor;
            Random rand = new Random(DateTime.Now.Millisecond);

            while (curr_depth < depth)
            {
                List<ChessMove> moves = ChessSolver.GenerateAllMoves(mp, currColor);
                int index = rand.Next(moves.Count);
                // just get random move
                var move = moves[index];

                mp.ProvidePlayerMove(move, currColor);
                currColor = currColor.GetOppositeColor();

                curr_depth += 1;
            }

            return mp.History.Moves;
        }
    }
}
