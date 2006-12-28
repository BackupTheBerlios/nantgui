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
using Crownwood.Magic.Menus;
using NAntGui.Framework;

namespace NAntGui.Gui.Controls.Menu.ViewMenu
{
	/// <summary>
	/// Summary description for OutputMenuCommand.
	/// </summary>
	public class OutputMenuCommand : MenuCommand
	{
		MainFormMediator _mediator;

		public OutputMenuCommand(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "mediator");
			_mediator = mediator;

			GuiUtils utils = GuiUtils.Instance();
			Description = "MenuCommand";
			ImageIndex = 6;
			Text = "&Output";
			ImageList = utils.ImageList;
		}

		public override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			_mediator.OutputClicked();
		}
	}
}