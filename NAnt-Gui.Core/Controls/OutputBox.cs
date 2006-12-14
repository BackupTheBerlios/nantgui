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
			Initialize();
			CreateOutputContextMenu();
		}

		private void Initialize()
		{
			// 
			// OutputSaveFileDialog
			// 
			_saveDialog.DefaultExt = "txt";
			_saveDialog.FileName = "Output";
			_saveDialog.Filter = "Text Document|*.txt|Rich Text Format (RTF)|*.rtf";

			Dock = DockStyle.Fill;
			BorderStyle = BorderStyle.FixedSingle;
			DetectUrls = false;
			ReadOnly = true;
			WordWrap = false;
			MouseUp += new MouseEventHandler(OutputTextBox_MouseUp);
			Enter += new EventHandler(DoEnter);
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
			if (WordWrapChanged != null)
				WordWrapChanged(WordWrap);
		}

		public void SaveOutput()
		{
			_saveDialog.InitialDirectory = Settings.SaveOutputInitialDir;
			DialogResult result = _saveDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				Save();
			}
		}

		private void Save()
		{
			int filterIndex = _saveDialog.FilterIndex;

			if (filterIndex == PLAIN_TEXT_INDEX)
			{
				SavePlainTextOutput(_saveDialog.FileName);
			}
			else if (filterIndex == RICH_TEXT_INDEX)
			{
				SaveRichTextOutput(_saveDialog.FileName);
			}

			FileInfo file = new FileInfo(_saveDialog.FileName);
			Settings.SaveOutputInitialDir = file.DirectoryName;
		}

		private void SavePlainTextOutput(string fileName)
		{
			SaveFile(fileName,
			         RichTextBoxStreamType.PlainText);
		}

		private void SaveRichTextOutput(string fileName)
		{
			SaveFile(fileName,
			         RichTextBoxStreamType.RichText);
		}

		private void WriteOutput(string pMessage)
		{
			lock (this)
			{
				if (!Focused) Focus();

				Outputter.AppendRtfText(OutputHighlighter.Highlight(pMessage));

				SelectionStart = TextLength;
				SelectedRtf = Outputter.RtfDocument;
				ScrollToCaret();
			}
		}

		public void DoWordWrap()
		{
			WordWrap = !WordWrap;
			if (WordWrapChanged != null)
				WordWrapChanged(WordWrap);
		}

		public new void Clear()
		{
			base.Clear();
			Outputter.Clear();
		}

		public void LogMessage(string message)
		{
			if (InvokeRequired)
			{
				MessageEventHandler messageEH =
					new MessageEventHandler(WriteOutput);

				BeginInvoke(messageEH, new Object[1] {message});
			}
			else
			{
				WriteOutput(message);
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
			string text = Text;
			Clear();
			WriteOutput(text);
		}

//		protected override void OnFontChanged(EventArgs e)
//		{
//			MessageBox.Show(this.Font.FontFamily.Name);
//		}

//		protected override void OnTextChanged(EventArgs e)
//		{
//			MessageBox.Show(this.Rtf);
//		}
	}
}