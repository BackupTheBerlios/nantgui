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

namespace NAntGui.Core.Controls.Menu
{
	/// <summary>
	/// Summary description for MainMenuCommand.
	/// </summary>
	public class MainMenuControl : MenuControl
	{
		private FileMenu.FileMenu _fileMenu = new FileMenu.FileMenu();
		private EditMenu.EditMenu _editMenu = new EditMenu.EditMenu();
		private ViewMenu.ViewMenu _viewMenu = new ViewMenu.ViewMenu();
		private BuildMenu.BuildMenu _buildMenu = new BuildMenu.BuildMenu();
		private ToolsMenu.ToolsMenu _toolsMenu = new ToolsMenu.ToolsMenu();
		private HelpMenu.HelpMenu _helpMenu = new HelpMenu.HelpMenu();

		public MainMenuControl()
		{
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
					_viewMenu,
					_buildMenu,
					_toolsMenu,
					_helpMenu
				});
			this.Name = "MainMenu";
			this.Size = new Size(824, 25);
			this.Style = VisualStyle.IDE;
			this.TabIndex = 13;
			this.TabStop = false;
		}

		public MainFormMediator Mediator
		{
			set
			{
				_fileMenu.Mediator = value;
				_editMenu.Mediator = value;
				_viewMenu.Mediator = value;
				_buildMenu.Mediator = value;
				_toolsMenu.Mediator = value;
				_helpMenu.Mediator = value;
			}
		}

		public void Enable()
		{
			_fileMenu.Enable();
			_buildMenu.Enable();
		}

		public void Disable()
		{
			_fileMenu.Disable();
			_buildMenu.Disable();
		}

		public void AddRecentItem(string file)
		{
			_fileMenu.AddRecentItem(file);
		}

		public void RemoveRecentItem(string file)
		{
			_fileMenu.RemoveRecentItem(file);
		}

		public void EnablePasteAndDelete()
		{
			_editMenu.EnablePasteAndDelete();
		}

		public void DisablePasteAndDelete()
		{
			_editMenu.DisablePasteAndDelete();
		}

		public RunState State
		{
			set { _buildMenu.State = value; }
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

		public EventHandler ReloadClick
		{
			set { _fileMenu.Reload_Click = value; }
		}

		public EventHandler CloseClick
		{
			set { _fileMenu.Close_Click = value; }
		}

		public EventHandler SaveOutputClick
		{
			set { _fileMenu.SaveOutput_Click = value; }
		}

		public EventHandler ExitClick
		{
			set { _fileMenu.Exit_Click = value; }
		}

		public EventHandler UndoClick
		{
			set { _editMenu.Undo_Click = value; }
		}

		public EventHandler RedoClick
		{
			set { _editMenu.Redo_Click = value; }
		}

		public EventHandler CopyClick
		{
			set { _editMenu.Copy_Click = value; }
		}

		public EventHandler SelectAllClick
		{
			set { _editMenu.SelectAll_Click = value; }
		}

		public EventHandler WordWrapClick
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

		public EventHandler RunClick
		{
			set { _buildMenu.RunClick = value; }
		}

		public EventHandler StopClick
		{
			set { _buildMenu.StopClick = value; }
		}

		public EventHandler OptionsClick
		{
			set { _toolsMenu.Options_Click = value; }
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

		public EventHandler AboutClick
		{
			set { _helpMenu.About_Click = value; }
		}

		public EventHandler Recent_Click
		{
			set { _fileMenu.Recent_Click = value; }
		}

		public bool HasRecentItems
		{
			get { return _fileMenu.HasRecentItems; }
		}

		public string FirstRecentItem
		{
			get { return _fileMenu.FirstRecentItem; }
		}

		#endregion

	}
}