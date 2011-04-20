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
        protected int bestMoveValue = 0;
        protected TranspositionTable cacheTable;
        
        // constrains data
        protected int max_qs_checks = 4;

        // statistics data
        protected int reusedPositions = 0;
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

            cacheTable = new TranspositionTable();

            this.nullMovesCutOffs = 0;
            this.nullMovesResearches = 0;
            this.nullMovesCalls = 0;

            this.reusedPositions = 0;

            //List<EvaluatedMove> evaluatedMoves = new List<EvaluatedMove>();
            this.bestMoveValue = -PositionEvaluator.MateValue;
            for (int d = 3; d < maxdepth; d++)
            {
                int value = Search(d, player, oppositePlayer, currPlayer);
            }

            //SortEvaluatedMoves(evaluatedMoves);

            // of course, will be changed in future...
            //return evaluatedMoves[0].Move;
            return bestMove;
        }

        protected int Search(int maxdepth,
            ChessPlayerBase player, ChessPlayerBase opponentPlayer,
            CurrentPlayer currPlayer)
        {
            ply = 0;
            nodesSearched = 0;

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
            /*
            bool wasOwnKingInCheck = provider.IsInCheck(player, opponentPlayer);
            
            #region Null move region

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
                
                this.nullMovesResearches += 1;

                if (value > cni.Alpha)
                {
                    //cni.Depth -= 1 + r;
                    cni.Alpha = value;
                }
            }

            #endregion
            */
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

                if (maxdepth > 1)
                    value = -AlphaBetaPruning(cni.GetNext());

                provider.CancelLastPlayerMove(player.FiguresColor);
                --ply;

                if (value > cni.Alpha)
                {
                    // paste history heuristics here...
                    cni.Alpha = value;
                }
                
                if (value > bestMoveValue)
                {
                    bestMoveValue = value;
                    bestMove = move;
                }
            }

            if (moves.Count == 0)
            {
                if (provider.IsCheckmate(player, opponentPlayer))
                    value = (-PositionEvaluator.MateValue + ply);
                else
                    // stalemate
                    value = 0;
            }

            return value;
        }

        protected int AlphaBetaPruning(ChessNodeInfo cni)
        {
            #region Initializations

            var nextPlayer = cni.CurrPlayer.GetOppositePlayer();
            //IncPlayerDepth(cni.CurrPlayer);
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
            
            #region Transposition table stuff

            TableMoveInfo cachedValue = cacheTable[provider.ChessBoard.HashCode];
            if ((cachedValue != null) &&
                (cachedValue.Depth >= cni.Depth))
            {
                reusedPositions += 1;
                switch (cachedValue.Type)
                {
                    case TableValueType.Exact:
                        return cachedValue.Value;
                    case TableValueType.LowerBound:
                        if (cachedValue.Value > cni.Alpha)
                            cni.Alpha = cachedValue.Value;
                        break;
                    case TableValueType.UpperBound:
                        if (cachedValue.Value < cni.Beta)
                            cni.Beta = cachedValue.Value;
                        break;
                }
                if (cni.Alpha >= cni.Beta)
                    return cachedValue.Value;
            }

            #endregion
            
            #region Depth finished stuff

            if (cni.Depth <= 0)
            {
                if (cni.WasNullMoveDone)
                {
                    value = -this.QuiescenceSearch(cni.GetNextQS());
                }
                else
                {
                    value = cni.Evaluator(player, opponentPlayer) -
                        cni.Evaluator(opponentPlayer, player);
                }

                TableValueType type = TableValueType.Exact;
                if (value <= cni.Alpha)
                {
                    type = TableValueType.LowerBound;
                }
                else if (value >= cni.Beta)
                {
                    type = TableValueType.UpperBound;
                }

                cacheTable.AddPosition(provider.ChessBoard.HashCode,
                        value, type, cni.Depth);

                return value;
            }

            #endregion

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

                    cacheTable.AddPosition(provider.ChessBoard.HashCode,
                            value, TableValueType.UpperBound, cni.Depth);

                    return value;
                }

                this.nullMovesResearches += 1;

                if (value > cni.Alpha)
                {
                    //cni.Depth -= 1 + r;
                    cni.Alpha = value;
                }
            }
            
            #endregion

            //var currBestMoves = bestMoves[(int)currPlayer];

            MoveResult mr;

            var moves = MovesGenerator.GetSortedMoves(provider, player, opponentPlayer);

            if (moves.Count == 0)
            {
                if (wasOwnKingInCheck)
                {
                    // checkmate
                    value = (-PositionEvaluator.MateValue + ply);

                    cacheTable.AddPosition(provider.ChessBoard.HashCode,
                       value, TableValueType.Exact, cni.Depth);

                    return value;
                }
                else
                {
                    cacheTable.AddPosition(provider.ChessBoard.HashCode,
                       0, TableValueType.Exact, cni.Depth);
                    // position is a stalemate
                    return 0;
                }
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
                    cni.Depth -= 1;
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
                        cacheTable.AddPosition(provider.ChessBoard.HashCode,
                            cni.Beta, TableValueType.UpperBound, cni.Depth);

                        return cni.Beta;
                    }

                    cni.Alpha = value;
                }

                big_delta += PositionEvaluator.QueenValue;

                // delta pruning
                if (value < (cni.Alpha - big_delta))
                {
                    cacheTable.AddPosition(provider.ChessBoard.HashCode,
                        cni.Alpha, TableValueType.LowerBound, cni.Depth);

                    return cni.Alpha;
                }
            }

            cacheTable.AddPosition(provider.ChessBoard.HashCode,
                    cni.Alpha, TableValueType.LowerBound, cni.Depth);

            return cni.Alpha;
        }

        protected int QuiescenceSearch(QSNodeInfo qni)
        {
            #region Initializations

            var nextPlayer = qni.CurrPlayer.GetOppositePlayer();
            nodesSearched += 1;

            ChessPlayerBase player = provider.Player1;
            ChessPlayerBase opponentPlayer = provider.Player2;

            if (qni.CurrPlayer == CurrentPlayer.CPU)
            {
                player = provider.Player2;
                opponentPlayer = provider.Player1;
            }

            #endregion

            #region Transposition table stuff

            TableMoveInfo cachedValue = cacheTable[provider.ChessBoard.HashCode];
            if ((cachedValue != null) &&
                (cachedValue.Depth >= qni.Depth))
            {
                reusedPositions += 1;

                switch (cachedValue.Type)
                {
                    case TableValueType.Exact:
                        return cachedValue.Value;
                    case TableValueType.LowerBound:
                        if (cachedValue.Value > qni.Alpha)
                            qni.Alpha = cachedValue.Value;
                        break;
                    case TableValueType.UpperBound:
                        if (cachedValue.Value < qni.Beta)
                            qni.Beta = cachedValue.Value;
                        break;
                }
                if (qni.Alpha >= qni.Beta)
                    return cachedValue.Value;
            }

            #endregion

            int value = qni.Evaluator(player, opponentPlayer) - 
                qni.Evaluator(opponentPlayer, player);

            if (value >= qni.Beta)
            {
                cacheTable.AddPosition(provider.ChessBoard.HashCode,
                    qni.Beta, TableValueType.UpperBound, qni.Depth);

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
                    cacheTable.AddPosition(provider.ChessBoard.HashCode,
                        qni.Alpha, TableValueType.LowerBound, qni.Depth);

                    return qni.Alpha;
                }

                moves = MovesGenerator.GetPlayerMoves(provider,
                    player, opponentPlayer);
                
                // checkmate
                if (moves.Count == 0)
                {
                    value = (-PositionEvaluator.MateValue + ply);

                    cacheTable.AddPosition(provider.ChessBoard.HashCode,
                        value, TableValueType.Exact, qni.Depth);

                    return value;
                }
            }
            else
            {
                moves = MovesGenerator.GetAttackingPlayerMoves(provider, 
                    player, opponentPlayer);

                // no moves left - end of quiescence
                if (moves.Count == 0)
                {
                    cacheTable.AddPosition(provider.ChessBoard.HashCode,
                        qni.Alpha, TableValueType.Exact, qni.Depth);

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
                        cacheTable.AddPosition(provider.ChessBoard.HashCode,
                            qni.Beta, TableValueType.UpperBound, qni.Depth);

                        return qni.Beta;
                    }

                    qni.Alpha = value;
                }
            }
            
            cacheTable.AddPosition(provider.ChessBoard.HashCode,
                    qni.Alpha, TableValueType.LowerBound, qni.Depth);
            
            return qni.Alpha;
        }
    }
}
