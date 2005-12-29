using System.Windows.Forms;

namespace NAntGui.Core.ToolBar
{
	/// <summary>
	/// Summary description for NewToolBarButton.
	/// </summary>
	public class NewToolBarButton : ToolBarButton, Command
	{
		private MainFormMediator _mediator;

		public NewToolBarButton()
		{
			this.ImageIndex = 8;
			this.ToolTipText = "Not built yet";
			this.Enabled = false;
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.NewClicked();
		}
	}
}
