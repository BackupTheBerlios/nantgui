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
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using NAntGui.Framework;
using System.ComponentModel;
using NAntGui.Core;
using NAntGui.Gui.Controls;
using NAntGui.Gui.Properties;
using System.IO;
using System.Collections.Generic;

namespace NAntGui.Gui
{
    /// <summary>
    /// Summary description for NAntGuiForm.
    /// </summary>
    public partial class NAntGuiForm
    {        
        MainController _controller;
        PropertyWindow _propertyWindow = new PropertyWindow();
        TargetsWindow _targetsWindow = new TargetsWindow();
        OutputWindow _outputWindow = new OutputWindow();
        CommandLineOptions _initialOptions;

        public NAntGuiForm(MainController controller, CommandLineOptions options)
        {
            InitializeComponent();

            Assert.NotNull(controller, "controller");            
            _controller = controller;
            _controller.SetControls(this, _outputWindow);

            Assert.NotNull(options, "options");
            _initialOptions = options;

            AttachEventHandlers();

            SetStyle(ControlStyles.DoubleBuffer, true);
            MainFormSerializer.Attach(this, _propertyWindow, _standardToolStrip, _buildToolStrip);

            this.Disable();
        }

        public void ProcessArguments(CommandLineOptions options)
        {
            if (!String.IsNullOrEmpty(options.BuildFile))
                _controller.LoadDocument(options.BuildFile);
        }

        internal void SetStatus(string name, string fullname)
        {
            _statusStrip.Items[0].Text = name;
            _statusStrip.Items[2].Text = fullname;
        }

        internal void SetStateStopped(object sender, BuildFinishedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker invoker = new MethodInvoker(
                    delegate { RunState = RunState.Stopped; }
                    );

                Invoke(invoker);
            }
            else
            {
                RunState = RunState.Stopped;
            }
        }

        internal DockPanel DockPanel
        {
            get { return _dockPanel; }
        }

        internal void Enable()
        {
            _reloadMenuItem.Enabled = true;
            _saveMenuItem.Enabled = true;
            _saveAsMenuItem.Enabled = true;
            _closeMenuItem.Enabled = true;
            _runMenuItem.Enabled = true;
            _stopMenuItem.Enabled = false;

            _reloadToolStripButton.Enabled = true;
            _saveToolStripButton.Enabled = true;
            _runToolStripButton.Enabled = true;
        }


        internal void Disable()
        {
            _reloadMenuItem.Enabled = false;
            _saveMenuItem.Enabled = false;
            _saveAsMenuItem.Enabled = false;
            _closeMenuItem.Enabled = false;
            _runMenuItem.Enabled = false;
            _stopMenuItem.Enabled = false;

            _reloadToolStripButton.Enabled = false;
            _saveToolStripButton.Enabled = false;
            _stopToolStripButton.Enabled = false;
            _runToolStripButton.Enabled = false;

            // Not sure if this should be here:
            _targetsWindow.Clear();
            _propertyWindow.Clear();

            _statusStrip.Items[0].Text = "";
            _statusStrip.Items[2].Text = "";
        }

        internal void CreateRecentItemsMenu()
        {
            _recentMenuItem.DropDownItems.Clear();

            int count = 1;
            foreach (string item in Settings.Default.RecentItems)
            {
                // need to capture the item string locally otherwise it will
                // change in all the delegates to the most recent item.
                string file = item;     
                string name = count++ + " " + file;
                ToolStripMenuItem recentItem = new ToolStripMenuItem(name);
                recentItem.Click += delegate { _controller.LoadDocument(file); };
                _recentMenuItem.DropDownItems.Add(recentItem);
            }
        }

        internal void AddProperties(PropertyCollection properties)
        {
            _propertyWindow.AddProperties(properties);
        }

        internal void AddTargets(IBuildScript buildScript)
        {
            _targetsWindow.ProjectName = buildScript.Name;
            _targetsWindow.SetTargets(buildScript.Targets);
        }

        internal RunState RunState
        {
            set
            {
                switch (value)
                {
                    case RunState.Running:
                        _runMenuItem.Enabled = false;
                        _stopMenuItem.Enabled = true;
                        _runToolStripButton.Enabled = false;
                        _stopToolStripButton.Enabled = true;
                        break;
                    case RunState.Stopped:
                        _runMenuItem.Enabled = true;
                        _stopMenuItem.Enabled = false;
                        _runToolStripButton.Enabled = true;
                        _stopToolStripButton.Enabled = false;
                        break;
                }
            }
        }

