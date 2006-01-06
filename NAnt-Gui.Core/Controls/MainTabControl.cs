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

using System.Windows.Forms;
using TabControl = Crownwood.Magic.Controls.TabControl;

namespace NAntGui.Core.Controls
{
	/// <summary>
	/// Summary description for MainTabControl.
	/// </summary>
	public class MainTabControl : TabControl
	{
		private MainFormMediator _mediator;

		public MainTabControl()
		{
			this.SuspendLayout();

			this.Appearance = VisualAppearance.MultiDocument;
			this.Dock = DockStyle.Fill;
			this.IDEPixelArea = true;
			this.IDEPixelBorder = false;

			this.ResumeLayout(false);
		}

		public MainFormMediator Mediator
		{
			set
			{
				Assert.NotNull(value, "value");
				_mediator = value;
			}
		}


		protected override void OnDragEnter(DragEventArgs e)
		{
			Assert.NotNull(e, "e");
			Assert.NotNull(_mediator, "_mediator");
			base.OnDragEnter(e);
			_mediator.DragEnter(e);
		}

		protected override void OnDragDrop(DragEventArgs e)
		{
			Assert.NotNull(e, "e");
			Assert.NotNull(_mediator, "_mediator");
			base.OnDragDrop(e);
			_mediator.DragDrop(e);
		}
	}
}
