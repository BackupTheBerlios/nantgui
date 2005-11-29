#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2005 Colin Svingen
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
	/// Summary description for NantBuildFile.
	/// </summary>
	public class NantBuildFile : BuildFile
	{
		public NantBuildFile()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// This may be an issue.  ".build" is a bad
		/// extension for NAnt.  ".nant" would be better.
		/// Alternatively, the option should be given.
		/// </summary>
		protected override string Extension
		{
			get { return "build"; }
		}
	}
}
