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

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Settings.
	/// </summary>
	public class Settings
	{
		private const int DEFAULT_MAX_ITEMS = 6;	
		private RegUtil _regUtil;
		private static Settings _settings;

		public static Settings Instance()
		{
			if (_settings == null)
				_settings = new Settings();

			return _settings;
		}

		private Settings() 
		{
			if (NewRegistry.KeyRootExists())
				_regUtil = new NewRegistry();
			else
				_regUtil = new OldRegistry();
		}

		public int MaxRecentItems
		{
			get { return _regUtil.GetInt("MaxRecentItems", DEFAULT_MAX_ITEMS); }
			set { _regUtil.SetValue("MaxRecentItems", value); }
		}

		public bool HideTargetsWithoutDescription
		{
			get { return _regUtil.GetBool("HideTargets"); }
			set { _regUtil.SetValue("HideTargets", value); }
		}

		public string OpenScriptDir
		{
			get { return _regUtil.GetString("OpenInitialDirectory", "C:\\"); }
			set { _regUtil.SetValue("OpenInitialDirectory", value); }
		}

		public string SaveScriptInitialDir
		{
			get { return _regUtil.GetString("SaveScriptInitialDir", "C:\\"); }
			set { _regUtil.SetValue("SaveScriptInitialDir", value); }
		}

		public string SaveOutputInitialDir
		{
			get { return _regUtil.GetString("SaveOutputInitialDir", "C:\\"); }
			set { _regUtil.SetValue("SaveOutputInitialDir", value); }
		}

		public void SaveRecentItem(string name, string value)
		{
			_regUtil.SetRecentItem(name, value);
		}

		public string LoadRecentItem(string name, string defaultValue)
		{
			return _regUtil.GetRecentItem(name, defaultValue);
		}

		public void DeleteRecentItem(string name)
		{
			_regUtil.DeleteValue(name);
		}

		public void SaveWindowValue(string name, object value)
		{
			_regUtil.SetWindowValue(name, value);
		}

		public object LoadWindowValue(string name, object defaultValue)
		{
			return _regUtil.GetWindowValue(name, defaultValue);
		}
	}
}