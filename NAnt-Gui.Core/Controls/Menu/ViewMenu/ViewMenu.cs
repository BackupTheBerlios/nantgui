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
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Controls.Menu.ViewMenu
{
	/// <summary>
	/// Summary description for ViewMenu.
	/// </summary>
	public class ViewMenu : MenuCommand
	{
		private TargetsMenuCommand _targets = new TargetsMenuCommand();
		private PropertiesMenuCommand _properties = new PropertiesMenuCommand();
		private OutputMenuCommand _output = new OutputMenuCommand();

		public ViewMenu()
		{
			this.Description = "MenuCommand";
			this.MenuCommands.AddRange(new MenuCommand[]
				{
					_targets,
					_properties,
					_output
				});
			this.Text = "&View";	//
		}

		public MainFormMediator Mediator
		{
			set
			{
				_targets.SetMediator(value);
				_properties.SetMediator(value);
				_output.SetMediator(value);
			}
		}

		public EventHandler Targets_Click
		{
			set { _targets.Click += value; }
		}

		public EventHandler Properties_Click
		{
			set { _properties.Click += value; }
		}

		public EventHandler Output_Click
		{
			set { _output.Click += value; }
		}

	}
}