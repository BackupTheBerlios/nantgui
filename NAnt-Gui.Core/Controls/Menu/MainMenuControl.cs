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
		private FileMenu.FileMenu _fileMenu;
		private EditMenu.EditMenu _editMenu;
		private ViewMenu.ViewMenu _viewMenu;
		private BuildMenu.BuildMenu _buildMenu;
		private ToolsMenu.ToolsMenu _toolsMenu;
		private HelpMenu.HelpMenu _helpMenu;

		public MainMenuControl(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "mediator");
			_fileMenu = new FileMenu.FileMenu(mediator);
			_editMenu = new EditMenu.EditMenu(mediator);
			_viewMenu = new ViewMenu.ViewMenu(mediator);
			_buildMenu = new BuildMenu.BuildMenu(mediator);
			_toolsMenu = new ToolsMenu.ToolsMenu(mediator);
			_helpMenu = new HelpMenu.HelpMenu(mediator);

			this.Initialize();
		}

		#region Initialize

		private void Initialize()
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

		#endregion

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

		public RunState RunState
		{
			set { _buildMenu.State = value; }
		}

		public bool HasRecentItems
		{
			get { return _fileMenu.HasRecentItems; }
		}

		public string FirstRecentItem
		{
			get { return _fileMenu.FirstRecentItem; }
		}

		public EditState EditState
		{
			set { _editMenu.State = value; }
		}
	}
}