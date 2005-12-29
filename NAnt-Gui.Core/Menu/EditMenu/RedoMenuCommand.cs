using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.EditMenu
{
	/// <summary>
	/// Summary description for RedoMenuCommand.
	/// </summary>
	public class RedoMenuCommand : MenuCommand, IClicker
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

		public void ExecuteClick()
		{
			_mediator.RedoClicked();
		}
	}
}