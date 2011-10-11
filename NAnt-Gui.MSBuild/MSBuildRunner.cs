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
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using NAntGui.Framework;
using BuildFinishedEventArgs = Microsoft.Build.Framework.BuildFinishedEventArgs;

namespace NAntGui.MSBuild
{
    public class MSBuildRunner : BuildRunnerBase
    {
        List<ILogger> _loggers = new List<ILogger>();
        ProjectCollection _projects = new ProjectCollection();
        Project _project;

        public MSBuildRunner(FileInfo fileInfo, ILogsMessage logger, CommandLineOptions options) :
            base(fileInfo, logger, options)
        {
            _loggers.Add(new GuiLogger(_logger, OnBuildFinished));
        }

        protected override void DoRun()
        {
            Environment.CurrentDirectory = _fileInfo.DirectoryName;            
            
            List<string> targets = _targets.ConvertAll(prop => prop.Name);
            _project = _projects.LoadProject(_fileInfo.FullName);
            _project.Build(targets.ToArray(), _loggers);
            
            //SetTargetFramework();
        }

        public override void Close()
        {
            base.Close();
            _projects.UnregisterAllLoggers();
        }

        public void OnBuildFinished(object sender, BuildFinishedEventArgs e)
        {
            FinishBuild();
        }
    }
}