using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.FileMenu
{
	/// <summary>
	/// Summary description for SaveOutputMenuCommand.
	/// </summary>
	public class SaveOutputMenuCommand : MenuCommand, IClicker
	{
		MainFormMediator _mediator;

		public SaveOutputMenuCommand()
		{
			this.Description = "MenuCommand";
			this.Shortcut = Shortcut.CtrlU;
			this.Text = "SaveO&utput";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void ExecuteClick()
		{
			_mediator.SaveOutputClicked();
		}
	}
}