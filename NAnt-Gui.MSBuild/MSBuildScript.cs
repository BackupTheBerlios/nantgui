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
// Colin Svingen (swoogan@gmail.com)

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Evaluation;
using NAntGui.Framework;
using Microsoft.Build.Execution;


namespace NAntGui.MSBuild
{
    public class MSBuildScript : BuildScript
    {
        private ProjectCollection _projects = new ProjectCollection();
        private ProjectInstance _projectInstance;
        private Project _project;
        private List<string> _defaultTargets;
        private List<string> _initialTargets;

        public MSBuildScript(FileInfo file) : base(file)
        {
            Name = file.Name.Remove(file.Name.LastIndexOf('.'));
            Name = Name.Substring(0, 1).ToUpper() + Name.Substring(1);
            HideTargetsWithoutDescription = false;
        }

        public override void Parse()
        {
            CreateProject();

            Targets.Clear();
            Properties.Clear();

            _defaultTargets = _projectInstance.DefaultTargets;
            _initialTargets = _projectInstance.InitialTargets;
            ParseTargets();
            ParseProperties();
        }

        private void CreateProject()
        {
            try
            {
                _projectInstance = new ProjectInstance(_file.FullName);
                _project = _projects.LoadProject(_file.FullName);
            }
            catch (Exception error)
            {
                throw new BuildFileLoadException("Error parsing MSBuild project", error);
            }
        }

        private void ParseTargets()
        {
            foreach (ProjectTargetInstance mstarget in _projectInstance.Targets.Values)
            {
                // TODO: Figure out how to eliminate imported targets
                //if (!mstarget.IsImported || _defaultTargets.Contains(mstarget.Name))
                //{
                    MSBuildTarget target = new MSBuildTarget(mstarget.Name);
                    target.Condition = mstarget.Condition;
                    target.Depends = mstarget.DependsOnTargets.Replace(" ", "").Split(';');

                    if (_defaultTargets.Contains(target.Name))
                        target.Default = true;

                    if (_initialTargets.Contains(target.Name))
                        target.Initial = true;

                    Targets.Add(target);
                //}
            }
        }

        private void ParseProperties()
        {
            foreach (ProjectProperty msproperty in _project.Properties)
            {
                // TODO: Should allow the user to toggle shwoing imported properties
                if (!msproperty.Name.StartsWith("_") && !msproperty.IsImported && 
                    !msproperty.IsEnvironmentProperty && !msproperty.IsGlobalProperty && 
                    !msproperty.IsReservedProperty)
                {
                    // TODO: Should allow the user to toggle using the FinalValue on and off, 
                    // because sometimes they may want to change the actual value, and sometimes 
                    // the expanded value
                    MSBuildProperty property = new MSBuildProperty(msproperty.Name, msproperty.EvaluatedValue,
                                                                    string.Empty, string.Empty);
                    Properties.Add(property);
                }
                else if (!msproperty.Name.StartsWith("_") && !msproperty.IsImported &&                    
                    msproperty.IsReservedProperty)
                {
                    MSBuildProperty property = new MSBuildProperty(msproperty.Name, msproperty.EvaluatedValue,
                                                                   "Reserved", string.Empty);
                    Properties.Add(property);
                }
                else if (!msproperty.Name.StartsWith("_") && !msproperty.IsImported &&
                    msproperty.IsEnvironmentProperty)
                {
                    MSBuildProperty property = new MSBuildProperty(msproperty.Name, msproperty.EvaluatedValue,
                                                                   "Environment", string.Empty);
                    Properties.Add(property);
                }
            }
        }
    }
}