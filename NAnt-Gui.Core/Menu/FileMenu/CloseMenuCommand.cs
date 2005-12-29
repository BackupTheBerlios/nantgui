using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.FileMenu
{
	/// <summary>
	/// Summary description for CloseMenuCommand.
	/// </summary>
	public class CloseMenuCommand : MenuCommand, IClicker
	{
		MainFormMediator _mediator;

		public CloseMenuCommand()
		{
			this.Description = "MenuCommand";
			this.Shortcut = Shortcut.CtrlW;
			this.Text = "&Close";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void ExecuteClick()
		{
			_mediator.CloseClicked();
		}
	}
}