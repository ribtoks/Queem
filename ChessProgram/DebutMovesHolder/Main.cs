using System;

namespace DebutMovesHolder
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			DebutGraph dg = DebutsReader.ReadDebuts ("../simple_debut_moves", Queem.Core.PlayerPosition.Down);
			Console.WriteLine (dg.ToString ());
		}
	}
}

