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

using NAntGui.Core;
using NUnit.Framework;

namespace NAntGui.Unittests
{
	/// <summary>
	/// Summary description for OutputHighlighterTests.
	/// </summary>
	[TestFixture]
	public class OutputHighlighterTests
	{
		public OutputHighlighterTests()
		{
		}

		[Test]
		public void ModifyBuildFailed()
		{
			string lBuildFailed = "BUILD FAILED";
			string lModifiedBuildFailed = @"\cf1\b\fs18 BUILD FAILED\cf0\b0\fs17 ";
			Assert.AreEqual(lModifiedBuildFailed, OutputHighlighter.Highlight(lBuildFailed));
		}

		[Test]
		public void ModifyBuildSucceeded()
		{
			string lBuildSucceeded = "BUILD SUCCEEDED";
			string lModifiedBuildSucceeded = @"\cf3\b\fs18 BUILD SUCCEEDED\cf0\b0\fs17 ";
			Assert.AreEqual(lModifiedBuildSucceeded, OutputHighlighter.Highlight(lBuildSucceeded));
		}

		[Test]
		public void ModifySquareTag()
		{
			string lSquareTag = "this [that] theotherthing";
			string lModifiedSquareTag = @"this \cf2 [that]\cf0  theotherthing ";
			Assert.AreEqual(lModifiedSquareTag, OutputHighlighter.Highlight(lSquareTag));
		}

		[Test]
		public void ModifyError()
		{
			string lError = "asdfd error sfdsfd";
			string lModifiedError = @"asdfd \cf1\b error\cf0\b0  sfdsfd ";
			Assert.AreEqual(lModifiedError, OutputHighlighter.Highlight(lError));
		}
	}
}