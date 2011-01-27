using System;

namespace BasicChessClasses
{
	public class WrongFormatException : ApplicationException
	{
		public WrongFormatException ()
			: base()
		{
		}
		
		public WrongFormatException (string message)
			: base(message)
		{
		}
		
		public WrongFormatException (string message, Exception innerException)
			: base(message, innerException)
		{			
		}
	}
}
