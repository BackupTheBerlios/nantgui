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
// Colin Svingen (csvingen@businesswatch.ca)
#endregion

using System.Xml;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Target.
	/// </summary>
	public class Target
	{
		private string _name = "";
		private string _description = "";
		private string[] _depends;
		private bool _default = false;

		public Target(XmlElement element)
		{
			this.ParseAttributes(element);
		}

		private void ParseAttributes(XmlElement element)
		{
			this._name			= element.GetAttribute("name");
			this._description	= element.GetAttribute("description");
			string depends		= element.GetAttribute("depends");
			this._depends		= this.SplitDepends(depends);
		}

		private string[] SplitDepends(string depends)
		{
			return depends.Replace(" ", "").Split(',');
		}

		#region Properties

		public string Name
		{
			get { return this._name; }
		}

		public string Description
		{
			get { return this._description; }
		}

		public string[] Depends
		{
			get { return this._depends; }
		}

		public bool Default
		{
			get { return this._default; }
			set { this._default = value; }
		}

		#endregion
	}
}