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
        public bool WasNullMoveDone { get; protected set; }

        public ChessNodeInfo()
        {
            ableToMakeNullMoves = true;
            WasNullMoveDone = false;
        }

        public ChessNodeInfo(bool _ableToMakeNullMoves)
        {
            this.ableToMakeNullMoves = _ableToMakeNullMoves;
            WasNullMoveDone = false;
        }

        public ChessNodeInfo(ChessNodeInfo cni)
        {
            this.Alpha = cni.Alpha;
            this.Beta = cni.Beta;
            this.CurrPlayer = cni.CurrPlayer;
            this.Depth = cni.Depth;
            this.Evaluator = cni.Evaluator;
            this.CanMakeNullMove = cni.CanMakeNullMove;

            this.WasNullMoveDone = cni.WasNullMoveDone;
            // private
            this.ableToMakeNullMoves = cni.ableToMakeNullMoves;
        }

        public virtual ChessNodeInfo GetNext()
        {
            ChessNodeInfo cni = new ChessNodeInfo();
            cni.Alpha = -this.Beta;
            cni.Beta = -this.Alpha;
            cni.Depth = this.Depth - 1;
            cni.Evaluator = this.Evaluator;
            cni.CurrPlayer = this.CurrPlayer.GetOppositePlayer();

            cni.CanMakeNullMove =  this.ableToMakeNullMoves;
            cni.WasNullMoveDone = this.WasNullMoveDone;

            return cni;
        }

        public virtual ChessNodeInfo GetNextNullMove(int r)
        {
            ChessNodeInfo cni = new ChessNodeInfo();
            cni.Alpha = -this.Beta;
            cni.Beta = -this.Alpha; // -this.Beta + 1;
            cni.Depth = this.Depth - 1 - r;
            cni.Evaluator = this.Evaluator;
            cni.CurrPlayer = this.CurrPlayer.GetOppositePlayer();
            cni.CanMakeNullMove = false;
            cni.WasNullMoveDone = true;

            return cni;
        }

        public virtual QSNodeInfo GetNextQS()
        {
            QSNodeInfo qni = new QSNodeInfo();
            qni.Alpha = -this.Beta;
            qni.Beta = -this.Alpha;
            // max depth for quiescence
            qni.Depth = 20; //this.Depth - 1;
            qni.Evaluator = PositionEvaluator.SimpleEvaluatePosition;
            qni.CurrPlayer = this.CurrPlayer.GetOppositePlayer();
            qni.ChecksCount = 0;
            qni.ableToMakeNullMoves = false;
            qni.CanMakeNullMove = false;
            qni.WasNullMoveDone = this.WasNullMoveDone;

            return qni;
        }

        public virtual void DisableNullMove()
        {
            this.ableToMakeNullMoves = false;
        }
    }

    public class QSNodeInfo : ChessNodeInfo
    {
        public int ChecksCount { get; set; }

        public QSNodeInfo()
        {
            ableToMakeNullMoves = true;
        }

        public QSNodeInfo(bool _ableToMakeNullMoves)
        {
            this.ableToMakeNullMoves = _ableToMakeNullMoves;
        }

        public QSNodeInfo(ChessNodeInfo cni)
            : base(cni)
        {
        }

        public QSNodeInfo(QSNodeInfo qni)
            : base(qni)
        {
            this.ChecksCount = qni.ChecksCount;
        }

        public override QSNodeInfo GetNextQS()
        {
            QSNodeInfo qni = new QSNodeInfo();
            qni.Alpha = -this.Beta;
            qni.Beta = -this.Alpha;
            qni.Depth = this.Depth - 1;
            qni.Evaluator = this.Evaluator;
            qni.CurrPlayer = this.CurrPlayer.GetOppositePlayer();
            qni.CanMakeNullMove = this.CanMakeNullMove;
            qni.ChecksCount = this.ChecksCount;

            return qni;
        }
    }
}
