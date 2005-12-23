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

using System.IO;

namespace NAntGui.Core
{
	/// <summary>
	/// Source file.
	/// </summary>
	public class SourceFile
	{
		public event VoidBool SourceChanged;

		protected const string UNTITLED_FILE = "Untitled";

		protected string _name;
		protected string _fullName;
		protected string _path;
		protected string _contents;

		private Watcher	_watcher = new Watcher();

		public SourceFile()
		{
			_name = UNTITLED_FILE;
			_fullName = ".\\";
			_contents = "";
		}

		/// <summary>
		/// Create a new build file.
		/// </summary>
		public SourceFile(string buildFile, string contents)
		{
			Assert.NotNull(buildFile, "buildFile");
			Assert.NotNull(contents, "contents");
			Assert.FileExists(buildFile);
            			
			_fullName = buildFile;
			_contents = contents;

			FileInfo info = new FileInfo(_fullName);
			_name = info.Name;
			_path = info.DirectoryName;
			_watcher.WatchBuildFile(info);
		}

//			try
//			{
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

		public virtual void Close()
		{
			_watcher.DisableEvents();
		}

		#region Properties

		public string Contents
		{
			get { return _contents; }
			set { _contents = value; }
		}

		public string FullPath
		{
			get { return _fullName; }
		}

		public string Name
		{
			get { return _name; }
		}

		public string Path
		{
			get { return _path; }
		}

		#endregion
	}
}
