﻿using System;
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

        public ChessMove SolveProblem(MovesProvider snapshot, FigureColor playerColor, int maxdepth)
        {
            provider = snapshot;
            var moves = Search(maxdepth);
            SortEvaluatedMoves(moves);

            // of course, will be changed in future...
            //return moves[0].Move;
            return bestMove;
        }

        protected List<EvaluatedMove> Search(int maxdepth)
        {
            ply = 0;
            nodesSearched = 0;

            List<EvaluatedMove> evaluatedMoves = new List<EvaluatedMove>();

            int alpha = -PositionEvaluator.MateValue;
            int beta = PositionEvaluator.MateValue;
            int value = 0;

            InitializeBestMoves();
            InitializePlayerDepth();

            var player = provider.Player2;
            var opponentPlayer = provider.Player1;
            CurrentPlayer nextPlayer = CurrentPlayer.Me;
            MoveResult mr = MoveResult.Fail;

            nodesSearched += 1;
            var moves = GetPlayerMoves(player, opponentPlayer);
            for (int i = 0; i < moves.Count; ++i)
            {
                var move = moves[i];
                mr = provider.ProvidePlayerMove(move, player, opponentPlayer);

                if ((mr == MoveResult.PawnReachedEnd) ||
                    (mr == MoveResult.CapturedAndPawnReachedEnd))
                {
                    PromotionType promotionFigure = (move as PromotionMove).Promotion;
                    provider.ReplacePawnAtTheOtherSide(move.End, (FigureType)promotionFigure, player);
                }

                if (maxdepth > 1)
                    value = -AlphaBetaPruning(-beta, -alpha, maxdepth - 1, nextPlayer);
                else
                    value = EvaluatePosition(player, opponentPlayer) - EvaluatePosition(opponentPlayer, player);

                provider.CancelLastPlayerMove(player, opponentPlayer);
                --ply;

                if (value > alpha)
                {
                    // paste history heuristics here...
                    bestMove = move;
                    alpha = value;
                }

                evaluatedMoves.Add(new EvaluatedMove() 
                    { Move = move, Value = value }
                    );
            }

            // TODO check for checkmate and stalemate

            //return snapshot.GetFilteredCells(
            return evaluatedMoves;
        }

        protected int AlphaBetaPruning(int alpha, int beta, int depth, CurrentPlayer currPlayer)
        {
            var nextPlayer = currPlayer.GetOppositePlayer();
            IncPlayerDepth(currPlayer);
            nodesSearched += 1;

            ChessPlayerBase player = provider.Player1;
            ChessPlayerBase opponentPlayer = provider.Player2;

            if (currPlayer == CurrentPlayer.CPU)
            {
                player = provider.Player2;
                opponentPlayer = provider.Player1;
            }
            //var currBestMoves = bestMoves[(int)currPlayer];

            bool wasOwnKingInCheck = provider.IsCheckmate(player, opponentPlayer);

            int value = 0;
            MoveResult mr;

            var moves = GetPlayerMoves(player, opponentPlayer);

            if (moves.Count == 0)
            {
                DecPlayerDepth(currPlayer);
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

                mr = provider.ProvidePlayerMove(move, player, opponentPlayer);
                ++ply;

                if ((mr == MoveResult.PawnReachedEnd) ||
                    (mr == MoveResult.CapturedAndPawnReachedEnd))
                {
                    PromotionType promotionFigure = (move as PromotionMove).Promotion;
                    provider.ReplacePawnAtTheOtherSide(move.End, (FigureType)promotionFigure, player);
                }

                value = EvaluatePosition(player, opponentPlayer) - EvaluatePosition(opponentPlayer, player);

                if (depth > 1)
                    value = -AlphaBetaPruning(-beta, -alpha, depth - 1, nextPlayer);

                provider.CancelLastPlayerMove(player, opponentPlayer);
                --ply;

                if (value > alpha)
                {
                    // paste history heuristics here...

                    if (value >= beta)
                    {
                        DecPlayerDepth(currPlayer);
                        return beta;
                    }

                    alpha = value;
                }
            }

            DecPlayerDepth(currPlayer);
            return alpha;
        }

        protected List<ChessMove> GetPlayerMoves(ChessPlayerBase player, ChessPlayerBase opponent)
        {
            // in future, use sorted dictionary
            var moves = new List<ChessMove>(30);
            ChessMove move = new ChessMove();

            var pMoves = new List<ChessMove>();
            foreach (var pawn in player.FiguresManager.Pawns)
            {
                var pawnMoves = provider.GetFilteredCells(pawn.Coordinates, player, opponent);
                move.Start = pawn.Coordinates;

                for (int i = 0; i < pawnMoves.Count; ++i)
                {
                    move.End = pawnMoves[i];
                    // pawn reached other board side
                    if ((move.End.Y == 0) ||
                        (move.End.Y == 7))
                    {
                        pMoves.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Queen));

                        pMoves.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Rook));

                        pMoves.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Bishop));

                        pMoves.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Horse));
                    }
                    else
                        pMoves.Add(new ChessMove(move));
                }
            }

            moves.AddRange(pMoves);

            foreach (var horse in player.FiguresManager.Horses)
            {
                move.Start = horse.Coordinates;
                var horseMoves = provider.GetFilteredCells(horse.Coordinates, player, opponent);

                for (int i = 0; i < horseMoves.Count; ++i)
                {
                    move.End = horseMoves[i];
                    moves.Add(new ChessMove(move));
                }
            }

            foreach (var bishop in player.FiguresManager.Bishops)
            {
                move.Start = bishop.Coordinates;
                var bishopMoves = provider.GetFilteredCells(bishop.Coordinates, player, opponent);

                for (int i = 0; i < bishopMoves.Count; ++i)
                {
                    move.End = bishopMoves[i];
                    moves.Add(new ChessMove(move));
                }
            }

            foreach (var rook in player.FiguresManager.Rooks)
            {
                move.Start = rook.Coordinates;
                var rookMoves = provider.GetFilteredCells(rook.Coordinates, player, opponent);

                for (int i = 0; i < rookMoves.Count; ++i)
                {
                    move.End = rookMoves[i];
                    moves.Add(new ChessMove(move));
                }
            }

            foreach (var queen in player.FiguresManager.Queens)
            {
                move.Start = queen.Coordinates;
                var queenMoves = provider.GetFilteredCells(queen.Coordinates, player, opponent);

                for (int i = 0; i < queenMoves.Count; ++i)
                {
                    move.End = queenMoves[i];
                    moves.Add(new ChessMove(move));
                }
            }

            move.Start = player.FiguresManager.Kings.King.Coordinates;
            var kingMoves = provider.GetFilteredCells(move.Start, player, opponent);

            for (int i = 0; i < kingMoves.Count; ++i)
            {
                move.End = kingMoves[i];
                moves.Add(new ChessMove(move));
            }

            return moves;
        }
    }
}
