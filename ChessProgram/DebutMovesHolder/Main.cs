using System;
using BasicChessClasses;

namespace DebutMovesHolder
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			DebutGraph dg = DebutsReader.ReadDebuts ("../simple_debut_moves", FigureStartPosition.Up);
			Console.WriteLine (dg.ToString ());
		}
	}
}

