using System;
using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Menus;
using NAntGui.Framework;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for TargetsTreeView.
	/// </summary>
	public class TargetsTreeView : TreeView, Command
	{
		private ToolTip ToolTip = new ToolTip();
		private PopupMenu _targetsPopupMenu = new PopupMenu();
		private MenuCommand _build = new MenuCommand("&Build");
		private MainFormMediator _mediator;

		public TargetsTreeView()
		{
			this.Initialize();
			this.CreateContextMenu();
		}

		private void Initialize()
		{
			// 
			// TargetsTreeView
			// 
			this.CheckBoxes = true;
			this.Dock = DockStyle.Top;
			this.ImageIndex = -1;
			this.Location = new Point(0, 0);
			this.Name = "TargetsTreeView";
			this.SelectedImageIndex = -1;
			this.Size = new Size(175, 148);
			this.TabIndex = 6;
			this.AfterCheck += new TreeViewEventHandler(this.BuildTreeView_AfterCheck);
			this.MouseUp += new MouseEventHandler(this.BuildTreeView_MouseUp);
			this.MouseMove += new MouseEventHandler(this.BuildTreeView_MouseMove);
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
			TreeNode node = this.GetNodeAt(e.X, e.Y);
			if (node == null || node.Parent == null)
			{
				this.ToolTip.SetToolTip(this, "");
			}
			else
			{
				BuildTarget target = node.Tag as BuildTarget;
				this.ToolTip.SetToolTip(this, target.Description);
			}
		}

		private void CreateContextMenu()
		{
			_build.ImageList = NAntGuiApp.ImageList;
			_build.ImageIndex = 7;
			_targetsPopupMenu.MenuCommands.AddRange(new MenuCommand[] {_build});
		}

		public TargetCollection GetTargets()
		{
			TargetCollection targets = new TargetCollection();
			foreach (TreeNode node in this.Nodes[0].Nodes)
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
			this.Nodes.Clear();

			this.Nodes.Add(new TreeNode(projectName));

			foreach (BuildTarget target in targets)
			{
				this.AddTargetTreeNode(target);
			}

			this.ExpandAll();
		}

		private void AddTargetTreeNode(BuildTarget buildTarget)
		{
			if (!(Settings.HideTargetsWithoutDescription && !HasDescription(buildTarget.Description)))
			{
				string targetName = FormatTargetName(buildTarget.Name, buildTarget.Description);
				TreeNode node = new TreeNode(targetName);
				node.Checked = buildTarget.Default;
				node.Tag = buildTarget;
				this.Nodes[0].Nodes.Add(node);
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

		public void Execute()
		{
			_mediator.BuildClicked();
		}

		public EventHandler BuildClick
		{
			set { _build.Click += value; }
		}

		public MainFormMediator Mediator
		{
			set { _mediator = value; }
		}
	}
}
