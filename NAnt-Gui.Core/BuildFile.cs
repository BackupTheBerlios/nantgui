#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2005 Colin Svingen
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Colin Svingen (nantgui@swoogan.com)

#endregion

using System;
using System.IO;

namespace NAntGui.Core
{
	/// <summary>
	/// Abstract class for deriving build files types from.
	/// </summary>
	public abstract class BuildFile
	{
		protected const string UNTITLED_FILE = "Untitled";

		protected string _name;
		protected string _fullPath;
		protected string _contents;

		private Watcher	_watcher = new Watcher();

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
			_name = UNTITLED_FILE + "." + Extension;
			_fullPath = ".\\";
		}

		/// <summary>
		/// Create a build file from the given contents.
		/// </summary>
		/// <param name="buildFileContents">Text contents of a build file</param>
		public BuildFile(string buildFileContents) : this()
		{
			_contents = buildFileContents;
		}

		/// <summary>
		/// Create a build file with the path to the file.
		/// </summary>
		/// <param name="buildFile">Path to the file</param>
		public BuildFile(FileInfo buildFile)
		{
			this.Load(buildFile);
		}

		public virtual void Load(string buildFile)
		{
			Assert.NotNull(buildFile, "buildFile");
			Assert.FileExists(buildFile);
			
//			try
//			{
				FileInfo info = new FileInfo(buildFile);
				this.Load(info);
//			}
//			catch (ApplicationException error)
//			{
//				HandleErrorInBuildFile(error);
//			}
//			catch (Exception error)
//			{
//				// all other exceptions should have been caught
//				string message = error.Message + Environment.NewLine + 
//					error.StackTrace;
//				MessageBox.Show(message, "Internal Error");
//			}
		}

		public virtual void Load(FileInfo buildFile)
		{
			using (TextReader reader = buildFile.OpenText())
			{
				_contents = reader.ReadToEnd();
			}

			_fullPath = buildFile.FullName;
			_name = buildFile.Name;
			_watcher.WatchBuildFile(buildFile);
		}

		public virtual void ReLoad()
		{
			
		}

//		public virtual void Save(string text)
//		{
//			using (TextWriter writer = File.CreateText(_fullPath))
//			{
//				writer.Write(text);
//			}
//		}

		public virtual void Save()
		{
			using (TextWriter writer = File.CreateText(_fullPath))
			{
				writer.Write(_contents);
			}
		}

		public virtual void SaveAs(string fileName)
		{
			_fullPath = fileName;
			this.Save();
		}

		public virtual void Close()
		{
			_watcher.DisableEvents();
		}

		public virtual bool SaveRequired
		{
			get{ return _isDirty; }
		}

		protected abstract string Extension { get; }

		#region Properties

		public string Contents
		{
			get { return _contents; }
			set { _contents = value; }
		}

		#endregion
	}
}
