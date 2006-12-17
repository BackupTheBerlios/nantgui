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

using System.Collections;
using Microsoft.Win32;

namespace NAntGui.Gui
{
	/// <summary>
	/// Summary description for RecentItems.
	/// </summary>
	public class RecentItems : IEnumerable
	{
		private const string LOOP_ESCAPE = "DONE";
		private ArrayList _items = new ArrayList(Settings.MaxRecentItems);

		public RecentItems()
		{
			RegistryKey key = GetRegKey(Settings.RECENT_ITEMS_KEY_PATH);

			if (KeyExists(key))
			{
				LoadRecentItemsFromReg(key);
			}
			else
			{
				LoadRecentItemsFromOldRegKey();
			}
		}

		public void Save()
		{
			RegistryKey key = Registry.CurrentUser.CreateSubKey(Settings.RECENT_ITEMS_KEY_PATH);

			int count = 0;
			foreach (string item in _items)
			{
				key.SetValue("Recent" + count++, item);
			}

			for (int i = count; i < Settings.MaxRecentItems; i++)
			{
				key.DeleteValue("Recent" + i, false);
			}
		}

		public string this[int index]
		{
			get { return _items[index].ToString(); }
		}

		public bool HasRecentItems
		{
			get { return _items.Count > 0; }
		}

		public IEnumerator GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		public void Add(string item)
		{
			if (Exists(item))
			{
				ReplaceItem(item);
			}
			else
			{
				AddItem(item);
			}
		}

		public void Remove(string item)
		{
			if (Exists(item))
			{
				_items.Remove(item);
			}
		}

		private static bool KeyExists(RegistryKey key)
		{
			return key != null;
		}

		private void LoadRecentItemsFromReg(RegistryKey key)
		{
			int itemsAddedCount = 0;
			for (int count = 0; count < Settings.MaxRecentItems; count++)
			{
				string name = GetItemName(key, count);

				if (name == LOOP_ESCAPE) break;

				InsertItem(name, itemsAddedCount++);
			}
		}

		private void InsertItem(string itemName, int count)
		{
			_items.Insert(count, itemName);
		}

		private static string GetItemName(RegistryKey key, int count)
		{
			return key.GetValue("Recent" + count, LOOP_ESCAPE).ToString();
		}

		private static RegistryKey GetRegKey(string oldKeyPath)
		{
			return Registry.CurrentUser.OpenSubKey(oldKeyPath);
		}

		private void AddItem(string item)
		{
			if (TooManyItems)
			{
				RemoveLast();
			}

			_items.Insert(0, item);
		}

		private bool TooManyItems
		{
			get { return _items.Count >= Settings.MaxRecentItems; }
		}

		private void RemoveLast()
		{
			int lastItem = _items.Count - 1;

			if (lastItem >= 0)
			{
				_items.RemoveAt(lastItem);
			}
		}

		private bool Exists(string item)
		{
			foreach (string recentItem in _items)
			{
				if (recentItem == item) return true;
			}

			return false;
		}

		private void ReplaceItem(string pItem)
		{
			int index = _items.IndexOf(pItem);
			object temp = _items[index];
			_items.RemoveAt(index);
			_items.Insert(0, temp);
		}

		private void LoadRecentItemsFromOldRegKey()
		{
			RegistryKey key = GetRegKey(Settings.OLD_KEY_PATH);

			if (KeyExists(key))
			{
				LoadRecentItemsFromReg(key);
			}
		}
	}
}