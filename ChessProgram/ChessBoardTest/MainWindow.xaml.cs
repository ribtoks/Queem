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
using System.Threading;
using System.ComponentModel;
using System.IO;
using Queem.CoreInterface;
using Queem.Core.ChessBoard;
using DebutMovesHolder;
using Queem.Core;
using Queem.CoreInterface.Adapters;
using Queem.AI;
using Queem.Core.History;

namespace ChessBoardTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Data

        GameProvider gameProvider;
        Queem.Core.Color myColor = Queem.Core.Color.White;
        PlayerPosition myStartPos = PlayerPosition.Down;
        List<MoveWithDecision> redoMoves;
        BackgroundWorker bw;
        bool solverDisabled = false;
        DebutGraph simpleDebuts;

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            this.gameProvider = new GameProvider();
            this.redoMoves = new List<MoveWithDecision>();
            this.bw = new BackgroundWorker();

            this.bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            this.bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            #region ChessBoard Events Handlers
            chessBoardControl.InitializeControl(new MovesProviderAdapter(this.gameProvider));
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

            Move move = (Move)e.Result;

            this.gameProvider.ProcessMove(move, chessBoardControl.CurrPlayerColor);

            chessBoardControl.AnimateFigureMove(
                new DeltaChange(mp.History.LastChanges),
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

            Move move = cs.SolveProblem(this.gameProvider,
                chessBoardControl.CurrPlayerColor,
                6);
            e.Result = move;
        }

        protected void chessBoardControl_PawnChanged(object source, PawnChangedEventArgs e)
        {
            this.gameProvider.PromotePawn(chessBoardControl.CurrPlayerColor, e.Square, e.FigureType);

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
                if (this.gameProvider.IsCheckmate(chessBoardControl.CurrPlayerColor))
                {
                    MessageBox.Show("You checkmated computer.");
                    return;
                }

                if (this.gameProvider.IsStalemate(chessBoardControl.CurrPlayerColor))
                {
                    MessageBox.Show("Computer is in stalemate.");
                }

                bw.RunWorkerAsync();
            }

            if (this.gameProvider.History.HasItems())
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
                this.gameProvider.ProcessMove(
                mr = mp.ProvideMyMove(new Move(e.MoveStart, e.MoveEnd));
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
            /*
            if (!this.gameProvider.History.HasItems())
                return;

            Move lastMove = new ChessMove(mp.History.LastMove);
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
             */
        }

        protected void redoButton_Click(object sender, RoutedEventArgs e)
        {
            /*
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
             * */
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + "chess.game";

            System.IO.File.WriteAllLines(path, mp.History.Moves.Select(x => x.ToString()).ToArray());
        }

        private void readButton_Click(object sender, RoutedEventArgs e)
        {
            this.gameProvider.History.ClearAll();
            redoMoves = new List<MoveWithDecision>();

            string path = Directory.GetCurrentDirectory() + System.IO.Path.DirectorySeparatorChar + "chess.game";

            string[] lines = System.IO.File.ReadAllLines(path);
            int i = 0;
            foreach (var line in lines)
            {
                if ((i % 2) == 0)
                    this.gameProvider.ProcessMove(new Move(line), Queem.Core.Color.White);
                else
                    this.gameProvider.ProcessMove(new Move(line), Queem.Core.Color.Black);
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
