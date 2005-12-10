
namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for MainFormMediator.
	/// </summary>
	public class MainFormMediator
	{
		private MainForm _mainForm;

		public MainFormMediator()
		{
		}

		public void Attach(MainForm mainForm)
		{
			_mainForm = mainForm;
		}
	}
}
