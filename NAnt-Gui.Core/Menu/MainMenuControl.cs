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
using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu
{
	/// <summary>
	/// Summary description for MainMenuCommand.
	/// </summary>
	public class MainMenuControl : MenuControl
	{
		private FileMenu.FileMenu _fileMenu = new FileMenu.FileMenu();
		private EditMenu.EditMenu _editMenu = new EditMenu.EditMenu();
		private HelpMenu.HelpMenu _helpMenu = new HelpMenu.HelpMenu();
		private ViewMenu.ViewMenu _viewMenu = new ViewMenu.ViewMenu();
		
		private MenuCommand NAntMenuCommand = new MenuCommand();
		private MenuCommand BuildMenuCommand = new MenuCommand();
		private MenuCommand ToolsMenuCommand = new MenuCommand();
		private MenuCommand OptionsMenuCommand = new MenuCommand();
		
		public MainMenuControl()
		{
			this.Initialize();
		}

		#region Initialize

		private void Initialize()
		{
			// 
			// MainMenu
			// 
			this.AnimateStyle = Animation.System;
			this.AnimateTime = 100;
			this.Cursor = Cursors.Arrow;
			this.Direction = Direction.Horizontal;
			this.Dock = DockStyle.Top;
			this.Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.World, ((Byte) (0)));
			this.HighlightTextColor = SystemColors.MenuText;
			this.Location = new Point(0, 0);
			this.MenuCommands.AddRange(new MenuCommand[]
				{
					_fileMenu,
					_editMenu,
					this._viewMenu,
					this.NAntMenuCommand,
					this.ToolsMenuCommand,
					_helpMenu
				});
			this.Name = "MainMenu";
			this.Size = new Size(824, 25);
			this.Style = VisualStyle.IDE;
			this.TabIndex = 13;
			this.TabStop = false;
			// 
			// NAntMenuCommand
			// 
			this.NAntMenuCommand.Description = "MenuCommand";
			this.NAntMenuCommand.MenuCommands.AddRange(new MenuCommand[]
				{
					this.BuildMenuCommand
				});
			this.NAntMenuCommand.Text = "&NAnt";
			// 
			// BuildMenuCommand
			// 
			this.BuildMenuCommand.Description = "Builds the current build file";
			this.BuildMenuCommand.ImageIndex = 7;
			this.BuildMenuCommand.Shortcut = Shortcut.F5;
			this.BuildMenuCommand.Text = "&Build";
			this.BuildMenuCommand.ImageList = NAntGuiApp.ImageList;
			// 
			// ToolsMenuCommand
			// 
			this.ToolsMenuCommand.Description = "MenuCommand";
			this.ToolsMenuCommand.MenuCommands.AddRange(new MenuCommand[]
				{
					this.OptionsMenuCommand
				});
			this.ToolsMenuCommand.Text = "&Tools";
			// 
			// OptionsMenuCommand
			// 
			this.OptionsMenuCommand.Description = "MenuCommand";
			this.OptionsMenuCommand.Text = "&Options";
		}

		#endregion

		public void SetMediator(MainFormMediator mediator)
		{
			_fileMenu.SetMediator(mediator);
			_editMenu.SetMediator(mediator);
			_helpMenu.SetMediator(mediator);
			_viewMenu.SetMediator(mediator);
		}

		public void Enable()
		{
			_fileMenu.Enable();
			this.BuildMenuCommand.Enabled = true;
		}

		public void Disable()
		{
			_fileMenu.Disable();
			this.BuildMenuCommand.Enabled = false;
		}

		public void ClearRecentItems()
		{
			_fileMenu.ClearRecentItems();
		}

		public void AddRecentItem(MenuCommand item)
		{
			_fileMenu.AddRecentItem(item);
		}

		#region Events

		public EventHandler New_Click
		{
			set { _fileMenu.New_Click = value; }
		}

		public EventHandler Open_Click
		{
			set { _fileMenu.Open_Click = value; }
		}

		public EventHandler Save_Click
		{
			set { _fileMenu.Save_Click = value; }
		}

		public EventHandler SaveAs_Click
		{
			set { _fileMenu.SaveAs_Click = value; }
		}

		public EventHandler Reload_Click
		{
			set { _fileMenu.Reload_Click = value; }
		}

		public EventHandler Close_Click
		{
			set { _fileMenu.Close_Click = value; }
		}

		public EventHandler SaveOutput_Click
		{
			set { _fileMenu.SaveOutput_Click = value; }
		}

		public EventHandler Exit_Click
		{
			set { _fileMenu.Exit_Click = value; }
		}

		public EventHandler Undo_Click
		{
			set { _editMenu.Undo_Click = value; }
		}

		public EventHandler Redo_Click
		{
			set { _editMenu.Redo_Click = value; }
		}

		public EventHandler Copy_Click
		{
			set { _editMenu.Copy_Click = value; }
		}

		public EventHandler SelectAll_Click
		{
			set { _editMenu.SelectAll_Click = value; }
		}

		public EventHandler WordWrap_Click
		{
			set { _editMenu.WordWrap_Click = value; }
		}

		public EventHandler Targets_Click
		{
			set { _viewMenu.Targets_Click = value; }
		}

		public EventHandler Properties_Click
		{
			set { _viewMenu.Properties_Click = value; }
		}

		public EventHandler Output_Click
		{
			set { _viewMenu.Output_Click = value; }
		}

		public EventHandler Build_Click
		{
			set { this.BuildMenuCommand.Click += value; }
		}

		public EventHandler Options_Click
		{
			set { this.OptionsMenuCommand.Click += value; }
		}

		public EventHandler NAntHelp_Click
		{
			set { _helpMenu.NAntHelp_Click = value; }
		}

		public EventHandler NAntSDK_Click
		{
			set { _helpMenu.NAntSDK_Click = value; }
		}

		public EventHandler NAntContrib_Click
		{
			set { _helpMenu.NAntContrib_Click = value; }
		}

		public EventHandler About_Click
		{
			set { _helpMenu.About_Click = value; }
		}

		#endregion


	}
}