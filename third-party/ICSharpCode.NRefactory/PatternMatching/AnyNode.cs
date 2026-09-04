using System;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Matches any node.
	/// </summary>
	/// <remarks>Does not match null nodes.</remarks>
	// Token: 0x02000026 RID: 38
	public class AnyNode : Pattern
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000150 RID: 336 RVA: 0x0000492F File Offset: 0x0000392F
		public string GroupName
		{
			get
			{
				return this.groupName;
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00004937 File Offset: 0x00003937
		public AnyNode(string groupName = null)
		{
			this.groupName = groupName;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00004946 File Offset: 0x00003946
		public override bool DoMatch(INode other, Match match)
		{
			match.Add(this.groupName, other);
			return other != null && !other.IsNull;
		}

		// Token: 0x04000043 RID: 67
		private readonly string groupName;
	}
}
