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

using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : Form
	{
		private MainFormMediator _mediator;
		private IContainer components;

		public MainForm(MainFormMediator mediator)
		{
			Initialize();
			Assert.NotNull(mediator, "mediator");
			_mediator = mediator;
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Initialize

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void Initialize()
		{
			components = new Container();
			ResourceManager resources = new ResourceManager(typeof (MainForm));
			SuspendLayout();

			// 
			// MainForm
			// 
			AllowDrop = true;
			AutoScaleBaseSize = new Size(5, 13);
			ClientSize = new Size(824, 553);
			Icon = ((Icon) (resources.GetObject("$this.Icon")));
			MinimumSize = new Size(480, 344);
			Name = "MainForm";
			Text = "NAnt-Gui";
			SetStyle(ControlStyles.DoubleBuffer, true);
			ResumeLayout(false);
		}

		#endregion

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			_mediator.MainFormClosing(e);
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
	}
}