using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace NAntGui.Core
{
    /// <summary>
    /// Summary description for Utils.
    /// </summary>
    public static class Utils
    {
        public static string DockingConfigPath
        {
            get { return Path.Combine(Path.GetDirectoryName(Application.LocalUserAppDataPath), "DockPanel.config"); }
        }

        public static string RunDirectory
        {
            get
            {
                Assembly ass = Assembly.GetExecutingAssembly();
                FileInfo info = new FileInfo(ass.Location);
                return info.DirectoryName;
            }
        }

        public static bool HasAsterisk(string text)
        {
            return text.EndsWith("*");
        }

        public static string RemoveAsterisk(string text)
        {
            return text.TrimEnd(new[] {'*'});
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
                MessageBox.Show("Help not found.  It may not have been installed.", "Help Not Found");
            }
        }

        public static void ShowFileNotFoundError(string recentItem)
        {
            MessageBox.Show(recentItem + " was not found.", "Build File Not Found",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static string GetDragFilename(DragEventArgs e)
        {
            object dragData = e.Data.GetData(DataFormats.FileDrop, false);
            string[] files = dragData as string[];
            return files[0];
        }
    }
}