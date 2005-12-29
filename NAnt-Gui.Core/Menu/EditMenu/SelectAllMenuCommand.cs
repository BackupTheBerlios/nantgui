using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.EditMenu
{
	/// <summary>
	/// Summary description for SelectAllMenuCommand.
	/// </summary>
	public class SelectAllMenuCommand : MenuCommand, IClicker
	{
		MainFormMediator _mediator;

		public SelectAllMenuCommand()
		{
			this.Description = "MenuCommand";
			this.Shortcut = Shortcut.CtrlA;
			this.Text = "Select &All";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void ExecuteClick()
		{
			_mediator.SelectAllClicked();
		}
	}
}