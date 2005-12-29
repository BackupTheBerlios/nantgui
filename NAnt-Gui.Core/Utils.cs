using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Utils.
	/// </summary>
	public class Utils
	{
		public static bool HasAsterisk(string text)
		{
			return text.EndsWith("*");
		}

		public static string RemoveAsterisk(string text)
		{
			return text.TrimEnd(new char[] {'*'});
		}
		
		public static string AddAsterisk(string text)
		{
			return text += "*";
		}

		public static void LoadHelpFile(string filename)
		{
			if (File.Exists(filename))
			{
				Process.Start(filename);
			}
			else
			{
				MessageBox.Show("Help not found.  It may not have been installed.", "Help Not Found");
			}
		}

		public static string GetRunDirectory()
		{
			Assembly ass = Assembly.GetExecutingAssembly();
			FileInfo info = new FileInfo(ass.Location);
			return info.DirectoryName;
		}
	}
}
