#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2005 Colin Svingen, Business Watch International
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
using System.Reflection;
using System.Windows.Forms;
using NAnt.Core.Util;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for AppEntry.
	/// </summary>
	public class Main
	{
		private static CommandLineParser _commandLineParser;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static int Run(string[] args)
		{
			AppDomain.CurrentDomain.AssemblyResolve +=
				new ResolveEventHandler(ResolveHandler);

			CommandLineOptions cmdLineOptions = new CommandLineOptions();

			try
			{
				_commandLineParser = new CommandLineParser(typeof (CommandLineOptions), false);
				_commandLineParser.Parse(args, cmdLineOptions);
			}
			catch (CommandLineArgumentException ex)
			{
				// Write logo banner to console if parser was created successfully
				if (_commandLineParser != null)
				{
					Console.WriteLine(_commandLineParser.LogoBanner);
					// insert empty line
					Console.Error.WriteLine();
				}
				// Write message of exception to console
				Console.Error.WriteLine(ex.Message);
				// get first nested exception
				Exception nestedException = ex.InnerException;
				// set initial indentation level for the nested exceptions
				int exceptionIndentationLevel = 0;
				while (nestedException != null && !StringUtils.IsNullOrEmpty(nestedException.Message))
				{
					// indent exception message with 4 extra spaces 
					// (for each nesting level)
					exceptionIndentationLevel += 4;
					// output exception message
					Console.Error.WriteLine(new string(' ', exceptionIndentationLevel)
						+ nestedException.Message);
					// move on to next inner exception
					nestedException = nestedException.InnerException;
				}

				// insert empty line
				Console.WriteLine();

				// instruct users to check the usage instructions
				/*
				Console.WriteLine("Try 'NAnt-Gui -help' for more information");
				*/

				// signal error
				return 1;
			}

			MainForm lMAINForm = new MainForm(cmdLineOptions);
			PersistWindowState.Attach(lMAINForm);

			Application.Run(lMAINForm);

			return 0;
		}

		static Assembly ResolveHandler(object sender, ResolveEventArgs args)
		{
			const string nantCoreAssembly = "NAnt.Core.dll";
			string nantPath = @"C:\Program Files\nant-0.85-rc3\bin\";
			return Assembly.LoadFrom(nantPath + nantCoreAssembly);
		}
	}
}