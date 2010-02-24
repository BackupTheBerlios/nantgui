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
using System.Drawing;

namespace NAntGui.Gui
{
    class FileNameEventArgs : EventArgs
    {
        private string _fileName;
        private Point _point;

        internal FileNameEventArgs(string fileName, Point p)
        {
            _fileName = fileName;
            _point = p;
        }

        internal string FileName
        {
            get { return _fileName; }
        }

        internal Point Point
        {
            get { return _point; }
        }
    }

    class RecentItemsEventArgs : EventArgs
    {
        private string _item;

        internal RecentItemsEventArgs(string item)
        {
            _item = item;
        }

        internal string Item
        {
            get { return _item; }
        }
    }

    class RunEventArgs : EventArgs
    {
        private string _target;

        internal RunEventArgs(string target)
        {
            _target = target;
        }

        internal string Target
        {
            get { return _target; }
        }
    }
}
