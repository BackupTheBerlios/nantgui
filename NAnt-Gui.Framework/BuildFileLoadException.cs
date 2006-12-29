using System;
using System.Xml;

namespace NAntGui.Framework
{
	public class BuildFileLoadException : ApplicationException
	{
		private int _line;
		private int _column;

		public BuildFileLoadException(string s) : base(s) {}

		public BuildFileLoadException(string s, Exception innerException) : 
			base(s, innerException) {}

		public BuildFileLoadException(string s, int line, int column, Exception innerException) : 
			this(s, innerException)
		{
			_line = line;
			_column = column;
		}

		public BuildFileLoadException(string message, XmlException error) : 
			this(message, error.LineNumber, error.LinePosition, error)  {}
	}
}