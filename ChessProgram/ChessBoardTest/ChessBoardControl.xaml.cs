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
    public enum MoveTurn { My, Other }

    /// <summary>
    /// Interaction logic for Chessboard.xaml
    /// </summary>
    public partial class ChessBoardControl : UserControl
    {
        #region Data

        // basic class of move provider
        protected MovesProvider mp;
        protected bool isMoving = false;

        protected Coordinates startCoord;
        protected Coordinates endCoord;
        
        protected List<Coordinates> lastPossibleMoves;

        // color of figures of player,
        // which turn to move is now
        protected FigureColor currPlayerColor;

        private int lastZIndex;

        #endregion

        #region Events

        public event PlayerMoveEventHandler PlayerMoveStartPreview;

        protected void OnPlayerMoveStartPreview()
        {
            if (PlayerMoveStartPreview != null)
            {
                PlayerMoveStartPreview(this, new PlayerMoveEventArgs() { MoveStart = new Coordinates(startCoord), PlayerColor = currPlayerColor });
            }
        }

        public event PlayerMoveEventHandler PlayerMoveFinished;

        protected void OnPlayerMoveFinish()
        {
            if (PlayerMoveFinished != null)
            {
                PlayerMoveFinished(this, new PlayerMoveEventArgs() { 
                    MoveStart = new Coordinates(startCoord),
                    MoveEnd = new Coordinates(endCoord),
                    PlayerColor = currPlayerColor });
            }
        }

        public event EventHandler PlayerMoveAnimatinFinished;

        protected void OnPlayerMoveAnimatinFinish()
        {
            if (PlayerMoveAnimatinFinished != null)
            {
                PlayerMoveAnimatinFinished(this, EventArgs.Empty);
            }
        }

        #endregion

        public ChessBoardControl()
        {
            InitializeComponent();
        }

        public void InitializeControl(MovesProvider mp_base)
        {
            mp = mp_base;
            currPlayerColor = mp_base.Player1.FiguresColor;
            
            InitializeBoard();
            BindBoardHandlers();

            startCoord = new Coordinates();
            endCoord = new Coordinates();
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
                     * Border (White/Black cell background)
                     * |_ OuterGrid (OnMouseOver when is moving)
                     *    |_ Grid (PossibleCells)
                     *    -------------- optional --------------
                     *       |_ Border (Figure VisualBrush, OnAnimate)
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

        #region Border mouse handlers

        protected void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int index = chessBoardGrid.Children.IndexOf(sender as UIElement);

            int i = index / 8;
            int j = index % 8;

            // we selected some figure
            if (!isMoving)
            {
                if (mp.ChessBoard[j, i].Type == FigureType.Nobody)
                    return;

                // save last coordinages
                startCoord.Set(j, i);

                // just initialize a new move
                StartFigureMoving(i, j);               
            }
            else
            {
                endCoord.Set(j, i);
                unhightlightBorder(sender);
                FinishFigureMoving(i, j);
            }
        }

        protected void border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int index = chessBoardGrid.Children.IndexOf(sender as UIElement);

            int i = index / 8;
            int j = index % 8;

            endCoord.Set(j, i);

            if (isMoving)
            {
                if (startCoord != endCoord)
                {
                    unhightlightBorder(sender);

                    FinishFigureMoving(i, j);
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

        public void ChangePlayer()
        {
            currPlayerColor = currPlayerColor.GetOppositeColor();
        }

        public void RedrawAll()
        {
            chessBoardGrid.Children.Clear();
            InitializeBoard();
            BindBoardHandlers();            
        }
    }
}
