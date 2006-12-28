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
using NAntGui.Core;

namespace NAntGui.Gui
{
	/// <summary>
	/// Summary description for RecentItems.
	/// </summary>
	public class RecentItems : CollectionBase
	{
		private Settings _settings = Settings.Instance();

		public RecentItems() : base() {}

//		public string this[int index]
//		{
//			get { return List[index].ToString(); }
//		}

		public string FirstItem
		{
			get { return List[0].ToString(); }
		}

		public bool HasItems
		{
			get { return Count > 0; }
		}

		public void Add(string item)
		{
			if (List.Contains(item))
			{
				ReplaceItem(item);
			}
			else
			{
				AddItem(item);
			}
		}

		private void AddItem(string item)
		{
			if (TooManyItems)
			{
				RemoveLast();
			}

			List.Insert(0, item);
		}

		public void Remove(string item)
		{
			if (List.Contains(item))
			{
				List.Remove(item);
			}
		}

		private bool TooManyItems
		{
			get { return List.Count >= _settings.MaxRecentItems; }
		}

		private void RemoveLast()
		{
			int lastItem = List.Count - 1;

			if (lastItem >= 0)
			{
				List.RemoveAt(lastItem);
			}
		}

		private void ReplaceItem(string item)
		{
			List.Remove(item);
			List.Insert(0, item);
		}
	}
}