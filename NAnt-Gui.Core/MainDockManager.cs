using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Docking;

namespace NAntGui.Core
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

		public MainDockManager(MainForm mainForm, SourceTabControl sourceTabs, TargetsTreeView targetsTree, 
			OutputBox outputBox, MainPropertyGrid propertyGrid, MainStatusBar statusBar)
		{
			// Create the object that manages the docking state
			_dockManager = new DockingManager(mainForm, VisualStyle.IDE);
			// Ensure that the RichTextBox is always the innermost control
			_dockManager.InnerControl = sourceTabs.Tabs;

			_targetsContent		= _dockManager.Contents.Add(targetsTree, "Targets");
			_outputContent		= _dockManager.Contents.Add(outputBox, "Output");
			_propertiesContent	= _dockManager.Contents.Add(propertyGrid, "Properties");

			_targetsContent.ImageList = NAntGuiApp.ImageList;
			_targetsContent.ImageIndex = 9;

			_propertiesContent.ImageList = NAntGuiApp.ImageList;
			_propertiesContent.ImageIndex = 0;

			_outputContent.ImageList = NAntGuiApp.ImageList;
			_outputContent.ImageIndex = 6;

			// Request a new Docking window be created for the above Content on the left edge
			_targetWindowContent = _dockManager.AddContentWithState(_targetsContent, State.DockLeft) as WindowContent;
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
	}
}
