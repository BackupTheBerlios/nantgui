using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu
{
	/// <summary>
	/// Summary description for CopyMenuCommand.
	/// </summary>
	public class CopyMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public CopyMenuCommand()
		{
			this.Description = "MenuCommand";
			this.Shortcut = Shortcut.CtrlC;
			this.Text = "Cop&y";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.CopyClicked();
		}
	}
}