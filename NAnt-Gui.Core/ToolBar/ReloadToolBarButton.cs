using System.Windows.Forms;

namespace NAntGui.Core.ToolBar
{
	/// <summary>
	/// Summary description for ReloadToolBarButton.
	/// </summary>
	public class ReloadToolBarButton : ToolBarButton, IClicker
	{
		private MainFormMediator _mediator;

		public ReloadToolBarButton()
		{
			this.ImageIndex = 4;
			this.ToolTipText = "Reload Build File";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void ExecuteClick()
		{
			_mediator.ReloadClicked();
		}
	}
}
