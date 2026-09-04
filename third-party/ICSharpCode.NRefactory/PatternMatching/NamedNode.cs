using System;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Represents a named node within a pattern.
	/// </summary>
	// Token: 0x0200002A RID: 42
	public class NamedNode : Pattern
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00004F1D File Offset: 0x00003F1D
		public string GroupName
		{
			get
			{
				return this.groupName;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00004F25 File Offset: 0x00003F25
		public INode ChildNode
		{
			get
			{
				return this.childNode;
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00004F2D File Offset: 0x00003F2D
		public NamedNode(string groupName, INode childNode)
		{
			if (childNode == null)
			{
				throw new ArgumentNullException("childNode");
			}
			this.groupName = groupName;
			this.childNode = childNode;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00004F51 File Offset: 0x00003F51
		public override bool DoMatch(INode other, Match match)
		{
			match.Add(this.groupName, other);
			return this.childNode.DoMatch(other, match);
		}

		// Token: 0x04000046 RID: 70
		private readonly string groupName;

		// Token: 0x04000047 RID: 71
		private readonly INode childNode;
	}
}
