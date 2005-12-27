using System.Drawing;
using Crownwood.Magic.Controls;
using ICSharpCode.TextEditor.Document;
using NAntGui.Framework;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for ScriptTabPage.
	/// </summary>
	public class ScriptTabPage
	{
		public event VoidVoid SourceChanged;

		private TabPage _scriptTab			= new TabPage();
		private ScriptEditor _sourceEditor	= new ScriptEditor();
		private SourceFile _file;
		private IBuildRunner _buildRunner;

		public ScriptTabPage(ILogsMessage logger, CommandLineOptions options)
		{
			_file = new SourceFile(logger, options);
			this.Initialize();
		}

		public ScriptTabPage(string filename, ILogsMessage logger, CommandLineOptions options)
		{
			this.Load(filename, logger, options);
			this.Initialize();
		}

        private void Initialize()
		{
			_sourceEditor.Document.DocumentChanged += new DocumentEventHandler(this.Editor_TextChanged);

			_scriptTab.Controls.Add(_sourceEditor);
			_scriptTab.Location = new Point(0, 0);
			_scriptTab.Selected = true;
			_scriptTab.Size = new Size(824, 453);
			_scriptTab.Title = _file.Name;
		}


		private void Load(string filename, ILogsMessage logger, CommandLineOptions options)
		{
			Assert.FileExists(filename);
			_sourceEditor.LoadFile(filename);
			_file = new SourceFile(filename, _sourceEditor.Text, logger, options);
			_buildRunner = BuildRunnerFactory.Create(_file);
			_scriptTab.Title = _file.Name;
		}

		private void Editor_TextChanged(object sender, DocumentEventArgs e)
		{
			if (this.IsDirty && !Utils.HasAsterisk(_scriptTab.Title))
			{
				_scriptTab.Title = Utils.AddAsterisk(_scriptTab.Title);
			}
			else if (!this.IsDirty && Utils.HasAsterisk(_scriptTab.Title))
			{
				_scriptTab.Title = Utils.RemoveAsterisk(_scriptTab.Title);
			}

			if (this.SourceChanged != null)
			{
				this.SourceChanged();
			}
		}

		public TabPage ScriptTab
		{
			get { return _scriptTab; }
		}

		public SourceFile File
		{
			get { return _file; }
		}

		public void ReLoad()
		{
			_sourceEditor.LoadFile(_file.FullName);
			_scriptTab.Title = _file.Name;
		}

		public void SaveAs(string fileName)
		{
			_sourceEditor.SaveFile(fileName);
			_file = new SourceFile(fileName, _sourceEditor.Text, 
				_file.MessageLogger, _file.Options);
		}

		public void Save()
		{
			_sourceEditor.SaveFile(_file.FullName);
		}

		public bool IsDirty
		{
			get{ return _file.Contents != _sourceEditor.Text; }
		}

		public void Undo()
		{
			_sourceEditor.Undo();
		}

		public void Redo()
		{
			_sourceEditor.Redo();
		}

		public void Stop()
		{
			_buildRunner.Stop();
		}

		public void Run()
		{
			_buildRunner.Run();
		}

		public IBuildScript BuildScript
		{
			get { return _buildRunner.BuildScript; }
		}

		public VoidVoid BuildFinished
		{
			set { _buildRunner.BuildFinished = value; }
		}

		public string Title
		{
			get { return _scriptTab.Title; }
		}

		public void SetProperties(PropertyCollection properties)
		{
			_buildRunner.SetProperties(properties);
		}

		public void SetTargets(TargetCollection targets)
		{
			_buildRunner.SetTargets(targets);
		}
	}
}
