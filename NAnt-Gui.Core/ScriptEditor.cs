using System;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for ScriptEditor.
	/// </summary>
	public class ScriptEditor : TextEditorControl
	{
		private ScriptEditorContextMenu _sourceContextMenu;

		public ScriptEditor()
		{
			Initialize();
		}

		#region Initialize

		private void Initialize()
		{
			_sourceContextMenu = new ScriptEditorContextMenu(this.ActiveTextAreaControl.TextArea.ClipboardHandler);
	
			// 
			// ScriptEditor
			// 
			this.Dock = DockStyle.Fill;
			this.Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((Byte) (0)));
			this.ShowVRuler = false;
			this.ShowEOLMarkers = false;
			this.ShowSpaces = false;
			this.ShowTabs = false;
			this.EnableFolding = true;
			this.ShowMatchingBracket = true;
			this.ActiveTextAreaControl.TextArea.MouseUp += new MouseEventHandler(this.PopupMenu);
		}

		#endregion

		private void PopupMenu(object sender, MouseEventArgs e)
		{
			Assert.NotNull(sender, "sender");
			Assert.NotNull(e, "e");

			if (sender is TextArea && e.Button == MouseButtons.Right)
			{
				TextArea area = sender as TextArea;
				_sourceContextMenu.TrackPopup(area.PointToScreen(new Point(e.X, e.Y)));
			}
		}
	}
}
