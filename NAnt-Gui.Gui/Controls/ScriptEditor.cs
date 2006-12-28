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

using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Menus;
using ICSharpCode.TextEditor;
using NAntGui.Framework;
using NAntGui.Gui.Controls.Menu.EditMenu;

namespace NAntGui.Gui.Controls
{
	/// <summary>
	/// Summary description for ScriptEditor.
	/// </summary>
	public class ScriptEditor : TextEditorControl
	{
		private PopupMenu _sourceContextMenu;
		private MainFormMediator _mediator;
		private GuiUtils _utils = GuiUtils.Instance();

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
						new MenuCommand("Cu&t", _utils.ImageList, 12,
						Shortcut.CtrlX, new System.EventHandler(Cut_Click)),
						new MenuCommand("&Copy", _utils.ImageList, 13,
						Shortcut.CtrlC, new System.EventHandler(Copy_Click)),
						new MenuCommand("&Paste", _utils.ImageList, 14,
						Shortcut.CtrlV, new System.EventHandler(Paste_Click)),
						new MenuCommand("&Delete", _utils.ImageList, 15, 
						Shortcut.Del, new System.EventHandler(Delete_Click)),
						new MenuCommand("-"),
						new SelectAllMenuCommand(_mediator)
					});

			Dock = DockStyle.Fill;
			Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
			ShowVRuler = false;
			ShowEOLMarkers = false;
			ShowSpaces = false;
			ShowTabs = false;
			EnableFolding = true;
			ShowMatchingBracket = true;

			ActiveTextAreaControl.TextArea.MouseUp += new MouseEventHandler(PopupMenu);
			ActiveTextAreaControl.TextArea.DragEnter += new DragEventHandler(Drag_Enter);
			ActiveTextAreaControl.TextArea.DragDrop += new DragEventHandler(Drag_Drop);
		}

		#endregion

		private void Cut_Click(object sender, System.EventArgs e)
		{
			_mediator.CutClicked();
		}

		private void Copy_Click(object sender, System.EventArgs e)
		{
			_mediator.CopyClicked();
		}

		private void Paste_Click(object sender, System.EventArgs e)
		{
			_mediator.PasteClicked();
		}

		private void Delete_Click(object sender, System.EventArgs e)
		{
			_mediator.DeleteClicked();
		}

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