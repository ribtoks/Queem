using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Queem.Core;
using Queem.Core.BitBoards.Helpers;

namespace ChessBoardVisualLib.Converters
{
    public class MoveToDeltaXConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Square start = (Square)values[0];
            Square end = (Square)values[1];
            double actualWidth = (double)values[2];

            int fileStart = (int)BitBoardHelper.GetFileFromSquare(start);
            int fileEnd = (int)BitBoardHelper.GetFileFromSquare(end);

            return (fileEnd - fileStart) * actualWidth;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
