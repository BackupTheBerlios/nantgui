using System;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.FileMenu
{
	/// <summary>
	/// Summary description for RecentItemsMenuCommand.
	/// </summary>
	public class RecentItemsMenu : MenuCommand
	{
		private RecentItems _recentItems = new RecentItems();
		EventHandler _onClick;
		private MainFormMediator _mediator;

		public RecentItemsMenu()
		{
			this.Description = "MenuCommand";
			this.Text = "Recent &Items";
			this.CreateMenuCommands();
		}

		public void AddRecentItem(string file)
		{
			_recentItems.Add(file);
			_recentItems.Save();

			this.CreateMenuCommands();
		}
		
		public void RemoveRecentItem(string file)
		{
			_recentItems.Remove(file);
			_recentItems.Save();

			this.CreateMenuCommands();
		}

		private void CreateMenuCommands()
		{
			this.MenuCommands.Clear();
	
			int count = 1;
			foreach (string item in _recentItems)
			{
				string name = count++ + " " + item;
				RecentItemMenuCommand recentItem = new RecentItemMenuCommand(name, _onClick);
				recentItem.Mediator = _mediator;
				this.MenuCommands.Add(recentItem);
			}
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public EventHandler Recent_Click
		{
			set { _onClick = value; }
		}

		public bool HasRecentItems
		{
			get { return _recentItems.HasRecentItems; }
		}

		public string FirstRecentItem
		{
			get { return _recentItems[0]; }
		}
	}
}