using System.Windows.Forms;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for IDragDropper.
	/// </summary>
	public interface IDragDropper
	{
		MainFormMediator Mediator { set; }
		void ExecuteDragEnter(DragEventArgs e);
		void ExecuteDragDrop(DragEventArgs e);
	}
}
