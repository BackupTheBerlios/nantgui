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
	public class ZoneCollection : CollectionWithEvents
	{
		public Zone Add(Zone value)
		{
			// Use base class to process actual collection operation
			List.Add(value);

			return value;
		}

		public void AddRange(Zone[] values)
		{
			// Use existing method to add each array entry
			foreach (Zone page in values)
				Add(page);
		}

		public void Remove(Zone value)
		{
			// Use base class to process actual collection operation
			List.Remove(value);
		}

		public void Insert(int index, Zone value)
		{
			// Use base class to process actual collection operation
			List.Insert(index, value);
		}

		public bool Contains(Zone value)
		{
			// Use base class to process actual collection operation
			return List.Contains(value);
		}

		public Zone this[int index]
		{
			// Use base class to process actual collection operation
			get { return (List[index] as Zone); }
		}

		public int IndexOf(Zone value)
		{
			// Find the 0 based index of the requested entry
			return List.IndexOf(value);
		}
	}
}