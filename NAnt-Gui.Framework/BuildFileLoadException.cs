using System;

namespace NAntGui
{
	public class BuildFileLoadException : ApplicationException
	{
		public BuildFileLoadException(string s) : base(s)
		{
		}
	}
}