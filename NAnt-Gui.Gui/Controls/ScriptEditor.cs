#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2007 Colin Svingen
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General internal License as published by
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
// Colin Svingen (swoogan@gmail.com)

#endregion

using System;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.XmlEditor;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.SharpDevelop.DefaultEditor;
using NAntGui.Framework;

namespace NAntGui.Gui.Controls
{
	/// <summary>
	/// Summary description for ScriptEditor.
	/// </summary>
    public partial class ScriptEditor : IEditCommands
	{
		public ScriptEditor()
		{
			InitializeComponent();
		}
		
		void CutToolStripMenuItemClick(object sender, System.EventArgs e)
		{
			this.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);			
		}
		
		void CopyToolStripMenuItemClick(object sender, System.EventArgs e)
		{
			this.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);
		}
		
		void PasteToolStripMenuItemClick(object sender, System.EventArgs e)
		{
			this.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);
		}
		
		void DeleteToolStripMenuItemClick(object sender, System.EventArgs e)
		{
			this.ActiveTextAreaControl.TextArea.ClipboardHandler.Delete(sender, e);
		}
		
		void SelectAllToolStripMenuItemClick(object sender, System.EventArgs e)
		{
			this.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);
		}

        public void Cut()
		{
			CutToolStripMenuItemClick(this, EventArgs.Empty);
		}

        public void Copy()
		{
			CopyToolStripMenuItemClick(this, EventArgs.Empty);
		}

        public void Paste()
		{
			PasteToolStripMenuItemClick(this, EventArgs.Empty);
		}

        public void Delete()
		{
			DeleteToolStripMenuItemClick(this, EventArgs.Empty);
		}

        public void SelectAll()
		{
			SelectAllToolStripMenuItemClick(this, EventArgs.Empty);
		}		
	}
}
