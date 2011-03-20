using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace QueemAI
{
    public enum CurrentPlayer { Me, CPU }

    public static class CurrentPlayerExtensions
    {
        public static CurrentPlayer GetOppositePlayer(this CurrentPlayer player)
        {
            if (player == CurrentPlayer.Me)
                return CurrentPlayer.CPU;
            return CurrentPlayer.Me;
        }
    }

    public class ChessSolver
    {
        protected MovesProvider provider;
        protected CurrentPlayer currPlayer;
        protected int ply;
        protected int nodesSearched;

        public ChessMove SolveProblem(MovesProvider snapshot, FigureColor playerColor)
        {
            ply = 0;
            //return snapshot.GetFilteredCells(
            return new ChessMove();
        }

        protected int AlphaBetaPruning(int alpha, int beta, int depth)
        {
            nodesSearched += 1;

            ChessPlayerBase player = provider.Player1;
            ChessPlayerBase opponentPlayer = provider.Player2;

            if (currPlayer == CurrentPlayer.CPU)
            {
                player = provider.Player2;
                opponentPlayer = provider.Player1;
            }
            currPlayer = currPlayer.GetOppositePlayer();

            bool wasOwnKingInCheck = provider.IsCheckmate(player, opponentPlayer);

            int value = 0;
            MoveResult mr;

            var moves = GetPlayerMoves(player, opponentPlayer);

            if (moves.Count == 0)
            {
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
                    FigureType promotionFigure = (move as ChessPromotionMove).Promotion;
                    provider.ReplacePawnAtTheOtherSide(move.End, promotionFigure, player);
                }

                value = EvaluatePosition(player, opponentPlayer) - EvaluatePosition(opponentPlayer, player);

                if (depth > 1)
                    value = -AlphaBetaPruning(-beta, -alpha, depth - 1);

                provider.CancelLastPlayerMove(player, opponentPlayer);
                --ply;

                if (value > alpha)
                {
                    if (value >= beta)
                    {
                        return beta;
                    }

                    alpha = value;
                }
            }

            return alpha;
        }

        protected List<ChessMove> GetPlayerMoves(ChessPlayerBase player, ChessPlayerBase opponent)
        {
            // in future, use sorted dictionary
            var moves = new List<ChessMove>(40);
            ChessMove move = new ChessMove();

            var pMoves = new List<ChessMove>();
            foreach (var pawn in player.FiguresManager.Pawns)
            {
                var pawnMoves = provider.GetFilteredCells(pawn.Coordinates);
                move.Start = pawn.Coordinates;

                for (int i = 0; i < pawnMoves.Count; ++i)
                {
                    move.End = pawnMoves[i];
                    // pawn reached other board side
                    if ((move.End.Y == 0) ||
                        (move.End.Y == 7))
                    {
                        pMoves.Add(
                            new ChessPromotionMove(
                                new ChessMove(move),
                                FigureType.Queen));

                        pMoves.Add(
                            new ChessPromotionMove(
                                new ChessMove(move),
                                FigureType.Rook));

                        pMoves.Add(
                            new ChessPromotionMove(
                                new ChessMove(move),
                                FigureType.Bishop));

                        pMoves.Add(
                            new ChessPromotionMove(
                                new ChessMove(move),
                                FigureType.Horse));
                    }
                    else
                        pMoves.Add(new ChessMove(move));
                }
            }

            moves.AddRange(pMoves);

            foreach (var horse in player.FiguresManager.Horses)
            {
                move.Start = horse.Coordinates;
                var horseMoves = provider.GetFilteredCells(horse.Coordinates);

                for (int i = 0; i < horseMoves.Count; ++i)
                {
                    moves.Add(new ChessMove(move));
                }
            }

            foreach (var bishop in player.FiguresManager.Bishops)
            {
                move.Start = bishop.Coordinates;
                var bishopMoves = provider.GetFilteredCells(bishop.Coordinates);

                for (int i = 0; i < bishopMoves.Count; ++i)
                {
                    moves.Add(new ChessMove(move));
                }
            }

            foreach (var rook in player.FiguresManager.Rooks)
            {
                move.Start = rook.Coordinates;
                var rookMoves = provider.GetFilteredCells(rook.Coordinates);

                for (int i = 0; i < rookMoves.Count; ++i)
                {
                    moves.Add(new ChessMove(move));
                }
            }

            foreach (var queen in player.FiguresManager.Queens)
            {
                move.Start = queen.Coordinates;
                var queenMoves = provider.GetFilteredCells(queen.Coordinates);

                for (int i = 0; i < queenMoves.Count; ++i)
                {
                    moves.Add(new ChessMove(move));
                }
            }

            move.Start = player.FiguresManager.Kings.King.Coordinates;
            var kingMoves = provider.GetFilteredCells(move.Start);

            for (int i = 0; i < kingMoves.Count; ++i)
            {
                moves.Add(new ChessMove(move));
            }

            return moves;
        }

        protected int EvaluatePosition(ChessPlayerBase player, ChessPlayerBase opponentPlayer)
        {
            int result = 0;

            // add actual figures
            result += player.FiguresManager.PawnCount * PositionEvaluator.PawnValue;
            result += player.FiguresManager.BishopCount * PositionEvaluator.BishopValue;
            result += player.FiguresManager.HorseCount * PositionEvaluator.HorseValue;
            result += player.FiguresManager.RookCount * PositionEvaluator.RookValue;
            result += player.FiguresManager.QueenCount * PositionEvaluator.QueenValue;

            foreach (var figure in player.FiguresManager)
            {
                var coords = figure.Coordinates;
                result += PositionEvaluator.PositionValue[coords.Y, coords.X];
            }

            if (player.FiguresManager.Bishops.Count == 2)
                result += PositionEvaluator.PawnValue / 3;

            // TODO write lots of other conditions

            return result;
        }
    }
}
