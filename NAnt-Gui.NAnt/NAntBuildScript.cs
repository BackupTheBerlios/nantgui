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
		private TargetCollection _targets = new TargetCollection();
		private DependsCollection _depends = new DependsCollection();
		private PropertyCollection _properties = new PropertyCollection();
		
		private string _name;
		private FileInfo _file;
		private string _baseDir;
		private string _description = "";
		private string _defaultTargetName;

		/// <summary>
		/// Create a new project parser.
		/// </summary>
		public NAntBuildScript(string file)
		{
			Assert.NotNull(file, "file");

			_file = new FileInfo(file);
		}

		public void Parse()
		{
			XmlDocument doc = CreateXmlDoc();
			Project project = CreateProject(doc);
			
			_targets.Clear();
			_depends.Clear();
			_properties.Clear();

			ParseName(doc);
			ParseBaseDir(doc);
			ParseDefaultTarget(doc);		
			ParseDescription(doc);
			ParseTargetsAndDependencies(doc);
			ParseProperties(project, doc);
			ParseNonPropertyProperties(doc);
			FollowIncludes(project, doc);		
		}

		private Project CreateProject(XmlDocument doc)
		{
			try
			{
				return new Project(doc, Level.Info, 0);
			}
			catch(ArgumentException)
			{
				return null;
			}
			catch (Exception error)
			{
				throw new BuildFileLoadException("Error parsing NAnt project", error);
			}
		}

		private XmlDocument CreateXmlDoc()
		{
			XmlDocument doc = new XmlDocument();

			try
			{
				doc.Load(_file.FullName);
			}
			catch(XmlException error)
			{
				throw new BuildFileLoadException("Error parsing NAnt file", error);
			}
				
			return doc;
		}

		private void ParseName(XmlDocument doc)
		{
			_name = _file.Name;

			// there should only be one, but in order for the parser to 
			// be fault tolerent, it must be a little lazy
			foreach (XmlElement element in doc.GetElementsByTagName("project"))
			{
				if (element.HasAttribute("name"))
				{
					_name = element.GetAttribute("name");
					break;
				}
			}
		}

		private void ParseDefaultTarget(XmlDocument doc)
		{
			// there should only be one, but in order for the parser to 
			// be fault tolerent, it must be a little lazy
			foreach (XmlElement element in doc.GetElementsByTagName("project"))
			{
				if (element.HasAttribute("default"))
				{
					_defaultTargetName = element.GetAttribute("default");
					break;
				}
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

		private void FollowIncludes(Project project, XmlDocument doc)
		{
			foreach (XmlElement element in doc.GetElementsByTagName("include"))
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
			string fullName = Path.Combine(_baseDir, filename);

			try
			{
				document.Load(fullName);
			}
			finally
			{
				ParseTargetsAndDependencies(document);
				ParseProperties(project, document);
				ParseNonPropertyProperties(document);
			}
		}

		private void ParseBaseDir(XmlDocument doc)
		{
			_baseDir = _file.DirectoryName;

			// there should only be one, but in order for the parser to 
			// be fault tolerent, it must be a little lazy
			foreach (XmlElement element in doc.GetElementsByTagName("project"))
			{
				if (element.HasAttribute("basedir"))
				{
					_baseDir = element.GetAttribute("basedir");
					break;
				}
			}

			NAntProperty prop = new NAntProperty("BaseDir", _baseDir, "Project", false);

			//project.Properties.AddReadOnly(prop.Name, project.BaseDirectory);
			_properties.Add(prop);
		}

		private void ParseProperties(Project project, XmlDocument doc)
		{
			foreach (XmlElement element in doc.GetElementsByTagName("property"))
			{
				NAntProperty nantProperty = new NAntProperty(element);

				if (project != null)
				{
					TryExpandingProperty(project, nantProperty);

					if (!project.Properties.Contains(nantProperty.Name))
					{
						project.Properties.AddReadOnly(
							nantProperty.Name, nantProperty.ExpandedValue);

						_properties.Add(nantProperty);
					}
				}
				else
				{
					_properties.Add(nantProperty);
				}
			}
		}

		private void TryExpandingProperty(Project project, NAntProperty property)
		{
			try
			{
				property.ExpandedValue = project.ExpandProperties(
					property.Value, new Location("Buildfile"));
			}
			catch (BuildException)
			{
				/* ignore */
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
