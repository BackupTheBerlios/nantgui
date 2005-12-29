using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu
{
	/// <summary>
	/// Summary description for RecentMenuCommand.
	/// </summary>
	public class RecentMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public RecentMenuCommand()
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
			_mediator.RecentClicked();
		}
	}
}