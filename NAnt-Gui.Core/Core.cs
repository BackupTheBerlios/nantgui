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

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using NAnt.Core;
using NAntGui.Utils;
using NProject = NAnt.Core.Project;

namespace NAntGui.Core
{
	public delegate void BuildFileChangedEH(Project project);
	/// <summary>
	/// Summary description for Core.
	/// </summary>
	public class Core
	{
		public event BuildFileChangedEH BuildFileChanged;
		public event BuildEventHandler BuildFinished;

		private Watcher		_watcher;
		private NAntForm	_nantForm;
		private Project		_myProject;
		private Thread		_thread;
		private string		_buildFile;

		public Core(NAntForm nantForm)
		{
			_nantForm	= nantForm;
			_watcher	= new Watcher(this, _nantForm);
		}

		public void LoadBuildFile(string buildFile)
		{
			if (Utils.BuildfileIsValid(buildFile))
			{
				try
				{
					this.LoadingBuildFile(buildFile);
				}
				catch (ApplicationException error)
				{
					HandleErrorInBuildFile(error);
				}
//				catch (Exception error)
//				{
//					// all other exceptions should have been caught
//					MessageBox.Show(error.Message, "Internal Error");
//				}
			}
			else
			{
				throw new BuildFileNotFoundException(buildFile + " not found.");
			}
			
		}

		private void LoadingBuildFile(string buildFile)
		{
			FileInfo buildFileInfo		 = new FileInfo(buildFile);
			Environment.CurrentDirectory = buildFileInfo.DirectoryName;
	
			_watcher.WatchBuildFile(buildFileInfo);
	
			NProject nantProject = new NProject(buildFile, Level.Info, 0);
			_myProject = new Project(nantProject);
	
			if (this.BuildFileChanged != null)
			{
				if (_nantForm.InvokeRequired)
				{
					_nantForm.Invoke(this.BuildFileChanged, new object[]{ _myProject });
				}
				else
				{
					this.BuildFileChanged(_myProject);
				}
			}
		}

		private static void HandleErrorInBuildFile(ApplicationException error)
		{
			MessageBox.Show(error.GetType().ToString());
			MessageBox.Show(error.StackTrace);
	
			if (error.InnerException != null && error.InnerException.Message != null)
			{
				MessageBox.Show(error.Message, "Error encountered in build file");
			}
			else
			{
				MessageBox.Show(error.Message, "Error");
			}
		}

		public void Run(string buildFile)
		{
			Assert.NotNull(buildFile, "buildFile");
			_buildFile = buildFile;

			 _thread = new Thread(new ThreadStart(this.DoRun));
			_thread.Start();
		}

		private void DoRun()
		{
			try
			{
				NProject project = new NProject(_buildFile, Level.Debug, 0);
				project.BuildFinished += this.BuildFinished;

				_nantForm.AddTreeTargetsToBuild(project);
				_nantForm.AddPropertiesToProject(project);
				this.AddBuildListeners(project);

				project.Run();
			}
			catch (BuildException error)
			{
				_nantForm.OutputMessage(error.Message);
			}
		}

		/// <summary>
		/// Add the listeners specified in the command line arguments,
		/// along with the default listener, to the specified project.
		/// </summary>
		/// <param name="project">The <see cref="Project" /> to add listeners to.</param>
		private void AddBuildListeners(NAnt.Core.Project project)
		{
			BuildListenerCollection listeners = new BuildListenerCollection();
			IBuildLogger lBuildLogger = new GuiLogger(_nantForm);

			// set threshold of build logger equal to threshold of project
			//lBuildLogger.Threshold = pProject.Threshold;
			lBuildLogger.Threshold = Level.Info;

			// add build logger to listeners collection
			listeners.Add(lBuildLogger);

			// attach listeners to project
			project.AttachBuildListeners(listeners);
		}

		public void DisableWatcher()
		{
			_watcher.DisableEvents();
		}
	}
}
