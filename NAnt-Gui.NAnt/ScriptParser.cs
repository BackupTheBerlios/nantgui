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
	/// Summary description for ScriptParser.
	/// </summary>
	public class ScriptParser
	{
		Project _project;
		private TargetCollection _targets = new TargetCollection();
		private DependsCollection _depends = new DependsCollection();
		private PropertyCollection _properties = new PropertyCollection();

		public ScriptParser(Project project)
		{
			Assert.NotNull(project, "project");
			_project = project;
		}

		public void Parse()
		{
			this.ParseTargetsAndDependencies();
			this.ParseProperties();
			this.ParseNonPropertyProperties();
			this.FollowIncludes();
			this.ParseBaseDir();
		}

		private void ParseTargetsAndDependencies()
		{
			this.ParseTargetsAndDependencies(_project.Document);
		}

		private void ParseTargetsAndDependencies(XmlDocument doc)
		{
			foreach (XmlElement element in doc.GetElementsByTagName("target"))
			{
				Target target = new Target(element);
				if (target.Name == _project.DefaultTargetName) target.Default = true;
				_targets.Add(target);
				_depends.Add(target.Depends);
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
					try
					{
						this.ParseIncludeFile(filename);
					}
					catch (IOException error)
					{
						// Might not be a not found exception.  SHould fix.
						throw new BuildFileNotFoundException(error.Message);
					}
				}
			}
		}

        private void ParseIncludeFile(string filename)
		{
			XmlDocument document = new XmlDocument();
			document.Load(filename);
			this.ParseTargetsAndDependencies(document);
			this.ParseProperties(document);
			this.ParseNonPropertyProperties(_project);
		}

		private void ParseBaseDir()
		{
			NAntProperty nAntProperty = new NAntProperty("BaseDir", _project.BaseDirectory, "Project", false);
			_project.Properties.AddReadOnly(nAntProperty.Name, _project.BaseDirectory);
			_properties.Add(nAntProperty);
		}

		private void ParseProperties()
		{
			this.ParseProperties(_project.Document);
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
				{ /* ignore */
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
			this.ParseNonPropertyProperties(_project);
		}

		private void ParseNonPropertyProperties(Project project)
		{
			this.ParseTstamps(project);
			this.AddReadRegistrys(project);
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

						NAntProperty lNAntProperty = new NAntProperty(task.Property, task.Properties[task.Property], lElement.ParentNode.Attributes["name"].Value, false);
						lNAntProperty.ExpandedValue = lNAntProperty.Value;
						_properties.Add(lNAntProperty);
					}
				}
			}
		}

		private void AddReadRegistrys(Project project)
		{
			foreach (XmlElement lElement in project.Document.GetElementsByTagName("readregistry"))
			{
				if (TypeFactory.TaskBuilders.Contains(lElement.Name))
				{
					ReadRegistryTask task = (ReadRegistryTask) project.CreateTask(lElement);
					if (task != null && task.PropertyName != null)
					{
						task.Execute();

						NAntProperty lNAntProperty = new NAntProperty(task.PropertyName, task.Properties[task.PropertyName], lElement.ParentNode.Attributes["name"].Value, false);
						lNAntProperty.ExpandedValue = lNAntProperty.Value;
						_properties.Add(lNAntProperty);
					}
				}
			}
		}

		#region Properties

		public string Description
		{
			get
			{
				string description = "";

				foreach (XmlElement element in _project.Document.GetElementsByTagName("description"))
				{
					description = element.InnerText;
				}

				return description;
			}
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
