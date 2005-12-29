using System;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.FileMenu
{
	/// <summary>
	/// Summary description for RecentMenuCommand.
	/// </summary>
	public class RecentItemMenuCommand : MenuCommand, IClicker
	{
		MainFormMediator _mediator;

		public RecentItemMenuCommand(string text, EventHandler clickHandler) 
			: base(text, clickHandler)
		{
			this.Description = "MenuCommand";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void ExecuteClick()
		{
			_mediator.RecentItemClicked(this.Text.Substring(2));
		}
	}
}