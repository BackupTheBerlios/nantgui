using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu
{
	/// <summary>
	/// Summary description for SaveAsMenuCommand.
	/// </summary>
	public class SaveAsMenuCommand : MenuCommand, Command
	{
		MainFormMediator _mediator;

		public SaveAsMenuCommand()
		{
			this.Description = "MenuCommand";
			this.ImageIndex = 2;
			this.Shortcut = Shortcut.F12;
			this.Text = "Save &As";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.SaveAsClicked();
		}
	}
}