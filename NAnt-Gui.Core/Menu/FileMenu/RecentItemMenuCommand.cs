using System;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.FileMenu
{
	/// <summary>
	/// Summary description for RecentMenuCommand.
	/// </summary>
	public class RecentItemMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public RecentItemMenuCommand(string text, EventHandler clickHandler) 
			: base(text, clickHandler)
		{
			this.Description = "MenuCommand";
			this.Text = "Recent &Files";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.RecentItemClicked(this.Text.Substring(2));
		}
	}
}