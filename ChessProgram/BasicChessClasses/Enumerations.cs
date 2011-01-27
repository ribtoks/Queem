using System;

namespace BasicChessClasses
{
	public enum PromotionType { Rook = 2, Bishop = 4, Horse = 6, Queen = 10 };
	
	public enum FigureType { Pawn = 0, Rook = 1, Bishop = 2, Horse = 3, King = 4, Queen = 5,
		Nobody = 6, Null = 7 };

    public enum FigureColor { Black = 0, White = 1 };

    /// <summary>
    /// Letter in my indexing scheme
    /// </summary>
    public enum FieldLetter { A = 0, B = 1, C = 2, D = 3, E = 4, F = 5, G = 6, H = 7, Below = -1, 
		BelowBelow = -2, Above = 8, AboveAbove = 9 };

    /// <summary>
    /// Status of player in HUMANvsRemoteHUMAN game mode
    /// </summary>
    public enum PlayerStatus { OnlineFree, OnlineBusy, Offline };

    /// <summary>
    /// Start position of figures of some player
    /// </summary>
    public enum FigureStartPosition { Up = 2, Down = 7, DontKnow = -1 };

    /// <summary>
    /// Type of game, in which Chess Client is working
    /// </summary>
    public enum GameType { HUMANvsRemoteHUMAN, HUMANvsCPU, HUMANvsHUMAN };
	
	public static class FigureColorExtensions
	{
		public static FigureColor GetOppositeColor (this FigureColor color)
		{
			if (color == FigureColor.White)
				return FigureColor.Black;
			return FigureColor.White;
		}
	}
	
	public static class StartPositionExtensions
	{
		public static FigureStartPosition GetOppositePosition (this FigureStartPosition pos)
		{
			if (pos == FigureStartPosition.Up)
				return FigureStartPosition.Down;
			return FigureStartPosition.Up;
		}
	}
}

