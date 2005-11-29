#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2005 Colin Svingen, Business Watch International
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
using System.Threading;
using System.Windows.Forms;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Watcher.
	/// </summary>
	public class Watcher
	{
		public event VoidVoid BuildFileChanged;
		public event VoidVoid BuildFileDeleted;
		private FileSystemWatcher _watcher = new FileSystemWatcher();

		public Watcher()
		{
			_watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
			_watcher.Changed += new FileSystemEventHandler(this.OnFileChanged);
			_watcher.Deleted += new FileSystemEventHandler(this.OnFileDeleted);
		}

		public void WatchBuildFile(FileInfo buildFile)
		{
			Assert.NotNull(buildFile, "buildFile");

			_watcher.Path = buildFile.DirectoryName;
			_watcher.Filter = "*" + buildFile.Extension;
			_watcher.EnableRaisingEvents = true;
		}

		private void OnFileChanged(object sender, FileSystemEventArgs e)
		{
			this.DisableEvents();
			// without this the file changed event 
			// seems to be fired twice\
			Application.DoEvents();

			Thread.Sleep(100);

			if (this.BuildFileChanged != null)
				this.BuildFileChanged();
		}

		private void OnFileDeleted(object sender, FileSystemEventArgs e)
		{
			this.DisableEvents();
			// without this the file changed event 
			// seems to be fired twice\
			Application.DoEvents();

			if (this.BuildFileDeleted != null)
				this.BuildFileDeleted();
		}

		public void DisableEvents()
		{
			_watcher.EnableRaisingEvents = false;
		}
	}
}