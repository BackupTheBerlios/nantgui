using System;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.FileMenu
{
	/// <summary>
	/// Summary description for FileMenu.
	/// </summary>
	public class FileMenu : MenuCommand
	{
		private NewMenuCommand _new = new NewMenuCommand();
		private OpenMenuCommand _open = new OpenMenuCommand();
		private SaveMenuCommand _save = new SaveMenuCommand();
		private SaveAsMenuCommand _saveAs = new SaveAsMenuCommand();
		private ReloadMenuCommand _reload = new ReloadMenuCommand();
		private CloseMenuCommand _close = new CloseMenuCommand();
		private SaveOutputMenuCommand _saveOutput = new SaveOutputMenuCommand();
		private RecentItemsMenu _recent = new RecentItemsMenu();
		private ExitMenuCommand _exit = new ExitMenuCommand();

		public FileMenu()
		{
			this.Description = "MenuCommand";
			this.MenuCommands.AddRange(new MenuCommand[]
				{
					_new, _open, _save, _saveAs, _reload, _close,
					_saveOutput, new MenuCommand("-"), _recent,
					new MenuCommand("-"), _exit
				});
			this.Text = "&File";
		}

		public void SetMediator(MainFormMediator mediator)
		{
			_new.Mediator		 = mediator;
			_open.Mediator		 = mediator;
			_save.Mediator		 = mediator;
			_saveAs.Mediator	 = mediator;
			_reload.Mediator	 = mediator;
			_close.Mediator		 = mediator;
			_saveOutput.Mediator = mediator;
			_exit.Mediator		 = mediator;
			_recent.Mediator	 = mediator;
		}

		public void Enable()
		{
			_reload.Enabled =
				_save.Enabled =
					_saveAs.Enabled =
						_close.Enabled = true;
		}

		public void Disable()
		{
			_reload.Enabled =
				_save.Enabled =
					_saveAs.Enabled =
						_close.Enabled = false;
		}


		public void AddRecentItem(string file)
		{
			_recent.AddRecentItem(file);
		}

		public void RemoveRecentItem(string file)
		{
			_recent.RemoveRecentItem(file);
		}

		#region Events

		public EventHandler New_Click
		{
			set { _new.Click += value; }
		}

		public EventHandler Open_Click
		{
			set { _open.Click += value; }
		}

		public EventHandler Save_Click
		{
			set { _save.Click += value; }
		}

		public EventHandler SaveAs_Click
		{
			set { _saveAs.Click += value; }
		}

		public EventHandler Reload_Click
		{
			set { _reload.Click += value; }
		}

		public EventHandler Close_Click
		{
			set { _close.Click += value; }
		}

		public EventHandler SaveOutput_Click
		{
			set { _saveOutput.Click += value; }
		}

		public EventHandler Exit_Click
		{
			set { _exit.Click += value; }
		}

		public EventHandler Recent_Click
		{
			set { _recent.Recent_Click = value; }
		}

		public bool HasRecentItems
		{
			get { return _recent.HasRecentItems; }
		}

		public string FirstRecentItem
		{
			get { return _recent.FirstRecentItem; }
		}

		#endregion

	}
}