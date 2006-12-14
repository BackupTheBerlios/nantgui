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

using System;
using System.Text.RegularExpressions;
using NAnt.Core;
using NAnt.Core.Util;
using NAntGui.Framework;
using TargetCollection = NAntGui.Framework.TargetCollection;

namespace NAntGui.NAnt
{
	/// <summary>
	/// Contains the logic for parsing the build file.
	/// </summary>
	public class NAntBuildScript : IBuildScript
	{
		public event VoidVoid BuildFinished;

		private string _description = "";
		private Project _project;
		private SourceFile _sourceFile;

		private TargetCollection _targets = new TargetCollection();
		private PropertyCollection _properties = new PropertyCollection();
		private DependsCollection _depends = new DependsCollection();

		/// <summary>
		/// Create a new project parser.
		/// </summary>
		public NAntBuildScript(SourceFile sourceFile)
		{
			Assert.NotNull(sourceFile, "sourceFile");
			_sourceFile = sourceFile;
		}

		public void ParseBuildScript()
		{
			_project = new Project(_sourceFile.FullName, GetThreshold(), 0);
			ScriptParser parser = new ScriptParser(_project);
			parser.Parse();

			_description = parser.Description;
			_targets = parser.Targets;
			_depends = parser.Depends;
			_properties = parser.Properties;
		}


		private void AddProperties()
		{
			foreach (BuildProperty property in _properties)
			{
				if (property.Category == "Project")
				{
					_project.BaseDirectory = property.Value;
				}
				else if (property.Category == "Global" || ValidTarget(property.Category))
				{
					LoadNonProjectProperty(property);
				}
			}
		}

		private void AddTargets()
		{
			foreach (BuildTarget target in _targets)
			{
				_project.BuildTargets.Add(target.Name);
			}
		}

		public void Run()
		{
			try
			{
				_project = new Project(_sourceFile.FullName, GetThreshold(), 0);
				_project.BuildFinished += new BuildEventHandler(Build_Finished);
				SetTargetFramework();
				AddBuildListeners();
				AddTargets();
				AddProperties();

				_project.Run();
			}
			catch (BuildException error)
			{
				_sourceFile.MessageLogger.LogMessage(error.Message);
				FinishBuild();
			}
		}

		private void LoadNonProjectProperty(BuildProperty property)
		{
			string expandedValue = property.Value;

			try
			{
				expandedValue = _project.ExpandProperties(property.Value,
				                                          new Location(_sourceFile.FullName));
			}
			catch (BuildException)
			{
				/* ignore */
			}

			// If the expanded value doesn't have any "unexpanded" values
			// add it to the project.
			Regex regex = new Regex(@"\$\{.+\}");
			if (!regex.IsMatch(expandedValue))
			{
				//_project.Properties.AddReadOnly(property.Name, expandedValue);
				_project.Properties.Add(property.Name, expandedValue);
			}
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
			IBuildLogger buildLogger = new GuiLogger(_sourceFile.MessageLogger);

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
			if (_sourceFile.Options.Debug)
			{
				projectThreshold = Level.Debug;
			}
			else if (_sourceFile.Options.Verbose)
			{
				projectThreshold = Level.Verbose;
			}
			else if (_sourceFile.Options.Quiet)
			{
				projectThreshold = Level.Warning;
			}
			return projectThreshold;
		}

		private void SetTargetFramework()
		{
			if (_sourceFile.Options.TargetFramework != null)
			{
				if (_project.Frameworks.Count == 0)
				{
					const string message = "There are no supported frameworks available on your system.";
					throw new ApplicationException(message);
				}
				else
				{
					FrameworkInfo frameworkInfo = _project.Frameworks[_sourceFile.Options.TargetFramework];

					if (frameworkInfo != null)
					{
						_project.TargetFramework = frameworkInfo;
					}
					else
					{
						const string format = "Invalid framework '{0}' specified.";
						string message = string.Format(format, _sourceFile.Options.TargetFramework);
						throw new CommandLineArgumentException(message);
					}
				}
			}
		}

		private void Build_Finished(object sender, BuildEventArgs e)
		{
			FinishBuild();
		}

		private void FinishBuild()
		{
			if (BuildFinished != null)
			{
				BuildFinished();
			}
		}


		#region Properties

		public SourceFile SourceFile
		{
			get { return _sourceFile; }
		}

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
			set { _targets = value; }
		}

		public PropertyCollection Properties
		{
			get { return _properties; }
			set { _properties = value; }
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
			get { return _project != null && _project.ProjectName.Length > 0; }
		}

		#endregion
	}
}
