using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;
using DebutMovesHolder;

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
        protected CoordsTable<CoordsTable<int>> historyTable;
        protected DebutGraph debutGraph;

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

        public ChessSolver(DebutGraph graph)
        {
            this.debutGraph = graph;
        }

        /// <summary>
        /// Tries to get predefined reply
        /// on last opponents move
        /// </summary>
        /// <returns></returns>
        protected bool FindDebutMove()
        {
            if (this.debutGraph == null)
                return false;

            var end_iter = DebutGraph.MoveIterator.End;
            DebutGraph.MoveIterator mi = this.debutGraph.CheckMoves(
                provider.History.Moves);

            if (mi == end_iter)
                return false;

            // gets the random reply on this move
            ++mi;

            if (mi == end_iter)
                return false;

            this.bestMove = mi.CurrentNode.Move;
            return true;
        }

        public ChessMove SolveProblem(MovesProvider snapshot, FigureColor playerColor, int maxdepth)
        {
            #region Initializations

            this.provider = snapshot;
            var player = provider.Player1;
            //var oppositePlayer = provider.Player2;
            CurrentPlayer currPlayer = CurrentPlayer.Me;

            if (playerColor != player.FiguresColor)
            {
                //player = provider.Player2;
                //oppositePlayer = provider.Player1;
                currPlayer = CurrentPlayer.CPU;
            }

            #endregion

            if (FindDebutMove())
                return this.bestMove;

            this.cacheTable = new TranspositionTable();
            this.historyTable = new CoordsTable<CoordsTable<int>>();

            this.nullMovesCutOffs = 0;
            this.nullMovesResearches = 0;
            this.nullMovesCalls = 0;

            this.reusedPositions = 0;

            int firstguess = -PositionEvaluator.MateValue;
            this.bestMoveValue = firstguess;            

            if (maxdepth < 4)
                maxdepth = 4;

            this.bestMove = null;
            bool isFirst = true;
            // some statistics data
            int all_nodes = 0;
            
            for (int d = 3; d < maxdepth; d++)
            {
                this.ply = 0;
                this.nodesSearched = 0;

                int lower = -PositionEvaluator.MateValue;
                int upper = PositionEvaluator.MateValue;

                int window = PositionEvaluator.PawnValue;

                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    lower = firstguess - window;
                    upper = firstguess + window;
                }

                ChessNodeInfo cni = new ChessNodeInfo()
                {
                    Alpha = lower,
                    Beta = upper,
                    Evaluator = PositionEvaluator.SimpleEvaluatePosition,
                    CanMakeNullMove = true,
                    CurrPlayer = currPlayer,
                    Depth = d
                };

                firstguess = this.Search(cni);

                all_nodes += this.nodesSearched;
            }

            return bestMove;
        }

        protected int Search(ChessNodeInfo cni)
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

            MovesGenParams movesGenParams = new MovesGenParams()
            {
                Provider = provider,
                Player = player,
                OpponentPlayer = opponentPlayer,
                HistoryTable = historyTable
            };

            #endregion

            int value = 0;

            //bool wasOwnKingInCheck = provider.IsInCheck(player, opponentPlayer);
            
            MoveResult mr = MoveResult.Fail;

            var moves = MovesGenerator.GetSortedMoves(movesGenParams);

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

                value = -AlphaBetaPruning(cni.GetNext());

                provider.CancelLastPlayerMove(player.FiguresColor);
                --ply;

                if (value > cni.Alpha)
                {
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

            MovesGenParams movesGenParams = new MovesGenParams()
            {
                Provider = provider,
                Player = player,
                OpponentPlayer = opponentPlayer,
                HistoryTable = historyTable
            };

            #endregion

            TableValueType type;
            int value = 0;
            int depth = cni.Depth;
            int alpha = cni.Alpha;
            int beta = cni.Beta;

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

                type = TableValueType.Exact;
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

            bool wasOwnKingInCheck = provider.IsInCheck(player, opponentPlayer);

            #region Null move

            int r = 1;
            // prepare values for null move
            cni.CanMakeNullMove &= (wasOwnKingInCheck == false);
            cni.CanMakeNullMove &= (cni.Depth > 2);
            // cannot use null move in end of game
            cni.CanMakeNullMove &= ((player.FiguresManager.Count + 
                opponentPlayer.FiguresManager.Count) > 7);

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
            
            MoveResult mr;
            var moves = MovesGenerator.GetSortedMoves(movesGenParams);

            #region No moves region

            if (moves.Count == 0)
            {
                if (wasOwnKingInCheck)
                {
                    // checkmate
                    value = (-PositionEvaluator.MateValue + ply);

                    //cacheTable.AddPosition(provider.ChessBoard.HashCode,
                    //   value, TableValueType.Exact, cni.Depth);

                    return value;
                }
                else
                {
                    //cacheTable.AddPosition(provider.ChessBoard.HashCode,
                    //   0, TableValueType.Exact, cni.Depth);
                    // position is a stalemate
                    return 0;
                }
            }

            #endregion

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

                    // extension here
                    cni.Depth += 1;

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

                #region Extensions/Reductions region

                bool canMakeReduction = true;
                canMakeReduction &= cni.CanMakeNullMove;
                if (canMakeReduction)
                {
                    canMakeReduction &= (!wasOwnKingInCheck);
                    canMakeReduction &= (mr == MoveResult.MoveOk);
                    //canMakeReduction &= (depth > 2);
                }
                if (canMakeReduction)
                    cni.Depth -= 1;

                bool canMakeExtension = true;
                canMakeExtension &= cni.CanMakeNullMove;
                if (canMakeExtension)
                {
                    canMakeExtension &= wasOwnKingInCheck;
                    canMakeExtension &= (mr == MoveResult.CaptureOk);
                    canMakeExtension &= (depth > 2);
                }
                if (canMakeExtension)
                    cni.Depth += 1;

                #endregion

                if (depth > 1)
                {
                    value = -AlphaBetaPruning(cni.GetNext());
                }
                else
                {
                    if ((mr == MoveResult.CaptureOk) ||
                        (mr == MoveResult.CapturedAndPawnReachedEnd) ||
                        (mr == MoveResult.CapturedInPassing) ||
                        wasOwnKingInCheck)
                        value = -this.QuiescenceSearch(cni.GetNextQS());
                }

                provider.CancelLastPlayerMove(player.FiguresColor);
                --ply;

                if (value > cni.Alpha)
                {
                    if (value >= cni.Beta)
                    {
                        cacheTable.AddPosition(provider.ChessBoard.HashCode,
                            cni.Beta, TableValueType.UpperBound, cni.Depth);

                        // save history move
                        if (mr == MoveResult.MoveOk)
                            historyTable[move.Start][move.End] += (1 << cni.Depth);

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
                    cni.Alpha, TableValueType.Exact, cni.Depth);

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

            TableValueType type;

            int value =  qni.Evaluator(player, opponentPlayer) -
                 qni.Evaluator(opponentPlayer, player);

            if (value > qni.Beta)
            {
                cacheTable.AddPosition(provider.ChessBoard.HashCode,
                    qni.Beta, TableValueType.UpperBound, qni.Depth);

                return qni.Beta;
            }

            if (value > qni.Alpha)
                qni.Alpha = value;

            if (qni.Depth <= 0)
            {
                type = TableValueType.Exact;
                if (value <= qni.Alpha)
                {
                    type = TableValueType.LowerBound;
                }
                else if (value >= qni.Beta)
                {
                    type = TableValueType.UpperBound;
                }

                cacheTable.AddPosition(provider.ChessBoard.HashCode,
                        value, type, qni.Depth);

                return value;
            }

            bool wasOwnKingInCheck = provider.IsInCheck(player, opponentPlayer);

            MoveResult mr;
            List<ChessMove> moves = null;

            #region Moves generation

            if (wasOwnKingInCheck)
            {
                qni.ChecksCount += 1;
                if (qni.ChecksCount > this.max_qs_checks)
                {
                    // if value > alpha, alpha was 
                    // assigned to value before
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

                    //cacheTable.AddPosition(provider.ChessBoard.HashCode,
                    //    value, TableValueType.Exact, qni.Depth);

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
                    type = TableValueType.Exact;
                    if (value <= qni.Alpha)
                    {
                        type = TableValueType.LowerBound;
                    }
                    else if (value >= qni.Beta)
                    {
                        type = TableValueType.UpperBound;
                    }

                    cacheTable.AddPosition(provider.ChessBoard.HashCode,
                            value, type, qni.Depth);

                    return value;
                }
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
