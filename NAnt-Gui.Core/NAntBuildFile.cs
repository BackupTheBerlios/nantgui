
namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for NantBuildFile.
	/// </summary>
	public class NantBuildFile : BuildFile
	{
		public NantBuildFile()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// This may be an issue.  ".build" is a bad
		/// extension for NAnt.  ".nant" would be better.
		/// Alternatively, the option should be given.
		/// </summary>
		protected override string Extension
		{
			get { return "build"; }
		}
	}
}
