
namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Command.
	/// </summary>
	public interface Command
	{
		MainFormMediator Mediator { set; }
		void Execute();
	}
}
