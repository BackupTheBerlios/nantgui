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
// Colin Svingen (csvingen@businesswatch.ca)
#endregion

using System.Text.RegularExpressions;

#if TEST
using NUnit.Framework;
#endif

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for SyntaxHighlighter.
	/// </summary>
	#region TestFixture
#if TEST
	[TestFixture]
#endif
	#endregion
	public class SyntaxHighlighter
	{
		private const string BUILD_SUCCEEDED = "BUILD SUCCEEDED";
		public const string BUILD_FAILED = "BUILD FAILED";

		private struct Tags
		{
			public const string BLACK = "\\cf0";

			public const string BOLD = "\\b";
			public const string END_BOLD = "\\b0";

			public const string BIG = "\\fs18";
			public const string END_BIG = "\\fs17";

			public const string P = "\\par";

			public const string SPACE = " ";
		}


		#region TestConstuctor
#if TEST
		public SyntaxHighlighter(){}
#endif
		#endregion

		public static string Highlight(string pText)
		{
			string[] lExpressions = {BUILD_FAILED, BUILD_SUCCEEDED, @"\[[^\[]+\]", "error", "ERROR"};
			string lHighlightedText = Escape(pText);

			foreach (string lExpression in lExpressions)
			{
				Regex lRegex = new Regex(lExpression, RegexOptions.IgnoreCase);
				
				if (lRegex.IsMatch(lHighlightedText))
				{
					Match lMatch = lRegex.Match(lHighlightedText);

					switch (lExpression)
					{
						case BUILD_FAILED:
							string lOutput = ColorTable.RedTag + Tags.BOLD + Tags.BIG + Tags.SPACE
								+ lMatch.Value + Tags.BLACK + Tags.END_BOLD + Tags.END_BIG;

							lHighlightedText = lHighlightedText.Replace(lMatch.Value, lOutput);
							break;
						case BUILD_SUCCEEDED:
							lOutput = ColorTable.GreenTag + Tags.BOLD + Tags.BIG + Tags.SPACE
								+ lMatch.Value + Tags.BLACK + Tags.END_BOLD + Tags.END_BIG;

							lHighlightedText = lHighlightedText.Replace(lMatch.Value, lOutput);
							break;

						case @"\[[^\[]+\]":
							lOutput = ColorTable.BlueTag + Tags.SPACE
								+ lMatch.Value + Tags.BLACK + Tags.SPACE;

							lHighlightedText = lHighlightedText.Replace(lMatch.Value, lOutput);
							break;

						case "error":
						case "ERROR":
							lOutput = ColorTable.RedTag + Tags.BOLD + Tags.SPACE
								+ lMatch.Value + Tags.BLACK + Tags.END_BOLD + Tags.SPACE;

							lHighlightedText = lHighlightedText.Replace(lMatch.Value, lOutput);
							break;
					}

				}
			}

			return lHighlightedText + Tags.SPACE;
		}

		private static string Escape(string pText)
		{
			string lText = pText.Replace(@"\", @"\\");
			lText = lText.Replace("{", @"\{");
			lText = lText.Replace("}", @"\}");
			return lText;
		}

		#region Tests
#if TEST
		[Test]
		public void ModifyBuildFailed()
		{
			string lBuildFailed = "BUILD FAILED";
			string lModifiedBuildFailed = @"\cf1\b\fs18 BUILD FAILED\cf0\b0\fs17 ";
			Assert.AreEqual(lModifiedBuildFailed, Highlight(lBuildFailed));
		}

		[Test]
		public void ModifyBuildSucceeded()
		{
			string lBuildSucceeded = "BUILD SUCCEEDED";
			string lModifiedBuildSucceeded = @"\cf3\b\fs18 BUILD SUCCEEDED\cf0\b0\fs17 ";
			Assert.AreEqual(lModifiedBuildSucceeded, Highlight(lBuildSucceeded));
		}

		[Test]
		public void ModifySquareTag()
		{
			string lSquareTag = "this [that] theotherthing";
			string lModifiedSquareTag = @"this \cf2 [that]\cf0  theotherthing ";
			Assert.AreEqual(lModifiedSquareTag, Highlight(lSquareTag));
		}

		[Test]
		public void ModifyError()
		{
			string lError = "asdfd error sfdsfd";
			string lModifiedError = @"asdfd \cf1\b error\cf0\b0  sfdsfd ";
			Assert.AreEqual(lModifiedError, Highlight(lError));
		}
#endif
		#endregion
	}
}
