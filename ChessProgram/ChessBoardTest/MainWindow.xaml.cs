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

namespace ChessBoardTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Data

        HH_MovesProvider mp;
        FigureColor myColor = FigureColor.White;
        FigureStartPosition myStartPos = FigureStartPosition.Down;

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            mp = new HH_MovesProvider(myColor, myStartPos);
            chessBoardControl.InitializeControl(mp);
            chessBoardControl.PlayerMoveFinished += new PlayerMoveEventHandler(chessBoardControl_PlayerMoveFinished);
        }

        protected void chessBoardControl_PlayerMoveFinished(object source, PlayerMoveEventArgs e)
        {
            if (e.PlayerColor == myColor)
            {
                mp.ProvideMyMove(new ChessMove(e.MoveStart, e.MoveEnd));
            }
            else
            {
                mp.ProvideOpponenMove(new ChessMove(e.MoveStart, e.MoveEnd));
            }
            chessBoardControl.ChangePlayer();
            chessBoardControl.AnimateFigureMove(new DeltaChanges(mp.History.LastChanges), mp.History.LastMove);
            // next line for debug - substitution for animation
            //chessBoardControl.RedrawAll();
        }
    }
}
