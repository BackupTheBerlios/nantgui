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

using System.Collections;

namespace NAntGui.Framework
{
	/// <summary>
	/// Summary description for PropertyCollection.
	/// </summary>
	public class PropertyCollection : IEnumerable
	{
		private Hashtable _properties;

		public PropertyCollection()
		{
			_properties = new Hashtable();
		}

		public PropertyCollection(int size)
		{
			_properties = new Hashtable(size);
		}

		public void Add(BuildProperty buildProperty)
		{
			_properties.Add(buildProperty.Name, buildProperty);
		}

		public void AddRange(PropertyCollection properties)
		{
			foreach (BuildProperty property in properties)
			{
				_properties.Add(property.Name, property);
			}
		}

		public IEnumerator GetEnumerator()
		{
			return _properties.Values.GetEnumerator();
		}

		public bool Contains(string key)
		{
			return _properties.Contains(key);
		}

		public BuildProperty this[string key]
		{
			get { return (BuildProperty) _properties[key]; }
			set { _properties[key] = value; }
		}
	}
}