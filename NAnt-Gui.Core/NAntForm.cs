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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Menus;
using Flobbster.Windows.Forms;
using NAntGui.Core.NAnt;
using NC = NAnt.Core;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class NAntForm : Form
	{
		private const int RICH_TEXT_INDEX = 2;
		private const int PLAIN_TEXT_INDEX = 1;

		private delegate void MessageEventHandler(string pMessage);

		private BuildRunner _nantBuildRunner;
		private CommandLineOptions _options;
		private RecentItems _recentItems = new RecentItems();
		private bool _firstLoad = true;
		private DockingManager _dockManager;
		private string _buildFile;

		#region GUI Items

		private StatusBarPanel FileStatusBarPanel;
		private OpenFileDialog OpenFileDialog;
		private StatusBar MainStatusBar;
		
		private ImageList _imageList;

		private TreeView TargetsTreeView;
		private PopupMenu _targetsPopupMenu = new PopupMenu();
		private Content _targetsContent;
		
		private WindowContent _targetWindowContent;

		public PropertyGrid ProjectPropertyGrid;
		private PropertyTable _propertyTable = new PropertyTable();
		private Content _propertiesContent;

		private StatusBarPanel ProgressPanel;
		private ToolTip ToolTip;
		private StatusBarPanel FullFileStatusBarPanel;
		
		private MainMenuControl MainMenu;
		private ToolBarControl MainToolBar;

		private SourceTabControl SourceTabs;
		
		private SaveFileDialog OutputSaveFileDialog;
		private SaveFileDialog XMLSaveFileDialog;

		private IContainer components;

		#endregion

		public NAntForm(CommandLineOptions options)
		{
			_options = options;

			_imageList = ResourceHelper.LoadBitmapStrip(
				this.GetType(), "NAntGui.Core.Images.MenuItems.bmp",
				new Size(16, 16), new Point(0, 0));

			InitializeComponent();

			// Reduce the amount of flicker that occurs when windows are redocked within
			// the container. As this prevents unsightly backcolors being drawn in the
			// WM_ERASEBACKGROUND that seems to occur.
			this.SetStyle(ControlStyles.DoubleBuffer, true);
//			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			this.SetupDockManager();
			this.CreateTargetTreeViewMenu();

			_nantBuildRunner = new NAntBuildRunner(this);
			_nantBuildRunner.BuildFileLoaded += new BuildFileChangedEH(this.BuildFileLoaded);
			_nantBuildRunner.BuildFinished += new NC.BuildEventHandler(this.Build_Finished);

			this.AssignMenuCommands();
			this.AssignToolBarButtons();
			this.UpdateRecentItemsMenu();
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NAntForm));
			this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.MainStatusBar = new System.Windows.Forms.StatusBar();
			this.FileStatusBarPanel = new System.Windows.Forms.StatusBarPanel();
			this.FullFileStatusBarPanel = new System.Windows.Forms.StatusBarPanel();
			this.ProgressPanel = new System.Windows.Forms.StatusBarPanel();
			this.MainMenu = new NAntGui.Core.MainMenuControl();
			this.MainToolBar = new NAntGui.Core.ToolBarControl();
			this.TargetsTreeView = new System.Windows.Forms.TreeView();
			this.ProjectPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.OutputSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SourceTabs = new NAntGui.Core.SourceTabControl();
			this.XMLSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.FileStatusBarPanel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.FullFileStatusBarPanel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ProgressPanel)).BeginInit();
			this.SuspendLayout();
			// 
			// OpenFileDialog
			// 
			this.OpenFileDialog.DefaultExt = "build";
			this.OpenFileDialog.Filter = "Build Files (*.build)|*.build";
			// 
			// MainStatusBar
			// 
			this.MainStatusBar.Location = new System.Drawing.Point(0, 531);
			this.MainStatusBar.Name = "MainStatusBar";
			this.MainStatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																							 this.FileStatusBarPanel,
																							 this.FullFileStatusBarPanel,
																							 this.ProgressPanel});
			this.MainStatusBar.ShowPanels = true;
			this.MainStatusBar.Size = new System.Drawing.Size(824, 22);
			this.MainStatusBar.SizingGrip = false;
			this.MainStatusBar.TabIndex = 2;
			// 
			// FileStatusBarPanel
			// 
			this.FileStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.FileStatusBarPanel.Width = 10;
			// 
			// FullFileStatusBarPanel
			// 
			this.FullFileStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.FullFileStatusBarPanel.Width = 804;
			// 
			// ProgressPanel
			// 
			this.ProgressPanel.MinWidth = 0;
			this.ProgressPanel.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.ProgressPanel.Width = 10;
			// 
			// MainMenu
			// 
			this.MainMenu.AnimateStyle = Crownwood.Magic.Menus.Animation.System;
			this.MainMenu.AnimateTime = 100;
			this.MainMenu.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.MainMenu.Direction = Crownwood.Magic.Common.Direction.Horizontal;
			this.MainMenu.Dock = System.Windows.Forms.DockStyle.Top;
			this.MainMenu.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((System.Byte)(0)));
			this.MainMenu.HighlightTextColor = System.Drawing.SystemColors.MenuText;
			this.MainMenu.Location = new System.Drawing.Point(0, 0);
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.Size = new System.Drawing.Size(824, 25);
			this.MainMenu.Style = Crownwood.Magic.Common.VisualStyle.IDE;
			this.MainMenu.TabIndex = 13;
			this.MainMenu.TabStop = false;
			this.MainMenu.WordWrapChecked = false;
			// 
			// MainToolBar
			// 
			this.MainToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.MainToolBar.DropDownArrows = true;
			this.MainToolBar.Location = new System.Drawing.Point(0, 25);
			this.MainToolBar.Name = "MainToolBar";
			this.MainToolBar.ShowToolTips = true;
			this.MainToolBar.Size = new System.Drawing.Size(824, 28);
			this.MainToolBar.TabIndex = 4;
			// 
			// TargetsTreeView
			// 
			this.TargetsTreeView.CheckBoxes = true;
			this.TargetsTreeView.Dock = System.Windows.Forms.DockStyle.Top;
			this.TargetsTreeView.ImageIndex = -1;
			this.TargetsTreeView.Location = new System.Drawing.Point(0, 0);
			this.TargetsTreeView.Name = "TargetsTreeView";
			this.TargetsTreeView.SelectedImageIndex = -1;
			this.TargetsTreeView.Size = new System.Drawing.Size(175, 148);
			this.TargetsTreeView.TabIndex = 6;
			this.TargetsTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.BuildTreeView_AfterCheck);
			this.TargetsTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BuildTreeView_MouseUp);
			this.TargetsTreeView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BuildTreeView_MouseMove);
			// 
			// ProjectPropertyGrid
			// 
			this.ProjectPropertyGrid.CommandsVisibleIfAvailable = true;
			this.ProjectPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ProjectPropertyGrid.LargeButtons = false;
			this.ProjectPropertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.ProjectPropertyGrid.Location = new System.Drawing.Point(0, 252);
			this.ProjectPropertyGrid.Name = "ProjectPropertyGrid";
			this.ProjectPropertyGrid.Size = new System.Drawing.Size(175, 351);
			this.ProjectPropertyGrid.TabIndex = 4;
			this.ProjectPropertyGrid.Text = "Build Properties";
			this.ProjectPropertyGrid.ViewBackColor = System.Drawing.SystemColors.Window;
			this.ProjectPropertyGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// OutputSaveFileDialog
			// 
			this.OutputSaveFileDialog.DefaultExt = "txt";
			this.OutputSaveFileDialog.FileName = "Output";
			this.OutputSaveFileDialog.Filter = "Text Document|*.txt|Rich Text Format (RTF)|*.rtf";
			// 
			// SourceTabs
			// 
			this.SourceTabs.Appearance = Crownwood.Magic.Controls.TabControl.VisualAppearance.MultiDocument;
			this.SourceTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SourceTabs.IDEPixelArea = true;
			this.SourceTabs.IDEPixelBorder = false;
			this.SourceTabs.Location = new System.Drawing.Point(0, 53);
			this.SourceTabs.Name = "SourceTabs";
			this.SourceTabs.SelectedIndex = 0;
			this.SourceTabs.Size = new System.Drawing.Size(824, 478);
			this.SourceTabs.TabIndex = 12;
			this.SourceTabs.WordWrapChanged += new NAntGui.Core.VoidBool(this.WordWrap_Changed);
			this.SourceTabs.SourceRestored += new NAntGui.Core.VoidVoid(this.Source_Restored);
			this.SourceTabs.SourceChanged += new NAntGui.Core.VoidVoid(this.Source_Changed);
			// 
			// XMLSaveFileDialog
			// 
			this.XMLSaveFileDialog.DefaultExt = "build";
			this.XMLSaveFileDialog.Filter = "NAnt Buildfile|*.build|NAnt Include|*.inc";
			// 
			// NAntForm
			// 
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(824, 553);
			this.Controls.Add(this.SourceTabs);
			this.Controls.Add(this.MainStatusBar);
			this.Controls.Add(this.MainToolBar);
			this.Controls.Add(this.MainMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(480, 344);
			this.Name = "NAntForm";
			this.Text = "NAnt-Gui";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.NAntForm_Closing);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.NAnt_DragDrop);
			this.Closed += new System.EventHandler(this.NAnt_Closed);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.NAnt_DragEnter);
			((System.ComponentModel.ISupportInitialize)(this.FileStatusBarPanel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.FullFileStatusBarPanel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ProgressPanel)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private void AssignToolBarButtons()
		{
			this.MainToolBar.Build_Click	+= new VoidVoid(this.Build);
			this.MainToolBar.Open_Click		+= new VoidVoid(this.BrowseForBuildFile);
			this.MainToolBar.Save_Click		+= new VoidVoid(this.Save);
			this.MainToolBar.Reload_Click	+= new VoidVoid(this.Reload);
			this.MainToolBar.Stop_Click		+= new VoidVoid(_nantBuildRunner.Stop);
		}

		private void AssignMenuCommands()
		{
			this.MainMenu.About_Click		= new EventHandler(this.AboutMenuCommand_Click);
			this.MainMenu.Build_Click		= new EventHandler(this.BuildMenuCommand_Click);
			this.MainMenu.Close_Click		= new EventHandler(this.CloseMenuCommand_Click);
			this.MainMenu.Copy_Click		= new EventHandler(this.SourceTabs.CopyText);
			this.MainMenu.Exit_Click		= new EventHandler(this.ExitMenuCommand_Click);
			this.MainMenu.NAntContrib_Click = new EventHandler(this.NAntContribMenuCommand_Click);
			this.MainMenu.NAntHelp_Click	= new EventHandler(this.NAntHelpMenuCommand_Click);
			this.MainMenu.NAntSDK_Click		= new EventHandler(this.NAntSDKMenuCommand_Click);
			this.MainMenu.Open_Click		= new EventHandler(this.OpenMenuCommand_Click);
			this.MainMenu.Options_Click		= new EventHandler(this.OptionsMenuCommand_Click);
			this.MainMenu.Properties_Click	= new EventHandler(this.PropertiesMenuCommand_Click);
			this.MainMenu.Reload_Click		= new EventHandler(this.ReloadMenuCommand_Click);
			this.MainMenu.SelectAll_Click	= new EventHandler(this.SourceTabs.SelectAllText);
			this.MainMenu.SaveOutput_Click	= new EventHandler(this.SaveOutputMenuCommand_Click);
			this.MainMenu.Save_Click		= new EventHandler(this.SaveMenuCommand_Click);
			this.MainMenu.SaveAs_Click		= new EventHandler(this.SaveAsMenuCommand_Click);
			this.MainMenu.Targets_Click		= new EventHandler(this.TargetsMenuCommand_Click);
			this.MainMenu.WordWrap_Click	= new EventHandler(this.SourceTabs.WordWrap);
		}


		private void SetupDockManager()
		{
			// Create the object that manages the docking state
			_dockManager = new DockingManager(this, VisualStyle.IDE);

			// Ensure that the RichTextBox is always the innermost control
			_dockManager.InnerControl = this.SourceTabs;

			// Create Content which contains a RichTextBox
			_targetsContent = _dockManager.Contents.Add(this.TargetsTreeView, "Targets");

			_propertiesContent = _dockManager.Contents.Add(this.ProjectPropertyGrid, "Properties");
			_propertiesContent.ImageList = this._imageList;
			_propertiesContent.ImageIndex = 0;

			// Request a new Docking window be created for the above Content on the left edge
			_targetWindowContent = _dockManager.AddContentWithState(_targetsContent, State.DockLeft) as WindowContent;

			_dockManager.AddContentToZone(_propertiesContent, _targetWindowContent.ParentZone, 1);

			//_dockManager.OuterControl = this.MainStatusBar;
			_dockManager.OuterControl = this.MainToolBar;
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
			catch (BuildFileNotFoundException)
			{
				MessageBox.Show("Build file: '" + _options.BuildFile + "' does not exist.",
					"Build File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		private void LoadBuildFile(string buildFile)
		{
			_buildFile = buildFile;
			_nantBuildRunner.LoadBuildFile(buildFile);
		}

		private void LoadRecentBuildFile()
		{
			if (_recentItems.HasRecentItems)
			{
				this.LoadBuildFile(_recentItems[0]);
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
			_nantBuildRunner.Run(_buildFile);
		}

		private void ClearOutput()
		{
			this.SourceTabs.Clear();
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

		private void BuildTreeView_AfterCheck(object sender, TreeViewEventArgs e)
		{
			foreach (TreeNode lNode in e.Node.Nodes)
			{
				lNode.Checked = e.Node.Checked;
			}
		}

		private void AboutMenuCommand_Click(object sender, EventArgs e)
		{
			About lAbout = new About();
			lAbout.ShowDialog();
		}

		public void OutputMessage(string message)
		{
			if (this.InvokeRequired)
			{
				MessageEventHandler messageEH =
					new MessageEventHandler(this.SourceTabs.WriteOutput);

				this.BeginInvoke(messageEH, new Object[1] {message});
			}
			else
			{
				this.SourceTabs.WriteOutput(message);
			}
		}

		public void Build_Finished(object sender, NC.BuildEventArgs e)
		{
			this.Update();
		}

		public void CloseBuildFile()
		{
//			_buildFile.Close();
			_buildFile = "";
			this.ClearOutput();
			this.TargetsTreeView.Nodes.Clear();
			this.ProjectPropertyGrid.SelectedObject = null;

			this.DisableMenuCommandsAndButtons();
		}

		private void DisableMenuCommandsAndButtons()
		{
			this.MainMenu.Disable();
			this.MainToolBar.Disable();
		}

		private void EnableMenuCommandsAndButtons()
		{
			this.MainMenu.Enable();
			this.MainToolBar.Enable();
		}

		private void OptionsMenuCommand_Click(object sender, EventArgs e)
		{
			OptionsForm lOptionsForm = new OptionsForm();
			lOptionsForm.ShowDialog();
		}

		private void BuildFileLoaded(Project project)
		{
			this.ClearOutput();
			this.EnableMenuCommandsAndButtons();
			this.UpdateDisplay(project);
			this.AddTargets(project);
			this.AddProperties(project);

			_firstLoad = false;
		}

		private void UpdateDisplay(Project project)
		{
			this.SourceTabs.LoadSource(_buildFile);

			string filename = new FileInfo(_buildFile).Name;

			this.Text = "NAnt-Gui" + string.Format(" - {0}", filename);

			string projectName = project.HasName ? project.Name : filename;

			_recentItems.Add(_buildFile);
			_recentItems.Save();
			this.UpdateRecentItemsMenu();

			this.MainStatusBar.Panels[0].Text = string.Format("{0} ({1})", projectName, project.Description);
			this.MainStatusBar.Panels[1].Text = _buildFile;

			this.TargetsTreeView.Nodes.Clear();
			this.TargetsTreeView.Nodes.Add(new TreeNode(projectName));
		}


		private void AddTargets(Project project)
		{
			foreach (Target target in project.Targets)
			{
				this.AddTargetTreeNode(target);
			}

			this.TargetsTreeView.ExpandAll();
		}

		private void AddProperties(Project project)
		{
			_propertyTable.Properties.Clear();

			foreach (Property property in project.Properties)
			{
				PropertySpec spec = new PropertySpec(property.Name, property.Type,
				                                     property.Category, property.ExpandedValue, property.Value);

				if (property.IsReadOnly)
				{
					spec.Attributes = new Attribute[] {ReadOnlyAttribute.Yes};
				}

				_propertyTable.Properties.Add(spec);

				if (_firstLoad && cmdPropsContains(spec.Name))
				{
					_propertyTable[GetKey(spec)] = _options.Properties[spec.Name];
				}
				else
				{
					_propertyTable[GetKey(spec)] = property.Value;
				}
			}

			this.ProjectPropertyGrid.SelectedObject = _propertyTable;
		}

		private bool cmdPropsContains(string name)
		{
			foreach (string key in _options.Properties.AllKeys)
				if (key == name)
					return true;

			return false;
		}

		public void AddTreeTargetsToBuild(NC.Project project)
		{
			foreach (TreeNode lItem in this.TargetsTreeView.Nodes[0].Nodes)
			{
				if (lItem.Checked)
				{
					project.BuildTargets.Add(lItem.Text);
				}
			}
		}

		public void AddPropertiesToProject(NC.Project project)
		{
			foreach (PropertySpec spec in _propertyTable.Properties)
			{
				if (spec.Category == "Project")
				{
					project.BaseDirectory = _propertyTable[GetKey(spec)].ToString();
				}
				else if (spec.Category == "Global" || ValidTarget(spec.Category, project))
				{
					this.LoadNonProjectProperty(spec, project);
				}
			}
		}

		private void LoadNonProjectProperty(PropertySpec spec, NC.Project project)
		{
			string lValue = _propertyTable[GetKey(spec)].ToString();
			string lExpandedProperty = lValue;
			try
			{
				lExpandedProperty = project.ExpandProperties(lValue,
				                                             new NC.Location(_buildFile));
			}
			catch (NC.BuildException)
			{ /* ignore */
			}

			project.Properties.AddReadOnly(spec.Name, lExpandedProperty);
		}

		private bool ValidTarget(string category, NC.Project project)
		{
			return project.BuildTargets.Contains(category);
		}

		private static string GetKey(PropertySpec pSpec)
		{
			return string.Format("{0}.{1}", pSpec.Category, pSpec.Name);
		}

		private void AddTargetTreeNode(Target target)
		{
			if (!(Settings.HideTargetsWithoutDescription && !HasDescription(target.Description)))
			{
				string targetName = FormatTargetName(target.Name, target.Description);
				TreeNode node = new TreeNode(targetName);
				node.Checked = target.Default;
				node.Tag = target;
				this.TargetsTreeView.Nodes[0].Nodes.Add(node);
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

		private void BuildTreeView_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TreeView tree = sender as TreeView;
				TreeNode node = tree.GetNodeAt(e.X, e.Y);
				if (node != null)
				{
					_targetsPopupMenu.TrackPopup(tree.PointToScreen(new Point(e.X, e.Y)));
				}
			}
		}

		private void BuildTreeView_MouseMove(object sender, MouseEventArgs e)
		{
			TreeNode node = this.TargetsTreeView.GetNodeAt(e.X, e.Y);
			if (node == null || node.Parent == null)
			{
				this.ToolTip.SetToolTip(this.TargetsTreeView, "");
			}
			else
			{
				Target target = (Target) node.Tag;
				this.ToolTip.SetToolTip(this.TargetsTreeView, target.Description);
			}
		}

		private void UpdateRecentItemsMenu()
		{
			this.MainMenu.ClearRecentItems();

			int count = 1;
			foreach (string item in _recentItems)
			{
				EventHandler onClick = new EventHandler(this.RecentFile_Clicked);
				MenuCommand command = new MenuCommand(count++ + " " + item, onClick);
				this.MainMenu.AddRecentItem(command);
			}
		}

		private void RecentFile_Clicked(object sender, EventArgs e)
		{
			MenuCommand lItem = (MenuCommand) sender;
			this.LoadBuildFile(lItem.Text.Substring(2));
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
				this.SourceTabs.SaveSource(_buildFile);
				this.Text = this.Text.TrimEnd(new char[] {'*'});
			}
		}

		private void SaveOutput()
		{
			int filterIndex = this.OutputSaveFileDialog.FilterIndex;

			if (filterIndex == PLAIN_TEXT_INDEX)
			{
				this.SourceTabs.SavePlainTextOutput(this.OutputSaveFileDialog.FileName);
			}
			else if (filterIndex == RICH_TEXT_INDEX)
			{
				this.SourceTabs.SaveRichTextOutput(this.OutputSaveFileDialog.FileName);
			}
		}

		private void WordWrap_Changed(bool checkValue)
		{
			this.MainMenu.WordWrapChecked = checkValue;
		}

		private void CreateTargetTreeViewMenu()
		{
			MenuCommand build = new MenuCommand("&Build", new EventHandler(this.BuildMenuCommand_Click));
			build.ImageList = this._imageList;
			build.ImageIndex = 7;
			_targetsPopupMenu.MenuCommands.AddRange(new MenuCommand[] {build});
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

		private void NAntForm_Closing(object sender, CancelEventArgs e)
		{
			if (this.SourceTabs.SourceHasChanged)
			{
				DialogResult result =
					MessageBox.Show("You have unsaved changes.  Save?", "Save Changes?",
					                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

				if (result == DialogResult.Yes)
				{
					this.SourceTabs.SaveSource(_buildFile);
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
				this.SourceTabs.SaveSource(this.XMLSaveFileDialog.FileName);

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