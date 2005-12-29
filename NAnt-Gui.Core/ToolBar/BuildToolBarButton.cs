using System.Windows.Forms;

namespace NAntGui.Core.ToolBar
{
	/// <summary>
	/// Summary description for BuildToolBarButton.
	/// </summary>
	public class BuildToolBarButton : ToolBarButton, Command
	{
		private MainFormMediator _mediator;

		public BuildToolBarButton()
		{
			this.ImageIndex = 7;
			this.ToolTipText = "Build Default Target";
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}

		public void Execute()
		{
			_mediator.BuildClicked();
		}
	}
}
