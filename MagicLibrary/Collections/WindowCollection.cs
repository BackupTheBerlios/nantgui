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

using Crownwood.Magic.Docking;

namespace Crownwood.Magic.Collections
{
	public class WindowCollection : CollectionWithEvents
	{
		public Window Add(Window value)
		{
			// Use base class to process actual collection operation
			List.Add(value);

			return value;
		}

		public void AddRange(Window[] values)
		{
			// Use existing method to add each array entry
			foreach (Window page in values)
				Add(page);
		}

		public void Remove(Window value)
		{
			// Use base class to process actual collection operation
			List.Remove(value);
		}

		public void Insert(int index, Window value)
		{
			// Use base class to process actual collection operation
			List.Insert(index, value);
		}

		public bool Contains(Window value)
		{
			// Use base class to process actual collection operation
			return List.Contains(value);
		}

		public Window this[int index]
		{
			// Use base class to process actual collection operation
			get { return (List[index] as Window); }
		}

		public int IndexOf(Window value)
		{
			// Find the 0 based index of the requested entry
			return List.IndexOf(value);
		}
	}
}