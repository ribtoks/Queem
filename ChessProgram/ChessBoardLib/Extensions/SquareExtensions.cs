using System;
using Queem.Core.BitBoards.Helpers;
using Queem.Core;

namespace ChessBoardVisualLib.Extensions
{
    public static class SquareExtension
    {
        public static int GetRealIndex(this Square sq)
        {
            int file = (int)BitBoardHelper.GetFileFromSquare(sq);
            int rank = BitBoardHelper.GetRankFromSquare(sq);

            return (7 - rank) * 8 + file;
        }
    }
}
