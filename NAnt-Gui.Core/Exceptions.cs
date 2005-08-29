using System;

namespace NAntGui.Core
{
	internal class BuildFileNotFoundException : ApplicationException
	{
		public BuildFileNotFoundException(string s) : base(s){}
	}
}
