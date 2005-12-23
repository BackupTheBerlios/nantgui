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
		protected ILogsMessage _messageLogger;
		protected CommandLineOptions _options;

		private Watcher	_watcher = new Watcher();
		

		public SourceFile(ILogsMessage messageLogger, CommandLineOptions options)
		{
			_name			= "Untitled";
			_path			= ".\\";
			_fullName		= _path + _name;
			_extension		= "";
			_contents		= "";
			_messageLogger	= messageLogger;
			_options		= options;
		}

		/// <summary>
		/// Create a new build file.
		/// </summary>
		public SourceFile(string buildFile, string contents, 
			ILogsMessage messageLogger, CommandLineOptions options)
		{
			Assert.NotNull(buildFile, "buildFile");
			Assert.NotNull(contents, "contents");
			Assert.FileExists(buildFile);
            			
			_fullName		= buildFile;
			_contents		= contents;
			FileInfo info	= new FileInfo(_fullName);
			_name			= info.Name;
			_path			= info.DirectoryName;
			_extension		= info.Extension;
			_messageLogger	= messageLogger;
			_options		= options;

			_watcher.WatchBuildFile(info);
		}

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

		public ILogsMessage MessageLogger
		{
			get { return _messageLogger; }
		}

		public CommandLineOptions Options
		{
			get { return _options; }
		}

		#endregion
	}
}
