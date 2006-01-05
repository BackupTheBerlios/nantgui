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

namespace NAntGui.Core
{
	public class ScriptTabs
	{
		private ArrayList _tabs = new ArrayList();
		private MainTabControl _tabControl = new MainTabControl();
		private MainFormMediator _mediator;

		public ScriptTabs()
		{
			_tabControl.ClosePressed += new EventHandler(this.Close_Pressed);
		}

		public void AddTab(ScriptTabPage tab)
		{
			Assert.NotNull(tab, "tab");
			Assert.NotNull(_mediator, "_mediator");

			tab.Mediator	= _mediator;

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

		public void CloseSelectedTab()
		{
			this.SelectedTab.CloseFile();
		}

		public MainFormMediator Mediator
		{
			set
			{
				_mediator = value;
				_tabControl.Mediator = value;
			}
		}
	}
}