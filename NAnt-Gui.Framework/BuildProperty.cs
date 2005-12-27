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

using System;

namespace NAntGui.Framework
{
	public class BuildProperty
	{
		protected string _name = "";
		protected string _value = "";
		protected string _expandedValue = "";
		protected string _category = "Global";
		protected bool _isReadOnly = false;

		public BuildProperty(){}

		public BuildProperty(string name)
		{
			Assert.NotNull(name, "name");
			_name = name;
		}

		public BuildProperty(string name, string value) : this(name)
		{
			Assert.NotNull(value, "value");
			_value = value;
		}

		public BuildProperty(string name, string value, string expandedValue) : this(name, value)
		{
			Assert.NotNull(expandedValue, "expandedValue");
			_expandedValue = expandedValue;
		}

		public BuildProperty(string name, string value, string expandedValue, 
			string category) : this(name, value, expandedValue)
		{
			Assert.NotNull(category, "category");
			_category = category;
		}

		public BuildProperty(string name, string value, string expandedValue, 
			string category, bool isReadOnly) : this(name, value, expandedValue, category)
		{
			_isReadOnly = isReadOnly;
		}

		public virtual string Name
		{
			get { return _name; }
		}

		public virtual string Value
		{
			get { return _value; }
			set { _value = value; }
		}

		public virtual string Category
		{
			get { return _category; }
			set { _category = value; }
		}

		public virtual bool IsReadOnly
		{
			get { return _isReadOnly; }
		}

		public virtual Type Type
		{
			get
			{
				string val = _value.ToLower();
				return (val == "true" || val == "false") ? typeof (bool) : typeof (string);
			}
		}

		public virtual string ExpandedValue
		{
			get { return _expandedValue; }
			set { _expandedValue = value; }
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", _name, _value);
		}
	}
}
