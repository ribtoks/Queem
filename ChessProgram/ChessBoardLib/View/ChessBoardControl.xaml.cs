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
using ChessBoardVisualLib.ViewModel;
using Queem.Core.ChessBoard;

namespace ChessBoardVisualLib.View
{
    /// <summary>
    /// Interaction logic for ChessBoardControl.xaml
    /// </summary>
    public partial class ChessBoardControl : UserControl
    {
        private ChessBoardViewModel viewModel;
        private Grid moveFromGrid;

        public ChessBoardControl()
        {
            InitializeComponent();
        }

        public void SetupGameProvider(GameProvider provider)
        {
            this.viewModel = new ChessBoardViewModel(provider);
            this.DataContext = this.viewModel;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as Grid;
            var squareItem = element.DataContext as SquareItem;

            var result = this.viewModel.MouseClick(squareItem);

            switch (result)
            {
                case Enums.MouseClickResults.NewMove:
                    this.moveFromGrid = element;
                    break;

                case Enums.MouseClickResults.MoveFinished:
                    this.AnimateLastMove();
                    break;

                case Enums.MouseClickResults.MoveCanceled:
                    this.moveFromGrid = null;
                    break;
            }
        }

        private void AnimateLastMove()
        {
            
        }
    }
}
