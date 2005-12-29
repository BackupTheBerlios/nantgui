using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.FileMenu
{
	/// <summary>
	/// Summary description for OpenMenuCommand.
	/// </summary>
	public class OpenMenuCommand : MenuCommand, IClicker
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

		public void ExecuteClick()
		{
			_mediator.OpenClicked();
		}
	}
}