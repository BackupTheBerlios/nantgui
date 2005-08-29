using System.Collections;

namespace NAntGui.Core
{
	public class DependsCollection
	{
		private ArrayList mDepends;

		public DependsCollection()
		{
			this.mDepends = new ArrayList();
		}

		public DependsCollection(int pSize)
		{
			this.mDepends = new ArrayList(pSize);
		}

		public void Add(string[] pDepends)
		{
			this.mDepends.AddRange(pDepends);
		}

		public IEnumerator GetEnumerator()
		{
			return this.mDepends.GetEnumerator();
		}

		public bool Contains(string pTarget)
		{
			return this.mDepends.Contains(pTarget);
		}
	}
}