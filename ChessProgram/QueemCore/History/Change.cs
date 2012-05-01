using System;

namespace Queem.Core.History
{
	public enum MoveAction { Creation, Move, Deletion, PawnChange }

	public class Change
	{
		public MoveAction Action { get; set; }
		public Square Square { get; set; }
		public Square AdditionalSquare { get; set; }
		public int Data { get; set; }
		public Color FigureColor { get; set; }
		public Figure FigureType { get; set; }

        public Change() { }

        public Change(Change from)
        {
            this.Action = from.Action;
            this.Square = from.Square;
            this.AdditionalSquare = from.AdditionalSquare;
            this.Data = from.Data;
            this.FigureColor = from.FigureColor;
            this.FigureType = from.FigureType;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {3} at {4}",
                FigureColor,
                FigureType,
                Action,
                Square);
        }
	}
}

