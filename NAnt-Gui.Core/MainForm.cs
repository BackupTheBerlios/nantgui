#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2005 Colin Svingen
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
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Menus;
using Flobbster.Windows.Forms;
using NAntGui.Core.NAnt;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : Form, ILogsMessage
	{
		private const int RICH_TEXT_INDEX = 2;
		private const int PLAIN_TEXT_INDEX = 1;

		private delegate void MessageEventHandler(string pMessage);

		private BuildRunner _buildRunner;
		private CommandLineOptions _options;
		private RecentItems _recentItems = new RecentItems();
		private DockingManager _dockManager;
		private string _buildFile;
		private bool _firstLoad = true;

		#region GUI Items

		private StatusBarPanel FileStatusBarPanel;
		private OpenFileDialog OpenFileDialog;
		private StatusBar MainStatusBar;

		private ImageList _imageList;

		private OutputBox _outputBox = new OutputBox();
		private Content _outputContent;

		private TargetsTreeView _targetsTree;
		private Content _targetsContent;
		private WindowContent _targetWindowContent;

		public MainPropertyGrid _propertyGrid;
		private Content _propertiesContent;

		private StatusBarPanel ProgressPanel;
		private StatusBarPanel FullFileStatusBarPanel;

		private MainMenuControl _mainMenu;
		private ToolBarControl _mainToolBar;

		private SourceTabControl _sourceTabs;

		private SaveFileDialog OutputSaveFileDialog;
		private SaveFileDialog XMLSaveFileDialog;

		private IContainer components;
		

		#endregion

		public MainForm(CommandLineOptions options)
		{
			_options = options;

			_propertyGrid = new MainPropertyGrid(_options.Properties);

			_imageList = ResourceHelper.LoadBitmapStrip(
				this.GetType(), "NAntGui.Core.Images.MenuItems.bmp",
				new Size(16, 16), new Point(0, 0));

			_targetsTree = new TargetsTreeView(new EventHandler(this.BuildMenuCommand_Click));

			this.Initialize();

			// Reduce the amount of flicker that occurs when windows are redocked within
			// the container. As this prevents unsightly backcolors being drawn in the
			// WM_ERASEBACKGROUND that seems to occur.
			this.SetStyle(ControlStyles.DoubleBuffer, true);
//			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			this.SetupDockManager();

			_outputBox.WordWrapChanged += new VoidBool(this.WordWrap_Changed);

			_buildRunner = new NAntBuildRunner(this);
			_buildRunner.BuildFileChanged += new BuildFileChangedEH(this.BuildFileLoaded);
			_buildRunner.OnBuildFinished = new VoidVoid(this.Update);

			_recentItems.ItemsUpdated += new VoidVoid(this.UpdateRecentItemsMenu);
			_recentItems.Load();

			this.AssignMenuCommands();
			this.AssignToolBarButtons();
			this.LoadInitialBuildFile();
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Initialize

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void Initialize()
		{
			this.components = new Container();
			ResourceManager resources = new ResourceManager(typeof (MainForm));
			this.OpenFileDialog = new OpenFileDialog();
			this.MainStatusBar = new StatusBar();
			this.FileStatusBarPanel = new StatusBarPanel();
			this.FullFileStatusBarPanel = new StatusBarPanel();
			this.ProgressPanel = new StatusBarPanel();
			this._mainMenu = new MainMenuControl();
			this._mainToolBar = new ToolBarControl();
			this.OutputSaveFileDialog = new SaveFileDialog();
			this._sourceTabs = new SourceTabControl();
			this.XMLSaveFileDialog = new SaveFileDialog();
			this.FileStatusBarPanel.BeginInit();
			this.FullFileStatusBarPanel.BeginInit();
			this.ProgressPanel.BeginInit();
			this.SuspendLayout();
			// 
			// OpenFileDialog
			// 
			this.OpenFileDialog.DefaultExt = "build";
			this.OpenFileDialog.Filter = "Build Files (*.build)|*.build";
			// 
			// MainStatusBar
			// 
			this.MainStatusBar.Location = new Point(0, 531);
			this.MainStatusBar.Name = "MainStatusBar";
			this.MainStatusBar.Panels.AddRange(new StatusBarPanel[]
				{
					this.FileStatusBarPanel,
					this.FullFileStatusBarPanel,
					this.ProgressPanel
				});
			this.MainStatusBar.ShowPanels = true;
			this.MainStatusBar.Size = new Size(824, 22);
			this.MainStatusBar.SizingGrip = false;
			this.MainStatusBar.TabIndex = 2;
			// 
			// FileStatusBarPanel
			// 
			this.FileStatusBarPanel.AutoSize = StatusBarPanelAutoSize.Contents;
			this.FileStatusBarPanel.Width = 10;
			// 
			// FullFileStatusBarPanel
			// 
			this.FullFileStatusBarPanel.AutoSize = StatusBarPanelAutoSize.Spring;
			this.FullFileStatusBarPanel.Width = 804;
			// 
			// ProgressPanel
			// 
			this.ProgressPanel.MinWidth = 0;
			this.ProgressPanel.Style = StatusBarPanelStyle.OwnerDraw;
			this.ProgressPanel.Width = 10;
			// 
			// OutputSaveFileDialog
			// 
			this.OutputSaveFileDialog.DefaultExt = "txt";
			this.OutputSaveFileDialog.FileName = "Output";
			this.OutputSaveFileDialog.Filter = "Text Document|*.txt|Rich Text Format (RTF)|*.rtf";
			// 
			// SourceTabs
			// 
			this._sourceTabs.SourceRestored += new VoidVoid(this.Source_Restored);
			this._sourceTabs.SourceChanged += new VoidVoid(this.Source_Changed);
			// 
			// XMLSaveFileDialog
			// 
			this.XMLSaveFileDialog.DefaultExt = "build";
			this.XMLSaveFileDialog.Filter = "NAnt Buildfile|*.build|NAnt Include|*.inc";
			// 
			// NAntForm
			// 
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new Size(5, 13);
			this.ClientSize = new Size(824, 553);
			this.Controls.Add(this._sourceTabs);
			this.Controls.Add(this.MainStatusBar);
			this.Controls.Add(this._mainToolBar);
			this.Controls.Add(this._mainMenu);
			
			this.Icon = ((Icon) (resources.GetObject("$this.Icon")));
			this.MinimumSize = new Size(480, 344);
			this.Name = "NAntForm";
			this.Text = "NAnt-Gui";
			this.Closing += new CancelEventHandler(this.MainForm_Closing);
			this.DragDrop += new DragEventHandler(this.NAnt_DragDrop);
			this.Closed += new EventHandler(this.NAnt_Closed);
			this.DragEnter += new DragEventHandler(this.NAnt_DragEnter);
			this.FileStatusBarPanel.EndInit();
			this.FullFileStatusBarPanel.EndInit();
			this.ProgressPanel.EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private void AssignToolBarButtons()
		{
			this._mainToolBar.Build_Click += new VoidVoid(this.Build);
			this._mainToolBar.Open_Click += new VoidVoid(this.BrowseForBuildFile);
			this._mainToolBar.Save_Click += new VoidVoid(this.Save);
			this._mainToolBar.Reload_Click += new VoidVoid(this.Reload);
			this._mainToolBar.Stop_Click += new VoidVoid(_buildRunner.Stop);
		}

		private void AssignMenuCommands()
		{
			this._mainMenu.About_Click = new EventHandler(this.AboutMenuCommand_Click);
			this._mainMenu.Build_Click = new EventHandler(this.BuildMenuCommand_Click);
			this._mainMenu.Close_Click = new EventHandler(this.CloseMenuCommand_Click);
			this._mainMenu.Copy_Click = new EventHandler(_outputBox.CopyText);
			this._mainMenu.Exit_Click = new EventHandler(this.ExitMenuCommand_Click);
			this._mainMenu.NAntContrib_Click = new EventHandler(this.NAntContribMenuCommand_Click);
			this._mainMenu.NAntHelp_Click = new EventHandler(this.NAntHelpMenuCommand_Click);
			this._mainMenu.NAntSDK_Click = new EventHandler(this.NAntSDKMenuCommand_Click);
			this._mainMenu.Open_Click = new EventHandler(this.OpenMenuCommand_Click);
			this._mainMenu.Options_Click = new EventHandler(this.OptionsMenuCommand_Click);
			this._mainMenu.Properties_Click = new EventHandler(this.PropertiesMenuCommand_Click);
			this._mainMenu.Reload_Click = new EventHandler(this.ReloadMenuCommand_Click);
			this._mainMenu.SelectAll_Click = new EventHandler(_outputBox.SelectAllText);
			this._mainMenu.SaveOutput_Click = new EventHandler(this.SaveOutputMenuCommand_Click);
			this._mainMenu.Save_Click = new EventHandler(this.SaveMenuCommand_Click);
			this._mainMenu.SaveAs_Click = new EventHandler(this.SaveAsMenuCommand_Click);
			this._mainMenu.Targets_Click = new EventHandler(this.TargetsMenuCommand_Click);
			this._mainMenu.WordWrap_Click = new EventHandler(_outputBox.DoWordWrap);
		}


		private void SetupDockManager()
		{
			// Create the object that manages the docking state
			_dockManager = new DockingManager(this, VisualStyle.IDE);

			// Ensure that the RichTextBox is always the innermost control
			_dockManager.InnerControl = this._sourceTabs;

			_targetsContent		= _dockManager.Contents.Add(_targetsTree, "Targets");
			_outputContent		= _dockManager.Contents.Add(_outputBox, "Output");
			_propertiesContent	= _dockManager.Contents.Add(this._propertyGrid, "Properties");

			_propertiesContent.ImageList = this._imageList;
			_propertiesContent.ImageIndex = 0;

			// Request a new Docking window be created for the above Content on the left edge
			_targetWindowContent = _dockManager.AddContentWithState(_targetsContent, State.DockLeft) as WindowContent;
			_dockManager.AddContentToZone(_propertiesContent, _targetWindowContent.ParentZone, 1);

			_dockManager.AddContentWithState(_outputContent, State.DockBottom);

			_dockManager.OuterControl = this.MainStatusBar;
		}

		private void LoadInitialBuildFile()
		{
			if (_options.BuildFile == null || !this.LoadCmdLineBuildFile())
			{
				this.LoadRecentBuildFile();
			}
		}

		private bool LoadCmdLineBuildFile()
		{
			try
			{
				this.LoadBuildFile(_options.BuildFile);
				return true;
			}
			catch (BuildFileNotFoundException error)
			{
				MessageBox.Show(error.Message, "Build File Not Found", 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
		}

		private void LoadBuildFile(string buildFile)
		{
			_buildFile = buildFile;

			try
			{
				_buildRunner.LoadBuildFile(buildFile);
			}
			catch (BuildFileNotFoundException error)
			{
				MessageBox.Show(error.Message, "Build File Not Found", 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void LoadRecentBuildFile()
		{
			if (_recentItems.HasRecentItems)
			{
				try
				{	
					this.LoadBuildFile(_recentItems[0]);
				}
				catch (BuildFileNotFoundException error)
				{
					_recentItems.Remove(_recentItems[0]);
					MessageBox.Show(error.Message, "Build File Not Found", 
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}


		private void ExitMenuCommand_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void OpenMenuCommand_Click(object sender, EventArgs e)
		{
			this.BrowseForBuildFile();
		}

		private void NAnt_DragEnter(object sender, DragEventArgs e)
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

		private void NAnt_DragDrop(object sender, DragEventArgs e)
		{
			this.LoadBuildFile(GetDragFilename(e));
		}

		private static string GetDragFilename(DragEventArgs e)
		{
			object dragData = e.Data.GetData(DataFormats.FileDrop, false);
			string[] files = dragData as string[];
			return files[0];
		}

		private void CloseMenuCommand_Click(object sender, EventArgs e)
		{
			this.CloseBuildFile();
		}

		private void BrowseForBuildFile()
		{
			this.OpenFileDialog.InitialDirectory = _buildFile;
			if (this.OpenFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.LoadBuildFile(this.OpenFileDialog.FileName);
			}
		}

		private void BuildMenuCommand_Click(object sender, EventArgs e)
		{
			this.Build();
		}

		private void Build()
		{
			this.ClearOutput();
			_buildRunner.Run(_buildFile);
		}

		private void ClearOutput()
		{
			_outputBox.Clear();
			Outputter.Clear();
		}

		private void NAnt_Closed(object sender, EventArgs e)
		{
			_recentItems.Save();
		}

		private void ReloadMenuCommand_Click(object sender, EventArgs e)
		{
			this.Reload();
		}

		private void Reload()
		{
			this.LoadBuildFile(_buildFile);
		}

		private void AboutMenuCommand_Click(object sender, EventArgs e)
		{
			About about = new About();
			about.ShowDialog();
		}

		public void LogMessage(string message)
		{
			if (this.InvokeRequired)
			{
				MessageEventHandler messageEH =
					new MessageEventHandler(_outputBox.WriteOutput);

				this.BeginInvoke(messageEH, new Object[1] {message});
			}
			else
			{
				_outputBox.WriteOutput(message);
			}
		}

		public void CloseBuildFile()
		{
//			_buildFile.Close();
			_buildFile = "";
			this.ClearOutput();
			_targetsTree.Nodes.Clear();
			this._propertyGrid.SelectedObject = null;

			this.DisableMenuCommandsAndButtons();
		}

		private void DisableMenuCommandsAndButtons()
		{
			this._mainMenu.Disable();
			this._mainToolBar.Disable();
		}

		private void EnableMenuCommandsAndButtons()
		{
			this._mainMenu.Enable();
			this._mainToolBar.Enable();
		}

		private void OptionsMenuCommand_Click(object sender, EventArgs e)
		{
			OptionsForm lOptionsForm = new OptionsForm();
			lOptionsForm.ShowDialog();
		}

		private void BuildFileLoaded(IProject project)
		{
			this.ClearOutput();
			this.EnableMenuCommandsAndButtons();
			this.UpdateDisplay(project);
			this.AddTargets(project.Targets);
			_propertyGrid.AddProperties(project.Properties, _firstLoad);

			_firstLoad = false;
		}

		private void UpdateDisplay(IProject project)
		{
			this._sourceTabs.LoadSource(_buildFile);

			string filename = new FileInfo(_buildFile).Name;

			this.Text = "NAnt-Gui" + string.Format(" - {0}", filename);

			string projectName = project.HasName ? project.Name : filename;

			_recentItems.Add(_buildFile);
			_recentItems.Save();
			this.UpdateRecentItemsMenu();

			this.MainStatusBar.Panels[0].Text = string.Format("{0} ({1})", projectName, project.Description);
			this.MainStatusBar.Panels[1].Text = _buildFile;

			_targetsTree.Nodes.Clear();
			_targetsTree.Nodes.Add(new TreeNode(projectName));
		}


		private void AddTargets(TargetCollection targets)
		{
			foreach (ITarget target in targets)
			{
				this.AddTargetTreeNode(target);
			}

			_targetsTree.ExpandAll();
		}

		public ArrayList GetTreeTargets()
		{
			ArrayList targets = new ArrayList();
			foreach (TreeNode node in _targetsTree.Nodes[0].Nodes)
			{
				if (node.Checked)
				{
					targets.Add(node.Text);
				}
			}

			return targets;
		}

		public PropertySpecCollection GetProjectProperties()
		{
			return _propertyGrid.GetProjectProperties();
		}

		private void AddTargetTreeNode(ITarget target)
		{
			if (!(Settings.HideTargetsWithoutDescription && !HasDescription(target.Description)))
			{
				string targetName = FormatTargetName(target.Name, target.Description);
				TreeNode node = new TreeNode(targetName);
				node.Checked = target.Default;
				node.Tag = target;
				this._targetsTree.Nodes[0].Nodes.Add(node);
			}
		}

		private static string FormatTargetName(string name, string description)
		{
			//const string format = "{0} - {1}";
			const string format = "{0}";
			return HasDescription(description) ? string.Format(format, name, description) : name;
		}

		private static bool HasDescription(string description)
		{
			return description.Length > 0;
		}

		private void UpdateRecentItemsMenu()
		{
			this._mainMenu.ClearRecentItems();

			int count = 1;
			foreach (string item in _recentItems)
			{
				EventHandler onClick = new EventHandler(this.RecentFile_Clicked);
				MenuCommand command = new MenuCommand(count++ + " " + item, onClick);
				this._mainMenu.AddRecentItem(command);
			}
		}

		private void RecentFile_Clicked(object sender, EventArgs e)
		{
			MenuCommand item = (MenuCommand) sender;
			string recentItem = item.Text.Substring(2);

			try
			{	
				this.LoadBuildFile(recentItem);
			}
			catch (BuildFileNotFoundException error)
			{
				_recentItems.Remove(recentItem);
				MessageBox.Show(error.Message, "Build File Not Found", 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void NAntSDKMenuCommand_Click(object sender, EventArgs e)
		{
			const string nantHelpPath = @"\..\nant-docs\sdk\";
			const string nantSDKHelp = "NAnt-SDK.chm";
			string filename = this.GetRunDirectory() + nantHelpPath + nantSDKHelp;

			LoadHelpFile(filename);
		}

		private static void LoadHelpFile(string filename)
		{
			if (File.Exists(filename))
			{
				Process.Start(filename);
			}
			else
			{
				MessageBox.Show("Help not found.  It may not have been installed.", "Help Not Found");
			}
		}

		private void NAntHelpMenuCommand_Click(object sender, EventArgs e)
		{
			const string nantHelp = @"\..\nant-docs\help\index.html";
			LoadHelpFile(this.GetRunDirectory() + nantHelp);
		}

		private void NAntContribMenuCommand_Click(object sender, EventArgs e)
		{
			const string nantContribHelp = @"\..\nantcontrib-docs\help\index.html";
			LoadHelpFile(this.GetRunDirectory() + nantContribHelp);
		}

		private string GetRunDirectory()
		{
			Assembly ass = Assembly.GetExecutingAssembly();
			FileInfo info = new FileInfo(ass.Location);
			return info.DirectoryName;
		}

		private void SaveOutputMenuCommand_Click(object sender, EventArgs e)
		{
			this.OutputSaveFileDialog.InitialDirectory = _buildFile;
			DialogResult result = this.OutputSaveFileDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				this.SaveOutput();
			}
		}

		private void SaveMenuCommand_Click(object sender, EventArgs e)
		{
			this.Save();
		}

		private void Save()
		{
			if (File.Exists(_buildFile))
			{
				this._sourceTabs.SaveSource(_buildFile);
				this.Text = this.Text.TrimEnd(new char[] {'*'});
			}
		}

		private void SaveOutput()
		{
			int filterIndex = this.OutputSaveFileDialog.FilterIndex;

			if (filterIndex == PLAIN_TEXT_INDEX)
			{
				_outputBox.SavePlainTextOutput(this.OutputSaveFileDialog.FileName);
			}
			else if (filterIndex == RICH_TEXT_INDEX)
			{
				_outputBox.SaveRichTextOutput(this.OutputSaveFileDialog.FileName);
			}
		}

		private void WordWrap_Changed(bool checkValue)
		{
			this._mainMenu.WordWrapChecked = checkValue;
		}


		private void TargetsMenuCommand_Click(object sender, EventArgs e)
		{
			_dockManager.BringAutoHideIntoView(_targetsContent);
			_dockManager.ShowContent(_targetsContent);
		}

		private void PropertiesMenuCommand_Click(object sender, EventArgs e)
		{
			_dockManager.BringAutoHideIntoView(_propertiesContent);
			_dockManager.ShowContent(_propertiesContent);
		}

		private void Source_Changed()
		{
			if (!this.HasUnsavedAsterisk())
			{
				this.Text += "*";
			}
		}

		private void Source_Restored()
		{
			if (this.HasUnsavedAsterisk())
			{
				this.RemoveUnsavedFileAsterisk();
			}
		}

		private void MainForm_Closing(object sender, CancelEventArgs e)
		{
			_buildRunner.Stop();
			if (this._sourceTabs.SourceHasChanged)
			{
				DialogResult result =
					MessageBox.Show("You have unsaved changes.  Save?", "Save Changes?",
					                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

				if (result == DialogResult.Yes)
				{
					this._sourceTabs.SaveSource(_buildFile);
				}
				else if (result == DialogResult.Cancel)
				{
					e.Cancel = true;
				}
			}
		}

		private void SaveAsMenuCommand_Click(object sender, EventArgs e)
		{
			DialogResult result = this.XMLSaveFileDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				this._sourceTabs.SaveSource(this.XMLSaveFileDialog.FileName);

				this.RemoveUnsavedFileAsterisk();
				this.LoadBuildFile(this.XMLSaveFileDialog.FileName);
//				_buildFile = this.XMLSaveFileDialog.FileName;
			}
		}

		private void RemoveUnsavedFileAsterisk()
		{
			this.Text = this.Text.TrimEnd(new char[] {'*'});
		}

		private bool HasUnsavedAsterisk()
		{
			return this.Text.EndsWith("*");
		}

		public CommandLineOptions Options
		{
			set { _options = value; }
			get { return _options; }
		}
	}
}
