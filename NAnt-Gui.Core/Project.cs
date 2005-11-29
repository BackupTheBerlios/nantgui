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

using System.IO;
using System.Xml;
using log4net;
using NAnt.Core;
using NAnt.Core.Tasks;
using NAnt.Win32.Tasks;

namespace NAntGui.Core
{
	/// <summary>
	/// Contains the logic for parsing the build file.
	/// </summary>
	public class Project
	{
		private static readonly ILog Logger = LogManager.GetLogger("NAnt");

		private string _description = "";

		private TargetCollection _targets = new TargetCollection();
		private PropertyCollection _properties = new PropertyCollection();
		private DependsCollection _depends = new DependsCollection();
		private NAnt.Core.Project _project;

		/// <summary>
		/// Create a new project parser.
		/// </summary>
		/// <param name="project">NAnt Project to add information to</param>
		public Project(NAnt.Core.Project project)
		{
			_project = project;

			this.AddTargets(_project.Document);
			this.AddDescription();
			this.AddBaseDir();
			this.AddProperties(_project.Document);
			this.AddNonPropertyProperties(_project);
			this.FollowIncludes();

			_targets.Sort();
		}

		private void AddTargets(XmlDocument doc)
		{
			foreach (XmlElement element in doc.GetElementsByTagName("target"))
			{
				Target target = new Target(element);
				if (target.Name == this.DefaultTarget) target.Default = true;
				_targets.Add(target);
				_depends.Add(target.Depends);
			}
		}

		private void FollowIncludes()
		{
			foreach (XmlElement element in _project.Document.GetElementsByTagName("include"))
			{
				string buildFile = element.GetAttribute("buildfile");
				string filename = _project.ExpandProperties(buildFile, new Location("Buildfile"));
				if (File.Exists(filename))
				{
					try
					{
						XmlDocument document = new XmlDocument();
						document.Load(filename);
						this.AddTargets(document);
						this.AddProperties(document);
						this.AddNonPropertyProperties(this._project);
					}
					catch (IOException error)
					{
						Logger.Error("", error);
					}
				}
			}
		}

		private void AddBaseDir()
		{
			Property property = new Property("BaseDir", this._project.BaseDirectory, "Project", false);
			_project.Properties.AddReadOnly(property.Name, this._project.BaseDirectory);
			_properties.Add(property);
		}

		private void AddProperties(XmlDocument doc)
		{
			foreach (XmlElement element in doc.GetElementsByTagName("property"))
			{
				Property property = new Property(element);
				try
				{
					property.ExpandedValue = _project.ExpandProperties(property.Value, new Location("Buildfile"));
				}
				catch (BuildException)
				{ /* ignore */
				}

				if (!_project.Properties.Contains(property.Name))
				{
					_project.Properties.AddReadOnly(property.Name, property.ExpandedValue);
					_properties.Add(property);
				}
			}
		}

		private void AddNonPropertyProperties(NAnt.Core.Project pProject)
		{
			this.AddTstamps(pProject);
			this.AddReadRegistrys(pProject);
		}

		private void AddTstamps(NAnt.Core.Project pProject)
		{
			foreach (XmlElement lElement in pProject.Document.GetElementsByTagName("tstamp"))
			{
				if (TypeFactory.TaskBuilders.Contains(lElement.Name))
				{
					TStampTask task = (TStampTask) pProject.CreateTask(lElement);
					if (task != null)
					{
						task.Execute();

						Property lProperty = new Property(task.Property, task.Properties[task.Property], lElement.ParentNode.Attributes["name"].Value, false);
						lProperty.ExpandedValue = lProperty.Value;
						_properties.Add(lProperty);
					}
				}
			}
		}

		private void AddReadRegistrys(NAnt.Core.Project pProject)
		{
			foreach (XmlElement lElement in pProject.Document.GetElementsByTagName("readregistry"))
			{
				if (TypeFactory.TaskBuilders.Contains(lElement.Name))
				{
					ReadRegistryTask task = (ReadRegistryTask) pProject.CreateTask(lElement);
					if (task != null && task.PropertyName != null)
					{
						task.Execute();

						Property lProperty = new Property(task.PropertyName, task.Properties[task.PropertyName], lElement.ParentNode.Attributes["name"].Value, false);
						lProperty.ExpandedValue = lProperty.Value;
						_properties.Add(lProperty);
					}
				}
			}
		}

		private void AddDescription()
		{
			foreach (XmlElement lElement in _project.Document.GetElementsByTagName("description"))
			{
				_description = lElement.InnerText;
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

		#endregion
	}

}