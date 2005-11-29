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

//		public class Window
//		{
//			public static int Left
//			{
//				int	left	= Convert.ToInt32(key.GetValue("Left", _nantForm.Left));
//			}
//			int	top		= Convert.ToInt32(key.GetValue("Top", _nantForm.Top));
//			int	width	= Convert.ToInt32(key.GetValue("Width", _nantForm.Width));
//			int	height	= Convert.ToInt32(key.GetValue("Height", _nantForm.Height));
//
//			int	horzSplitter = (int)key.GetValue("HorzSplitter", _nantForm.BuildTreeView.Height);
//			int	vertSplitter = (int)key.GetValue("VertSplitter", _nantForm.LeftPanel.Width);
//
//			FormWindowState	windowState	= (FormWindowState)key.GetValue("WindowState", (int)_nantForm.WindowState);
//			PropertySort propertySort	= (PropertySort)key.GetValue("PropertySort", (int)_nantForm.ProjectPropertyGrid.PropertySort);
//		}

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

		private static bool GetRegKeyBoolValue(string keyName)
		{
			RegistryKey key = _currentUser.CreateSubKey(KEY_PATH);

//			if (KeyExists(key))
//			{
			return Convert.ToBoolean(key.GetValue(keyName, false));
//			}
		}

		private static int GetRegKeyIntValue(string keyName, int defaultValue)
		{
			RegistryKey key = _currentUser.CreateSubKey(KEY_PATH);
//
//			if (KeyExists(key))
//			{
			return Convert.ToInt32(key.GetValue(keyName, defaultValue));
//			}
		}

//		private static bool KeyExists(RegistryKey key)
//		{
//			return key != null;
//		}

		private static void SetRegKeyValue(string keyName, object value)
		{
			RegistryKey key = Registry.CurrentUser.CreateSubKey(KEY_PATH);
			key.SetValue(keyName, value);
		}
	}
}