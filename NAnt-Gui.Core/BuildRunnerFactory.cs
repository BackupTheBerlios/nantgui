#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2007 Colin Svingen
//
// NAnt-Gui is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// NAnt-Gui is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//
// Colin Svingen (swoogan@gmail.com)

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using NAntGui.Framework;
using NAntGui.NAnt;

namespace NAntGui.Core
{
    /// <summary>
    /// Summary description for BuildRunnerFactory.
    /// </summary>
    public static class BuildRunnerFactory
    {
        public static BuildRunnerBase Create(FileInfo fileInfo, ILogsMessage logger, CommandLineOptions options)
        {
            if (Utils.NantExtensions.Contains(fileInfo.Extension))
                return new NAntBuildRunner(fileInfo, logger, options);

            if (Utils.MsbuildExtensions.Contains(fileInfo.Extension) || fileInfo.Extension.EndsWith("proj"))
                throw new NotImplementedException("MSBuild runner not implemented");

            // TODO: What to do with generic files?
            throw new NotImplementedException("OMG, can't handle files I don't know about?");
        }
    }
}