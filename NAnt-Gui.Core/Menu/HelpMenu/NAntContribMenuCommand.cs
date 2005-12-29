using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.HelpMenu
{
	/// <summary>
	/// Summary description for NAntContribMenuCommand.
	/// </summary>
	public class NAntContribMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public NAntContribMenuCommand()
		{
			this.Description = "MenuCommand";
			this.Text = "NAnt-&Contrib Help";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.NAntContribClicked();
		}
	}
}