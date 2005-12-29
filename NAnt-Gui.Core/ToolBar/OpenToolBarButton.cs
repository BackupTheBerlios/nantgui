using System.Windows.Forms;

namespace NAntGui.Core.ToolBar
{
	/// <summary>
	/// Summary description for OpenToolBarButton.
	/// </summary>
	public class OpenToolBarButton : ToolBarButton, Command
	{
		private MainFormMediator _mediator;

		public OpenToolBarButton()
		{
			this.ImageIndex = 5;
			this.ToolTipText = "Open Build File";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.OpenClicked();
		}
	}
}
