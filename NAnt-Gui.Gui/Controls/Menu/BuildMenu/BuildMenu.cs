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

using System;
using NAntGui.Framework;
using Crownwood.Magic.Menus;
using System.Windows.Forms;

namespace NAntGui.Gui.Controls.Menu.BuildMenu
{
	/// <summary>
	/// Summary description for BuildMenu.
	/// </summary>
	public class BuildMenu : MenuCommand
	{
		private MenuCommand _runMenu;
		private MenuCommand _stopMenu;
		private GuiUtils _utils = GuiUtils.Instance();
		private MainFormMediator _mediator;

		public BuildMenu(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "mediator");
			_mediator = mediator;

			_runMenu = new MenuCommand("&Run", _utils.ImageList, 7, 
				Shortcut.F5);

			_runMenu.Description = "Builds the current build file";

			_stopMenu = new MenuCommand("&Cancel Build", _utils.ImageList, 3,
				Shortcut.CtrlDel);

			_stopMenu.Description = "Aborts the current build";

			MenuCommands.AddRange(new MenuCommand[]
			                      	{
			                      		_runMenu,
			                      		_stopMenu
			                      	});
			Text = "&Build";
		}
		
		public event EventHandler RunClick
		{
			add { _runMenu.Click += value; }
			remove { _runMenu.Click -= value; }
		}		
		
		public event EventHandler StopClick
		{
			add { _stopMenu.Click += value; }
			remove { _stopMenu.Click -= value; }
		}

		public void Enable()
		{
			_runMenu.Enabled = true;
			_stopMenu.Enabled = false;
		}

		public void Disable()
		{
			_runMenu.Enabled = false;
			_stopMenu.Enabled = false;
		}

		public RunState State
		{
			set
			{
				switch (value)
				{
					case RunState.Running:
						_runMenu.Enabled = false;
						_stopMenu.Enabled = true;
						break;
					case RunState.Stopped:
						_runMenu.Enabled = true;
						_stopMenu.Enabled = false;
						break;
				}
			}
		}
	}
}