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
using System.Collections;
using System.Text;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for ColorTable.
	/// </summary>
	public class ColorTable
	{
		private const string COLOR_TAG = @"\cf";
		private const string RED = @"\red255\green0\blue0;";
		private const string GREEN = @"\red0\green255\blue0;";
		private const string BLUE = @"\red0\green0\blue255;";
		private const string HEADER = @"{\colortbl ;";
		private const string FOOTER = "}";

		private static StringBuilder ColorList = new StringBuilder();
		private static ArrayList UsedColors = new ArrayList(3);

		private enum Colors
		{
			Red,
			Green,
			Blue
		}

		public static string RedTag
		{
			get { return GetColorTag(Colors.Red); }
		}

		public static string BlueTag
		{
			get { return GetColorTag(Colors.Blue); }
		}

		public static string GreenTag
		{
			get { return GetColorTag(Colors.Green); }
		}

		private static string GetColorTag(Colors pColor)
		{
			if (!UsedColors.Contains(pColor))
			{
				UsedColors.Add(pColor);
				ColorList.Append(GetColor(pColor));
			}
			return COLOR_TAG + (UsedColors.IndexOf(pColor) + 1);
		}

		public static string ColorTableRtf
		{
			get { return HEADER + ColorList + FOOTER; }
		}

		public static void Reset()
		{
			ColorList = new StringBuilder();
			UsedColors.Clear();
		}

		private static string GetColor(Colors pColor)
		{
			switch (pColor)
			{
				case Colors.Red:
					return RED;
				case Colors.Green:
					return GREEN;
				case Colors.Blue:
					return BLUE;
				default:
					throw new ArgumentException("Invalid color: " + pColor, "pColor");
			}
		}
	}
}