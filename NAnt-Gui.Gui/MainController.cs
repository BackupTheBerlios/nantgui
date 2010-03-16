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
// Colin Svingen (swoogan@gmail.com)

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using NAntGui.Core;
using NAntGui.Framework;
using NAntGui.Gui.Controls;
using NAntGui.Gui.Properties;
using WeifenLuo.WinFormsUI.Docking;

namespace NAntGui.Gui
{
    public class MainController
    {
        private const string BLANK_PROJECT = "BlankProject.build";
        
        private readonly BackgroundWorker _loader = new BackgroundWorker();
        private readonly CommandLineOptions _options;

        private NAntGuiForm _mainForm;
        private IEditCommands _editCommands;
        private OutputWindow _outputWindow;

        private readonly Dictionary<DocumentWindow, NAntDocument> _documents = new Dictionary<DocumentWindow, NAntDocument>();
        private readonly List<FileWatcher> _watchers = new List<FileWatcher>();

        public MainController(CommandLineOptions options)
        {
            Assert.NotNull(options, "options");
            _options = options;
            _loader.DoWork += LoaderDoWork;
            _loader.RunWorkerCompleted += LoaderRunWorkerCompleted;
            RecentItems.ItemAdded += RecentItems_ItemAdded;
        }

        private void RecentItems_ItemAdded(object sender, RecentItemsEventArgs e)
        {
            _mainForm.CreateRecentItemsMenu();
        }

        internal void NewBlankDocument()
        {
            NAntDocument doc = new NAntDocument(_outputWindow, _options);
            DocumentWindow window = new DocumentWindow(doc.FullName);
            SetupWindow(window, doc);
        }

        internal void NewNAntProjectClicked()
        {
            NewProjectForm form = new NewProjectForm();
            form.NewClicked += CreateNewProject;
            form.Show();
        }

        private void CreateNewProject(object sender, NewProjectEventArgs e)
        {
            NAntDocument doc = new NAntDocument(_outputWindow, _options);
//            doc.Contents = GetNewDocumentContents(e.Info);
            DocumentWindow window = new DocumentWindow(doc.FullName);
            SetupWindow(window, doc);
            window.Contents = GetNewDocumentContents(e.Info);
        }