        // TODO: See if this is needed
        internal EditState EditState
        {
            set
            {
                switch (value)
                {
                    case EditState.TabFocused:
                        _cutMenuItem.Enabled = true;
                        _pasteMenuItem.Enabled = true;
                        _deleteMenuItem.Enabled = true;
                        _undoMenuItem.Enabled = true;
                        _redoMenuItem.Enabled = true;
                        _copyMenuItem.Enabled = true;
                        _selectAllMenuItem.Enabled = true;
                        _wordWrapMenuItem.Enabled = true;
                        break;
                    case EditState.OutputFocused:
                        _cutMenuItem.Enabled = false;
                        _pasteMenuItem.Enabled = false;
                        _deleteMenuItem.Enabled = false;
                        _undoMenuItem.Enabled = false;
                        _redoMenuItem.Enabled = false;
                        _copyMenuItem.Enabled = true;
                        _selectAllMenuItem.Enabled = true;
                        _wordWrapMenuItem.Enabled = true;
                        break;
                    case EditState.NoFocus:
                        _cutMenuItem.Enabled = false;
                        _pasteMenuItem.Enabled = false;
                        _deleteMenuItem.Enabled = false;
                        _undoMenuItem.Enabled = false;
                        _redoMenuItem.Enabled = false;
                        _copyMenuItem.Enabled = false;
                        _selectAllMenuItem.Enabled = false;
                        _wordWrapMenuItem.Enabled = false;
                        break;
                }
            }
        }

        internal List<BuildTarget> SelectedTargets
        {
            get { return _targetsWindow.SelectedTargets;  }
            set { _targetsWindow.SelectedTargets = value;  }
        }

        internal void CanRedo(bool canRedo)
        {
            _redoMenuItem.Enabled = canRedo;
            _redoToolStripButton.Enabled = canRedo;
        }

        internal void CanUndo(bool canUndo)
        {
            _undoMenuItem.Enabled = canUndo;
            _undoToolStripButton.Enabled = canUndo;
        }

        #region Private Methods

        private void NAntGuiForm_DragDrop(object sender, DragEventArgs e)
        {
            _controller.DragDrop(sender, e);
        }

        private void NAntGuiForm_DragEnter(object sender, DragEventArgs e)
        {
            _controller.DragEnter(sender, e);
        }

        private void NantGuiForm_Closing(object sender, FormClosingEventArgs e)
        {
            _dockPanel.SaveAsXml(Utils.DockingConfigPath);
            // Don't need this event firing when the app is closing
            _dockPanel.ContentRemoved -= new System.EventHandler<DockContentEventArgs>(_dockPanel_ContentRemoved);

            _controller.MainFormClosing(e);
        }

        private void SaveClicked(object sender, System.EventArgs e)
        {
            _controller.SaveDocument();
        }

        private void StopClicked(object sender, System.EventArgs e)
        {
            SetStateStopped(sender, new BuildFinishedEventArgs());
            _controller.Stop();
        }

        private void RunClicked(object sender, System.EventArgs e)
        {
            RunState = RunState.Running;
            _outputWindow.Clear();
            _outputWindow.Show(_dockPanel);
            _controller.Run(_targetsWindow.SelectedTargets);
        }

        private void NewClicked(object sender, System.EventArgs e)
        {
            _controller.NewBlankDocument();
        }

        private void OpenClicked(object sender, System.EventArgs e)
        {
            _controller.OpenDocument();
        }

        private void ReloadClicked(object sender, System.EventArgs e)
        {
            SetStateStopped(sender, new BuildFinishedEventArgs());
            _controller.ReloadActiveDocument();
        }

        private void _exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _saveOutput_Click(object sender, EventArgs e)
        {
            _outputWindow.SaveOutput();
        }

        private void _targetsMenuItemClick(object sender, EventArgs e)
        {
            _targetsWindow.Show(_dockPanel);
        }

        private void _propertiesMenuItemClick(object sender, EventArgs e)
        {
            _propertyWindow.Show(_dockPanel);
        }

        private void _outputMenuItemClick(object sender, EventArgs e)
        {
            _outputWindow.Show(_dockPanel);
        }

        private void _aboutMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void _nAntContribHelpMenuItem_Click(object sender, EventArgs e)
        {
            const string nantContribHelp = @"\..\nantcontrib-docs\help\index.html";
            Utils.LoadHelpFile(Utils.RunDirectory + nantContribHelp);
        }

        private void _nAntSDKHelpMenuItem_Click(object sender, EventArgs e)
        {
            const string nantHelpPath = @"\..\nant-docs\sdk\";
            const string nantSDKHelp = "NAnt-SDK.chm";
            string filename = Utils.RunDirectory + nantHelpPath + nantSDKHelp;

            Utils.LoadHelpFile(filename);
        }

        private void _nAntHelpMenuItem_Click(object sender, EventArgs e)
        {
            const string nantHelp = @"\..\nant-docs\help\index.html";
            Utils.LoadHelpFile(Utils.RunDirectory + nantHelp);
        }

