using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.ViewMenu
{
	/// <summary>
	/// Summary description for PropertiesMenuCommand.
	/// </summary>
	public class PropertiesMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public PropertiesMenuCommand()
		{
			this.Description = "MenuCommand";
			this.ImageIndex = 0;
			this.Text = "&Properties";
			this.ImageList = NAntGuiApp.ImageList;
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.PropertiesClicked();
		}
	}
}