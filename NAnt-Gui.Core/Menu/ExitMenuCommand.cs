using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu
{
	/// <summary>
	/// Summary description for ExitMenuCommand.
	/// </summary>
	public class ExitMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public ExitMenuCommand()
		{
			this.Description = "MenuCommand";
			this.Text = "&Exit";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.ExitClicked();
		}
	}
}