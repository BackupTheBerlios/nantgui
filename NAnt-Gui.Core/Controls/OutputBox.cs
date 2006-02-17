using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Crownwood.Magic.Menus;
using NAntGui.Core.Controls.Menu.EditMenu;
using NAntGui.Framework;

namespace NAntGui.Core.Controls
{
	/// <summary>
	/// Summary description for OutputBox.
	/// </summary>
	public class OutputBox : RichTextBox, ILogsMessage, IEditCommands
	{
		private delegate void MessageEventHandler(string message);
		public event VoidBool WordWrapChanged;

		private const int RICH_TEXT_INDEX = 2;
		private const int PLAIN_TEXT_INDEX = 1;
		
		private PopupMenu _outputContextMenu = new PopupMenu();
		private SaveFileDialog _saveDialog = new SaveFileDialog();
		private MainFormMediator _mediator;

		public OutputBox(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "value");
			_mediator = mediator;
			this.Initialize();
			this.CreateOutputContextMenu();
		}

		private void Initialize()
		{
			// 
			// OutputSaveFileDialog
			// 
			_saveDialog.DefaultExt = "txt";
			_saveDialog.FileName = "Output";
			_saveDialog.Filter = "Text Document|*.txt|Rich Text Format (RTF)|*.rtf";

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
			CopyMenuCommand copy = new CopyMenuCommand(_mediator);
			SelectAllMenuCommand selectAll = new SelectAllMenuCommand(_mediator);

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
			_saveDialog.InitialDirectory = Settings.SaveOutputInitialDir;
			DialogResult result = _saveDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				this.Save();
			}
		}

		private void Save()
		{
			int filterIndex = _saveDialog.FilterIndex;

			if (filterIndex == PLAIN_TEXT_INDEX)
			{
				this.SavePlainTextOutput(_saveDialog.FileName);
			}
			else if (filterIndex == RICH_TEXT_INDEX)
			{
				this.SaveRichTextOutput(_saveDialog.FileName);
			}

			FileInfo file = new FileInfo(_saveDialog.FileName);
			Settings.SaveOutputInitialDir = file.DirectoryName;
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

		private void WriteOutput(string pMessage)
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

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			_mediator.OutputGotFocused();
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			_mediator.OutputLostFocused();
		}

		public void Delete()
		{
			/* delete not supported, just ignore */
		}

		public void ReHightlight()
		{
			string text = this.Text;
			this.Clear();
			this.WriteOutput(text);
		}
	}
}
