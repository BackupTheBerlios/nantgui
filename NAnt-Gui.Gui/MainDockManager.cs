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

using System;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;
using NAntGui.Framework;
using NAntGui.Gui.Controls;
using TabControl = Crownwood.Magic.Controls.TabControl;

namespace NAntGui.Gui
{
	/// <summary>
	/// Summary description for MainDockManager.
	/// </summary>
	public class MainDockManager
	{
		private static readonly string DOCKING_CONFIG = Application.StartupPath + "\\MainFormDocking.config";

		private DockingManager _dockManager;
		private WindowContent _targetWindowContent;
		private Content _targetsContent;
		private Content _propertiesContent;
		private Content _outputContent;
		private OutputBox _outputBox;

		public MainDockManager(MainForm mainForm, ScriptTabs scriptTabs, TargetsTreeView targetsTree,
		                       OutputBox outputBox, MainPropertyGrid propertyGrid, MainStatusBar statusBar)
		{
			Assert.NotNull(outputBox, "outputBox");
			_outputBox = outputBox;
			// Create the object that manages the docking state
			_dockManager = new DockingManager(mainForm, VisualStyle.IDE);

			scriptTabs.SetToDockManager(this);

			_targetsContent = _dockManager.Contents.Add(targetsTree, "Targets");
			_outputContent = _dockManager.Contents.Add(outputBox, "Output");
			_propertiesContent = _dockManager.Contents.Add(propertyGrid, "Properties");

			_targetsContent.ImageList = NAntGuiApp.ImageList;
			_targetsContent.ImageIndex = 9;

			_propertiesContent.ImageList = NAntGuiApp.ImageList;
			_propertiesContent.ImageIndex = 0;

			_outputContent.ImageList = NAntGuiApp.ImageList;
			_outputContent.ImageIndex = 6;

			// Request a new Docking window be created for the above Content on the left edge
			_targetWindowContent = _dockManager.AddContentWithState(_targetsContent, State.DockLeft);
			_dockManager.AddContentToZone(_propertiesContent, _targetWindowContent.ParentZone, 1);

			_dockManager.AddContentWithState(_outputContent, State.DockBottom);
			
			_dockManager.OuterControl = statusBar;

			_dockManager.LoadConfigFromFile(DOCKING_CONFIG);
		}

		public void SaveConfig()
		{
			_dockManager.SaveConfigToFile(DOCKING_CONFIG);
		}

		public void ShowTargets()
		{
			_dockManager.BringAutoHideIntoView(_targetsContent);
			_dockManager.ShowContent(_targetsContent);
		}

		public void ShowProperties()
		{
			_dockManager.BringAutoHideIntoView(_propertiesContent);
			_dockManager.ShowContent(_propertiesContent);
		}

		public void ShowOutput()
		{
			_dockManager.BringAutoHideIntoView(_outputContent);
			_dockManager.ShowContent(_outputContent);
		}

		public void AddTabControl(TabControl tabControl)
		{
			_dockManager.InnerControl = tabControl;
		}

		private void AutoHide(object sender, EventArgs e)
		{
			_outputBox.ReHightlight();
		}

		// commented because it doesn't work
//		private void ContentChanged(Content c, EventArgs cea)
//		{
//			if (c == _outputContent)
//			{
//				// have to do this because the highlighting is undone when
//				// the toolbox is reshown.  Didn't feel like wasting any time
//				// figuring out exactly why that happens.
//				_outputBox.ReHightlight();
//			}
//		}
	}
}