using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Crownwood.Magic.Menus;
using NAntGui.Gui.Controls.Menu.EditMenu;
using NAntGui.Framework;

namespace NAntGui.Gui.Controls
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

		private PointConverter _pc = new PointConverter();
		private PopupMenu _outputContextMenu = new PopupMenu();
		private SaveFileDialog _saveDialog = new SaveFileDialog();
		private MainFormMediator _mediator;

		public OutputBox(MainFormMediator mediator)
		{			
			Assert.NotNull(mediator, "value");
			_mediator = mediator;
			Initialize();
		}

		private void Initialize()
		{
			// 
			// OutputSaveFileDialog
			// 
			_saveDialog.DefaultExt = "txt";
			_saveDialog.FileName = "Output";
			_saveDialog.Filter = "Text Document|*.txt|Rich Text Format (RTF)|*.rtf";

			SetStyle(ControlStyles.StandardClick, true);
			Dock = DockStyle.Fill;
			BorderStyle = BorderStyle.FixedSingle;
			DetectUrls = false;
			ReadOnly = true;
			WordWrap = false;
			MouseUp += new MouseEventHandler(OutputTextBox_MouseUp);
			Enter += new EventHandler(DoEnter);
			
			CreateOutputContextMenu();
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
		
		public int GetLineAtCursor()
		{
			return GetLineFromCharIndex(SelectionStart);
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
		
		protected override void OnDoubleClick(EventArgs e)
		{
			CheckLineForFile();
			base.OnDoubleClick(e);
		}

		private void CheckLineForFile()
		{
			string line = GetLine();
			if (HasFile(line))
			{
				string file = GetFile(line);
				_mediator.LoadBuildFile(file);
				Point p = GetPoint(line);
				_mediator.SetCursor(p.X, p.Y);
			}
		}

		private Point GetPoint(string line)
		{
			const string FILE_REG = @"[a-zA-Z]:(\\[^\\]*)+(\.[\\\.]+)*\((\d+,\d+)\)";
			Regex reg = new Regex(FILE_REG);
			Match m = reg.Match(line);
			Group g = m.Groups[3];
			string coords = g.Value;
			return (Point) _pc.ConvertFromString(coords);
		}

		private string GetFile(string line)
		{
			const string FILE_REG = @"[a-zA-Z]:(\\[^\\\(\),]*)+";
			Regex reg = new Regex(FILE_REG);
			return reg.Match(line).Value;
		}

		private bool HasFile(string line)
		{
			const string FILE_REG = @"[a-zA-Z]:(\\[^\\]*)+(\.[\\\.]+)*\(\d+,\d+\)";
			Regex reg = new Regex(FILE_REG);
			return reg.IsMatch(line);
		}

		private string GetLine()
		{
			int lineNumber = GetLineAtCursor();
			return Lines[lineNumber];
		}
	}
}