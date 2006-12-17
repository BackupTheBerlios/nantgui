// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002-2003
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Crownwood, Bracknell, Berkshire, England and are supplied subject 
//  to licence terms.
// 
//  Magic Version 1.7.4.0 	www.dotnetmagic.com
// *****************************************************************************

using System;
using System.Xml;

namespace Crownwood.Magic.Collections
{
	public class StringCollection : CollectionWithEvents
	{
		public String Add(String value)
		{
			// Use base class to process actual collection operation
			List.Add(value);

			return value;
		}

		public void AddRange(String[] values)
		{
			// Use existing method to add each array entry
			foreach (String item in values)
				Add(item);
		}

		public void Remove(String value)
		{
			// Use base class to process actual collection operation
			List.Remove(value);
		}

		public void Insert(int index, String value)
		{
			// Use base class to process actual collection operation
			List.Insert(index, value);
		}

		public bool Contains(String value)
		{
			// Value comparison
			foreach (String s in List)
				if (value.Equals(s))
					return true;

			return false;
		}

		public bool Contains(StringCollection values)
		{
			foreach (String c in values)
			{
				// Use base class to process actual collection operation
				if (Contains(c))
					return true;
			}

			return false;
		}

		public String this[int index]
		{
			// Use base class to process actual collection operation
			get { return (List[index] as String); }
		}

		public int IndexOf(String value)
		{
			// Find the 0 based index of the requested entry
			return List.IndexOf(value);
		}

		public void SaveToXml(string name, XmlTextWriter xmlOut)
		{
			xmlOut.WriteStartElement(name);
			xmlOut.WriteAttributeString("Count", Count.ToString());

			foreach (String s in List)
			{
				xmlOut.WriteStartElement("Item");
				xmlOut.WriteAttributeString("Name", s);
				xmlOut.WriteEndElement();
			}

			xmlOut.WriteEndElement();
		}

		public void LoadFromXml(string name, XmlTextReader xmlIn)
		{
			// Move to next xml node
			if (!xmlIn.Read())
				throw new ArgumentException("Could not read in next expected node");

			// Check it has the expected name
			if (xmlIn.Name != name)
				throw new ArgumentException("Incorrect node name found");

			Clear();

			// Grab raw position information
			string attrCount = xmlIn.GetAttribute(0);

			// Convert from string to proper types
			int count = int.Parse(attrCount);

			for (int index = 0; index < count; index++)
			{
				// Move to next xml node
				if (!xmlIn.Read())
					throw new ArgumentException("Could not read in next expected node");

				// Check it has the expected name
				if (xmlIn.Name != "Item")
					throw new ArgumentException("Incorrect node name found");

				Add(xmlIn.GetAttribute(0));
			}

			if (count > 0)
			{
				// Move over the end element of the collection
				if (!xmlIn.Read())
					throw new ArgumentException("Could not read in next expected node");

				// Check it has the expected name
				if (xmlIn.Name != name)
					throw new ArgumentException("Incorrect node name found");
			}
		}
	}
}