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
using Crownwood.Magic.Common;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for ToolBarControl.
	/// </summary>
	public class ToolBarControl : ToolBar
	{
		public event VoidVoid New_Click;
		public event VoidVoid Build_Click;
		public event VoidVoid Open_Click;
		public event EventHandler Save_Click;
		public event EventHandler Reload_Click;
		public event VoidVoid Stop_Click;

		private ToolBarButton NewToolBarButton		= new ToolBarButton();
		private ToolBarButton OpenToolBarButton		= new ToolBarButton();
		private ToolBarButton SaveToolBarButton		= new ToolBarButton();
		private ToolBarButton BuildToolBarButton	= new ToolBarButton();
		private ToolBarButton ReloadToolBarButton	= new ToolBarButton();
		private ToolBarButton StopToolBarButton		= new ToolBarButton();

		public ToolBarControl()
		{
			this.SetImageList();
			this.SetProperties();
		}

		private void SetImageList()
		{
			this.ImageList = ResourceHelper.LoadBitmapStrip(
				this.GetType(), "NAntGui.Core.Images.MenuItems.bmp",
				new Size(16, 16), new Point(0, 0));
		}

		private void SetProperties()
		{
			// 
			// MainToolBar
			// 
			this.Appearance = ToolBarAppearance.Flat;
			this.Buttons.AddRange(new ToolBarButton[]
				{
					this.NewToolBarButton,
					this.OpenToolBarButton,
					this.SaveToolBarButton,
					this.ReloadToolBarButton,
					this.BuildToolBarButton,
					this.StopToolBarButton
				});
			this.DropDownArrows = true;
			this.Location = new Point(0, 25);
			this.Name = "MainToolBar";
			this.ShowToolTips = true;
			this.Size = new Size(824, 28);
			this.TabIndex = 4;
			this.ButtonClick += new ToolBarButtonClickEventHandler(this.Button_Click);
			// 
			// NewToolBarButton
			// 
			this.NewToolBarButton.ImageIndex = 8;
			this.NewToolBarButton.ToolTipText = "Not built yet";
			this.NewToolBarButton.Enabled = false;
			// 
			// OpenToolBarButton
			// 
			this.OpenToolBarButton.ImageIndex = 5;
			this.OpenToolBarButton.ToolTipText = "Open Build File";
			// 
			// SaveToolBarButton
			// 
			this.SaveToolBarButton.ImageIndex = 2;
			this.SaveToolBarButton.ToolTipText = "Save Build File";
			// 
			// ReloadToolBarButton
			// 
			this.ReloadToolBarButton.ImageIndex = 4;
			this.ReloadToolBarButton.ToolTipText = "Reload Build File";
			// 
			// BuildToolBarButton
			// 
			this.BuildToolBarButton.ImageIndex = 7;
			this.BuildToolBarButton.ToolTipText = "Build Default Target";
			// 
			// StopToolBarButton
			// 
			this.StopToolBarButton.ImageIndex = 3;
			this.StopToolBarButton.ToolTipText = "Abort the Current Build";
		}

		private void Button_Click(object sender, ToolBarButtonClickEventArgs e)
		{
			if (e.Button == this.NewToolBarButton && this.New_Click != null)
			{
				this.New_Click();
			}
			else if (e.Button == this.BuildToolBarButton && this.Build_Click != null)
			{
				this.Build_Click();
			}
			else if (e.Button == this.OpenToolBarButton && this.Open_Click != null)
			{
				this.Open_Click();
			}
			else if (e.Button == this.SaveToolBarButton && this.Save_Click != null)
			{
				this.Save_Click(this, new EventArgs());
			}
			else if (e.Button == this.ReloadToolBarButton && this.Reload_Click != null)
			{
				this.Reload_Click(this, new EventArgs());
			}
			else if (e.Button == this.StopToolBarButton && this.Stop_Click != null)
			{
				this.Stop_Click();
			}
		}

		public void Disable()
		{
			this.ReloadToolBarButton.Enabled = false;
			this.SaveToolBarButton.Enabled = false;
			this.StopToolBarButton.Enabled = false;
			this.BuildToolBarButton.Enabled = false;
		}

		public void Enable()
		{
			this.ReloadToolBarButton.Enabled = true;
			this.SaveToolBarButton.Enabled = true;
			this.StopToolBarButton.Enabled = true;
			this.BuildToolBarButton.Enabled = true;
		}
	}
}