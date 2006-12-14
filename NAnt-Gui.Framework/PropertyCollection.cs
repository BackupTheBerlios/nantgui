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

using System.Collections;

namespace NAntGui.Framework
{
	/// <summary>
	/// Summary description for PropertyCollection.
	/// </summary>
	public class PropertyCollection : IEnumerable
	{
		private const string KEY = "{0}.{1}";
		private Hashtable _properties;

		public PropertyCollection()
		{
			_properties = new Hashtable();
		}

//		public PropertyCollection(int size)
//		{
//			_properties = new Hashtable(size);
//		}

		public void Add(BuildProperty prop)
		{
			string key = string.Format(KEY, prop.Category, prop.Name);
			_properties.Add(key, prop);
		}

//		public void AddRange(PropertyCollection properties)
//		{
//			foreach (BuildProperty property in properties)
//			{
//				string key = string.Format(KEY, property.Category, property.Name);
//				_properties.Add(key, property);
//			}
//		}

		public IEnumerator GetEnumerator()
		{
			return _properties.Values.GetEnumerator();
		}

		public BuildProperty this[string key]
		{
			get { return (BuildProperty) _properties[key]; }
			set { _properties[key] = value; }
		}
	}
}