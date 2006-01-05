using System.Windows.Forms;

namespace NAntGui.Core.Controls.ToolBar
{
	/// <summary>
	/// Summary description for SaveToolBarButton.
	/// </summary>
	public class SaveToolBarButton : ToolBarButton, IClicker
	{
		private MainFormMediator _mediator;

		public SaveToolBarButton()
		{
			this.ImageIndex = 2;
			this.ToolTipText = "Save Build File";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void ExecuteClick()
		{
			_mediator.SaveClicked();
		}
	}
}
