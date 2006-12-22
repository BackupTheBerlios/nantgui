using System;
using NAntGui.Framework;

namespace NAntGui.Gui
{
	/// <summary>
	/// Summary description for BlankBuildScript.
	/// </summary>
	public class BlankBuildScript : IBuildScript
	{
		public string Description
		{
			get { return ""; }
		}

		public void ParseBuildScript(){ /* do nothing */ }

		public string Name
		{
			get { return "Untitled"; }
		}

		public PropertyCollection Properties
		{
			get { return new PropertyCollection(); }
			set	{ /* do nothing */ }
		}

		public TargetCollection Targets
		{
			get { return new TargetCollection(); }
			set	{ /* do nothing */ }
		}
	}
}
