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
using System.ComponentModel;
using Queem.Core.ChessBoard;
using Queem.Core;
using Queem.AI;
using Queem.Core.Extensions;
using DebutsLib;
using System.IO;
using System.Diagnostics;

namespace ChessDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker worker;
        private GameProvider gameProvider;
        private DebutGraph debutsGraph;

        private Queem.Core.Color myColor = Queem.Core.Color.White;
        private PlayerPosition myPosition = PlayerPosition.Down;

        private List<MoveWithDecision> redoMoves;
        private int maxdepth;
        private bool canSolverStart;
        private bool needsOneMoreCancel = false;
        private bool isSolverMove = false;
        private bool isCanceling = false;

        public MainWindow()
        {
            InitializeComponent();

            this.gameProvider = new GameProvider(new MovesArrayAllocator());

            this.chessboardControl.SetupGameProvider(gameProvider);
            this.worker = new BackgroundWorker();

            this.redoMoves = new List<MoveWithDecision>();

            this.worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            this.maxdepth = 5;
            this.debutsGraph = DebutsReader.ReadDebuts("simple_debut_moves", PlayerPosition.Down);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.buttonsPanel.IsEnabled = true;

            if (e.Result == null)
            {
                if (this.gameProvider.IsStalemate(this.myColor))
                    MessageBox.Show("You're in stalemate");
                else
                    MessageBox.Show("You're in checkmate");

                this.chessboardControl.IsEnabled = false;
            }
            else
            {
                this.isSolverMove = true;
                var move = e.Result as Move;
                this.gameProvider.ProcessMove(move, Queem.Core.Color.Black);
                this.chessboardControl.AnimateLast();
                bool needPawnPromotion = (int)move.Type >= (int)MoveType.Promotion;

                if (needPawnPromotion)
                    this.chessboardControl.PromotePawn(Queem.Core.Color.Black, move.To, move.Type.GetPromotionFigure());

                this.chessboardControl.ChangeCurrentPlayer();                
            }

            this.canSolverStart = false;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ChessSolver solver = new ChessSolver(this.debutsGraph);
            e.Result = solver.SolveProblem(
                this.gameProvider, 
                this.chessboardControl.CurrentPlayerColor, 
                this.maxdepth);
        }

        private void chessboardControl_MoveFinished(object sender, EventArgs e)
        {
            var lastMove = this.gameProvider.History.GetLastMove();
            bool needPawnPromotion = (int)lastMove.Type >= (int)MoveType.Promotion;
            this.canSolverStart = true;

            this.chessboardControl.AnimateLast();

            if (needPawnPromotion)
            {
                this.chessboardControl.UserPromotePawn();
                this.canSolverStart = false;
            }

            this.chessboardControl.ChangeCurrentPlayer();
            this.UpdateRedoStatus();
        }

        private void UpdateRedoStatus()
        {
            // TODO replace with viewmodel
            this.redoMoves.Clear();
            this.redoButton.IsEnabled = false;
            this.cancelButton.IsEnabled = true;
        }

        private void chessboardControl_MoveAnimationFinished(object sender, EventArgs e)
        {
            if (needsOneMoreCancel)
            {
                this.CancelLastMove();
                this.needsOneMoreCancel = false;
            }
            else
            {
                if (!this.isSolverMove && !this.isCanceling)
                    this.buttonsPanel.IsEnabled = false;
                else
                {
                    if (this.isSolverMove)
                        this.isSolverMove = false;

                    if (this.isCanceling)
                    {
                        this.cancelButton.IsEnabled = true;
                        this.isCanceling = false;

                        if (!this.gameProvider.History.HasItems())
                            this.cancelButton.IsEnabled = false;
                    }
                }

                this.StartSolver();
            }
        }

        private void chessboardControl_MoveAnimationPreview(object sender, EventArgs e)
        {
            if (!this.isSolverMove && !this.isCanceling)
                this.buttonsPanel.IsEnabled = false;
        }

        private void chessboardControl_PawnPromoted(object sender, EventArgs e)
        {
            this.canSolverStart = true;
            this.StartSolver();
        }

        private void StartSolver()
        {
            if (!this.canSolverStart)
                return;

            if (this.CanComputerContinue())
                this.worker.RunWorkerAsync();
        }

        private bool CanComputerContinue()
        {
            if (this.gameProvider.IsCheckmate(Queem.Core.Color.Black))
            {
                MessageBox.Show("You checkmated computer");
                return false;
            }

            if (this.gameProvider.IsStalemate(Queem.Core.Color.Black))
            {
                MessageBox.Show("Computer is in stalemate");
                return false;
            }

            return true;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetCurrentDirectory() +
                System.IO.Path.DirectorySeparatorChar + "chess.game";

            System.IO.File.WriteAllLines(path,
                this.gameProvider.History.GetMovesArray());

            MessageBox.Show("Game saved");
        }

        private void readButton_Click(object sender, RoutedEventArgs e)
        {
            this.gameProvider = new GameProvider(this.gameProvider.Allocator);
            this.chessboardControl.SetupGameProvider(this.gameProvider);
            this.redoMoves = new List<MoveWithDecision>();

            string path = Directory.GetCurrentDirectory() + 
                System.IO.Path.DirectorySeparatorChar + "chess.game";

            string[] lines = System.IO.File.ReadAllLines(path);
            Queem.Core.Color color = Queem.Core.Color.White;
            foreach (var line in lines)
            {
                var move = new Move(line);
                
                if (this.gameProvider.PlayerBoards[(int)color].Figures[(int)move.From] == Queem.Core.Figure.King)
                    if (Math.Abs((int)move.From - (int)move.To) == 2)
                        move.Type = MoveType.KingCastle;

                this.gameProvider.ProcessMove(move, color);
                color = (Queem.Core.Color)(1 - (int)color);
            }

            this.chessboardControl.RedrawAll();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.CancelLastMove();
            this.needsOneMoreCancel = true;
        }

        private void CancelLastMove()
        {
            if (!this.gameProvider.History.HasItems())
                return;

            var lastMove = new Move(this.gameProvider.History.GetLastMove());
            var lastDeltaChanges = this.gameProvider.History.GetLastDeltaChange().GetCopy();

            this.isCanceling = true;
            this.cancelButton.IsEnabled = false;
            this.chessboardControl.ChangeCurrentPlayer();

            this.gameProvider.CancelLastMove(this.chessboardControl.CurrentPlayerColor);
            this.chessboardControl.AnimateCancelMove(lastDeltaChanges, lastMove);

            redoButton.IsEnabled = true;
            redoMoves.Add(new MoveWithDecision() { Move = lastMove, Decision = lastMove.Type.GetPromotionFigure() });

        }

        private void redoButton_Click(object sender, RoutedEventArgs e)
        {
            if (redoMoves.Count == 0)
                return;

            var moveWithDecition = redoMoves.Last();
            redoMoves.RemoveAt(redoMoves.Count - 1);

            this.gameProvider.ProcessMove(moveWithDecition.Move,
                this.chessboardControl.CurrentPlayerColor);

            this.chessboardControl.AnimateLast();

            if (moveWithDecition.Decision != Queem.Core.Figure.Nobody)
            {
                this.chessboardControl.PromotePawn(
                    this.chessboardControl.CurrentPlayerColor,
                    moveWithDecition.Move.To,
                    moveWithDecition.Decision);
            }

            this.chessboardControl.ChangeCurrentPlayer();

            this.redoButton.IsEnabled = (this.redoMoves.Count > 0);
        }
    }
}
