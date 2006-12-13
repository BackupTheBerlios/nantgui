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

using Crownwood.Magic.Menus;

namespace NAntGui.Core.Controls.Menu.HelpMenu
{
	/// <summary>
	/// Summary description for HelpMenu.
	/// </summary>
	public class HelpMenu : MenuCommand
	{
		private NAntContribMenuCommand _nantContrib;
		private NAntHelpMenuCommand _nant;
		private NAntSDKMenuCommand _nantSDK;
		private AboutMenuCommand _about;

		public HelpMenu(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "mediator");

			_nantContrib = new NAntContribMenuCommand(mediator);
			_nant = new NAntHelpMenuCommand(mediator);
			_nantSDK = new NAntSDKMenuCommand(mediator);
			_about = new AboutMenuCommand(mediator);

			Description = "MenuCommand";
			MenuCommands.AddRange(new MenuCommand[]
			                      	{
			                      		_nant,
			                      		_nantSDK,
			                      		_nantContrib,
			                      		new MenuCommand("-"),
			                      		_about
			                      	});
			Text = "&Help";
		}
	}
}