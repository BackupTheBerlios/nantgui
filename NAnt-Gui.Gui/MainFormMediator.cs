#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2007 Colin Svingen
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

using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using NAntGui.Gui.Controls;
using NAntGui.Gui.Controls.Menu;
using NAntGui.Framework;

namespace NAntGui.Gui
{
	/// <summary>
	/// Summary description for MainFormMediator.
	/// </summary>
	public class MainFormMediator
	{
		private MainPropertyGrid _propertyGrid = new MainPropertyGrid();
		private MainStatusBar _statusBar = new MainStatusBar();

		private ScriptTabs _sourceTabs;
		private TargetsTreeView _targetsTree;
		private OutputBox _outputBox;
		private MainDockManager _dockManager;
		private ToolBarControl _toolBar;
		private MainForm _form;
		private IEditCommands _editCommands;
		private MainMenuControl _mainMenu;

		public MainFormMediator()
		{
			_form			= new MainForm(this);
			_mainMenu		= new MainMenuControl(this);
			_toolBar		= new ToolBarControl(this);
			_sourceTabs		= new ScriptTabs(this);
			_outputBox		= new OutputBox(this);
			_targetsTree	= new TargetsTreeView(this);

			InitMainForm();

			_dockManager = new MainDockManager(_form, _sourceTabs,
				_targetsTree, _outputBox, 
				_propertyGrid, _statusBar);

			LoadInitialBuildFile();
		}

		private void InitMainForm()
		{
			MainFormSerializer.Attach(_form, _propertyGrid);

			_sourceTabs.AddTabsToControls(_form.Controls);
			_form.Controls.Add(_statusBar);
			_form.Controls.Add(_toolBar);
			_form.Controls.Add(_mainMenu);
		}

		public void NewClicked()
		{
//			if (CloseTabs())
//			{
//				_form.Text = "NAnt-Gui";

//				_propertyGrid.Clear();
//				_outputBox.Clear();
//				_targetsTree.Clear();
//
//				_mainMenu.Disable();
//				_toolBar.Disable();

				AddBlankTab();
//			}
		}

		public void RunClicked()
		{
			_toolBar.State = RunState.Running;
			_mainMenu.RunState = RunState.Running;
			_outputBox.Clear();
			_dockManager.ShowOutput();

			ScriptTabPage tab = _sourceTabs.SelectedTab;

			tab.Save(false);
			tab.SetProperties(_propertyGrid.GetProperties());
			tab.SetTargets(_targetsTree.GetTargets());			
			tab.Run();
		}

		private void Tab_BuildFinished()
		{
			SetStateStopped();
		}

		public void StopClicked()
		{
			_sourceTabs.SelectedTab.Stop();
			SetStateStopped();
		}

		public void SaveClicked()
		{
			_sourceTabs.SelectedTab.Save(true);
			_mainMenu.Enable();
			_toolBar.Enable();
		}

		public void SaveAsClicked()
		{
			string file = BuildFileBrowser.BrowseForSave();
			if (file != null)
			{
				_sourceTabs.SelectedTab.SaveAs(file);
				_sourceTabs.SelectedTab.BuildFinished = new VoidVoid(Tab_BuildFinished);

				Settings.OpenInitialDir = _sourceTabs.SelectedTab.FilePath;

				_mainMenu.AddRecentItem(file);

				_mainMenu.Enable();
				_toolBar.Enable();
			}
		}

		public void ReloadClicked()
		{
			SetStateStopped();
			_sourceTabs.SelectedTab.ReLoad();
		}

		private void SetStateStopped()
		{
			_toolBar.State = RunState.Stopped;
			_mainMenu.RunState = RunState.Stopped;
		}

		public void OpenClicked()
		{
			foreach (string filename in BuildFileBrowser.BrowseForLoad())
			{
				LoadBuildFile(filename);
			}
		}

		public void ExitClicked()
		{
			_form.Close();
		}

