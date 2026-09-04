using System;
using System.Linq;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Matches the last entry in the specified named group.
	/// </summary>
	// Token: 0x02000027 RID: 39
	public class Backreference : Pattern
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00004964 File Offset: 0x00003964
		public string ReferencedGroupName
		{
			get
			{
				return this.referencedGroupName;
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000496C File Offset: 0x0000396C
		public Backreference(string referencedGroupName)
		{
			if (referencedGroupName == null)
			{
				throw new ArgumentNullException("referencedGroupName");
			}
			this.referencedGroupName = referencedGroupName;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000498C File Offset: 0x0000398C
		public override bool DoMatch(INode other, Match match)
		{
			INode node = match.Get(this.referencedGroupName).Last<INode>();
			return (node == null && other == null) || node.IsMatch(other);
		}

		// Token: 0x04000044 RID: 68
		private readonly string referencedGroupName;
	}
}
