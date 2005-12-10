using System;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for IDisplayer.
	/// </summary>
	public interface IDisplayer
	{
		string Text{ get; set; }
		event EventHandler TextChanged;
	}
}
