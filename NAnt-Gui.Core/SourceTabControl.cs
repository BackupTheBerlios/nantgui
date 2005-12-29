#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2005 Colin Svingen, Business Watch International
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Colin Svingen (nantgui@swoogan.com)

#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using TabControl = Crownwood.Magic.Controls.TabControl;

namespace NAntGui.Core
{
	public class SourceTabControl : IDragDropper
	{
		private ArrayList _tabs = new ArrayList();
		private TabControl _tabControl = new TabControl();
		private MainFormMediator _mediator;
		private DragEventHandler _dragHandler;
		private DragEventHandler _dropHandler;

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
			Assert.NotNull(_mediator, "_mediator");
			Assert.NotNull(_dragHandler, "_dragHandler");
			Assert.NotNull(_dropHandler, "_dropHandler");

			tab.Mediator	= _mediator;
			tab.DragEnter	= _dragHandler;
			tab.DragDrop	= _dropHandler;

			_tabs.Add(tab);
			tab.AddTabToControl(_tabControl.TabPages);
			//_tabControl.SelectedTab = tab.ScriptTab;
		}

		public ScriptTabPage SelectedTab
		{
			get
			{
				foreach (ScriptTabPage page in _tabs)
					if (page.Equals(_tabControl.SelectedTab)) return page;

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

		public void SetToDockManager(MainDockManager dockManager)
		{
			dockManager.AddTabControl(_tabControl);
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
			set
			{
				_tabControl.DragDrop += value;
				_dropHandler = value;
			}
		}

		public DragEventHandler DragEnter
		{
			set
			{
				_tabControl.DragEnter += value;
				_dragHandler = value;
			}
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}
	}
}