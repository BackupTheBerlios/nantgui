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
// Colin Svingen (swoogan@gmail.com)

#endregion

using System;
using System.IO;
using System.Windows.Forms;
using ICSharpCode.XmlEditor;
using NAntGui.Core;

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
#if DEBUG
            const string fileName = @"..\..\..\Tools\nant-0.85\schema\nant.xsd";
#else
            const string fileName = @"..\Data\nant.xsd";
#endif
            string path = Path.Combine(Utils.RunDirectory, fileName);
            if (File.Exists(path))
            {
                try
                {
                    XmlSchemaCompletionData data = new XmlSchemaCompletionData(path);
                    DefaultSchemaCompletionData = data;
                    SchemaCompletionDataItems = new XmlSchemaCompletionDataCollection(new[] { data });
                    CodeCompletionPopupCommand command = new CodeCompletionPopupCommand();
                    editactions.Add(Keys.Space | Keys.Control, command);
                }
                catch (Exception)
                {
                }
            }
        }

        public void UpdateFolding()
        {
            Document.FoldingManager.UpdateFoldings(String.Empty, null);
        }

        #region IEditCommands Members

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

        #endregion

        private void CutToolStripMenuItemClick(object sender, EventArgs e)
        {
            ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e);
        }

        private void CopyToolStripMenuItemClick(object sender, EventArgs e)
        {
            ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e);
        }

        private void PasteToolStripMenuItemClick(object sender, EventArgs e)
        {
            ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e);
        }

        private void DeleteToolStripMenuItemClick(object sender, EventArgs e)
        {
            ActiveTextAreaControl.TextArea.ClipboardHandler.Delete(sender, e);
        }

        private void SelectAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(sender, e);
        }
    }
}