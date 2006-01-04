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
		public event EventHandler NewClick;
		public event EventHandler BuildClick;
		public event EventHandler OpenClick;
		public event EventHandler SaveClick;
		public event EventHandler ReloadClick;
		public event EventHandler StopClick;

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

		public MainFormMediator Mediator
		{
			set
			{
				_newButton.Mediator = value;
				_openButton.Mediator = value;
				_saveButton.Mediator = value;
				_reloadButton.Mediator = value;
				_buildButton.Mediator = value;
				_stopButton.Mediator = value;
			}
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
			if (e.Button == _newButton && this.NewClick != null)
			{
				this.NewClick(_newButton, new EventArgs());
			}
			else if (e.Button == _buildButton && this.BuildClick != null)
			{
				this.BuildClick(_buildButton, new EventArgs());
			}
			else if (e.Button == _openButton && this.OpenClick != null)
			{
				this.OpenClick(_openButton, new EventArgs());
			}
			else if (e.Button == _saveButton && this.SaveClick != null)
			{
				this.SaveClick(_saveButton, new EventArgs());
			}
			else if (e.Button == _reloadButton && this.ReloadClick != null)
			{
				this.ReloadClick(_reloadButton, new EventArgs());
			}
			else if (e.Button == _stopButton && this.StopClick != null)
			{
				this.StopClick(_stopButton, new EventArgs());
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

		public RunState State
		{
			set 
			{ 
				switch (value)
				{
					case RunState.Running:
						_buildButton.Enabled = false;
						_stopButton.Enabled = true;
						break;
					case RunState.Stopped:
						_buildButton.Enabled = true;
						_stopButton.Enabled = false;
						break;
				}
			}
		}
	}
}