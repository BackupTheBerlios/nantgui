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

using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for RegUtil.
	/// </summary>
	public class RegUtil
	{
		public const string KEY_PATH = @"Software\NAnt-Gui";

		private RegistryKey _currentUser = Registry.CurrentUser;

		private bool GetRegKeyBoolValue(string keyName)
		{
			RegistryKey key = _currentUser.CreateSubKey(KEY_PATH);
			return Convert.ToBoolean(key.GetValue(keyName, false));
		}

		private string GetRegKeyStringValue(string keyName, string defaultValue)
		{
			RegistryKey key = _currentUser.CreateSubKey(KEY_PATH);
			return key.GetValue(keyName, defaultValue).ToString();
		}

		private int GetRegKeyIntValue(string keyName, int defaultValue)
		{
			RegistryKey key = _currentUser.CreateSubKey(KEY_PATH);
			return Convert.ToInt32(key.GetValue(keyName, defaultValue));
		}

		private PropertySort GetRegKeyPropertySortValue(string keyName, PropertySort defaultValue)
		{
			string regKeyStringValue = GetRegKeyStringValue(keyName, defaultValue.ToString());
			return (PropertySort) Enum.Parse(typeof (PropertySort), regKeyStringValue);
		}

		private FormWindowState GetRegKeyWindowStateValue(string keyName, FormWindowState defaultValue)
		{
			string regKeyStringValue = GetRegKeyStringValue(keyName, defaultValue.ToString());
			return (FormWindowState) Enum.Parse(typeof (FormWindowState), regKeyStringValue);
		}

		private void SetRegKeyValue(string keyName, object value)
		{
			RegistryKey key = Registry.CurrentUser.CreateSubKey(KEY_PATH);
			key.SetValue(keyName, value);
		}
	}
}
