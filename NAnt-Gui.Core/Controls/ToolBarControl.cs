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

using System.Drawing;
using System.Windows.Forms;

namespace NAntGui.Core.Controls
{
	/// <summary>
	/// Summary description for ToolBarControl.
	/// </summary>
	public class ToolBarControl : System.Windows.Forms.ToolBar
	{
		private MainFormMediator _mediator;

		private ToolBarButton _newButton = new ToolBarButton();
		private ToolBarButton _openButton = new ToolBarButton();
		private ToolBarButton _saveButton = new ToolBarButton();
		private ToolBarButton _reloadButton = new ToolBarButton();
		private ToolBarButton _runButton = new ToolBarButton();
		private ToolBarButton _stopButton = new ToolBarButton();

		public ToolBarControl(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "mediator");
			_mediator = mediator;

			Initialize();
		}

		#region Initialize

		private void Initialize()
		{
			//
			// _saveButton
			//
			_saveButton.ImageIndex = 2;
			_saveButton.ToolTipText = "Save Build File";
			//
			// _reloadButton
			//
			_reloadButton.ImageIndex = 4;
			_reloadButton.ToolTipText = "Reload Build File";
			//
			// _openButton
			//
			_openButton.ImageIndex = 5;
			_openButton.ToolTipText = "Open Build File";
			//
			// _newButton
			//
			_newButton.ImageIndex = 8;
			_newButton.ToolTipText = "Not built yet";
			//
			// _reloadButton
			//
			_runButton.ImageIndex = 7;
			_runButton.ToolTipText = "Build Default Target";
			//
			// _reloadButton
			//
			_stopButton.ImageIndex = 3;
			_stopButton.ToolTipText = "Abort the Current Build";
			_stopButton.Enabled = false;
			//
			// MainToolBar
			//
			Appearance = ToolBarAppearance.Flat;
			Buttons.AddRange(new ToolBarButton[]
			                 	{
			                 		_newButton,
			                 		_openButton,
			                 		_saveButton,
			                 		_reloadButton,
			                 		_runButton,
			                 		_stopButton
			                 	});
			DropDownArrows = true;
			Location = new Point(0, 25);
			Name = "MainToolBar";
			ShowToolTips = true;
			Size = new Size(824, 28);
			TabIndex = 4;
			ImageList = NAntGuiApp.ImageList;
			ButtonClick += new ToolBarButtonClickEventHandler(Button_Click);
		}

		#endregion

		private void Button_Click(object sender, ToolBarButtonClickEventArgs e)
		{
			if (e.Button == _newButton)
			{
				_mediator.NewClicked();
			}
			else if (e.Button == _runButton)
			{
				_mediator.RunClicked();
			}
			else if (e.Button == _openButton)
			{
				_mediator.OpenClicked();
			}
			else if (e.Button == _saveButton)
			{
				_mediator.SaveClicked();
			}
			else if (e.Button == _reloadButton)
			{
				_mediator.ReloadClicked();
			}
			else if (e.Button == _stopButton)
			{
				_mediator.StopClicked();
			}
		}

		public void Disable()
		{
			_reloadButton.Enabled = false;
//			_saveButton.Enabled = false;
			_stopButton.Enabled = false;
			_runButton.Enabled = false;
		}

		public void Enable()
		{
			_reloadButton.Enabled = true;
//			_saveButton.Enabled = true;
			_runButton.Enabled = true;
		}

		public RunState State
		{
			set
			{
				switch (value)
				{
					case RunState.Running:
						_runButton.Enabled = false;
						_stopButton.Enabled = true;
						break;
					case RunState.Stopped:
						_runButton.Enabled = true;
						_stopButton.Enabled = false;
						break;
				}
			}
		}
	}
}