using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.EditMenu
{
	/// <summary>
	/// Summary description for UndoMenuCommand.
	/// </summary>
	public class UndoMenuCommand : MenuCommand, IClicker
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

		public void ExecuteClick()
		{
			_mediator.UndoClicked();
		}
	}
}