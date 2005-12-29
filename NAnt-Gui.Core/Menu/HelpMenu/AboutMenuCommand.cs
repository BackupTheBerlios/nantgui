using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.HelpMenu
{
	/// <summary>
	/// Summary description for AboutMenuCommand.
	/// </summary>
	public class AboutMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public AboutMenuCommand()
		{
			this.Description = "MenuCommand";
			this.Text = "&About NAnt-Gui";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.AboutClicked();
		}
	}
}