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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using NAntGui.Gui.Controls;
using NAntGui.Core;

namespace NAntGui.Gui
{
	public class MainFormSerializer
	{
		private const bool ALLOW_SAVE_MINIMIZED = false;

		private MainForm _mainForm;
		private MainPropertyGrid _propertyGrid;
		private Settings _settings = Settings.Instance();

		public static void Attach(MainForm form, MainPropertyGrid propertyGrid)
		{
			new MainFormSerializer(form, propertyGrid);
		}

		private MainFormSerializer(MainForm form, MainPropertyGrid propertyGrid)
		{
			_mainForm = form;
			_mainForm.Load += new EventHandler(OnLoad);
			_propertyGrid = propertyGrid;
		}

		private void OnLoad(object sender, EventArgs e)
		{
			int left = Convert.ToInt32(_settings.LoadWindowValue("Left", _mainForm.Left));
			int top = Convert.ToInt32(_settings.LoadWindowValue("Top", _mainForm.Top));
			int width = Convert.ToInt32(_settings.LoadWindowValue("Width", _mainForm.Width));
			int height = Convert.ToInt32(_settings.LoadWindowValue("Height", _mainForm.Height));

			FormWindowState windowState = (FormWindowState) 
				_settings.LoadWindowValue("WindowState", (int) _mainForm.WindowState);
			PropertySort propertySort = (PropertySort) 
				_settings.LoadWindowValue("PropertySort", (int) _propertyGrid.PropertySort);

			_mainForm.Location = new Point(left, top);
			_mainForm.Size = new Size(width, height);
			_mainForm.WindowState = windowState;

			try
			{
				_propertyGrid.PropertySort = propertySort;
			}
			catch (ArgumentException)
			{
				/* ignore */
			}

			_mainForm.Closing += new CancelEventHandler(OnClosing);
		}

		/// <summary>
		/// save position, size and state
		/// </summary>
		private void OnClosing(object sender, CancelEventArgs e)
		{
			if (_mainForm.WindowState != FormWindowState.Maximized)
			{
				_settings.SaveWindowValue("Left", _mainForm.Left);
				_settings.SaveWindowValue("Top", _mainForm.Top);
				_settings.SaveWindowValue("Width", _mainForm.Width);
				_settings.SaveWindowValue("Height", _mainForm.Height);
			}

			_settings.SaveWindowValue("WindowState", AdjustWindowState());
			_settings.SaveWindowValue("PropertySort", (int) _propertyGrid.PropertySort);
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