        private void _optionsMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm optionsForm = new OptionsForm();
            optionsForm.ShowDialog();
        }

        private void UndoClicked(object sender, EventArgs e)
        {
            _controller.Undo();
        }

        private void _saveAsMenuItem_Click(object sender, EventArgs e)
        {
            _controller.SaveDocumentAs();
        }

        private void _closeMenuItem_Click(object sender, EventArgs e)
        {
            _controller.Close();
        }

        private void RedoClicked(object sender, EventArgs e)
        {
            _controller.Redo();
        }

        private void CutClicked(object sender, EventArgs e)
        {
            _controller.Cut();
        }

        private void CopyClicked(object sender, EventArgs e)
        {
            _controller.Copy();
        }

        private void PasteClicked(object sender, EventArgs e)
        {
            _controller.Paste();
        }

        private void _deleteMenuItem_Click(object sender, EventArgs e)
        {
            _controller.Delete();
        }

        private void _selectAllMenuItem_Click(object sender, EventArgs e)
        {
            _controller.SelectAll();
        }

        private void closeAllDocumentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _controller.CloseAllDocuments();
        }

        private void wordWrapOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _wordWrapMenuItem.Checked = !_wordWrapMenuItem.Checked;
            _outputWindow.WordWrap = _wordWrapMenuItem.Checked;
            Settings.Default.WordWrapOutput = _wordWrapMenuItem.Checked;
        }

        private void NAntGuiForm_Load(object sender, EventArgs e)
        {
            DeserializeDockContent deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            if (File.Exists(Utils.DockingConfigPath))
                _dockPanel.LoadFromXml(Utils.DockingConfigPath, deserializeDockContent);

            if (!String.IsNullOrEmpty(_initialOptions.BuildFile))
                ProcessArguments(_initialOptions);

            // Don't need this after the app loads
            _initialOptions = null;

            _wordWrapMenuItem.Checked = Settings.Default.WordWrapOutput;
            _outputWindow.WordWrap = Settings.Default.WordWrapOutput;

            CreateRecentItemsMenu();
        }

        private void _dockPanel_ContentAdded(object sender, DockContentEventArgs e)
        {
            if (e.Content is DocumentWindow)
            {
                _controller.ContentAdded();

                //DocumentWindow window = e.Content as DocumentWindow;
                //ToolStripMenuItem item = new ToolStripMenuItem(window.TabText);
                //item.Tag = window.Document.FullName;
                //item.Click += new EventHandler(Document_Click);
                //                _documentsMenuItem.DropDownItems.Add(item);
            }
        }

        private void _dockPanel_ContentRemoved(object sender, DockContentEventArgs e)
        {
            _controller.ContentRemoved(e);
        }

        private void Document_Click(object sender, EventArgs e)
        {
            _controller.LoadDocument(((ToolStripMenuItem)sender).Tag.ToString());
        }

        private void AttachEventHandlers()
        {
            _outputWindow.Enter += new EventHandler(_outputWindow_Enter);
            _outputWindow.Leave += new EventHandler(_outputWindow_Leave);

            _outputWindow.DragEnter += new DragEventHandler(NAntGuiForm_DragEnter);
            _outputWindow.DragDrop += new DragEventHandler(NAntGuiForm_DragDrop);

            _outputWindow.FileNameClicked += new EventHandler<FileNameEventArgs>(_outputWindow_FileNameClicked);

            _propertyWindow.DragEnter += new DragEventHandler(NAntGuiForm_DragEnter);
            _propertyWindow.DragDrop += new DragEventHandler(NAntGuiForm_DragDrop);

            _targetsWindow.DragEnter += new DragEventHandler(NAntGuiForm_DragEnter);
            _targetsWindow.DragDrop += new DragEventHandler(NAntGuiForm_DragDrop);
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(TargetsWindow).ToString())
                return _targetsWindow;
            else if (persistString == typeof(PropertyWindow).ToString())
                return _propertyWindow;
            else if (persistString == typeof(OutputWindow).ToString())
                return _outputWindow;
            else
            {
                // DocumentWindow overrides GetPersistString to add extra information into persistString.
                string[] parsedStrings = persistString.Split(new char[] { '|' });
                if (parsedStrings.Length == 2 &&
                    parsedStrings[0] == typeof(DocumentWindow).ToString() &&
                    parsedStrings[1] != string.Empty &&
                    Settings.Default.OpenPreviousTabs)
                {
                    return _controller.GetWindow(parsedStrings[1]);
                }

                return null;
            }
        }

        private void _outputWindow_Enter(object sender, EventArgs e)
        {
            _controller.OutputGotFocused();
            EditState = EditState.TabFocused;
        }

        private void _outputWindow_Leave(object sender, EventArgs e)
        {
            _controller.OutputLostFocused();
            EditState = EditState.NoFocus;
        }

        private void _outputWindow_FileNameClicked(object sender, FileNameEventArgs e)
        {
            _controller.LoadDocument(e.FileName);
            _controller.SetCursor(e.Point.X, e.Point.Y);
        }

        private void saveAllMenuItem_Click(object sender, EventArgs e)
        {
            _controller.SaveAllDocuments();
        }

        private void NewProjectClicked(object sender, EventArgs e)
        {
            _controller.NewNAntProjectClicked();
        }

        #endregion
    }
}
