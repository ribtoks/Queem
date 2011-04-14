using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace QueemAI
{
    public partial class ChessSolver
    {
        protected MovesProvider provider;
        protected int ply;
        protected int nodesSearched;
        protected List<EvaluatedMove>[] bestMoves = new List<EvaluatedMove>[2];
        protected int[] playerDepth = new int[2];
        protected ChessMove bestMove = null;

        #region Internal stuff

        internal MovesProvider Provider
        {
            get { return provider; }
        }
        
        internal ChessSolver(MovesProvider snapshot)
        {
            provider = snapshot;
        }

        #endregion

        public ChessSolver()
        {
        }

        public ChessMove SolveProblem(MovesProvider snapshot, FigureColor playerColor, int maxdepth)
        {
            provider = snapshot;
            bestMove = null;
            var player = provider.Player1;
            var oppositePlayer = provider.Player2;
            CurrentPlayer currPlayer = CurrentPlayer.Me;

            if (playerColor != player.FiguresColor)
            {
                player = provider.Player2;
                oppositePlayer = provider.Player1;
                currPlayer = CurrentPlayer.CPU;
            }

            var moves = Search(maxdepth, player, oppositePlayer, currPlayer);
            //SortEvaluatedMoves(moves);

            // of course, will be changed in future...
            //return moves[0].Move;
            return bestMove;
        }

        protected List<EvaluatedMove> Search(int maxdepth,
            ChessPlayerBase player, ChessPlayerBase opponentPlayer,
            CurrentPlayer currPlayer)
        {
            ply = 0;
            nodesSearched = 0;

            // TODO search debut moves here

            List<EvaluatedMove> evaluatedMoves = new List<EvaluatedMove>();

            ChessNodeInfo cni = new ChessNodeInfo()
            {
                Alpha = -PositionEvaluator.MateValue,
                Beta = PositionEvaluator.MateValue,
                Evaluator = PositionEvaluator.SimpleEvaluatePosition,
                CanMakeNullMove = true,
                CurrPlayer = currPlayer,
                Depth = maxdepth
            };

            int value = 0;

            InitializeBestMoves();
            InitializePlayerDepth();

            CurrentPlayer nextPlayer = currPlayer.GetOppositePlayer();
            MoveResult mr = MoveResult.Fail;

            nodesSearched += 1;
            var moves = MovesGenerator.GetPlayerMoves(provider, player, opponentPlayer);
            for (int i = 0; i < moves.Count; ++i)
            {
                var move = moves[i];
                mr = provider.ProvidePlayerMove(move, player.FiguresColor);

                if ((mr == MoveResult.PawnReachedEnd) ||
                    (mr == MoveResult.CapturedAndPawnReachedEnd))
                {
                    PromotionType promotionFigure = (move as PromotionMove).Promotion;
                    provider.ReplacePawn(move.End, 
                        (FigureType)promotionFigure);
                }

                value = cni.Evaluator(player, opponentPlayer) -
                    cni.Evaluator(opponentPlayer, player);

                if (value <= cni.Alpha)
                {
                    provider.CancelLastPlayerMove(player.FiguresColor);
                    --ply;

                    continue;
                }

                if (maxdepth > 1)
                    value = -AlphaBetaPruning(cni.GetNext());

                provider.CancelLastPlayerMove(player.FiguresColor);
                --ply;

                if (value > cni.Alpha)
                {
                    // paste history heuristics here...
                    bestMove = move;
                    cni.Alpha = value;
                }

                evaluatedMoves.Add(new EvaluatedMove() { Move = move, Value = value }
                    );
            }

            if (moves.Count == 0)
            {
                if (provider.IsCheckmate(player, opponentPlayer))
                    value = (-PositionEvaluator.MateValue + ply);
                else
                    // stalemate
                    value = 0;
            }

            return evaluatedMoves;
        }

        protected int AlphaBetaPruning(ChessNodeInfo cni)
        {
            var nextPlayer = cni.CurrPlayer.GetOppositePlayer();
            IncPlayerDepth(cni.CurrPlayer);
            nodesSearched += 1;

            ChessPlayerBase player = provider.Player1;
            ChessPlayerBase opponentPlayer = provider.Player2;

            if (cni.CurrPlayer == CurrentPlayer.CPU)
            {
                player = provider.Player2;
                opponentPlayer = provider.Player1;
            }
            //var currBestMoves = bestMoves[(int)currPlayer];

            bool wasOwnKingInCheck = provider.IsCheckmate(player, opponentPlayer);

            int value = 0;
            MoveResult mr;

            var moves = MovesGenerator.GetPlayerMoves(provider, player, opponentPlayer);

            if (moves.Count == 0)
            {
                DecPlayerDepth(cni.CurrPlayer);

                if (wasOwnKingInCheck)
                    return (-PositionEvaluator.MateValue + ply);
                else
                    // position is a stalemate
                    return 0;
            }

            ChessMove move = new ChessMove();
            for (int i = 0; i < moves.Count; ++i)
            {
                move = moves[i];

                mr = provider.ProvidePlayerMove(move, player.FiguresColor);
                ++ply;

                if ((mr == MoveResult.PawnReachedEnd) ||
                    (mr == MoveResult.CapturedAndPawnReachedEnd))
                {
                    PromotionType promotionFigure = (move as PromotionMove).Promotion;
                    provider.ReplacePawn(move.End,
                        (FigureType)promotionFigure);
                }

                value = cni.Evaluator(player, opponentPlayer) -
                    cni.Evaluator(opponentPlayer, player);

                // if value is so bad, opponent player
                // will only make it worse on his next
                // move for current player
                if (value <= cni.Alpha)
                {
                    provider.CancelLastPlayerMove(player.FiguresColor);
                    --ply;

                    continue;
                }

                if (cni.Depth > 1)
                {                    
                    value = -AlphaBetaPruning(cni.GetNext());
                }

                provider.CancelLastPlayerMove(player.FiguresColor);
                --ply;

                if (value > cni.Alpha)
                {
                    // paste history heuristics here...

                    if (value >= cni.Beta)
                    {
                        DecPlayerDepth(cni.CurrPlayer);
                        return cni.Beta;
                    }

                    cni.Alpha = value;
                }
            }

            DecPlayerDepth(cni.CurrPlayer);
            return cni.Alpha;
        }

        protected int QuiescenceSearch(ChessNodeInfo cni)
        {
            var nextInfo = cni.GetNext();
            nextInfo.Evaluator = PositionEvaluator.SophisticatedEvaluatePosition;
            nextInfo.CanMakeNullMove = false;

            return AlphaBetaPruning(nextInfo);
        }
    }
}
