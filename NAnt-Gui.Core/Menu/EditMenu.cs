using System;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu
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

		public void SetMediator(MainFormMediator mediator)
		{
			_undo.Mediator		= mediator;
			_redo.Mediator		= mediator;
			_copy.Mediator		= mediator;
			_selectAll.Mediator = mediator;
			_wordWrap.Mediator	= mediator;
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