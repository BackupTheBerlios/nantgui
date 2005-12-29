using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.FileMenu
{
	/// <summary>
	/// Summary description for NewMenuCommand.
	/// </summary>
	public class NewMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public NewMenuCommand()
		{
			this.Description = "MenuCommand";
			this.ImageIndex = 8;
			this.Shortcut = Shortcut.CtrlN;
			this.Text = "&New";
			this.Enabled = false;
			this.ImageList = NAntGuiApp.ImageList;
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.NewClicked();
		}
	}
}