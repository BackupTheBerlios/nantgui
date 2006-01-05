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
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Controls.Menu.EditMenu
{
	/// <summary>
	/// Summary description for EditMenu.
	/// </summary>
	public class EditMenu : MenuCommand
	{
		private UndoMenuCommand _undo = new UndoMenuCommand();
		private RedoMenuCommand _redo = new RedoMenuCommand();
		private CopyMenuCommand _copy = new CopyMenuCommand();
		private SelectAllMenuCommand _selectAll = new SelectAllMenuCommand();
		private WordWrapMenuCommand _wordWrap = new WordWrapMenuCommand();

		public EditMenu()
		{
			//
			// EditMainMenuCommand
			//
			this.Description = "MenuCommand";
			this.MenuCommands.AddRange(new MenuCommand[]
				{
					_undo,
					_redo,
					new MenuCommand("-"),
					_copy,
					_selectAll,
					_wordWrap
				});
			this.Text = "&Edit";
		}

		public MainFormMediator Mediator
		{
			set
			{
				_undo.Mediator = value;
				_redo.Mediator = value;
				_copy.Mediator = value;
				_selectAll.Mediator = value;
				_wordWrap.Mediator = value;
			}
		}

		public void EnablePasteAndDelete()
		{
			throw new NotImplementedException();
		}

		public void DisablePasteAndDelete()
		{
			throw new NotImplementedException();
		}

		public EventHandler Undo_Click
		{
			set { _undo.Click += value; }
		}

		public EventHandler Redo_Click
		{
			set { _redo.Click += value; }
		}

		public EventHandler Copy_Click
		{
			set { _copy.Click += value; }
		}

		public EventHandler SelectAll_Click
		{
			set { _selectAll.Click += value; }
		}

		public EventHandler WordWrap_Click
		{
			set { _wordWrap.Click += value; }
		}

		public bool WordWrapChecked
		{
			get { return _wordWrap.Checked; }
			set { _wordWrap.Checked = value; }
		}
	}
}