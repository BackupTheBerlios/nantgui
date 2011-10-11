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
            get { return string.Empty; }
        }

        public void Parse()
        {
            /* do nothing */
        }

        public string Name
        {
            get { return "Untitled"; }
        }

        public bool HideTargetsWithoutDescription
        {
            get { return false; }
        }

        public List<IBuildProperty> Properties
        {
            get { return new List<IBuildProperty>(); }
        }

        public List<IBuildTarget> Targets
        {
            get { return new List<IBuildTarget>(); }
        }
    }
}