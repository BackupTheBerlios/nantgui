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
using System.Windows.Forms;
using Microsoft.Win32;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Settings.
	/// </summary>
	public class Settings
	{
		private static RegistryKey _currentUser = Registry.CurrentUser;
		private const int DEFAULT_MAX_ITEMS = 6;
		public const string OLD_KEY_PATH = @"Software\Business Watch\NAnt-Gui";
		public const string RECENT_ITEMS_KEY_PATH = @"Software\NAnt-Gui\Recent Items";
		public const string WINDOW_KEY_PATH = @"Software\NAnt-Gui\Window";
		public const string KEY_PATH = @"Software\NAnt-Gui";

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

		public static int MaxRecentItems
		{
			get { return GetRegKeyIntValue("MaxRecentItems", DEFAULT_MAX_ITEMS); }
			set { SetRegKeyValue("MaxRecentItems", value); }
		}

		public static bool HideTargetsWithoutDescription
		{
			get { return GetRegKeyBoolValue("HideTargets"); }
			set { SetRegKeyValue("HideTargets", value); }
		}

		public static string OpenInitialDir
		{
			get { return GetRegKeyStringValue("OpenInitialDirectory", "C:\\"); }
			set { SetRegKeyValue("OpenInitialDirectory", value); }
		}

		public static string SaveOutputInitialDir
		{
			get { return GetRegKeyStringValue("SaveOutputInitialDir", "C:\\"); }
			set { SetRegKeyValue("SaveOutputInitialDir", value); }
		}

		private static bool GetRegKeyBoolValue(string keyName)
		{
			RegistryKey key = _currentUser.CreateSubKey(KEY_PATH);
			return Convert.ToBoolean(key.GetValue(keyName, false));
		}

		private static string GetRegKeyStringValue(string keyName, string defaultValue)
		{
			RegistryKey key = _currentUser.CreateSubKey(KEY_PATH);
			return key.GetValue(keyName, defaultValue).ToString();
		}

		private static int GetRegKeyIntValue(string keyName, int defaultValue)
		{
			RegistryKey key = _currentUser.CreateSubKey(KEY_PATH);
			return Convert.ToInt32(key.GetValue(keyName, defaultValue));
		}

		private static PropertySort GetRegKeyPropertySortValue(string keyName, PropertySort defaultValue)
		{
			string regKeyStringValue = GetRegKeyStringValue(keyName, defaultValue.ToString());
			return (PropertySort) Enum.Parse(typeof (PropertySort), regKeyStringValue);
		}

		private static FormWindowState GetRegKeyWindowStateValue(string keyName, FormWindowState defaultValue)
		{
			string regKeyStringValue = GetRegKeyStringValue(keyName, defaultValue.ToString());
			return (FormWindowState) Enum.Parse(typeof (FormWindowState), regKeyStringValue);
		}

		private static void SetRegKeyValue(string keyName, object value)
		{
			RegistryKey key = Registry.CurrentUser.CreateSubKey(KEY_PATH);
			key.SetValue(keyName, value);
		}
	}
}