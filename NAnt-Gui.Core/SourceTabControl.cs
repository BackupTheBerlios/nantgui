using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Crownwood.Magic.Menus;
using TabControl = Crownwood.Magic.Controls.TabControl;
using TabPage = Crownwood.Magic.Controls.TabPage;
using ICSharpCode.TextEditor;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for SourceTabControl.
	/// </summary>
	public class SourceTabControl : TabControl
	{
		private string _source = "";

		private TabPage _sourceTabPage = new TabPage();
		private TextEditorControl _sourceTextEditor = new TextEditorControl();
		private PopupMenu _xmlContextMenu = new PopupMenu();
		private TextAreaClipboardHandler _clipboardHandler;

		public event VoidVoid SourceChanged;
		public event VoidVoid SourceRestored;

		public SourceTabControl()
		{
			this.Initialize();
			
			this.CreateSourceContextMenu();
		}

		#region Initialize

		private void Initialize()
		{
			_sourceTabPage.SuspendLayout();

			// 
			// TabControl
			// 
			this.Appearance = VisualAppearance.MultiDocument;
			this.Dock = DockStyle.Fill;
			this.IDEPixelArea = true;
			this.Location = new Point(0, 53);
			this.Name = "TabControl";
			this.SelectedIndex = 0;
			this.SelectedTab = _sourceTabPage;
			this.Size = new Size(824, 478);
			this.TabIndex = 12;
			this.TabPages.AddRange(new TabPage[]
				{
					_sourceTabPage
				});
			// 
			// XMLTabPage
			// 
			_sourceTabPage.Controls.Add(this._sourceTextEditor);
			_sourceTabPage.Location = new Point(0, 0);
			_sourceTabPage.Name = "_sourceTabPage";
			_sourceTabPage.Selected = false;
			_sourceTabPage.Size = new Size(824, 453);
			_sourceTabPage.TabIndex = 4;
			_sourceTabPage.Title = "Source";
			// 
			// _sourceRichTextBox
			// 
			_sourceTextEditor.Anchor = (((((AnchorStyles.Top | AnchorStyles.Bottom)
				| AnchorStyles.Left)
				| AnchorStyles.Right)));
			_sourceTextEditor.Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((Byte) (0)));
			_sourceTextEditor.Location = new Point(0, 0);
			_sourceTextEditor.Name = "XMLRichTextBox";
			_sourceTextEditor.Size = new Size(824, 454);
			_sourceTextEditor.TabIndex = 4;
			_sourceTextEditor.Text = "";
			_sourceTextEditor.ShowVRuler = false;
			_sourceTextEditor.ShowEOLMarkers = false;
			_sourceTextEditor.ShowSpaces = false;
			_sourceTextEditor.ShowTabs = false;
			_sourceTextEditor.EnableFolding = true;
			_sourceTextEditor.SetHighlighting("XML");
			_sourceTextEditor.TextChanged += new EventHandler(this.SourceRichTextBox_TextChanged);
			_sourceTextEditor.ActiveTextAreaControl.TextArea.MouseUp += new MouseEventHandler(this.SourceTextBox_MouseUp);
			
			// 
			// _clipboardHandler
			// 
			_clipboardHandler = _sourceTextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler;

			this.Appearance = TabControl.VisualAppearance.MultiDocument;
			this.Dock = DockStyle.Fill;
			this.IDEPixelArea = true;
			this.IDEPixelBorder = false;
			this.Location = new Point(0, 53);
			this.Name = "SourceTabs";
			this.SelectedIndex = 0;
			this.Size = new Size(824, 478);
			this.TabIndex = 12;

			this.ResumeLayout(false);
			_sourceTabPage.ResumeLayout(false);
		}



		private void CreateSourceContextMenu()
		{
			MenuCommand copy = new MenuCommand("Cop&y", new EventHandler(_clipboardHandler.Copy));
			MenuCommand cut = new MenuCommand("Cu&t", new EventHandler(_clipboardHandler.Cut));
			MenuCommand paste = new MenuCommand("&Paste", new EventHandler(_clipboardHandler.Paste));
			MenuCommand delete = new MenuCommand("&Delete", new EventHandler(_clipboardHandler.Delete));
			MenuCommand spacer = new MenuCommand("-");
			MenuCommand selectAll = new MenuCommand("Select &All", new EventHandler(_clipboardHandler.SelectAll));

			_xmlContextMenu.MenuCommands.AddRange(new MenuCommand[] {copy, cut, paste, delete, spacer, selectAll});
		}

		#endregion

		private void SourceTextBox_MouseUp(object sender, MouseEventArgs e)
		{
			if (sender is TextArea)
			{
				TextArea area = sender as TextArea;
				if (e.Button == MouseButtons.Right)
				{
					_xmlContextMenu.TrackPopup(area.PointToScreen(new Point(e.X, e.Y)));
				}
			}
		}

		private void SourceRichTextBox_TextChanged(object sender, EventArgs e)
		{
			if (this.SourceHasChanged)
			{
				if (this.SourceChanged != null) this.SourceChanged();
			}
			else
			{
				if (this.SourceRestored != null) this.SourceRestored();
			}
		}

		public string LoadSource(string buildFile)
		{
			Assert.NotNull(buildFile, "buildFile");
			using (TextReader reader = File.OpenText(buildFile))
			{
				_source = _sourceTextEditor.Text = reader.ReadToEnd();
			}

			return _source;
		}

		public void SaveSource(string buildFile)
		{
			Assert.NotNull(buildFile, "buildFile");
			using (TextWriter writer = File.CreateText(buildFile))
			{
				writer.Write(_sourceTextEditor.Text);
			}
		}

		public bool SourceHasChanged
		{
			get { return (_sourceTextEditor.Text != _source && 
				_sourceTextEditor.Text != _source.Replace("\r", "")); }
		}
	}
}