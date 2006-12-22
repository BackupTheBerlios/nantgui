#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2007 Colin Svingen
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
using NAntGui.Framework;
using NAntGui.Gui.Controls;

namespace NAntGui.Gui
{
	public class ScriptTabs
	{
		private ArrayList _tabs = new ArrayList();
		private MainTabControl _tabControl;		
		private MainFormMediator _mediator;

		public ScriptTabs(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "mediator");
			_mediator	= mediator;
			_tabControl = new MainTabControl(mediator);			
			
			_tabControl.SelectionChanged += new EventHandler(TabIndex_Changed);
			_tabControl.ClosePressed += new EventHandler(Close_Pressed);
		}

		public void AddTab(ScriptTabPage tab)
		{
			Assert.NotNull(tab, "tab");
			_tabs.Add(tab);
			_tabControl.TabPages.Add(tab.ScriptTab);
			tab.Focus();
		}

		/// <summary>
		/// Called when the application is closing and
		/// any open tabs might need to be saved.
		/// </summary>
		/// <param name="e">
		/// Passed by the closing event.  Used to cancel 
		/// the close if the user made a mistake.
		/// </param>
		public void CloseTabs(CancelEventArgs e)
		{
			object[] pages = _tabs.ToArray();
            
			foreach (ScriptTabPage page in pages)
			{
				CloseTab(page, e);
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

		/// <summary>
		/// Called with the Close File menu item is clicked.
		/// </summary>
		/// <param name="e">
		/// Passed in to cancel the close if the user made 
		/// a mistake (when the file isn't saved.
		/// </param>
//		public void CloseSelectedTab(CancelEventArgs e)
//		{
//			CloseTab(_selectedTab, e);
//		}

		public void CloseTab(ScriptTabPage tab, CancelEventArgs e)
		{
			tab.Close(e);
			_tabControl.TabPages.Remove(tab.ScriptTab);
			_tabs.Remove(tab);
		}

		private void Close_Pressed(object sender, EventArgs e)
		{
			_mediator.CloseClicked();
		}

		private void TabIndex_Changed(object sender, EventArgs e)
		{
			foreach (ScriptTabPage page in _tabs)
			{
				if (page.ScriptTab == _tabControl.SelectedTab)
				{
					_mediator.TabIndexChanged(page);
					break;
				}
			}
		}

		#region Properties

//		public ScriptTabPage SelectedTab
//		{
//			get { return _selectedTab; }
//		}

		public int TabCount
		{
			get { return _tabs.Count; }
		}

		#endregion
	}
}