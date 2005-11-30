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

		private BuildRunner _nantBuildRunner;
		private CommandLineOptions _options;
		private RecentItems _recentItems = new RecentItems();
		private bool _firstLoad = true;
		private DockingManager _dockManager;
		private string _buildFile;
		private string _XML = "";

		#region GUI Items
		private PopupMenu _outputContextMenu = new PopupMenu();
		private RichTextBox OutputTextBox;

		private PopupMenu _xmlContextMenu = new PopupMenu();
		private RichTextBox XMLRichTextBox;

		private StatusBarPanel FileStatusBarPanel;
		private OpenFileDialog OpenFileDialog;
		private StatusBar MainStatusBar;
		
		private ToolBarControl MainToolBar;
		private ImageList _imageList;
		private TreeView TargetsTreeView;
		public PropertyGrid ProjectPropertyGrid;
		private StatusBarPanel ProgressPanel;
		private ToolTip ToolTip;
		private StatusBarPanel FullFileStatusBarPanel;
		private IContainer components;
		private PropertyTable _propertyTable = new PropertyTable();
		
		private MainMenuControl MainMenu;

//		private ToolBarButton OpenToolBarButton;
//		private ToolBarButton SaveToolBarButton;
//		private ToolBarButton BuildToolBarButton;
//		private ToolBarButton ReloadToolBarButton;
//		private ToolBarButton StopToolBarButton;

		private TabControl TabControl;
		private TabPage XMLTabPage;
		private TabPage OutputTabPage;

		private PopupMenu _targetsPopupMenu = new PopupMenu();
		private Content _targetsContent;
		private Content _propertiesContent;
		private WindowContent _targetWindowContent;
		private SaveFileDialog OutputSaveFileDialog;
		private SaveFileDialog XMLSaveFileDialog;
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
			this.CreateOutputContextMenu();
			this.CreateXmlContextMenu();
			this.CreateTargetTreeViewMenu();

			this.TabControl.Appearance = TabControl.VisualAppearance.MultiDocument;

			_nantBuildRunner = new NAntBuildRunner(this);
			_nantBuildRunner.BuildFileLoaded += new BuildFileChangedEH(this.BuildFileLoaded);
			_nantBuildRunner.BuildFinished += new NC.BuildEventHandler(this.Build_Finished);

			this.AssignMenuCommands();
			this.AssignToolBarButtons();
			this.UpdateRecentItemsMenu();
			this.LoadInitialBuildFile();
		}

		private void AssignToolBarButtons()
		{
			this.MainToolBar.Build_Click += new VoidVoid(this.Build);
			this.MainToolBar.Build_Click += new VoidVoid(this.BrowseForBuildFile);
			this.MainToolBar.Build_Click += new VoidVoid(this.Save);
			this.MainToolBar.Build_Click += new VoidVoid(this.Reload);
			this.MainToolBar.Build_Click += new VoidVoid(_nantBuildRunner.Stop);
		}

		private void AssignMenuCommands()
		{
			this.MainMenu.About_Click		= new EventHandler(this.AboutMenuCommand_Click);
			this.MainMenu.Build_Click		= new EventHandler(this.BuildMenuCommand_Click);
			this.MainMenu.Close_Click		= new EventHandler(this.CloseMenuCommand_Click);
			this.MainMenu.Copy_Click		= new EventHandler(this.CopyMenuCommand_Click);
			this.MainMenu.Exit_Click		= new EventHandler(this.ExitMenuCommand_Click);
			this.MainMenu.NAntContrib_Click = new EventHandler(this.NAntContribMenuCommand_Click);
			this.MainMenu.NAntHelp_Click	= new EventHandler(this.NAntHelpMenuCommand_Click);
			this.MainMenu.NAntSDK_Click		= new EventHandler(this.NAntSDKMenuCommand_Click);
			this.MainMenu.Open_Click		= new EventHandler(this.OpenMenuCommand_Click);
			this.MainMenu.Options_Click		= new EventHandler(this.OptionsMenuCommand_Click);
			this.MainMenu.Properties_Click	= new EventHandler(this.PropertiesMenuCommand_Click);
			this.MainMenu.Reload_Click		= new EventHandler(this.ReloadMenuCommand_Click);
			this.MainMenu.SelectAll_Click	= new EventHandler(this.SelectAllMenuCommand_Click);
			this.MainMenu.SaveOutput_Click	= new EventHandler(this.SaveOutputMenuCommand_Click);
			this.MainMenu.Save_Click		= new EventHandler(this.SaveMenuCommand_Click);
			this.MainMenu.SaveAs_Click		= new EventHandler(this.SaveAsMenuCommand_Click);
			this.MainMenu.Targets_Click		= new EventHandler(this.TargetsMenuCommand_Click);
			this.MainMenu.WordWrap_Click	= new EventHandler(this.WordWrapMenuCommand_Click);
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
			this.MainMenu = new MainMenuControl();
			this.OutputTextBox = new System.Windows.Forms.RichTextBox();
			this.MainToolBar = new ToolBarControl();
//			this.OpenToolBarButton = new System.Windows.Forms.ToolBarButton();
//			this.SaveToolBarButton = new System.Windows.Forms.ToolBarButton();
//			this.ReloadToolBarButton = new System.Windows.Forms.ToolBarButton();
//			this.BuildToolBarButton = new System.Windows.Forms.ToolBarButton();
//			this.StopToolBarButton = new System.Windows.Forms.ToolBarButton();
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
//			// 
//			// MainToolBar
//			// 
//			this.MainToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
//			this.MainToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[]
//				{
//					this.OpenToolBarButton,
//					this.SaveToolBarButton,
//					this.ReloadToolBarButton,
//					this.BuildToolBarButton,
//					this.StopToolBarButton
//				});
//			this.MainToolBar.DropDownArrows = true;
//			this.MainToolBar.Location = new System.Drawing.Point(0, 25);
//			this.MainToolBar.Name = "MainToolBar";
//			this.MainToolBar.ShowToolTips = true;
//			this.MainToolBar.Size = new System.Drawing.Size(824, 28);
//			this.MainToolBar.TabIndex = 4;
//			this.MainToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.MainToolBar_ButtonClick);
//			// 
//			// OpenToolBarButton
//			// 
//			this.OpenToolBarButton.ImageIndex = 5;
//			this.OpenToolBarButton.ToolTipText = "Open Build File";
//			// 
//			// SaveToolBarButton
//			// 
//			this.SaveToolBarButton.ImageIndex = 2;
//			this.SaveToolBarButton.ToolTipText = "Save Build File";
//			// 
//			// ReloadToolBarButton
//			// 
//			this.ReloadToolBarButton.ImageIndex = 4;
//			this.ReloadToolBarButton.ToolTipText = "Reload Build File";
//			// 
//			// BuildToolBarButton
//			// 
//			this.BuildToolBarButton.ImageIndex = 7;
//			this.BuildToolBarButton.ToolTipText = "Build Default Target";
//			// 
//			// StopToolBarButton
//			// 
//			this.StopToolBarButton.ImageIndex = 3;
//			this.StopToolBarButton.ToolTipText = "Abort the Current Build";
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

//		private void MainToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
//		{
//			if (e.Button == this.BuildToolBarButton)
//			{
//				this.Build();
//			}
//			else if (e.Button == this.OpenToolBarButton)
//			{
//				this.BrowseForBuildFile();
//			}
//			else if (e.Button == this.SaveToolBarButton)
//			{
//				this.Save();
//			}
//			else if (e.Button == this.ReloadToolBarButton)
//			{
//				this.Reload();
//			}
//			else if (e.Button == this.StopToolBarButton)
//			{
//				_nantBuildRunner.Stop();
//			}
//		}

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
			this.TabControl.SelectedTab = this.OutputTabPage;
			this.OutputTextBox.Clear();
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

				Outputter.AppendRtfText(OutputHighlighter.Highlight(pMessage));

				this.OutputTextBox.SelectionStart = this.OutputTextBox.TextLength;
				this.OutputTextBox.SelectedRtf = Outputter.RtfDocument;
				this.OutputTextBox.ScrollToCaret();
			}
		}

		public void Build_Finished(object sender, NC.BuildEventArgs e)
		{
			this.Update();
		}

		private void CopyMenuCommand_Click(object sender, EventArgs e)
		{
			if (this.TabControl.SelectedTab == this.OutputTabPage)
			{
				this.OutputTextBox.Copy();
			}
			else
			{
				this.XMLRichTextBox.Copy();
			}
		}

		private void SelectAllMenuCommand_Click(object sender, EventArgs e)
		{
			if (this.TabControl.SelectedTab == this.OutputTabPage)
			{
				this.OutputTextBox.SelectAll();
			}
			else
			{
				this.XMLRichTextBox.SelectAll();
			}
		}

		private void CopyOutputText(object sender, EventArgs e)
		{
			this.OutputTextBox.Copy();
		}

		private void SelectAllOutputText(object sender, EventArgs e)
		{
			this.OutputTextBox.SelectAll();
		}

		private void CopyXmlText(object sender, EventArgs e)
		{
			this.XMLRichTextBox.Copy();
		}

		private void CutXmlText(object sender, EventArgs e)
		{
			this.XMLRichTextBox.Cut();
		}

		private void PasteXmlText(object sender, EventArgs e)
		{
			this.XMLRichTextBox.Paste();
		}

		private void SelectAllXmlText(object sender, EventArgs e)
		{
			this.XMLRichTextBox.SelectAll();
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
//			this.ReloadToolBarButton.Enabled	= false;
//			this.SaveToolBarButton.Enabled		= false;
//			this.StopToolBarButton.Enabled		= false;
//			this.BuildToolBarButton.Enabled		= false;
		}

		private void EnableMenuCommandsAndButtons()
		{
			this.MainMenu.Enable();
			this.MainToolBar.Enable();
//			this.ReloadToolBarButton.Enabled	= true;
//			this.SaveToolBarButton.Enabled		= true;
//			this.StopToolBarButton.Enabled		= true;
//			this.BuildToolBarButton.Enabled		= true;
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
			using (TextReader reader = File.OpenText(_buildFile))
			{
				this.XMLRichTextBox.Text = reader.ReadToEnd();
				_XML = this.XMLRichTextBox.Text;
			}

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

		private void WordWrapMenuCommand_Click(object sender, EventArgs e)
		{
			if (this.TabControl.SelectedTab == this.OutputTabPage)
			{
				this.MainMenu.WordWrapChecked = this.OutputTextBox.WordWrap =
					!this.OutputTextBox.WordWrap;
			}
			else if (this.TabControl.SelectedTab == this.XMLTabPage)
			{
				this.MainMenu.WordWrapChecked = this.XMLRichTextBox.WordWrap =
					!this.XMLRichTextBox.WordWrap;
			}
		}

		private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.TabControl.SelectedTab == this.OutputTabPage)
			{
				this.MainMenu.WordWrapChecked = this.OutputTextBox.WordWrap;
			}
			else if (this.TabControl.SelectedTab == this.XMLTabPage)
			{
				this.MainMenu.WordWrapChecked = this.XMLRichTextBox.WordWrap;
			}
		}

		private void CreateOutputContextMenu()
		{
			MenuCommand copy = new MenuCommand("Cop&y", new EventHandler(this.CopyOutputText));
			MenuCommand selectAll = new MenuCommand("Select &All", new EventHandler(this.SelectAllOutputText));

			_outputContextMenu.MenuCommands.AddRange(new MenuCommand[] {copy, selectAll});
		}

		private void CreateXmlContextMenu()
		{
			MenuCommand copy = new MenuCommand("Cop&y", new EventHandler(this.CopyXmlText));
			MenuCommand cut = new MenuCommand("Cu&t", new EventHandler(this.CutXmlText));
			MenuCommand paste = new MenuCommand("&Paste", new EventHandler(this.PasteXmlText));
			MenuCommand spacer = new MenuCommand("-");
			MenuCommand selectAll = new MenuCommand("Select &All", new EventHandler(this.SelectAllXmlText));

			_xmlContextMenu.MenuCommands.AddRange(new MenuCommand[] {copy, cut, paste, spacer, selectAll});
		}

		private void OutputTextBox_MouseUp(object sender, MouseEventArgs e)
		{
			RichTextBox box = sender as RichTextBox;
			if (e.Button == MouseButtons.Right)
			{
				_outputContextMenu.TrackPopup(box.PointToScreen(new Point(e.X, e.Y)));
			}
		}

		private void XMLTextBox_MouseUp(object sender, MouseEventArgs e)
		{
			RichTextBox box = sender as RichTextBox;
			if (e.Button == MouseButtons.Right)
			{
				_xmlContextMenu.TrackPopup(box.PointToScreen(new Point(e.X, e.Y)));
			}
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

		private void SaveAsMenuCommand_Click(object sender, EventArgs e)
		{
			DialogResult result = this.XMLSaveFileDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				using (TextWriter writer = File.CreateText(this.XMLSaveFileDialog.FileName))
				{
					writer.Write(this.XMLRichTextBox.Text);
				}

				this.RemoveUnsavedFileAsterisk();
				this.LoadBuildFile(this.XMLSaveFileDialog.FileName);
//				_buildFile = this.XMLSaveFileDialog.FileName;
			}
		}

		private void RemoveUnsavedFileAsterisk()
		{
			this.Text = this.Text.TrimEnd(new char[] {'*'});
		}

		public CommandLineOptions Options
		{
			set { _options = value; }
			get { return _options; }
		}
	}
}