using System.Drawing;
using System.Windows.Forms;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for MainStatusBar.
	/// </summary>
	public class MainStatusBar : StatusBar
	{
		private StatusBarPanel _progressPanel = new StatusBarPanel();
		private StatusBarPanel _fullFilePanel = new StatusBarPanel();
		private StatusBarPanel _filePanel = new StatusBarPanel();

		public MainStatusBar()
		{
			this.Location = new Point(0, 531);
			this.Name = "MainStatusBar";
			this.Panels.AddRange(new StatusBarPanel[]
				{
					_filePanel,
					_fullFilePanel,
					_progressPanel
				});
			this.ShowPanels = true;
			this.Size = new Size(824, 22);
			this.SizingGrip = false;
			this.TabIndex = 2;
			// 
			// FileStatusBarPanel
			// 
			_filePanel.AutoSize = StatusBarPanelAutoSize.Contents;
			_filePanel.Width = 10;
			// 
			// FullFileStatusBarPanel
			// 
			_fullFilePanel.AutoSize = StatusBarPanelAutoSize.Spring;
			_fullFilePanel.Width = 804;
			// 
			// ProgressPanel
			// 
			_progressPanel.MinWidth = 0;
			_progressPanel.Style = StatusBarPanelStyle.OwnerDraw;
			_progressPanel.Width = 10;
		}
	}
}
