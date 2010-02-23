#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2007 Colin Svingen
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General internal License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General internal License for more details.
//
// You should have received a copy of the GNU General internal License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Colin Svingen (swoogan@gmail.com)

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using Microsoft.VisualBasic.ApplicationServices;
using NAnt.Core.Util;
using NAntGui.Core;
using NAntGui.Framework;
using NAntGui.Gui.Controls;
using WeifenLuo.WinFormsUI.Docking;
using NAntGui.Gui.Properties;
using System.Threading;

namespace NAntGui.Gui
{
    public class MainController
    {
		private BackgroundWorker _loader = new BackgroundWorker();
        private NAntGuiForm _mainForm;
		private IEditCommands _editCommands;		
		private CommandLineOptions _options;
        private OutputWindow _outputWindow;
        private bool _ignoreDocumentChanged = false;

        public MainController(CommandLineOptions options)
		{
            Assert.NotNull(options, "options");
            _options = options;
			_loader.DoWork += new DoWorkEventHandler(loader_DoWork);
            _loader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_loader_RunWorkerCompleted);
		}

		internal void NewBlankDocument()
		{            
            NAntDocument doc = new NAntDocument(_outputWindow, this);
            DocumentWindow window = new DocumentWindow(doc);
            SetupWindow(window, doc);
		}
		
		internal void NewNAntProjectClicked()
		{
			NewProjectForm form = new NewProjectForm();
			form.NewClicked += new EventHandler(CreateNewProject);
			form.Show();			
		}	
		
		private void CreateNewProject(object sender, EventArgs e)
		{
            // TODO: Implement New Project
			ProjectInfo info = ((NewProjectEventArgs)e).Info;
			DocumentWindow window = new DocumentWindow();
            window.DocumentChanged += new DocumentEventHandler(window_DocumentChanged);
		}

        internal void Run(List<BuildTarget> targets)
		{
            if (ActiveDocument.IsDirty(ActiveWindow.Contents))
            {
                SaveDocument();
                UpdateTitle();
            }

            ActiveDocument.SetTargets(targets);
            ActiveDocument.Run();
		}
		
		internal void Stop()
		{
            ActiveDocument.Stop();			
		}

        private void UpdateTitle()
        {
            bool isDirty = ActiveDocument.IsDirty(ActiveWindow.Contents);

            if (isDirty && !Utils.HasAsterisk(ActiveWindow.TabText))
            {
                ActiveWindow.TabText = Utils.AddAsterisk(ActiveWindow.TabText);
            }
            else if (!isDirty && Utils.HasAsterisk(ActiveWindow.TabText))
            {
                ActiveWindow.TabText = Utils.RemoveAsterisk(ActiveWindow.TabText);
            }
        }

        internal void SaveDocument()
        {
            _ignoreDocumentChanged = true;
            ActiveDocument.Save(ActiveWindow.Contents, true);
            _ignoreDocumentChanged = false;            

            UpdateTitle();
            UpdateDisplay();
            _mainForm.Enable();
        }

		internal bool SaveDocumentAs()
		{
			string filename = BuildFileBrowser.BrowseForSave();
			if (filename != null)
			{
                ActiveDocument.SaveAs(filename, ActiveWindow.Contents);
                ActiveWindow.TabText = ActiveDocument.Name;
				this.ActiveDocument.BuildFinished = _mainForm.SetStateStopped;

				Settings.Default.SaveScriptInitialDir = this.ActiveDocument.Directory;
                Settings.Default.Save();

                RecentItems.Add(filename);              

				_mainForm.CreateRecentItemsMenu();
				_mainForm.Enable();

				return true;
			}
			else
			{
				return false;
			}
		}

		internal void ReloadActiveDocument()
		{
            TextLocation position = ActiveWindow.CaretPosition;
            ActiveDocument.Reload();
            ActiveWindow.Contents = ActiveDocument.Contents;
            ActiveWindow.TabText = ActiveDocument.Name;
            ActiveWindow.MoveCaretTo(position.Line, position.Column);

            UpdateDisplay();
		}

