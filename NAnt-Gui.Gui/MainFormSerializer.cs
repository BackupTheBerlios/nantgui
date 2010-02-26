#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2007 Colin Svingen
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General internal License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General internal License for more details.
//
// You should have received a copy of the GNU General internal License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Colin Svingen (swoogan@gmail.com)

#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using NAntGui.Gui.Controls;
using NAntGui.Gui.Properties;

namespace NAntGui.Gui
{
	class MainFormSerializer
	{
		private const bool ALLOW_SAVE_MINIMIZED = false;

        private NAntGuiForm _mainForm;
        private PropertyWindow _propertyWindow;
        private ToolStrip _standardToolStrip;
        private ToolStrip _buildToolStrip;

        internal static void Attach(NAntGuiForm form, PropertyWindow propertyWindow, ToolStrip standardToolStrip, ToolStrip buildToolStrip)
		{
            new MainFormSerializer(form, propertyWindow, standardToolStrip, buildToolStrip);
		}

        private MainFormSerializer(NAntGuiForm form, PropertyWindow propertyWindow, ToolStrip standardToolStrip, ToolStrip buildToolStrip)
		{
			_mainForm = form;
			_mainForm.Load += new EventHandler(OnLoad);
            _mainForm.Closing += new CancelEventHandler(OnClosing);
			_propertyWindow = propertyWindow;
            _standardToolStrip = standardToolStrip;
            _buildToolStrip = buildToolStrip;
		}

		private void OnLoad(object sender, EventArgs e)
		{
            _mainForm.Location = Settings.Default.MainFormLocation;
            _mainForm.WindowState = Settings.Default.MainFormState;
            _mainForm.Size = Settings.Default.MainFormSize;
            _propertyWindow.PropertyGrid.PropertySort = Settings.Default.PropertySort;
            _standardToolStrip.Location = Settings.Default.StandardToolStripLocation;
            _buildToolStrip.Location = Settings.Default.BuildToolStripLocation;
		}

		/// <summary>
		/// save position, size and state
		/// </summary>
		private void OnClosing(object sender, CancelEventArgs e)
		{
			if (_mainForm.WindowState != FormWindowState.Maximized)
			{
                Settings.Default.MainFormLocation = _mainForm.Location;
                Settings.Default.MainFormSize = _mainForm.Size;
			}

			Settings.Default.MainFormState = AdjustWindowState();
			Settings.Default.PropertySort = _propertyWindow.PropertyGrid.PropertySort;
            Settings.Default.StandardToolStripLocation = _standardToolStrip.Location;
            Settings.Default.BuildToolStripLocation = _buildToolStrip.Location;
            Settings.Default.Save();
		}

        private FormWindowState AdjustWindowState()
		{
            return SaveCurrentState() ? FormWindowState.Normal : _mainForm.WindowState;
		}

        private bool SaveCurrentState()
        {
            return IsMinimized() && !ALLOW_SAVE_MINIMIZED;
        }

        private bool IsMinimized()
        {
            return _mainForm.WindowState == FormWindowState.Minimized;
        }
    }
}