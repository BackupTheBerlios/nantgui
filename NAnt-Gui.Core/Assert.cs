using System;

namespace NAntGui.Utils
{
	/// <summary>
	/// Summary description for Assert.
	/// </summary>
	public class Assert
	{
		public static void NotNull(object item, string param)
		{
			if (item == null) throw new ArgumentNullException(param);
		}
	}
}
