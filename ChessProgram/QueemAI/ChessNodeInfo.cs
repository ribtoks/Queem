using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace QueemAI
{
    public delegate int EvaluatorDelegate(ChessPlayerBase player,
        ChessPlayerBase opponentPlayer);

    public class ChessNodeInfo
    {
        public ChessNodeInfo()
        {
        }

        public ChessNodeInfo(ChessNodeInfo cni)
        {
            this.Alpha = cni.Alpha;
            this.Beta = cni.Beta;
            this.CurrPlayer = cni.CurrPlayer;
            this.Depth = cni.Depth;
            this.Evaluator = cni.Evaluator;
            this.CanMakeNullMove = cni.CanMakeNullMove;
        }

        public ChessNodeInfo GetNext()
        {
            ChessNodeInfo cni = new ChessNodeInfo();
            cni.Alpha = -this.Beta;
            cni.Beta = -this.Alpha;
            cni.Depth = this.Depth - 1;
            cni.Evaluator = this.Evaluator;
            cni.CurrPlayer = this.CurrPlayer.GetOppositePlayer();
            cni.CanMakeNullMove = this.CanMakeNullMove;

            return cni;
        }

        public ChessNodeInfo GetNextNullMove()
        {
            ChessNodeInfo cni = new ChessNodeInfo();
            cni.Alpha = -this.Beta;
            cni.Beta = -this.Alpha;
            cni.Depth = this.Depth - 2;
            cni.Evaluator = this.Evaluator;
            cni.CurrPlayer = this.CurrPlayer.GetOppositePlayer();
            cni.CanMakeNullMove = false;

            return cni;
        }

        public int Alpha { get; set; }
        public int Beta { get; set; }
        public CurrentPlayer CurrPlayer { get; set; }
        public int Depth { get; set; }
        public EvaluatorDelegate Evaluator { get; set; }
        public bool CanMakeNullMove { get; set; }
    }
}
