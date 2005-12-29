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
using System.Drawing;
using System.Windows.Forms;

namespace NAntGui.Core.ToolBar
{
	/// <summary>
	/// Summary description for ToolBarControl.
	/// </summary>
	public class ToolBarControl : System.Windows.Forms.ToolBar
	{
		public event EventHandler New_Click;
		public event EventHandler Build_Click;
		public event EventHandler Open_Click;
		public event EventHandler Save_Click;
		public event EventHandler Reload_Click;
		public event EventHandler Stop_Click;

		private NewToolBarButton _newButton			= new NewToolBarButton();
		private OpenToolBarButton _openButton		= new OpenToolBarButton();
		private SaveToolBarButton _saveButton		= new SaveToolBarButton();
		private ReloadToolBarButton _reloadButton	= new ReloadToolBarButton();
		private BuildToolBarButton _buildButton		= new BuildToolBarButton();
		private StopToolBarButton _stopButton		= new StopToolBarButton();

		public ToolBarControl()
		{
			this.Initialize();
		}

		public void SetMediator(MainFormMediator mediator)
		{
			_newButton.Mediator		= mediator;
			_openButton.Mediator	= mediator;
			_saveButton.Mediator	= mediator;
			_reloadButton.Mediator	= mediator;
			_buildButton.Mediator	= mediator;
			_stopButton.Mediator	= mediator;
		}

		private void Initialize()
		{
			// 
			// MainToolBar
			// 
			this.Appearance = ToolBarAppearance.Flat;
			this.Buttons.AddRange(new ToolBarButton[]
				{
					_newButton,
					_openButton,
					_saveButton,
					_reloadButton,
					_buildButton,
					_stopButton
				});
			this.DropDownArrows = true;
			this.Location = new Point(0, 25);
			this.Name = "MainToolBar";
			this.ShowToolTips = true;
			this.Size = new Size(824, 28);
			this.TabIndex = 4;
			this.ImageList = NAntGuiApp.ImageList;
			this.ButtonClick += new ToolBarButtonClickEventHandler(this.Button_Click);
		}

		private void Button_Click(object sender, ToolBarButtonClickEventArgs e)
		{
			if (e.Button == _newButton && this.New_Click != null)
			{
				this.New_Click(_newButton, new EventArgs());
			}
			else if (e.Button == _buildButton && this.Build_Click != null)
			{
				this.Build_Click(_buildButton, new EventArgs());
			}
			else if (e.Button == _openButton && this.Open_Click != null)
			{
				this.Open_Click(_openButton, new EventArgs());
			}
			else if (e.Button == _saveButton && this.Save_Click != null)
			{
				this.Save_Click(_saveButton, new EventArgs());
			}
			else if (e.Button == _reloadButton && this.Reload_Click != null)
			{
				this.Reload_Click(_reloadButton, new EventArgs());
			}
			else if (e.Button == _stopButton && this.Stop_Click != null)
			{
				this.Stop_Click(_stopButton, new EventArgs());
			}
		}

		public void Disable()
		{
			_reloadButton.Enabled	= false;
			_saveButton.Enabled		= false;
			_stopButton.Enabled		= false;
			_buildButton.Enabled	= false;
		}

		public void Enable()
		{
			_reloadButton.Enabled	= true;
			_saveButton.Enabled		= true;
			_buildButton.Enabled	= true;
		}

		public void EnableStop()
		{
			_stopButton.Enabled = true;
		}

		public void DisableStop()
		{
			_stopButton.Enabled = false;
		}

		public void DisableRun()
		{
			_buildButton.Enabled = false;
		}

		public void EnableRun()
		{
			_buildButton.Enabled = true;
		}
	}
}