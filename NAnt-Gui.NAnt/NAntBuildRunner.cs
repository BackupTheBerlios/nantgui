#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2005 Colin Svingen
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
// Colin Svingen (nantgui@swoogan.com)

#endregion

using System;
using NAnt.Core;
using NAntGui.Framework;

namespace NAntGui.NAnt
{
	public class NAntBuildRunner : IBuildRunner
	{
		private NAntBuildScript _script;
		private ILogsMessage _messageLogger;

		public NAntBuildRunner(SourceFile sourceFile, ILogsMessage messageLogger, 
			CommandLineOptions options)
		{
			_messageLogger = messageLogger;

			try
			{
				_script = new NAntBuildScript(sourceFile, options, _messageLogger);
			}
			catch (ApplicationException error)
			{
				throw new BuildFileLoadException(GetErrorMessage(error));
			}
#if RELEASE
			catch (Exception error)
			{
				// all other exceptions should have been caught
				string message = error.Message + Environment.NewLine + 
					error.StackTrace;
				throw new BuildFileLoadException(message);
			}
#endif
		}

		private static string GetErrorMessage(ApplicationException error)
		{
			string message = "";
#if DEBUG
			string errorType = error.GetType().ToString() 
				+ Environment.NewLine + error.StackTrace;

			message += errorType;
#endif

			if (error.InnerException != null && error.InnerException.Message != null)
			{
				message += error.Message + Environment.NewLine + error.InnerException.Message;
			}
			else
			{
				message += error.Message;
			}

			return message;
		}

		protected override void DoRun()
		{
			try
			{
				Environment.CurrentDirectory = _script.SourceFile.Path;
//				ArrayList targets = _nantForm.GetTreeTargets();
//				Hashtable properties = _nantForm.GetProjectProperties();

				_script.Run();
			}
			catch (BuildException error)
			{
				_messageLogger.LogMessage(error.Message);
			}
		}

		public override VoidVoid BuildFinished
		{
			set { _script.BuildFinished += value; }
		}

		public override IBuildScript BuildScript
		{
			get { return _script; }
		}
	}
}
