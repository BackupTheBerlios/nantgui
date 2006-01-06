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
using System.IO;
using System.Windows.Forms;
using NAntGui.Core.Controls;
using NAntGui.Core.Controls.Menu;
using NAntGui.Core.Controls.ToolBar;
using NAntGui.Framework;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for MainFormMediator.
	/// </summary>
	public class MainFormMediator
	{
		private bool _firstLoad = true;

		private ToolBarControl _mainToolBar = new ToolBarControl();
		private ScriptTabs _sourceTabs = new ScriptTabs();
		private MainStatusBar _mainStatusBar = new MainStatusBar();
		private TargetsTreeView _targetsTree = new TargetsTreeView();
		private OutputBox _outputBox = new OutputBox();
		private MainPropertyGrid _propertyGrid = new MainPropertyGrid();
		private MainDockManager _dockManager;
		private MainStatusBar _statusBar = new MainStatusBar();
		private ToolBarControl _toolBar = new ToolBarControl();
		private MainForm _mainForm;
		private IEditCommands _editCommands;
		private MainMenuControl _mainMenu;

		public MainFormMediator()
		{
			_mainForm = new MainForm(this);
			_mainMenu = new MainMenuControl(this);

			_dockManager	= new MainDockManager(_mainForm, _sourceTabs, 
				_targetsTree, _outputBox, _propertyGrid, _statusBar);

			_toolBar.Mediator		= this;
			_mainMenu.Mediator		= this;
			_targetsTree.Mediator	= this;
			_sourceTabs.Mediator	= this;
			_outputBox.Mediator		= this;

			this.InitMainForm();
			this.AssignEventHandler();
			this.LoadInitialBuildFile();
		}

		private void InitMainForm()
		{
			MainFormSerializer.Attach(_mainForm, _propertyGrid);
	
			_sourceTabs.AddTabsToControls(_mainForm.Controls);
			_mainForm.Controls.Add(_mainStatusBar);
			_mainForm.Controls.Add(_mainToolBar);
			_mainForm.Controls.Add(_mainMenu);
		}

		private void AssignEventHandler()
		{
			EventHandler clickHandler = new EventHandler(this.ClickHandler);

			_toolBar.BuildClick		+= clickHandler;
			_toolBar.OpenClick		+= clickHandler;
			_toolBar.StopClick		+= clickHandler;
			_toolBar.SaveClick		+= clickHandler;
			_toolBar.ReloadClick	+= clickHandler;

			_mainMenu.UndoClick = clickHandler;
			_mainMenu.RedoClick = clickHandler;
			_mainMenu.CopyClick = clickHandler;
			_mainMenu.PasteClick = clickHandler;
			_mainMenu.SelectAllClick = clickHandler;
			_mainMenu.SaveOutputClick = clickHandler;
			_mainMenu.WordWrapClick = clickHandler;
			_mainMenu.ReloadClick = clickHandler;
			_mainMenu.AboutClick = clickHandler;
			_mainMenu.RunClick = clickHandler;
			_mainMenu.StopClick = clickHandler;
			_mainMenu.Recent_Click = clickHandler;
			_mainMenu.CloseClick = clickHandler;
			_mainMenu.ExitClick = clickHandler;
			_mainMenu.NAntContrib_Click = clickHandler;
			_mainMenu.NAntHelp_Click = clickHandler;
			_mainMenu.NAntSDK_Click = clickHandler;
			_mainMenu.OpenClick = clickHandler;
			_mainMenu.OptionsClick = clickHandler;
			_mainMenu.Properties_Click = clickHandler;
			_mainMenu.Save_Click = clickHandler;
			_mainMenu.SaveAs_Click = clickHandler;
			_mainMenu.Targets_Click = clickHandler;
			_mainMenu.Output_Click = clickHandler;
			_targetsTree.BuildClick = clickHandler;

			_mainForm.Closing += new CancelEventHandler(this.MainForm_Closing);
		}

		private void ClickHandler(object sender, EventArgs e)
		{
			if (sender is IClicker)
			{
				IClicker clicker = sender as IClicker;
				clicker.ExecuteClick();
			}
		}

		public void NewClicked()
		{
			throw new NotImplementedException();
		}

		public void RunClicked()
		{
			_toolBar.State = RunState.Running;
			_mainMenu.State = RunState.Running;
			_outputBox.Clear();
			_dockManager.ShowOutput();

			ScriptTabPage selectedTab = _sourceTabs.SelectedTab;
			
			selectedTab.SetProperties(_propertyGrid.GetProperties());
			selectedTab.SetTargets(_targetsTree.GetTargets());
			selectedTab.Save(false);
			selectedTab.Run();
		}

		private void Tab_BuildFinished()
		{
			this.SetStateStopped();
		}

		public void StopClicked()
		{
			_sourceTabs.SelectedTab.Stop();
			this.SetStateStopped();
		}

		public void SaveClicked()
		{
			_sourceTabs.SelectedTab.Save();
		}

		public void ReloadClicked()
		{
			this.SetStateStopped();
			_sourceTabs.SelectedTab.ReLoad();
		}

		private void SetStateStopped()
		{
			_toolBar.State = RunState.Stopped;
			_mainMenu.State = RunState.Stopped;
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

			_mainForm.Text = "NAnt-Gui";

			_targetsTree.Clear();
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
			_editCommands.SelectAll();	
		}

		public void CopyClicked()
		{
			_editCommands.Copy();	
		}

		public void PasteClicked()
		{
			_editCommands.Paste();
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
			_sourceTabs.AddTab(new ScriptTabPage(_outputBox));
		}

		private bool LoadBuildFile(string filename)
		{
			if (File.Exists(filename))
			{
				ScriptTabPage page = new ScriptTabPage(filename, _outputBox, NAntGuiApp.Options);
				page.BuildFinished = new VoidVoid(this.Tab_BuildFinished);

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
				_mainMenu.Enable();
				_toolBar.Enable();

				return true;
			}
			else
			{
				Utils.ShowFileNotFoundError(filename);
				return false;
			}
		}

		public void UpdateDisplay()
		{
			_outputBox.Clear();

			IBuildScript buildScript = _sourceTabs.SelectedTab.BuildScript;

			_mainForm.Text = string.Format("NAnt-Gui - {0}", _sourceTabs.SelectedTab.Title);

			string projectName = buildScript.HasName ? buildScript.Name : _sourceTabs.SelectedTab.FileName;

			_statusBar.Panels[0].Text = string.Format("{0} ({1})", projectName, buildScript.Description);
			_statusBar.Panels[1].Text = _sourceTabs.SelectedTab.FileFullName;

			_targetsTree.AddTargets(projectName, buildScript.Targets);
			_propertyGrid.AddProperties(buildScript.Properties, _firstLoad);

			_firstLoad = false;
		}

		public void DragDrop(DragEventArgs e)
		{
			this.LoadBuildFile(Utils.GetDragFilename(e));
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

		public void TabFocused()
		{
			_mainMenu.EnablePasteAndDelete();
			_editCommands = _sourceTabs.SelectedTab;
		}

		public void OutputFocused()
		{
			_mainMenu.DisablePasteAndDelete();
			_editCommands = _outputBox;
		}

		public void RunApplication()
		{
			Application.Run(_mainForm);
		}
	}
}
