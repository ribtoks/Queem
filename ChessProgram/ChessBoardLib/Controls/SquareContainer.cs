using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace ChessBoardVisualLib.Controls
{
    public class SquareContainer : ContentControl
    {
        private Size CalculateSize(Size availableSize)
        {
            double maxDimension = Math.Min(availableSize.Width, availableSize.Height);
            return new Size(maxDimension, maxDimension);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var size = CalculateSize(constraint);

            if ((Content != null) && (Content is FrameworkElement))
            {
                ((FrameworkElement)Content).Measure(size);
            }

            return size;
        }
        
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            base.ArrangeOverride(arrangeBounds);

            var size = CalculateSize(arrangeBounds);

            if ((Content != null) && (Content is FrameworkElement))
            {
                ((FrameworkElement)Content).Arrange(new Rect(new Point(0, 0), size));
            }

            return arrangeBounds;
        }         
    }
}
