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
using NAntGui.Framework;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : Form, ILogsMessage
	{
		private delegate void MessageEventHandler(string pMessage);

		private CommandLineOptions _options;
		private RecentItems _recentItems = new RecentItems();
		private DockingManager _dockManager;
		private bool _firstLoad = true;

		#region GUI Items
		private OpenFileDialog OpenFileDialog;
		
		private SaveFileDialog XMLSaveFileDialog;
		
		private ImageList _imageList;

		private OutputBox _outputBox = new OutputBox();
		
		private TargetsTreeView _targetsTree;

		private WindowContent _targetWindowContent;
		private Content _targetsContent;
		private Content _propertiesContent;
		private Content _outputContent;

		public MainPropertyGrid _propertyGrid;
		private MainMenuControl _mainMenu = new MainMenuControl();
		private ToolBarControl _mainToolBar = new ToolBarControl();
		private SourceTabControl _sourceTabs = new SourceTabControl();
		private MainStatusBar _mainStatusBar = new MainStatusBar();

		private IContainer components;

		#endregion

		public MainForm(CommandLineOptions options)
		{
			_options = options;

			this.Initialize();
			this.SetupDockManager();

			_recentItems.ItemsUpdated += new VoidVoid(this.UpdateRecentItemsMenu);
			_recentItems.Load();

			this.LoadInitialBuildFile();
			this.AssignMenuCommands();
			this.AssignToolBarButtons();
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
			this.XMLSaveFileDialog = new SaveFileDialog();
			_propertyGrid = new MainPropertyGrid(_options.Properties);
			_imageList = ResourceHelper.LoadBitmapStrip(this.GetType(), "NAntGui.Core.Images.MenuItems.bmp",new Size(16, 16), new Point(0, 0));
			_targetsTree = new TargetsTreeView(new EventHandler(this.BuildMenuCommand_Click));
			this.SuspendLayout();
			// 
			// OpenFileDialog
			// 
			this.OpenFileDialog.DefaultExt = "build";
			this.OpenFileDialog.Filter = "Build Files (*.build)|*.build|NAnt Include|*.inc";
			// 
			// XMLSaveFileDialog
			// 
			this.XMLSaveFileDialog.DefaultExt = "build";
			this.XMLSaveFileDialog.Filter = "NAnt Buildfile|*.build|NAnt Include|*.inc";
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new Size(5, 13);
			this.ClientSize = new Size(824, 553);
			this.Controls.Add(_sourceTabs.Tabs);
			this.Controls.Add(_mainStatusBar);
			this.Controls.Add(_mainToolBar);
			this.Controls.Add(_mainMenu);
			
			this.Icon = ((Icon) (resources.GetObject("$this.Icon")));
			this.MinimumSize = new Size(480, 344);
			this.Name = "MainForm";
			this.Text = "NAnt-Gui";
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.Closing += new CancelEventHandler(this.MainForm_Closing);
			this.DragDrop += new DragEventHandler(this.NAnt_DragDrop);
			this.Closed += new EventHandler(this.NAnt_Closed);
			this.DragEnter += new DragEventHandler(this.NAnt_DragEnter);
			this.ResumeLayout(false);
		}

		#endregion

		private void AssignToolBarButtons()
		{
			_mainToolBar.Build_Click += new VoidVoid(this.Build);
			_mainToolBar.Open_Click += new VoidVoid(this.BrowseForBuildFile);
			_mainToolBar.Stop_Click += new VoidVoid(this.Stop_Click);
			_mainToolBar.Save_Click += new EventHandler(this.SaveMenuCommand_Click);
			_mainToolBar.Reload_Click += new EventHandler(this.Reload_Click);
		}

		private void AssignMenuCommands()
		{
			_mainMenu.Undo_Click = new EventHandler(this.Undo_Click);
			_mainMenu.Redo_Click = new EventHandler(this.Redo_Click);
			_mainMenu.Copy_Click = new EventHandler(_outputBox.CopyText);
			_mainMenu.SelectAll_Click = new EventHandler(_outputBox.SelectAllText);
			_mainMenu.SaveOutput_Click = new EventHandler(_outputBox.SaveOutput);
			_mainMenu.WordWrap_Click = new EventHandler(_outputBox.DoWordWrap);
			_mainMenu.Reload_Click = new EventHandler(this.Reload_Click);
			_mainMenu.About_Click = new EventHandler(this.AboutMenuCommand_Click);
			_mainMenu.Build_Click = new EventHandler(this.BuildMenuCommand_Click);
			_mainMenu.Close_Click = new EventHandler(this.CloseMenuCommand_Click);
			_mainMenu.Exit_Click = new EventHandler(this.ExitMenuCommand_Click);
			_mainMenu.NAntContrib_Click = new EventHandler(this.NAntContribMenuCommand_Click);
			_mainMenu.NAntHelp_Click = new EventHandler(this.NAntHelpMenuCommand_Click);
			_mainMenu.NAntSDK_Click = new EventHandler(this.NAntSDKMenuCommand_Click);
			_mainMenu.Open_Click = new EventHandler(this.OpenMenuCommand_Click);
			_mainMenu.Options_Click = new EventHandler(this.OptionsMenuCommand_Click);
			_mainMenu.Properties_Click = new EventHandler(this.PropertiesMenuCommand_Click);
			_mainMenu.Save_Click = new EventHandler(this.SaveMenuCommand_Click);
			_mainMenu.SaveAs_Click = new EventHandler(this.SaveAsMenuCommand_Click);
			_mainMenu.Targets_Click = new EventHandler(this.TargetsMenuCommand_Click);
			
		}

		private void SetupDockManager()
		{
			// Create the object that manages the docking state
			_dockManager = new DockingManager(this, VisualStyle.IDE);

			// Ensure that the RichTextBox is always the innermost control
			_dockManager.InnerControl = _sourceTabs.Tabs;

			_targetsContent		= _dockManager.Contents.Add(_targetsTree, "Targets");
			_outputContent		= _dockManager.Contents.Add(_outputBox, "Output");
			_propertiesContent	= _dockManager.Contents.Add(_propertyGrid, "Properties");

			_targetsContent.ImageList = _imageList;
			_targetsContent.ImageIndex = 9;

			_propertiesContent.ImageList = _imageList;
			_propertiesContent.ImageIndex = 0;

			// Request a new Docking window be created for the above Content on the left edge
			_targetWindowContent = _dockManager.AddContentWithState(_targetsContent, State.DockLeft) as WindowContent;
			_dockManager.AddContentToZone(_propertiesContent, _targetWindowContent.ParentZone, 1);

			_dockManager.AddContentWithState(_outputContent, State.DockBottom);

			_dockManager.OuterControl = _mainStatusBar;
		}

		private void LoadInitialBuildFile()
		{
			if (_options.BuildFile == null || !this.LoadBuildFile(_options.BuildFile))
			{
				if (!_recentItems.HasRecentItems || !this.LoadBuildFile(_recentItems[0]))
				{
					_sourceTabs.Clear();
					_sourceTabs.AddTab(new ScriptTabPage(this, _options));
				}
			}
		}

		private bool LoadBuildFile(string filename)
		{
			try
			{
				ScriptTabPage page = new ScriptTabPage(filename, this, _options);
				_sourceTabs.Clear();
				_sourceTabs.AddTab(page);
				this.BuildFileLoaded(page.BuildScript);
				page.BuildFinished = new VoidVoid(this.Update);
				page.SourceChanged += new VoidVoid(this.Source_Changed);
				return true;
			}
			catch (BuildFileNotFoundException error)
			{
				MessageBox.Show(error.Message, "Build File Not Found", 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			catch(BuildFileLoadException error)
			{
				MessageBox.Show(error.Message, "Error Loading Build File", 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				return false;
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
			_sourceTabs.SelectedTab.File.Close();
			_targetsTree.Nodes.Clear();
			_propertyGrid.SelectedObject = null;

			_outputBox.Clear();
			this.DisableMenuCommandsAndButtons();
		}

		private void BrowseForBuildFile()
		{
			this.OpenFileDialog.InitialDirectory = Settings.OpenInitialDirectory;
			if (this.OpenFileDialog.ShowDialog() == DialogResult.OK)
			{
				Settings.OpenInitialDirectory = this.OpenFileDialog.InitialDirectory;

				foreach (string filename in this.OpenFileDialog.FileNames)
				{
					this.LoadBuildFile(filename);	
				}
			}
		}

		private void BuildMenuCommand_Click(object sender, EventArgs e)
		{
			this.Build();
		}

		private void Build()
		{
			_outputBox.Clear();

			ScriptTabPage selectedTab = _sourceTabs.SelectedTab;

			selectedTab.SetProperties(_propertyGrid.GetProperties());
			selectedTab.SetTargets(_targetsTree.GetTargets());
			selectedTab.Run();
		}

		private void NAnt_Closed(object sender, EventArgs e)
		{
			_recentItems.Save();
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

		private void DisableMenuCommandsAndButtons()
		{
			_mainMenu.Disable();
			_mainToolBar.Disable();
		}

		private void EnableMenuCommandsAndButtons()
		{
			_mainMenu.Enable();
			_mainToolBar.Enable();
		}

		private void OptionsMenuCommand_Click(object sender, EventArgs e)
		{
			OptionsForm lOptionsForm = new OptionsForm();
			lOptionsForm.ShowDialog();
		}

		private void BuildFileLoaded(IBuildScript buildScript)
		{
			_outputBox.Clear();
			this.EnableMenuCommandsAndButtons();
			this.UpdateDisplay(buildScript);
			this.AddTargets(buildScript.Targets);
			_propertyGrid.AddProperties(buildScript.Properties, _firstLoad);

			_firstLoad = false;
		}

		private void UpdateDisplay(IBuildScript buildScript)
		{
			this.Text = string.Format("NAnt-Gui - {0}", _sourceTabs.SelectedTab.File.Name);

			string projectName = buildScript.HasName ? buildScript.Name : _sourceTabs.SelectedTab.File.Name;

			_recentItems.Add(_sourceTabs.SelectedTab.File.FullName);
			_recentItems.Save();
			this.UpdateRecentItemsMenu();

			_mainStatusBar.Panels[0].Text = string.Format("{0} ({1})", projectName, buildScript.Description);
			_mainStatusBar.Panels[1].Text = _sourceTabs.SelectedTab.File.FullName;

			_targetsTree.Nodes.Clear();
			_targetsTree.Nodes.Add(new TreeNode(projectName));
		}


		private void AddTargets(TargetCollection targets)
		{
			foreach (BuildTarget target in targets)
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

		private void AddTargetTreeNode(BuildTarget buildTarget)
		{
			if (!(Settings.HideTargetsWithoutDescription && !HasDescription(buildTarget.Description)))
			{
				string targetName = FormatTargetName(buildTarget.Name, buildTarget.Description);
				TreeNode node = new TreeNode(targetName);
				node.Checked = buildTarget.Default;
				node.Tag = buildTarget;
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
			_mainMenu.ClearRecentItems();

			int count = 1;
			foreach (string item in _recentItems)
			{
				EventHandler onClick = new EventHandler(this.RecentFile_Clicked);
				MenuCommand command = new MenuCommand(count++ + " " + item, onClick);
				_mainMenu.AddRecentItem(command);
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

		private void SaveMenuCommand_Click(object sender, EventArgs e)
		{
			_sourceTabs.SelectedTab.Save();
			this.Text = Utils.RemoveAsterisk(this.Text);
		}

//		private void WordWrap_Changed(bool checkValue)
//		{
//			_mainMenu.WordWrapChecked = checkValue;
//		}


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
			this.Text = "NAnt-Gui - " + _sourceTabs.SelectedTab.Title;
		}

		private void MainForm_Closing(object sender, CancelEventArgs e)
		{
			_sourceTabs.SelectedTab.Stop();
			_sourceTabs.CloseTabs(e);
		}

		private void SaveAsMenuCommand_Click(object sender, EventArgs e)
		{
			DialogResult result = this.XMLSaveFileDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				_sourceTabs.SelectedTab.SaveAs(this.XMLSaveFileDialog.FileName);

				this.Text = Utils.RemoveAsterisk(this.Text);
				this.LoadBuildFile(this.XMLSaveFileDialog.FileName);
			}
		}

		public CommandLineOptions Options
		{
			set { _options = value; }
			get { return _options; }
		}

		public PropertySort PropertySort
		{
			get { return _propertyGrid.PropertySort; }
			set { _propertyGrid.PropertySort = value; }
		}

		private void Reload_Click(object sender, EventArgs e)
		{
			_sourceTabs.SelectedTab.ReLoad();
		}

		private void Undo_Click(object sender, EventArgs e)
		{
			_sourceTabs.SelectedTab.Undo();
		}

		private void Redo_Click(object sender, EventArgs e)
		{
			_sourceTabs.SelectedTab.Redo();
		}

		private void Stop_Click()
		{
			_sourceTabs.SelectedTab.Stop();
		}
	}
}
