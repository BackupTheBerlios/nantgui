using System;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.BuildMenu
{
	/// <summary>
	/// Summary description for BuildMenu.
	/// </summary>
	public class BuildMenu : MenuCommand
	{
		private RunMenuCommand _runMenu = new RunMenuCommand();

		public BuildMenu()
		{
			this.Description = "MenuCommand";
			this.MenuCommands.AddRange(new MenuCommand[]
				{
					_runMenu
				});
			this.Text = "&Build";
		}

		public void SetMediator(MainFormMediator mediator)
		{
			_runMenu.Mediator = mediator;
		}

		public void Enable()
		{
			_runMenu.Enabled = true;
		}

		public void Disable()
		{
			_runMenu.Enabled = false;
		}

		public EventHandler Build_Click
		{
			set { _runMenu.Click += value; }
		}
	}
}