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
        public int Alpha { get; set; }
        public int Beta { get; set; }
        public CurrentPlayer CurrPlayer { get; set; }
        public int Depth { get; set; }
        public EvaluatorDelegate Evaluator { get; set; }
        public bool CanMakeNullMove { get; set; }
        protected bool ableToMakeNullMoves;

        public ChessNodeInfo()
        {
            ableToMakeNullMoves = true;
        }

        public ChessNodeInfo(bool _ableToMakeNullMoves)
        {
            this.ableToMakeNullMoves = _ableToMakeNullMoves;
        }

        public ChessNodeInfo(ChessNodeInfo cni)
        {
            this.Alpha = cni.Alpha;
            this.Beta = cni.Beta;
            this.CurrPlayer = cni.CurrPlayer;
            this.Depth = cni.Depth;
            this.Evaluator = cni.Evaluator;
            this.CanMakeNullMove = cni.CanMakeNullMove;

            // private
            this.ableToMakeNullMoves = cni.ableToMakeNullMoves;
        }

        public ChessNodeInfo GetNext()
        {
            ChessNodeInfo cni = new ChessNodeInfo();
            cni.Alpha = -this.Beta;
            cni.Beta = -this.Alpha;
            cni.Depth = this.Depth - 1;
            cni.Evaluator = this.Evaluator;
            cni.CurrPlayer = this.CurrPlayer.GetOppositePlayer();
            cni.CanMakeNullMove = this.ableToMakeNullMoves;

            return cni;
        }

        public ChessNodeInfo GetNextNullMove(int r)
        {
            ChessNodeInfo cni = new ChessNodeInfo();
            cni.Alpha = -this.Beta;
            cni.Beta = -this.Alpha; // -this.Beta + 1;
            cni.Depth = this.Depth - 1 - r;
            cni.Evaluator = this.Evaluator;
            cni.CurrPlayer = this.CurrPlayer.GetOppositePlayer();
            cni.CanMakeNullMove = false;

            return cni;
        }

        public ChessNodeInfo GetNextQS()
        {
            ChessNodeInfo cni = new ChessNodeInfo();
            cni.Alpha = -this.Beta;
            cni.Beta = -this.Alpha;
            cni.Depth = 0;// this.Depth - 1;
            cni.Evaluator = PositionEvaluator.SimpleEvaluatePosition;
            cni.CurrPlayer = this.CurrPlayer.GetOppositePlayer();

            cni.ableToMakeNullMoves = false;
            cni.CanMakeNullMove = false;

            return cni;
        }

        public void DisableNullMove()
        {
            this.ableToMakeNullMoves = false;
        }
    }
}
