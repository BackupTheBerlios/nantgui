using System;
using Crownwood.Magic.Menus;
using ICSharpCode.TextEditor;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for ScriptEditorContextMenu.
	/// </summary>
	public class ScriptEditorContextMenu : PopupMenu
	{
		public ScriptEditorContextMenu(TextAreaClipboardHandler clipboard)
		{
			MenuCommand copy = 
				new MenuCommand("Cop&y", new EventHandler(clipboard.Copy));
			MenuCommand cut = 
				new MenuCommand("Cu&t", new EventHandler(clipboard.Cut));
			MenuCommand paste = 
				new MenuCommand("&Paste", new EventHandler(clipboard.Paste));
			MenuCommand delete = 
				new MenuCommand("&Delete", new EventHandler(clipboard.Delete));
			MenuCommand spacer = new MenuCommand("-");
			MenuCommand selectAll = 
				new MenuCommand("Select &All", new EventHandler(clipboard.SelectAll));

			this.MenuCommands.AddRange(
				new MenuCommand[] {copy, cut, paste, delete, spacer, selectAll});
		}
	}
}
