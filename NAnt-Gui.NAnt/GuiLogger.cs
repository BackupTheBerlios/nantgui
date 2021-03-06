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

using NAnt.Core;
using NAntGui.Framework;

namespace NAntGui.NAnt
{
    /// <summary>
    /// Summary description for GuiLogger.
    /// </summary>
    public class GuiLogger : DefaultLogger
    {
        private readonly ILogsMessage _messageLogger;

        public GuiLogger(ILogsMessage messageLogger)
        {
            _messageLogger = messageLogger;
        }

        protected override void Log(string message)
        {
            _messageLogger.LogMessage(message);
        }
    }
}