using System;
using System.ComponentModel;
using NAntGui.Core.Menu;
using NAntGui.Core.ToolBar;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for MainFormMediator.
	/// </summary>
	public class MainFormMediator
	{
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
//			_targetsTree.SetMediator(this);
//			_outputBox.SetMediator(this);
//			_propertyGrid.SetMediator(this);
//			_statusBar.SetMediator(this);
			_mainMenu.SetMediator(this);

			this.AssignEventHandler();
		}

		private void AssignEventHandler()
		{
			EventHandler eventHandler = new EventHandler(this.ClickHandler);

			_toolBar.Build_Click	+= eventHandler;
			_toolBar.Open_Click		+= eventHandler;
			_toolBar.Stop_Click		+= eventHandler;
			_toolBar.Save_Click		+= eventHandler;
			_toolBar.Reload_Click	+= eventHandler;

			_mainMenu.Undo_Click = eventHandler;
			_mainMenu.Redo_Click = eventHandler;
			_mainMenu.Copy_Click = eventHandler;
			_mainMenu.SelectAll_Click = eventHandler;
			_mainMenu.SaveOutput_Click = eventHandler;
			_mainMenu.WordWrap_Click = eventHandler;
			_mainMenu.Reload_Click = eventHandler;
			_mainMenu.About_Click = eventHandler;
			_mainMenu.Build_Click = eventHandler;
			_mainMenu.Close_Click = eventHandler;
			_mainMenu.Exit_Click = eventHandler;
			_mainMenu.NAntContrib_Click = eventHandler;
			_mainMenu.NAntHelp_Click = eventHandler;
			_mainMenu.NAntSDK_Click = eventHandler;
			_mainMenu.Open_Click = eventHandler;
			_mainMenu.Options_Click = eventHandler;
			_mainMenu.Properties_Click = eventHandler;
			_mainMenu.Save_Click = eventHandler;
			_mainMenu.SaveAs_Click = eventHandler;
			_mainMenu.Targets_Click = eventHandler;
			_targetsTree.BuildClick = eventHandler;

			_mainForm.Closing += new CancelEventHandler(this.MainForm_Closing);
		}

		private void ClickHandler(object sender, EventArgs e)
		{
			if (sender is Command)
			{
				Command cmd = sender as Command;
				cmd.Execute();
			}
		}

		public void ShowTargetsClicked(object sender, EventArgs e)
		{
			_dockManager.ShowTargets();
		}

		public void ShowPropertiesClicked(object sender, EventArgs e)
		{
			_dockManager.ShowProperties();
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
			_mainForm.BrowseForBuildFile();
		}

		public void ExitClicked()
		{
			_mainForm.Close();
		}

		public void CloseClicked()
		{
			_sourceTabs.SelectedTab.File.Close();
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

		public void RecentClicked()
		{
			throw new NotImplementedException();
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
	}
}
