using System;

namespace QueemCore.History
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
	}
}

