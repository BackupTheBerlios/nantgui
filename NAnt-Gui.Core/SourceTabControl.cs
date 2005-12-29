using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Crownwood.Magic.Docking;
using TabControl = Crownwood.Magic.Controls.TabControl;

namespace NAntGui.Core
{
	public class SourceTabControl : IDragDropper
	{
		private ArrayList _tabs = new ArrayList();
		private TabControl _tabControl = new TabControl();
		private MainFormMediator _mediator;

		public SourceTabControl()
		{
			this.Initialize();
		}

		#region Initialize

		private void Initialize()
		{
			_tabControl.SuspendLayout();

			_tabControl.Appearance = TabControl.VisualAppearance.MultiDocument;
			_tabControl.Dock = DockStyle.Fill;
			_tabControl.IDEPixelArea = true;
			_tabControl.IDEPixelBorder = false;
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
						MessageBox.Show("You have unsaved changes to " + page.FileName + ".  Save?", "Save Changes?",
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

		public void SetToDockManager(DockingManager dockManager)
		{
			dockManager.InnerControl = _tabControl;
		}

		public void AddTabsToControls(Control.ControlCollection controls)
		{
			controls.Add(_tabControl);
		}

		public void ExecuteDragEnter(DragEventArgs e)
		{
			_mediator.DragEnter(e);
		}

		public void ExecuteDragDrop(DragEventArgs e)
		{
			_mediator.DragDrop(e);
		}

		public void CloseSelectedTab()
		{
			this.SelectedTab.CloseFile();
		}

		public DragEventHandler DragDrop
		{
			set { _tabControl.DragDrop += value; }
		}

		public DragEventHandler DragEnter
		{
			set { _tabControl.DragEnter += value; }
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}
	}
}