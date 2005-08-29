

using System;
using System.Xml;

namespace NAntGui.Core
{
	/// <summary>
	/// Summary description for Property.
	/// </summary>
	public class Property
	{
		private string _name			= "";
		private string _value			= "";
		private string _expandedValue	= "";
		private string _category		= "Global";
		private bool _isReadOnly		= false;
		private static int Count		= 1;
		private int _rank;

		public Property(XmlElement element)
		{
			_name = element.GetAttribute("name");
			_value = element.GetAttribute("value");
			this.SetIsReadonly(element);
			this.SetCategory(element);
			_rank = Count++;
		}

		public Property(string name, string value, string category, bool isReadOnly)
		{
			_name		= name;
			_value		= value;
			_category	= category;
			_isReadOnly	= isReadOnly;
			_rank		= Count++;
		}

		private void SetIsReadonly(XmlElement element)
		{
			if (element.HasAttribute("readonly"))
			{
				_isReadOnly = element.GetAttribute("readonly").Equals("true");
			}
		}

		private void SetCategory(XmlElement element)
		{
			if (element.ParentNode.Name == "target")
			{
				foreach (XmlAttribute lAttribute in element.ParentNode.Attributes)
				{
					if (lAttribute.Name == "name")
					{
						_category = lAttribute.Value;
					}
				}
			}
		}

		#region Properties

		public string Name
		{
			get { return _name; }
		}

		public string Value
		{
			get { return _value; }
			set { _value = value; }
		}
		
		public int Rank
		{
			get { return _rank; }
		}

		public string Category
		{
			get { return _category; }
			set { _category = value; }
		}

		public bool IsReadOnly
		{
			get { return _isReadOnly; }
		}

		public Type Type
		{
			get 
			{
				string lValue = _value.ToLower();
				return (lValue == "true" || lValue == "false") ? typeof (bool) : typeof (string);
			}
		}

		public string ExpandedValue
		{
			get { return _expandedValue; }
			set { _expandedValue = value; }
		}

		public override string ToString()
		{
			return string.Format("{1}: {2}", _name, _value);
		}

		#endregion
	}
}