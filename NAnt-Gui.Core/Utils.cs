
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
	}
}
