using System;
using System.Drawing;
using System.Windows.Forms;
using TabControl = Crownwood.Magic.Controls.TabControl;
using TabPage = Crownwood.Magic.Controls.TabPage;

namespace NAntGui.Core
{
	public class SourceTabControl : TabControl
	{
		public SourceTabControl()
		{
			this.Initialize();
		}

		#region Initialize

		private void Initialize()
		{
			this.SuspendLayout();

			// 
			// TabControl
			// 
			this.Appearance = VisualAppearance.MultiDocument;
			this.Dock = DockStyle.Fill;
			this.IDEPixelArea = true;
			this.Location = new Point(0, 53);
			this.SelectedIndex = 0;
			this.Size = new Size(824, 478);
			this.IDEPixelBorder = false;
			this.Name = "SourceTabs";

			this.ClosePressed += new EventHandler(this.Close_Pressed);

			this.ResumeLayout(false);
		}

		#endregion

		public void AddTab(TabPage tab)
		{
			Assert.NotNull(tab, "tab");
            this.TabPages.Add(tab);
			this.SelectedTab = tab;
		}

		public void Close_Pressed(object sender, EventArgs e)
		{
			this.TabPages.Remove(this.SelectedTab);
		}
	}
}