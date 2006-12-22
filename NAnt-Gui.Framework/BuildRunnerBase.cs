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
// Colin Svingen (nantgui@swoogan.com)

#endregion

using System.Threading;

namespace NAntGui.Framework
{
	public abstract class BuildRunnerBase
	{
		public event VoidVoid BuildFinished;

		protected ILogsMessage _logger;
		protected CommandLineOptions _options;

		private Thread _thread;
//		protected IBuildScript _script;		

		protected abstract void DoRun();

		public BuildRunnerBase(ILogsMessage logger, CommandLineOptions options)
		{
//			Assert.NotNull(script, "script");
			Assert.NotNull(logger, "logger");
			Assert.NotNull(options, "options");

//			_script = script;
			_logger = logger;
			_options = options;
		}

		public void Run()
		{
			_thread = new Thread(new ThreadStart(DoRun));
			_thread.Start();
		}

		public void Stop()
		{
			if (_thread != null)
			{
				_thread.Abort();
			}
		}

		public abstract void AddProperties(PropertyCollection properties);

		public abstract void AddTargets(TargetCollection targets);

		protected void FinishBuild()
		{
			if (BuildFinished != null)
			{
				BuildFinished();
			}
		}
	}
}