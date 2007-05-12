#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2007 Colin Svingen
//
// NAnt-Gui is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// NAnt-Gui is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//
// Colin Svingen (nantgui@swoogan.com)

#endregion

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for OldRegistry.
	/// </summary>
	public class OldRegistry : RegUtil
	{
		private const string OLD_KEY_PATH = @"Software\Business Watch\NAnt-Gui";

		public OldRegistry() : base(KEY_PATH, OLD_KEY_PATH, "", "")
		{
		}
	}
}
