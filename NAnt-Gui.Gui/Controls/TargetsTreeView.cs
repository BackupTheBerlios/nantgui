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

using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Menus;
using NAntGui.Gui.Controls.Menu.BuildMenu;
using NAntGui.Framework;

namespace NAntGui.Gui.Controls
{
	/// <summary>
	/// Summary description for TargetsTreeView.
	/// </summary>
	public class TargetsTreeView : TreeView
	{
		private ToolTip ToolTip = new ToolTip();
		private PopupMenu _targetsPopupMenu = new PopupMenu();
		private RunMenuCommand _runMenu;

		public TargetsTreeView(MainFormMediator mediator)
		{
			Assert.NotNull(mediator, "mediator");
			_runMenu = new RunMenuCommand(mediator);
			Initialize();
		}

		private void Initialize()
		{
			CheckBoxes = true;
			Dock = DockStyle.Top;
			ImageIndex = -1;
			Location = new Point(0, 0);
			Name = "TargetsTreeView";
			SelectedImageIndex = -1;
			Size = new Size(175, 148);
			TabIndex = 6;
			AfterCheck += new TreeViewEventHandler(BuildTreeView_AfterCheck);
			MouseUp += new MouseEventHandler(BuildTreeView_MouseUp);
			MouseMove += new MouseEventHandler(BuildTreeView_MouseMove);

			_targetsPopupMenu.MenuCommands.AddRange(new MenuCommand[] {_runMenu});
		}

		private void BuildTreeView_AfterCheck(object sender, TreeViewEventArgs e)
		{
			foreach (TreeNode node in e.Node.Nodes)
			{
				node.Checked = e.Node.Checked;
			}
		}

		private void BuildTreeView_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TreeView tree = sender as TreeView;
				TreeNode node = tree.GetNodeAt(e.X, e.Y);
				if (node != null)
				{
					_targetsPopupMenu.TrackPopup(tree.PointToScreen(new Point(e.X, e.Y)));
				}
			}
		}

		private void BuildTreeView_MouseMove(object sender, MouseEventArgs e)
		{
			TreeNode node = GetNodeAt(e.X, e.Y);
			if (node == null || node.Parent == null)
			{
				ToolTip.SetToolTip(this, "");
			}
			else
			{
				BuildTarget target = node.Tag as BuildTarget;
				ToolTip.SetToolTip(this, target.Description);
			}
		}

		public TargetCollection GetTargets()
		{
			TargetCollection targets = new TargetCollection();
			foreach (TreeNode node in Nodes[0].Nodes)
			{
				if (node.Checked)
				{
					BuildTarget target = node.Tag as BuildTarget;
					targets.Add(target);
				}
			}

			return targets;
		}

		public void AddTargets(string projectName, TargetCollection targets)
		{
			Nodes.Clear();

			Nodes.Add(new TreeNode(projectName));

			foreach (BuildTarget target in targets)
			{
				AddTargetTreeNode(target);
			}

			ExpandAll();
		}

		private void AddTargetTreeNode(BuildTarget buildTarget)
		{
			if (!(Settings.HideTargetsWithoutDescription && !HasDescription(buildTarget.Description)))
			{
				string targetName = FormatTargetName(buildTarget.Name, buildTarget.Description);
				TreeNode node = new TreeNode(targetName);
				node.Checked = buildTarget.Default;
				node.Tag = buildTarget;
				Nodes[0].Nodes.Add(node);
			}
		}

		private static string FormatTargetName(string name, string description)
		{
			//const string format = "{0} - {1}";
			const string format = "{0}";
			return HasDescription(description) ? string.Format(format, name, description) : name;
		}

		private static bool HasDescription(string description)
		{
			return description.Length > 0;
		}

		public void Clear()
		{
			Nodes.Clear();
		}
	}
}