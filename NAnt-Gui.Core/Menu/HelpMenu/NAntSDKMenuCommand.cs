using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.HelpMenu
{
	/// <summary>
	/// Summary description for NAntSDKMenuCommand.
	/// </summary>
	public class NAntSDKMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public NAntSDKMenuCommand()
		{
			this.Description = "MenuCommand";
			this.Text = "NAnt &SDK Help";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.NAntSDKClicked();
		}
	}
}