using System;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.HelpMenu
{
	/// <summary>
	/// Summary description for HelpMenu.
	/// </summary>
	public class HelpMenu : MenuCommand
	{
		private NAntContribMenuCommand _nantContrib = new NAntContribMenuCommand();
		private NAntHelpMenuCommand _nant = new NAntHelpMenuCommand();
		private NAntSDKMenuCommand _nantSDK = new NAntSDKMenuCommand();
		private AboutMenuCommand _about = new AboutMenuCommand();

		public HelpMenu()
		{
			this.Description = "MenuCommand";
			this.MenuCommands.AddRange(new MenuCommand[]
				{
					_nant,
					_nantSDK,
					_nantContrib,
					new MenuCommand("-"), 
					_about
				});
			this.Text = "&Help";
		}

		public void SetMediator(MainFormMediator mediator)
		{
			_nant.Mediator			= mediator;
			_nantSDK.Mediator		= mediator;
			_nantContrib.Mediator	= mediator;
			_about.Mediator			= mediator;
		}

		public EventHandler NAntHelp_Click
		{
			set { _nant.Click += value; }
		}

		public EventHandler NAntSDK_Click
		{
			set { _nantSDK.Click += value; }
		}

		public EventHandler NAntContrib_Click
		{
			set { _nantContrib.Click += value; }
		}

		public EventHandler About_Click
		{
			set { _about.Click += value; }
		} 
	}
}