        private static string GetNewDocumentContents(ProjectInfo projectInfo)
        {
            string contents = "";
            string path = Path.Combine("..", BLANK_PROJECT);
            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(path);
                XmlElement element = xml.GetElementsByTagName("project")[0] as XmlElement;
                element.Attributes["name"].Value = projectInfo.Name;
                element.Attributes["default"].Value = projectInfo.Default;

                if (string.IsNullOrEmpty(projectInfo.Basedir))
                    element.RemoveAttribute("basedir");
                else
                    element.Attributes["basedir"].Value = projectInfo.Basedir;


                XmlNode node = xml.GetElementsByTagName("description")[0];
                node.InnerText = projectInfo.Description;

                node = xml.GetElementsByTagName("target")[0];
                node.Attributes["name"].Value = projectInfo.Default;
                node.Attributes["description"].Value = projectInfo.Default;

                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter xtw = new XmlTextWriter(sw))
                    {
                        xtw.Formatting = Formatting.Indented;
                        xml.WriteTo(xtw);
                        contents = sw.ToString();
                    }
                }
            }
            catch
            {
                MessageBox.Show("New project template missing.  Please re-install the application.");
            }

            return contents;
        }

        internal void Run(List<BuildTarget> targets)
        {
            if (ActiveDocument.IsDirty(ActiveWindow.Contents))
            {
                SaveDocument();
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
            SaveDocument(ActiveWindow);
        }

        private void SaveDocument(DocumentWindow window)
        {
            NAntDocument document = _documents[window];

            if (document.FileType == FileType.New)
            {
                SaveDocumentAs(window);
            }
            else if (document.IsDirty(window.Contents))
            {
                try
                {
                    //_ignoreDocumentChanged = true;
                    document.Save(window.Contents, true);
                    //_ignoreDocumentChanged = false;

                    if (window == ActiveWindow)
                    {
                        List<BuildTarget> targets = _mainForm.SelectedTargets;
                        UpdateTitle();
                        UpdateDisplay();
                        _mainForm.SelectedTargets = targets;

                        _mainForm.Enable();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while saving the file:\n" + ex.Message, "Error while saving",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        internal void SaveDocumentAs()
        {
            SaveDocumentAs(ActiveWindow);
        }

        private void SaveDocumentAs(DocumentWindow window)
        {
            string filename = BuildFileBrowser.BrowseForSave();
            if (filename != null)
            {
                NAntDocument document = _documents[window];

                try
                {
                    document.SaveAs(filename, window.Contents);
                    window.TabText = document.Name;
                    document.BuildFinished = _mainForm.SetStateStopped;

                    Settings.Default.SaveScriptInitialDir = document.Directory;
                    Settings.Default.Save();

                    RecentItems.Add(filename);

                    _mainForm.CreateRecentItemsMenu();
                    _mainForm.Enable();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while saving the file:\n" + ex.Message, "Error while saving",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        internal void SaveAllDocuments()
        {
            foreach (DocumentWindow window in _mainForm.DockPanel.Documents)
            {
                SaveDocument(window);
            }
        }

        internal void ReloadActiveDocument()
        {
            try
            {
                TextLocation position = ActiveWindow.CaretPosition;
                ActiveDocument.Reload();
                ActiveWindow.Contents = ActiveDocument.Contents;
                ActiveWindow.TabText = ActiveDocument.Name;
                ActiveWindow.MoveCaretTo(position.Line, position.Column);

                UpdateDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the file:\n" + ex.Message, "Error while loading",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
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
            CloseDocument(ActiveWindow, e);

            if (!e.Cancel)
            {
                ActiveWindow.Close();
            }
        }

        private void CloseDocument(object sender, FormClosingEventArgs e)
        {
            DocumentWindow window;

            if (sender is DocumentWindow)
                window = sender as DocumentWindow;
            else
                window = ActiveWindow;

            NAntDocument document = _documents[window];

            if (document.IsDirty(window.Contents))
            {
                DialogResult result = MessageBox.Show("You have unsaved changes to " +
                                                      document.Name + ".  Save?",
                                                      "Save Changes?", MessageBoxButtons.YesNoCancel,
                                                      MessageBoxIcon.Exclamation);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        //_ignoreDocumentChanged = true;
                        document.Save(window.Contents, false);
                        //_ignoreDocumentChanged = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while saving the file:\n" + ex.Message, "Error while saving",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }

            if (!e.Cancel)
                _mainForm.RemoveDocumentMenuItem(document);
        }

        internal void CloseAllDocuments()
        {
            for (int index = _mainForm.DockPanel.Contents.Count - 1; index >= 0; index--)
            {
                if (_mainForm.DockPanel.Contents[index] is DocumentWindow)
                {
                    DocumentWindow window = (DocumentWindow) _mainForm.DockPanel.Contents[index];
                    window.Close();
                }
            }
        }

        private void CloseAllButThisClicked()
        {
            for (int index = _mainForm.DockPanel.Contents.Count - 1; index >= 0; index--)
            {
                IDockContent content = _mainForm.DockPanel.Contents[index];
                if (content is DocumentWindow && content != ActiveWindow)
                {
                    DocumentWindow window = content as DocumentWindow;
                    window.Close();
                }
            }
        }

        internal static void ShowAboutForm()
        {
            About about = new About();
            about.ShowDialog();
        }

        internal void MainFormClosing(FormClosingEventArgs e)
        {
            // Don't need this event while closing (should be in CloseAllDocuments)
            _mainForm.DockPanel.ActiveDocumentChanged -= DockPanel_ActiveDocumentChanged;
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

        internal static void ShowNAntDocumentation()
        {
            const string nantHelp = @"\..\nant-docs\help\index.html";
            Utils.LoadHelpFile(Utils.RunDirectory + nantHelp);
        }

        internal static void ShowNAntContribDocumentation()
        {
            const string nantContribHelp = @"\..\nantcontrib-docs\help\index.html";
            Utils.LoadHelpFile(Utils.RunDirectory + nantContribHelp);
        }

        internal static void ShowNAntSdkHelp()
        {
            const string nantHelpPath = @"\..\nant-docs\sdk\";
            const string nantSdkHelp = "NAnt-SDK.chm";
            string filename = Utils.RunDirectory + nantHelpPath + nantSdkHelp;

            Utils.LoadHelpFile(filename);
        }

        internal static void ShowOptions()
        {
            OptionsForm optionsForm = new OptionsForm();
            optionsForm.ShowDialog();
        }

        private DocumentWindow FindDocumentWindow(string file)
        {
            foreach (DocumentWindow window in _mainForm.DockPanel.Documents)
            {
                if (_documents[window].FullName == file)
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
                NAntDocument doc = new NAntDocument(filename, _outputWindow, _options);
                doc.BuildFinished = _mainForm.SetStateStopped;

                Settings.Default.OpenScriptDir = doc.Directory;
                Settings.Default.Save();

                window = new DocumentWindow(doc.FullName);
                SetupWindow(window, doc);

                RecentItems.Add(doc.FullName);

                // Parse the file in the background
                _loader.RunWorkerAsync();
            }
        }

        private void SetupWindow(DocumentWindow window, NAntDocument doc)
        {
            _documents.Add(window, doc);
            _mainForm.AddDocumentMenuItem(doc);
            _mainForm.Enable();

            window.Contents = doc.Contents;
            window.TabText = doc.Name;

            window.DocumentChanged += WindowDocumentChanged;
            window.FormClosing += CloseDocument;
            window.FormClosed += WindowFormClosed;
            window.CloseClicked += delegate { Close(); };
            window.CloseAllClicked += delegate { CloseAllDocuments(); };
            window.CloseAllButThisClicked += delegate { CloseAllButThisClicked(); };
            window.SaveClicked += delegate { SaveDocument(); };
            window.Show(_mainForm.DockPanel);

            if (doc.FileType == FileType.Existing)
            {
                FileWatcher watcher = new FileWatcher();
                watcher.SynchronizingObject = window;
                watcher.Watch(doc.FullName);
                watcher.Changed += WindowDocumentChangedOutside;
                _watchers.Add(watcher);
            }
        }

        private void WindowFormClosed(object sender, FormClosedEventArgs e)
        {
            if (sender is DocumentWindow)
            {
                DocumentWindow window = sender as DocumentWindow;

                _documents[window].Close();
                _documents.Remove(window);
                
                if (_documents.Count == 0) _mainForm.Disable();
            }
        }

        private void WindowDocumentChangedOutside(object sender, FileSystemEventArgs e)
        {
            FileWatcher watcher = (FileWatcher)sender;

            watcher.Disable();
            LoadDocument(e.FullPath);
            watcher.Enable();
        }

        internal DocumentWindow GetWindow(string filename)
        {
            if (File.Exists(filename))
            {
                NAntDocument doc = new NAntDocument(filename, _outputWindow, _options);
                doc.BuildFinished = _mainForm.SetStateStopped;

                DocumentWindow window = new DocumentWindow(doc.FullName);
                SetupWindow(window, doc);

                RecentItems.Add(doc.FullName);

                ParseBuildFile(doc);
                UpdateDisplay();

                return window;
            }

            Utils.ShowFileNotFoundError(filename);
            return null;
        }

        private void UpdateDisplay()
        {
            if (ActiveDocument != null)
            {
                _mainForm.Text = string.Format("NAnt-Gui - {0}", ActiveWindow.TabText);

                IBuildScript buildScript = ActiveDocument.BuildScript;

                string name = string.Format("{0} ({1})", buildScript.Name, buildScript.Description);

                _mainForm.SetStatus(name, ActiveDocument.FullName);
                _mainForm.AddTargets(buildScript);
                _mainForm.AddProperties(buildScript.Properties);
            }
        }

        internal void DragDrop(object sender, DragEventArgs e)
        {
            LoadDocument(Utils.GetDragFilename(e));
        }

        internal void DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
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
            if (ActiveWindow != null) // can be null when the app is loading
                ActiveWindow.MoveCaretTo(lineNumber - 1, columnNumber - 1);
        }

        internal void Undo()
        {
            ActiveWindow.Undo();
        }

        internal void Redo()
        {
            ActiveWindow.Redo();
        }

        internal void SetControls(NAntGuiForm mainForm, OutputWindow outputWindow)
        {
            // may decouple the form and the controller (using events) later
            _mainForm = mainForm;
            _outputWindow = outputWindow;

            _mainForm.DockPanel.ActiveDocumentChanged += DockPanel_ActiveDocumentChanged;
            _mainForm.DockPanel.Leave += DockPanel_Leave;
        }

        public void SelectWindow(string filename)
        {
            DocumentWindow window = FindDocumentWindow(filename);

            if (window != null)
            {
                window.Activate();
            }
        }

        //internal void ContentAdded()
        //{
        //    if (_mainForm.DockPanel.DocumentsCount == 0)
        //        _mainForm.Enable();
        //}

        //internal void ContentRemoved(DockContentEventArgs e)
        //{
        //    if (e.Content is DocumentWindow)
        //    {
        //        DocumentWindow window = e.Content as DocumentWindow;
        //        ToolStripItem[] items = _documentsMenuItem.DropDownItems.Find(window.TabText, false);
        //        _documentsMenuItem.DropDownItems.Remove(items[0]);

        //        if (_mainForm.DockPanel.DocumentsCount == 0)
        //            _mainForm.Disable();
        //    }
        //}

 

        #region Private Methods

        private DocumentWindow ActiveWindow
        {
            get { return _mainForm.DockPanel.ActiveDocument as DocumentWindow; }
        }

        private NAntDocument ActiveDocument
        {
            get { return (ActiveWindow != null) ? _documents[ActiveWindow] : null; }
        }

        private void DockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (ActiveWindow != null)
            {
                _editCommands = ActiveWindow.EditCommands;
                _mainForm.CanUndo(ActiveWindow.CanUndo);
                _mainForm.CanRedo(ActiveWindow.CanRedo);
                _mainForm.CheckActiveDocuemnt(_documents[ActiveWindow]);
            }
            UpdateDisplay();
        }

        private void DockPanel_Leave(object sender, EventArgs e)
        {
            _editCommands = null;
        }

        private void LoaderDoWork(object sender, DoWorkEventArgs e)
        {
            ParseBuildFile(ActiveDocument);
        }

        private void LoaderRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

        private void WindowDocumentChanged(object sender, DocumentEventArgs e)
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
            _mainForm.CanRedo(ActiveWindow.CanRedo);
            _mainForm.CanUndo(ActiveWindow.CanUndo);
        }

        #endregion

 
    }
}