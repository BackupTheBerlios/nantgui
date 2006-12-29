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

using NAntGui.Framework;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for RecentItemsStore.
	/// </summary>
	public class RecentItemsStore
	{
		private const string LOOP_ESCAPE = "DONE";
		private Settings _settings = Settings.Instance();
		private RecentItems _items;

		public RecentItemsStore(RecentItems items)
		{
			Assert.NotNull(items, "items");
			_items = items;
		}

		public void Load()
		{
			for (int count = 0; count < _settings.MaxRecentItems; count++)
			{
				string name = _settings.LoadRecentItem("Recent" + count, LOOP_ESCAPE);

				if (name == LOOP_ESCAPE) break;

				_items.Append(name);
			}
		}

		public void Save()
		{
			int count = 0;

			foreach (string item in _items)
			{
				_settings.SaveRecentItem("Recent" + count++, item);
			}

			for (int i = count; i < _settings.MaxRecentItems; i++)
			{
				_settings.DeleteRecentItem("Recent" + i);
			}
		}
	}
}
