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

namespace NAntGui.Framework
{
    public class BuildProperty
    {
        private const string KEY = "{0}.{1}";

        private readonly string _key;
        private readonly string _name;
        private readonly bool _readOnly;
        private string _category;
        private string _defaultExpandedValue;
        private string _expandedValue;
        private string _value;

        public BuildProperty() : this("")
        {
        }

        public BuildProperty(string name) : this(name, "")
        {
        }

        public BuildProperty(string name, string value) : this(name, value, "Global")
        {
        }

        public BuildProperty(string name, string value, string category) :
            this(name, value, category, false)
        {
        }

        public BuildProperty(string name, string value, string category, bool readOnly) :
            this(name, value, category, readOnly, value)
        {
        }

        public BuildProperty(string name, string value, string category,
                             bool readOnly, string expandedValue)
        {
            Assert.NotNull(name, "name");
            Assert.NotNull(value, "value");
            Assert.NotNull(category, "category");
            Assert.NotNull(expandedValue, "expandedValue");

            _name = name;
            _value = value;
            _category = category;
            _readOnly = readOnly;
            _key = string.Format(KEY, category, name);
            _defaultExpandedValue = expandedValue;
            _expandedValue = expandedValue;
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

        public virtual string DefaultExpandedValue
        {
            get { return _defaultExpandedValue; }
            set { _defaultExpandedValue = value; }
        }

        public virtual bool ReadOnly
        {
            get { return _readOnly; }
        }

        public virtual string Key
        {
            get { return _key; }
        }

        public virtual Type Type
        {
            get
            {
                string val = (_value == null) ? "" : _value.ToLower();
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