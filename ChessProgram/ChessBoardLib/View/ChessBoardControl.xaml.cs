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

        public ChessBoardControl()
        {
            InitializeComponent();
        }

        public void SetupGameProvider(GameProvider provider)
        {
            this.viewModel = new ChessBoardViewModel(provider);
            this.DataContext = this.viewModel;
            this.viewModel.IsFigureMoving = true;
        }
    }
}
