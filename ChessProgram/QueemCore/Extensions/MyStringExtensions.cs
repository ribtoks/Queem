using System;

namespace Queem.Core.Extensions
{
	public static class MyStringExtensions
	{
		public static string LJust (this string str, int length, char ch)
		{
			if (str.Length < length)
				return (new string (ch, length - str.Length)) + str;
			return str;
		}
		
		public static string MyReverse (this string str)
		{
			var arr = str.ToCharArray ();
			Array.Reverse (arr);
			return new string (arr);
		}
	}
}

