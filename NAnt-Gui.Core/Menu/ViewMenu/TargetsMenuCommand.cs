using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.ViewMenu
{
	/// <summary>
	/// Summary description for TargetsMenuCommand.
	/// </summary>
	public class TargetsMenuCommand : MenuCommand, IClicker
	{
		MainFormMediator _mediator;

		public TargetsMenuCommand()
		{
			this.Description = "MenuCommand";
			this.ImageList = NAntGuiApp.ImageList;
			this.ImageIndex = 9;
			this.Text = "&Targets";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void ExecuteClick()
		{
			_mediator.TargetsClicked();
		}
	}
}