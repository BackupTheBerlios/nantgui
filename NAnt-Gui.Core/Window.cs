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

using NAntGui.Framework;
using System.Windows.Forms;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Window.
	/// </summary>
	public class Window
	{
		private RegUtil _regUtil;

        public Window(RegUtil regUtil)
		{
			Assert.NotNull(regUtil, "regUtil");
			_regUtil = regUtil;
		}

		public int Left
		{
			get { return _regUtil.GetInt("Left", 100); }
			set { _regUtil.SetValue("Left", value); }
		}

		public int Top
		{
			get { return _regUtil.GetInt("Top", 100); }
			set { _regUtil.SetValue("Top", value); }
		}

		public int Width
		{
			get { return _regUtil.GetInt("Width", 800); }
			set { _regUtil.SetValue("Width", value); }
		}

		public int Height
		{
			get { return _regUtil.GetInt("Height", 600); }
			set { _regUtil.SetValue("Height", value); }
		}

		public PropertySort PropertySort
		{
			get { return _regUtil.GetPropertySort("PropertySort", PropertySort.Categorized); }
			set { _regUtil.SetValue("PropertySort", value); }
		}

		public FormWindowState WindowState
		{
			get { return _regUtil.GetWindowState("WindowState", FormWindowState.Normal); }
			set { _regUtil.SetValue("WindowState", value); }
		}
	}
}
