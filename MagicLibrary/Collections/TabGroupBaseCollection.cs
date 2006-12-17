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
using Crownwood.Magic.Controls;

namespace Crownwood.Magic.Collections
{
	public class TabGroupBaseCollection : CollectionWithEvents
	{
		public TabGroupBase Add(TabGroupBase value)
		{
			// Use base class to process actual collection operation
			List.Add(value);

			return value;
		}

		public void AddRange(TabGroupBase[] values)
		{
			// Use existing method to add each array entry
			foreach (TabGroupBase item in values)
				Add(item);
		}

		public void Remove(TabGroupBase value)
		{
			// Use base class to process actual collection operation
			List.Remove(value);
		}

		public void Insert(int index, TabGroupBase value)
		{
			// Use base class to process actual collection operation
			List.Insert(index, value);
		}

		public bool Contains(TabGroupBase value)
		{
			// Value comparison
			foreach (String s in List)
				if (value.Equals(s))
					return true;

			return false;
		}

		public bool Contains(TabGroupBaseCollection values)
		{
			foreach (TabGroupBase c in values)
			{
				// Use base class to process actual collection operation
				if (Contains(c))
					return true;
			}

			return false;
		}

		public TabGroupBase this[int index]
		{
			// Use base class to process actual collection operation
			get { return (List[index] as TabGroupBase); }
			set { List[index] = value; }
		}

		public int IndexOf(TabGroupBase value)
		{
			// Find the 0 based index of the requested entry
			return List.IndexOf(value);
		}
	}
}