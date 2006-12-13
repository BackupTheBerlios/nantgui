namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for IEditCommands.
	/// </summary>
	public interface IEditCommands
	{
		void Cut();
		void Copy();
		void Paste();
		void SelectAll();
//		void WordWrap();
//		bool WordWrapped { get; }
		void Delete();
	}
}