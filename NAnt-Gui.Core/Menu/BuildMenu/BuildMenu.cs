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

using System;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.BuildMenu
{
	/// <summary>
	/// Summary description for BuildMenu.
	/// </summary>
	public class BuildMenu : MenuCommand
	{
		private RunMenuCommand _runMenu = new RunMenuCommand();

		public BuildMenu()
		{
			this.Description = "MenuCommand";
			this.MenuCommands.AddRange(new MenuCommand[]
				{
					_runMenu
				});
			this.Text = "&Build";
		}

		public MainFormMediator Mediator
		{
			set { _runMenu.Mediator = value; }
		}

		public void Enable()
		{
			_runMenu.Enabled = true;
		}

		public void Disable()
		{
			_runMenu.Enabled = false;
		}

		public EventHandler Build_Click
		{
			set { _runMenu.Click += value; }
		}
	}
}