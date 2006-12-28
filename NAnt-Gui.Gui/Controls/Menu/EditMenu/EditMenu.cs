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

using NAntGui.Core;
using NAntGui.Framework;
using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Gui.Controls.Menu.EditMenu
{
	/// <summary>
	/// Summary description for EditMenu.
	/// </summary>
	public class EditMenu : MenuCommand
	{
		private MenuCommand _undo;
		private MenuCommand _redo;
		private MenuCommand _cut;
		private MenuCommand _copy;
		private MenuCommand _paste;
		private MenuCommand _delete;
		private MenuCommand _selectAll;
		private MenuCommand _wordWrap;

		MainFormMediator _mediator;
		GuiUtils _utils = GuiUtils.Instance();

		public EditMenu(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "mediator");
			_mediator = mediator;		

			Initialize();
		}

		#region Initialize

		private void Initialize()
		{
			// _undo
			_undo = new MenuCommand("&Undo", _utils.ImageList, 10, 
				Shortcut.CtrlZ, new System.EventHandler(Undo_Click));
			_undo.Description = "Undo previous edit";

			// _redo
			_redo = new MenuCommand("&Redo", _utils.ImageList, 11,
				Shortcut.CtrlY, new System.EventHandler(Redo_Click));
			_redo.Description = "Redo previous edit";

			// _cut
			_cut = new MenuCommand("Cu&t", _utils.ImageList, 12,
				Shortcut.CtrlX, new System.EventHandler(Cut_Click));
			_cut.Description = "Cut selected text";

			// _copy
			_copy = new MenuCommand("&Copy", _utils.ImageList, 13,
				Shortcut.CtrlC, new System.EventHandler(Copy_Click));
			_copy.Description = "Copy selected text";

			// _paste
			_paste = new MenuCommand("&Paste", _utils.ImageList, 14,
				Shortcut.CtrlV, new System.EventHandler(Paste_Click));
			_paste.Description = "Paste copied text";

			// _delete
			_delete = new MenuCommand("&Delete", _utils.ImageList, 15, 
				Shortcut.Del, new System.EventHandler(Delete_Click));
			_delete.Description = "Delete the selected text";

			// _selectAll
			_selectAll = new MenuCommand("Select &All", Shortcut.CtrlA, 
				new System.EventHandler(SelectAll_Click));

			// _wordWrap
			_wordWrap = new MenuCommand("&Word Wrap", 
				new System.EventHandler(WordWrap_Click));


			MenuCommands.AddRange(new MenuCommand[]
			                      	{
			                      		_undo,
			                      		_redo,
			                      		new MenuCommand("-"),
			                      		_cut,
			                      		_copy,
			                      		_paste,
			                      		_delete,
			                      		new MenuCommand("-"),
			                      		_selectAll,
			                      		_wordWrap
			                      	});
			Text = "&Edit";
		}

		#endregion

		#region Click Handlers

		private void Undo_Click(object sender, System.EventArgs e)
		{
			_mediator.UndoClicked();
		}

		private void Redo_Click(object sender, System.EventArgs e)
		{
			_mediator.RedoClicked();
		}

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

		private void SelectAll_Click(object sender, System.EventArgs e)
		{
			_mediator.SelectAllClicked();
		}

		private void WordWrap_Click(object sender, System.EventArgs e)
		{
			_mediator.WordWrapClicked();
		}

		#endregion

		public bool WordWrapChecked
		{
			get { return _wordWrap.Checked; }
			set { _wordWrap.Checked = value; }
		}

		public EditState State
		{
			set
			{
				switch (value)
				{
					case EditState.TabFocused:
						_cut.Enabled = true;
						_paste.Enabled = true;
						_delete.Enabled = true;
						_undo.Enabled = true;
						_redo.Enabled = true;
						_copy.Enabled = true;
						_selectAll.Enabled = true;
						_wordWrap.Enabled = true;
						break;
					case EditState.OutputFocused:
						_cut.Enabled = false;
						_paste.Enabled = false;
						_delete.Enabled = false;
						_undo.Enabled = false;
						_redo.Enabled = false;
						_copy.Enabled = true;
						_selectAll.Enabled = true;
						_wordWrap.Enabled = true;
						break;
					case EditState.NoFocus:
						_cut.Enabled = false;
						_paste.Enabled = false;
						_delete.Enabled = false;
						_undo.Enabled = false;
						_redo.Enabled = false;
						_copy.Enabled = false;
						_selectAll.Enabled = false;
						_wordWrap.Enabled = false;
						break;
				}
			}
		}
	}
}