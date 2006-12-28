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

using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Common;

namespace NAntGui.Gui
{
	/// <summary>
	/// Summary description for Settings.
	/// </summary>
	public class Settings
	{
		private const int DEFAULT_MAX_ITEMS = 6;
		private const string IMAGE_PATH = "NAntGui.Gui.Images.MenuItems.bmp";
		public const string OLD_KEY_PATH = @"Software\Business Watch\NAnt-Gui";
		public const string RECENT_ITEMS_KEY_PATH = @"Software\NAnt-Gui\Recent Items";
		public const string WINDOW_KEY_PATH = @"Software\NAnt-Gui\Window";
		

		private static Settings _settings;

		private ImageList _imageList;		

		#region Window

		public class Window
		{
			public static int Left
			{
				get { return GetRegKeyIntValue("Left", 100); }
				set { SetRegKeyValue("Left", value); }
			}

			public static int Top
			{
				get { return GetRegKeyIntValue("Top", 100); }
				set { SetRegKeyValue("Top", value); }
			}

			public static int Width
			{
				get { return GetRegKeyIntValue("Width", 800); }
				set { SetRegKeyValue("Width", value); }
			}

			public static int Height
			{
				get { return GetRegKeyIntValue("Height", 600); }
				set { SetRegKeyValue("Height", value); }
			}

			public static PropertySort PropertySort
			{
				get { return GetRegKeyPropertySortValue("PropertySort", PropertySort.Categorized); }
				set { SetRegKeyValue("PropertySort", value); }
			}

			public static FormWindowState WindowState
			{
				get { return GetRegKeyWindowStateValue("WindowState", FormWindowState.Normal); }
				set { SetRegKeyValue("WindowState", value); }
			}
		}

		#endregion

		public Settings Instance()
		{
			if (_settings == null)
				_settings = new Settings();

			return _settings;
		}

		private Settings()
		{
			_imageList = ResourceHelper.LoadBitmapStrip(typeof (Settings),
				IMAGE_PATH, new Size(16, 16), new Point(0, 0));
		}

		public int MaxRecentItems
		{
			get { return GetRegKeyIntValue("MaxRecentItems", DEFAULT_MAX_ITEMS); }
			set { SetRegKeyValue("MaxRecentItems", value); }
		}

		public bool HideTargetsWithoutDescription
		{
			get { return GetRegKeyBoolValue("HideTargets"); }
			set { SetRegKeyValue("HideTargets", value); }
		}

		public string OpenInitialDir
		{
			get { return GetRegKeyStringValue("OpenInitialDirectory", "C:\\"); }
			set { SetRegKeyValue("OpenInitialDirectory", value); }
		}

		public string SaveOutputInitialDir
		{
			get { return GetRegKeyStringValue("SaveOutputInitialDir", "C:\\"); }
			set { SetRegKeyValue("SaveOutputInitialDir", value); }
		}

		public static ImageList ImageList
		{
			get { return _imageList; }
		}
	}
}