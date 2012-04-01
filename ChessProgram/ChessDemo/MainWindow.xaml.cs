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
using DebutMovesHolder;

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

        public MainWindow()
        {
            InitializeComponent();

            this.gameProvider = new GameProvider();

            this.chessboardControl.SetupGameProvider(gameProvider);
            this.worker = new BackgroundWorker();

            this.redoMoves = new List<MoveWithDecision>();

            this.worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            this.maxdepth = 6;
            this.debutsGraph = DebutsReader.ReadDebuts("simple_debut_moves", PlayerPosition.Down);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == null)
            {
                if (this.gameProvider.IsStalemate(this.myColor))
                    MessageBox.Show("You're in stalemate");
                else
                    MessageBox.Show("You're in checkmate");
            }
            else
            {
                var move = e.Result as Move;
                this.gameProvider.ProcessMove(move, Queem.Core.Color.Black);
                this.chessboardControl.AnimateLast();
                bool needPawnPromotion = (int)move.Type >= (int)MoveType.Promotion;

                if (needPawnPromotion)
                    this.chessboardControl.PromotePawn(Queem.Core.Color.Black, move.To, move.Type.GetPromotionFigure());
            }
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
            this.StartSolver();
        }

        private void chessboardControl_MoveAnimationPreview(object sender, EventArgs e)
        {
            this.buttonsPanel.IsEnabled = false;
        }

        private void chessboardControl_PawnPromoted(object sender, EventArgs e)
        {
            this.canSolverStart = true;
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

            return false;
        }
    }
}
