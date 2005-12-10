using System;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for ScriptEditor.
	/// </summary>
	public class ScriptEditor : TextEditorControl, IDisplayer, ISourceIO
	{
		private ScriptEditorContextMenu _xmlContextMenu;

		public ScriptEditor()
		{
			Initialize();
		}

		#region Initialize

		private void Initialize()
		{
			_xmlContextMenu = new ScriptEditorContextMenu(this.ActiveTextAreaControl.TextArea.ClipboardHandler);
	
			// 
			// ScriptEditor
			// 
			this.Anchor = (((((AnchorStyles.Top | AnchorStyles.Bottom)
				| AnchorStyles.Left)
				| AnchorStyles.Right)));
			this.Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((Byte) (0)));
			this.Location = new Point(0, 0);
			this.Name = "XMLRichTextBox";
			this.Size = new Size(824, 454);
			this.TabIndex = 4;
			this.Text = "";
			this.ShowVRuler = false;
			this.ShowEOLMarkers = false;
			this.ShowSpaces = false;
			this.ShowTabs = false;
			this.EnableFolding = true;
			this.SetHighlighting("XML");
			this.ActiveTextAreaControl.TextArea.MouseUp += new MouseEventHandler(this.PopupMenu);
		}

		#endregion

		private void PopupMenu(object sender, MouseEventArgs e)
		{
			if (sender is TextArea && e.Button == MouseButtons.Right)
			{
				TextArea area = sender as TextArea;
				_xmlContextMenu.TrackPopup(area.PointToScreen(new Point(e.X, e.Y)));
			}
		}
	}
}
