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

using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Collections;
using ICSharpCode.TextEditor.Document;
using NAntGui.Framework;
using TabPage = Crownwood.Magic.Controls.TabPage;
using TargetCollection = NAntGui.Framework.TargetCollection;

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
		private IBuildRunner _buildRunner;
		private MainFormMediator _mediator;

		public ScriptTabPage(ILogsMessage logger)
		{
			Assert.NotNull(logger, "logger");
			_file = new SourceFile(logger);
			this.Initialize();
		}

		public ScriptTabPage(string filename, ILogsMessage logger, CommandLineOptions options)
		{
			Assert.NotNull(filename, "filename");
			Assert.NotNull(logger, "logger");
			Assert.NotNull(options, "options");

			_sourceEditor.LoadFile(filename);
			_file = new SourceFile(filename, _sourceEditor.Text, logger, options);

			this.Initialize();

			_buildRunner = BuildRunnerFactory.Create(_file);
		}

		public void ParseBuildScript()
		{
			_buildRunner.ParseBuildScript();
		}

		public MainFormMediator Mediator
		{
			set
			{
				Assert.NotNull(value, "value");
				_mediator = value;
				_sourceEditor.Mediator = value;
			}
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

			Assert.NotNull(_mediator, "_mediator");
			_mediator.UpdateDisplay();
		}

		public void ReLoad()
		{
			_sourceEditor.LoadFile(_file.FullName);
			_scriptTab.Title = _file.Name;
			try{ _buildRunner.ParseBuildScript(); }
			catch { /* ignore */ }
		}

		public void SaveAs(string fileName)
		{
			_sourceEditor.SaveFile(fileName);
			_file = new SourceFile(fileName, _sourceEditor.Text, 
				_file.MessageLogger, _file.Options);

			this.ReInitialize();
		}

		public void Save()
		{
			_sourceEditor.SaveFile(_file.FullName);
			_file.Contents = _sourceEditor.Text;
			this.ReInitialize();
		}

		private void ReInitialize()
		{
			_scriptTab.Title = Utils.RemoveAsterisk(_scriptTab.Title);
	
			try{ _buildRunner.ParseBuildScript(); }
			catch { /* ignore */ }
	
			Assert.NotNull(_mediator, "_mediator");
			_mediator.UpdateDisplay();
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
			if (_buildRunner != null)
			{
				_buildRunner.Stop();
			}
		}

		public void Run()
		{
			Assert.NotNull(_buildRunner, "_buildRunner");
			_buildRunner.Run();
		}

		public void SetProperties(PropertyCollection properties)
		{
			Assert.NotNull(properties, "properties");
			_buildRunner.SetProperties(properties);
		}

		public void SetTargets(TargetCollection targets)
		{
			Assert.NotNull(targets, "targets");
			_buildRunner.SetTargets(targets);
		}

		public void CloseFile()
		{
			_file.Close();
		}

		public void AddTabToControl(TabPageCollection tabPages)
		{
			tabPages.Add(_scriptTab);
		}

		public bool Equals(TabPage page)
		{
			return page == _scriptTab;
		}

		public IBuildScript BuildScript
		{
			get
			{
				Assert.NotNull(_buildRunner, "_buildRunner");
				return _buildRunner.BuildScript;
			}
		}

		public VoidVoid BuildFinished
		{
			set
			{
				Assert.NotNull(_buildRunner, "_buildRunner");
				_buildRunner.BuildFinished = value;
			}
		}

		public DragEventHandler DragDrop
		{
			set 
			{
				_sourceEditor.DragDrop += value;
			}
		}

		public DragEventHandler DragEnter
		{
			set
			{
				_sourceEditor.DragEnter += value;
			}
		}

		public string FileName
		{
			get { return _file.Name; }
		}

		public string FileFullName
		{
			get { return _file.FullName; }
		}

		public string FilePath
		{
			get { return _file.Path; }
		}

		public string Title
		{
			get { return _scriptTab.Title; }
		}

		public bool IsDirty
		{
			get{ return _file.Contents != _sourceEditor.Text; }
		}
	}
}
