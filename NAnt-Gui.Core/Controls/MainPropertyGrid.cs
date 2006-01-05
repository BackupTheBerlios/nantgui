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

using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using Flobbster.Windows.Forms;
using NAntGui.Framework;

namespace NAntGui.Core.Controls
{
	/// <summary>
	/// Summary description for MainPropertyGrid.
	/// </summary>
	public class MainPropertyGrid : PropertyGrid
	{
		private PropertyTable _propertyTable = new PropertyTable();
		private NameValueCollection _commandLineProperties;

		public MainPropertyGrid()
		{
			_commandLineProperties = NAntGuiApp.Options.Properties;

			this.CommandsVisibleIfAvailable = true;
			this.Dock = DockStyle.Fill;
			this.LargeButtons = false;
			this.LineColor = SystemColors.ScrollBar;
			this.Location = new Point(0, 252);
			this.Name = "ProjectPropertyGrid";
			this.Size = new Size(175, 351);
			this.TabIndex = 4;
			this.Text = "Build Properties";
			this.ViewBackColor = SystemColors.Window;
			this.ViewForeColor = SystemColors.WindowText;
		}

		public void AddProperties(PropertyCollection properties, bool firstLoad)
		{
			_propertyTable.Properties.Clear();

			foreach (BuildProperty property in properties)
			{
				PropertySpec spec = new PropertySpec(property);
				_propertyTable.Properties.Add(spec);

				if (firstLoad && this.CommandLinePropertiesContains(spec.Name))
				{
					_propertyTable[spec.Key] = _commandLineProperties[spec.Name];
				}
				else
				{
					_propertyTable[spec.Key] = property.Value;
				}
			}

			this.SelectedObject = _propertyTable;
		}

		public PropertyCollection GetProperties()
		{
			PropertyCollection properties = new PropertyCollection();

			foreach (PropertySpec spec in _propertyTable.Properties)
			{
				properties.Add((BuildProperty)spec.Tag);
			}

			return properties;
		}

		private bool CommandLinePropertiesContains(string name)
		{
			foreach (string key in _commandLineProperties.AllKeys)
				if (key == name)
					return true;

			return false;
		}
	}
}
