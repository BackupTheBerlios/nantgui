using System;
using System.Drawing;
using System.Windows.Forms;
using Crownwood.Magic.Common;
using Crownwood.Magic.Menus;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for MainMenuCommand.
	/// </summary>
	public class MainMenuControl : MenuControl
	{
		#region MenuCommands
		private MenuCommand ExitMenuCommand;
		private MenuCommand MenuCommand4;
		private MenuCommand MenuCommand5;
		private MenuCommand FileMenuCommand;
		private MenuCommand OpenMenuCommand;
		private MenuCommand SaveMenuCommand;
		private MenuCommand SaveAsMenuCommand;
		private MenuCommand CloseMenuCommand;
		private MenuCommand RecentMenuCommand;
		private MenuCommand ReloadMenuCommand;
		private MenuCommand HelpMenuCommand;
		private MenuCommand AboutMenuCommand;
		private MenuCommand NAntMenuCommand;
		private MenuCommand BuildMenuCommand;
		private MenuCommand EditMainMenuCommand;
		private MenuCommand CopyMenuCommand;
		private MenuCommand SelectAllMenuCommand;
		private MenuCommand ToolsMenuCommand;
		private MenuCommand OptionsMenuCommand;
		private MenuCommand NAntContribMenuCommand;
		private MenuCommand NAntHelpMenuCommand;
		private MenuCommand NAntSDKMenuCommand;
		private MenuCommand MenuCommand1;
		private MenuCommand SaveOutputMenuCommand;
		private MenuCommand WordWrapMenuCommand;
		private MenuCommand ViewMenuCommand;
		private MenuCommand TargetsMenuCommand;
		private MenuCommand PropertiesMenuCommand;
		#endregion

		private ImageList _imageList;

		public MainMenuControl()
		{
			#region Instansiation of MenuCommands
			this.FileMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.OpenMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.SaveMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.SaveAsMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.ReloadMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.CloseMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.SaveOutputMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.MenuCommand4 = new Crownwood.Magic.Menus.MenuCommand();
			this.RecentMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.MenuCommand5 = new Crownwood.Magic.Menus.MenuCommand();
			this.ExitMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.EditMainMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.CopyMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.SelectAllMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.WordWrapMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.ViewMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.TargetsMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.PropertiesMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.NAntMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.BuildMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.ToolsMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.OptionsMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.HelpMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.NAntHelpMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.NAntSDKMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.NAntContribMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			this.MenuCommand1 = new Crownwood.Magic.Menus.MenuCommand();
			this.AboutMenuCommand = new Crownwood.Magic.Menus.MenuCommand();
			#endregion

			this.LoadImageList();
			this.SetProperties();
			this.AssignImages();
		}

		private void LoadImageList()
		{
			_imageList = ResourceHelper.LoadBitmapStrip(
				this.GetType(), "NAntGui.Core.Images.MenuItems.bmp", 
				new Size(16, 16), new Point(0, 0));
		}

		#region Set Properties
		private void SetProperties()
		{
			// 
			// MainMenu
			// 
			this.AnimateStyle = Crownwood.Magic.Menus.Animation.System;
			this.AnimateTime = 100;
			this.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.Direction = Crownwood.Magic.Common.Direction.Horizontal;
			this.Dock = System.Windows.Forms.DockStyle.Top;
			this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((System.Byte) (0)));
			this.HighlightTextColor = System.Drawing.SystemColors.MenuText;
			this.Location = new System.Drawing.Point(0, 0);
			this.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[]
				{
					this.FileMenuCommand,
					this.EditMainMenuCommand,
					this.ViewMenuCommand,
					this.NAntMenuCommand,
					this.ToolsMenuCommand,
					this.HelpMenuCommand
				});
			this.Name = "MainMenu";
			this.Size = new System.Drawing.Size(824, 25);
			this.Style = Crownwood.Magic.Common.VisualStyle.IDE;
			this.TabIndex = 13;
			this.TabStop = false;
			// 
			// FileMenuCommand
			// 
			this.FileMenuCommand.Description = "MenuCommand";
			this.FileMenuCommand.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[]
				{
					this.OpenMenuCommand,
					this.SaveMenuCommand,
					this.SaveAsMenuCommand,
					this.ReloadMenuCommand,
					this.CloseMenuCommand,
					this.SaveOutputMenuCommand,
					this.MenuCommand4,
					this.RecentMenuCommand,
					this.MenuCommand5,
					this.ExitMenuCommand
				});
			this.FileMenuCommand.Text = "&File";
			// 
			// OpenMenuCommand
			// 
			this.OpenMenuCommand.Description = "MenuCommand";
			this.OpenMenuCommand.ImageIndex = 5;
			this.OpenMenuCommand.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.OpenMenuCommand.Text = "&Open";
			// 
			// SaveMenuCommand
			// 
			this.SaveMenuCommand.Description = "MenuCommand";
			this.SaveMenuCommand.ImageIndex = 2;
			this.SaveMenuCommand.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.SaveMenuCommand.Text = "&Save";
			// 
			// SaveAsMenuCommand
			// 
			this.SaveAsMenuCommand.Description = "MenuCommand";
			this.SaveAsMenuCommand.ImageIndex = 2;
			this.SaveAsMenuCommand.Shortcut = System.Windows.Forms.Shortcut.F12;
			this.SaveAsMenuCommand.Text = "Save &As";
			// 
			// ReloadMenuCommand
			// 
			this.ReloadMenuCommand.Description = "MenuCommand";
			this.ReloadMenuCommand.ImageIndex = 4;
			this.ReloadMenuCommand.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
			this.ReloadMenuCommand.Text = "&Reload";
			// 
			// CloseMenuCommand
			// 
			this.CloseMenuCommand.Description = "MenuCommand";
			this.CloseMenuCommand.Shortcut = System.Windows.Forms.Shortcut.CtrlW;
			this.CloseMenuCommand.Text = "&Close";
			// 
			// SaveOutputMenuCommand
			// 
			this.SaveOutputMenuCommand.Description = "MenuCommand";
			this.SaveOutputMenuCommand.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
			this.SaveOutputMenuCommand.Text = "Save O&utput";
			// 
			// MenuCommand4
			// 
			this.MenuCommand4.Description = "MenuCommand";
			this.MenuCommand4.Text = "-";
			// 
			// RecentMenuCommand
			// 
			this.RecentMenuCommand.Description = "MenuCommand";
			this.RecentMenuCommand.Text = "Recent &Files";
			// 
			// MenuCommand5
			// 
			this.MenuCommand5.Description = "MenuCommand";
			this.MenuCommand5.Text = "-";
			// 
			// ExitMenuCommand
			// 
			this.ExitMenuCommand.Description = "MenuCommand";
			this.ExitMenuCommand.Text = "E&xit";
			// 
			// EditMainMenuCommand
			// 
			this.EditMainMenuCommand.Description = "MenuCommand";
			this.EditMainMenuCommand.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[]
				{
					this.CopyMenuCommand,
					this.SelectAllMenuCommand,
					this.WordWrapMenuCommand
				});
			this.EditMainMenuCommand.Text = "&Edit";
			// 
			// CopyMenuCommand
			// 
			this.CopyMenuCommand.Description = "MenuCommand";
			this.CopyMenuCommand.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.CopyMenuCommand.Text = "Cop&y";
			// 
			// SelectAllMenuCommand
			// 
			this.SelectAllMenuCommand.Description = "MenuCommand";
			this.SelectAllMenuCommand.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			this.SelectAllMenuCommand.Text = "Select &All";
			// 
			// WordWrapMenuCommand
			// 
			this.WordWrapMenuCommand.Description = "MenuCommand";
			this.WordWrapMenuCommand.Text = "&Word Wrap";
			// 
			// ViewMenuCommand
			// 
			this.ViewMenuCommand.Description = "MenuCommand";
			this.ViewMenuCommand.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[]
				{
					this.TargetsMenuCommand,
					this.PropertiesMenuCommand
				});
			this.ViewMenuCommand.Text = "&View";
			// 
			// TargetsMenuCommand
			// 
			this.TargetsMenuCommand.Description = "MenuCommand";
			this.TargetsMenuCommand.Text = "&Targets";
			// 
			// PropertiesMenuCommand
			// 
			this.PropertiesMenuCommand.Description = "MenuCommand";
			this.PropertiesMenuCommand.ImageIndex = 0;
			this.PropertiesMenuCommand.Text = "&Properties";
			// 
			// NAntMenuCommand
			// 
			this.NAntMenuCommand.Description = "MenuCommand";
			this.NAntMenuCommand.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[]
				{
					this.BuildMenuCommand
				});
			this.NAntMenuCommand.Text = "&NAnt";
			// 
			// BuildMenuCommand
			// 
			this.BuildMenuCommand.Description = "Builds the current build file";
			this.BuildMenuCommand.ImageIndex = 7;
			this.BuildMenuCommand.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.BuildMenuCommand.Text = "&Build";
			// 
			// ToolsMenuCommand
			// 
			this.ToolsMenuCommand.Description = "MenuCommand";
			this.ToolsMenuCommand.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[]
				{
					this.OptionsMenuCommand
				});
			this.ToolsMenuCommand.Text = "&Tools";
			// 
			// OptionsMenuCommand
			// 
			this.OptionsMenuCommand.Description = "MenuCommand";
			this.OptionsMenuCommand.Text = "&Options";
			// 
			// HelpMenuCommand
			// 
			this.HelpMenuCommand.Description = "MenuCommand";
			this.HelpMenuCommand.MenuCommands.AddRange(new Crownwood.Magic.Menus.MenuCommand[]
				{
					this.NAntHelpMenuCommand,
					this.NAntSDKMenuCommand,
					this.NAntContribMenuCommand,
					this.MenuCommand1,
					this.AboutMenuCommand
				});
			this.HelpMenuCommand.Text = "&Help";
			// 
			// NAntHelpMenuCommand
			// 
			this.NAntHelpMenuCommand.Description = "MenuCommand";
			this.NAntHelpMenuCommand.Text = "NAnt &Help";
			// 
			// NAntSDKMenuCommand
			// 
			this.NAntSDKMenuCommand.Description = "MenuCommand";
			this.NAntSDKMenuCommand.Text = "NAnt &SDK Help";
			// 
			// NAntContribMenuCommand
			// 
			this.NAntContribMenuCommand.Description = "MenuCommand";
			this.NAntContribMenuCommand.Text = "NAnt-&Contrib Help";
			// 
			// MenuCommand1
			// 
			this.MenuCommand1.Description = "MenuCommand";
			this.MenuCommand1.Text = "-";
			// 
			// AboutMenuCommand
			// 
			this.AboutMenuCommand.Description = "MenuCommand";
			this.AboutMenuCommand.Text = "&About NAnt-Gui";
		}
		#endregion

		private void AssignImages()
		{
			this.OpenMenuCommand.ImageList			= _imageList;
			this.SaveMenuCommand.ImageList			= _imageList;
			this.ReloadMenuCommand.ImageList		= _imageList;
			this.PropertiesMenuCommand.ImageList	= _imageList;
			this.BuildMenuCommand.ImageList			= _imageList;
		}

		public void Enable()
		{
			this.ReloadMenuCommand.Enabled	= true;
			this.SaveMenuCommand.Enabled	= true;
			this.SaveAsMenuCommand.Enabled	= true;
			this.BuildMenuCommand.Enabled	= true;
			this.CloseMenuCommand.Enabled	= true;
		}

		public void Disable()
		{
			this.ReloadMenuCommand.Enabled	= false;
			this.SaveMenuCommand.Enabled	= false;
			this.SaveAsMenuCommand.Enabled	= false;
			this.BuildMenuCommand.Enabled	= false;
			this.CloseMenuCommand.Enabled	= false;
		}

		public void ClearRecentItems()
		{
			this.RecentMenuCommand.MenuCommands.Clear();
		}

		public void AddRecentItem(MenuCommand item)
		{
			this.RecentMenuCommand.MenuCommands.Add(item);
		}

		#region Events

		public EventHandler Open_Click
		{
			set { this.OpenMenuCommand.Click += value; }
		}
		public EventHandler Save_Click
		{
			set { this.SaveMenuCommand.Click += value; }
		}
		public EventHandler SaveAs_Click
		{
			set { this.SaveAsMenuCommand.Click += value; }
		}
		public EventHandler Reload_Click
		{
			set { this.ReloadMenuCommand.Click += value; }
		}
		public EventHandler Close_Click
		{
			set { this.CloseMenuCommand.Click += value; }
		}
		public EventHandler SaveOutput_Click
		{
			set { this.SaveOutputMenuCommand.Click += value; }
		}
		public EventHandler Exit_Click
		{
			set { this.ExitMenuCommand.Click += value; }
		}
		public EventHandler Copy_Click
		{
			set { this.CopyMenuCommand.Click += value; }
		}
		public EventHandler SelectAll_Click
		{
			set { this.SelectAllMenuCommand.Click += value; }
		}
		public EventHandler WordWrap_Click
		{
			set { this.WordWrapMenuCommand.Click += value; }
		}
		public EventHandler Targets_Click
		{
			set { this.TargetsMenuCommand.Click += value; }
		}
		public EventHandler Properties_Click
		{
			set { this.PropertiesMenuCommand.Click += value; }
		}
		public EventHandler Build_Click
		{
			set { this.BuildMenuCommand.Click += value; }
		}
		public EventHandler Options_Click
		{
			set { this.OptionsMenuCommand.Click += value; }
		}
		public EventHandler NAntHelp_Click
		{
			set { this.NAntHelpMenuCommand.Click += value; }
		}
		public EventHandler NAntSDK_Click
		{
			set { this.NAntSDKMenuCommand.Click += value; }
		}
		public EventHandler NAntContrib_Click
		{
			set { this.NAntContribMenuCommand.Click += value; }
		}
		public EventHandler About_Click
		{
			set { this.AboutMenuCommand.Click += value; }
		}

		#endregion

		public bool WordWrapChecked
		{
			get { return this.WordWrapMenuCommand.Checked; }
			set { this.WordWrapMenuCommand.Checked = value; }
		}
	}
}
