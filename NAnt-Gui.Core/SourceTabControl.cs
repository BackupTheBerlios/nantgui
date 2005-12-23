using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TabControl = Crownwood.Magic.Controls.TabControl;

namespace NAntGui.Core
{
	public class SourceTabControl
	{
		private ArrayList _tabs = new ArrayList();
		private TabControl _tabControl = new TabControl();

		public SourceTabControl()
		{
			this.Initialize();
		}

		#region Initialize

		private void Initialize()
		{
			_tabControl.SuspendLayout();

			// 
			// TabControl
			// 
			_tabControl.Appearance = TabControl.VisualAppearance.MultiDocument;
			_tabControl.Dock = DockStyle.Fill;
			_tabControl.IDEPixelArea = true;
			_tabControl.Location = new Point(0, 53);
			_tabControl.SelectedIndex = 0;
			_tabControl.Size = new Size(824, 478);
			_tabControl.IDEPixelBorder = false;
			_tabControl.Name = "SourceTabs";

			_tabControl.ClosePressed += new EventHandler(this.Close_Pressed);

			_tabControl.ResumeLayout(false);
		}

		#endregion

		public void AddTab(ScriptTabPage tab)
		{
			Assert.NotNull(tab, "tab");
			_tabs.Add(tab);
			_tabControl.TabPages.Add(tab.ScriptTab);
			//_tabControl.SelectedTab = tab.ScriptTab;
		}

		public ScriptTabPage SelectedTab
		{
			get
			{
				foreach (ScriptTabPage page in _tabs)
					if (page.ScriptTab == _tabControl.SelectedTab) return page;

				return null;
			}
		}

		public void Close_Pressed(object sender, EventArgs e)
		{
//			this.TabPages.Remove(this.SelectedTab);
		}

		public TabControl Tabs
		{
			get { return _tabControl; }
		}

		public void Clear()
		{
			_tabControl.TabPages.Clear();
		}

		public void CloseTabs(CancelEventArgs e)
		{
			foreach (ScriptTabPage page in _tabs)
			{
				if (page.IsDirty)
				{
					DialogResult result =
						MessageBox.Show("You have unsaved changes to " + page.File.Name + ".  Save?", "Save Changes?",
						MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

					if (result == DialogResult.Yes)
					{
						page.Save();
					}
					else if (result == DialogResult.Cancel)
					{
						e.Cancel = true;
					}
				}
			}
		}
	}
}