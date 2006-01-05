using System;
using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Menus;
using NAntGui.Framework;

namespace NAntGui.Core.Controls
{
	/// <summary>
	/// Summary description for OutputBox.
	/// </summary>
	public class OutputBox : RichTextBox, ILogsMessage
	{
		private delegate void MessageEventHandler(string message);
		public event VoidBool WordWrapChanged;

		private const int RICH_TEXT_INDEX = 2;
		private const int PLAIN_TEXT_INDEX = 1;
		
		private PopupMenu _outputContextMenu = new PopupMenu();
		private SaveFileDialog OutputSaveFileDialog = new SaveFileDialog();

		public OutputBox()
		{
			this.Initialize();
			this.CreateOutputContextMenu();
		}

		private void Initialize()
		{
			// 
			// OutputSaveFileDialog
			// 
			this.OutputSaveFileDialog.DefaultExt = "txt";
			this.OutputSaveFileDialog.FileName = "Output";
			this.OutputSaveFileDialog.Filter = "Text Document|*.txt|Rich Text Format (RTF)|*.rtf";

			this.Dock = DockStyle.Fill;
			this.BorderStyle = BorderStyle.FixedSingle;
			this.DetectUrls = false;
			this.ReadOnly = true;
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

		public void SaveOutput()
		{
//			this.OutputSaveFileDialog.InitialDirectory = _sourceFile.FullPath;
			DialogResult result = this.OutputSaveFileDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				this.Save();
			}
		}

		private void Save()
		{
			int filterIndex = this.OutputSaveFileDialog.FilterIndex;

			if (filterIndex == PLAIN_TEXT_INDEX)
			{
				this.SavePlainTextOutput(this.OutputSaveFileDialog.FileName);
			}
			else if (filterIndex == RICH_TEXT_INDEX)
			{
				this.SaveRichTextOutput(this.OutputSaveFileDialog.FileName);
			}
		}

		private void CopyText(object sender, EventArgs e)
		{
			this.Copy();
		}

		private void SelectAllText(object sender, EventArgs e)
		{
			this.SelectAll();
		}

		private void SavePlainTextOutput(string fileName)
		{
			this.SaveFile(fileName,
				RichTextBoxStreamType.PlainText);
		}

		private void SaveRichTextOutput(string fileName)
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

		public void DoWordWrap()
		{
			this.WordWrap = !this.WordWrap;
			if (this.WordWrapChanged != null)
				this.WordWrapChanged(this.WordWrap);
		}

		public new void Clear()
		{
			base.Clear();
			Outputter.Clear();
		}

		public void LogMessage(string message)
		{
			if (this.InvokeRequired)
			{
				MessageEventHandler messageEH =
					new MessageEventHandler(this.WriteOutput);

				this.BeginInvoke(messageEH, new Object[1] {message});
			}
			else
			{
				this.WriteOutput(message);
			}
		}
	}
}
