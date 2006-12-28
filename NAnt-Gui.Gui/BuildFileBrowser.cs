using System.Windows.Forms;

namespace NAntGui.Gui
{
	/// <summary>
	/// Summary description for BuildFileBrowser.
	/// </summary>
	public class BuildFileBrowser
	{
		private static OpenFileDialog _openDialog = new OpenFileDialog();
		private static SaveFileDialog _saveDialog = new SaveFileDialog();

		static BuildFileBrowser()
		{
			_openDialog.DefaultExt =
				_saveDialog.DefaultExt =
				"build";

			_openDialog.Filter =
				_saveDialog.Filter =
				"Build Files (*.build; *.nant)|*.build;*.nant|NAnt Includes (*.inc)|*.inc|All Files (*.*)|*.*";
		}

		public static string[] BrowseForLoad()
		{
			Settings settings = Settings.Instance();
			_openDialog.InitialDirectory = settings.OpenInitialDir;

			if (_openDialog.ShowDialog() == DialogResult.OK)
			{
				return _openDialog.FileNames;
			}
			else
			{
				return new string[] {};
			}
		}

		public static string BrowseForSave()
		{
			DialogResult result = _saveDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				return _saveDialog.FileName;
			}
			else
			{
				return null;
			}
		}
	}
}