
namespace NAntGui.Framework
{
	/// <summary>
	/// Summary description for IBuildRunner.
	/// </summary>
	public interface IBuildRunner
	{
		VoidVoid BuildFinished { set; }
		IProject BuildScript { get; }
		void Run();
	}
}
