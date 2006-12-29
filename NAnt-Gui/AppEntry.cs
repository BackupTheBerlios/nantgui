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

using System;
using System.Reflection;
using NAnt.Core.Util;
using NAntGui.Framework;
using NAntGui.Gui;

namespace NAntGui
{
	/// <summary>
	/// Summary description for AppEntry.
	/// </summary>
	public class AppEntry
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
//			AppDomain.CurrentDomain.AssemblyResolve +=
//				new ResolveEventHandler(ResolveHandler);

			CommandLineOptions options = ParseCommandLine(args);

			bool createdNew; 

			string uniqueName = string.Join("*", 
				new string[] {System.Windows.Forms.Application.ProductName,
								 Environment.UserName, "ColinSvingen"});

			System.Threading.Mutex mutex = 
				new System.Threading.Mutex(true, uniqueName, out createdNew); 

			if (createdNew) 
			{ 
				try
				{
					MainFormMediator mediator = new MainFormMediator(options);
					mediator.RunApplication();
				}
				finally
				{
					mutex.Close();
					mutex = null;
				}
			} 
		}

		private static CommandLineOptions ParseCommandLine(string[] args)
		{
			CommandLineParser parser = null;
			CommandLineOptions cmdLineOptions = new CommandLineOptions();

			try
			{
				parser = new CommandLineParser(typeof (CommandLineOptions), false);
				parser.Parse(args, cmdLineOptions);
			}
			catch (CommandLineArgumentException ex)
			{
				HandleError(ex, parser);
			}

			return cmdLineOptions;
		}

		private static void HandleError(CommandLineArgumentException ex, CommandLineParser parser)
		{
			// Write logo banner to console if parser was created successfully
			if (parser != null)
			{
				Console.WriteLine(parser.LogoBanner);
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
		}

//		private static Assembly ResolveHandler(object sender, ResolveEventArgs args)
//		{
//			const string nantCoreAssembly = "NAnt.Core.dll";
//			string nantPath = @"C:\Program Files\nant-0.85\bin\";
//			return Assembly.LoadFrom(nantPath + nantCoreAssembly);
//		}
	}
}