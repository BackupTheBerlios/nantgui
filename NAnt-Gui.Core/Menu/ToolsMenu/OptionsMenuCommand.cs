using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.ToolsMenu
{
	/// <summary>
	/// Summary description for OptionsMenuCommand.
	/// </summary>
	public class OptionsMenuCommand : MenuCommand, IClicker
	{
		MainFormMediator _mediator;

		public OptionsMenuCommand()
		{
			this.Description = "MenuCommand";
			this.Text = "&Options";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void ExecuteClick()
		{
			_mediator.OptionsClicked();
		}
	}
}