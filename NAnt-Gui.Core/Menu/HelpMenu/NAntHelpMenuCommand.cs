using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.HelpMenu
{
	/// <summary>
	/// Summary description for NAntHelpMenuCommand.
	/// </summary>
	public class NAntHelpMenuCommand : MenuCommand, IClicker
	{
		MainFormMediator _mediator;

		public NAntHelpMenuCommand()
		{
			this.Description = "MenuCommand";
			this.Text = "NAnt &Help";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void ExecuteClick()
		{
			_mediator.NAntHelpClicked();
		}
	}
}