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
// Colin Svingen (swoogan@gmail.com)

#endregion

using System;
using System.Xml;
using NAntGui.Framework;

namespace NAntGui.NAnt
{
	/// <summary>
	/// Summary description for Target.
	/// </summary>    
	public class NAntTarget : BuildTarget, IComparable
	{
        // TODO: Convert from XmlDocument to XmlReader.  Create to get line numbers for later use.
		private string[] _depends;

		public NAntTarget(XmlElement element)
		{
			ParseAttributes(element);
		}

		private void ParseAttributes(XmlElement element)
		{
			_name = element.GetAttribute("name");
			_description = element.GetAttribute("description");
			string depends = element.GetAttribute("depends");
			_depends = SplitDepends(depends);
		}

		private string[] SplitDepends(string depends)
		{
			return depends.Replace(" ", "").Split(',');
		}

		public int CompareTo(object obj)
		{
			if (obj.GetType() == typeof (NAntTarget))
			{
				return _name.CompareTo(((NAntTarget) obj).Name);
			}
			else
			{
				throw new ArgumentException("Wasn't a target object.", "obj");
			}
		}

		#region Properties

		public string[] Depends
		{
			get { return _depends; }
		}

		#endregion
	}
}