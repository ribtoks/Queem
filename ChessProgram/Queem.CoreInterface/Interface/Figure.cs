using System;

namespace Queem.CoreInterface.Interface
{
	public abstract class Figure
	{
		public abstract Coordinates Coordinates { get; }
		public abstract FigureType Type { get; }
		public abstract FigureColor Color { get; }
	}
}

