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
using System.Resources;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using Crownwood.Magic.Menus;
using Flobbster.Windows.Forms;
using NAnt.Core;
using NAntGui.Core.NAnt;
using Core_Project = NAnt.Core.Project;
using NC = NAnt.Core;
using Project = NAntGui.Core.NAnt.Project;
using TabControl = Crownwood.Magic.Controls.TabControl;
using Target = NAntGui.Core.NAnt.Target;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : Form
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

		private OutputBox _outputBox = new OutputBox();
		private Content _outputContent;

		private TargetsTreeView _targetsTree;
		private Content _targetsContent;
		private WindowContent _targetWindowContent;

		public PropertyGrid ProjectPropertyGrid;
		private PropertyTable _propertyTable = new PropertyTable();
		private Content _propertiesContent;

		private StatusBarPanel ProgressPanel;
		private StatusBarPanel FullFileStatusBarPanel;

		private MainMenuControl MainMenu;
		private ToolBarControl MainToolBar;

		private SourceTabControl SourceTabs;

		private SaveFileDialog OutputSaveFileDialog;
		private SaveFileDialog XMLSaveFileDialog;

		private IContainer components;
		

		#endregion

		public MainForm(CommandLineOptions options)
		{
			_options = options;

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

			_nantBuildRunner = new NAntBuildRunner(this);
			_nantBuildRunner.BuildFileLoaded += new BuildFileChangedEH(this.BuildFileLoaded);
			_nantBuildRunner.BuildFinished += new BuildEventHandler(this.Build_Finished);

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
			this.MainMenu = new MainMenuControl();
			this.MainToolBar = new ToolBarControl();
			this.ProjectPropertyGrid = new PropertyGrid();
			this.OutputSaveFileDialog = new SaveFileDialog();
			this.SourceTabs = new SourceTabControl();
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
			// MainMenu
			// 
			this.MainMenu.AnimateStyle = Animation.System;
			this.MainMenu.AnimateTime = 100;
			this.MainMenu.Cursor = Cursors.Arrow;
			this.MainMenu.Direction = Direction.Horizontal;
			this.MainMenu.Dock = DockStyle.Top;
			this.MainMenu.Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.World, ((Byte) (0)));
			this.MainMenu.HighlightTextColor = SystemColors.MenuText;
			this.MainMenu.Location = new Point(0, 0);
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.Size = new Size(824, 25);
			this.MainMenu.Style = VisualStyle.IDE;
			this.MainMenu.TabIndex = 13;
			this.MainMenu.TabStop = false;
			this.MainMenu.WordWrapChecked = false;
			// 
			// MainToolBar
			// 
			this.MainToolBar.Appearance = ToolBarAppearance.Flat;
			this.MainToolBar.DropDownArrows = true;
			this.MainToolBar.Location = new Point(0, 25);
			this.MainToolBar.Name = "MainToolBar";
			this.MainToolBar.ShowToolTips = true;
			this.MainToolBar.Size = new Size(824, 28);
			this.MainToolBar.TabIndex = 4;
			// 
			// ProjectPropertyGrid
			// 
			this.ProjectPropertyGrid.CommandsVisibleIfAvailable = true;
			this.ProjectPropertyGrid.Dock = DockStyle.Fill;
			this.ProjectPropertyGrid.LargeButtons = false;
			this.ProjectPropertyGrid.LineColor = SystemColors.ScrollBar;
			this.ProjectPropertyGrid.Location = new Point(0, 252);
			this.ProjectPropertyGrid.Name = "ProjectPropertyGrid";
			this.ProjectPropertyGrid.Size = new Size(175, 351);
			this.ProjectPropertyGrid.TabIndex = 4;
			this.ProjectPropertyGrid.Text = "Build Properties";
			this.ProjectPropertyGrid.ViewBackColor = SystemColors.Window;
			this.ProjectPropertyGrid.ViewForeColor = SystemColors.WindowText;
			// 
			// OutputSaveFileDialog
			// 
			this.OutputSaveFileDialog.DefaultExt = "txt";
			this.OutputSaveFileDialog.FileName = "Output";
			this.OutputSaveFileDialog.Filter = "Text Document|*.txt|Rich Text Format (RTF)|*.rtf";
			// 
			// SourceTabs
			// 
			this.SourceTabs.Appearance = TabControl.VisualAppearance.MultiDocument;
			this.SourceTabs.Dock = DockStyle.Fill;
			this.SourceTabs.IDEPixelArea = true;
			this.SourceTabs.IDEPixelBorder = false;
			this.SourceTabs.Location = new Point(0, 53);
			this.SourceTabs.Name = "SourceTabs";
			this.SourceTabs.SelectedIndex = 0;
			this.SourceTabs.Size = new Size(824, 478);
			this.SourceTabs.TabIndex = 12;
			this.SourceTabs.SourceRestored += new VoidVoid(this.Source_Restored);
			this.SourceTabs.SourceChanged += new VoidVoid(this.Source_Changed);
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
			this.Controls.Add(this.SourceTabs);
			this.Controls.Add(this.MainStatusBar);
			this.Controls.Add(this.MainToolBar);
			this.Controls.Add(this.MainMenu);
			
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
			this.MainToolBar.Build_Click += new VoidVoid(this.Build);
			this.MainToolBar.Open_Click += new VoidVoid(this.BrowseForBuildFile);
			this.MainToolBar.Save_Click += new VoidVoid(this.Save);
			this.MainToolBar.Reload_Click += new VoidVoid(this.Reload);
			this.MainToolBar.Stop_Click += new VoidVoid(_nantBuildRunner.Stop);
		}

		private void AssignMenuCommands()
		{
			this.MainMenu.About_Click = new EventHandler(this.AboutMenuCommand_Click);
			this.MainMenu.Build_Click = new EventHandler(this.BuildMenuCommand_Click);
			this.MainMenu.Close_Click = new EventHandler(this.CloseMenuCommand_Click);
			this.MainMenu.Copy_Click = new EventHandler(_outputBox.CopyText);
			this.MainMenu.Exit_Click = new EventHandler(this.ExitMenuCommand_Click);
			this.MainMenu.NAntContrib_Click = new EventHandler(this.NAntContribMenuCommand_Click);
			this.MainMenu.NAntHelp_Click = new EventHandler(this.NAntHelpMenuCommand_Click);
			this.MainMenu.NAntSDK_Click = new EventHandler(this.NAntSDKMenuCommand_Click);
			this.MainMenu.Open_Click = new EventHandler(this.OpenMenuCommand_Click);
			this.MainMenu.Options_Click = new EventHandler(this.OptionsMenuCommand_Click);
			this.MainMenu.Properties_Click = new EventHandler(this.PropertiesMenuCommand_Click);
			this.MainMenu.Reload_Click = new EventHandler(this.ReloadMenuCommand_Click);
			this.MainMenu.SelectAll_Click = new EventHandler(_outputBox.SelectAllText);
			this.MainMenu.SaveOutput_Click = new EventHandler(this.SaveOutputMenuCommand_Click);
			this.MainMenu.Save_Click = new EventHandler(this.SaveMenuCommand_Click);
			this.MainMenu.SaveAs_Click = new EventHandler(this.SaveAsMenuCommand_Click);
			this.MainMenu.Targets_Click = new EventHandler(this.TargetsMenuCommand_Click);
			this.MainMenu.WordWrap_Click = new EventHandler(_outputBox.DoWordWrap);
		}


		private void SetupDockManager()
		{
			// Create the object that manages the docking state
			_dockManager = new DockingManager(this, VisualStyle.IDE);

			// Ensure that the RichTextBox is always the innermost control
			_dockManager.InnerControl = this.SourceTabs;

			_targetsContent		= _dockManager.Contents.Add(_targetsTree, "Targets");
			_outputContent		= _dockManager.Contents.Add(_outputBox, "Output");
			_propertiesContent	= _dockManager.Contents.Add(this.ProjectPropertyGrid, "Properties");

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
				_nantBuildRunner.LoadBuildFile(buildFile);
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
			_nantBuildRunner.Run(_buildFile);
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
			About lAbout = new About();
			lAbout.ShowDialog();
		}

		public void OutputMessage(string message)
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

		public void Build_Finished(object sender, BuildEventArgs e)
		{
			this.Update();
		}

		public void CloseBuildFile()
		{
//			_buildFile.Close();
			_buildFile = "";
			this.ClearOutput();
			_targetsTree.Nodes.Clear();
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

			_targetsTree.Nodes.Clear();
			_targetsTree.Nodes.Add(new TreeNode(projectName));
		}


		private void AddTargets(Project project)
		{
			foreach (Target target in project.Targets)
			{
				this.AddTargetTreeNode(target);
			}

			_targetsTree.ExpandAll();
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

		public void AddTreeTargetsToBuild(Core_Project project)
		{
			foreach (TreeNode lItem in _targetsTree.Nodes[0].Nodes)
			{
				if (lItem.Checked)
				{
					project.BuildTargets.Add(lItem.Text);
				}
			}
		}

		public void AddPropertiesToProject(Core_Project project)
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

		private void LoadNonProjectProperty(PropertySpec spec, Core_Project project)
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

		private bool ValidTarget(string category, Core_Project project)
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
				this.SourceTabs.SaveSource(_buildFile);
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
			this.MainMenu.WordWrapChecked = checkValue;
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
			_nantBuildRunner.Stop();
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
