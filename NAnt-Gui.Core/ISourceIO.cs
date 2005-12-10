namespace NAntGui.Core
{
	public interface ISourceIO
	{
		void LoadFile(string filename);
		void SaveFile(string filename);
	}
}