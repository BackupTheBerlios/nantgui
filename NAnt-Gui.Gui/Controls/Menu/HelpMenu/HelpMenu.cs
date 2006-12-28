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

using Crownwood.Magic.Menus;
using NAntGui.Framework;

namespace NAntGui.Gui.Controls.Menu.HelpMenu
{
	/// <summary>
	/// Summary description for HelpMenu.
	/// </summary>
	public class HelpMenu : MenuCommand
	{
		MainFormMediator _mediator;

		public HelpMenu(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "mediator");
			_mediator = mediator;

			Description = "MenuCommand";
			MenuCommands.AddRange(new MenuCommand[]
			                      	{
			                      		new MenuCommand("NAnt &Help",
											new System.EventHandler(NAnt_Click)),
			                      		new MenuCommand("NAnt &SDK Help", 
											new System.EventHandler(NAntSDK_Click)),
			                      		new MenuCommand("NAnt-&Contrib Help", 
											new System.EventHandler(NAntContrib_Click)),
			                      		new MenuCommand("-"),
			                      		new MenuCommand("&About NAnt-Gui", 
											new System.EventHandler(About_Click))
			                      	});
			Text = "&Help";
		}

		private void NAnt_Click(object sender, System.EventArgs e)
		{
			_mediator.NAntHelpClicked();
		}

		private void NAntSDK_Click(object sender, System.EventArgs e)
		{
			_mediator.NAntSDKClicked();
		}

		private void NAntContrib_Click(object sender, System.EventArgs e)
		{
			_mediator.NAntContribClicked();
		}

		private void About_Click(object sender, System.EventArgs e)
		{
			_mediator.AboutClicked();
		}
	}
}