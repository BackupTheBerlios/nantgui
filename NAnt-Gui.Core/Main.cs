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

using System.Windows.Forms;


namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for AppEntry.
	/// </summary>
	public class Main
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static void Run(CommandLineOptions cmdLineOptions)
		{
			MainForm mainForm = new MainForm(cmdLineOptions);
			MainFormSerializer.Attach(mainForm);

			Application.Run(mainForm);
		}
	}
}