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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using NAntGui.Gui.Controls;
using NAntGui.Framework;
using NAntGui.Core;
using TabPage = Crownwood.Magic.Controls.TabPage;
using TargetCollection = NAntGui.Framework.TargetCollection;

namespace NAntGui.Gui
{
	/// <summary>
	/// This class needs to be separated.  Currently there is a mingling between
	/// the GUI TabPage/ScriptEditor, and the non-GUI SourceFile/BuildRunner.
	/// This problem is sufficiently difficult do to the fact that the ScriptEditor
	/// does the actual file IO.
	/// </summary>
	public class ScriptTabPage : IEditCommands
	{
		private TabPage _scriptTab = new TabPage();
		private ScriptEditor _scriptEditor;
		private SourceFile _file;
		private BuildRunnerBase _buildRunner;
		private IBuildScript _buildScript;
		private MainFormMediator _mediator;
		private ILogsMessage _logger;
		private TextAreaClipboardHandler _clipboardHandler;
		// this might be better moved to the SourceFile class
		private FileType _fileType;

		public ScriptTabPage(ILogsMessage logger, MainFormMediator mediator)
		{
			Assert.NotNull(logger, "logger");
			Assert.NotNull(mediator, "mediator");

			_mediator = mediator;
			_logger = logger;

			_scriptEditor = new ScriptEditor(mediator);
			_scriptEditor.SetHighlighting("XML");

			_file = new SourceFile();
			_fileType = FileType.New;

			Initialize();

			_buildScript = new BlankBuildScript();
		}

		public ScriptTabPage(string filename, ILogsMessage logger, MainFormMediator mediator)
		{
			Assert.NotNull(filename, "filename");
			Assert.NotNull(logger, "logger");
			Assert.NotNull(mediator, "mediator");
			_mediator = mediator;
			_logger = logger;

			_scriptEditor = new ScriptEditor(mediator);
			_scriptEditor.LoadFile(filename);

			_file = new SourceFile(filename, _scriptEditor.Text);
			_fileType = FileType.Existing;

			Initialize();

			_buildRunner = BuildRunnerFactory.Create(_file, logger, _mediator.Options);
			_buildScript = ScriptParserFactory.Create(_file);			

			_buildRunner.Properties = _buildScript.Properties;
		}

		public void ParseBuildScript()
		{
			_buildScript.Parse();
		}

		public void ReLoad()
		{
			_scriptEditor.LoadFile(_file.FullName);
			_scriptTab.Title = _file.Name;

			ParseBuildFile();
			_mediator.UpdateDisplay();
		}

		public void SaveAs(string fileName)
		{
			_scriptEditor.SaveFile(fileName);

			_file				= new SourceFile(fileName, _scriptEditor.Text);
			_buildRunner		= BuildRunnerFactory.Create(_file, _logger, _mediator.Options);
			_fileType			= FileType.Existing;
			_scriptTab.Title	= _file.Name;

			ParseBuildFile();
		}

		public void Save(bool update)
		{
			if (_fileType == FileType.Existing)
			{
				_scriptEditor.SaveFile(_file.FullName);
				_file.Contents = _scriptEditor.Text;
				_scriptTab.Title = Utils.RemoveAsterisk(_scriptTab.Title);
				ParseBuildFile();				

				if (update) _mediator.UpdateDisplay();
			}
			else if (_fileType == FileType.New)
			{
				if (_mediator.SaveAsClicked() && update)
				{
					 _mediator.UpdateDisplay();
				}
			}
		}



		public void LostFocus(object sender, EventArgs e)
		{
			_mediator.TabLostFocus();
		}

		public void Undo()
		{
			_scriptEditor.Undo();
		}

		public void Redo()
		{
			_scriptEditor.Redo();
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
			if (_buildRunner != null)
			{
				_buildRunner.Run();
			}
		}

//		public void SetProperties(PropertyCollection properties)
//		{
//			Assert.NotNull(properties, "properties");
//			if (_buildRunner != null)
//			{
//				_buildRunner.Properties = properties;	
//			}
//		}

		public void SetTargets(TargetCollection targets)
		{
			Assert.NotNull(targets, "targets");
			if (_buildRunner != null)
			{
				_buildRunner.Targets = targets;
			}
		}

		public void Close(CancelEventArgs e)
		{
			DialogResult result;
			
			if (IsDirty)
			{
				result = MessageBox.Show("You have unsaved changes to " + _file.Name + ".  Save?", 
							"Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

				if (result == DialogResult.Yes)
				{
					Save(false);
				}
				else if (result == DialogResult.Cancel)
				{
					e.Cancel = true;
				}
			}
			else
			{
				result = DialogResult.Yes;
			}
			
			if (result != DialogResult.Cancel)
			{
				_file.Close();
				_file = null;
				_buildRunner = null;
			}
		}

		public void Cut()
		{
			_clipboardHandler.Cut(this, new EventArgs());
		}

		public void Copy()
		{
			_clipboardHandler.Copy(this, new EventArgs());
		}

		public void Paste()
		{
			_clipboardHandler.Paste(this, new EventArgs());
		}

		public void Delete()
		{
			_clipboardHandler.Delete(this, new EventArgs());
		}

		public void SelectAll()
		{
			_clipboardHandler.SelectAll(this, new EventArgs());
		}

//		public void Focus()
//		{
//			_scriptEditor.Focus();
//		}
		
		public void SetCursor(int line, int column)
		{
			_scriptEditor.ActiveTextAreaControl.JumpTo(line, column);
		}

		private void Initialize()
		{
			TextArea textArea = _scriptEditor.ActiveTextAreaControl.TextArea;
			_clipboardHandler = textArea.ClipboardHandler;
			_scriptEditor.Document.DocumentChanged += new DocumentEventHandler(Editor_TextChanged);
			textArea.Enter += new EventHandler(GotFocus);
			textArea.Leave += new EventHandler(LostFocus);

			_scriptTab.Controls.Add(_scriptEditor);
			_scriptTab.Location = new Point(0, 0);
			_scriptTab.Selected = true;
			_scriptTab.Size = new Size(824, 453);
			_scriptTab.Title = _file.Name;
		}

		private void Editor_TextChanged(object sender, DocumentEventArgs e)
		{
			if (IsDirty && !Utils.HasAsterisk(_scriptTab.Title))
			{
				_scriptTab.Title = Utils.AddAsterisk(_scriptTab.Title);
			}
			else if (!IsDirty && Utils.HasAsterisk(_scriptTab.Title))
			{
				_scriptTab.Title = Utils.RemoveAsterisk(_scriptTab.Title);
			}

			// Can't parse a file that doesn't exist on the harddrive
			if (_fileType == FileType.Existing)
			{
				_mediator.UpdateDisplay();
			}
		}

		private void ParseBuildFile()
		{
			// Might want a more specific exception type to be caught here.
			// For example, a NullReferenceException on _buildRunner 
			// shouldn't be ignored.
			try
			{
				_buildScript.Parse();
			}
			catch
			{
				/* ignore */
			}
		}

		private void GotFocus(object sender, EventArgs e)
		{
			_mediator.TabGotFocus();
		}

		#region Properties

		public IBuildScript BuildScript
		{
			get { return _buildScript; }
		}

		public VoidVoid BuildFinished
		{
			set { _buildRunner.BuildFinished += value; }
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
			get { return _file.Contents != _scriptEditor.Text; }
		}
		
		public TabPage ScriptTab
		{
			get { return _scriptTab; }
		}

		#endregion
	}
}