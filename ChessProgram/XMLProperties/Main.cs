using System;
using System.Collections.Generic;

namespace XMLProperties
{
	class MainClass
	{
		public static void PrintDictionary<T, Q> (Dictionary<T, Q> dict)
		{
			foreach (KeyValuePair<T, Q> pair in dict)
			{
				Console.WriteLine (pair.Key + " " + pair.Value);
			}
		}
		
		public static void Main (string[] args)
		{
			string fileName = args[0];
			
			InternetServersOptions options = new InternetServersOptions ();
			options.Parse (fileName);
			
			PrintDictionary (options.Properties);
			
			//Console.ReadLine ();	
		}
	}
}
