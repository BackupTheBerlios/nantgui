using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.EditMenu
{
	/// <summary>
	/// Summary description for RedoMenuCommand.
	/// </summary>
	public class RedoMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public RedoMenuCommand()
		{
			this.Description = "MenuCommand";
			this.Shortcut = Shortcut.CtrlY;
			this.Text = "&Redo";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.RedoClicked();
		}
	}
}