
namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for MSBuildBuildFile.
	/// </summary>
	public class MSBuildBuildFile : BuildFile
	{
		public MSBuildBuildFile()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		protected override string Extension
		{
			get { return "proj"; }
		}

	}
}
