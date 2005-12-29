using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu
{
	/// <summary>
	/// Summary description for OpenMenuCommand.
	/// </summary>
	public class OpenMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public OpenMenuCommand()
		{
			this.Description = "MenuCommand";
			this.ImageIndex = 5;
			this.Shortcut = Shortcut.CtrlO;
			this.Text = "&Open";
			this.ImageList = NAntGuiApp.ImageList;
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.OpenClicked();
		}
	}
}