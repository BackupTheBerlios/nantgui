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

using System;
using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Menus;
using ICSharpCode.TextEditor;
using NAntGui.Core.Controls.Menu.EditMenu;

namespace NAntGui.Core.Controls
{
	/// <summary>
	/// Summary description for ScriptEditor.
	/// </summary>
	public class ScriptEditor : TextEditorControl
	{
		private PopupMenu _sourceContextMenu;
		private MainFormMediator _mediator;

		public ScriptEditor(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "value");
			_mediator = mediator;

			Initialize();
		}

		#region Initialize

		private void Initialize()
		{
			_sourceContextMenu = new PopupMenu();
			_sourceContextMenu.MenuCommands.AddRange(
				new MenuCommand[]
					{
						new CopyMenuCommand(_mediator), 
						new CutMenuCommand(_mediator), 
						new PasteMenuCommand(_mediator), 
						new DeleteMenuCommand(_mediator), 
						new MenuCommand("-"), 
						new SelectAllMenuCommand(_mediator)
					});
	
			this.Dock = DockStyle.Fill;
			this.Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((Byte) (0)));
			this.ShowVRuler = false;
			this.ShowEOLMarkers = false;
			this.ShowSpaces = false;
			this.ShowTabs = false;
			this.EnableFolding = true;
			this.ShowMatchingBracket = true;
			this.ActiveTextAreaControl.TextArea.MouseUp += new MouseEventHandler(this.PopupMenu);
			this.ActiveTextAreaControl.TextArea.DragEnter += new DragEventHandler(this.Drag_Enter);
			this.ActiveTextAreaControl.TextArea.DragDrop += new DragEventHandler(this.Drag_Drop);
		}

		#endregion

		private void PopupMenu(object sender, MouseEventArgs e)
		{
			Assert.NotNull(sender, "sender");
			Assert.NotNull(e, "e");

			if (sender is TextArea && e.Button == MouseButtons.Right)
			{
				TextArea area = sender as TextArea;
				_sourceContextMenu.TrackPopup(area.PointToScreen(new Point(e.X, e.Y)));
			}
		}

		private void Drag_Drop(object sender, DragEventArgs e)
		{
			_mediator.DragDrop(e);
		}

		public void Drag_Enter(object sender, DragEventArgs e)
		{
			_mediator.DragEnter(e);
		}
	}
}
