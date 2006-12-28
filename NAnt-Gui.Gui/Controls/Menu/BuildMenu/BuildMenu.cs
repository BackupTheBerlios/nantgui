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
using NAntGui.Core;
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
		private Settings _settings = Settings.Instance();
		private MainFormMediator _mediator;

		public BuildMenu(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "mediator");
			_mediator = mediator;

			_runMenu = new MenuCommand("&Run", _settings.ImageList, 7, 
				Shortcut.F5, new EventHandler(Run_Click));

			_runMenu.Description = "Builds the current build file";

			_stopMenu = new MenuCommand("&Cancel Build", _settings.ImageList, 3,
				Shortcut.CtrlDel, new EventHandler(Stop_Click));

			_stopMenu.Description = "Aborts the current build";

			MenuCommands.AddRange(new MenuCommand[]
			                      	{
			                      		_runMenu,
			                      		_stopMenu
			                      	});
			Text = "&Build";
		}

		private void Run_Click(object sender, EventArgs e)
		{
			_mediator.RunClicked();
		}

		private void Stop_Click(object sender, EventArgs e)
		{
			_mediator.StopClicked();
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