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

using Crownwood.Magic.Controls;

namespace Crownwood.Magic.Collections
{
	public class TabPageCollection : CollectionWithEvents
	{
		public TabPage Add(TabPage value)
		{
			// Use base class to process actual collection operation
			List.Add(value);

			return value;
		}

		public void AddRange(TabPage[] values)
		{
			// Use existing method to add each array entry
			foreach (TabPage page in values)
				Add(page);
		}

		public void Remove(TabPage value)
		{
			// Use base class to process actual collection operation
			List.Remove(value);
		}

		public void Insert(int index, TabPage value)
		{
			// Use base class to process actual collection operation
			List.Insert(index, value);
		}

		public bool Contains(TabPage value)
		{
			// Use base class to process actual collection operation
			return List.Contains(value);
		}

		public TabPage this[int index]
		{
			// Use base class to process actual collection operation
			get { return (List[index] as TabPage); }
		}

		public TabPage this[string title]
		{
			get
			{
				// Search for a Page with a matching title
				foreach (TabPage page in List)
					if (page.Title == title)
						return page;

				return null;
			}
		}

		public int IndexOf(TabPage value)
		{
			// Find the 0 based index of the requested entry
			return List.IndexOf(value);
		}
	}
}