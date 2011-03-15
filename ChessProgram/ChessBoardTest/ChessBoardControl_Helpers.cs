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

        protected bool StartFigureMoving(int i, int j)
        {
            GeneralFigure gf = mp.ChessBoard[j, i];
            if (gf.Type != FigureType.Nobody)
            {
                // i can move only mine figures
                if (gf.Color == currPlayerColor)
                {
                    // raise event
                    OnPlayerMoveStartPreview();

                    isMoving = true;
                    lastPossibleMoves = mp.GetFilteredCells(startCoord);
                    ShowPossibleCells(lastPossibleMoves);
                    return true;
                }
            }
            return false;
        }

        protected void FinishFigureMoving(int i, int j)
        {
            // if it's allowed move
            if (lastPossibleMoves.IndexOf(endCoord) != -1)
            {
                isMoving = false;
                HidePossibleCells(lastPossibleMoves);
                OnPlayerMoveFinish();
                //AnimateFigureMove(startCoord, endCoord);

                // (a real player move in event handler after animate)
            }
            else
            {
                GeneralFigure gf = mp.ChessBoard[j, i];
                if (gf.Type != FigureType.Nobody)
                {
                    // will return true if it's current
                    // player's figure, and false if opponent's one
                    if (gf.Color == currPlayerColor)
                    {
                        // if user clicked on his other figure
                        // initialize a new move
                        HidePossibleCells(lastPossibleMoves);

                        startCoord.Set(j, i);
                        StartFigureMoving(i, j);
                        return;
                    }
                }

                isMoving = false;
                HidePossibleCells(lastPossibleMoves);
            }

            lastPossibleMoves.Clear();
        }

        protected void innerAnimateFigureMove(ChessMove move)
        {
            var start = move.Start;
            var end = move.End;

            int startIndex = start.Y * 8 + start.X;
            Border startBorder = chessBoardGrid.Children[startIndex] as Border;
            Grid startGrid = startBorder.Child as Grid;
            Border startTarget = (startGrid.Children[0] as Grid).Children[0] as Border;
            int zIndex = Panel.GetZIndex(startBorder);
            Panel.SetZIndex(startBorder, 9001);


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

            story.Completed += new EventHandler((sender, e) =>
            {
                Panel.SetZIndex(startBorder, zIndex);
                ReplaceAnimationFigures(new ChessMove(move));

                animationsDone += 1;
                if (animationsDone == animationEventsCount)
                    OnPlayerMoveAnimationFinish();
            });

            story.Begin();
        }

        protected void innerAnimateCancelFigureMove(ChessMove move, GeneralFigure figureDied)
        {
            var start = move.Start;
            var end = move.End;

            int startIndex = start.Y * 8 + start.X;
            Border startBorder = chessBoardGrid.Children[startIndex] as Border;
            Grid startGrid = startBorder.Child as Grid;
            if (figureDied.Type != FigureType.Nobody)
                addFigureImageAt(start, figureDied);

            // -------------------------------------

            double deltaX = (start.X - end.X) * startGrid.ActualWidth;
            double deltaY = (start.Y - end.Y) * startGrid.ActualHeight;

            // -------------------------------------
            
            int finishIndex = end.Y * 8 + end.X;
            Border finishBorder = chessBoardGrid.Children[finishIndex] as Border;
            Grid finishGrid = finishBorder.Child as Grid;
            
            Grid grid = createFigureBoardGrid(mp.ChessBoard[end]);
            Border endTarget = grid.Children[0] as Border;
            endTarget.RenderTransform = new TranslateTransform(deltaX, deltaY);
            // must NOT clear children, 'cause of 
            // capture situation
            //finishGrid.Children.Clear();
            finishGrid.Children.Add(grid);
            if (figureDied.Type == FigureType.Nobody)
                deleteFigureImageAt(start);
            // ------------------------------------
            int zIndex = Panel.GetZIndex(finishBorder);
            Panel.SetZIndex(finishBorder, 9001);

            DoubleAnimation shiftX = new DoubleAnimation();
            shiftX.To = 0;
            shiftX.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            shiftX.AccelerationRatio = 0.4;
            shiftX.DecelerationRatio = 0.6;

            DoubleAnimation shiftY = new DoubleAnimation();
            shiftY.To = 0;
            shiftY.Duration = shiftX.Duration;
            shiftY.AccelerationRatio = shiftX.AccelerationRatio;
            shiftY.DecelerationRatio = shiftX.DecelerationRatio;

            Storyboard story = new Storyboard();
            story.Children.Add(shiftX);
            story.Children.Add(shiftY);

            Storyboard.SetTarget(shiftX, endTarget);
            Storyboard.SetTarget(shiftY, endTarget);

            Storyboard.SetTargetProperty(shiftX, new PropertyPath("RenderTransform.X"));
            Storyboard.SetTargetProperty(shiftY, new PropertyPath("RenderTransform.Y"));

            story.Completed += new EventHandler((sender, e) =>
            {
                Panel.SetZIndex(finishBorder, zIndex);
                finishGrid.Children.Clear();
                finishGrid.Children.Add(grid);
                chessBoardGrid.IsHitTestVisible = true;

                animationsDone += 1;
                if (animationsDone == animationEventsCount)
                    OnPlayerMoveAnimationFinish();
            });

            story.Begin();
        }

        protected void deleteFigureImageAt(Coordinates coords)
        {
            int index = coords.Y * 8 + coords.X;
            Border border = chessBoardGrid.Children[index] as Border;
            Grid grid= border.Child as Grid;
            grid.Children.Clear();

            grid.Children.Add(CreateFigureGrid(FigureType.Rook));
        }

        protected Grid createFigureBoardGrid(GeneralFigure gf)
        {
            ResourceDictionary rd = (ResourceDictionary)this.FindResource("Dictionaries");
            ResourceDictionary myStyles = rd.MergedDictionaries[0];

            // create grid with needed margins
            Grid grid = CreateFigureGrid(gf.Type);

            Border border = new Border();
            string brushName = string.Format("{0}{1}", gf.Color, gf.Type);
            object vb = myStyles[brushName];

            border.Background = (VisualBrush)vb;

            grid.Children.Add(border);
            Grid.SetColumn(border, 1);
            Grid.SetRow(border, 1);

            return grid;
        }

        protected void addFigureImageAt(Coordinates coords, GeneralFigure gf)
        {
            int index = coords.Y * 8 + coords.X;
            Border outerBorder = chessBoardGrid.Children[index] as Border;
            Grid parentGrid = outerBorder.Child as Grid;
            parentGrid.Children.Clear();

            // create grid with needed margins
            Grid grid = createFigureBoardGrid(gf);

            parentGrid.Children.Add(grid);

            // don't forget about event handlers,
            // but it looks like they're binded
            // to chessBoard uniform grid childs
        }
        protected void ReplaceAnimationFigures(ChessMove move)
        {
            Coordinates start = move.Start;
            Coordinates end = move.End;

            deleteFigureImageAt(start);
            addFigureImageAt(end, mp.ChessBoard[end]);

            chessBoardGrid.IsHitTestVisible = true;
        }

        public void AnimateFigureMove(DeltaChanges dc, ChessMove move, MoveResult moveResult)
        {
            OnPlayerMoveAnimationPreview();

            chessBoardGrid.IsHitTestVisible = false;
            if (moveResult == MoveResult.Castling)
                animationEventsCount = 2;
            else
                animationEventsCount = 1;
            animationsDone = 0;

            while (dc.Changes.Count > 0)
            {
                Change change = dc.Changes.Pop();
                switch (change.Action)
                {
                    case MoveAction.Move:
                        chessBoardGrid.IsHitTestVisible = false;
                        innerAnimateFigureMove(new ChessMove(change.Coords,
    change.AdditionalCoords));    
                        break;
                    case MoveAction.Deletion:
                        // delete figure image only when 
                        // processing in passing capture
                        if (change.Coords != move.End)
                            deleteFigureImageAt(change.Coords);
                        break;
                    case MoveAction.Creation:
                        addFigureImageAt(change.Coords,
                            new GeneralFigure(change.FigureType, 
                                change.FigureColor));
                        break;
                }
            }
        }

        public void AnimateCancelFigureMove(DeltaChanges dc, ChessMove move, MoveResult moveResult)
        {
            OnPlayerMoveAnimationPreview();

            chessBoardGrid.IsHitTestVisible = false;

            if (moveResult == MoveResult.Castling)
                animationEventsCount = 2;
            else
                animationEventsCount = 1;
            animationsDone = 0;

            GeneralFigure figureDied = new GeneralFigure();
            // select figure that was taken directly 
            // by move, but not passing pawn
            var deadFigures = dc.Changes.Where((x) => (x.Action == MoveAction.Deletion) && // was taken
                (x.FigureColor != mp.ChessBoard[move.Start].Color) && // opponents figure
                (x.Coords == move.End)); // not passing pawn
            if (deadFigures.Count() == 1)
            {
                var firstChange = deadFigures.First();
                figureDied = new GeneralFigure(firstChange.FigureType, firstChange.FigureColor);
            }
#if DEBUG
            else if (deadFigures.Count() > 1)
                throw new Exception("Something strange here");
#endif

            while (dc.Changes.Count > 0)
            {
                Change change = dc.Changes.Pop();
                switch (change.Action)
                {
                    case MoveAction.Move:
                        chessBoardGrid.IsHitTestVisible = false;
                        //if (moveResult == MoveResult.Castling)
                            innerAnimateCancelFigureMove(new ChessMove(change.AdditionalCoords,
                                change.Coords), figureDied);
                        //else
                        //    innerAnimateFigureMove(new ChessMove(change.AdditionalCoords,
                        //    change.Coords));
                        break;
                    case MoveAction.Creation:
                        //deleteFigureImageAt(change.Coords);
                        break;
                    case MoveAction.Deletion:
                        if (move.End != change.Coords)
                            addFigureImageAt(change.Coords,
                                new GeneralFigure(change.FigureType,
                                    change.FigureColor));
                        break;
                }
            }

            // raise event
            //OnPlayerMoveAnimatinFinish();
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