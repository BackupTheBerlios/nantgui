using System.Collections.Generic;
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

        public void Parse()
        {
            /* do nothing */
        }

        public string Name
        {
            get { return "Untitled"; }
        }

        public Dictionary<string, IBuildProperty> Properties
        {
            get { return new Dictionary<string, IBuildProperty>(); }
        }

        public List<IBuildTarget> Targets
        {
            get { return new List<IBuildTarget>(); }
        }
    }
}