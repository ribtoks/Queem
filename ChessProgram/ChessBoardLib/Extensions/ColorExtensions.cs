using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Queem.Core;

namespace ChessBoardVisualLib.Extensions
{
    public static class ColorExtensions
    {
        public static Color GetOpposite(this Color color)
        {
            if (Color.White == color)
                return Color.Black;
            else
                return Color.White;
        }
    }
}
