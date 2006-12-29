#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2007 Colin Svingen
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

namespace NAntGui.Framework
{
	/// <summary>
	/// Source file.
	/// </summary>
	public class SourceFile
	{
		protected string _name;
		protected string _fullName;
		protected string _path;
		protected string _extension;
		protected string _contents;

//		private Watcher _watcher = new Watcher();

		public SourceFile()
		{
			_name = "Untitled";
			_path = ".\\";
			_fullName = _path + _name;
			_extension = "";
			_contents = "";
		}

		/// <summary>
		/// Create a new build file.
		/// </summary>
		public SourceFile(string buildFile, string contents)
		{
			Assert.NotNull(buildFile, "buildFile");
			Assert.NotNull(contents, "contents");

			_fullName		= buildFile;
			_contents		= contents;

			FileInfo info	= new FileInfo(_fullName);
			_name			= info.Name;
			_path			= info.DirectoryName;
			_extension		= info.Extension;

//			_watcher.WatchBuildFile(info);
		}

		public virtual void Close()
		{
//			_watcher.DisableEvents();
		}

		#region Properties

		public string Contents
		{
			get { return _contents; }
			set { _contents = value; }
		}

		public string FullName
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

		public string Extension
		{
			get { return _extension; }
		}

		#endregion
	}
}