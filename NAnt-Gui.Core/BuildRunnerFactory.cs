using System;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for BuildRunnerFactory.
	/// </summary>
	public class BuildRunnerFactory
	{
		public static BuildRunner Create(string extension, ILogsMessage messageLogger, CommandLineOptions options)
		{
			switch (extension)
			{
				default:
				case "nant":
				case "build":
					return new NAnt.NAntBuildRunner(messageLogger, options);
				case "proj":
					throw new NotImplementedException("Pete's code goes here :)");
			}
		}
	}
}
