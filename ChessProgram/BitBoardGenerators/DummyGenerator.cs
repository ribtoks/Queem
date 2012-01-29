using System;

namespace MovesGenerators
{
	public class DummyGenerator : Generator
	{
		public override void Run ()
		{}
		
		public override object GetResults ()
		{
			return null;
		}
		
		public override void WriteResults (System.IO.TextWriter tw)
		{
		}
	}
}

