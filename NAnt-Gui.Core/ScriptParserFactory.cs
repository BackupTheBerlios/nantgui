using System;
using System.IO;
using NAnt_Gui.MSBuild;
using NAntGui.Framework;
using NAntGui.NAnt;

namespace NAntGui.Core
{
    /// <summary>
    /// Summary description for ScriptParserFactory.
    /// </summary>
    public static class ScriptParserFactory
    {
        public static IBuildScript Create(FileInfo fileInfo)
        {
            IBuildScript script;

            if (Utils.NantExtensions.Contains(fileInfo.Extension))
                script = new NAntBuildScript(fileInfo);
            else if (Utils.MsbuildExtensions.Contains(fileInfo.Extension) || fileInfo.Extension.EndsWith("proj"))
                script = new MSBuildScript(fileInfo);
            else
                // TODO: What to do with generic files?
                throw new NotImplementedException("OMG, can't handle files I don't know about!?!");

            return script;
        }
    }


}