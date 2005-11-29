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
using System.Threading;
using System.Windows.Forms;
using NAnt.Core;
using NProject = NAnt.Core.Project;

namespace NAntGui.Core
{
	public delegate void BuildFileChangedEH(Project project);

	public abstract class BuildRunner
	{
		public event BuildFileChangedEH BuildFileLoaded;
		public event BuildEventHandler BuildFinished;

		private Watcher		_watcher;
		private NAntForm	_nantForm;
		private Project		_myProject;
		private Thread		_thread;
		private string		_buildFile;
		private CommandLineOptions _options;

		public BuildRunner(NAntForm nantForm)
		{
			_nantForm	= nantForm;
			_options	= nantForm.Options;

			_watcher	= new Watcher();
			_watcher.BuildFileChanged += new VoidVoid(this.ReloadBuildFile);
			_watcher.BuildFileDeleted += new VoidVoid(_nantForm.CloseBuildFile);
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
					this.LoadingBuildFile(buildFile);
				}
				catch (ApplicationException error)
				{
					HandleErrorInBuildFile(error);
				}
				catch (Exception error)
				{
					// all other exceptions should have been caught
					string message = error.Message + Environment.NewLine + 
						error.StackTrace;
					MessageBox.Show(message, "Internal Error");
				}
			}
			else
			{
				throw new BuildFileNotFoundException(buildFile + " not found.");
			}
		}

		private void LoadingBuildFile(string buildFile)
		{
			FileInfo buildFileInfo = new FileInfo(buildFile);
			Environment.CurrentDirectory = buildFileInfo.DirectoryName;

			_watcher.WatchBuildFile(buildFileInfo);

			NProject nantProject = new NProject(buildFile, Level.Info, 0);
			_myProject = new Project(nantProject);

			if (this.BuildFileLoaded != null)
			{
				if (_nantForm.InvokeRequired)
				{
					_nantForm.Invoke(this.BuildFileLoaded, new object[] {_myProject});
				}
				else
				{
					this.BuildFileLoaded(_myProject);
				}
			}
		}

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

		private void DoRun()
		{
			try
			{
				Level projectThreshold = this.GetThreshold();

				NProject project = new NProject(_buildFile, projectThreshold, 0);
				project.BuildFinished += this.BuildFinished;

				if (_options.TargetFramework != null) 
				{
					this.SetTargetFramework(project);
				}

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

		private void SetTargetFramework(NAnt.Core.Project project)
		{
			FrameworkInfo frameworkInfo = project.Frameworks[_options.TargetFramework];

			if (frameworkInfo != null) 
			{
				project.TargetFramework = frameworkInfo; 
			} 
			else 
			{
				Console.Error.WriteLine("Invalid framework '{0}' specified.", 
					_options.TargetFramework);

				// insert empty line
				Console.Error.WriteLine();

				if (project.Frameworks.Count == 0) 
				{
					Console.Error.WriteLine("There are no supported frameworks available on your system.");
				} 
				else 
				{
					Console.Error.WriteLine("Possible values include:");
					// insert empty line
					Console.Error.WriteLine();

					foreach (string framework in project.Frameworks.Keys) 
					{
						Console.Error.WriteLine(" {0} ({1})", framework, project.Frameworks[framework].Description);
					}
				}
				// signal error
				return;
			}
		}

		private Level GetThreshold()
		{
			Level projectThreshold = Level.Info;
			// determine the project message threshold
			if (_options.Debug) 
			{
				projectThreshold = Level.Debug;
			} 
			else if (_options.Verbose) 
			{
				projectThreshold = Level.Verbose;
			} 
			else if (_options.Quiet) 
			{
				projectThreshold = Level.Warning;
			}
			return projectThreshold;
		}

		public void Stop()
		{
			if (_thread != null)
			{
				_thread.Abort();
			}
		}

		/// <summary>
		/// Add the listeners specified in the command line arguments,
		/// along with the default listener, to the specified project.
		/// </summary>
		/// <param name="project">The <see cref="Project" /> to add listeners to.</param>
		private void AddBuildListeners(NProject project)
		{
			BuildListenerCollection listeners = new BuildListenerCollection();
			IBuildLogger buildLogger = new GuiLogger(_nantForm);

			// set threshold of build logger equal to threshold of project
			buildLogger.Threshold = project.Threshold;

			// add build logger to listeners collection
			listeners.Add(buildLogger);

			// attach listeners to project
			project.AttachBuildListeners(listeners);
		}
	}
}