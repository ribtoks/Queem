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

namespace ChessBoardTest
{
    /// <summary>
    /// Interaction logic for Chessboard.xaml
    /// </summary>
    public partial class ChessBoardControl : UserControl
    {
        protected MovesProvider mp;

        public ChessBoardControl()
        {
            InitializeComponent();
            mp = new MovesProvider(FigureColor.White, FigureStartPosition.Down);
            InitializeBoard();
        }

        protected void InitializeBoard()
        {
            ResourceDictionary rd = (ResourceDictionary)this.FindResource("Dictionaries");
            ResourceDictionary myStyles = rd.MergedDictionaries[0];

            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    GeneralFigure gf = mp.ChessBoard[j, i];

                    if (gf.Type == FigureType.Nobody)
                        continue;

                    Border border = new Border();
                    string brushName = string.Format("{0}{1}", gf.Color, gf.Type);
                    object vb = myStyles[brushName];
                    
                    border.Background = (VisualBrush)vb;

                    int index = i * 8 + j;
                    Border existingBorder = (Border)chessBoardGrid.Children[index];
                    existingBorder.Child = border;
                }
            }
        }
    }
}
