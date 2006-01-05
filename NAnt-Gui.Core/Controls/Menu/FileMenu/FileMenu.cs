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
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Controls.Menu.FileMenu
{
	/// <summary>
	/// Summary description for FileMenu.
	/// </summary>
	public class FileMenu : MenuCommand
	{
		private NewMenuCommand _new = new NewMenuCommand();
		private OpenMenuCommand _open = new OpenMenuCommand();
		private SaveMenuCommand _save = new SaveMenuCommand();
		private SaveAsMenuCommand _saveAs = new SaveAsMenuCommand();
		private ReloadMenuCommand _reload = new ReloadMenuCommand();
		private CloseMenuCommand _close = new CloseMenuCommand();
		private SaveOutputMenuCommand _saveOutput = new SaveOutputMenuCommand();
		private RecentItemsMenu _recent = new RecentItemsMenu();
		private ExitMenuCommand _exit = new ExitMenuCommand();

		public FileMenu()
		{
			this.Description = "MenuCommand";
			this.MenuCommands.AddRange(new MenuCommand[]
				{
					_new, _open, _save, _saveAs, _reload, _close,
					_saveOutput, new MenuCommand("-"), _recent,
					new MenuCommand("-"), _exit
				});
			this.Text = "&File";
		}

		public MainFormMediator Mediator
		{
			set
			{
				_new.Mediator = value;
				_open.Mediator = value;
				_save.Mediator = value;
				_saveAs.Mediator = value;
				_reload.Mediator = value;
				_close.Mediator = value;
				_saveOutput.Mediator = value;
				_exit.Mediator = value;
				_recent.Mediator = value;
			}
		}

		public void Enable()
		{
			_reload.Enabled =
				_save.Enabled =
					_saveAs.Enabled =
						_close.Enabled = true;
		}

		public void Disable()
		{
			_reload.Enabled =
				_save.Enabled =
					_saveAs.Enabled =
						_close.Enabled = false;
		}


		public void AddRecentItem(string file)
		{
			_recent.AddRecentItem(file);
		}

		public void RemoveRecentItem(string file)
		{
			_recent.RemoveRecentItem(file);
		}

		#region Events

		public EventHandler New_Click
		{
			set { _new.Click += value; }
		}

		public EventHandler Open_Click
		{
			set { _open.Click += value; }
		}

		public EventHandler Save_Click
		{
			set { _save.Click += value; }
		}

		public EventHandler SaveAs_Click
		{
			set { _saveAs.Click += value; }
		}

		public EventHandler Reload_Click
		{
			set { _reload.Click += value; }
		}

		public EventHandler Close_Click
		{
			set { _close.Click += value; }
		}

		public EventHandler SaveOutput_Click
		{
			set { _saveOutput.Click += value; }
		}

		public EventHandler Exit_Click
		{
			set { _exit.Click += value; }
		}

		public EventHandler Recent_Click
		{
			set { _recent.Recent_Click = value; }
		}

		public bool HasRecentItems
		{
			get { return _recent.HasRecentItems; }
		}

		public string FirstRecentItem
		{
			get { return _recent.FirstRecentItem; }
		}

		#endregion

	}
}