		/// <summary>
		/// Called with the Close File menu item is clicked.
		/// </summary>
		public void CloseClicked()
		{
			CancelEventArgs e = new CancelEventArgs();
			_sourceTabs.CloseSelectedTab(e);

			if (!e.Cancel && _sourceTabs.TabCount == 0)
			{
				NewClicked();
			}
		}

		public void AboutClicked()
		{
			About about = new About();
			about.ShowDialog();
		}

		public void MainFormClosing(CancelEventArgs e)
		{
			_sourceTabs.SelectedTab.Stop();
			_sourceTabs.CloseTabs(e);
			_dockManager.SaveConfig();
		}

		public void SaveOutputClicked()
		{
			_outputBox.SaveOutput();
		}

		public void RecentItemClicked(string file)
		{
			if (File.Exists(file))
			{
				LoadBuildFile(file);
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
			if (NAntGuiApp.Options.BuildFile == null || !LoadBuildFile(NAntGuiApp.Options.BuildFile))
			{
				if (!_mainMenu.HasRecentItems || !LoadBuildFile(_mainMenu.FirstRecentItem))
				{
					AddBlankTab();
				}
			}
		}

		private bool CloseTabs()
		{
			CancelEventArgs e = new CancelEventArgs();
			_sourceTabs.CloseTabs(e);
			return !e.Cancel;
		}

		private void AddBlankTab()
		{
//			_sourceTabs.Clear();
			ScriptTabPage page = new ScriptTabPage(_outputBox, this);
			_sourceTabs.AddTab(page);			
		}

		private bool LoadBuildFile(string filename)
		{
			if (CloseTabs())
			{
				if (File.Exists(filename))
				{
					ScriptTabPage page = new ScriptTabPage(filename, _outputBox, this);
					page.BuildFinished = new VoidVoid(Tab_BuildFinished);

					Settings.OpenInitialDir = page.FilePath;

//					_sourceTabs.Clear();
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

					_mainMenu.Enable();
					_toolBar.Enable();
					UpdateDisplay(true);

					return true;
				}
				else
				{
					Utils.ShowFileNotFoundError(filename);
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		public void UpdateDisplay(bool firstLoad)
		{
			_outputBox.Clear();

			IBuildScript buildScript = _sourceTabs.SelectedTab.BuildScript;

			_form.Text = string.Format("NAnt-Gui - {0}", _sourceTabs.SelectedTab.Title);

			string projectName = buildScript.HasName ? buildScript.Name : _sourceTabs.SelectedTab.FileName;

			_statusBar.Panels[0].Text = string.Format("{0} ({1})", projectName, buildScript.Description);
			_statusBar.Panels[1].Text = _sourceTabs.SelectedTab.FileFullName;

			_targetsTree.AddTargets(projectName, buildScript.Targets);
			_propertyGrid.AddProperties(buildScript.Properties, firstLoad);
		}

		public void DragDrop(DragEventArgs e)
		{
			LoadBuildFile(Utils.GetDragFilename(e));
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

		public void TabGotFocus()
		{
			_mainMenu.EditState = EditState.TabFocused;
			_editCommands = _sourceTabs.SelectedTab;
		}

		public void TabLostFocus()
		{
			_mainMenu.EditState = EditState.NoFocus;
			_editCommands = null;
		}

		public void OutputGotFocused()
		{
			_mainMenu.EditState = EditState.OutputFocused;
			_editCommands = _outputBox;
		}

		public void OutputLostFocused()
		{
			_mainMenu.EditState = EditState.NoFocus;
			_editCommands = null;
		}

		public void RunApplication()
		{
			Application.Run(_form);
		}

		public void CutClicked()
		{
			_editCommands.Cut();
		}

		public void DeleteClicked()
		{
			_editCommands.Delete();
		}

		public void TabIndexChanged()
		{
			;
		}
	}
}