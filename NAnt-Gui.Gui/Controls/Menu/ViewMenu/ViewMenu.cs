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

namespace NAntGui.Gui.Controls.Menu.ViewMenu
{
	/// <summary>
	/// Summary description for ViewMenu.
	/// </summary>
	public class ViewMenu : MenuCommand
	{
		private MainFormMediator _mediator;

		public ViewMenu(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "mediator");
			_mediator = mediator;

			GuiUtils utils = GuiUtils.Instance();

			Description = "MenuCommand";
			MenuCommands.AddRange(new MenuCommand[]
			                      	{
			                      		new MenuCommand("&Targets", utils.ImageList, 9, 
											new System.EventHandler(Targets_Click)),
										new MenuCommand("&Properties", utils.ImageList, 0, 
											new System.EventHandler(Properties_Click)),
										new MenuCommand("&Output", utils.ImageList, 6, 
											new System.EventHandler(Output_Click))
			                      	});
			Text = "&View";
		}

		private void Targets_Click(object sender, System.EventArgs e)
		{
			_mediator.TargetsClicked();
		}

		private void Properties_Click(object sender, System.EventArgs e)
		{
			_mediator.PropertiesClicked();
		}

		private void Output_Click(object sender, System.EventArgs e)
		{
			_mediator.OutputClicked();
		}
	}
}