		internal void OpenDocument()
		{
			foreach (string filename in BuildFileBrowser.BrowseForLoad())
			{
				LoadDocument(filename);
			}
		}

		/// <summary>
		/// Called with the Close File menu item is clicked.
		/// </summary>
		internal void Close()
		{
			FormClosingEventArgs e = new FormClosingEventArgs(CloseReason.UserClosing, false);
			CloseDocument(_mainForm, e);

			if (!e.Cancel)
			{
				ActiveWindow.Close();				
			}
		}
		
		internal void CloseDocument(object sender, FormClosingEventArgs e)
		{
            DocumentWindow window;

            if (sender is DocumentWindow)
                window = sender as DocumentWindow;
            else
                window = ActiveWindow;

			if (window.Document.IsDirty(window.Contents))
			{
                DialogResult result = MessageBox.Show("You have unsaved changes to " +
                                         window.Document.Name + ".  Save?",
				                         "Save Changes?", MessageBoxButtons.YesNoCancel, 
				                         MessageBoxIcon.Exclamation);

				if (result == DialogResult.Yes)
				{
                    _ignoreDocumentChanged = true;
                    window.Document.Save(window.Contents, false);
                    _ignoreDocumentChanged = false;
				}
				else if (result == DialogResult.Cancel)
				{
					e.Cancel = true;
				}
			}
    	}
		
		internal void CloseAllDocuments()
		{
            for (int index = _mainForm.DockPanel.Contents.Count - 1; index >= 0; index--)
            {
                if (_mainForm.DockPanel.Contents[index] is DocumentWindow)
                {
                    DocumentWindow window = (DocumentWindow)_mainForm.DockPanel.Contents[index];
                    window.Close();
                }
            }
		}
		
		internal void CloseAllButThisClicked()
		{
            for (int index = _mainForm.DockPanel.Contents.Count - 1; index >= 0; index--)
            {
                IDockContent content = _mainForm.DockPanel.Contents[index];
                if (content is DocumentWindow && content != this.ActiveWindow)
                {
                    DocumentWindow window = content as DocumentWindow;
                    window.Close();
                }
            }
		}		

		internal void About()
		{
			About about = new About();
			about.ShowDialog();
		}

		internal void MainFormClosing(FormClosingEventArgs e)
		{
            if (this.ActiveDocument != null)
			    this.ActiveDocument.Stop();

            // Don't need this event while closing (should be in CloseAllDocuments)
            _mainForm.DockPanel.ActiveDocumentChanged -= new EventHandler(DockPanel_ActiveDocumentChanged);
			//CloseAllDocuments(e);			
		}

		internal void RecentItemClicked(string file)
		{
			if (File.Exists(file))
			{
				LoadDocument(file);
			}
			else
			{
				RecentItems.Remove(file);                
				_mainForm.CreateRecentItemsMenu();

				Utils.ShowFileNotFoundError(file);
			}
		}

		internal void SelectAll()
		{
			_editCommands.SelectAll();
		}

		internal void Copy()
		{
			_editCommands.Copy();
		}

		internal void Paste()
		{
			_editCommands.Paste();
		}

		internal void NAntHelpClicked()
		{
			const string nantHelp = @"\..\nant-docs\help\index.html";
			Utils.LoadHelpFile(Utils.RunDirectory + nantHelp);
		}

		internal void NAntContribClicked()
		{
			const string nantContribHelp = @"\..\nantcontrib-docs\help\index.html";
			Utils.LoadHelpFile(Utils.RunDirectory + nantContribHelp);
		}

		internal void NAntSDKClicked()
		{
			const string nantHelpPath = @"\..\nant-docs\sdk\";
			const string nantSDKHelp = "NAnt-SDK.chm";
			string filename = Utils.RunDirectory + nantHelpPath + nantSDKHelp;

			Utils.LoadHelpFile(filename);
		}

