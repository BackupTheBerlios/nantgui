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

using System.Windows.Forms;
using NAntGui.Framework;
using TabControl = Crownwood.Magic.Controls.TabControl;

namespace NAntGui.Gui.Controls
{
	/// <summary>
	/// Summary description for MainTabControl.
	/// </summary>
	public class MainTabControl : TabControl
	{
		private MainFormMediator _mediator;

		public MainTabControl(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "value");
			_mediator = mediator;

			SuspendLayout();

			Appearance = VisualAppearance.MultiDocument;
			Dock = DockStyle.Fill;
			IDEPixelArea = true;
			IDEPixelBorder = false;

			ContextPopupMenu = new Crownwood.Magic.Menus.PopupMenu();
			ContextPopupMenu.MenuCommands.Add(new Menu.FileMenu.CloseMenuCommand(_mediator));
			this.PopupMenuDisplay += new System.ComponentModel.CancelEventHandler(PopupCancel);

			ResumeLayout(false);
		}

		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);
			_mediator.DragEnter(e);
		}

		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);
			_mediator.DragDrop(e);
		}

		protected void PopupCancel(object sender, System.ComponentModel.CancelEventArgs e)
		{
			MessageBox.Show("hello");
		}
	}
}