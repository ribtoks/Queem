using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Queem.Core;
using Queem.Core.ChessBoard;
using Queem.Core.Extensions;

namespace BenchmarksGenerator
{
    public static class BenchmarkGenerator
    {
        public static List<Move> GenerateSituation(int depth)
        {
            int curr_depth = 0;
            Color myColor = Color.White;
            PlayerPosition myPosition = PlayerPosition.Down;

            GameProvider provider = new GameProvider(new MovesArrayAllocator());

            Color color = myColor;
            Random rand = new Random(DateTime.Now.Millisecond);

            while (curr_depth < depth)
            {
                var player = provider.PlayerBoards[(int)color];
                var opponent = provider.PlayerBoards[1 - (int)color];

                var lastMove = new Move(Square.A1, Square.A1);
                if (provider.History.HasItems())
                    lastMove = provider.History.GetLastMove();

                var moves = player.GetMoves(
                    opponent, 
                    lastMove,
                    MovesMask.AllMoves);
                provider.FilterMoves(moves, color);

                if (moves.Size == 0)
                {
                    // some checkmate found
                    break;
                }
                int index = rand.Next(moves.Size);
                // just get random move
                var move = new Move(moves.InnerArray[index]);

                provider.ProcessMove(move, color);

                bool needsPromotion = (int)move.Type >= (int)MoveType.Promotion;
                if (needsPromotion)
                    provider.PromotePawn(
                        color,
                        move.To,
                        move.Type.GetPromotionFigure());

                color = (Queem.Core.Color)(1 - (int)color);
                curr_depth += 1;
				
                provider.Allocator.ReleaseLast();
            }

            return provider.History.Moves;
        }
    }
}
