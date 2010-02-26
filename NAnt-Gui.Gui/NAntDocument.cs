#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2007 Colin Svingen
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General internal License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General internal License for more details.
//
// You should have received a copy of the GNU General internal License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Colin Svingen (swoogan@gmail.com)

#endregion

using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using NAntGui.Core;
using NAntGui.NAnt;
using NAntGui.Framework;

namespace NAntGui.Gui
{
	/// <summary>
	/// </summary>
	class NAntDocument
	{
		private BuildRunnerBase _buildRunner;
		private IBuildScript _buildScript;
		private MainController _controller;
        private ILogsMessage _logger;        
        private FileType _fileType;
        private string _contents;
        private string _name;
        private string _fullName;
        private string _directory;

		/// <summary>
		/// Creates new untitled document
		/// </summary>
        internal NAntDocument(ILogsMessage logger, MainController controller)
		{
			Assert.NotNull(logger, "logger");
			Assert.NotNull(controller, "mediator");

            _controller = controller;
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
        internal NAntDocument(string filename, ILogsMessage logger, MainController controller)
		{
			Assert.NotNull(filename, "filename");
			Assert.NotNull(logger, "logger");
            Assert.NotNull(controller, "controller");
			
			_controller = controller;            
			_logger = logger;
            _fullName = filename;

            FileInfo fileInfo = new FileInfo(_fullName);
            _name = fileInfo.Name;
            _directory = fileInfo.DirectoryName;

			Load(filename);

            _buildScript = ScriptParserFactory.Create(fileInfo);
			_buildRunner = BuildRunnerFactory.Create(fileInfo, logger, _controller.Options);
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
            _buildRunner = BuildRunnerFactory.Create(fileInfo, _logger, _controller.Options);			

			ParseBuildFile();
		}

		internal void Save(string contents, bool update)
		{
			if (_fileType == FileType.Existing)
			{
//                _watcher.EnableRaisingEvents = false;
                File.WriteAllText(_fullName, contents);
//                _watcher.EnableRaisingEvents = true;

				_contents = contents;

				if (update)
					ParseBuildFile();				
			}
			else if (_fileType == FileType.New)
			{
                // TODO: Should make this an event to decouple the control from this class
				_controller.SaveDocumentAs();
			}
		}

		internal void Load(string filename)
		{
            _fileType = FileType.Existing;
            _contents = File.ReadAllText(_fullName);
		}

		internal void Stop()
		{
			if (_buildRunner != null)
			{
				_buildRunner.Stop();
			}
		}

		internal void Run()
		{
			if (_buildRunner != null)
			{
//				_buildRunner.Properties = _buildScript.Properties;
				_buildRunner.Run();
			}
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
			catch(Exception error)
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
