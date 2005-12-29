using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Common;

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


namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for AppEntry.
	/// </summary>
	public class NAntGuiApp
	{
		private const string IMAGE_PATH = "NAntGui.Core.Images.MenuItems.bmp";
		private static CommandLineOptions _options;
		private static ImageList _imageList;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static void Run(CommandLineOptions options)
		{
			_options = options;

			_imageList = ResourceHelper.LoadBitmapStrip(typeof(NAntGuiApp), 
				IMAGE_PATH, new Size(16, 16), new Point(0, 0));

			MainForm mainForm = new MainForm();
			MainFormSerializer.Attach(mainForm);
			Application.Run(mainForm);
		}

		public static CommandLineOptions Options
		{
			get { return _options; }
		}

		public static ImageList ImageList
		{
			get { return _imageList; }
		}
	}
}