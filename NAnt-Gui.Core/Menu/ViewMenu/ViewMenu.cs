using System;
using Crownwood.Magic.Menus;

namespace NAntGui.Core.Menu.ViewMenu
{
	/// <summary>
	/// Summary description for ViewMenu.
	/// </summary>
	public class ViewMenu : MenuCommand
	{
		private TargetsMenuCommand _targets = new TargetsMenuCommand();
		private PropertiesMenuCommand _properties = new PropertiesMenuCommand();
		private OutputMenuCommand _output = new OutputMenuCommand();

		public ViewMenu()
		{
			this.Description = "MenuCommand";
			this.MenuCommands.AddRange(new MenuCommand[]
				{
					_targets,
					_properties,
					_output
				});
			this.Text = "&View";	//
		}

		public void SetMediator(MainFormMediator mediator)
		{
			_targets.Mediator		= mediator;
			_properties.Mediator	= mediator;
			_output.Mediator		= mediator;
		}

		public EventHandler Targets_Click
		{
			set { _targets.Click += value; }
		}

		public EventHandler Properties_Click
		{
			set { _properties.Click += value; }
		}

		public EventHandler Output_Click
		{
			set { _output.Click += value; }
		}

	}
}