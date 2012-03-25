using System;
using System.Collections.Generic;

namespace Queem.CoreInterface.Interface
{
	public abstract class Player
	{
		public abstract IEnumerable<Figure> FiguresManager { get; }
		
		public abstract FigureColor FiguresColor { get;}
	}
}

