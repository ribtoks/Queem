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

        // constrains data
        protected int max_qs_checks = 4;

        // statistics data
        protected int nullMovesCutOffs = 0;
        protected int nullMovesResearches = 0;
        protected int nullMovesCalls = 0;

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

            this.nullMovesCutOffs = 0;
            this.nullMovesResearches = 0;
            this.nullMovesCalls = 0;

            Search(maxdepth, player, oppositePlayer, currPlayer);

            //SortEvaluatedMoves(moves);

            // of course, will be changed in future...
            //return moves[0].Move;
            return bestMove;
        }

        protected void Search(int maxdepth,
            ChessPlayerBase player, ChessPlayerBase opponentPlayer,
            CurrentPlayer currPlayer)
        {
            ply = 0;
            nodesSearched = 0;

            // TODO search debut moves here

            //List<EvaluatedMove> evaluatedMoves = new List<EvaluatedMove>();

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

                //evaluatedMoves.Add(new EvaluatedMove() { Move = move, Value = value }
                //    );
            }

            if (moves.Count == 0)
            {
                if (provider.IsCheckmate(player, opponentPlayer))
                    value = (-PositionEvaluator.MateValue + ply);
                else
                    // stalemate
                    value = 0;
            }

            //return evaluatedMoves;
        }

        protected int AlphaBetaPruning(ChessNodeInfo cni)
        {
            #region Initializations

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

            #endregion

            int value = 0;

            if (cni.Depth <= 0)
            {
                if (cni.WasNullMoveDone)
                {
                    value = -this.QuiescenceSearch(cni.GetNextQS());
                    this.DecPlayerDepth(cni.CurrPlayer);
                    return value;
                }
                else
                {
                    DecPlayerDepth(cni.CurrPlayer);

                    return cni.Evaluator(player, opponentPlayer) -
                        cni.Evaluator(opponentPlayer, player);
                }
            }

            bool wasOwnKingInCheck = provider.IsInCheck(player, opponentPlayer);

            #region Null move 

            int r = 1;
            // prepare values for null move
            cni.CanMakeNullMove &= (wasOwnKingInCheck == false);

            // TODO test if this "if" statement is needed
            if (cni.CanMakeNullMove)
            {
                cni.CanMakeNullMove &= (player.FiguresManager.Count > 5);
                cni.CanMakeNullMove &= (player.FiguresManager.WorkingFiguresCount != 0);
            }

            if (cni.CanMakeNullMove)
            {
                this.nullMovesCalls += 1;
                value = -this.AlphaBetaPruning(cni.GetNextNullMove(r));

                // null move pruning
                if (value >= cni.Beta)
                {
                    this.nullMovesCutOffs += 1;

                    DecPlayerDepth(cni.CurrPlayer);
                    return value;
                }

                this.nullMovesResearches += 1;


                if (value > cni.Alpha)
                {
                    //cni.Depth -= 1 + r;
                    cni.Alpha = value;
                }
            }


            //var currBestMoves = bestMoves[(int)currPlayer];


            #endregion

            //var currBestMoves = bestMoves[(int)currPlayer];

            MoveResult mr;

            var moves = MovesGenerator.GetSortedMoves(provider, player, opponentPlayer);

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
                // delta-pruning
                int big_delta = 0;

                mr = provider.ProvidePlayerMove(move, player.FiguresColor);
                ++ply;


                if ((mr == MoveResult.PawnReachedEnd) ||
                    (mr == MoveResult.CapturedAndPawnReachedEnd))
                {
                    PromotionType promotionFigure = (move as PromotionMove).Promotion;
                    provider.ReplacePawn(move.End,
                        (FigureType)promotionFigure);

                    big_delta += PositionEvaluator.QueenValue - 
                        PositionEvaluator.PawnValue;
                }


                value = cni.Evaluator(player, opponentPlayer) -
                    cni.Evaluator(opponentPlayer, player);

                // if value is so bad, opponent player
                // will only make it worse on his next
                // move for current player
                if (value <= cni.Alpha)
                {
                    //provider.CancelLastPlayerMove(player.FiguresColor);
                    //--ply;

                    //continue;
                    cni.Depth -= 2;
                }

                if (cni.Depth > 1)
                {
                    value = -AlphaBetaPruning(cni.GetNext());
                }
                else
                {
                    if ((mr == MoveResult.CaptureOk) || 
                        (mr == MoveResult.CapturedAndPawnReachedEnd) ||
                        (mr == MoveResult.CapturedInPassing))
                        value = -this.QuiescenceSearch(cni.GetNextQS());
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

                big_delta += PositionEvaluator.QueenValue;

                // delta pruning
                if (value < (cni.Alpha - big_delta))
                {
                    DecPlayerDepth(cni.CurrPlayer);
                    return cni.Alpha;
                }
            }

            DecPlayerDepth(cni.CurrPlayer);
            return cni.Alpha;
        }

        protected int QuiescenceSearch(QSNodeInfo qni)
        {
            #region Initializations

            var nextPlayer = qni.CurrPlayer.GetOppositePlayer();
            IncPlayerDepth(qni.CurrPlayer);
            nodesSearched += 1;

            ChessPlayerBase player = provider.Player1;
            ChessPlayerBase opponentPlayer = provider.Player2;

            if (qni.CurrPlayer == CurrentPlayer.CPU)
            {
                player = provider.Player2;
                opponentPlayer = provider.Player1;
            }

            #endregion

            int value = qni.Evaluator(player, opponentPlayer) - 
                qni.Evaluator(opponentPlayer, player);

            if (value >= qni.Beta)
            {
                DecPlayerDepth(qni.CurrPlayer);
                return qni.Beta;
            }

            if (value > qni.Alpha)
                qni.Alpha = value;

            bool wasOwnKingInCheck = provider.IsInCheck(player, opponentPlayer);

            //var currBestMoves = bestMoves[(int)currPlayer];

            MoveResult mr;

            List<ChessMove> moves = null;

            #region Moves generagion

            if (wasOwnKingInCheck)
            {
                qni.ChecksCount += 1;
                if (qni.ChecksCount > this.max_qs_checks)
                {
                    DecPlayerDepth(qni.CurrPlayer);
                    return qni.Alpha;
                }

                moves = MovesGenerator.GetPlayerMoves(provider,
                    player, opponentPlayer);
                
                // checkmate
                if (moves.Count == 0)
                {
                    DecPlayerDepth(qni.CurrPlayer);
                    return (-PositionEvaluator.MateValue + ply);
                }
            }
            else
            {
                moves = MovesGenerator.GetAttackingPlayerMoves(provider, 
                    player, opponentPlayer);

                // stalemate
                if (moves.Count == 0)
                {
                    DecPlayerDepth(qni.CurrPlayer);
                    return qni.Alpha;
                }
            }

            #endregion

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
                
                value = -this.QuiescenceSearch(qni.GetNextQS());

                provider.CancelLastPlayerMove(player.FiguresColor);
                --ply;

                if (value > qni.Alpha)
                {
                    // paste history heuristics here...

                    if (value >= qni.Beta)
                    {
                        DecPlayerDepth(qni.CurrPlayer);
                        return qni.Beta;
                    }

                    qni.Alpha = value;
                }
            }

            DecPlayerDepth(qni.CurrPlayer);
            return qni.Alpha;
        }
    }
}
