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
using NAnt.Core;
using NProject = NAnt.Core.Project;
using TabControl = Crownwood.Magic.Controls.TabControl;
using TabPage = Crownwood.Magic.Controls.TabPage;

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
		private DockingManager _dockManager;
		private string _buildFile;
		private PopupMenu _editContextMenu = new PopupMenu();

		private MenuCommand ExitMenuItem;
		private MenuCommand menuItem4;
		private MenuCommand menuItem5;
		private StatusBarPanel FileStatusBarPanel;
		private OpenFileDialog OpenFileDialog;
		private StatusBar MainStatusBar;
		private MenuControl MainMenu;
		private RichTextBox OutputTextBox;
		private MenuCommand FileMenuItem;
		private MenuCommand OpenMenuItem;
		private MenuCommand SaveMenuItem;
		private MenuCommand SaveAsMenuItem;
		private MenuCommand CloseMenuItem;
		private MenuCommand RecentMenuItem;
		private ToolBar MainToolBar;
		private ImageList ToolBarImageList;
		private MenuCommand ReloadMenuItem;
		private TreeView TargetsTreeView;
		public PropertyGrid ProjectPropertyGrid;
		private MenuCommand HelpMenuItem;
		private MenuCommand AboutMenuItem;
		private MenuCommand NAntMenuItem;
		private MenuCommand BuildMenuItem;
		private StatusBarPanel ProgressPanel;
		private MenuCommand EditMainMenuItem;
		private MenuCommand CopyMenuItem;
		private MenuCommand SelectAllMenuItem;
		private MenuCommand ToolsMenuItem;
		private MenuCommand OptionsMenuItem;
		private ToolTip ToolTip;
		private StatusBarPanel FullFileStatusBarPanel;
		private IContainer components;
		private ToolBarButton OpenToolBarButton;
		private ToolBarButton SaveToolBarButton;
		private ToolBarButton BuildToolBarButton;
		private ToolBarButton ReloadToolBarButton;
		private ToolBarButton StopToolBarButton;
		private PropertyTable _propertyTable = new PropertyTable();
		private RichTextBox XMLRichTextBox;
		private MenuCommand NAntContribMenuItem;
		private MenuCommand NAntHelpMenuItem;
		private MenuCommand NAntSDKMenuItem;
		private MenuCommand menuItem1;
		private MenuCommand SaveOutputMenuItem;
		private MenuCommand WordWrapMenuItem;
		private TabControl TabControl;
		private TabPage XMLTabPage;
		private TabPage OutputTabPage;
		private MenuCommand ViewMenuItem;
		private MenuCommand TargetsMenuItem;
		private MenuCommand PropertiesMenuItem;
		private PopupMenu _targetsPopupMenu = new PopupMenu();
		private Content _targetsContent;
		private Content _propertiesContent;
		private WindowContent _targetWindowContent;
		private SaveFileDialog OutputSaveFileDialog;
		private SaveFileDialog XMLSaveFileDialog;
		private string _XML = "";

		public NAntForm(CommandLineOptions options)
		{
			_options = options;

			this.ToolBarImageList = ResourceHelper.LoadBitmapStrip(
				this.GetType(), "NAntGui.Core.Images.MenuItems.bmp", new Size(16, 16), new Point(0, 0));

			InitializeComponent();

			this.MainToolBar.ImageList			= this.ToolBarImageList;
			this.OpenMenuItem.ImageList			= this.ToolBarImageList;
			this.SaveMenuItem.ImageList			= this.ToolBarImageList;
			this.ReloadMenuItem.ImageList		= this.ToolBarImageList;
			this.PropertiesMenuItem.ImageList	= this.ToolBarImageList;
			this.BuildMenuItem.ImageList			= this.ToolBarImageList;

			// Reduce the amount of flicker that occurs when windows are redocked within
			// the container. As this prevents unsightly backcolors being drawn in the
			// WM_ERASEBACKGROUND that seems to occur.
			this.SetStyle(ControlStyles.DoubleBuffer, true);
//			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			this.SetupDockManager();
			this.CreateOutputTextBoxMenu();
			this.CreateTargetTreeViewMenu();

			this.TabControl.Appearance = TabControl.VisualAppearance.MultiDocument;

			_core = new Core(this);
			_core.BuildFileChanged += new BuildFileChangedEH(this.BuildFileLoaded);
			_core.BuildFinished += new BuildEventHandler(this.Build_Finished);

			this.UpdateRecentItemsMenu();
			this.LoadInitialBuildFile();
		}

		private void SetupDockManager()
		{
			// Create the object that manages the docking state
			_dockManager = new DockingManager(this, VisualStyle.IDE);

			// Ensure that the RichTextBox is always the innermost control
			_dockManager.InnerControl = this.TabControl;

			// Create Content which contains a RichTextBox
			_targetsContent = _dockManager.Contents.Add(this.TargetsTreeView, "Targets");

			_propertiesContent = _dockManager.Contents.Add(this.ProjectPropertyGrid, "Properties");
			_propertiesContent.ImageList = this.ToolBarImageList;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof (NAntForm));
			this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.MainStatusBar = new System.Windows.Forms.StatusBar();
			this.FileStatusBarPanel = new System.Windows.Forms.StatusBarPanel();
			this.FullFileStatusBarPanel = new System.Windows.Forms.StatusBarPanel();
			this.ProgressPanel = new System.Windows.Forms.StatusBarPanel();
			this.MainMenu = new Crownwood.Magic.Menus.MenuControl();
			this.FileMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.OpenMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.SaveMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.SaveAsMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.ReloadMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.CloseMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.SaveOutputMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.menuItem4 = new Crownwood.Magic.Menus.MenuCommand();
			this.RecentMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.menuItem5 = new Crownwood.Magic.Menus.MenuCommand();
			this.ExitMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.EditMainMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.CopyMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.SelectAllMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.WordWrapMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.TargetsMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.PropertiesMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.NAntMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.BuildMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.ToolsMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.OptionsMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.HelpMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.NAntHelpMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.NAntSDKMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.NAntContribMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.menuItem1 = new Crownwood.Magic.Menus.MenuCommand();
			this.AboutMenuItem = new Crownwood.Magic.Menus.MenuCommand();
			this.OutputTextBox = new System.Windows.Forms.RichTextBox();
			this.MainToolBar = new System.Windows.Forms.ToolBar();
			this.OpenToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.SaveToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.ReloadToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.BuildToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.StopToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.TargetsTreeView = new System.Windows.Forms.TreeView();
			this.XMLRichTextBox = new System.Windows.Forms.RichTextBox();
			this.ProjectPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.OutputSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.TabControl = new Crownwood.Magic.Controls.TabControl();
			this.XMLTabPage = new Crownwood.Magic.Controls.TabPage();
			this.OutputTabPage = new Crownwood.Magic.Controls.TabPage();
			this.XMLSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
			((System.ComponentModel.ISupportInitialize) (this.FileStatusBarPanel)).BeginInit();
			((System.ComponentModel.ISupportInitialize) (this.FullFileStatusBarPanel)).BeginInit();
			((System.ComponentModel.ISupportInitialize) (this.ProgressPanel)).BeginInit();
			this.TabControl.SuspendLayout();
			this.XMLTabPage.SuspendLayout();
			this.OutputTabPage.SuspendLayout();
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
			this.MainStatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[]
				{
					this.FileStatusBarPanel,
					this.FullFileStatusBarPanel,
					this.ProgressPanel
				});
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
			this.MainMenu.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((System.Byte) (0)));
			this.MainMenu.HighlightTextColor = System.Drawing.SystemColors.MenuText;
			this.MainMenu.Location = new System.Drawing.Point(0, 0);
			this.MainMenu.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[]
				{
					this.FileMenuItem,
					this.EditMainMenuItem,
					this.ViewMenuItem,
					this.NAntMenuItem,
					this.ToolsMenuItem,
					this.HelpMenuItem
				});
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.Size = new System.Drawing.Size(824, 25);
			this.MainMenu.Style = Crownwood.Magic.Common.VisualStyle.IDE;
			this.MainMenu.TabIndex = 13;
			this.MainMenu.TabStop = false;
			// 
			// FileMenuItem
			// 
			this.FileMenuItem.Description = "MenuItem";
			this.FileMenuItem.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[]
				{
					this.OpenMenuItem,
					this.SaveMenuItem,
					this.SaveAsMenuItem,
					this.ReloadMenuItem,
					this.CloseMenuItem,
					this.SaveOutputMenuItem,
					this.menuItem4,
					this.RecentMenuItem,
					this.menuItem5,
					this.ExitMenuItem
				});
			this.FileMenuItem.Text = "&File";
			// 
			// OpenMenuItem
			// 
			this.OpenMenuItem.Description = "MenuItem";
			this.OpenMenuItem.ImageIndex = 5;
			this.OpenMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.OpenMenuItem.Text = "&Open";
			this.OpenMenuItem.Click += new System.EventHandler(this.BrowseButton_Click);
			// 
			// SaveMenuItem
			// 
			this.SaveMenuItem.Description = "MenuItem";
			this.SaveMenuItem.ImageIndex = 2;
			this.SaveMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.SaveMenuItem.Text = "&Save";
			this.SaveMenuItem.Click += new System.EventHandler(this.SaveMenuItem_Click);
			// 
			// SaveAsMenuItem
			// 
			this.SaveAsMenuItem.Description = "MenuItem";
			this.SaveAsMenuItem.ImageIndex = 2;
			this.SaveAsMenuItem.Shortcut = System.Windows.Forms.Shortcut.F12;
			this.SaveAsMenuItem.Text = "Save &As";
			this.SaveAsMenuItem.Click += new System.EventHandler(this.SaveAsMenuItem_Click);
			// 
			// ReloadMenuItem
			// 
			this.ReloadMenuItem.Description = "MenuItem";
			this.ReloadMenuItem.ImageIndex = 4;
			this.ReloadMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
			this.ReloadMenuItem.Text = "&Reload";
			this.ReloadMenuItem.Click += new System.EventHandler(this.ReloadMenuItem_Click);
			// 
			// CloseMenuItem
			// 
			this.CloseMenuItem.Description = "MenuItem";
			this.CloseMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlW;
			this.CloseMenuItem.Text = "&Close";
			this.CloseMenuItem.Click += new System.EventHandler(this.CloseMenuItem_Click);
			// 
			// SaveOutputMenuItem
			// 
			this.SaveOutputMenuItem.Description = "MenuItem";
			this.SaveOutputMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
			this.SaveOutputMenuItem.Text = "Save O&utput";
			this.SaveOutputMenuItem.Click += new System.EventHandler(this.SaveOutputMenuItem_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Description = "MenuItem";
			this.menuItem4.Text = "-";
			// 
			// RecentMenuItem
			// 
			this.RecentMenuItem.Description = "MenuItem";
			this.RecentMenuItem.Text = "Recent &Files";
			// 
			// menuItem5
			// 
			this.menuItem5.Description = "MenuItem";
			this.menuItem5.Text = "-";
			// 
			// ExitMenuItem
			// 
			this.ExitMenuItem.Description = "MenuItem";
			this.ExitMenuItem.Text = "E&xit";
			this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
			// 
			// EditMainMenuItem
			// 
			this.EditMainMenuItem.Description = "MenuItem";
			this.EditMainMenuItem.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[]
				{
					this.CopyMenuItem,
					this.SelectAllMenuItem,
					this.WordWrapMenuItem
				});
			this.EditMainMenuItem.Text = "&Edit";
			// 
			// CopyMenuItem
			// 
			this.CopyMenuItem.Description = "MenuItem";
			this.CopyMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.CopyMenuItem.Text = "&Copy";
			this.CopyMenuItem.Click += new System.EventHandler(this.CopyText);
			// 
			// SelectAllMenuItem
			// 
			this.SelectAllMenuItem.Description = "MenuItem";
			this.SelectAllMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			this.SelectAllMenuItem.Text = "Select &All";
			this.SelectAllMenuItem.Click += new System.EventHandler(this.SelectAllText);
			// 
			// WordWrapMenuItem
			// 
			this.WordWrapMenuItem.Description = "MenuItem";
			this.WordWrapMenuItem.Text = "&Word Wrap";
			this.WordWrapMenuItem.Click += new System.EventHandler(this.WordWrapMenuItem_Click);
			// 
			// ViewMenuItem
			// 
			this.ViewMenuItem.Description = "MenuItem";
			this.ViewMenuItem.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[]
				{
					this.TargetsMenuItem,
					this.PropertiesMenuItem
				});
			this.ViewMenuItem.Text = "&View";
			// 
			// TargetsMenuItem
			// 
			this.TargetsMenuItem.Description = "MenuItem";
			this.TargetsMenuItem.Text = "&Targets";
			this.TargetsMenuItem.Click += new System.EventHandler(this.TargetsMenuItem_Click);
			// 
			// PropertiesMenuItem
			// 
			this.PropertiesMenuItem.Description = "MenuItem";
			this.PropertiesMenuItem.ImageIndex = 0;
			this.PropertiesMenuItem.Text = "&Properties";
			this.PropertiesMenuItem.Click += new System.EventHandler(this.PropertiesMenuItem_Click);
			// 
			// NAntMenuItem
			// 
			this.NAntMenuItem.Description = "MenuItem";
			this.NAntMenuItem.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[]
				{
					this.BuildMenuItem
				});
			this.NAntMenuItem.Text = "&NAnt";
			// 
			// RunMenuItem
			// 
			this.BuildMenuItem.Description = "MenuItem";
			this.BuildMenuItem.ImageIndex = 7;
			this.BuildMenuItem.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.BuildMenuItem.Text = "&Build";
			this.BuildMenuItem.Click += new System.EventHandler(this.RunMenuItem_Click);
			// 
			// ToolsMenuItem
			// 
			this.ToolsMenuItem.Description = "MenuItem";
			this.ToolsMenuItem.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[]
				{
					this.OptionsMenuItem
				});
			this.ToolsMenuItem.Text = "&Tools";
			// 
			// OptionsMenuItem
			// 
			this.OptionsMenuItem.Description = "MenuItem";
			this.OptionsMenuItem.Text = "&Options";
			this.OptionsMenuItem.Click += new System.EventHandler(this.OptionsMenuItem_Click);
			// 
			// HelpMenuItem
			// 
			this.HelpMenuItem.Description = "MenuItem";
			this.HelpMenuItem.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[]
				{
					this.NAntHelpMenuItem,
					this.NAntSDKMenuItem,
					this.NAntContribMenuItem,
					this.menuItem1,
					this.AboutMenuItem
				});
			this.HelpMenuItem.Text = "&Help";
			// 
			// NAntHelpMenuItem
			// 
			this.NAntHelpMenuItem.Description = "MenuItem";
			this.NAntHelpMenuItem.Text = "NAnt &Help";
			this.NAntHelpMenuItem.Click += new System.EventHandler(this.NAntHelpMenuItem_Click);
			// 
			// NAntSDKMenuItem
			// 
			this.NAntSDKMenuItem.Description = "MenuItem";
			this.NAntSDKMenuItem.Text = "NAnt &SDK Help";
			this.NAntSDKMenuItem.Click += new System.EventHandler(this.NAntSDKMenuItem_Click);
			// 
			// NAntContribMenuItem
			// 
			this.NAntContribMenuItem.Description = "MenuItem";
			this.NAntContribMenuItem.Text = "NAnt-&Contrib Help";
			this.NAntContribMenuItem.Click += new System.EventHandler(this.NAntContribMenuItem_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Description = "MenuItem";
			this.menuItem1.Text = "-";
			// 
			// AboutMenuItem
			// 
			this.AboutMenuItem.Description = "MenuItem";
			this.AboutMenuItem.Text = "&About NAnt-Gui";
			this.AboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
			// 
			// OutputTextBox
			// 
			this.OutputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.OutputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.OutputTextBox.DetectUrls = false;
			this.OutputTextBox.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.OutputTextBox.Location = new System.Drawing.Point(0, 0);
			this.OutputTextBox.Name = "OutputTextBox";
			this.OutputTextBox.ReadOnly = true;
			this.OutputTextBox.Size = new System.Drawing.Size(824, 454);
			this.OutputTextBox.TabIndex = 3;
			this.OutputTextBox.Text = "";
			this.OutputTextBox.WordWrap = false;
			this.OutputTextBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OutputTextBox_MouseUp);
			// 
			// MainToolBar
			// 
			this.MainToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.MainToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[]
				{
					this.OpenToolBarButton,
					this.SaveToolBarButton,
					this.ReloadToolBarButton,
					this.BuildToolBarButton,
					this.StopToolBarButton
				});
			this.MainToolBar.DropDownArrows = true;
			this.MainToolBar.Location = new System.Drawing.Point(0, 25);
			this.MainToolBar.Name = "MainToolBar";
			this.MainToolBar.ShowToolTips = true;
			this.MainToolBar.Size = new System.Drawing.Size(824, 28);
			this.MainToolBar.TabIndex = 4;
			this.MainToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.MainToolBar_ButtonClick);
			// 
			// OpenToolBarButton
			// 
			this.OpenToolBarButton.ImageIndex = 5;
			this.OpenToolBarButton.ToolTipText = "Open Build File";
			// 
			// SaveToolBarButton
			// 
			this.SaveToolBarButton.ImageIndex = 2;
			this.SaveToolBarButton.ToolTipText = "Save Build File";
			// 
			// ReloadToolBarButton
			// 
			this.ReloadToolBarButton.ImageIndex = 4;
			this.ReloadToolBarButton.ToolTipText = "Reload Build File";
			// 
			// BuildToolBarButton
			// 
			this.BuildToolBarButton.ImageIndex = 7;
			this.BuildToolBarButton.ToolTipText = "Build Default Target";
			// 
			// StopToolBarButton
			// 
			this.StopToolBarButton.ImageIndex = 3;
			this.StopToolBarButton.ToolTipText = "Abort the Current Build";
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
			// XMLRichTextBox
			// 
			this.XMLRichTextBox.AcceptsTab = true;
			this.XMLRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.XMLRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.XMLRichTextBox.DetectUrls = false;
			this.XMLRichTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
			this.XMLRichTextBox.Location = new System.Drawing.Point(0, 0);
			this.XMLRichTextBox.Name = "XMLRichTextBox";
			this.XMLRichTextBox.Size = new System.Drawing.Size(824, 454);
			this.XMLRichTextBox.TabIndex = 4;
			this.XMLRichTextBox.Text = "";
			this.XMLRichTextBox.WordWrap = false;
			this.XMLRichTextBox.TextChanged += new System.EventHandler(this.XMLRichTextBox_TextChanged);
			this.XMLRichTextBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.XMLTextBox_MouseUp);
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
			// OutputOutputSaveFileDialog
			// 
			this.OutputSaveFileDialog.DefaultExt = "txt";
			this.OutputSaveFileDialog.FileName = "Output";
			this.OutputSaveFileDialog.Filter = "Text Document|*.txt|Rich Text Format (RTF)|*.rtf";
			// 
			// TabControl
			// 
			this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TabControl.IDEPixelArea = true;
			this.TabControl.Location = new System.Drawing.Point(0, 53);
			this.TabControl.Name = "TabControl";
			this.TabControl.SelectedIndex = 0;
			this.TabControl.SelectedTab = this.OutputTabPage;
			this.TabControl.Size = new System.Drawing.Size(824, 478);
			this.TabControl.TabIndex = 12;
			this.TabControl.TabPages.AddRange(new Crownwood.Magic.Controls.TabPage[]
				{
					this.OutputTabPage,
					this.XMLTabPage
				});
			this.TabControl.SelectionChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
			// 
			// XMLTabPage
			// 
			this.XMLTabPage.Controls.Add(this.XMLRichTextBox);
			this.XMLTabPage.Location = new System.Drawing.Point(0, 0);
			this.XMLTabPage.Name = "XMLTabPage";
			this.XMLTabPage.Selected = false;
			this.XMLTabPage.Size = new System.Drawing.Size(824, 453);
			this.XMLTabPage.TabIndex = 4;
			this.XMLTabPage.Title = "XML";
			// 
			// OutputTabPage
			// 
			this.OutputTabPage.Controls.Add(this.OutputTextBox);
			this.OutputTabPage.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
			this.OutputTabPage.Location = new System.Drawing.Point(0, 0);
			this.OutputTabPage.Name = "OutputTabPage";
			this.OutputTabPage.Size = new System.Drawing.Size(824, 453);
			this.OutputTabPage.TabIndex = 3;
			this.OutputTabPage.Title = "Output";
			// 
			// XMLOutputSaveFileDialog
			// 
			this.XMLSaveFileDialog.DefaultExt = "build";
			this.XMLSaveFileDialog.Filter = "NAnt Buildfile|*.build|NAnt Include|*.inc";
			// 
			// NAntForm
			// 
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(824, 553);
			this.Controls.Add(this.TabControl);
			this.Controls.Add(this.MainStatusBar);
			this.Controls.Add(this.MainToolBar);
			this.Controls.Add(this.MainMenu);
			this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(480, 344);
			this.Name = "NAntForm";
			this.Text = "NAnt-Gui";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.NAntForm_Closing);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.NAnt_DragDrop);
			this.Closed += new System.EventHandler(this.NAnt_Closed);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.NAnt_DragEnter);
			((System.ComponentModel.ISupportInitialize) (this.FileStatusBarPanel)).EndInit();
			((System.ComponentModel.ISupportInitialize) (this.FullFileStatusBarPanel)).EndInit();
			((System.ComponentModel.ISupportInitialize) (this.ProgressPanel)).EndInit();
			this.TabControl.ResumeLayout(false);
			this.XMLTabPage.ResumeLayout(false);
			this.OutputTabPage.ResumeLayout(false);
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
			else if (e.Button == this.SaveToolBarButton)
			{
				this.Save();
			}
			else if (e.Button == this.ReloadToolBarButton)
			{
				this.Reload();
			}
			else if (e.Button == this.StopToolBarButton)
			{
				_core.Stop();
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
			this.TabControl.SelectedTab = this.OutputTabPage;
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
				if (!this.OutputTextBox.Focused) this.OutputTextBox.Focus();

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

		private void CopyText(object sender, EventArgs e)
		{
			if (this.TabControl.SelectedTab == this.OutputTabPage)
			{
				this.OutputTextBox.Copy();
			}
			else if (this.TabControl.SelectedTab == this.XMLTabPage)
			{
				this.XMLRichTextBox.Copy();
			}
		}

		private void SelectAllText(object sender, EventArgs e)
		{
			if (this.TabControl.SelectedTab == this.OutputTabPage)
			{
				this.OutputTextBox.SelectAll();
			}
			else if (this.TabControl.SelectedTab == this.XMLTabPage)
			{
				this.XMLRichTextBox.SelectAll();
			}
		}


		public void DoClose()
		{
			_core.DisableWatcher();
			_buildFile = "";
			this.ClearOutput();
			this.TargetsTreeView.Nodes.Clear();
			this.ProjectPropertyGrid.SelectedObject = null;

			this.DisableMenuItemsAndButtons();
		}

		private void DisableMenuItemsAndButtons()
		{
			this.ReloadMenuItem.Enabled			= false;
			this.ReloadToolBarButton.Enabled	= false;
			this.SaveMenuItem.Enabled			= false;
			this.SaveToolBarButton.Enabled		= false;
			this.StopToolBarButton.Enabled		= false;
			this.SaveAsMenuItem.Enabled			= false;
			this.BuildMenuItem.Enabled			= false;
			this.BuildToolBarButton.Enabled		= false;
			this.CloseMenuItem.Enabled			= false;
		}

		private void EnableMenuItemsAndButtons()
		{
			this.ReloadMenuItem.Enabled			= true;
			this.ReloadToolBarButton.Enabled	= true;
			this.SaveMenuItem.Enabled			= true;
			this.SaveToolBarButton.Enabled		= true;
			this.StopToolBarButton.Enabled		= true;
			this.SaveAsMenuItem.Enabled			= true;
			this.BuildMenuItem.Enabled			= true;
			this.BuildToolBarButton.Enabled		= true;
			this.CloseMenuItem.Enabled			= true;
		}

		private void OptionsMenuItem_Click(object sender, EventArgs e)
		{
			OptionsForm lOptionsForm = new OptionsForm();
			lOptionsForm.ShowDialog();
		}

		private void BuildFileLoaded(Project project)
		{
			this.ClearOutput();
			this.EnableMenuItemsAndButtons();
			this.UpdateDisplay(project);
			this.AddTargets(project);
			this.AddProperties(project);

			_firstLoad = false;
		}

		private void UpdateDisplay(Project project)
		{
			using (TextReader reader = File.OpenText(_buildFile))
			{
				this.XMLRichTextBox.Text = reader.ReadToEnd();
				_XML = this.XMLRichTextBox.Text;
			}

			string filename = new FileInfo(_buildFile).Name;

			this.Text = "NAnt-Gui" + string.Format(" - {0}", filename);

			bool hasProjectName = project.Name.Length > 0;
			string projectName = hasProjectName ? project.Name : filename;

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

		public void AddTreeTargetsToBuild(NProject project)
		{
			foreach (TreeNode lItem in this.TargetsTreeView.Nodes[0].Nodes)
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
			catch (BuildException)
			{ /* ignore */
			}

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

		private void BuildMenuItem_Click(object sender, EventArgs e)
		{
			this.ClearOutput();
			_core.Run(_buildFile);
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
			int count = 1;
			this.RecentMenuItem.MenuCommands.Clear();

			foreach (string lItem in this._recentItems)
			{
				EventHandler lOnClick = new EventHandler(this.RecentFile_Clicked);
				MenuCommand lMenuCommand = new MenuCommand(count++ + " " + lItem, lOnClick);
				this.RecentMenuItem.MenuCommands.Add(lMenuCommand);
			}
		}

		private void RecentFile_Clicked(object sender, EventArgs e)
		{
			MenuCommand lItem = (MenuCommand) sender;
			this.LoadBuildFile(lItem.Text.Substring(2));
		}

		private void NAntSDKMenuItem_Click(object sender, EventArgs e)
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

		private void NAntHelpMenuItem_Click(object sender, EventArgs e)
		{
			const string nantHelp = @"\..\nant-docs\help\index.html";
			LoadHelpFile(this.GetRunDirectory() + nantHelp);
		}

		private void NAntContribMenuItem_Click(object sender, EventArgs e)
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

		private void SaveOutputMenuItem_Click(object sender, EventArgs e)
		{
			this.OutputSaveFileDialog.InitialDirectory = _buildFile;
			DialogResult result = this.OutputSaveFileDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				this.SaveOutput();
			}
		}

		private void SaveMenuItem_Click(object sender, EventArgs e)
		{
			this.Save();
		}

		private void Save()
		{
			if (File.Exists(_buildFile))
			{
				using (TextWriter writer = File.CreateText(_buildFile))
				{
					writer.Write(this.XMLRichTextBox.Text);
					this.Text = this.Text.TrimEnd(new char[] {'*'});
				}
			}
		}

		private void SaveOutput()
		{
			int filterIndex = this.OutputSaveFileDialog.FilterIndex;

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
			this.OutputTextBox.SaveFile(this.OutputSaveFileDialog.FileName,
			                            RichTextBoxStreamType.PlainText);
		}

		private void SaveRichTextOutput()
		{
			this.OutputTextBox.SaveFile(this.OutputSaveFileDialog.FileName,
			                            RichTextBoxStreamType.RichText);
		}

		private void WordWrapMenuItem_Click(object sender, EventArgs e)
		{
			if (this.TabControl.SelectedTab == this.OutputTabPage)
			{
				this.WordWrapMenuItem.Checked = this.OutputTextBox.WordWrap =
					!this.OutputTextBox.WordWrap;
			}
			else if (this.TabControl.SelectedTab == this.XMLTabPage)
			{
				this.WordWrapMenuItem.Checked = this.XMLRichTextBox.WordWrap =
					!this.XMLRichTextBox.WordWrap;
			}
		}

		private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.TabControl.SelectedTab == this.OutputTabPage)
			{
				this.WordWrapMenuItem.Checked = this.OutputTextBox.WordWrap;
			}
			else if (this.TabControl.SelectedTab == this.XMLTabPage)
			{
				this.WordWrapMenuItem.Checked = this.XMLRichTextBox.WordWrap;
			}
		}

		private void CreateOutputTextBoxMenu()
		{
			MenuCommand copy = new MenuCommand("&Copy", new EventHandler(this.CopyText));
			MenuCommand selectAll = new MenuCommand("Select &All", new EventHandler(this.SelectAllText));

			_editContextMenu.MenuCommands.AddRange(new MenuCommand[] {copy, selectAll});
		}

		private void OutputTextBox_MouseUp(object sender, MouseEventArgs e)
		{
			RichTextBox box = sender as RichTextBox;
			if (e.Button == MouseButtons.Right)
			{
				_editContextMenu.TrackPopup(box.PointToScreen(new Point(e.X, e.Y)));
			}
		}

		private void XMLTextBox_MouseUp(object sender, MouseEventArgs e)
		{
			RichTextBox box = sender as RichTextBox;
			if (e.Button == MouseButtons.Right)
			{
				_editContextMenu.TrackPopup(box.PointToScreen(new Point(e.X, e.Y)));
			}
		}

		private void CreateTargetTreeViewMenu()
		{
			MenuCommand build = new MenuCommand("&Build", new EventHandler(this.BuildMenuItem_Click));
			build.ImageList = this.ToolBarImageList;
			build.ImageIndex = 7;
			_targetsPopupMenu.MenuCommands.AddRange(new MenuCommand[] {build});
		}

		private void TargetsMenuItem_Click(object sender, EventArgs e)
		{
			_dockManager.BringAutoHideIntoView(_targetsContent);
			_dockManager.ShowContent(_targetsContent);
		}

		private void PropertiesMenuItem_Click(object sender, EventArgs e)
		{
			_dockManager.BringAutoHideIntoView(_propertiesContent);
			_dockManager.ShowContent(_propertiesContent);
		}

		private void XMLRichTextBox_TextChanged(object sender, EventArgs e)
		{
			if (XMLRichTextBox.Text != _XML.Replace("\r", ""))
			{
				if (!this.Text.EndsWith("*"))
				{
					this.Text += "*";
				}
			}
			else if (this.Text.EndsWith("*"))
			{
				this.Text = this.Text.TrimEnd(new char[] {'*'});
			}
		}

		private void NAntForm_Closing(object sender, CancelEventArgs e)
		{
			if (XMLRichTextBox.Text != _XML && XMLRichTextBox.Text != _XML.Replace("\r", "")) //.Replace("\r", ""))
			{
				DialogResult result =
					MessageBox.Show("You have unsaved changes.  Save?", "Save Changes?",
					                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

				if (result == DialogResult.Yes)
				{
					using (TextWriter writer = File.CreateText(_buildFile))
					{
						writer.Write(this.XMLRichTextBox.Text);
					}
				}
				else if (result == DialogResult.Cancel)
				{
					e.Cancel = true;
				}
			}
		}

		private void SaveAsMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult result = this.XMLSaveFileDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				using (TextWriter writer = File.CreateText(this.XMLSaveFileDialog.FileName))
				{
					writer.Write(this.XMLRichTextBox.Text);
				}
				this.Text = this.Text.TrimEnd(new char[] {'*'});
				this.LoadBuildFile(this.XMLSaveFileDialog.FileName);
			}
		}

		public CommandLineOptions Options
		{
			set { _options = value; }
			get { return _options; }
		}
	}
}