using System;

namespace BasicChessClasses
{
	public class ChessPlayerBase : ICloneable
	{
		FiguresManager figuresManager;
		FigureStartPosition startPos;
		FigureColor figureColor;

		public ChessPlayerBase (FigureColor color, FigureStartPosition pos)
		{
			startPos = pos;
			figureColor = color;
			
			figuresManager = new FiguresManager (startPos, figureColor);
		}
		
		protected ChessPlayerBase (ChessPlayerBase copy)
		{
			startPos = copy.startPos;
			figureColor = copy.figureColor;
			
			figuresManager = new FiguresManager (copy.figuresManager);
		}
		
		public FiguresManager FiguresManager
		{
			get { return figuresManager; }
			set { figuresManager = value; }
		}
	
		public FigureColor FiguresColor 
		{
			get { return this.figureColor; }
			set { figureColor = value; }
		}

		public FigureStartPosition StartPos 
		{
			get { return this.startPos; }
			set { startPos = value; }
		}

		#region ICloneable implementation
		
		public object Clone ()
		{
			return new ChessPlayerBase (this);
		}
		
		object ICloneable.Clone ()
		{
			return Clone ();
		}
		#endregion
		
		public void Reflect (bool withColors)
		{
			figuresManager.Reflect (withColors);
			
			if (withColors)
				figureColor = figureColor.GetOppositeColor ();
		}
	}
}
