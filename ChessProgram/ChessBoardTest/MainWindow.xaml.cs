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
using System.Threading;
using QueemAI;
using System.ComponentModel;

namespace ChessBoardTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Data

        CH_MovesProvider mp;
        FigureColor myColor = FigureColor.White;
        FigureStartPosition myStartPos = FigureStartPosition.Down;
        List<MoveWithDecision> redoMoves;
        BackgroundWorker bw;

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            mp = new CH_MovesProvider(myColor, myStartPos);
            redoMoves = new List<MoveWithDecision>();
            bw = new BackgroundWorker();

            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            #region ChessBoard Events Handlers
            chessBoardControl.InitializeControl(mp);
            chessBoardControl.PlayerMoveFinished += new PlayerMoveEventHandler(chessBoardControl_PlayerMoveFinished);
            chessBoardControl.PlayerMoveAnimationFinished += new EventHandler(chessBoardControl_PlayerMoveAnimationFinished);
            chessBoardControl.PlayerMoveAnimationPreview += new EventHandler(chessBoardControl_PlayerMoveAnimationPreview);
            chessBoardControl.PawnChanged += new PawnChangedEventHandler(chessBoardControl_PawnChanged);
            #endregion
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ChessMove move = (ChessMove)e.Result;
            //chessBoardControl.RedrawAll();
            //return;

            MoveResult mr = mp.ProvideOpponetMove(move);

            chessBoardControl.AnimateFigureMove(new DeltaChanges(mp.History.LastChanges), mp.History.LastMove, mp.History.LastMoveResult);

            if ((mr == MoveResult.PawnReachedEnd) ||
                (mr == MoveResult.CapturedAndPawnReachedEnd))
            {
                PromotionType prType = (move as PromotionMove).Promotion;
                mp.ReplacePawn(move.End, (FigureType)prType, mp.ChessBoard[move.End].Color);
            }

            chessBoardControl.ChangePlayer();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            ChessSolver cs = new ChessSolver();

            ChessMove move = cs.SolveProblem(mp, chessBoardControl.CurrPlayerColor, 4);
            e.Result = move;
        }

        protected void chessBoardControl_PawnChanged(object source, PawnChangedEventArgs e)
        {
            mp.ReplacePawn(e.Coords, e.Type,
                chessBoardControl.CurrPlayerColor);
            chessBoardControl.ChangePlayer();
        }

        protected void chessBoardControl_PlayerMoveAnimationPreview(object sender, EventArgs e)
        {
            cancelButton.IsEnabled = false;
        }

        protected void chessBoardControl_PlayerMoveAnimationFinished(object sender, EventArgs e)
        {
            if (chessBoardControl.CurrPlayerColor != myColor)
            {
                bw.RunWorkerAsync();
            }

            if (mp.History.Count > 0)
                cancelButton.IsEnabled = true;
        }

        protected void chessBoardControl_PlayerMoveFinished(object source, PlayerMoveEventArgs e)
        {
            MoveResult mr = MoveResult.Fail;

            if (e.PlayerColor == myColor)
            {
                mr = mp.ProvideMyMove(new ChessMove(e.MoveStart, e.MoveEnd));
            }
            else
            {
                mr = mp.ProvideOpponetMove(new ChessMove(e.MoveStart, e.MoveEnd));
            }

            chessBoardControl.AnimateFigureMove(new DeltaChanges(mp.History.LastChanges), mp.History.LastMove, mp.History.LastMoveResult);

            if ((mr == MoveResult.PawnReachedEnd) ||
                (mr == MoveResult.CapturedAndPawnReachedEnd))
            {
                chessBoardControl.ReplacePawn(mp.History.LastMove.End, chessBoardControl.CurrPlayerColor);
            }
            else
                chessBoardControl.ChangePlayer();

            redoMoves.Clear();
            redoButton.IsEnabled = false;
            cancelButton.IsEnabled = true;
            // next line for debug - substitution for animation
            //chessBoardControl.RedrawAll();
        }

        protected void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (mp.History.Count == 0)
                return;

            ChessMove lastMove = new ChessMove(mp.History.LastMove);
            DeltaChanges lastChanges = new DeltaChanges(mp.History.LastChanges);
            MoveResult lastResult = mp.History.LastMoveResult;

            chessBoardControl.ChangePlayer();
            chessBoardControl.HideHighlitedCells();
            mp.CancelMove(chessBoardControl.CurrPlayerColor);
            chessBoardControl.AnimateCancelFigureMove(new DeltaChanges(lastChanges), lastMove, lastResult);

            redoButton.IsEnabled = true;
            MoveWithDecision mi = new MoveWithDecision();
            mi.Move = lastMove;
            if ((lastResult == MoveResult.CapturedAndPawnReachedEnd) ||
                (lastResult == MoveResult.PawnReachedEnd))
            {
                mi.PawnDecision = lastChanges.Changes.Where((x) => x.Action == MoveAction.Creation).Select((x) => x.FigureType).First();
            }
            redoMoves.Add(mi);

            if (mp.History.Count == 0)
                cancelButton.IsEnabled = false;
        }

        protected void redoButton_Click(object sender, RoutedEventArgs e)
        {
            if (redoMoves.Count == 0)
                return;

            MoveResult mr = MoveResult.Fail;
            MoveWithDecision redoInfo = redoMoves.Last();
            ChessMove move = redoInfo.Move;
            redoMoves.RemoveAt(redoMoves.Count - 1);

            if (chessBoardControl.CurrPlayerColor == myColor)
            {
                mr = mp.ProvideMyMove(move);
            }
            else
            {
                mr = mp.ProvideOpponetMove(move);
            }

            chessBoardControl.AnimateFigureMove(new DeltaChanges(mp.History.LastChanges), mp.History.LastMove, mp.History.LastMoveResult);

            if ((mr == MoveResult.PawnReachedEnd) ||
                (mr == MoveResult.CapturedAndPawnReachedEnd))
            {
                //chessBoardControl.ReplacePawn(mp.History.LastMove.End, chessBoardControl.CurrPlayerColor);
                mp.ReplacePawn(move.End, redoInfo.PawnDecision,
                    chessBoardControl.CurrPlayerColor);
            }

            chessBoardControl.ChangePlayer();

            if (redoMoves.Count == 0)
            {
                redoButton.IsEnabled = false;
            }
            cancelButton.IsEnabled = true;
        }
    }
}
