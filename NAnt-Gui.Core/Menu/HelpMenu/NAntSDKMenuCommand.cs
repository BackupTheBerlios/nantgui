using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.HelpMenu
{
	/// <summary>
	/// Summary description for NAntSDKMenuCommand.
	/// </summary>
	public class NAntSDKMenuCommand : MenuCommand, IClicker
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

		public void ExecuteClick()
		{
			_mediator.NAntSDKClicked();
		}
	}
}