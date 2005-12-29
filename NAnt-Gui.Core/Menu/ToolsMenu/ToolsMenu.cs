using System;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.ToolsMenu
{
	/// <summary>
	/// Summary description for ToolsMenu.
	/// </summary>
	public class ToolsMenu : MenuCommand
	{
		private OptionsMenuCommand _options = new OptionsMenuCommand();

		public ToolsMenu()
		{
			this.Description = "MenuCommand";
			this.MenuCommands.AddRange(new MenuCommand[]
				{
					_options
				});
			this.Text = "&Tools";
		}

		public void SetMediator(MainFormMediator mediator)
		{
			_options.Mediator		= mediator;
		}

		public EventHandler Options_Click
		{
			set { _options.Click += value; }
		}

	}
}