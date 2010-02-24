#region Copyleft and Copyright

// NAnt-Gui - Gui frontend to the NAnt .NET build tool
// Copyright (C) 2004-2007 Colin Svingen
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General internal License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General internal License for more details.
//
// You should have received a copy of the GNU General internal License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Colin Svingen (swoogan@gmail.com)

#endregion

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using NAntGui.Framework;
using NAntGui.Gui.Properties;

namespace NAntGui.Gui.Controls
{
	/// <summary>
	/// Summary description for TargetsTreeView.
	/// </summary>
	public partial class TargetsWindow
	{        
        internal event EventHandler<RunEventArgs> RunTarget;

        private string _projectName = "";
		
		public TargetsWindow()
		{
			InitializeComponent();
		}

        internal void SetTargets(List<BuildTarget> targets)
        {
            _treeView.Nodes.Clear();

            _treeView.Nodes.Add(new TreeNode(_projectName));

            foreach (BuildTarget target in targets)
            {
                AddTargetTreeNode(target);
            }

            _treeView.ExpandAll();
        }


        internal List<BuildTarget> SelectedTargets
        {
            get
            {
                List<BuildTarget> targets = new List<BuildTarget>();
                foreach (TreeNode node in _treeView.Nodes[0].Nodes)
                {
                    if (node.Checked)
                    {
                        BuildTarget target = node.Tag as BuildTarget;
                        targets.Add(target);
                    }
                }

                return targets;
            }
            set
            {
                foreach (BuildTarget target in value)
                {
                    SelectTarget(target);
                }

                //_treeView.ExpandAll();
            }
        }

        private void SelectTarget(BuildTarget target)
        {
            foreach (TreeNode node in _treeView.Nodes[0].Nodes)
                if (node.Text == target.Name)
                    node.Checked = true;
        }

        internal string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        #region Event Handlers

        private void _treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = e.Node.Checked;
            }
        }

        private void _treeView_MouseMove(object sender, MouseEventArgs e)
        {
            TreeNode node = _treeView.GetNodeAt(e.X, e.Y);
            if (node == null || node.Parent == null)
            {
                _toolTip.SetToolTip(_treeView, "");
            }
            else
            {
                BuildTarget target = node.Tag as BuildTarget;
                _toolTip.SetToolTip(_treeView, target.Description);
            }
        }

        private void _runMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is TreeNode)
            {
                TreeNode node = sender as TreeNode;
                OnRunTarget(node.Name);
            }
        }

        #endregion

        #region Private Methods

        private void AddTargetTreeNode(BuildTarget buildTarget)
		{			
			if (!Settings.Default.HideTargetsWithoutDescription || HasDescription(buildTarget.Description))
			{
				string targetName = FormatTargetName(buildTarget.Name, buildTarget.Description);
				TreeNode node = new TreeNode(targetName);
				node.Checked = buildTarget.Default;
				node.Tag = buildTarget;
				_treeView.Nodes[0].Nodes.Add(node);
			}
		}

		private static string FormatTargetName(string name, string description)
		{
			const string format = "{0}";
			return HasDescription(description) ? string.Format(format, name, description) : name;
		}

		private static bool HasDescription(string description)
		{
			return description.Length > 0;
		}

		internal void Clear()
		{
			_treeView.Nodes.Clear();
		}
		
        private void OnRunTarget(string target)
        {
            // need to figure out which target to run
            if (RunTarget != null)
                RunTarget(this, new RunEventArgs(target));
        }

        #endregion

    }
}
