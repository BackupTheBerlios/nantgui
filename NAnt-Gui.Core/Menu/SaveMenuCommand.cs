using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu
{
	/// <summary>
	/// Summary description for SaveMenuCommand.
	/// </summary>
	public class SaveMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public SaveMenuCommand()
		{
			this.Description = "MenuCommand";
			this.ImageIndex = 2;
			this.Shortcut = Shortcut.CtrlS;
			this.Text = "&Save";
			this.ImageList = NAntGuiApp.ImageList;
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.SaveClicked();
		}
	}
}