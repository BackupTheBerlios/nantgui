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

using Crownwood.Magic.Menus;

namespace NAntGui.Core.Controls.Menu.FileMenu
{
	/// <summary>
	/// Summary description for FileMenu.
	/// </summary>
	public class FileMenu : MenuCommand
	{
		private NewMenuCommand _new;
		private OpenMenuCommand _open;
		private SaveMenuCommand _save;
		private SaveAsMenuCommand _saveAs;
		private ReloadMenuCommand _reload;
		private CloseMenuCommand _close;
		private SaveOutputMenuCommand _saveOutput;
		private RecentItemsMenu _recent;
		private ExitMenuCommand _exit;

		public FileMenu(MainFormMediator mediator)
		{
			_new = new NewMenuCommand(mediator);
			_open = new OpenMenuCommand(mediator);
			_save = new SaveMenuCommand(mediator);
			_saveAs = new SaveAsMenuCommand(mediator);
			_reload = new ReloadMenuCommand(mediator);
			_close = new CloseMenuCommand(mediator);
			_saveOutput = new SaveOutputMenuCommand(mediator);
			_recent = new RecentItemsMenu(mediator);
			_exit = new ExitMenuCommand(mediator);


			Description = "MenuCommand";
			MenuCommands.AddRange(new MenuCommand[]
			                      	{
			                      		_new, _open, _save, _saveAs, _reload, _close,
			                      		_saveOutput, new MenuCommand("-"), _recent,
			                      		new MenuCommand("-"), _exit
			                      	});
			Text = "&File";
		}

		public void Enable()
		{
			_reload.Enabled = true;
//			_save.Enabled = true;
//			_saveAs.Enabled = true;
//			_close.Enabled = true;
		}

		public void Disable()
		{
			// the following lines are commented because, currently, there will always be
			// a file open.  When the user clicks "close" a new file is created.
			_reload.Enabled = false;
//			_save.Enabled = false;
//			_saveAs.Enabled = false;
//			_close.Enabled = false;
		}


		public void AddRecentItem(string file)
		{
			_recent.AddRecentItem(file);
		}

		public void RemoveRecentItem(string file)
		{
			_recent.RemoveRecentItem(file);
		}

		public bool HasRecentItems
		{
			get { return _recent.HasRecentItems; }
		}

		public string FirstRecentItem
		{
			get { return _recent.FirstRecentItem; }
		}
	}
}