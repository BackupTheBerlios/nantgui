using System.Drawing;
using Crownwood.Magic.Controls;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for ScriptTabPage.
	/// </summary>
	public class ScriptTabPage
	{
		private TabPage _scriptTab = new TabPage();
		private ScriptEditor _sourceEditor = new ScriptEditor();
		private SourceFile _file = new SourceFile();

		public ScriptTabPage()
		{
			Initialize();
		}

		public ScriptTabPage(string filename)
		{
			Initialize();
			this.Load(filename);
		}

        private void Initialize()
		{
			_file.SourceChanged += new VoidBool(this.Source_Changed);
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
			_file.Load(filename, _sourceEditor, _sourceEditor);
			_scriptTab.Title = _file.Name;
		}

		private void Source_Changed(bool isChanged)
		{
			if (isChanged && !Utils.HasAsterisk(_scriptTab.Title))
			{
				_scriptTab.Title = Utils.AddAsterisk(_scriptTab.Title);
			}
			else if (!isChanged && Utils.HasAsterisk(_scriptTab.Title))
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
	}
}
