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
    }
}