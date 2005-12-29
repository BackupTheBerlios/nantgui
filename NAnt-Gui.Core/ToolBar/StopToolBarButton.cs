using System.Windows.Forms;

namespace NAntGui.Core.ToolBar
{
	/// <summary>
	/// Summary description for StopToolBarButton.
	/// </summary>
	public class StopToolBarButton : ToolBarButton, Command
	{
		private MainFormMediator _mediator;

		public StopToolBarButton()
		{
			this.ImageIndex = 3;
			this.ToolTipText = "Abort the Current Build";
			this.Enabled = false;
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.StopClicked();
		}
	}
}
