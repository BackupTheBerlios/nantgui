using System;
using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Menus;
using NAntGui.Framework;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for TargetsTreeView.
	/// </summary>
	public class TargetsTreeView : TreeView
	{
		private EventHandler _buildClick;
		private ToolTip ToolTip = new ToolTip();
		private PopupMenu _targetsPopupMenu = new PopupMenu();
		private ImageList _imageList;

		public TargetsTreeView(EventHandler buildClick)
		{
			_buildClick = buildClick;
			this.Initialize();
			this.CreateContextMenu();
		}

		private void Initialize()
		{
			_imageList = ResourceHelper.LoadBitmapStrip(
				this.GetType(), "NAntGui.Core.Images.MenuItems.bmp",
				new Size(16, 16), new Point(0, 0));
	
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
			MenuCommand build = new MenuCommand("&Build", new EventHandler(_buildClick));
			build.ImageList = _imageList;
			build.ImageIndex = 7;
			_targetsPopupMenu.MenuCommands.AddRange(new MenuCommand[] {build});
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
	}
}
