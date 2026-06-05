using System;

namespace MonoDevelop.Core.Text
{
	// Token: 0x0200020A RID: 522
	public abstract class StringMatcher
	{
		// Token: 0x060013C6 RID: 5062 RVA: 0x00051A81 File Offset: 0x0004FC81
		public static StringMatcher GetMatcher(string filter, bool matchWordStartsOnly)
		{
			if (matchWordStartsOnly)
			{
				return new BacktrackingStringMatcher(filter);
			}
			return new LaneStringMatcher(filter);
		}

		// Token: 0x060013C7 RID: 5063
		public abstract bool CalcMatchRank(string name, out int matchRank);

		// Token: 0x060013C8 RID: 5064
		public abstract bool IsMatch(string name);

		// Token: 0x060013C9 RID: 5065
		public abstract int[] GetMatch(string text);
	}
}
