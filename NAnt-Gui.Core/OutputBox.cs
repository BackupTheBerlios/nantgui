using System;
using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Menus;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for OutputBox.
	/// </summary>
	public class OutputBox : RichTextBox
	{
		public event VoidBool WordWrapChanged;
		private PopupMenu _outputContextMenu = new PopupMenu();

		public OutputBox()
		{
			this.Initialize();
			this.CreateOutputContextMenu();
		}

		private void Initialize()
		{
			this.Anchor = (((((AnchorStyles.Top | AnchorStyles.Bottom)
				| AnchorStyles.Left)
				| AnchorStyles.Right)));
			this.BorderStyle = BorderStyle.FixedSingle;
			this.DetectUrls = false;
			this.Font = new Font("Arial", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((Byte) (0)));
			this.Location = new Point(0, 0);
			this.Name = "OutputTextBox";
			this.ReadOnly = true;
			this.Size = new Size(824, 454);
			this.TabIndex = 3;
			this.Text = "";
			this.WordWrap = false;
			this.MouseUp += new MouseEventHandler(this.OutputTextBox_MouseUp);
			this.Enter += new EventHandler(this.DoEnter);
		}

		private void CreateOutputContextMenu()
		{
			MenuCommand copy = new MenuCommand("Cop&y", new EventHandler(this.CopyText));
			MenuCommand selectAll = new MenuCommand("Select &All", new EventHandler(this.SelectAllText));

			_outputContextMenu.MenuCommands.AddRange(new MenuCommand[] {copy, selectAll});
		}

		private void OutputTextBox_MouseUp(object sender, MouseEventArgs e)
		{
			RichTextBox box = sender as RichTextBox;
			if (e.Button == MouseButtons.Right)
			{
				_outputContextMenu.TrackPopup(box.PointToScreen(new Point(e.X, e.Y)));
			}
		}

		private void DoEnter(object sender, EventArgs e)
		{
			if (this.WordWrapChanged != null)
				this.WordWrapChanged(this.WordWrap);
		}

		public void CopyText(object sender, EventArgs e)
		{
			this.Copy();
		}

		public void SelectAllText(object sender, EventArgs e)
		{
			this.SelectAll();
		}

		public void SavePlainTextOutput(string fileName)
		{
			this.SaveFile(fileName,
				RichTextBoxStreamType.PlainText);
		}

		public void SaveRichTextOutput(string fileName)
		{
			this.SaveFile(fileName,
				RichTextBoxStreamType.RichText);
		}

		public void WriteOutput(string pMessage)
		{
			lock (this)
			{
				if (!this.Focused) this.Focus();

				Outputter.AppendRtfText(OutputHighlighter.Highlight(pMessage));

				this.SelectionStart = this.TextLength;
				this.SelectedRtf = Outputter.RtfDocument;
				this.ScrollToCaret();
			}
		}

		public void DoWordWrap(object sender, EventArgs e)
		{
			this.WordWrap = !this.WordWrap;
			if (this.WordWrapChanged != null)
				this.WordWrapChanged(this.WordWrap);
		}
	}
}
