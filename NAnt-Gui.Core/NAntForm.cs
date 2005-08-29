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
// Colin Svingen (csvingen@businesswatch.ca)

#endregion

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Flobbster.Windows.Forms;
using NAnt.Core;

using NProject = NAnt.Core.Project;

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

		private Core _core;
		private CommandLineOptions _options;
		private RecentItems _recentItems = new RecentItems();
		private bool _firstLoad = true;

		private MenuItem ExitMenuItem;
		private MenuItem menuItem4;
		private MenuItem menuItem5;
		private StatusBarPanel FileStatusBarPanel;
		private OpenFileDialog OpenFileDialog;
		private StatusBar MainStatusBar;
		private MainMenu MainMenu;
		private RichTextBox OutputTextBox;
		private MenuItem FileMenuItem;
		private MenuItem OpenMenuItem;
		private MenuItem CloseMenuItem;
		private MenuItem RecentMenuItem;
		private ToolBar MainToolBar;
		private ImageList ToolBarImageList;
		private MenuItem ReloadMenuItem;
		public TreeView BuildTreeView;
		private Panel RightPanel;
		public Panel LeftPanel;
		private Splitter HorzSplitter;
		public System.Windows.Forms.PropertyGrid ProjectPropertyGrid;
		private Splitter VertSplitter;
		private MenuItem HelpMenuItem;
		private MenuItem AboutMenuItem;
		private MenuItem BuildMenuItem;
		private MenuItem RunMenuItem;
		private StatusBarPanel ProgressPanel;
		private MenuItem EditMainMenuItem;
		private MenuItem CopyMenuItem;
		private MenuItem SelectAllMenuItem;
		private ContextMenu OutputTextBoxContextMenu;
		private MenuItem CopyContextMenuItem;
		private MenuItem SelectAllContextMenuItem;
		private System.Windows.Forms.MenuItem ToolsMenuItem;
		private System.Windows.Forms.MenuItem OptionsMenuItem;
		private System.Windows.Forms.SaveFileDialog SaveFileDialog;
		private System.Windows.Forms.ContextMenu TreeContextMenu;
		public System.Windows.Forms.ToolTip ToolTip;
		private System.Windows.Forms.MenuItem RunContextMenuItem;
		private System.Windows.Forms.StatusBarPanel ullFileStatusBarPanel;
		private IContainer components;
		private System.Windows.Forms.ToolBarButton OpenToolBarButton;
		private System.Windows.Forms.ToolBarButton BuildToolBarButton;
		private System.Windows.Forms.ToolBarButton ReloadToolBarButton;
		private System.Windows.Forms.ToolBarButton StopToolBarButton;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage OutputTabPage;
		private System.Windows.Forms.TabPage XMLTabPage;
		private PropertyTable _propertyTable = new PropertyTable();
		private System.Windows.Forms.RichTextBox XMLRichTextBox;
		private System.Windows.Forms.MenuItem NAntContribMenuItem;
		private System.Windows.Forms.MenuItem NAntHelpMenuItem;
		private System.Windows.Forms.MenuItem NAntSDKMenuItem;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem SaveOutputMenuItem;
		private System.Windows.Forms.MenuItem WordWrapMenuItem;
		private string _buildFile;

		public NAntForm(CommandLineOptions options)
		{
			InitializeComponent();

			_core = new Core(this);
			_core.BuildFileChanged += new BuildFileChangedEH(this.BuildFileLoaded);
			_core.BuildFinished += new BuildEventHandler(this.Build_Finished);
			_options = options;
			
			this.UpdateRecentItemsMenu();
			this.LoadInitialBuildFile();
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
			_core.LoadBuildFile(buildFile);
		}

		private void LoadRecentBuildFile()
		{
			if (_recentItems.HasRecentItems)
			{
				this.LoadBuildFile(_recentItems[0]);
			}
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
			this.ullFileStatusBarPanel = new System.Windows.Forms.StatusBarPanel();
			this.ProgressPanel = new System.Windows.Forms.StatusBarPanel();
			this.MainMenu = new System.Windows.Forms.MainMenu();
			this.FileMenuItem = new System.Windows.Forms.MenuItem();
			this.OpenMenuItem = new System.Windows.Forms.MenuItem();
			this.ReloadMenuItem = new System.Windows.Forms.MenuItem();
			this.CloseMenuItem = new System.Windows.Forms.MenuItem();
			this.SaveOutputMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.RecentMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.ExitMenuItem = new System.Windows.Forms.MenuItem();
			this.EditMainMenuItem = new System.Windows.Forms.MenuItem();
			this.CopyMenuItem = new System.Windows.Forms.MenuItem();
			this.SelectAllMenuItem = new System.Windows.Forms.MenuItem();
			this.BuildMenuItem = new System.Windows.Forms.MenuItem();
			this.RunMenuItem = new System.Windows.Forms.MenuItem();
			this.ToolsMenuItem = new System.Windows.Forms.MenuItem();
			this.OptionsMenuItem = new System.Windows.Forms.MenuItem();
			this.HelpMenuItem = new System.Windows.Forms.MenuItem();
			this.NAntHelpMenuItem = new System.Windows.Forms.MenuItem();
			this.NAntSDKMenuItem = new System.Windows.Forms.MenuItem();
			this.NAntContribMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.AboutMenuItem = new System.Windows.Forms.MenuItem();
			this.OutputTextBox = new System.Windows.Forms.RichTextBox();
			this.OutputTextBoxContextMenu = new System.Windows.Forms.ContextMenu();
			this.CopyContextMenuItem = new System.Windows.Forms.MenuItem();
			this.SelectAllContextMenuItem = new System.Windows.Forms.MenuItem();
			this.MainToolBar = new System.Windows.Forms.ToolBar();
			this.OpenToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.ReloadToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.BuildToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.StopToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.ToolBarImageList = new System.Windows.Forms.ImageList(this.components);
			this.BuildTreeView = new System.Windows.Forms.TreeView();
			this.RightPanel = new System.Windows.Forms.Panel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.OutputTabPage = new System.Windows.Forms.TabPage();
			this.XMLTabPage = new System.Windows.Forms.TabPage();
			this.XMLRichTextBox = new System.Windows.Forms.RichTextBox();
			this.ProjectPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.LeftPanel = new System.Windows.Forms.Panel();
			this.HorzSplitter = new System.Windows.Forms.Splitter();
			this.VertSplitter = new System.Windows.Forms.Splitter();
			this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.TreeContextMenu = new System.Windows.Forms.ContextMenu();
			this.RunContextMenuItem = new System.Windows.Forms.MenuItem();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.WordWrapMenuItem = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.FileStatusBarPanel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ullFileStatusBarPanel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ProgressPanel)).BeginInit();
			this.RightPanel.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.OutputTabPage.SuspendLayout();
			this.XMLTabPage.SuspendLayout();
			this.LeftPanel.SuspendLayout();
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
																							 this.ullFileStatusBarPanel,
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
			// ullFileStatusBarPanel
			// 
			this.ullFileStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.ullFileStatusBarPanel.Width = 804;
			// 
			// ProgressPanel
			// 
			this.ProgressPanel.MinWidth = 0;
			this.ProgressPanel.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.ProgressPanel.Width = 10;
			// 
			// MainMenu
			// 
			this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.FileMenuItem,
																					 this.EditMainMenuItem,
																					 this.BuildMenuItem,
																					 this.ToolsMenuItem,
																					 this.HelpMenuItem});
			// 
			// FileMenuItem
			// 
			this.FileMenuItem.Index = 0;
			this.FileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.OpenMenuItem,
																						 this.ReloadMenuItem,
																						 this.CloseMenuItem,
																						 this.SaveOutputMenuItem,
																						 this.menuItem4,
																						 this.RecentMenuItem,
																						 this.menuItem5,
																						 this.ExitMenuItem});
			this.FileMenuItem.Text = "&File";
			// 
			// OpenMenuItem
			// 
			this.OpenMenuItem.Index = 0;
			this.OpenMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.OpenMenuItem.Text = "&Open";
			this.OpenMenuItem.Click += new System.EventHandler(this.BrowseButton_Click);
			// 
			// ReloadMenuItem
			// 
			this.ReloadMenuItem.Index = 1;
			this.ReloadMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
			this.ReloadMenuItem.Text = "&Reload";
			this.ReloadMenuItem.Click += new System.EventHandler(this.ReloadMenuItem_Click);
			// 
			// CloseMenuItem
			// 
			this.CloseMenuItem.Index = 2;
			this.CloseMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlW;
			this.CloseMenuItem.Text = "&Close";
			this.CloseMenuItem.Click += new System.EventHandler(this.CloseMenuItem_Click);
			// 
			// SaveOutputMenuItem
			// 
			this.SaveOutputMenuItem.Index = 3;
			this.SaveOutputMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.SaveOutputMenuItem.Text = "&Save Output";
			this.SaveOutputMenuItem.Click += new System.EventHandler(this.SaveOutputMenuItem_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 4;
			this.menuItem4.Text = "-";
			// 
			// RecentMenuItem
			// 
			this.RecentMenuItem.Index = 5;
			this.RecentMenuItem.Text = "Recent &Files";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 6;
			this.menuItem5.Text = "-";
			// 
			// ExitMenuItem
			// 
			this.ExitMenuItem.Index = 7;
			this.ExitMenuItem.Text = "E&xit";
			this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
			// 
			// EditMainMenuItem
			// 
			this.EditMainMenuItem.Index = 1;
			this.EditMainMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							 this.CopyMenuItem,
																							 this.SelectAllMenuItem,
																							 this.WordWrapMenuItem});
			this.EditMainMenuItem.Text = "&Edit";
			// 
			// CopyMenuItem
			// 
			this.CopyMenuItem.Index = 0;
			this.CopyMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.CopyMenuItem.Text = "&Copy";
			this.CopyMenuItem.Click += new System.EventHandler(this.CopyMenuItem_Click);
			// 
			// SelectAllMenuItem
			// 
			this.SelectAllMenuItem.Index = 1;
			this.SelectAllMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			this.SelectAllMenuItem.Text = "Select &All";
			this.SelectAllMenuItem.Click += new System.EventHandler(this.SelectAllMenuItem_Click);
			// 
			// BuildMenuItem
			// 
			this.BuildMenuItem.Index = 2;
			this.BuildMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.RunMenuItem});
			this.BuildMenuItem.Text = "&NAnt";
			// 
			// RunMenuItem
			// 
			this.RunMenuItem.Index = 0;
			this.RunMenuItem.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.RunMenuItem.Text = "&Build";
			this.RunMenuItem.Click += new System.EventHandler(this.RunMenuItem_Click);
			// 
			// ToolsMenuItem
			// 
			this.ToolsMenuItem.Index = 3;
			this.ToolsMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						  this.OptionsMenuItem});
			this.ToolsMenuItem.Text = "&Tools";
			// 
			// OptionsMenuItem
			// 
			this.OptionsMenuItem.Index = 0;
			this.OptionsMenuItem.Text = "&Options";
			this.OptionsMenuItem.Click += new System.EventHandler(this.OptionsMenuItem_Click);
			// 
			// HelpMenuItem
			// 
			this.HelpMenuItem.Index = 4;
			this.HelpMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.NAntHelpMenuItem,
																						 this.NAntSDKMenuItem,
																						 this.NAntContribMenuItem,
																						 this.menuItem1,
																						 this.AboutMenuItem});
			this.HelpMenuItem.Text = "&Help";
			// 
			// NAntHelpMenuItem
			// 
			this.NAntHelpMenuItem.Index = 0;
			this.NAntHelpMenuItem.Text = "NAnt &Help";
			this.NAntHelpMenuItem.Click += new System.EventHandler(this.NAntHelpMenuItem_Click);
			// 
			// NAntSDKMenuItem
			// 
			this.NAntSDKMenuItem.Index = 1;
			this.NAntSDKMenuItem.Text = "NAnt &SDK Help";
			this.NAntSDKMenuItem.Click += new System.EventHandler(this.NAntSDKMenuItem_Click);
			// 
			// NAntContribMenuItem
			// 
			this.NAntContribMenuItem.Index = 2;
			this.NAntContribMenuItem.Text = "NAnt-&Contrib Help";
			this.NAntContribMenuItem.Click += new System.EventHandler(this.NAntContribMenuItem_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 3;
			this.menuItem1.Text = "-";
			// 
			// AboutMenuItem
			// 
			this.AboutMenuItem.Index = 4;
			this.AboutMenuItem.Text = "&About NAnt-Gui";
			this.AboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
			// 
			// OutputTextBox
			// 
			this.OutputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.OutputTextBox.ContextMenu = this.OutputTextBoxContextMenu;
			this.OutputTextBox.DetectUrls = false;
			this.OutputTextBox.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.OutputTextBox.Location = new System.Drawing.Point(0, 0);
			this.OutputTextBox.Name = "OutputTextBox";
			this.OutputTextBox.ReadOnly = true;
			this.OutputTextBox.Size = new System.Drawing.Size(640, 480);
			this.OutputTextBox.TabIndex = 3;
			this.OutputTextBox.Text = "";
			this.OutputTextBox.WordWrap = false;
			// 
			// OutputTextBoxContextMenu
			// 
			this.OutputTextBoxContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																									 this.CopyContextMenuItem,
																									 this.SelectAllContextMenuItem});
			// 
			// CopyContextMenuItem
			// 
			this.CopyContextMenuItem.Index = 0;
			this.CopyContextMenuItem.Text = "&Copy";
			this.CopyContextMenuItem.Click += new System.EventHandler(this.CopyMenuItem_Click);
			// 
			// SelectAllContextMenuItem
			// 
			this.SelectAllContextMenuItem.Index = 1;
			this.SelectAllContextMenuItem.Text = "Select &All";
			this.SelectAllContextMenuItem.Click += new System.EventHandler(this.SelectAllMenuItem_Click);
			// 
			// MainToolBar
			// 
			this.MainToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						   this.OpenToolBarButton,
																						   this.ReloadToolBarButton,
																						   this.BuildToolBarButton,
																						   this.StopToolBarButton});
			this.MainToolBar.DropDownArrows = true;
			this.MainToolBar.ImageList = this.ToolBarImageList;
			this.MainToolBar.Location = new System.Drawing.Point(0, 0);
			this.MainToolBar.Name = "MainToolBar";
			this.MainToolBar.ShowToolTips = true;
			this.MainToolBar.Size = new System.Drawing.Size(824, 28);
			this.MainToolBar.TabIndex = 4;
			this.MainToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.MainToolBar_ButtonClick);
			// 
			// OpenToolBarButton
			// 
			this.OpenToolBarButton.ImageIndex = 0;
			this.OpenToolBarButton.ToolTipText = "Open Build File";
			// 
			// ReloadToolBarButton
			// 
			this.ReloadToolBarButton.ImageIndex = 1;
			this.ReloadToolBarButton.ToolTipText = "Reload Build File";
			// 
			// BuildToolBarButton
			// 
			this.BuildToolBarButton.ImageIndex = 2;
			this.BuildToolBarButton.ToolTipText = "Build Default Target";
			// 
			// StopToolBarButton
			// 
			this.StopToolBarButton.Enabled = false;
			this.StopToolBarButton.ImageIndex = 3;
			// 
			// ToolBarImageList
			// 
			this.ToolBarImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.ToolBarImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.ToolBarImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ToolBarImageList.ImageStream")));
			this.ToolBarImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// BuildTreeView
			// 
			this.BuildTreeView.CheckBoxes = true;
			this.BuildTreeView.Dock = System.Windows.Forms.DockStyle.Top;
			this.BuildTreeView.ImageIndex = -1;
			this.BuildTreeView.Location = new System.Drawing.Point(0, 0);
			this.BuildTreeView.Name = "BuildTreeView";
			this.BuildTreeView.SelectedImageIndex = -1;
			this.BuildTreeView.Size = new System.Drawing.Size(175, 248);
			this.BuildTreeView.TabIndex = 6;
			this.BuildTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BuildTreeView_MouseDown);
			this.BuildTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.BuildTreeView_AfterCheck);
			this.BuildTreeView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BuildTreeView_MouseMove);
			// 
			// RightPanel
			// 
			this.RightPanel.Controls.Add(this.tabControl1);
			this.RightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RightPanel.Location = new System.Drawing.Point(178, 28);
			this.RightPanel.Name = "RightPanel";
			this.RightPanel.Size = new System.Drawing.Size(646, 503);
			this.RightPanel.TabIndex = 9;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.OutputTabPage);
			this.tabControl1.Controls.Add(this.XMLTabPage);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(648, 504);
			this.tabControl1.TabIndex = 4;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// OutputTabPage
			// 
			this.OutputTabPage.Controls.Add(this.OutputTextBox);
			this.OutputTabPage.Location = new System.Drawing.Point(4, 22);
			this.OutputTabPage.Name = "OutputTabPage";
			this.OutputTabPage.Size = new System.Drawing.Size(640, 478);
			this.OutputTabPage.TabIndex = 0;
			this.OutputTabPage.Text = "Output";
			// 
			// XMLTabPage
			// 
			this.XMLTabPage.Controls.Add(this.XMLRichTextBox);
			this.XMLTabPage.Location = new System.Drawing.Point(4, 22);
			this.XMLTabPage.Name = "XMLTabPage";
			this.XMLTabPage.Size = new System.Drawing.Size(640, 478);
			this.XMLTabPage.TabIndex = 1;
			this.XMLTabPage.Text = "XML";
			// 
			// XMLRichTextBox
			// 
			this.XMLRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.XMLRichTextBox.ContextMenu = this.OutputTextBoxContextMenu;
			this.XMLRichTextBox.DetectUrls = false;
			this.XMLRichTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.XMLRichTextBox.Location = new System.Drawing.Point(0, -1);
			this.XMLRichTextBox.Name = "XMLRichTextBox";
			this.XMLRichTextBox.ReadOnly = true;
			this.XMLRichTextBox.Size = new System.Drawing.Size(640, 480);
			this.XMLRichTextBox.TabIndex = 4;
			this.XMLRichTextBox.Text = "";
			this.XMLRichTextBox.WordWrap = false;
			// 
			// ProjectPropertyGrid
			// 
			this.ProjectPropertyGrid.CommandsVisibleIfAvailable = true;
			this.ProjectPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ProjectPropertyGrid.LargeButtons = false;
			this.ProjectPropertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.ProjectPropertyGrid.Location = new System.Drawing.Point(0, 252);
			this.ProjectPropertyGrid.Name = "ProjectPropertyGrid";
			this.ProjectPropertyGrid.Size = new System.Drawing.Size(175, 251);
			this.ProjectPropertyGrid.TabIndex = 4;
			this.ProjectPropertyGrid.Text = "Build Properties";
			this.ProjectPropertyGrid.ViewBackColor = System.Drawing.SystemColors.Window;
			this.ProjectPropertyGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// LeftPanel
			// 
			this.LeftPanel.Controls.Add(this.ProjectPropertyGrid);
			this.LeftPanel.Controls.Add(this.HorzSplitter);
			this.LeftPanel.Controls.Add(this.BuildTreeView);
			this.LeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.LeftPanel.Location = new System.Drawing.Point(0, 28);
			this.LeftPanel.Name = "LeftPanel";
			this.LeftPanel.Size = new System.Drawing.Size(175, 503);
			this.LeftPanel.TabIndex = 10;
			// 
			// HorzSplitter
			// 
			this.HorzSplitter.Dock = System.Windows.Forms.DockStyle.Top;
			this.HorzSplitter.Location = new System.Drawing.Point(0, 248);
			this.HorzSplitter.Name = "HorzSplitter";
			this.HorzSplitter.Size = new System.Drawing.Size(175, 4);
			this.HorzSplitter.TabIndex = 7;
			this.HorzSplitter.TabStop = false;
			// 
			// VertSplitter
			// 
			this.VertSplitter.Location = new System.Drawing.Point(175, 28);
			this.VertSplitter.Name = "VertSplitter";
			this.VertSplitter.Size = new System.Drawing.Size(3, 503);
			this.VertSplitter.TabIndex = 11;
			this.VertSplitter.TabStop = false;
			// 
			// SaveFileDialog
			// 
			this.SaveFileDialog.DefaultExt = "txt";
			this.SaveFileDialog.FileName = "Output";
			this.SaveFileDialog.Filter = "Text Document|*.txt|Rich Text Format (RTF)|*.rtf";
			// 
			// TreeContextMenu
			// 
			this.TreeContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							this.RunContextMenuItem});
			// 
			// RunContextMenuItem
			// 
			this.RunContextMenuItem.Index = 0;
			this.RunContextMenuItem.Text = "&Run";
			this.RunContextMenuItem.Click += new System.EventHandler(this.RunContextMenuItem_Click);
			// 
			// WordWrapMenuItem
			// 
			this.WordWrapMenuItem.Index = 2;
			this.WordWrapMenuItem.Text = "&Word Wrap";
			this.WordWrapMenuItem.Click += new System.EventHandler(this.WordWrapMenuItem_Click);
			// 
			// NAntForm
			// 
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(824, 553);
			this.Controls.Add(this.RightPanel);
			this.Controls.Add(this.VertSplitter);
			this.Controls.Add(this.LeftPanel);
			this.Controls.Add(this.MainToolBar);
			this.Controls.Add(this.MainStatusBar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.MainMenu;
			this.MinimumSize = new System.Drawing.Size(480, 344);
			this.Name = "NAntForm";
			this.Text = "NAnt-Gui";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.NAnt_DragDrop);
			this.Closed += new System.EventHandler(this.NAnt_Closed);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.NAnt_DragEnter);
			((System.ComponentModel.ISupportInitialize)(this.FileStatusBarPanel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ullFileStatusBarPanel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ProgressPanel)).EndInit();
			this.RightPanel.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.OutputTabPage.ResumeLayout(false);
			this.XMLTabPage.ResumeLayout(false);
			this.LeftPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion


		private void ExitMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void BrowseButton_Click(object sender, EventArgs e)
		{
			this.BrowseForBuildFile();
		}

		private void NAnt_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string lFileName = GetDragFilename(e);

				if (Utils.ExtensionIsValid(lFileName))
				{
					e.Effect = DragDropEffects.Copy;
				}
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

		private void CloseMenuItem_Click(object sender, EventArgs e)
		{
			this.DoClose();
		}

		private void MainToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			if (e.Button == this.BuildToolBarButton)
			{
				this.Build();
			}
			else if (e.Button == this.OpenToolBarButton)
			{
				this.BrowseForBuildFile();
			}
			else if (e.Button == this.ReloadToolBarButton)
			{
				this.Reload();
			}
		}

		private void BrowseForBuildFile()
		{
			this.OpenFileDialog.InitialDirectory = _buildFile;
			if (this.OpenFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.LoadBuildFile(this.OpenFileDialog.FileName);
			}
		}

		private void RunMenuItem_Click(object sender, EventArgs e)
		{
			this.Build();
		}

		private void Build()
		{
			this.ClearOutput();
			_core.Run(_buildFile);
		}

		private void ClearOutput()
		{
			this.OutputTabPage.Focus();
			this.OutputTextBox.Clear();
			Outputter.Clear();
		}

		private void NAnt_Closed(object sender, EventArgs e)
		{
			_recentItems.Save();
		}

		private void ReloadMenuItem_Click(object sender, EventArgs e)
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

		private void AboutMenuItem_Click(object sender, EventArgs e)
		{
			About lAbout = new About();
			lAbout.ShowDialog();
		}

		public void OutputMessage(string message)
		{
			if (this.InvokeRequired)
			{
				MessageEventHandler messageEH =
					new MessageEventHandler(this.OutputTextBox_OutputMessage);

				this.BeginInvoke(messageEH, new Object[1] {message});
			}
			else
			{
				this.OutputTextBox_OutputMessage(message);
			}
		}

		private void OutputTextBox_OutputMessage(string pMessage)
		{
			lock (this)
			{
				if(!this.OutputTextBox.Focused) this.OutputTextBox.Focus();

				Outputter.AppendRtfText(SyntaxHighlighter.Highlight(pMessage));

				this.OutputTextBox.SelectionStart = this.OutputTextBox.TextLength;
				this.OutputTextBox.SelectedRtf = Outputter.RtfDocument;
				this.OutputTextBox.ScrollToCaret();
			}
		}

		public void Build_Finished(object sender, BuildEventArgs e)
		{
			this.Update();
		}

		private void CopyMenuItem_Click(object sender, EventArgs e)
		{
			this.OutputTextBox.Copy();
		}

		private void SelectAllMenuItem_Click(object sender, EventArgs e)
		{
			this.OutputTextBox.SelectAll();
		}

		public void DoClose()
		{
			_core.DisableWatcher();
			_buildFile = "";
			this.ClearOutput();
			this.BuildTreeView.Nodes.Clear();
			this.ProjectPropertyGrid.SelectedObject = null;
		}

		private void OptionsMenuItem_Click(object sender, System.EventArgs e)
		{
			OptionsForm lOptionsForm = new OptionsForm();
			lOptionsForm.ShowDialog();
		}

		private void BuildFileLoaded(Project project)
		{
			this.ClearOutput();

			this.UpdateDisplay(project);
			this.AddTargets(project);
			this.AddProperties(project);

			_firstLoad	= false;
		}

		private void UpdateDisplay(Project project)
		{
			using (TextReader reader = File.OpenText(_buildFile))
			{
				this.XMLRichTextBox.Text = reader.ReadToEnd();
			}
			
			bool hasProjectName	= project.Name.Length > 0;
			string filename		= new FileInfo(_buildFile).Name;

			this.Text = "NAnt-Gui";
			if (hasProjectName)
			{
				this.Text += string.Format(" ({0})", project.Name);
			}

			string projectName	= hasProjectName ? project.Name : filename;

			_recentItems.Add(_buildFile);
			this.UpdateRecentItemsMenu();

			this.MainStatusBar.Panels[0].Text = string.Format("{0} ({1})", projectName, project.Description);
			this.MainStatusBar.Panels[1].Text = _buildFile;

			this.BuildTreeView.Nodes.Clear();
			this.BuildTreeView.Nodes.Add(new TreeNode(projectName));
		}


		private void AddTargets(Project project)
		{
			foreach (Target target in project.Targets)
			{
				this.AddTargetTreeNode(target);
			}

			this.BuildTreeView.ExpandAll();
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

		public void AddTreeTargetsToBuild(NProject project)
		{
			foreach (TreeNode lItem in this.BuildTreeView.Nodes[0].Nodes)
			{
				if (lItem.Checked)
				{
					project.BuildTargets.Add(lItem.Text);
				}
			}
		}

		public void AddPropertiesToProject(NProject project)
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

		private void LoadNonProjectProperty(PropertySpec spec, NAnt.Core.Project project)
		{
			string lValue = _propertyTable[GetKey(spec)].ToString();
			string lExpandedProperty = lValue;
			try
			{
				lExpandedProperty = project.ExpandProperties(lValue, 
				                                             new Location(_buildFile));
			}
			catch (BuildException){ /* ignore */ }
	
			project.Properties.AddReadOnly(spec.Name, lExpandedProperty);
		}

		private bool ValidTarget(string category, NProject project)
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
				string targetName	= FormatTargetName(target.Name, target.Description);
				TreeNode node		= new TreeNode(targetName);
				node.Checked		= target.Default;
				node.Tag			= target;
				this.BuildTreeView.Nodes[0].Nodes.Add(node);
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

		private void BuildTreeView_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TreeNode node = this.BuildTreeView.GetNodeAt(e.X, e.Y);
				if (node != null)
				{
					TreeContextMenu.Show(this.BuildTreeView, new Point(e.X , e.Y));
				}
			}
		}

		private void RunContextMenuItem_Click(object sender, System.EventArgs e)
		{
			this.ClearOutput();
			_core.Run(_buildFile);
		}

		private void BuildTreeView_MouseMove(object sender, MouseEventArgs e)
		{
			TreeNode node = this.BuildTreeView.GetNodeAt(e.X, e.Y);
			if (node == null || node.Parent == null) 
			{
				this.ToolTip.SetToolTip(this.BuildTreeView, "");
			}
			else
			{
				Target target = (Target)node.Tag;
				this.ToolTip.SetToolTip(this.BuildTreeView, target.Description);
			}
		}

		private void UpdateRecentItemsMenu()
		{
			int count = 1;
			this.RecentMenuItem.MenuItems.Clear();

			foreach (string lItem in this._recentItems)
			{
				EventHandler lOnClick	= new EventHandler(this.RecentFile_Clicked);
				MenuItem lMenuItem		= new MenuItem(count++ + " " + lItem, lOnClick);
				this.RecentMenuItem.MenuItems.Add(lMenuItem);
			}
		}

		private void RecentFile_Clicked(object sender, EventArgs e)
		{
			MenuItem lItem = (MenuItem) sender;
			this.LoadBuildFile(lItem.Text.Substring(2));
		}

		private void NAntSDKMenuItem_Click(object sender, System.EventArgs e)
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

		private void NAntHelpMenuItem_Click(object sender, System.EventArgs e)
		{
			const string nantHelp = @"\..\nant-docs\help\index.html";
			LoadHelpFile(this.GetRunDirectory() + nantHelp);
		}

		private void NAntContribMenuItem_Click(object sender, System.EventArgs e)
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

		private void SaveOutputMenuItem_Click(object sender, System.EventArgs e)
		{
			this.SaveFileDialog.InitialDirectory = _buildFile;
			DialogResult result = this.SaveFileDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				this.SaveOutput();
			}
		}

		private void SaveOutput()
		{
			int filterIndex = this.SaveFileDialog.FilterIndex;
	
			if (filterIndex == PLAIN_TEXT_INDEX)
			{
				this.SavePlainTextOutput();
			}
			else if (filterIndex == RICH_TEXT_INDEX)
			{
				this.SaveRichTextOutput();
			}
		}

		private void SavePlainTextOutput()
		{
			this.OutputTextBox.SaveFile(this.SaveFileDialog.FileName, 
				RichTextBoxStreamType.PlainText);
		}

		private void SaveRichTextOutput()
		{
			this.OutputTextBox.SaveFile(this.SaveFileDialog.FileName, 
				RichTextBoxStreamType.RichText);
		}

		private void WordWrapMenuItem_Click(object sender, System.EventArgs e)
		{
			if (this.tabControl1.SelectedTab == this.OutputTabPage)
			{
				this.WordWrapMenuItem.Checked = this.OutputTextBox.WordWrap =
					!this.OutputTextBox.WordWrap;
			}
			else if (this.tabControl1.SelectedTab == this.XMLTabPage)
			{
				this.WordWrapMenuItem.Checked = this.XMLRichTextBox.WordWrap =
					!this.XMLRichTextBox.WordWrap;
			}
		}

		private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.tabControl1.SelectedTab == this.OutputTabPage)
			{
				this.WordWrapMenuItem.Checked = this.OutputTextBox.WordWrap;
			}
			else if (this.tabControl1.SelectedTab == this.XMLTabPage)
			{
				this.WordWrapMenuItem.Checked = this.XMLRichTextBox.WordWrap;
			}
		
		}
	}
}