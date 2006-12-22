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
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

using NAnt.Core;
using NAnt.Core.Util;
using NAnt.Core.Tasks;
using NAnt.Win32.Tasks;
using NAntGui.Framework;

using TargetCollection = NAntGui.Framework.TargetCollection;
//using CmdOptions = NAntGui.Framework.CommandLineOptions;

namespace NAntGui.NAnt
{
	/// <summary>
	/// Contains the logic for parsing the build file.
	/// </summary>
	public class NAntBuildScript : IBuildScript
	{
		private Project _project;

		private string _filePath;
		private string _fileName;
		private string _description = "";
		//private CmdOptions _options;

		private TargetCollection _targets = new TargetCollection();
		private DependsCollection _depends = new DependsCollection();
		private PropertyCollection _properties = new PropertyCollection();

		/// <summary>
		/// Create a new project parser.
		/// </summary>
		public NAntBuildScript(string filePath, string fileName)
		{
			Assert.NotNull(filePath, "filePath");
			Assert.NotNull(fileName, "fileName");
			//Assert.NotNull(options, "options");

			_filePath = filePath;
			_fileName = fileName;
			//_options = options;
		}

		public void Parse()
		{
			_project = new Project(_filePath, Level.Info, 0);

			ParseDescription();
			ParseTargetsAndDependencies();
			ParseProperties();
			ParseNonPropertyProperties();
			FollowIncludes();
			ParseBaseDir();
		}

//		private Level GetThreshold()
//		{
//			Level projectThreshold = Level.Info;
//			// determine the project message threshold
//			if (_options.Debug)
//			{
//				projectThreshold = Level.Debug;
//			}
//			else if (_options.Verbose)
//			{
//				projectThreshold = Level.Verbose;
//			}
//			else if (_options.Quiet)
//			{
//				projectThreshold = Level.Warning;
//			}
//			return projectThreshold;
//		}

		private void ParseDescription()
		{
			foreach (XmlElement element in _project.Document.GetElementsByTagName("description"))
			{
				_description += element.InnerText + " ";
			}
		}

		private void ParseTargetsAndDependencies()
		{
			ParseTargetsAndDependencies(_project.Document);
		}

		private void ParseTargetsAndDependencies(XmlDocument doc)
		{
			foreach (XmlElement element in doc.GetElementsByTagName("target"))
			{
				NAntTarget nantTarget = new NAntTarget(element);
				if (nantTarget.Name == _project.DefaultTargetName) nantTarget.Default = true;
				_targets.Add(nantTarget);
				_depends.Add(nantTarget.Depends);
			}

			_targets.Sort();
		}

		private void FollowIncludes()
		{
			foreach (XmlElement element in _project.Document.GetElementsByTagName("include"))
			{
				string buildFile = element.GetAttribute("buildfile");
				string filename = _project.ExpandProperties(buildFile, new Location("Buildfile"));

				if (File.Exists(filename))
				{
					ParseIncludeFile(filename);
				}
			}
		}

		private void ParseIncludeFile(string filename)
		{
			XmlDocument document = new XmlDocument();
			document.Load(filename);
			ParseTargetsAndDependencies(document);
			ParseProperties(document);
			ParseNonPropertyProperties(_project);
		}

		private void ParseBaseDir()
		{
			NAntProperty nAntProperty = new NAntProperty("BaseDir", _project.BaseDirectory, "Project", false);
			_project.Properties.AddReadOnly(nAntProperty.Name, _project.BaseDirectory);
			_properties.Add(nAntProperty);
		}

		private void ParseProperties()
		{
			ParseProperties(_project.Document);
		}

		private void ParseProperties(XmlDocument doc)
		{
			foreach (XmlElement element in doc.GetElementsByTagName("property"))
			{
				NAntProperty nantProperty = new NAntProperty(element);
				try
				{
					nantProperty.ExpandedValue = _project.ExpandProperties(nantProperty.Value, new Location("Buildfile"));
				}
				catch (BuildException)
				{
					/* ignore */
				}

				if (!_project.Properties.Contains(nantProperty.Name))
				{
					_project.Properties.AddReadOnly(nantProperty.Name, nantProperty.ExpandedValue);
					_properties.Add(nantProperty);
				}
			}
		}

		private void ParseNonPropertyProperties()
		{
			ParseNonPropertyProperties(_project);
		}

		private void ParseNonPropertyProperties(Project project)
		{
			ParseTstamps(project);
			AddReadRegistrys(project);
			//			AddRegex(project);
		}

		private void ParseTstamps(Project project)
		{
			foreach (XmlElement lElement in project.Document.GetElementsByTagName("tstamp"))
			{
				if (TypeFactory.TaskBuilders.Contains(lElement.Name))
				{
					TStampTask task = (TStampTask) project.CreateTask(lElement);
					if (task != null)
					{
						task.Execute();

						NAntProperty lNAntProperty =
							new NAntProperty(task.Property, task.Properties[task.Property], lElement.ParentNode.Attributes["name"].Value,
							true);
						lNAntProperty.ExpandedValue = lNAntProperty.Value;
						_properties.Add(lNAntProperty);
					}
				}
			}
		}

		private void AddReadRegistrys(Project project)
		{
			foreach (XmlElement element in project.Document.GetElementsByTagName("readregistry"))
			{
				if (TypeFactory.TaskBuilders.Contains(element.Name))
				{
					ReadRegistryTask task = (ReadRegistryTask) project.CreateTask(element);
					if (task != null && task.PropertyName != null)
					{
						task.Execute();
						
						object val = task.Properties[task.PropertyName];
						string value = val == null ? "" : val.ToString();
						
						NAntProperty nAntProperty = new NAntProperty(
							task.PropertyName, value,
							element.ParentNode.Attributes["name"].Value, false);

						nAntProperty.ExpandedValue = nAntProperty.Value;
						_properties.Add(nAntProperty);
					}
				}
			}
		}

		/*
				/// <summary>
				/// Not in use right now because it opens a can of worms.
				/// The only way to know what value the input attribute has 
				/// is to run the program.  
				/// </summary>
				/// <param name="project"></param>
				private void AddRegex(Project project)
				{
					foreach (XmlElement element in project.Document.GetElementsByTagName("regex"))
					{
						if (TypeFactory.TaskBuilders.Contains(element.Name))
						{
							ReadRegistryTask task = (ReadRegistryTask) project.CreateTask(element);
							if (task != null && task.PropertyName != null)
							{
								task.Execute();

								NAntProperty nAntProperty = new NAntProperty(
									task.PropertyName, task.Properties[task.PropertyName], 
									element.ParentNode.Attributes["name"].Value, false);

								nAntProperty.ExpandedValue = nAntProperty.Value;
								_properties.Add(nAntProperty);
							}
						}
					}
				}
		*/

		private bool HasName()
		{
			return _project != null && _project.ProjectName.Length > 0;
		}

		#region Properties

		public string DefaultTarget
		{
			get { return _project.DefaultTargetName; }
		}

		public string Name
		{
			get { return HasName() ? _project.ProjectName : _fileName; }
		}

		public string Description
		{
			get { return _description; }
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

		#endregion
	}
}
