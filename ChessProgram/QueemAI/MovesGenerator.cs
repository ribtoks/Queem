using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace QueemAI
{
    public static class MovesGenerator
    {
        internal static List<ChessMove> GetPlayerMoves(MovesProvider provider,
            ChessPlayerBase player, ChessPlayerBase opponent)
        {
            // in future, use sorted dictionary
            var moves = new List<ChessMove>(30);
            ChessMove move = new ChessMove();

            // take king moves first (when king is in check)
            move.Start = player.FiguresManager.Kings.King.Coordinates;
            var kingMoves = provider.GetFilteredCells(move.Start, player, opponent);

            for (int i = 0; i < kingMoves.Count; ++i)
            {
                move.End = kingMoves[i];
                moves.Add(new ChessMove(move));
            }

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
                        moves.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Queen));

                        moves.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Rook));

                        moves.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Bishop));

                        moves.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Horse));
                    }
                    else
                        moves.Add(new ChessMove(move));
                }
            }
            
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

            return moves;
        }

        internal static List<ChessMove> GetAttackingPlayerMoves(MovesProvider provider,
            ChessPlayerBase player, ChessPlayerBase opponent)
        {
            // in future, use sorted dictionary
            var moves = new List<ChessMove>(30);
            ChessMove move = new ChessMove();

            foreach (var queen in player.FiguresManager.Queens)
            {
                move.Start = queen.Coordinates;
                var queenMoves = provider.GetFilteredAttackingCells(queen.Coordinates, player, opponent);

                for (int i = 0; i < queenMoves.Count; ++i)
                {
                    move.End = queenMoves[i];
                    moves.Add(new ChessMove(move));
                }
            }

            foreach (var pawn in player.FiguresManager.Pawns)
            {
                var pawnMoves = provider.GetFilteredAttackingCells(pawn.Coordinates, player, opponent);
                move.Start = pawn.Coordinates;

                for (int i = 0; i < pawnMoves.Count; ++i)
                {
                    move.End = pawnMoves[i];
                    // pawn reached other board side
                    if ((move.End.Y == 0) ||
                        (move.End.Y == 7))
                    {
                        moves.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Queen));

                        moves.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Rook));

                        moves.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Bishop));

                        moves.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Horse));
                    }
                    else
                        moves.Add(new ChessMove(move));
                }
            }

            foreach (var horse in player.FiguresManager.Horses)
            {
                move.Start = horse.Coordinates;
                var horseMoves = provider.GetFilteredAttackingCells(horse.Coordinates, player, opponent);

                for (int i = 0; i < horseMoves.Count; ++i)
                {
                    move.End = horseMoves[i];
                    moves.Add(new ChessMove(move));
                }
            }

            foreach (var bishop in player.FiguresManager.Bishops)
            {
                move.Start = bishop.Coordinates;
                var bishopMoves = provider.GetFilteredAttackingCells(bishop.Coordinates, player, opponent);

                for (int i = 0; i < bishopMoves.Count; ++i)
                {
                    move.End = bishopMoves[i];
                    moves.Add(new ChessMove(move));
                }
            }

            foreach (var rook in player.FiguresManager.Rooks)
            {
                move.Start = rook.Coordinates;
                var rookMoves = provider.GetFilteredAttackingCells(rook.Coordinates, player, opponent);

                for (int i = 0; i < rookMoves.Count; ++i)
                {
                    move.End = rookMoves[i];
                    moves.Add(new ChessMove(move));
                }
            }

            move.Start = player.FiguresManager.Kings.King.Coordinates;
            var kingMoves = provider.GetFilteredAttackingCells(move.Start, player, opponent);

            for (int i = 0; i < kingMoves.Count; ++i)
            {
                move.End = kingMoves[i];
                moves.Add(new ChessMove(move));
            }

            return moves;
        }

        public static List<ChessMove> GenerateAllMoves(MovesProvider provider,
            FigureColor playerColor)
        {
            ChessSolver cs = new ChessSolver(provider);
            ChessPlayerBase player = provider.Player1;
            ChessPlayerBase opponent = provider.Player2;

            if (player.FiguresColor != playerColor)
            {
                player = provider.Player2;
                opponent = provider.Player1;
            }

            return GetPlayerMoves(provider, player, opponent);
        }

        public static List<ChessMove> GetSortedMoves(MovesProvider provider,
            ChessPlayerBase player, ChessPlayerBase opponent)
        {
            var winningCaptures = new List<ChessMove>();
            var valuablePromotions = new List<ChessMove>();
            var equalCaptures = new List<ChessMove>();
            var castlingMoves = new List<ChessMove>();
            var minorPromotions = new List<ChessMove>();
            var restMoves = new List<ChessMove>();

            var board = provider.ChessBoard;

            var moves = new List<ChessMove>(30);
            ChessMove move = new ChessMove();

            move.Start = player.FiguresManager.Kings.King.Coordinates;
            var kingMoves = provider.GetFilteredCells(move.Start, player, opponent);

            for (int i = 0; i < kingMoves.Count; ++i)
            {
                move.End = kingMoves[i];
                restMoves.Add(new ChessMove(move));
            }

            foreach (var queen in player.FiguresManager.Queens)
            {
                move.Start = queen.Coordinates;
                var queenMoves = provider.GetFilteredCells(queen.Coordinates, player, opponent);

                for (int i = 0; i < queenMoves.Count; ++i)
                {
                    move.End = queenMoves[i];

                    if (board[move.End].Type == FigureType.Queen)
                        equalCaptures.Add(new ChessMove(move));
                    else
                        restMoves.Add(new ChessMove(move));
                }
            }

            foreach (var rook in player.FiguresManager.Rooks)
            {
                move.Start = rook.Coordinates;
                var rookMoves = provider.GetFilteredCells(rook.Coordinates, player, opponent);

                for (int i = 0; i < rookMoves.Count; ++i)
                {
                    move.End = rookMoves[i];

                    switch (board[move.End].Type)
                    {
                        case FigureType.Pawn:
                        case FigureType.Horse:
                        case FigureType.Bishop:
                        case FigureType.Nobody:
                            restMoves.Add(new ChessMove(move));
                            break;
                        case FigureType.Rook:
                            equalCaptures.Add(new ChessMove(move));
                            break;
                        case FigureType.Queen:
                            winningCaptures.Add(new ChessMove(move));
                            break;
                    }
                }
            }

            foreach (var horse in player.FiguresManager.Horses)
            {
                move.Start = horse.Coordinates;
                var horseMoves = provider.GetFilteredCells(horse.Coordinates, player, opponent);

                for (int i = 0; i < horseMoves.Count; ++i)
                {
                    move.End = horseMoves[i];

                    switch (board[move.End].Type)
                    {
                        case FigureType.Pawn:
                        case FigureType.Nobody:
                            restMoves.Add(new ChessMove(move));
                            break;
                        case FigureType.Horse:
                        case FigureType.Bishop:
                            equalCaptures.Add(new ChessMove(move));
                            break;
                        case FigureType.Rook:
                        case FigureType.Queen:
                            winningCaptures.Add(new ChessMove(move));
                            break;
                    }
                }
            }

            foreach (var bishop in player.FiguresManager.Bishops)
            {
                move.Start = bishop.Coordinates;
                var bishopMoves = provider.GetFilteredCells(bishop.Coordinates, player, opponent);

                for (int i = 0; i < bishopMoves.Count; ++i)
                {
                    move.End = bishopMoves[i];

                    switch (board[move.End].Type)
                    {
                        case FigureType.Pawn:
                        case FigureType.Nobody:
                            restMoves.Add(new ChessMove(move));
                            break;
                        case FigureType.Horse:
                        case FigureType.Bishop:
                            equalCaptures.Add(new ChessMove(move));
                            break;
                        case FigureType.Rook:
                        case FigureType.Queen:
                            winningCaptures.Add(new ChessMove(move));
                            break;
                    }
                }
            }

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
                        if ((board[move.End].Type != FigureType.Nobody))
                        {
                            valuablePromotions.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Queen));

                            valuablePromotions.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Rook));
                        }

                        minorPromotions.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Bishop));

                        minorPromotions.Add(
                            new PromotionMove(
                                new ChessMove(move),
                                PromotionType.Horse));
                    }
                    else
                    {
                        if (board[move.End].Type != FigureType.Nobody)
                        {
                            if (board[move.End].Type != FigureType.Pawn)
                                winningCaptures.Add(new ChessMove(move));
                            else
                                equalCaptures.Add(new ChessMove(move));
                        }
                        else
                            restMoves.Add(new ChessMove(move));
                    }
                }
            }

            moves.AddRange(winningCaptures);
            moves.AddRange(valuablePromotions);
            moves.AddRange(equalCaptures);
            moves.AddRange(castlingMoves);
            moves.AddRange(minorPromotions);
            moves.AddRange(restMoves);

            return moves;
        }
    }
}
