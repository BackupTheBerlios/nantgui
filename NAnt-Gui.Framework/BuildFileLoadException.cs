using System;

namespace NAntGui.Framework
{
	public class BuildFileLoadException : ApplicationException
	{
		public BuildFileLoadException(string s) : base(s)
		{
		}
	}
}