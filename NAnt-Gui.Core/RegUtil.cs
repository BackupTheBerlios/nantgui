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
	public abstract class RegUtil
	{
		protected const string KEY_PATH = @"Software\NAnt-Gui";
		protected static RegistryKey _currentUser = Registry.CurrentUser;

		protected string _saveKeyRoot;
		protected string _loadKeyRoot;
		protected string _windowSubKey;
		protected string _recentItemsSubKey;

		protected RegUtil(string saveKeyRoot, string loadKeyRoot, string windowSubKey, string recentItemsSubKey)
		{
			_saveKeyRoot = saveKeyRoot;
			_loadKeyRoot = loadKeyRoot;
			_windowSubKey = windowSubKey;
			_recentItemsSubKey = recentItemsSubKey;
		}

		public virtual string GetRecentItem(string keyName, string defaultValue)
		{
			return GetString(keyName, defaultValue, _recentItemsSubKey);
		}

		public virtual void SetRecentItem(string keyName, string value)
		{
			SetValue(keyName, value, _recentItemsSubKey);
		}

		public virtual object GetWindowValue(string keyName, object defaultValue)
		{
			RegistryKey key = _currentUser.CreateSubKey(GetLoadPath(_windowSubKey));
			return key.GetValue(keyName, defaultValue);
		}

		public virtual void SetWindowValue(string keyName, object value)
		{
			SetValue(keyName, value, _windowSubKey);
		}

		public virtual bool GetBool(string keyName)
		{
			return GetBool(keyName, "");
		}

		protected virtual bool GetBool(string keyName, string subDir)
		{
			RegistryKey key = _currentUser.CreateSubKey(GetLoadPath(subDir));
			return Convert.ToBoolean(key.GetValue(keyName, false));
		}

		public virtual string GetString(string keyName, string defaultValue)
		{
			return GetString(keyName, defaultValue, "");
		}

		protected virtual string GetString(string keyName, string defaultValue, string subDir)
		{
			RegistryKey key = _currentUser.CreateSubKey(GetLoadPath(subDir));
			return key.GetValue(keyName, defaultValue).ToString();
		}

		public virtual int GetInt(string keyName, int defaultValue)
		{
			return GetInt(keyName, defaultValue, "");
		}

		protected virtual int GetInt(string keyName, int defaultValue, string subDir)
		{
			RegistryKey key = _currentUser.CreateSubKey(GetLoadPath(subDir));
			return Convert.ToInt32(key.GetValue(keyName, defaultValue));
		}

		public virtual PropertySort GetPropertySort(string keyName, PropertySort defaultValue)
		{
			return GetPropertySort(keyName, defaultValue, "");
		}

		public virtual PropertySort GetPropertySort(string keyName, PropertySort defaultValue, string subDir)
		{
			string regKeyStringValue = GetString(keyName, defaultValue.ToString(), subDir);
			return (PropertySort) Enum.Parse(typeof (PropertySort), regKeyStringValue);
		}

		public virtual FormWindowState GetWindowState(string keyName, FormWindowState defaultValue)
		{
			return GetWindowState(keyName, defaultValue, "");
		}

		public virtual FormWindowState GetWindowState(string keyName, FormWindowState defaultValue, string subDir)
		{
			string regKeyStringValue = GetString(keyName, defaultValue.ToString(), subDir);
			return (FormWindowState) Enum.Parse(typeof (FormWindowState), regKeyStringValue);
		}	

		public virtual void SetValue(string keyName, object value)
		{
			SetValue(keyName, value, "");
		}

		public virtual void SetValue(string keyName, object value, string subDir)
		{
			RegistryKey key = _currentUser.CreateSubKey(GetSavePath(subDir));
			key.SetValue(keyName, value);
		}

		public void DeleteValue(string keyName)
		{
			DeleteValue(keyName, "");
		}

		public void DeleteValue(string keyName, string subDir)
		{
			RegistryKey key = _currentUser.CreateSubKey(GetSavePath(subDir));
			key.DeleteValue(keyName, false);
		}

		protected static bool KeyRootExists(string keyName)
		{
			RegistryKey key = _currentUser.OpenSubKey(keyName);
			return key != null;
		}

		#region Paths

		protected virtual string GetWindowLoadPath()
		{
			return GetPath(_loadKeyRoot, _windowSubKey);
		}

		protected virtual string GetRecentItemsLoadPath()
		{
			return GetPath(_loadKeyRoot, _recentItemsSubKey);
		}

		protected virtual string GetLoadPath(string subDirectory)
		{
			return GetPath(_loadKeyRoot, subDirectory);
		}

		protected virtual string GetSavePath(string subDirectory)
		{
			return GetPath(_saveKeyRoot, subDirectory);
		}

		protected virtual string GetPath(string keyRoot, string subDirectory)
		{
			if (subDirectory != "")
				return keyRoot + "\\" + subDirectory;
			else
				return keyRoot;
		}

		#endregion
	}
}