		internal void OptionsClicked()
		{
			OptionsForm optionsForm = new OptionsForm();
			optionsForm.ShowDialog();
		}		
		
		internal DocumentWindow FindDocumentWindow(string file)
		{
            foreach (DocumentWindow window in _mainForm.DockPanel.Documents)
			{
				if (window.Document.FullName == file)
				{
					return window;
				}
			}
			
			return null;
		}

		internal void LoadDocument(string filename)
		{
			DocumentWindow window = FindDocumentWindow(filename);			
			
            if (window != null)
            {
                window.Select();
                // Need this for the file watcher, not sure if it breaks other things
                ReloadActiveDocument();
            }
            else if (!File.Exists(filename))
            {
                Utils.ShowFileNotFoundError(filename);
            }
            else
            {
                NAntDocument doc = new NAntDocument(filename, _outputWindow, this);
                doc.BuildFinished = _mainForm.SetStateStopped;

                Settings.Default.OpenScriptDir = doc.Directory;
                Settings.Default.Save();

                window = new DocumentWindow(doc);
                SetupWindow(window, doc);

                // TODO: combine the following with an event
                RecentItems.Add(doc.FullName);
                _mainForm.CreateRecentItemsMenu();

                // Parse the file in the background
                _loader.RunWorkerAsync();                
            }            
		}

        private void SetupWindow(DocumentWindow window, NAntDocument doc)
        {
            window.Contents = doc.Contents;
            window.TabText = doc.Name;
            window.DocumentChanged += new DocumentEventHandler(window_DocumentChanged);
            window.DocumentChangedOutside += new FileSystemEventHandler(window_DocumentChangedOutside);
            window.FormClosing += new FormClosingEventHandler(CloseDocument);
            window.FormClosed += new FormClosedEventHandler(window_FormClosed);
            window.Show(_mainForm.DockPanel);
        }

        void window_FormClosed(object sender, FormClosedEventArgs e)
        {
            DocumentWindow window = sender as DocumentWindow;
            window.Document.Close();            
        }

        void window_DocumentChangedOutside(object sender, FileSystemEventArgs e)
        {
            if (!_ignoreDocumentChanged)
                LoadDocument(e.FullPath);
        }

        internal DocumentWindow GetWindow(string filename)
        {
            if (!File.Exists(filename))
            {
                return null;
            }
            else
            {
                NAntDocument doc = new NAntDocument(filename, _outputWindow, this);
                doc.BuildFinished = _mainForm.SetStateStopped;

                DocumentWindow window = new DocumentWindow(doc);
                SetupWindow(window, doc);

                // TODO: combine the following with an event
                RecentItems.Add(doc.FullName);
                _mainForm.CreateRecentItemsMenu();

                ParseBuildFile(doc);
                UpdateDisplay();

                return window;
            }
        }

		internal void UpdateDisplay()
		{
            if (this.ActiveDocument != null)
            {
                _mainForm.Text = string.Format("NAnt-Gui - {0}", this.ActiveWindow.TabText);

                IBuildScript buildScript = this.ActiveDocument.BuildScript;

                string name = string.Format("{0} ({1})", buildScript.Name, buildScript.Description);

                _mainForm.SetStatus(name, this.ActiveDocument.FullName);
                _mainForm.AddTargets(buildScript.Name, buildScript.Targets);
                _mainForm.AddProperties(buildScript.Properties);
            }
		}

		internal void DragDrop(object sender, DragEventArgs e)
		{
			LoadDocument(Utils.GetDragFilename(e));
		}        

		internal void DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		internal void OutputGotFocused()
		{
            _editCommands = _outputWindow;
		}

		internal void OutputLostFocused()
		{
			_editCommands = null;
		}

		internal void Cut()
		{
			_editCommands.Cut();
		}

