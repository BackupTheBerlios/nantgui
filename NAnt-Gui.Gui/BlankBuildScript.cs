using System;
using NAntGui.Framework;
using System.Collections.Generic;

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

        public void Parse() { /* do nothing */ }

        public string Name
		{
			get { return "Untitled"; }
		}

        public PropertyCollection Properties
		{
			get { return new PropertyCollection(); }
		}

        public List<BuildTarget> Targets
		{
			get { return new List<BuildTarget>(); }
		}
	}
}
