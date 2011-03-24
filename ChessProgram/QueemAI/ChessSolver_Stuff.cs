using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace QueemAI
{
    public enum CurrentPlayer { Me = 0, CPU = 1 }

    public static class CurrentPlayerExtensions
    {
        public static CurrentPlayer GetOppositePlayer(this CurrentPlayer player)
        {
            if (player == CurrentPlayer.Me)
                return CurrentPlayer.CPU;
            return CurrentPlayer.Me;
        }
    }

    public partial class ChessSolver
    {
        public int NodesSearched
        {
            get { return nodesSearched; }
        }

        public int HistoryDepth
        {
            get { return provider.History.Count; }
        }



        protected void InitializeBestMoves()
        {
            int count = 0;
            for (int i = 0; i < 2; ++i)
            {
                bestMoves[i] = new List<EvaluatedMove>(
                    Array.CreateInstance(typeof(EvaluatedMove), count)
                    .Cast<EvaluatedMove>()
                    );
            }
        }

        protected void InitializePlayerDepth()
        {
            playerDepth[0] = 0;
            playerDepth[1] = 0;
        }

        protected void IncPlayerDepth(CurrentPlayer player)
        {
            playerDepth[(int)player] += 1;
        }

        protected void DecPlayerDepth(CurrentPlayer player)
        {
            playerDepth[(int)player] -= 1;
        }

        protected delegate void QuickSortDelegate(List<EvaluatedMove> items, int first, int last);

        protected void SortEvaluatedMoves(List<EvaluatedMove> moves)
        {
            QuickSortDelegate quicksort = null;
            quicksort = 
                (items, first, last) =>
                {
                    int left = first;
                    int right = last;
                    int mid = items[(left + right) >> 1].Value;

                    while (left <= right)
                    {
                        while (items[left].Value > mid)
                            ++left;

                        while (items[right].Value < mid)
                            --right;

                        if (left <= right)
                        {
                            var tempItem = items[left];
                            items[left] = items[right];
                            items[right] = tempItem;

                            ++left;
                            --right;
                        }
                    }

                    if (first < right)
                        quicksort(items, first, right);

                    if (left < last)
                        quicksort(items, left, last);
                };

            quicksort(moves, 0, moves.Count - 1);
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

            //foreach (var figure in player.FiguresManager)
            //{
            //    var coords = figure.Coordinates;
            //    result += PositionEvaluator.PositionValue[coords.Y, coords.X];
            //}
            result += player.FiguresManager.GetPositionValue(
                PositionEvaluator.PositionValue);

            if (player.FiguresManager.Bishops.Count == 2)
                result += PositionEvaluator.PawnValue / 3;

            // TODO write lots of other conditions

            return result;
        }
    }
}