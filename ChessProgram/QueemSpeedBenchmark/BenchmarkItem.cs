using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Queem.Core.ChessBoard;
using Queem.AI;
using DebutsLib;
using Queem.Core;
using Queem.Core.Extensions;

namespace QueemSpeedBenchmark
{
    public class BenchmarkItem
    {
        protected int historyCount;
        protected int nodesSearched;
        protected int depth;
        protected long milliseconds;
        protected ChessSolver solver;
        protected GameProvider provider;
        protected Color lastColor;
        
        public BenchmarkItem(string[] moves)
        {
            // for now only white color will be supported
            // (when i'll have time later, it will be expanded)
            this.provider = new GameProvider();

            Color color = Color.White;

            foreach (var moveStr in moves)
            {
                if (moveStr.Length == 0)
                    continue;

                Move move = new Move(moveStr);

                if (this.provider.PlayerBoards[(int)color].Figures[(int)move.From] == Queem.Core.Figure.King)
                    if (Math.Abs((int)move.From - (int)move.To) == 2)
                        move.Type = MoveType.KingCastle;

                this.provider.ProcessMove(move, color);

                bool needsPromotion = (int)move.Type >= (int)MoveType.Promotion;
                if (needsPromotion)
                    this.provider.PromotePawn(
                        color,
                        move.To,
                        move.Type.GetPromotionFigure());

                color = (Queem.Core.Color)(1 - (int)color);
            }

            solver = new ChessSolver(new DebutGraph());
            this.lastColor = color;
        }

        public void Run(int maxdepth)
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();

            Color color = this.lastColor;
            
            // TODO write some exceptions handling
            solver.SolveProblem(this.provider, color, maxdepth);
            
            watch.Stop();

            this.milliseconds = watch.ElapsedMilliseconds;
            this.nodesSearched = solver.NodesSearched;
            this.depth = maxdepth;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}",
                this.milliseconds,
                this.nodesSearched,
                this.depth
                );
        }
    }
}
