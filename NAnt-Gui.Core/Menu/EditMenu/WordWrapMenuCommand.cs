using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.EditMenu
{
	/// <summary>
	/// Summary description for WordWrapMenuCommand.
	/// </summary>
	public class WordWrapMenuCommand : MenuCommand, Command
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

		public void Execute()
		{
			_mediator.WordWrapClicked();
		}
	}
}