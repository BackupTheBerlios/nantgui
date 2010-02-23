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
using NAntGui.Framework;

namespace NAntGui.Gui
{
	/// <summary>
	/// Description of ProjectInfo.
	/// </summary>
	class ProjectInfo
	{
		private string _name;
		
		internal ProjectInfo(string name)
		{
			Assert.NotNull(name, "name");
			_name = name;
		}
		
		internal string Name
		{
			get { return _name; }
		}
	}
}
