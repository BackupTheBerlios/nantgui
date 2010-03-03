#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2007 Colin Svingen
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Colin Svingen (swoogan@gmail.com)

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NAntGui.Core;
using NAntGui.Framework;

namespace NAntGui.Gui
{
    /// <summary>
    /// </summary>
    internal class NAntDocument
    {
        private readonly CommandLineOptions _options;
        private readonly ILogsMessage _logger;
        private BuildRunnerBase _buildRunner;
        private IBuildScript _buildScript;
        private string _contents;
        private string _directory;
        private FileType _fileType;
        private string _fullName;
        private string _name;

        /// <summary>
        /// Creates new untitled document
        /// </summary>
        internal NAntDocument(ILogsMessage logger, CommandLineOptions options)
        {
            Assert.NotNull(logger, "logger");
            Assert.NotNull(options, "options");

            _options = options;
            _logger = logger;
            _name = "Untitled*";
            _directory = ".\\";
            _fullName = _directory + _name;
            _contents = "";
            _fileType = FileType.New;

            _buildScript = new BlankBuildScript();
        }

        /// <summary>
        /// Loads an existing project file
        /// </summary>
        internal NAntDocument(string filename, ILogsMessage logger, CommandLineOptions options)
        {
            Assert.NotNull(filename, "filename");
            Assert.NotNull(logger, "logger");
            Assert.NotNull(options, "options");

            _options = options;
            _logger = logger;
            _fullName = filename;

            FileInfo fileInfo = new FileInfo(_fullName);
            _name = fileInfo.Name;
            _directory = fileInfo.DirectoryName;

            Load(filename);

            _buildScript = ScriptParserFactory.Create(fileInfo);
            _buildRunner = BuildRunnerFactory.Create(fileInfo, logger, _options);
            _buildRunner.Properties = _buildScript.Properties;
        }

        internal void ParseBuildScript()
        {
            _buildScript.Parse();
        }

        internal void Reload()
        {
            if (_fileType == FileType.Existing)
            {
                Load(_fullName);
                ParseBuildFile();
            }
        }

        internal void SaveAs(string filename, string contents)
        {
            Assert.NotNull(filename, "filename");
            Assert.NotNull(contents, "contents");

            _fullName = filename;
            _contents = contents;

            File.WriteAllText(_fullName, _contents);

            FileInfo fileInfo = new FileInfo(filename);
            _name = fileInfo.Name;
            _directory = fileInfo.DirectoryName;

            _buildScript = ScriptParserFactory.Create(fileInfo);
            _buildRunner = BuildRunnerFactory.Create(fileInfo, _logger, _options);

            ParseBuildFile();
        }

        internal void Save(string contents, bool update)
        {
            File.WriteAllText(_fullName, contents);
            _contents = contents;

            if (update)
                ParseBuildFile();
        }

        internal void Load(string filename)
        {
            _fileType = FileType.Existing;
            _contents = File.ReadAllText(_fullName);
        }

        internal void Stop()
        {
            if (_buildRunner != null)
                _buildRunner.Stop();
        }

        internal void Run()
        {
            if (_buildRunner != null)
                _buildRunner.Run();
        }

        internal void SetTargets(List<BuildTarget> targets)
        {
            Assert.NotNull(targets, "targets");
            if (_buildRunner != null)
                _buildRunner.Targets = targets;
        }

        internal void Close()
        {
            if (_buildRunner != null)
                _buildRunner.Stop();
        }

        internal bool IsDirty(string contents)
        {
            return contents != _contents;
        }

        private void ParseBuildFile()
        {
            // Might want a more specific exception type to be caught here.
            // For example, a NullReferenceException on _buildScript 
            // shouldn't be ignored.
            try
            {
                _buildScript.Parse();
            }
#if DEBUG
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
#else
			catch
			{
				/* ignore */
			}
#endif
        }

        #region Properties

        public string FullName
        {
            get { return _fullName; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Directory
        {
            get { return _directory; }
        }

        public string Contents
        {
            get { return _contents; }
            set { _contents = value; }
        }

        internal IBuildScript BuildScript
        {
            get { return _buildScript; }
        }

        internal EventHandler<BuildFinishedEventArgs> BuildFinished
        {
            set { _buildRunner.BuildFinished += value; }
        }

        public FileType FileType
        {
            get { return _fileType; }
        }

        #endregion
    }
}