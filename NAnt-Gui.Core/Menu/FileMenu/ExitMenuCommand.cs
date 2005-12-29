using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.FileMenu
{
	/// <summary>
	/// Summary description for ExitMenuCommand.
	/// </summary>
	public class ExitMenuCommand : MenuCommand, IClicker
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

		public void ExecuteClick()
		{
			_mediator.ExitClicked();
		}
	}
}