using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.ViewMenu
{
	/// <summary>
	/// Summary description for OutputMenuCommand.
	/// </summary>
	public class OutputMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public OutputMenuCommand()
		{
			this.Description = "MenuCommand";
			this.ImageIndex = 6;
			this.Text = "&Output";
			this.ImageList = NAntGuiApp.ImageList;
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.OutputClicked();
		}
	}
}