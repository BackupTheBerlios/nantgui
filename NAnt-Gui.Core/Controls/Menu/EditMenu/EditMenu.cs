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

namespace NAntGui.Core.Controls.Menu.EditMenu
{
	/// <summary>
	/// Summary description for EditMenu.
	/// </summary>
	public class EditMenu : MenuCommand
	{
		private UndoMenuCommand _undo;
		private RedoMenuCommand _redo;
		private CutMenuCommand _cut;
		private CopyMenuCommand _copy;
		private PasteMenuCommand _paste;
		private DeleteMenuCommand _delete;
		private SelectAllMenuCommand _selectAll;
		private WordWrapMenuCommand _wordWrap;

		public EditMenu(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "mediator");

			_cut = new CutMenuCommand(mediator);
			_paste = new PasteMenuCommand(mediator);
			_delete = new DeleteMenuCommand(mediator);
			_undo = new UndoMenuCommand(mediator);
			_redo = new RedoMenuCommand(mediator);
			_copy = new CopyMenuCommand(mediator);
			_selectAll = new SelectAllMenuCommand(mediator);
			_wordWrap = new WordWrapMenuCommand(mediator);

			Initialize();
		}

		#region Initialize

		private void Initialize()
		{
			Description = "MenuCommand";
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