		internal void Delete()
		{
			_editCommands.Delete();
		}

		internal void SetCursor(int lineNumber, int columnNumber)
		{
            if (this.ActiveWindow != null)  // can be null when the app is loading
			    this.ActiveWindow.MoveCaretTo(lineNumber - 1, columnNumber - 1);
		}

		internal void Undo()
		{
			this.ActiveWindow.Undo();
		}

		internal void Redo()
		{
			this.ActiveWindow.Redo();
		}

		internal CommandLineOptions Options
		{
			get { return _options; }
		}

        internal void SetControls(NAntGuiForm mainForm, OutputWindow outputWindow)
        {
            // may decouple the form and the controller (using events) later
            _mainForm = mainForm;
            _outputWindow = outputWindow;

            _mainForm.DockPanel.ActiveDocumentChanged += new EventHandler(DockPanel_ActiveDocumentChanged);
            _mainForm.DockPanel.Leave += new EventHandler(DockPanel_Leave);
        }

        internal void ContentAdded()
        {
            if (_mainForm.DockPanel.DocumentsCount == 0)
                _mainForm.Enable();
        }

        internal void ContentRemoved(DockContentEventArgs e)
        {
            if (e.Content is DocumentWindow)
            {
                DocumentWindow window = e.Content as DocumentWindow;
                //ToolStripItem[] items = _documentsMenuItem.DropDownItems.Find(window.TabText, false);
                //_documentsMenuItem.DropDownItems.Remove(items[0]);

                if (_mainForm.DockPanel.DocumentsCount == 0)
                    _mainForm.Disable();
            }
        }

        internal void SaveAllDocuments()
        {
            throw new NotImplementedException();
        }

        #region Private Methods

        private void DockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (this.ActiveWindow != null)
            {
                _editCommands = this.ActiveWindow.EditCommands;
                _mainForm.CanUndo(this.ActiveWindow.CanUndo);
                _mainForm.CanRedo(this.ActiveWindow.CanRedo);
            }
            UpdateDisplay();
        }

        private DocumentWindow ActiveWindow
        {
            get { return _mainForm.DockPanel.ActiveDocument as DocumentWindow; }
        }

        private NAntDocument ActiveDocument
        {
            get
            {
                return (this.ActiveWindow != null) ? this.ActiveWindow.Document : null;
            }
        }

        private void DockPanel_Leave(object sender, EventArgs e)
        {
            _editCommands = null;
        }

        private void loader_DoWork(object sender, DoWorkEventArgs e)
        {
            ParseBuildFile(this.ActiveDocument);
        }

        private void _loader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _mainForm.Enable();
            UpdateDisplay();
        }

        private void ParseBuildFile(NAntDocument document)
        {
            try
            {
                document.ParseBuildScript();
            }
            catch (BuildFileLoadException error)
            {
                const string msgFrmt = "{0}: {1}{2}";
                string msg = string.Format(msgFrmt, error.Message,
                    Environment.NewLine,
                    error.InnerException.Message);

                MessageBox.Show(msg, "Error Loading Build File",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                SetCursor(error.LineNumber, error.ColumnNumber);
            }
        }

        private void window_DocumentChanged(object sender, DocumentEventArgs e)
        {
            /* 
             * The following is commented because the script is only parsed when the 
             * the document is saved.  Could change this, but will have to suppress
             * the errors.
             */
            //// Can't parse a file that doesn't exist on the harddrive
            //if (this.ActiveDocument.SourceFile.FileType == FileType.Existing &&
            //    !_loader.IsBusy)
            //{
            //    // Parse the file in the background
            //    _loader.RunWorkerAsync();
            //}

            UpdateTitle();
            _mainForm.CanRedo(this.ActiveWindow.CanRedo);
            _mainForm.CanUndo(this.ActiveWindow.CanUndo);
        }

        #endregion

 
    }
}
