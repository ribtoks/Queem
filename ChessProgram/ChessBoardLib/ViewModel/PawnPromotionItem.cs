using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Queem.Core;
using ChessBoardVisualLib.Enums;

namespace ChessBoardVisualLib.ViewModel
{
    public class PawnPromotionItem : DependencyObject
    {
        public PawnPromotionItem(Figure figure, Color color)
        {
            this.UpdateChessFigure(figure, color);
        }

        public static readonly DependencyProperty FigureTypeProperty =
                   DependencyProperty.Register("FigureType", typeof(Figure), typeof(PawnPromotionItem));

        public Figure FigureType
        {
            get { return (Figure)GetValue(FigureTypeProperty); }
            set { SetValue(FigureTypeProperty, value); }
        }

        public static readonly DependencyProperty FigureColorProperty =
                    DependencyProperty.Register("FigureColor", typeof(Color), typeof(PawnPromotionItem));

        public Color FigureColor
        {
            get { return (Color)GetValue(FigureColorProperty); }
            set { SetValue(FigureColorProperty, value); }
        }

        public void UpdateChessFigure(Figure newFigure, Color newColor)
        {
            this.FigureType = newFigure;
            this.FigureColor = newColor;
            this.ColoredFigure = ColoredFigureHelper.Create(this.FigureColor, this.FigureType);
        }

        public static readonly DependencyProperty ColoredFigureProperty =
            DependencyProperty.Register("ColoredFigure", typeof(ColoredFigure), typeof(PawnPromotionItem));

        public ColoredFigure ColoredFigure
        {
            get { return (ColoredFigure)GetValue(ColoredFigureProperty); }
            set { SetValue(ColoredFigureProperty, value); }
        }
    }
}
