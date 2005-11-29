using System.IO;

namespace NAntGui.Core
{
	/// <summary>
	/// Abstract class for deriving build files types from.
	/// </summary>
	public abstract class BuildFile
	{
		/// <summary>
		/// Create a new build file.
		/// </summary>
		public BuildFile()
		{

		}

		/// <summary>
		/// Create a build file from the given contents.
		/// </summary>
		/// <param name="buildFileContents">Text contents of a build file</param>
		public BuildFile(string buildFileContents)
		{

		}

		/// <summary>
		/// Create a build file with the path to the file.
		/// </summary>
		/// <param name="buildFile">Path to the file</param>
		public BuildFile(FileInfo buildFile)
		{
			
		}

		public virtual void Load(FileInfo buildFile)
		{
			
		}

		public virtual void ReLoad()
		{
			
		}

		public virtual void Save()
		{
			
		}

		public virtual void SaveAs()
		{
			
		}

		public virtual void SaveAs(string buildFileContents)
		{
			
		}

		public virtual void Close()
		{
			
		}

		public virtual void Build()
		{
			
		}
	}
}
