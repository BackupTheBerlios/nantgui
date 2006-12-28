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

using NAntGui.Core;
using System.Windows.Forms;

namespace NAntGui.Gui
{
	/// <summary>
	/// Summary description for BuildFileBrowser.
	/// </summary>
	public class BuildFileBrowser
	{
		private static OpenFileDialog _openDialog = new OpenFileDialog();
		private static SaveFileDialog _saveDialog = new SaveFileDialog();

		static BuildFileBrowser()
		{
			_openDialog.DefaultExt =
				_saveDialog.DefaultExt =
				"build";

			_openDialog.Filter =
				_saveDialog.Filter =
				"Build Files (*.build; *.nant)|*.build;*.nant|NAnt Includes (*.inc)|*.inc|All Files (*.*)|*.*";
		}

		public static string[] BrowseForLoad()
		{
			Settings settings = Settings.Instance();
			_openDialog.InitialDirectory = settings.OpenInitialDir;

			if (_openDialog.ShowDialog() == DialogResult.OK)
			{
				return _openDialog.FileNames;
			}
			else
			{
				return new string[] {};
			}
		}

		public static string BrowseForSave()
		{
			DialogResult result = _saveDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				return _saveDialog.FileName;
			}
			else
			{
				return null;
			}
		}
	}
}