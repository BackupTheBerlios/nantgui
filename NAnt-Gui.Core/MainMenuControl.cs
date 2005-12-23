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

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for MainMenuCommand.
	/// </summary>
	public class MainMenuControl : MenuControl
	{
		#region MenuCommands

		private MenuCommand ExitMenuCommand = new MenuCommand();
		private MenuCommand MenuCommand4 = new MenuCommand();
		private MenuCommand MenuCommand5 = new MenuCommand();
		private MenuCommand FileMenuCommand = new MenuCommand();
		private MenuCommand NewMenuCommand = new MenuCommand();
		private MenuCommand OpenMenuCommand = new MenuCommand();
		private MenuCommand SaveMenuCommand = new MenuCommand();
		private MenuCommand SaveAsMenuCommand = new MenuCommand();
		private MenuCommand CloseMenuCommand = new MenuCommand();
		private MenuCommand RecentMenuCommand = new MenuCommand();
		private MenuCommand ReloadMenuCommand = new MenuCommand();
		private MenuCommand HelpMenuCommand = new MenuCommand();
		private MenuCommand AboutMenuCommand = new MenuCommand();
		private MenuCommand NAntMenuCommand = new MenuCommand();
		private MenuCommand BuildMenuCommand = new MenuCommand();
		private MenuCommand EditMainMenuCommand = new MenuCommand();
		private MenuCommand UndoMenuCommand = new MenuCommand();
		private MenuCommand RedoMenuCommand = new MenuCommand();
		private MenuCommand CopyMenuCommand = new MenuCommand();
		private MenuCommand SelectAllMenuCommand = new MenuCommand();
		private MenuCommand ToolsMenuCommand = new MenuCommand();
		private MenuCommand OptionsMenuCommand = new MenuCommand();
		private MenuCommand NAntContribMenuCommand = new MenuCommand();
		private MenuCommand NAntHelpMenuCommand = new MenuCommand();
		private MenuCommand NAntSDKMenuCommand = new MenuCommand();
		private MenuCommand MenuCommand1 = new MenuCommand();
		private MenuCommand SaveOutputMenuCommand = new MenuCommand();
		private MenuCommand WordWrapMenuCommand = new MenuCommand();
		private MenuCommand ViewMenuCommand = new MenuCommand();
		private MenuCommand TargetsMenuCommand = new MenuCommand();
		private MenuCommand PropertiesMenuCommand = new MenuCommand();

		#endregion

		private ImageList _imageList;

		public MainMenuControl()
		{
			this.LoadImageList();
			this.SetProperties();
			this.AssignImages();
		}

		private void LoadImageList()
		{
			_imageList = ResourceHelper.LoadBitmapStrip(
				this.GetType(), "NAntGui.Core.Images.MenuItems.bmp",
				new Size(16, 16), new Point(0, 0));
		}

		#region Set Properties

		private void SetProperties()
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
					this.FileMenuCommand,
					this.EditMainMenuCommand,
					this.ViewMenuCommand,
					this.NAntMenuCommand,
					this.ToolsMenuCommand,
					this.HelpMenuCommand
				});
			this.Name = "MainMenu";
			this.Size = new Size(824, 25);
			this.Style = VisualStyle.IDE;
			this.TabIndex = 13;
			this.TabStop = false;
			// 
			// FileMenuCommand
			// 
			this.FileMenuCommand.Description = "MenuCommand";
			this.FileMenuCommand.MenuCommands.AddRange(new MenuCommand[]
				{
					this.NewMenuCommand, 
					this.OpenMenuCommand,
					this.SaveMenuCommand,
					this.SaveAsMenuCommand,
					this.ReloadMenuCommand,
					this.CloseMenuCommand,
					this.SaveOutputMenuCommand,
					this.MenuCommand4,
					this.RecentMenuCommand,
					this.MenuCommand5,
					this.ExitMenuCommand
				});
			this.FileMenuCommand.Text = "&File";
			// 
			// NewMenuCommand
			// 
			this.NewMenuCommand.Description = "MenuCommand";
			this.NewMenuCommand.ImageIndex = 8;
			this.NewMenuCommand.Shortcut = Shortcut.CtrlN;
			this.NewMenuCommand.Text = "&New";
			this.NewMenuCommand.Enabled = false;
			// 
			// OpenMenuCommand
			// 
			this.OpenMenuCommand.Description = "MenuCommand";
			this.OpenMenuCommand.ImageIndex = 5;
			this.OpenMenuCommand.Shortcut = Shortcut.CtrlO;
			this.OpenMenuCommand.Text = "&Open";
			// 
			// SaveMenuCommand
			// 
			this.SaveMenuCommand.Description = "MenuCommand";
			this.SaveMenuCommand.ImageIndex = 2;
			this.SaveMenuCommand.Shortcut = Shortcut.CtrlS;
			this.SaveMenuCommand.Text = "&Save";
			// 
			// SaveAsMenuCommand
			// 
			this.SaveAsMenuCommand.Description = "MenuCommand";
			this.SaveAsMenuCommand.ImageIndex = 2;
			this.SaveAsMenuCommand.Shortcut = Shortcut.F12;
			this.SaveAsMenuCommand.Text = "Save &As";
			// 
			// ReloadMenuCommand
			// 
			this.ReloadMenuCommand.Description = "MenuCommand";
			this.ReloadMenuCommand.ImageIndex = 4;
			this.ReloadMenuCommand.Shortcut = Shortcut.CtrlR;
			this.ReloadMenuCommand.Text = "&Reload";
			// 
			// CloseMenuCommand
			// 
			this.CloseMenuCommand.Description = "MenuCommand";
			this.CloseMenuCommand.Shortcut = Shortcut.CtrlW;
			this.CloseMenuCommand.Text = "&Close";
			// 
			// SaveOutputMenuCommand
			// 
			this.SaveOutputMenuCommand.Description = "MenuCommand";
			this.SaveOutputMenuCommand.Shortcut = Shortcut.CtrlU;
			this.SaveOutputMenuCommand.Text = "Save O&utput";
			// 
			// MenuCommand4
			// 
			this.MenuCommand4.Description = "MenuCommand";
			this.MenuCommand4.Text = "-";
			// 
			// RecentMenuCommand
			// 
			this.RecentMenuCommand.Description = "MenuCommand";
			this.RecentMenuCommand.Text = "Recent &Files";
			// 
			// MenuCommand5
			// 
			this.MenuCommand5.Description = "MenuCommand";
			this.MenuCommand5.Text = "-";
			// 
			// ExitMenuCommand
			// 
			this.ExitMenuCommand.Description = "MenuCommand";
			this.ExitMenuCommand.Text = "E&xit";
			// 
			// EditMainMenuCommand
			// 
			this.EditMainMenuCommand.Description = "MenuCommand";
			this.EditMainMenuCommand.MenuCommands.AddRange(new MenuCommand[]
				{
					this.UndoMenuCommand,
					this.RedoMenuCommand,
					new MenuCommand("-"), 
					this.CopyMenuCommand,
					this.SelectAllMenuCommand,
					this.WordWrapMenuCommand
				});
			this.EditMainMenuCommand.Text = "&Edit";
			// 
			// UndoMenuCommand
			// 
			this.UndoMenuCommand.Description = "MenuCommand";
			this.UndoMenuCommand.Shortcut = Shortcut.CtrlZ;
			this.UndoMenuCommand.Text = "&Undo";
			// 
			// RedoMenuCommand
			// 
			this.RedoMenuCommand.Description = "MenuCommand";
			this.RedoMenuCommand.Shortcut = Shortcut.CtrlY;
			this.RedoMenuCommand.Text = "&Redo";
			// 
			// CopyMenuCommand
			// 
			this.CopyMenuCommand.Description = "MenuCommand";
			this.CopyMenuCommand.Shortcut = Shortcut.CtrlC;
			this.CopyMenuCommand.Text = "Cop&y";
			// 
			// SelectAllMenuCommand
			// 
			this.SelectAllMenuCommand.Description = "MenuCommand";
			this.SelectAllMenuCommand.Shortcut = Shortcut.CtrlA;
			this.SelectAllMenuCommand.Text = "Select &All";
			// 
			// WordWrapMenuCommand
			// 
			this.WordWrapMenuCommand.Description = "MenuCommand";
			this.WordWrapMenuCommand.Text = "&Word Wrap";
			// 
			// ViewMenuCommand
			// 
			this.ViewMenuCommand.Description = "MenuCommand";
			this.ViewMenuCommand.MenuCommands.AddRange(new MenuCommand[]
				{
					this.TargetsMenuCommand,
					this.PropertiesMenuCommand
				});
			this.ViewMenuCommand.Text = "&View";
			// 
			// TargetsMenuCommand
			// 
			this.TargetsMenuCommand.Description = "MenuCommand";
			this.TargetsMenuCommand.Text = "&Targets";
			// 
			// PropertiesMenuCommand
			// 
			this.PropertiesMenuCommand.Description = "MenuCommand";
			this.PropertiesMenuCommand.ImageIndex = 0;
			this.PropertiesMenuCommand.Text = "&Properties";
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
			// 
			// HelpMenuCommand
			// 
			this.HelpMenuCommand.Description = "MenuCommand";
			this.HelpMenuCommand.MenuCommands.AddRange(new MenuCommand[]
				{
					this.NAntHelpMenuCommand,
					this.NAntSDKMenuCommand,
					this.NAntContribMenuCommand,
					this.MenuCommand1,
					this.AboutMenuCommand
				});
			this.HelpMenuCommand.Text = "&Help";
			// 
			// NAntHelpMenuCommand
			// 
			this.NAntHelpMenuCommand.Description = "MenuCommand";
			this.NAntHelpMenuCommand.Text = "NAnt &Help";
			// 
			// NAntSDKMenuCommand
			// 
			this.NAntSDKMenuCommand.Description = "MenuCommand";
			this.NAntSDKMenuCommand.Text = "NAnt &SDK Help";
			// 
			// NAntContribMenuCommand
			// 
			this.NAntContribMenuCommand.Description = "MenuCommand";
			this.NAntContribMenuCommand.Text = "NAnt-&Contrib Help";
			// 
			// MenuCommand1
			// 
			this.MenuCommand1.Description = "MenuCommand";
			this.MenuCommand1.Text = "-";
			// 
			// AboutMenuCommand
			// 
			this.AboutMenuCommand.Description = "MenuCommand";
			this.AboutMenuCommand.Text = "&About NAnt-Gui";
		}

		#endregion

		private void AssignImages()
		{
			this.NewMenuCommand.ImageList			= _imageList;
			this.OpenMenuCommand.ImageList			= _imageList;
			this.SaveMenuCommand.ImageList			= _imageList;
			this.ReloadMenuCommand.ImageList		= _imageList;
			this.PropertiesMenuCommand.ImageList	= _imageList;
			this.BuildMenuCommand.ImageList			= _imageList;
		}

		public void Enable()
		{
			this.ReloadMenuCommand.Enabled	= true;
			this.SaveMenuCommand.Enabled	= true;
			this.SaveAsMenuCommand.Enabled	= true;
			this.BuildMenuCommand.Enabled	= true;
			this.CloseMenuCommand.Enabled	= true;
		}

		public void Disable()
		{
			this.ReloadMenuCommand.Enabled	= false;
			this.SaveMenuCommand.Enabled	= false;
			this.SaveAsMenuCommand.Enabled	= false;
			this.BuildMenuCommand.Enabled	= false;
			this.CloseMenuCommand.Enabled	= false;
		}

		public void ClearRecentItems()
		{
			this.RecentMenuCommand.MenuCommands.Clear();
		}

		public void AddRecentItem(MenuCommand item)
		{
			this.RecentMenuCommand.MenuCommands.Add(item);
		}

		#region Events

		public EventHandler New_Click
		{
			set { this.NewMenuCommand.Click += value; }
		}

		public EventHandler Open_Click
		{
			set { this.OpenMenuCommand.Click += value; }
		}

		public EventHandler Save_Click
		{
			set { this.SaveMenuCommand.Click += value; }
		}

		public EventHandler SaveAs_Click
		{
			set { this.SaveAsMenuCommand.Click += value; }
		}

		public EventHandler Reload_Click
		{
			set { this.ReloadMenuCommand.Click += value; }
		}

		public EventHandler Close_Click
		{
			set { this.CloseMenuCommand.Click += value; }
		}

		public EventHandler SaveOutput_Click
		{
			set { this.SaveOutputMenuCommand.Click += value; }
		}

		public EventHandler Exit_Click
		{
			set { this.ExitMenuCommand.Click += value; }
		}

		public EventHandler Undo_Click
		{
			set { this.UndoMenuCommand.Click += value; }
		}

		public EventHandler Redo_Click
		{
			set { this.RedoMenuCommand.Click += value; }
		}

		public EventHandler Copy_Click
		{
			set { this.CopyMenuCommand.Click += value; }
		}

		public EventHandler SelectAll_Click
		{
			set { this.SelectAllMenuCommand.Click += value; }
		}

		public EventHandler WordWrap_Click
		{
			set { this.WordWrapMenuCommand.Click += value; }
		}

		public EventHandler Targets_Click
		{
			set { this.TargetsMenuCommand.Click += value; }
		}

		public EventHandler Properties_Click
		{
			set { this.PropertiesMenuCommand.Click += value; }
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
			set { this.NAntHelpMenuCommand.Click += value; }
		}

		public EventHandler NAntSDK_Click
		{
			set { this.NAntSDKMenuCommand.Click += value; }
		}

		public EventHandler NAntContrib_Click
		{
			set { this.NAntContribMenuCommand.Click += value; }
		}

		public EventHandler About_Click
		{
			set { this.AboutMenuCommand.Click += value; }
		}

		#endregion

		public bool WordWrapChecked
		{
			get { return this.WordWrapMenuCommand.Checked; }
			set { this.WordWrapMenuCommand.Checked = value; }
		}
	}
}