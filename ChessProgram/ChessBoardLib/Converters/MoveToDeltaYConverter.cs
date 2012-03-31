using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Queem.Core;
using Queem.Core.BitBoards.Helpers;

namespace ChessBoardVisualLib.Converters
{
    public class MoveToDeltaYConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Square start = (Square)values[0];
            Square end = (Square)values[1];
            double actualHeight = (double)values[2];

            int rankStart = (int)BitBoardHelper.GetRankFromSquare(start);
            int rankEnd = (int)BitBoardHelper.GetRankFromSquare(end);

            return (rankEnd - rankStart) * actualHeight;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
