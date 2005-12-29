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
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using Crownwood.Magic.Menus;
using NAntGui.Core.Menu;
using NAntGui.Core.ToolBar;
using NAntGui.Framework;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : Form, ILogsMessage
	{
		private delegate void MessageEventHandler(string pMessage);
		
		private bool _firstLoad = true;

		private RecentItems _recentItems = new RecentItems();
		private OutputBox _outputBox = new OutputBox();
		private TargetsTreeView _targetsTree = new TargetsTreeView();
		public MainPropertyGrid _propertyGrid = new MainPropertyGrid();
		private MainMenuControl _mainMenu = new MainMenuControl();
		private ToolBarControl _mainToolBar = new ToolBarControl();
		private SourceTabControl _sourceTabs = new SourceTabControl();
		private MainStatusBar _mainStatusBar = new MainStatusBar();

		private MainFormMediator _mediator;

		private IContainer components;


		public MainForm()
		{
			this.Initialize();

			_mediator = new MainFormMediator(this, _sourceTabs, _targetsTree, 
				_outputBox, _propertyGrid, _mainStatusBar, _mainMenu, _mainToolBar);

			_recentItems.ItemsUpdated += new VoidVoid(this.UpdateRecentItemsMenu);
			_recentItems.Load();
			
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
			this.SuspendLayout();
			
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
			this.DragDrop += new DragEventHandler(this.NAnt_DragDrop);
			this.Closed += new EventHandler(this.NAnt_Closed);
			this.DragEnter += new DragEventHandler(this.NAnt_DragEnter);
			this.ResumeLayout(false);
		}

		#endregion

		private void LoadInitialBuildFile()
		{
			if (NAntGuiApp.Options.BuildFile == null || !this.LoadBuildFile(NAntGuiApp.Options.BuildFile))
			{
				if (!_recentItems.HasRecentItems || !this.LoadBuildFile(_recentItems[0]))
				{
					_sourceTabs.Clear();
					_sourceTabs.AddTab(new ScriptTabPage(this));
				}
			}
		}

		private bool LoadBuildFile(string filename)
		{
			if (File.Exists(filename))
			{
				ScriptTabPage page = new ScriptTabPage(filename, this, NAntGuiApp.Options);
				page.SourceChanged += new VoidVoid(this.Source_Changed);
				page.BuildFinished = new VoidVoid(this.Update);

				Settings.OpenInitialDirectory = page.File.Path;

				_sourceTabs.Clear();
				_sourceTabs.AddTab(page);
				
				this.AddRecentItem();

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
				ShowFileNotFoundError(filename);
				return false;
			}
		}

		private void AddRecentItem()
		{
			_recentItems.Add(_sourceTabs.SelectedTab.File.FullName);
			_recentItems.Save();
			this.UpdateRecentItemsMenu();
		}

		private void UpdateDisplay()
		{
			_outputBox.Clear();

			IBuildScript buildScript = _sourceTabs.SelectedTab.BuildScript;

			this.Text = string.Format("NAnt-Gui - {0}", _sourceTabs.SelectedTab.Title);

			string projectName = buildScript.HasName ? buildScript.Name : _sourceTabs.SelectedTab.File.Name;

			_mainStatusBar.Panels[0].Text = string.Format("{0} ({1})", projectName, buildScript.Description);
			_mainStatusBar.Panels[1].Text = _sourceTabs.SelectedTab.File.FullName;

			this.EnableMenuCommandsAndButtons();

			_targetsTree.AddTargets(projectName, buildScript.Targets);
			_propertyGrid.AddProperties(buildScript.Properties, _firstLoad);

			_firstLoad = false;
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

		public void BrowseForBuildFile()
		{
			foreach (string filename in BuildFileBrowser.BrowseForLoad())
			{
				this.LoadBuildFile(filename);
			}
		}

		private void NAnt_Closed(object sender, EventArgs e)
		{
			_recentItems.Save();
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

		private void EnableMenuCommandsAndButtons()
		{
			_mainMenu.Enable();
			_mainToolBar.Enable();
		}

		private void OptionsMenuCommand_Click(object sender, EventArgs e)
		{
			OptionsForm optionsForm = new OptionsForm();
			optionsForm.ShowDialog();
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

			if (File.Exists(recentItem))
			{
				this.LoadBuildFile(recentItem);
			}
			else
			{
				_recentItems.Remove(recentItem);
				ShowFileNotFoundError(recentItem);
			}
		}

		private static void ShowFileNotFoundError(string recentItem)
		{
			MessageBox.Show(recentItem + " was not found.", "Build File Not Found", 
			                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

//		private void WordWrap_Changed(bool checkValue)
//		{
//			_mainMenu.WordWrapChecked = checkValue;
//		}

		private void Source_Changed()
		{
			this.UpdateDisplay();
		}

		public PropertySort PropertySort
		{
			get { return _propertyGrid.PropertySort; }
			set
			{
				try	{ _propertyGrid.PropertySort = value; }
				catch (ArgumentException) { /* ignore */ }
			}
		}
	}
}
