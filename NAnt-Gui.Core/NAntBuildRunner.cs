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
using NAnt.Core;
using NProject = NAnt.Core.Project;
using Project = NAntGui.Core.NAnt.Project;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for NAntBuildRunner.
	/// </summary>
	public class NAntBuildRunner : BuildRunner
	{
		public NAntBuildRunner(MainForm nantForm) : base(nantForm)
		{
			//
			// TODO: Add constructor logic here
			//
		}

		protected override IProject LoadingBuildFile(string buildFile)
		{
			NProject nantProject = new NProject(buildFile, Level.Info, 0);
			return new Project(nantProject);
		}

		protected override void DoRun()
		{
			try
			{
				Level projectThreshold = this.GetThreshold();

				NProject project = new NProject(_buildFile, projectThreshold, 0);
				project.BuildFinished += new BuildEventHandler(this.Build_Finished);

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

		private void SetTargetFramework(NProject project)
		{
			Assert.NotNull(project, "project");
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

		/// <summary>
		/// Add the listeners specified in the command line arguments,
		/// along with the default listener, to the specified project.
		/// </summary>
		/// <param name="project">The <see cref="NProject" /> to add listeners to.</param>
		private void AddBuildListeners(NProject project)
		{
			Assert.NotNull(project, "project");
			BuildListenerCollection listeners = new BuildListenerCollection();
			IBuildLogger buildLogger = new GuiLogger(_nantForm);

			// set threshold of build logger equal to threshold of project
			buildLogger.Threshold = project.Threshold;

			// add build logger to listeners collection
			listeners.Add(buildLogger);

			// attach listeners to project
			project.AttachBuildListeners(listeners);
		}

		private void Build_Finished(object sender, BuildEventArgs e)
		{
			base.FireBuildFinished();
		}
	}
}
