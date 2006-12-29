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

using NAnt.Core;
using NAnt.Core.Tasks;
using NAnt.Win32.Tasks;
using NAntGui.Framework;

using TargetCollection = NAntGui.Framework.TargetCollection;

namespace NAntGui.NAnt
{
	/// <summary>
	/// Contains the logic for parsing the build file.
	/// </summary>
	public class NAntBuildScript : IBuildScript
	{
//		private Project _project;

		private string _filePath;
		private string _fileName;
		private string _description = "";

		private TargetCollection _targets = new TargetCollection();
		private DependsCollection _depends = new DependsCollection();
		private PropertyCollection _properties = new PropertyCollection();
		private string _defaultTargetName;
		private string _name;

		/// <summary>
		/// Create a new project parser.
		/// </summary>
		public NAntBuildScript(string filePath, string fileName)
		{
			Assert.NotNull(filePath, "filePath");
			Assert.NotNull(fileName, "fileName");

			_filePath = filePath;
			_fileName = fileName;
		}

		public void Parse()
		{
			Project project = CreateProject();
			XmlDocument doc;

			if (project == null)
			{
				doc = new XmlDocument();
				doc.Load(_filePath);
			}
			else
			{
				doc = project.Document;
			}
			
			ParseDescription(doc);
			ParseDefaultTarget(doc);
			_targets.Clear();
			_depends.Clear();
			ParseTargetsAndDependencies(doc);
			ParseProperties(project, doc);
			ParseNonPropertyProperties(doc);
			FollowIncludes(project);
			ParseBaseDir(project);
			_defaultTargetName = project.DefaultTargetName;
			_name = HasName(project) ? project.ProjectName : _fileName;
		}

		private Project CreateProject()
		{
			try
			{
				return new Project(_filePath, Level.Info, 0);
			}
			catch(ArgumentException)
			{
				return null;
			}
			catch (Exception error)
			{
				if (error.InnerException == null)
				{
					throw new BuildFileLoadException(error.Message);
				}
				else
				{
					throw new BuildFileLoadException(error.InnerException.Message);
				}
			}
		}

		private void ParseDefaultTarget(XmlDocument doc)
		{
			// there should only be one, but in order for the parser to 
			// be fault tolerent, it must be a little lazy
			foreach (XmlElement element in doc.GetElementsByTagName("project"))
			{
				_defaultTargetName = element.Attributes["default"].Value;
				break;
			}
		}

		private void ParseDescription(XmlDocument doc)
		{
			foreach (XmlElement element in doc.GetElementsByTagName("description"))
			{
				_description += element.InnerText + " ";
			}
		}

		private void ParseTargetsAndDependencies(XmlDocument doc)
		{			
			foreach (XmlElement element in doc.GetElementsByTagName("target"))
			{
				NAntTarget nantTarget = new NAntTarget(element);
				if (nantTarget.Name == _defaultTargetName) nantTarget.Default = true;
				_targets.Add(nantTarget);
				_depends.Add(nantTarget.Depends);
			}

			_targets.Sort();
		}

		private void FollowIncludes(Project project)
		{
			foreach (XmlElement element in project.Document.GetElementsByTagName("include"))
			{
				string buildFile = element.GetAttribute("buildfile");
				string filename = project.ExpandProperties(buildFile, new Location("Buildfile"));

				if (File.Exists(filename))
				{
					ParseIncludeFile(project, filename);
				}
			}
		}

		private void ParseIncludeFile(Project project, string filename)
		{
			XmlDocument document = new XmlDocument();
			document.Load(filename);
			ParseTargetsAndDependencies(document);
			ParseProperties(project, document);
			ParseNonPropertyProperties(document);
		}

		private void ParseBaseDir(Project project)
		{
			NAntProperty nAntProperty = new NAntProperty("BaseDir", project.BaseDirectory, "Project", false);
			project.Properties.AddReadOnly(nAntProperty.Name, project.BaseDirectory);
			_properties.Add(nAntProperty);
		}

		private void ParseProperties(Project project, XmlDocument doc)
		{
			foreach (XmlElement element in doc.GetElementsByTagName("property"))
			{
				NAntProperty nantProperty = new NAntProperty(element);
				try
				{
					nantProperty.ExpandedValue = project.ExpandProperties(nantProperty.Value, 
						new Location("Buildfile"));
				}
				catch (BuildException)
				{
					/* ignore */
				}

				if (!project.Properties.Contains(nantProperty.Name))
				{
					project.Properties.AddReadOnly(nantProperty.Name, nantProperty.ExpandedValue);
					_properties.Add(nantProperty);
				}
			}
		}

		private void ParseNonPropertyProperties(XmlDocument doc)
		{
			ParseTstamps(doc);
			AddReadRegistrys(doc);
			//AddRegex(project);
		}

		private void ParseTstamps(XmlDocument doc)
		{
			foreach (XmlElement element in doc.GetElementsByTagName("tstamp"))
			{
				if (TypeFactory.TaskBuilders.Contains(element.Name))
				{
					TStampTask task = new TStampTask();
					task.Initialize(element);

					if (task != null)
					{
						task.Execute();

						NAntProperty nantProperty = new NAntProperty(
							task.Property, task.Properties[task.Property], 
							element.ParentNode.Attributes["name"].Value,
							true);

						nantProperty.ExpandedValue = nantProperty.Value;
						_properties.Add(nantProperty);
					}
				}
			}
		}

		private void AddReadRegistrys(XmlDocument doc)
		{
			foreach (XmlElement element in doc.GetElementsByTagName("readregistry"))
			{
				if (TypeFactory.TaskBuilders.Contains(element.Name))
				{
					//ReadRegistryTask task = (ReadRegistryTask) project.CreateTask(element);
					ReadRegistryTask task = new ReadRegistryTask();
					task.Initialize(element);

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

		private bool HasName(Project project)
		{
			return project != null && project.ProjectName.Length > 0;
		}

		#region Properties

		public string DefaultTarget
		{
			get { return _defaultTargetName; }
		}

		public string Name
		{
			get { return _name; }
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
