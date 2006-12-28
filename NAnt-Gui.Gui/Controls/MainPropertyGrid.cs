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

using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using NAntGui.Framework;

namespace NAntGui.Gui.Controls
{
	/// <summary>
	/// Summary description for MainPropertyGrid.
	/// </summary>
	public class MainPropertyGrid : PropertyGrid
	{
		private PropertyShelf _propertyTable = new PropertyShelf();
		private PropertyCollection _properties;

		public MainPropertyGrid()
		{
			_propertyTable.SetValue += new PropertySpecEventHandler(this.SetValue);
			_propertyTable.GetValue += new PropertySpecEventHandler(this.GetValue);

			CommandsVisibleIfAvailable = true;
			Dock			= DockStyle.Fill;
			LargeButtons	= false;
			LineColor		= SystemColors.ScrollBar;
			Location		= new Point(0, 252);
			Name			= "ProjectPropertyGrid";
			Size			= new Size(175, 351);
			TabIndex		= 4;
			Text			= "Build Properties";
			ViewBackColor	= SystemColors.Window;
			ViewForeColor	= SystemColors.WindowText;
		}

		public void AddProperties(PropertyCollection properties)
		{
			_propertyTable.Properties.Clear();
			_properties = properties;

			foreach (BuildProperty property in _properties)
			{
				PropertySpec spec = new PropertySpec(property);
				_propertyTable.Properties.Add(spec);
			}

			SelectedObject = _propertyTable;
		}

		public PropertyCollection GetProperties()
		{
			return _properties;
		}

		public void Clear()
		{
			SelectedObject = null;
		}

		private void SetValue(object sender, PropertySpecEventArgs e)
		{			
			_properties[e.Property.Key].Value = e.Value.ToString();			
		}

		private void GetValue(object sender, PropertySpecEventArgs e)
		{			
			e.Value = _properties[e.Property.Key].Value;
		}
	}
}