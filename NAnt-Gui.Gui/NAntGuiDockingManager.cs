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
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;

namespace NAntGui.Gui
{
	/// <summary>
	/// Summary description for NAntGuiDockingManager.
	/// </summary>
	public class NAntGuiDockingManager : DockingManager
	{
		public event EventHandler InvertAutoHide;
		
		public NAntGuiDockingManager(ContainerControl container, VisualStyle vs) : base(container, vs) {}
		
		protected override void OnInvertAutoHide(object sender, EventArgs e)
		{
			base.OnInvertAutoHide(sender, e);
			
			if (InvertAutoHide != null)
			{
				InvertAutoHide(sender, e);
			}
		}
		
		public override bool ShowContent(Content c)
		{
			if (InvertAutoHide != null)
			{
				InvertAutoHide(this, EventArgs.Empty);
			}
			
			return base.ShowContent(c);
		}
	}
}
