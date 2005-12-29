using System;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.FileMenu
{
	/// <summary>
	/// Summary description for RecentMenuCommand.
	/// </summary>
	public class RecentMenuCommand : MenuCommand, Command
	{
		private RecentItems _recentItems = new RecentItems();
		MainFormMediator _mediator;

		public RecentMenuCommand()
		{
			this.Description = "MenuCommand";
			this.Text = "Recent &Files";

			_recentItems.Load();
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.RecentItemAdded();
		}

		public EventHandler ItemAdded
		{
			set { _recentItems.ItemsUpdated += value; }
		}
	}
}