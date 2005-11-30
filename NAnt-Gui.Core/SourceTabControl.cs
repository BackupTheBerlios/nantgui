using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Crownwood.Magic.Menus;
using TabControl = Crownwood.Magic.Controls.TabControl;
using TabPage = Crownwood.Magic.Controls.TabPage;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for SourceTabControl.
	/// </summary>
	public class SourceTabControl : TabControl
	{
		private string _source = "";

		private TabPage _sourceTabPage = new TabPage();
		private TabPage _outputTabPage = new TabPage();

		private RichTextBox _outputRichTextBox = new RichTextBox();
		private RichTextBox _sourceRichTextBox = new RichTextBox();

		private PopupMenu _outputContextMenu = new PopupMenu();
		private PopupMenu _xmlContextMenu = new PopupMenu();

		public event VoidVoid SourceChanged;
		public event VoidVoid SourceRestored;
		public event VoidBool WordWrapChanged;

		public SourceTabControl()
		{
			Initialize();
			this.CreateOutputContextMenu();
			this.CreateXmlContextMenu();
		}

		#region Initialize

		private void Initialize()
		{
			_sourceTabPage.SuspendLayout();
			_outputTabPage.SuspendLayout();

			// 
			// TabControl
			// 
			this.Appearance = VisualAppearance.MultiDocument;
			this.Dock = DockStyle.Fill;
			this.IDEPixelArea = true;
			this.Location = new Point(0, 53);
			this.Name = "TabControl";
			this.SelectedIndex = 0;
			this.SelectedTab = this._outputTabPage;
			this.Size = new Size(824, 478);
			this.TabIndex = 12;
			this.TabPages.AddRange(new TabPage[]
				{
					this._outputTabPage,
					this._sourceTabPage
				});
			this.SelectionChanged += new EventHandler(this.SelectedIndexChanged);
			// 
			// XMLTabPage
			// 
			_sourceTabPage.Controls.Add(this._sourceRichTextBox);
			_sourceTabPage.Location = new Point(0, 0);
			_sourceTabPage.Name = "_sourceTabPage";
			_sourceTabPage.Selected = false;
			_sourceTabPage.Size = new Size(824, 453);
			_sourceTabPage.TabIndex = 4;
			_sourceTabPage.Title = "Source";
			// 
			// OutputTabPage
			// 
			_outputTabPage.Controls.Add(this._outputRichTextBox);
			_outputTabPage.Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.World);
			_outputTabPage.Location = new Point(0, 0);
			_outputTabPage.Name = "_outputTabPage";
			_outputTabPage.Size = new Size(824, 453);
			_outputTabPage.TabIndex = 3;
			_outputTabPage.Title = "Output";
			// 
			// _outputTextBox
			// 
			_outputRichTextBox.Anchor = (((((AnchorStyles.Top | AnchorStyles.Bottom)
				| AnchorStyles.Left)
				| AnchorStyles.Right)));
			_outputRichTextBox.BorderStyle = BorderStyle.FixedSingle;
			_outputRichTextBox.DetectUrls = false;
			_outputRichTextBox.Font = new Font("Arial", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((Byte) (0)));
			_outputRichTextBox.Location = new Point(0, 0);
			_outputRichTextBox.Name = "OutputTextBox";
			_outputRichTextBox.ReadOnly = true;
			_outputRichTextBox.Size = new Size(824, 454);
			_outputRichTextBox.TabIndex = 3;
			_outputRichTextBox.Text = "";
			_outputRichTextBox.WordWrap = false;
			_outputRichTextBox.MouseUp += new MouseEventHandler(this.OutputTextBox_MouseUp);
			// 
			// _sourceRichTextBox
			// 
			_sourceRichTextBox.AcceptsTab = true;
			_sourceRichTextBox.Anchor = (((((AnchorStyles.Top | AnchorStyles.Bottom)
				| AnchorStyles.Left)
				| AnchorStyles.Right)));
			_sourceRichTextBox.BorderStyle = BorderStyle.FixedSingle;
			_sourceRichTextBox.DetectUrls = false;
			_sourceRichTextBox.Font = new Font("Courier New", 9.75F, FontStyle.Regular, GraphicsUnit.Point, ((Byte) (0)));
			_sourceRichTextBox.Location = new Point(0, 0);
			_sourceRichTextBox.Name = "XMLRichTextBox";
			_sourceRichTextBox.Size = new Size(824, 454);
			_sourceRichTextBox.TabIndex = 4;
			_sourceRichTextBox.Text = "";
			_sourceRichTextBox.WordWrap = false;
			_sourceRichTextBox.TextChanged += new EventHandler(this.XMLRichTextBox_TextChanged);
			_sourceRichTextBox.MouseUp += new MouseEventHandler(this.XMLTextBox_MouseUp);


			this.ResumeLayout(false);
			_sourceTabPage.ResumeLayout(false);
			_outputTabPage.ResumeLayout(false);
		}

		private void CreateOutputContextMenu()
		{
			MenuCommand copy = new MenuCommand("Cop&y", new EventHandler(this.CopyOutputText));
			MenuCommand selectAll = new MenuCommand("Select &All", new EventHandler(this.SelectAllOutputText));

			_outputContextMenu.MenuCommands.AddRange(new MenuCommand[] {copy, selectAll});
		}

		private void CreateXmlContextMenu()
		{
			MenuCommand copy = new MenuCommand("Cop&y", new EventHandler(this.CopySourceText));
			MenuCommand cut = new MenuCommand("Cu&t", new EventHandler(this.CutSourceText));
			MenuCommand paste = new MenuCommand("&Paste", new EventHandler(this.PasteSourceText));
			MenuCommand spacer = new MenuCommand("-");
			MenuCommand selectAll = new MenuCommand("Select &All", new EventHandler(this.SelectAllSourceText));

			_xmlContextMenu.MenuCommands.AddRange(new MenuCommand[] {copy, cut, paste, spacer, selectAll});
		}

		#endregion

		private void SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.WordWrapChanged != null)
			{
				if (this.SelectedTab == _outputTabPage)
				{
					this.WordWrapChanged(_outputRichTextBox.WordWrap);
				}
				else if (this.SelectedTab == _sourceTabPage)
				{
					this.WordWrapChanged(_sourceRichTextBox.WordWrap);
				}
			}
		}

		private void OutputTextBox_MouseUp(object sender, MouseEventArgs e)
		{
			RichTextBox box = sender as RichTextBox;
			if (e.Button == MouseButtons.Right)
			{
				_outputContextMenu.TrackPopup(box.PointToScreen(new Point(e.X, e.Y)));
			}
		}

		private void XMLTextBox_MouseUp(object sender, MouseEventArgs e)
		{
			RichTextBox box = sender as RichTextBox;
			if (e.Button == MouseButtons.Right)
			{
				_xmlContextMenu.TrackPopup(box.PointToScreen(new Point(e.X, e.Y)));
			}
		}

		private void XMLRichTextBox_TextChanged(object sender, EventArgs e)
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

		public void WriteOutput(string pMessage)
		{
			lock (this)
			{
				if (!_outputRichTextBox.Focused) _outputRichTextBox.Focus();

				Outputter.AppendRtfText(OutputHighlighter.Highlight(pMessage));

				_outputRichTextBox.SelectionStart = _outputRichTextBox.TextLength;
				_outputRichTextBox.SelectedRtf = Outputter.RtfDocument;
				_outputRichTextBox.ScrollToCaret();
			}
		}

		public void CopyText(object sender, EventArgs e)
		{
			if (this.SelectedTab == _outputTabPage)
			{
				_outputRichTextBox.Copy();
			}
			else
			{
				_sourceRichTextBox.Copy();
			}
		}

		public void SelectAllText(object sender, EventArgs e)
		{
			if (this.SelectedTab == _outputTabPage)
			{
				_outputRichTextBox.SelectAll();
			}
			else
			{
				_sourceRichTextBox.SelectAll();
			}
		}

		public void CopyOutputText(object sender, EventArgs e)
		{
			_outputRichTextBox.Copy();
		}

		public void SelectAllOutputText(object sender, EventArgs e)
		{
			_outputRichTextBox.SelectAll();
		}

		public void CopySourceText(object sender, EventArgs e)
		{
			_sourceRichTextBox.Copy();
		}

		public void CutSourceText(object sender, EventArgs e)
		{
			_sourceRichTextBox.Cut();
		}

		public void PasteSourceText(object sender, EventArgs e)
		{
			_sourceRichTextBox.Paste();
		}

		public void SelectAllSourceText(object sender, EventArgs e)
		{
			_sourceRichTextBox.SelectAll();
		}

		public void SavePlainTextOutput(string fileName)
		{
			_outputRichTextBox.SaveFile(fileName,
				RichTextBoxStreamType.PlainText);
		}

		public void SaveRichTextOutput(string fileName)
		{
			_outputRichTextBox.SaveFile(fileName,
				RichTextBoxStreamType.RichText);
		}

		public void Clear()
		{
			this.SelectedTab = _outputTabPage;
			_outputRichTextBox.Clear();
		}

		public string LoadSource(string buildFile)
		{
			using (TextReader reader = File.OpenText(buildFile))
			{
				_source = _sourceRichTextBox.Text = reader.ReadToEnd();
			}

			return _source;
		}

		public void SaveSource(string buildFile)
		{
			using (TextWriter writer = File.CreateText(buildFile))
			{
				writer.Write(_sourceRichTextBox.Text);
			}
		}

		public void WordWrap(object sender, EventArgs e)
		{
			if (this.SelectedTab == _outputTabPage)
			{
				_outputRichTextBox.WordWrap = !_outputRichTextBox.WordWrap;
				if (this.WordWrapChanged != null)
					this.WordWrapChanged(_outputRichTextBox.WordWrap);
			}
			else if (this.SelectedTab == _sourceTabPage)
			{
				_sourceRichTextBox.WordWrap = !_sourceRichTextBox.WordWrap;
				if (this.WordWrapChanged != null)
					this.WordWrapChanged(_sourceRichTextBox.WordWrap);
			}
		}

		public bool SourceHasChanged
		{
			get { return (_sourceRichTextBox.Text != _source && 
					  _sourceRichTextBox.Text != _source.Replace("\r", "")); }
		}
	}
}