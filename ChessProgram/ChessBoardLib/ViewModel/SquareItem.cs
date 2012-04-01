using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Queem.Core;
using Queem.Core.BitBoards.Helpers;
using ChessBoardVisualLib.Enums;
using System.Windows.Media.Animation;

namespace ChessBoardVisualLib.ViewModel
{
    public class SquareItem : DependencyObject
    {
        private Square innerSquare;

        public SquareItem(Square square, Figure figure, Color color)
        {
            this.innerSquare = square;
            this.SetBackgroundColor(square);
            this.FigureType = figure;
            this.FigureColor = color;
            this.ColoredFigure = ColoredFigureHelper.Create(color, figure);
        }

        public Square Square
        {
            get { return this.innerSquare; }           
        }

        public void SetBackgroundColor(Square square)
        {
            int file = (int)BitBoardHelper.GetFileFromSquare(square);
            int rank = BitBoardHelper.GetRankFromSquare(square);

            if (0 == ((7 - file) + rank) % 2)
                this.SquareColor = Color.White;
            else
                this.SquareColor = Color.Black;
        }

        public static readonly DependencyProperty SquareColorProperty =
            DependencyProperty.Register("SquareColor", typeof(Color), typeof(SquareItem));

        public Color SquareColor
        {
            get { return (Color)GetValue(SquareColorProperty); }
            set { SetValue(SquareColorProperty, value); }
        }

        public static readonly DependencyProperty FigureTypeProperty =
            DependencyProperty.Register("FigureType", typeof(Figure), typeof(SquareItem));

        public Figure FigureType
        {
            get { return (Figure)GetValue(FigureTypeProperty); }
            set { SetValue(FigureTypeProperty, value); }
        }

        public void UpdateChessFigure(Figure newFigure, Color newColor)
        {
            this.FigureType = newFigure;
            this.FigureColor = newColor;
            this.ColoredFigure = ColoredFigureHelper.Create(this.FigureColor, this.FigureType);
        }

        public static readonly DependencyProperty ColoredFigureProperty =
            DependencyProperty.Register("ColoredFigure", typeof(ColoredFigure), typeof(SquareItem));

        public ColoredFigure ColoredFigure
        {
            get { return (ColoredFigure)GetValue(ColoredFigureProperty); }
            set { SetValue(ColoredFigureProperty, value); }
        }

        public static readonly DependencyProperty IsHighlightedProperty =
            DependencyProperty.Register("IsHighlighted", typeof(bool), typeof(SquareItem));

        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        public static readonly DependencyProperty FigureColorProperty =
            DependencyProperty.Register("FigureColor", typeof(Color), typeof(SquareItem));

        public Color FigureColor
        {
            get { return (Color)GetValue(FigureColorProperty); }
            set { SetValue(FigureColorProperty, value); }
        }

        public static readonly DependencyProperty DeltaXTransformProperty =
            DependencyProperty.Register("DeltaXTransform", typeof(double), typeof(SquareItem));

        public double DeltaXTransform
        {
            get { return (double)GetValue(DeltaXTransformProperty); }
            set { SetValue(DeltaXTransformProperty, value); }
        }

        public static readonly DependencyProperty DeltaYTransformProperty =
                    DependencyProperty.Register("DeltaYTransform", typeof(double), typeof(SquareItem));

        public double DeltaYTransform
        {
            get { return (double)GetValue(DeltaYTransformProperty); }
            set { SetValue(DeltaYTransformProperty, value); }
        }

        public event EventHandler MoveAnimationFinished;

        private void OnAnimationFinished()
        {
            if (this.MoveAnimationFinished != null)
                this.MoveAnimationFinished(this, EventArgs.Empty);
        }

        public void AnimateShift(double deltaX, double deltaY, Action<SquareItem> action)
        {
            DoubleAnimation shiftX = new DoubleAnimation();
            shiftX.From = 0;
            shiftX.To = deltaX;
            shiftX.Duration = new Duration(TimeSpan.FromSeconds(0.6));

            DoubleAnimation shiftY = new DoubleAnimation();
            shiftY.From = 0;
            shiftY.To = deltaY;
            shiftY.Duration = shiftX.Duration;

            ParallelTimeline timeline = new ParallelTimeline();
            timeline.Children.Add(shiftX);
            timeline.Children.Add(shiftY);

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(timeline);

            Storyboard.SetTarget(shiftX, this);
            Storyboard.SetTarget(shiftY, this);

            Storyboard.SetTargetProperty(shiftX, new PropertyPath("DeltaXTransform"));
            Storyboard.SetTargetProperty(shiftY, new PropertyPath("DeltaYTransform"));

            storyboard.Completed += new EventHandler((sender, e) => action(this));

            storyboard.Begin();
        }

        public void ResetTransform()
        {
            this.DeltaXTransform = 0;
            this.DeltaYTransform = 0;
        }
    }
}
