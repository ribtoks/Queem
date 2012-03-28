using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Queem.Core;

namespace ChessBoardTest
{
    public delegate void PlayerMoveEventHandler(object source, PlayerMoveEventArgs e);

    public class PlayerMoveEventArgs : EventArgs
    {
        public Square MoveStart { get; set; }
        public Square MoveEnd { get; set; }
        public Color PlayerColor { get; set; }
    }

    public class PawnChangedEventArgs : EventArgs
    {
        public Figure FigureType { get; set; }
        public Square Square { get; set; }
    }

    public delegate void PawnChangedEventHandler(object source, PawnChangedEventArgs e);
}
