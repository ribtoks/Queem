using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Queem.Core;
using System.Windows;

namespace ChessBoardVisualLib.Converters
{
    public class FigureToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Figure figure = (Figure)value;

            if (figure == Figure.Pawn)
                return new GridLength(4.0, GridUnitType.Star);
            else
                return new GridLength(8.0, GridUnitType.Star);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
