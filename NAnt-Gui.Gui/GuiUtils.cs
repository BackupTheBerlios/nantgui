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
using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Common;

namespace NAntGui.Gui
{
	/// <summary>
	/// Summary description for GuiUtils.
	/// </summary>
	public class GuiUtils
	{
		private const string IMAGE_PATH = "NAntGui.Gui.Images.MenuItems.bmp";
		private ImageList _imageList;	

		private static GuiUtils _guiUtils;

		public static GuiUtils Instance()
		{
			if (_guiUtils == null)
				_guiUtils = new GuiUtils();

			return _guiUtils;
		}

		private GuiUtils()
		{
			_imageList = ResourceHelper.LoadBitmapStrip(this.GetType(),
				IMAGE_PATH, new Size(16, 16), new Point(0, 0));
		}

		public ImageList ImageList
		{
			get { return _imageList; }
		}
	}
}
