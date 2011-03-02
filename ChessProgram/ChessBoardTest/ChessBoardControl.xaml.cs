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
        protected MovesProvider mp;
        protected bool isMoving = false;
        protected FigureColor myColor = FigureColor.White;
        protected FigureStartPosition myStartPos = FigureStartPosition.Down;
        protected Coordinates lastClickedCoords;
        protected List<Coordinates> lastPossibleMoves;

        public ChessBoardControl()
        {
            InitializeComponent();
            mp = new MovesProvider(myColor, myStartPos);
            InitializeBoard();
            BindBoardHandlers();
            lastClickedCoords = new Coordinates();
        }

        protected Grid CreateFigureGrid(FigureType type)
        {
            Grid grid = new Grid();

            // -------------------------------------

            grid.RowDefinitions.Add(
                new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Star) });
            grid.RowDefinitions.Add(
                new RowDefinition() { Height = new GridLength(8.0, GridUnitType.Star) });
            grid.RowDefinitions.Add(
                new RowDefinition() { Height = new GridLength(1.0, GridUnitType.Star) });

            // -------------------------------------

            grid.ColumnDefinitions.Add(
                new ColumnDefinition() { Width = new GridLength(1.0, GridUnitType.Star) });
            
            // need different scaling for pawns
            double columnLength = 8.0;
            if (type == FigureType.Pawn)
                columnLength = 4.0;

            grid.ColumnDefinitions.Add(
                new ColumnDefinition() { Width = new GridLength(columnLength, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(
                new ColumnDefinition() { Width = new GridLength(1.0, GridUnitType.Star) });

            return grid;
        }

        protected void InitializeBoard()
        {
            ResourceDictionary rd = (ResourceDictionary)this.FindResource("Dictionaries");
            ResourceDictionary myStyles = rd.MergedDictionaries[0];

            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    /*
                     * Border
                     * -> OuterGrid 
                     *    -> Grid ()
                     *       -> Border (VisualBrush)
                    */

                    GeneralFigure gf = mp.ChessBoard[j, i];

                    // create border with needed margins
                    Grid grid = CreateFigureGrid(gf.Type);
                    string color;
                    if (0 == (i + j) % 2)
                        color = "White";
                    else
                        color = "Black";

                    if (gf.Type != FigureType.Nobody)
                    {
                        Border border = new Border();
                        string brushName = string.Format("{0}{1}", gf.Color, gf.Type);
                        object vb = myStyles[brushName];

                        border.Background = (VisualBrush)vb;

                        grid.Children.Add(border);
                        Grid.SetColumn(border, 1);
                        Grid.SetRow(border, 1);                        
                    }
                    int index = i * 8 + j;
                    Border existingBorder = new Border();// (Border)chessBoardGrid.Children[index];

                    // create outer grid for 
                    // additional highlighting
                    Grid outerGrid = new Grid();
                    outerGrid.Children.Add(grid);

                    existingBorder.Background = (SolidColorBrush)this.FindResource(string.Format("{0}CellBrush", color));

                    existingBorder.Child = outerGrid;

                    chessBoardGrid.Children.Add(existingBorder);
                }
            }
        }

        protected void BindBoardHandlers()
        {
            foreach (Border border in chessBoardGrid.Children.OfType<Border>())
            {
                border.MouseEnter += new MouseEventHandler(border_MouseEnter);
                border.MouseLeave += new MouseEventHandler(border_MouseLeave);

                border.MouseDown += new MouseButtonEventHandler(border_MouseDown);
                border.MouseUp += new MouseButtonEventHandler(border_MouseUp);
            }
        }

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

        #region Border mouse handlers

        protected void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int index = chessBoardGrid.Children.IndexOf(sender as UIElement);

            int i = index / 8;
            int j = index % 8;

            // we selected some figure
            if (!isMoving)
            {
                // save last coordinages
                lastClickedCoords.X = j;
                lastClickedCoords.Y = i;

                // just initialize a new move
                StartMyFigureMoving(i, j);               
            }
            else
            {
                unhightlightBorder(sender);
                FinishMyFigureMooving(i, j);
            }
        }

        protected void border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int index = chessBoardGrid.Children.IndexOf(sender as UIElement);

            int i = index / 8;
            int j = index % 8;

            Coordinates mouseUpCoords = new Coordinates(j, i);

            if (isMoving)
            {
                if (mouseUpCoords != lastClickedCoords)
                {
                    unhightlightBorder(sender);

                    FinishMyFigureMooving(i, j);
                }
            }
        }

        protected void border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!isMoving)
                return;

            unhightlightBorder(sender);
        }

        protected void border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!isMoving)
                return;

            highlightBorder(sender);    
        }

        #endregion

        protected bool StartMyFigureMoving(int i, int j)
        {
            GeneralFigure gf = mp.ChessBoard[j, i];
            if (gf.Type != FigureType.Nobody)
            {
                // i can move only mine figures
                if (gf.Color == myColor)
                {
                    isMoving = true;
                    lastPossibleMoves = mp.GetFilteredCells(lastClickedCoords);

                    ShowPossibleCells(lastPossibleMoves);
                    return true;
                }
            }
            return false;
        }

        protected void FinishMyFigureMooving(int i, int j)
        {
            // if it's allowed move
            if (lastPossibleMoves.IndexOf(lastClickedCoords) != -1)
            {
                HidePossibleCells(lastPossibleMoves);
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
                    AnimateFigureMove(lastClickedCoords, new Coordinates(j, i));
                }
                // if user clicked on his other figure
                StartMyFigureMoving(i, j);
            }

            lastPossibleMoves.Clear();
        }

        #region Figure move animation

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

            TranslateTransform tt = new TranslateTransform();
            startTarget.RenderTransform = tt;

            DoubleAnimation shiftX = new DoubleAnimation();
            shiftX.From = 0;
            shiftX.To = deltaX;
            shiftX.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            
            DoubleAnimation shiftY = new DoubleAnimation();
            shiftY.From = 0;
            shiftY.To = deltaY;
            shiftY.Duration = shiftX.Duration;

            Storyboard story = new Storyboard();
            story.Children.Add(shiftX);
            story.Children.Add(shiftY);

            Storyboard.SetTarget(shiftX, startTarget);
            Storyboard.SetTarget(shiftY, startTarget);

            Storyboard.SetTargetProperty(shiftX, new PropertyPath("RenderTransform.X"));
            Storyboard.SetTargetProperty(shiftY, new PropertyPath("RenderTransform.Y"));

            story.Begin();
        }

        #endregion

        protected void highlightBorder(object sender)
        {
            Border border = (Border)sender;
            Grid grid = (Grid)border.Child;

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
