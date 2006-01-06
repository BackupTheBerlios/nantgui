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

using System.Xml;
using NAntGui.Framework;

namespace NAntGui.NAnt
{
	/// <summary>
	/// Summary description for Property.
	/// </summary>
	public class NAntProperty : BuildProperty
	{
		private static int Count = 1;
		private int _rank;

		public NAntProperty(XmlElement element)
		{
			_category	= "Global";
			_isReadOnly = false;
			_name		= element.GetAttribute("name");
			_value		= element.GetAttribute("value");
			this.SetIsReadonly(element);
			this.SetCategory(element);
			_rank = Count++;
		}

		public NAntProperty(string name, string value, string category, bool isReadOnly)
		{
			_name		= name;
			_value		= value;
			_category	= category;
			_isReadOnly	= isReadOnly;
			_rank		= Count++;
		}

		private void SetIsReadonly(XmlElement element)
		{
			if (element.HasAttribute("readonly"))
			{
				_isReadOnly = element.GetAttribute("readonly").Equals("true");
			}
		}

		private void SetCategory(XmlElement element)
		{
			if (element.ParentNode.Name == "target")
			{
				foreach (XmlAttribute attribute in element.ParentNode.Attributes)
				{
					if (attribute.Name == "name")
					{
						_category = attribute.Value;
					}
				}
			}
		}

		#region Properties

		public int Rank
		{
			get { return _rank; }
		}



		#endregion
	}
}