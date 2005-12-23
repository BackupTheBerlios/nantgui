using System;
using NAntGui.Framework;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for BuildRunnerFactory.
	/// </summary>
	public class BuildRunnerFactory
	{
		public static IBuildRunner Create(SourceFile sourceFile)
		{
			switch (sourceFile.Extension)
			{
				default:
				case "nant":
				case "build":
					return new NAnt.NAntBuildRunner(sourceFile);
				case "proj":
					throw new NotImplementedException("Pete's code goes here :)");
			}
		}
	}
}
