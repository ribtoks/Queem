using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QueemAI;
using BasicChessClasses;
using System.Diagnostics;

namespace QueemSpeedBenchmark
{
    public class BenchmarkItem
    {
        protected int historyCount;
        protected int nodesSearched;
        protected int depth;
        protected long milliseconds;
        protected ChessSolver solver;
        protected MovesProvider mp;
        
        public BenchmarkItem(string[] moves)
        {
            // for now only white color will be supported
            // (when i'll have time later, it will be expanded)
            mp = new CH_MovesProvider(FigureColor.White, 
                FigureStartPosition.Down);

            FigureColor currColor = FigureColor.White;

            foreach (var moveStr in moves)
            {
                ChessMove move = null;
                
                if (moveStr.Length == 5)
                    move = new ChessMove(moveStr);
                else
                    move = new PromotionMove(moveStr);

                MoveResult mr = MoveResult.Fail;
                mr = mp.ProvidePlayerMove(move, currColor);
                currColor = currColor.GetOppositeColor();

                if ((mr == MoveResult.PawnReachedEnd) ||
                    (mr == MoveResult.CapturedAndPawnReachedEnd))
                {
                    mp.ReplacePawn(mp.History.LastMove.End,
                        (FigureType)((move as PromotionMove).Promotion));
                }
            }

            solver = new ChessSolver();
        }

        public void Run(int maxdepth)
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();

            // TODO write some exceptions handling
            solver.SolveProblem(mp, FigureColor.White, maxdepth);
            
            watch.Stop();

            this.milliseconds = watch.ElapsedMilliseconds;
            this.historyCount = solver.HistoryDepth;
            this.nodesSearched = solver.NodesSearched;
            this.depth = maxdepth;
        }
    }
}
