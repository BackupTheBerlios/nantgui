using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu
{
	/// <summary>
	/// Summary description for UndoMenuCommand.
	/// </summary>
	public class UndoMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public UndoMenuCommand()
		{
			this.Description = "MenuCommand";
			this.Shortcut = Shortcut.CtrlZ;
			this.Text = "&Undo";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.UndoClicked();
		}
	}
}