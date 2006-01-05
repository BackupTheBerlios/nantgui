using System.Windows.Forms;

namespace NAntGui.Core.Controls.ToolBar
{
	/// <summary>
	/// Summary description for OpenToolBarButton.
	/// </summary>
	public class OpenToolBarButton : ToolBarButton, IClicker
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

		public void ExecuteClick()
		{
			_mediator.OpenClicked();
		}
	}
}
