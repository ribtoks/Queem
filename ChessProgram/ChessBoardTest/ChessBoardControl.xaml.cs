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
using System.Windows.Media.Animation;
using Queem.CoreInterface.Interface;
using Queem.CoreInterface.Adapters;

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

        protected int animationEventsCount = 0;
        protected int animationsDone = 0;

        protected Coordinates pawnChangeCoords;
        protected FigureType[] pawnReplacementTypes = new FigureType[] { FigureType.Knight, FigureType.Bishop, FigureType.Rook, FigureType.Queen };

        #endregion

        public Queem.Core.Color CurrPlayerColor
        {
            get { return (currPlayerColor == FigureColor.White) ? Queem.Core.Color.White : Queem.Core.Color.Black; }
        }

        #region Events

        public event PlayerMoveEventHandler PlayerMoveStartPreview;

        protected void OnPlayerMoveStartPreview()
        {
            if (PlayerMoveStartPreview != null)
            {
                PlayerMoveStartPreview(this, new PlayerMoveEventArgs() { MoveStart = new Coordinates(startCoord), PlayerColor = currPlayerColor });
            }
        }

        public event EventHandler PlayerMoveAnimationPreview;

        protected void OnPlayerMoveAnimationPreview()
        {
            if (PlayerMoveAnimationPreview != null)
            {
                PlayerMoveAnimationPreview(this, EventArgs.Empty);
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

        public event EventHandler PlayerMoveAnimationFinished;

        protected void OnPlayerMoveAnimationFinish()
        {
            if (PlayerMoveAnimationFinished != null)
            {
                PlayerMoveAnimationFinished(this, EventArgs.Empty);
            }
        }

        public event PawnChangedEventHandler PawnChanged;

        protected void OnPawnChange(FigureType changeType)
        {
            if (PawnChanged != null)
            {
                PawnChanged(this, new PawnChangedEventArgs() 
                { FigureType = changeType, Square = pawnChangeCoords });
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

            startCoord = new CoordinatesAdapter(Queem.Core.Square.NoSquare);
            endCoord = new CoordinatesAdapter(Queem.Core.Square.NoSquare);
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

        protected void InitializeBoardWithPlayer()
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

            SetPlayerFigures(mp.Player1);
            SetPlayerFigures(mp.Player2);
        }

        protected void SetPlayerFigures(ChessPlayerBase player)
        {
            ResourceDictionary rd = (ResourceDictionary)this.FindResource("Dictionaries");
            ResourceDictionary myStyles = rd.MergedDictionaries[0];

            foreach (var figure in player.FiguresManager)
            {
                int i = figure.Coordinates.Y;
                int j = figure.Coordinates.X;
                // create border with needed margins
                Grid grid = CreateFigureGrid(figure.Type);
                string color;
                if (0 == (i + j) % 2)
                    color = "White";
                else
                    color = "Black";

                Border border = new Border();
                string brushName = string.Format("{0}{1}", figure.Color, figure.Type);
                object vb = myStyles[brushName];

                border.Background = (VisualBrush)vb;

                grid.Children.Add(border);
                Grid.SetColumn(border, 1);
                Grid.SetRow(border, 1);

                int index = i * 8 + j;
                Border existingBorder = (Border)chessBoardGrid.Children[index];

                // create outer grid for 
                // additional highlighting
                Grid outerGrid = new Grid();
                outerGrid.Children.Add(grid);

                existingBorder.Background = (SolidColorBrush)this.FindResource(string.Format("{0}CellBrush", color));

                existingBorder.Child = outerGrid;
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

        public void ReplacePawn(Coordinates coords, FigureColor pawnColor)
        {
            alternativesGrid.Children.Clear();

            Storyboard showOverlay = (Storyboard)this.FindResource("showOverlay");
            ResourceDictionary rd = (ResourceDictionary)this.FindResource("Dictionaries");
            ResourceDictionary myStyles = rd.MergedDictionaries[0];

            pawnChangeCoords = new Coordinates(coords);

            foreach (var type in pawnReplacementTypes)
            {
                Border outerBorder = new Border();
                outerBorder.Style = (Style)alternativesGrid.Resources["borderWrapperStyle"];

                outerBorder.MouseUp += new MouseButtonEventHandler(FigureBorder_Click);

                Grid grid = CreateFigureGrid(type);
                
                Border border = new Border();
                border.Background = (VisualBrush)myStyles[string.Format("{0}{1}", pawnColor, type)];

                grid.Children.Add(border);
                Grid.SetRow(border, 1);
                Grid.SetColumn(border, 1);

                outerBorder.Child = grid;

                alternativesGrid.Children.Add(outerBorder);
            }

            showOverlay.Begin();
        }

        /// <summary>
        /// Debug method
        /// </summary>
        public void RedrawAll()
        {
            chessBoardGrid.Children.Clear();
            //InitializeBoard();
            InitializeBoardWithPlayer();
            BindBoardHandlers();            
        }

        private void FigureBorder_Click(object sender, MouseButtonEventArgs e)
        {
            int borderIndex = alternativesGrid.Children.IndexOf(sender as UIElement);
            FigureType type = pawnReplacementTypes[borderIndex];

            Storyboard hideOverlay = (Storyboard)this.FindResource("hideOverlay");

            hideOverlay.Begin();
            deleteFigureImageAt(pawnChangeCoords);
            addFigureImageAt(pawnChangeCoords, new GeneralFigure(type, currPlayerColor));

            OnPawnChange(type);
        }
    }
}
