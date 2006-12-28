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

using System.Windows.Forms;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Settings.
	/// </summary>
	public class Settings
	{
		private const int DEFAULT_MAX_ITEMS = 6;	
		
		private static Settings _settings;

		private RegUtil _regUtil = new RegUtil();

		#region Window

		public class Window
		{
			private RegUtil _regUtil = new RegUtil();

			public int Left
			{
				get { return _regUtil.GetRegKeyIntValue("Left", 100); }
				set { _regUtil.SetRegKeyValue("Left", value); }
			}

			public int Top
			{
				get { return _regUtil.GetRegKeyIntValue("Top", 100); }
				set { _regUtil.SetRegKeyValue("Top", value); }
			}

			public int Width
			{
				get { return _regUtil.GetRegKeyIntValue("Width", 800); }
				set { _regUtil.SetRegKeyValue("Width", value); }
			}

			public int Height
			{
				get { return _regUtil.GetRegKeyIntValue("Height", 600); }
				set { _regUtil.SetRegKeyValue("Height", value); }
			}

			public PropertySort PropertySort
			{
				get { return _regUtil.GetRegKeyPropertySortValue("PropertySort", PropertySort.Categorized); }
				set { _regUtil.SetRegKeyValue("PropertySort", value); }
			}

			public FormWindowState WindowState
			{
				get { return _regUtil.GetRegKeyWindowStateValue("WindowState", FormWindowState.Normal); }
				set { _regUtil.SetRegKeyValue("WindowState", value); }
			}
		}

		#endregion

		public static Settings Instance()
		{
			if (_settings == null)
				_settings = new Settings();

			return _settings;
		}

		private Settings() {}

		public int MaxRecentItems
		{
			get { return _regUtil.GetRegKeyIntValue("MaxRecentItems", DEFAULT_MAX_ITEMS); }
			set { _regUtil.SetRegKeyValue("MaxRecentItems", value); }
		}

		public bool HideTargetsWithoutDescription
		{
			get { return _regUtil.GetRegKeyBoolValue("HideTargets"); }
			set { _regUtil.SetRegKeyValue("HideTargets", value); }
		}

		public string OpenScriptDir
		{
			get { return _regUtil.GetRegKeyStringValue("OpenInitialDirectory", "C:\\"); }
			set { _regUtil.SetRegKeyValue("OpenInitialDirectory", value); }
		}

		public string SaveScriptInitialDir
		{
			get { return _regUtil.GetRegKeyStringValue("SaveScriptInitialDir", "C:\\"); }
			set { _regUtil.SetRegKeyValue("SaveScriptInitialDir", value); }
		}

		public string SaveOutputInitialDir
		{
			get { return _regUtil.GetRegKeyStringValue("SaveOutputInitialDir", "C:\\"); }
			set { _regUtil.SetRegKeyValue("SaveOutputInitialDir", value); }
		}
	}
}