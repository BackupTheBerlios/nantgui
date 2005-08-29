
#if TEST
using NUnit.Framework;
#endif

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Outputter.
	/// </summary>
	#region TestFixture
#if TEST
	[TestFixture]
#endif
	#endregion
	public class Outputter
	{
		private static string Header = 
								@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033" 
								+ @"{\fonttbl{\f0\fnil\fcharset0 Arial;}}" 
								+ ColorTable.ColorTableRtf 
								+ @"\viewkind4\uc1\pard\cf0\fs17 ";

		private const string FOOTER = @"\par}";

		private static string Output = "";

		#region TestConstuctor
#if TEST
		public Outputter(){}
#endif
		#endregion

		public static void AppendRtfText(string pRtfText)
		{
			Header = 
				@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033" 
				+ @"{\fonttbl{\f0\fnil\fcharset0 Arial;}}" 
				+ ColorTable.ColorTableRtf 
				+ @"\viewkind4\uc1\pard\cf0\fs17 ";

			Output = pRtfText;
		}

		public static void Clear()
		{
			Output = "";
			ColorTable.Reset();
		}

		public static string RtfDocument
		{
			get { return Header + Output + FOOTER; }
		}

		#region Tests
#if TEST
		[Test]
		public void Document()
		{
			Output = "";
			string lText = "BUILD FAILED";
			string lExpectedRtf = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}}{\colortbl ;\red255\green0\blue0;\red0\green0\blue255;\red0\green255\blue0;}\viewkind4\uc1\pard\cf0\fs17 BUILD FAILED\par}";

			AppendRtfText(lText);
			Assert.AreEqual(lExpectedRtf, RtfDocument);
		}
#endif
		#endregion

	}
}
