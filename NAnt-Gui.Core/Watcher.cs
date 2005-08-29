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
// Colin Svingen (csvingen@businesswatch.ca)
#endregion

using System.IO;
using System.Threading;
using System.Windows.Forms;
using NAntGui.Utils;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Watcher.
	/// </summary>
	public class Watcher
	{
		private FileSystemWatcher _watcher = new FileSystemWatcher();
		private Core _core;
		private NAntForm _nantForm;

		public Watcher(Core core, NAntForm nantForm)
		{
			Assert.NotNull(core, "core");
			Assert.NotNull(nantForm, "nantForm");

			_core = core;
			_nantForm = nantForm;

			_watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
			_watcher.Changed += new FileSystemEventHandler(this.FileChanges);
			_watcher.Deleted += new FileSystemEventHandler(this.FileChanges);
		}

		public void WatchBuildFile(FileInfo buildFile)
		{
			Assert.NotNull(buildFile, "buildFile");

			_watcher.Path	= buildFile.DirectoryName;
			_watcher.Filter = "*" + buildFile.Extension;
			_watcher.EnableRaisingEvents = true;
		}

//		private void OnFileChanged(object sender, FileSystemEventArgs e)
//		{
//			if (this.InvokeRequired)
//			{
//				FileSystemEventHandler lEventHandler = new FileSystemEventHandler(this.FileChanges);
//				this.Invoke(lEventHandler, new object[] {sender, e});
//			}
//			else
//			{
//				this.FileChanges(sender, e);
//			}
//		}

		private void FileChanges(object sender, FileSystemEventArgs e)
		{
			this.DisableEvents();
			// without this the file changed event 
			// seems to be fired twice\
			Application.DoEvents();

			if (e.ChangeType == WatcherChangeTypes.Changed)
			{
				Thread.Sleep(100);
				_core.LoadBuildFile(e.FullPath);
			}
			else if (e.ChangeType == WatcherChangeTypes.Deleted)
			{
				_nantForm.DoClose();
			}
		}

		public void DisableEvents()
		{
			_watcher.EnableRaisingEvents = false;
		}
	}
}
