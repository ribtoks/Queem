using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasicChessClasses;

namespace ChessBoardTest
{
    public delegate void PlayerMoveEventHandler(object source, PlayerMoveEventArgs e);

    public class PlayerMoveEventArgs : EventArgs
    {
        public Coordinates MoveStart { get; set; }
        public Coordinates MoveEnd { get; set; }
        public FigureColor PlayerColor { get; set; }
    }

    public class PawnChangedEventArgs : EventArgs
    {
        public FigureType Type { get; set; }
        public Coordinates Coords { get; set; }
    }

    public delegate void PawnChangedEventHandler(object source, PawnChangedEventArgs e);
}
