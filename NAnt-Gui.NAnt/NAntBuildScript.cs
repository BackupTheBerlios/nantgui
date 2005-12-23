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

using System;
using Flobbster.Windows.Forms;
using NAnt.Core.Util;
using NAnt.Core;
using NAntGui.Framework;
using TargetCollection = NAntGui.Framework.TargetCollection;

namespace NAntGui.NAnt
{
	/// <summary>
	/// Contains the logic for parsing the build file.
	/// </summary>
	public class NAntBuildScript : IProject
	{
		public event VoidVoid BuildFinished;

		private string _description = "";
		private Project _project;
		private string _sourceFilePath;
		private CommandLineOptions _options;
		private ILogsMessage _messageLogger;

		private TargetCollection _targets		= new TargetCollection();
		private PropertyCollection _properties	= new PropertyCollection();
		private DependsCollection _depends		= new DependsCollection();

		/// <summary>
		/// Create a new project parser.
		/// </summary>
		public NAntBuildScript(string sourceFilePath, CommandLineOptions options, ILogsMessage messageLogger)
		{
			_sourceFilePath = sourceFilePath;
			_options = options;
			_messageLogger = messageLogger;
			_project = new Project(_sourceFilePath, this.GetThreshold(), 0);

			this.ParseBuildScript();
		}

		private void ParseBuildScript()
		{
			ScriptParser parser = new ScriptParser(_project);
			parser.Parse();

			_description	= parser.Description;
			_targets		= parser.Targets;
			_depends		= parser.Depends;
			_properties		= parser.Properties;
		}


		public void SetProjectProperties(PropertySpecCollection properties)
		{
			foreach (PropertySpec spec in properties)
			{
				if (spec.Category == "Project")
				{
					_project.BaseDirectory = spec.ToString();
				}
				else if (spec.Category == "Global" || ValidTarget(spec.Category))
				{
					this.LoadNonProjectProperty(spec, properties);
				}
			}
		}

		private void LoadNonProjectProperty(PropertySpec spec, PropertySpecCollection properties)
		{
			string lValue = properties.ToString();
			string lExpandedProperty = lValue;
			try
			{
				lExpandedProperty = _project.ExpandProperties(lValue,
					new Location(_sourceFile.FullPath));
			}
			catch (BuildException)
			{ /* ignore */
			}

			_project.Properties.AddReadOnly(spec.Name, lExpandedProperty);
		}

		private bool ValidTarget(string category)
		{
			return _project.BuildTargets.Contains(category);
		}

		/// <summary>
		/// Add the listeners specified in the command line arguments,
		/// along with the default listener, to the specified project.
		/// </summary>
		private void AddBuildListeners()
		{
			Assert.NotNull(_project, "project");
			BuildListenerCollection listeners = new BuildListenerCollection();
			IBuildLogger buildLogger = new GuiLogger(_messageLogger);

			// set threshold of build logger equal to threshold of project
			buildLogger.Threshold = _project.Threshold;

			// add build logger to listeners collection
			listeners.Add(buildLogger);

			// attach listeners to project
			_project.AttachBuildListeners(listeners);
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

		public void Run()
		{
			try
			{
				_project.BuildFinished += new BuildEventHandler(this.Build_Finished);

				if (_options.TargetFramework != null) 
				{
					this.SetTargetFramework();
				}

//				ArrayList targets = _nantForm.GetTreeTargets();
//				Hashtable properties = _nantForm.GetProjectProperties();
				this.AddBuildListeners();

				_project.Run();
			}
			catch (BuildException error)
			{
				_messageLogger.LogMessage(error.Message);
			}
		}

		private void SetTargetFramework()
		{
			if (_project.Frameworks.Count == 0) 
			{
				const string message = "There are no supported frameworks available on your system.";
				throw new ApplicationException(message);
			} 
			else
			{
				FrameworkInfo frameworkInfo = _project.Frameworks[_options.TargetFramework];

				if (frameworkInfo != null) 
				{
					_project.TargetFramework = frameworkInfo; 
				} 
				else 
				{
					const string format = "Invalid framework '{0}' specified.";
					string message = string.Format(format, _options.TargetFramework);
					throw new CommandLineArgumentException(message);
				}
			}
		}

		private void Build_Finished(object sender, BuildEventArgs e)
		{
			if (this.BuildFinished != null)
			{
				this.BuildFinished();
			}
		}

		#region Properties

		public string DefaultTarget
		{
			get { return _project.DefaultTargetName; }
		}

		public string Name
		{
			get { return _project.ProjectName; }
		}

		public TargetCollection Targets
		{
			get { return _targets; }
		}

		public PropertyCollection Properties
		{
			get { return _properties; }
		}

		public DependsCollection Depends
		{
			get { return _depends; }
		}

		public string Description
		{
			get { return _description; }
		}

		public bool HasName
		{
			get { return _project.ProjectName.Length > 0; }
		}

		#endregion
	}
}