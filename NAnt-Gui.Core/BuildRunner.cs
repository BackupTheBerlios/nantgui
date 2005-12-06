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
using System.Collections;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace NAntGui.Core
{
	public delegate void BuildFileChangedEH(IProject project);

	public abstract class BuildRunner
	{
		public event BuildFileChangedEH BuildFileChanged;
		public VoidVoid OnBuildFinished;

		private Watcher		_watcher;
		private Thread		_thread;
		
		protected ILogsMessage _messageLogger;
		protected string _buildFile;
		protected CommandLineOptions _options;

		public BuildRunner(MainForm mainForm)
		{
			_messageLogger	= mainForm;
			_options	= mainForm.Options;

			_watcher	= new Watcher();
			_watcher.BuildFileChanged += new VoidVoid(this.ReloadBuildFile);
			_watcher.BuildFileDeleted += new VoidVoid(mainForm.CloseBuildFile);
		}

		private void ReloadBuildFile()
		{
			this.LoadBuildFile(_buildFile);	
		}

		public void LoadBuildFile(string buildFile)
		{
			if (File.Exists(buildFile))
			{
				try
				{
					FileInfo buildFileInfo = new FileInfo(buildFile);
					Environment.CurrentDirectory = buildFileInfo.DirectoryName;

					_watcher.WatchBuildFile(buildFileInfo);

					IProject _project = this.LoadingBuildFile(buildFile);

					if (this.BuildFileChanged != null)
					{
//						if (_nantForm.InvokeRequired)
//						{
//							_nantForm.Invoke(this.BuildFileChanged, new object[] {_project});
//						}
//						else
//						{
							this.BuildFileChanged(_project);
//						}
					}
				}
				catch (ApplicationException error)
				{
					HandleErrorInBuildFile(error);
				}
#if RELEASE
				catch (Exception error)
				{
					// all other exceptions should have been caught
					string message = error.Message + Environment.NewLine + 
						error.StackTrace;
					MessageBox.Show(message, "Internal Error");
				}
#endif
			}
			else
			{
				throw new BuildFileNotFoundException(buildFile + " does not exist.");
			}
		}

		protected abstract IProject LoadingBuildFile(string buildFile);

		private static void HandleErrorInBuildFile(ApplicationException error)
		{
#if DEBUG
			string errorType = error.GetType().ToString() 
				+ Environment.NewLine + error.StackTrace;

			MessageBox.Show(errorType);
#endif

			if (error.InnerException != null && error.InnerException.Message != null)
			{
				string message = error.Message + Environment.NewLine + error.InnerException.Message;
				MessageBox.Show(message , "Error Loading Build File");
			}
			else
			{
				MessageBox.Show(error.Message, "Error Loading Build File");
			}
		}

		public void Run(string buildFile)
		{
			Assert.NotNull(buildFile, "buildFile");
			_buildFile = buildFile;

			_thread = new Thread(new ThreadStart(this.DoRun));
			_thread.Start();
		}

		protected abstract void DoRun();

		public void Stop()
		{
			if (_thread != null)
			{
				_thread.Abort();
			}
		}

//		protected void FireBuildFinished()
//		{
//			if (this.BuildFinished != null)
//			{
//				this.BuildFinished();
//			}
//		}
	}
}