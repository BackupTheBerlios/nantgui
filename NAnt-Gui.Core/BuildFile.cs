using System.IO;

namespace NAntGui.Core
{
	/// <summary>
	/// Abstract class for deriving build files types from.
	/// </summary>
	public abstract class BuildFile
	{
		protected const string UNTITLED_FILE = "Untitled";

		protected string _buildFileName;
		protected string _buildFilePath;
		protected string _buildFileText;

		/// <summary>
		/// Flag used to determine if the file has been
		/// modified since the last save.
		/// </summary>
		protected bool _isDirty = false;

		/// <summary>
		/// Create a new build file.
		/// </summary>
		public BuildFile()
		{
			_buildFileName = UNTITLED_FILE + "." + Extension;
			_buildFilePath = ".\\";
		}

		/// <summary>
		/// Create a build file from the given contents.
		/// </summary>
		/// <param name="buildFileContents">Text contents of a build file</param>
		public BuildFile(string buildFileContents) : this()
		{
			_buildFileText = buildFileContents;
		}

		/// <summary>
		/// Create a build file with the path to the file.
		/// </summary>
		/// <param name="buildFile">Path to the file</param>
		public BuildFile(FileInfo buildFile)
		{
			this.Load(buildFile);
		}

		public virtual void Load(FileInfo buildFile)
		{
			
		}

		public virtual void ReLoad()
		{
			
		}

		public virtual void Save(string text)
		{
			using (TextWriter writer = File.CreateText(_buildFilePath))
			{
				writer.Write(text);
			}
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

		public virtual bool SaveRequired
		{
			get{ return _isDirty; }
		}

		protected abstract string Extension { get; }
	}
}
