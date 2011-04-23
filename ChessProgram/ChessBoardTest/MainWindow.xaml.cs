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
using System.IO;
using DebutMovesHolder;

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
        bool solverDisabled = false;
        DebutGraph simpleDebuts;

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

            // TODO implement two difference debuts db (depends on start pos)
            this.simpleDebuts = DebutsReader.ReadDebuts("simple_debut_moves", myStartPos);
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == null)
            {
                if (mp.IsStalemate(mp.Player1, mp.Player2))
                    MessageBox.Show("You're in stalemate.");
                else
                    MessageBox.Show("You're checkmated.");
                return;
            }

            ChessMove move = (ChessMove)e.Result;

            MoveResult mr = mp.ProvideOpponetMove(move);

            chessBoardControl.AnimateFigureMove(
                new DeltaChanges(mp.History.LastChanges),
                mp.History.LastMove,
                mp.History.LastMoveResult);

            if ((mr == MoveResult.PawnReachedEnd) ||
                (mr == MoveResult.CapturedAndPawnReachedEnd))
            {
                PromotionType prType = (move as PromotionMove).Promotion;
                mp.ReplacePawn(move.End, (FigureType)prType, mp.ChessBoard[move.End].Color);
                // TODO add replace pawn image code here
            }

            //chessBoardControl.RedrawAll();

            chessBoardControl.ChangePlayer();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            ChessSolver cs = new ChessSolver(this.simpleDebuts);

            ChessMove move = cs.SolveProblem(mp,
                chessBoardControl.CurrPlayerColor,
                6);
            e.Result = move;
        }

        protected void chessBoardControl_PawnChanged(object source, PawnChangedEventArgs e)
        {
            mp.ReplacePawn(e.Coords, e.Type,
                chessBoardControl.CurrPlayerColor);
            chessBoardControl.ChangePlayer();

            solverDisabled = false;
            StartSolver();
        }

        protected void chessBoardControl_PlayerMoveAnimationPreview(object sender, EventArgs e)
        {
            cancelButton.IsEnabled = false;
        }

        protected void StartSolver()
        {
            if (solverDisabled)
                return;

            if (chessBoardControl.CurrPlayerColor != myColor)
            {
                if (mp.IsCheckmate(mp.Player2, mp.Player1))
                {
                    MessageBox.Show("You checkmated computer.");
                    return;
                }

                if (mp.IsStalemate(mp.Player2, mp.Player1))
                {
                    MessageBox.Show("Computer is in stalemate.");
                }

                bw.RunWorkerAsync();
            }

            if (mp.History.Count > 0)
                cancelButton.IsEnabled = true;
        }

        protected void chessBoardControl_PlayerMoveAnimationFinished(object sender, EventArgs e)
        {
            StartSolver();
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

            bool needChangePawn = (mr == MoveResult.PawnReachedEnd) ||
                (mr == MoveResult.CapturedAndPawnReachedEnd);

            if (needChangePawn)
                solverDisabled = true;

            chessBoardControl.AnimateFigureMove(new DeltaChanges(mp.History.LastChanges), 
                mp.History.LastMove, mp.History.LastMoveResult);

            if (needChangePawn)
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

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + "chess.game";

            File.WriteAllLines(path, mp.History.Moves.Select(x => x.ToString()).ToArray());
        }

        private void readButton_Click(object sender, RoutedEventArgs e)
        {
            mp.ResetAll();
            redoMoves = new List<MoveWithDecision>();

            string path = Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + "chess.game";

            string[] lines = File.ReadAllLines(path);
            int i = 0;
            foreach (var line in lines)
            {
                if ((i % 2) == 0)
                    mp.ProvideMyMove(new ChessMove(line));
                else
                    mp.ProvideOpponetMove(new ChessMove(line));
                i += 1;
            }
            chessBoardControl.RedrawAll();
            if ((i % 2) == 0)
            {
                if (chessBoardControl.CurrPlayerColor != myColor)
                    chessBoardControl.ChangePlayer();
            }
            else
            {
                if (chessBoardControl.CurrPlayerColor == myColor)
                    chessBoardControl.ChangePlayer();
            }
        }
    }
}
