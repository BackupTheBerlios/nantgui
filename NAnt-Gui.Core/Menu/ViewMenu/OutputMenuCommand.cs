using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.ViewMenu
{
	/// <summary>
	/// Summary description for OutputMenuCommand.
	/// </summary>
	public class OutputMenuCommand : MenuCommand, IClicker
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

		public void ExecuteClick()
		{
			_mediator.OutputClicked();
		}
	}
}