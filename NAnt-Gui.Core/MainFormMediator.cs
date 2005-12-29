using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using NAntGui.Core.Menu;
using NAntGui.Core.ToolBar;
using NAntGui.Framework;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for MainFormMediator.
	/// </summary>
	public class MainFormMediator
	{
		private bool _firstLoad = true;

		private MainDockManager _dockManager;
		private SourceTabControl _sourceTabs;
		private TargetsTreeView _targetsTree;
		private OutputBox _outputBox;
		private MainPropertyGrid _propertyGrid;
		private MainStatusBar _statusBar;
		private MainMenuControl _mainMenu;
		private ToolBarControl _toolBar;
		private MainForm _mainForm;

		public MainFormMediator(MainForm mainForm, SourceTabControl sourceTabs, 
            TargetsTreeView targetsTree, OutputBox outputBox, MainPropertyGrid propertyGrid,
            MainStatusBar statusBar, MainMenuControl mainMenu, ToolBarControl toolBar)
		{
			_mainForm		= mainForm;
			_sourceTabs		= sourceTabs;
			_targetsTree	= targetsTree;
			_outputBox		= outputBox;
			_propertyGrid	= propertyGrid;
			_statusBar		= statusBar;
			_mainMenu		= mainMenu;
			_toolBar		= toolBar;

			_dockManager	= new MainDockManager(mainForm, _sourceTabs, _targetsTree, 
				_outputBox, _propertyGrid, _statusBar);

			_toolBar.SetMediator(this);
			_mainMenu.SetMediator(this);
			_targetsTree.Mediator = this;

			this.AssignEventHandler();
		}

		private void AssignEventHandler()
		{
			EventHandler clickHandler = new EventHandler(this.ClickHandler);

			_toolBar.BuildClick		+= clickHandler;
			_toolBar.OpenClick		+= clickHandler;
			_toolBar.StopClick		+= clickHandler;
			_toolBar.SaveClick		+= clickHandler;
			_toolBar.ReloadClick	+= clickHandler;

			_mainMenu.Undo_Click = clickHandler;
			_mainMenu.Redo_Click = clickHandler;
			_mainMenu.Copy_Click = clickHandler;
			_mainMenu.SelectAll_Click = clickHandler;
			_mainMenu.SaveOutput_Click = clickHandler;
			_mainMenu.WordWrap_Click = clickHandler;
			_mainMenu.Reload_Click = clickHandler;
			_mainMenu.About_Click = clickHandler;
			_mainMenu.Build_Click = clickHandler;
			_mainMenu.Recent_Click = clickHandler;
			_mainMenu.Close_Click = clickHandler;
			_mainMenu.Exit_Click = clickHandler;
			_mainMenu.NAntContrib_Click = clickHandler;
			_mainMenu.NAntHelp_Click = clickHandler;
			_mainMenu.NAntSDK_Click = clickHandler;
			_mainMenu.Open_Click = clickHandler;
			_mainMenu.Options_Click = clickHandler;
			_mainMenu.Properties_Click = clickHandler;
			_mainMenu.Save_Click = clickHandler;
			_mainMenu.SaveAs_Click = clickHandler;
			_mainMenu.Targets_Click = clickHandler;
			_mainMenu.Output_Click = clickHandler;
			_targetsTree.BuildClick = clickHandler;

			_mainForm.Closing += new CancelEventHandler(this.MainForm_Closing);

			DragEventHandler dropHandler = new DragEventHandler(this.DropHandler);
			_mainForm.DragDrop += dropHandler;
			_sourceTabs.DragDrop = dropHandler;

			DragEventHandler dragHandler = new DragEventHandler(this.DragHandler);
			_mainForm.DragEnter += dragHandler;
			_sourceTabs.DragEnter = dragHandler;
		}

		private void ClickHandler(object sender, EventArgs e)
		{
			if (sender is IClicker)
			{
				IClicker clicker = sender as IClicker;
				clicker.ExecuteClick();
			}
		}

		private void DropHandler(object sender, DragEventArgs e)
		{
			if (sender is IDragDropper)
			{
				IDragDropper dropper = sender as IDragDropper;
				dropper.ExecuteDragDrop(e);
			}
		}

		private void DragHandler(object sender, DragEventArgs e)
		{
			if (sender is IDragDropper)
			{
				IDragDropper dropper = sender as IDragDropper;
				dropper.ExecuteDragEnter(e);
			}
		}

		public void NewClicked()
		{
			throw new NotImplementedException();
		}

		public void BuildClicked()
		{
			_toolBar.DisableRun();
			_toolBar.EnableStop();
			_outputBox.Clear();
			_dockManager.ShowOutput();

			ScriptTabPage selectedTab = _sourceTabs.SelectedTab;

			selectedTab.Save();
			selectedTab.SetProperties(_propertyGrid.GetProperties());
			selectedTab.SetTargets(_targetsTree.GetTargets());
			selectedTab.Run();

			_toolBar.DisableStop();
			_toolBar.EnableRun();
		}

		public void StopClicked()
		{
			_toolBar.DisableStop();
			_sourceTabs.SelectedTab.Stop();
			_toolBar.EnableRun();
		}

		public void SaveClicked()
		{
			_sourceTabs.SelectedTab.Save();
		}

		public void ReloadClicked()
		{
			_sourceTabs.SelectedTab.ReLoad();
		}

		public void OpenClicked()
		{
			foreach (string filename in BuildFileBrowser.BrowseForLoad())
			{
				this.LoadBuildFile(filename);
			}
		}

		public void ExitClicked()
		{
			_mainForm.Close();
		}

		public void CloseClicked()
		{
			_sourceTabs.CloseSelectedTab();
			this.AddBlankTab();

			_targetsTree.Nodes.Clear();
			_propertyGrid.SelectedObject = null;

			_outputBox.Clear();

			_mainMenu.Disable();
			_toolBar.Disable();
		}

		public void AboutClicked()
		{
			About about = new About();
			about.ShowDialog();
		}

		private void MainForm_Closing(object sender, CancelEventArgs e)
		{
			_sourceTabs.SelectedTab.Stop();
			_sourceTabs.CloseTabs(e);
			_dockManager.SaveConfig();	
		}

		public void SaveAsClicked()
		{
			string file = BuildFileBrowser.BrowseForSave();
			if (file != null)
			{
				_sourceTabs.SelectedTab.SaveAs(file);
				//this.LoadBuildFile(this.XMLSaveFileDialog.FileName);
			}
		}

		public void SaveOutputClicked()
		{
			_outputBox.SaveOutput();
		}

		public void RecentItemClicked(string file)
		{
			if (File.Exists(file))
			{
				this.LoadBuildFile(file);
			}
			else
			{
				_mainMenu.RemoveRecentItem(file);
				Utils.ShowFileNotFoundError(file);
			}
		}

		public void WordWrapClicked()
		{
			_outputBox.DoWordWrap();
		}

		public void SelectAllClicked()
		{
			_outputBox.SelectAll();
		}

		public void CopyClicked()
		{
			_outputBox.Copy();
		}

		public void RedoClicked()
		{
			_sourceTabs.SelectedTab.Redo();
		}

		public void UndoClicked()
		{
			_sourceTabs.SelectedTab.Undo();
		}

		public void NAntHelpClicked()
		{
			const string nantHelp = @"\..\nant-docs\help\index.html";
			Utils.LoadHelpFile(Utils.RunDirectory + nantHelp);
		}

		public void NAntContribClicked()
		{
			const string nantContribHelp = @"\..\nantcontrib-docs\help\index.html";
			Utils.LoadHelpFile(Utils.RunDirectory + nantContribHelp);
		}

		public void NAntSDKClicked()
		{
			const string nantHelpPath = @"\..\nant-docs\sdk\";
			const string nantSDKHelp = "NAnt-SDK.chm";
			string filename = Utils.RunDirectory + nantHelpPath + nantSDKHelp;

			Utils.LoadHelpFile(filename);
		}

		public void OutputClicked()
		{
			_dockManager.ShowOutput();
		}

		public void TargetsClicked()
		{
			_dockManager.ShowTargets();
		}

		public void PropertiesClicked()
		{
			_dockManager.ShowProperties();
		}

		public void RunClicked()
		{
			throw new NotImplementedException();
		}

		public void OptionsClicked()
		{
			OptionsForm optionsForm = new OptionsForm();
			optionsForm.ShowDialog();
		}

		public void LoadInitialBuildFile()
		{
			if (NAntGuiApp.Options.BuildFile == null || !this.LoadBuildFile(NAntGuiApp.Options.BuildFile))
			{
				if (!_mainMenu.HasRecentItems || !this.LoadBuildFile(_mainMenu.FirstRecentItem))
				{
					this.AddBlankTab();
				}
			}
		}

		public void AddBlankTab()
		{
			_sourceTabs.Clear();
			_sourceTabs.AddTab(new ScriptTabPage(_mainForm));
		}

		private bool LoadBuildFile(string filename)
		{
			if (File.Exists(filename))
			{
				ScriptTabPage page = new ScriptTabPage(filename, _mainForm, NAntGuiApp.Options);
				page.SourceChanged += new VoidVoid(this.Source_Changed);
				page.BuildFinished = new VoidVoid(_mainForm.Update);

				Settings.OpenInitialDirectory = page.FilePath;

				_sourceTabs.Clear();
				_sourceTabs.AddTab(page);
				
				string file = _sourceTabs.SelectedTab.FileFullName;
				_mainMenu.AddRecentItem(file);

				try
				{
					page.ParseBuildScript();	
				}
				catch (BuildFileLoadException error)
				{
					MessageBox.Show(error.Message, "Error Loading Build File", 
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				
				this.UpdateDisplay();

				return true;
			}
			else
			{
				Utils.ShowFileNotFoundError(filename);
				return false;
			}
		}

		private void UpdateDisplay()
		{
			_outputBox.Clear();

			IBuildScript buildScript = _sourceTabs.SelectedTab.BuildScript;

			_mainForm.Text = string.Format("NAnt-Gui - {0}", _sourceTabs.SelectedTab.Title);

			string projectName = buildScript.HasName ? buildScript.Name : _sourceTabs.SelectedTab.FileName;

			_statusBar.Panels[0].Text = string.Format("{0} ({1})", projectName, buildScript.Description);
			_statusBar.Panels[1].Text = _sourceTabs.SelectedTab.FileFullName;

			this.EnableMenuCommandsAndButtons();

			_targetsTree.AddTargets(projectName, buildScript.Targets);
			_propertyGrid.AddProperties(buildScript.Properties, _firstLoad);

			_firstLoad = false;
		}

		public void DragDrop(DragEventArgs e)
		{
			this.LoadBuildFile(Utils.GetDragFilename(e));
		}

		private void EnableMenuCommandsAndButtons()
		{
			_mainMenu.Enable();
			_toolBar.Enable();
		}

		private void Source_Changed()
		{
			this.UpdateDisplay();
		}

		//		private void WordWrap_Changed(bool checkValue)
		//		{
		//			_mainMenu.WordWrapChecked = checkValue;
		//		}


		public void DragEnter(DragEventArgs e)
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
	}
}
