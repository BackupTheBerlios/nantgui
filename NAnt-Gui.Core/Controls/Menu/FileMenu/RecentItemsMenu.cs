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
	/// Summary description for RecentItemsMenuCommand.
	/// </summary>
	public class RecentItemsMenu : MenuCommand
	{
		private RecentItems _recentItems = new RecentItems();
		private MainFormMediator _mediator;

		public RecentItemsMenu(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "mediator");
			_mediator = mediator;
			Description = "MenuCommand";
			Text = "Recent &Items";
			CreateMenuCommands();
		}

		public void AddRecentItem(string file)
		{
			_recentItems.Add(file);
			_recentItems.Save();

			CreateMenuCommands();
		}

		public void RemoveRecentItem(string file)
		{
			_recentItems.Remove(file);
			_recentItems.Save();

			CreateMenuCommands();
		}

		private void CreateMenuCommands()
		{
			MenuCommands.Clear();

			int count = 1;
			foreach (string item in _recentItems)
			{
				string name = count++ + " " + item;
				RecentItemMenuCommand recentItem = new RecentItemMenuCommand(name, _mediator);
				MenuCommands.Add(recentItem);
			}
		}

		public bool HasRecentItems
		{
			get { return _recentItems.HasRecentItems; }
		}

		public string FirstRecentItem
		{
			get { return _recentItems[0]; }
		}
	}
}