using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BasicChessClasses;
using System.Windows.Media.Animation;

namespace ChessBoardTest
{
    /// <summary>
    /// Interaction logic for Chessboard.xaml
    /// </summary>
    public partial class ChessBoardControl : UserControl
    {
        protected void ShowPossibleCells(List<Coordinates> coordinates)
        {
            foreach (var coord in coordinates)
            {
                int index = coord.Y * 8 + coord.X;

                Border border = chessBoardGrid.Children[index] as Border;
                Grid figureGrid = ((Grid)border.Child).Children[0] as Grid;

                figureGrid.Background = new SolidColorBrush(
                    Color.FromArgb(0x44, 0x7F, 0x19, 0x63));
            }
        }

        protected void HidePossibleCells(List<Coordinates> coordinates)
        {
            foreach (var coord in coordinates)
            {
                int index = coord.Y * 8 + coord.X;

                Border border = chessBoardGrid.Children[index] as Border;
                Grid figureGrid = ((Grid)border.Child).Children[0] as Grid;

                figureGrid.Background = new SolidColorBrush();
            }
        }

        protected bool StartMyFigureMoving(int i, int j)
        {
            GeneralFigure gf = mp.ChessBoard[j, i];
            if (gf.Type != FigureType.Nobody)
            {
                // i can move only mine figures
                if (gf.Color == myColor)
                {
                    isMoving = true;
                    lastPossibleMoves = mp.GetFilteredCells(startCoord);

                    ShowPossibleCells(lastPossibleMoves);
                    return true;
                }
            }
            return false;
        }

        protected void FinishMyFigureMooving(int i, int j)
        {
            // if it's allowed move
            if (lastPossibleMoves.IndexOf(endCoord) != -1)
            {
                isMoving = false;
                HidePossibleCells(lastPossibleMoves);
                AnimateFigureMove(startCoord, endCoord);

                // provide move on board
                mp.ProvideMyMove(new ChessMove(startCoord, endCoord));
            }
            else
            {
                GeneralFigure gf = mp.ChessBoard[j, i];
                if (gf.Type != FigureType.Nobody)
                {
                    if (!StartMyFigureMoving(i, j))
                    {
                        isMoving = false;
                        HidePossibleCells(lastPossibleMoves);
                    }
                }
                else
                {
                    isMoving = false;
                    HidePossibleCells(lastPossibleMoves);
                }
                // if user clicked on his other figure
                // initialize a new move
                startCoord.Set(j, i);
                StartMyFigureMoving(i, j);
            }

            lastPossibleMoves.Clear();
        }

        protected void AnimateFigureMove(Coordinates start, Coordinates end)
        {
            int startIndex = start.Y * 8 + start.X;
            Border startBorder = chessBoardGrid.Children[startIndex] as Border;
            Grid startGrid = startBorder.Child as Grid;
            Border startTarget = (startGrid.Children[0] as Grid).Children[0] as Border;

            int finishIndex = end.Y * 8 + end.X;
            Border finishBorder = chessBoardGrid.Children[finishIndex] as Border;
            Grid finishGrid = finishBorder.Child as Grid;

            double deltaX = (end.X - start.X) * startGrid.ActualWidth;
            double deltaY = (end.Y - start.Y) * startGrid.ActualHeight;

            startTarget.RenderTransform = new TranslateTransform();

            DoubleAnimation shiftX = new DoubleAnimation();
            shiftX.From = 0;
            shiftX.To = deltaX;
            shiftX.Duration = new Duration(TimeSpan.FromSeconds(0.6));
            shiftX.AccelerationRatio = 0.4;
            shiftX.DecelerationRatio = 0.6;

            DoubleAnimation shiftY = new DoubleAnimation();
            shiftY.From = 0;
            shiftY.To = deltaY;
            shiftY.Duration = shiftX.Duration;
            shiftY.AccelerationRatio = shiftX.AccelerationRatio;
            shiftY.DecelerationRatio = shiftX.DecelerationRatio;

            Storyboard story = new Storyboard();
            story.Children.Add(shiftX);
            story.Children.Add(shiftY);

            Storyboard.SetTarget(shiftX, startTarget);
            Storyboard.SetTarget(shiftY, startTarget);

            Storyboard.SetTargetProperty(shiftX, new PropertyPath("RenderTransform.X"));
            Storyboard.SetTargetProperty(shiftY, new PropertyPath("RenderTransform.Y"));

            story.Begin();
            story.Completed += new EventHandler(story_Completed);
        }

        protected void story_Completed(object sender, EventArgs e)
        {
            ReplaceAnimationFigures();
        }

        protected void ReplaceAnimationFigures()
        {
            int startIndex = startCoord.Y * 8 + startCoord.X;
            Border startBorder = chessBoardGrid.Children[startIndex] as Border;
            Grid startGrid = startBorder.Child as Grid;
            Grid startFigureGrid = startGrid.Children[0] as Grid;
            Border figureBorder = startFigureGrid.Children[0] as Border;
            startGrid.Children.Clear();

            int finishIndex = endCoord.Y * 8 + endCoord.X;
            Border finishBorder = chessBoardGrid.Children[finishIndex] as Border;
            Grid finishGrid = finishBorder.Child as Grid;

            finishGrid.Children.Clear();
            figureBorder.RenderTransform = new TranslateTransform();
            finishGrid.Children.Add(startFigureGrid);
        }

        protected void highlightBorder(object sender)
        {
            Border border = (Border)sender;
            Grid grid = (Grid)border.Child;

            // TODO move such colors to resources
            grid.Background = new SolidColorBrush(Color.FromArgb(0x19, 0xff, 0xff, 0xff));
        }

        protected void unhightlightBorder(object sender)
        {
            Border border = (Border)sender;
            Grid grid = (Grid)border.Child;

            grid.Background = new SolidColorBrush();
        }
    }
}