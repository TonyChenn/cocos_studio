using System;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Matches any node.
	/// </summary>
	/// <remarks>Does not match null nodes.</remarks>
	// Token: 0x02000146 RID: 326
	public class AnyNodeOrNull : Pattern
	{
		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06000B2F RID: 2863 RVA: 0x00022656 File Offset: 0x00021656
		public string GroupName
		{
			get
			{
				return this.groupName;
			}
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0002265E File Offset: 0x0002165E
		public AnyNodeOrNull(string groupName = null)
		{
			this.groupName = groupName;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0002266D File Offset: 0x0002166D
		public override bool DoMatch(INode other, Match match)
		{
			if (other == null)
			{
				match.AddNull(this.groupName);
			}
			else
			{
				match.Add(this.groupName, other);
			}
			return true;
		}

		// Token: 0x040003E6 RID: 998
		private readonly string groupName;
	}
}
