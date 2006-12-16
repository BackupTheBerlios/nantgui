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

using System.Drawing;
using Crownwood.Magic.Docking;

namespace Crownwood.Magic.Collections
{
	public class HotZoneCollection : CollectionWithEvents
	{
		public HotZone Add(HotZone value)
		{
			// Use base class to process actual collection operation
			List.Add(value);

			return value;
		}

		public void AddRange(HotZone[] values)
		{
			// Use existing method to add each array entry
			foreach (HotZone page in values)
				Add(page);
		}

		public void Remove(HotZone value)
		{
			// Use base class to process actual collection operation
			List.Remove(value);
		}

		public void Insert(int index, HotZone value)
		{
			// Use base class to process actual collection operation
			List.Insert(index, value);
		}

		public bool Contains(HotZone value)
		{
			// Use base class to process actual collection operation
			return List.Contains(value);
		}

		public HotZone this[int index]
		{
			// Use base class to process actual collection operation
			get { return (List[index] as HotZone); }
		}

		public int IndexOf(HotZone value)
		{
			// Find the 0 based index of the requested entry
			return List.IndexOf(value);
		}

		public HotZone Contains(Point pt)
		{
			foreach (HotZone hz in List)
			{
				if (hz.HotArea.Contains(pt))
					return hz;
			}

			return null;
		}
	}
}