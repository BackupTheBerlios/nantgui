
namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for IClicker.
	/// </summary>
	public interface IClicker
	{
		MainFormMediator Mediator { set; }
		void ExecuteClick();
	}
}
