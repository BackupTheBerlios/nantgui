#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2005 Colin Svingen, Business Watch International
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
using ICSharpCode.TextEditor;

namespace NAntGui.Core.Controls
{
	/// <summary>
	/// Summary description for ScriptEditor.
	/// </summary>
	public class ScriptEditor : TextEditorControl
	{
		private ScriptEditorContextMenu _sourceContextMenu;
		private MainFormMediator _mediator;

		public ScriptEditor()
		{
			Initialize();
		}

		#region Initialize

		private void Initialize()
		{
			_sourceContextMenu = new ScriptEditorContextMenu(this.ActiveTextAreaControl.TextArea.ClipboardHandler);
	
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

		public MainFormMediator Mediator
		{
			set
			{
				Assert.NotNull(value, "value");
				_mediator = value;
			}
		}

		private void Drag_Drop(object sender, DragEventArgs e)
		{
			Assert.NotNull(e, "e");
			_mediator.DragDrop(e);
		}

		public void Drag_Enter(object sender, DragEventArgs e)
		{
			Assert.NotNull(e, "e");
			Assert.NotNull(_mediator, "_mediator");
			_mediator.DragEnter(e);
		}
	}
}
