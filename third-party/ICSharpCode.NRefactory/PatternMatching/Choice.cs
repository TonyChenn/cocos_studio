using System;
using System.Collections;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Matches one of several alternatives.
	/// </summary>
	// Token: 0x02000025 RID: 37
	public class Choice : Pattern, IEnumerable<INode>, IEnumerable
	{
		// Token: 0x0600014A RID: 330 RVA: 0x00004849 File Offset: 0x00003849
		public void Add(string name, INode alternative)
		{
			if (alternative == null)
			{
				throw new ArgumentNullException("alternative");
			}
			this.alternatives.Add(new NamedNode(name, alternative));
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000486B File Offset: 0x0000386B
		public void Add(INode alternative)
		{
			if (alternative == null)
			{
				throw new ArgumentNullException("alternative");
			}
			this.alternatives.Add(alternative);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00004888 File Offset: 0x00003888
		public override bool DoMatch(INode other, Match match)
		{
			int checkPoint = match.CheckPoint();
			foreach (INode node in this.alternatives)
			{
				if (node.DoMatch(other, match))
				{
					return true;
				}
				match.RestoreCheckPoint(checkPoint);
			}
			return false;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000048F8 File Offset: 0x000038F8
		IEnumerator<INode> IEnumerable<INode>.GetEnumerator()
		{
			return this.alternatives.GetEnumerator();
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000490A File Offset: 0x0000390A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.alternatives.GetEnumerator();
		}

		// Token: 0x04000042 RID: 66
		private readonly List<INode> alternatives = new List<INode>();
	}
}
