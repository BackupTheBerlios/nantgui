using System;
using NAntGui.Framework;
using NAntGui.NAnt;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for ScriptParserFactory.
	/// </summary>
	public class ScriptParserFactory
	{
		public static IBuildScript Create(SourceFile file)
		{
			switch (file.Extension)
			{
				default:
				case "nant":
				case "build":
					return new NAntBuildScript(file.FullName);
				case "proj":
					throw new NotImplementedException("Pete's code goes here :)");
			}
		}
	}
}
