using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.EditMenu
{
	/// <summary>
	/// Summary description for WordWrapMenuCommand.
	/// </summary>
	public class WordWrapMenuCommand : MenuCommand, IClicker
	{
		MainFormMediator _mediator;

		public WordWrapMenuCommand()
		{
			this.Description = "MenuCommand";
			this.Text = "&Word Wrap";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void ExecuteClick()
		{
			_mediator.WordWrapClicked();
		}
	}
}