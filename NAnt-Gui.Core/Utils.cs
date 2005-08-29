using System.IO;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Core.
	/// </summary>
	public class Utils
	{
		private static readonly string[] Extensions = {".build", ".nant"};

		public static bool ExtensionIsValid(string pFile)
		{
			string lFileExt = new FileInfo(pFile).Extension;
			foreach (string lExtension in Extensions)
			{
				if (lFileExt == lExtension) return true;
			}

			return false;
		}

		public static bool BuildfileIsValid(string buildFile)
		{
			return File.Exists(buildFile) && ExtensionIsValid(buildFile);
		}
	}
}