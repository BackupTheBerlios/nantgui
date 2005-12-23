using System;
using System.Drawing;
using Crownwood.Magic.Controls;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for ScriptTabPage.
	/// </summary>
	public class ScriptTabPage
	{
		private TabPage _scriptTab			= new TabPage();
		private ScriptEditor _sourceEditor	= new ScriptEditor();
		private SourceFile _file;

		public ScriptTabPage()
		{
			_file = new SourceFile();
			this.Initialize();
		}

		public ScriptTabPage(string filename)
		{
			this.Load(filename);
			this.Initialize();
		}

        private void Initialize()
		{
			// 
			// ScriptTabPage
			// 
			_scriptTab.Controls.Add(_sourceEditor);
			_scriptTab.Location = new Point(0, 0);
			_scriptTab.Selected = true;
			_scriptTab.Size = new Size(824, 453);
			_scriptTab.Title = _file.Name;
		}


		private void Load(string filename)
		{
			Assert.FileExists(filename);
			_sourceEditor.LoadFile(filename);
			_file = new SourceFile(filename, _sourceEditor.Text);
			_sourceEditor.TextChanged += new EventHandler(this.Editor_TextChanged);
			_scriptTab.Title = _file.Name;
		}

		private void Editor_TextChanged(object sender, EventArgs e)
		{
			if (this.IsDirty && !Utils.HasAsterisk(_scriptTab.Title))
			{
				_scriptTab.Title = Utils.AddAsterisk(_scriptTab.Title);
			}
			else if (!this.IsDirty && Utils.HasAsterisk(_scriptTab.Title))
			{
				_scriptTab.Title = Utils.RemoveAsterisk(_scriptTab.Title);
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
			_sourceEditor.LoadFile(_file.FullPath);
		}

		public void SaveAs(string fileName)
		{
			_sourceEditor.SaveFile(fileName);
			_file = new SourceFile(fileName, _sourceEditor.Text);
		}

		public void Save()
		{
			_sourceEditor.SaveFile(_file.FullPath);
		}

		public bool IsDirty
		{
			get{ return _file.Contents != _sourceEditor.Text; }
		}
	}
}
