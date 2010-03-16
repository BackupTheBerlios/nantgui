using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace NAntGui.Core
{
    /// <summary>
    /// Summary description for Utils.
    /// </summary>
    public static class Utils
    {
        private static readonly char[] _asterisk = new[] { '*' };
        private static ResourceManager _resources = new ResourceManager("NAntGui.Core.Properties.Resources", Assembly.GetExecutingAssembly());

        public static string DockingConfigPath
        {
            get { return Path.Combine(Path.GetDirectoryName(Application.LocalUserAppDataPath), "DockPanel.config"); }
        }

        public static string RunDirectory
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                if (assembly != null && assembly.Location != null)
                {
                    FileInfo info = new FileInfo(assembly.Location);
                    return info.DirectoryName;
                }
                return "";
            }
        }

        public static bool HasAsterisk(string text)
        {
            return text.EndsWith("*");
        }

        public static string RemoveAsterisk(string text)
        {
            return text.TrimEnd(_asterisk);
        }

        public static string AddAsterisk(string text)
        {
            return text + "*";
        }

        public static void LoadHelpFile(string filename)
        {
            if (File.Exists(filename))
            {
                Process.Start(filename);
            }
            else
            {
                MessageBox.Show(_resources.GetString("HelpNotFoundError"), _resources.GetString("HelpNotFoundTitle"),
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public static void ShowFileNotFoundError(string file)
        {
            MessageBox.Show(file + _resources.GetString("FileNotFoundError"), _resources.GetString("FileNotFoundTitle"),
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void ShowSaveError(string file, string exception)
        {
            string messageString = _resources.GetString("CouldNotSaveError");
            string message = messageString != null
                                 ? string.Format(messageString, file, exception)
                                 : string.Format("{0}: {1}", file, exception);

            MessageBox.Show(message, _resources.GetString("CouldNotSaveTitle"), MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }

        public static void ShowProjectTemplateMissingError()
        {
            MessageBox.Show(_resources.GetString("ProjectTemplateMissingError"));
        }

        public static string GetDragFilename(DragEventArgs e)
        {
            object dragData = e.Data.GetData(DataFormats.FileDrop, false);
            string[] files = dragData as string[];
            if (files != null && files.Length > 0) return files[0];
            return "";
        }
    }
}