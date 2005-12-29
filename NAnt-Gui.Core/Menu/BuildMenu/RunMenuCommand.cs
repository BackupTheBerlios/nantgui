using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.BuildMenu
{
	/// <summary>
	/// Summary description for RunMenuCommand.
	/// </summary>
	public class RunMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public RunMenuCommand()
		{
			this.Description = "Builds the current build file";
			this.ImageIndex = 7;
			this.Shortcut = Shortcut.F5;
			this.Text = "&Run";
			this.ImageList = NAntGuiApp.ImageList;
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.RunClicked();
		}
	}
}