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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NAntGui.Core
{
	public class MainFormSerializer
	{
		private const bool ALLOW_SAVE_MINIMIZED = false;
		private readonly RegistryKey _currentUser = Registry.CurrentUser;

		private MainForm _mainForm;

		public static void Attach(MainForm form)
		{
			new MainFormSerializer(form);
		}

		private MainFormSerializer(MainForm form)
		{
			_mainForm = form;
			_mainForm.Load += new EventHandler(this.OnLoad);
		}

		private void OnLoad(object sender, EventArgs e)
		{
			RegistryKey key = _currentUser.CreateSubKey(Settings.WINDOW_KEY_PATH);

			if (KeyExists(key))
			{
				this.LoadStateFromReg(key);
			}
			else
			{
				this.LoadStateFromOldRegKey();
			}
		}

		private void LoadStateFromReg(RegistryKey key)
		{
			int left	= Convert.ToInt32(key.GetValue("Left", _mainForm.Left));
			int top		= Convert.ToInt32(key.GetValue("Top", _mainForm.Top));
			int width	= Convert.ToInt32(key.GetValue("Width", _mainForm.Width));
			int height	= Convert.ToInt32(key.GetValue("Height", _mainForm.Height));

			FormWindowState windowState = (FormWindowState) key.GetValue("WindowState", (int) _mainForm.WindowState);
			PropertySort propertySort = (PropertySort) key.GetValue("PropertySort", (int) _mainForm._propertyGrid.PropertySort);

			_mainForm.Location = new Point(left, top);
			_mainForm.Size = new Size(width, height);
			_mainForm.WindowState = windowState;
			_mainForm.PropertySort = propertySort;

			_mainForm.Closing += new CancelEventHandler(this.OnClosing);
		}

		private void LoadStateFromOldRegKey()
		{
			RegistryKey key = _currentUser.OpenSubKey(Settings.OLD_KEY_PATH);

			if (KeyExists(key))
			{
				this.LoadStateFromReg(key);
			}
		}

		private static bool KeyExists(RegistryKey key)
		{
			return key != null;
		}

		/// <summary>
		/// save position, size and state
		/// </summary>
		private void OnClosing(object sender, CancelEventArgs e)
		{
			RegistryKey lKey = _currentUser.CreateSubKey(Settings.WINDOW_KEY_PATH);

			if (_mainForm.WindowState != FormWindowState.Maximized)
			{
				lKey.SetValue("Left", _mainForm.Left);
				lKey.SetValue("Top", _mainForm.Top);
				lKey.SetValue("Width", _mainForm.Width);
				lKey.SetValue("Height", _mainForm.Height);
			}

			lKey.SetValue("WindowState", this.AdjustWindowState());
			lKey.SetValue("PropertySort", (int) _mainForm.PropertySort);
		}

		private int AdjustWindowState()
		{
			// check if we are allowed to save the state as minimized (not normally)
			if (_mainForm.WindowState == FormWindowState.Minimized && !ALLOW_SAVE_MINIMIZED)
			{
				return (int) FormWindowState.Normal;
			}
			else
			{
				return (int) _mainForm.WindowState;
			}
		}
	}
}