using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.HelpMenu
{
	/// <summary>
	/// Summary description for NAntContribMenuCommand.
	/// </summary>
	public class NAntContribMenuCommand : MenuCommand, IClicker
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

		public void ExecuteClick()
		{
			_mediator.NAntContribClicked();
		}
	}
}