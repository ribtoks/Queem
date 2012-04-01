using System;
using Queem.Core;
using Queem.Core.BitBoards.Helpers;

namespace ChessBoardVisualLib.Extensions
{
    public static class MoveExtension
    {
        public static int GetDeltaX(this Move move)
        {
            int fileStart = (int)BitBoardHelper.GetFileFromSquare(move.From);
            int fileEnd = (int)BitBoardHelper.GetFileFromSquare(move.To);

            return fileEnd - fileStart;
        }

        public static int GetDeltaY(this Move move)
        {
            int rankStart = BitBoardHelper.GetRankFromSquare(move.From);
            int rankEnd = BitBoardHelper.GetRankFromSquare(move.To);

            return rankEnd - rankStart;
        }
    }
}
