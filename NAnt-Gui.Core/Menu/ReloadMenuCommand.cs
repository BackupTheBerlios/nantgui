using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu
{
	/// <summary>
	/// Summary description for ReloadMenuCommand.
	/// </summary>
	public class ReloadMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public ReloadMenuCommand()
		{
			this.Description = "MenuCommand";
			this.ImageIndex = 4;
			this.Shortcut = Shortcut.CtrlR;
			this.Text = "&Reload";
			this.ImageList = NAntGuiApp.ImageList;
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.ReloadClicked();
		}
	}
}