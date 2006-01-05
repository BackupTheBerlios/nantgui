#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2005 Colin Svingen
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
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using NAntGui.Core.Controls;
using NAntGui.Core.Menu;
using NAntGui.Core.ToolBar;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : Form
	{
		public MainPropertyGrid _propertyGrid = new MainPropertyGrid();
		private MainMenuControl _mainMenu = new MainMenuControl();
		private ToolBarControl _mainToolBar = new ToolBarControl();
		private ScriptTabs _sourceTabs = new ScriptTabs();
		private MainStatusBar _mainStatusBar = new MainStatusBar();

		private MainFormMediator _mediator;

		private IContainer components;


		public MainForm()
		{
			this.Initialize();

			_mediator = new MainFormMediator(this, _sourceTabs,  
				 _propertyGrid, _mainStatusBar, _mainMenu, _mainToolBar);

			_mediator.LoadInitialBuildFile();
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Initialize

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void Initialize()
		{
			this.components = new Container();
			ResourceManager resources = new ResourceManager(typeof (MainForm));
			this.SuspendLayout();
			
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new Size(5, 13);
			this.ClientSize = new Size(824, 553);
			_sourceTabs.AddTabsToControls(this.Controls);
			this.Controls.Add(_mainStatusBar);
			this.Controls.Add(_mainToolBar);
			this.Controls.Add(_mainMenu);
			
			this.Icon = ((Icon) (resources.GetObject("$this.Icon")));
			this.MinimumSize = new Size(480, 344);
			this.Name = "MainForm";
			this.Text = "NAnt-Gui";
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.ResumeLayout(false);
		}

		#endregion


		public MainFormMediator Mediator
		{
			set { throw new NotImplementedException(); }
		}

		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);
			_mediator.DragEnter(e);
		}

		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);
			_mediator.DragDrop(e);
		}
	}
}
