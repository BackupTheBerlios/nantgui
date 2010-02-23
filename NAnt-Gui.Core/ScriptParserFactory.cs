using System;
using NAntGui.Framework;
using NAntGui.NAnt;
using System.IO;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for ScriptParserFactory.
	/// </summary>
	public class ScriptParserFactory
	{
		public static IBuildScript Create(FileInfo fileInfo)
		{
            switch (fileInfo.Extension)
			{
				default:
				case "nant":
				case "build":
                    return new NAntBuildScript(fileInfo.FullName);
				case "proj":
					throw new NotImplementedException("Pete's code goes here :)");
			}
		}
	}
}
