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
			ParseTargetsAndDependencies();
			ParseProperties();
			ParseNonPropertyProperties();
			FollowIncludes();
			ParseBaseDir();
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
							                 false);
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

						NAntProperty nAntProperty = new NAntProperty(
							task.PropertyName, task.Properties[task.PropertyName],
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