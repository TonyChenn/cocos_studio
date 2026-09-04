using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Base class for all patterns.
	/// </summary>
	// Token: 0x02000023 RID: 35
	public abstract class Pattern : INode
	{
		// Token: 0x0600013F RID: 319 RVA: 0x000046E7 File Offset: 0x000036E7
		public static bool MatchString(string pattern, string text)
		{
			return pattern == Pattern.AnyString || pattern == text;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000140 RID: 320 RVA: 0x000046FF File Offset: 0x000036FF
		bool INode.IsNull
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00004702 File Offset: 0x00003702
		Role INode.Role
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00004705 File Offset: 0x00003705
		INode INode.NextSibling
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00004708 File Offset: 0x00003708
		INode INode.FirstChild
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000144 RID: 324
		public abstract bool DoMatch(INode other, Match match);

		// Token: 0x06000145 RID: 325 RVA: 0x0000470B File Offset: 0x0000370B
		public virtual bool DoMatchCollection(Role role, INode pos, Match match, BacktrackingInfo backtrackingInfo)
		{
			return this.DoMatch(pos, match);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00004718 File Offset: 0x00003718
		public static bool DoMatchCollection(Role role, INode firstPatternChild, INode firstOtherChild, Match match)
		{
			BacktrackingInfo backtrackingInfo = new BacktrackingInfo();
			Stack<INode> stack = new Stack<INode>();
			Stack<Pattern.PossibleMatch> backtrackingStack = backtrackingInfo.backtrackingStack;
			stack.Push(firstPatternChild);
			backtrackingStack.Push(new Pattern.PossibleMatch(firstOtherChild, match.CheckPoint()));
			while (backtrackingStack.Count > 0)
			{
				INode node = stack.Pop();
				INode node2 = backtrackingStack.Peek().NextOther;
				match.RestoreCheckPoint(backtrackingStack.Pop().Checkpoint);
				bool flag = true;
				while (node != null)
				{
					if (!flag)
					{
						break;
					}
					while (node != null)
					{
						if (node.Role == role)
						{
							break;
						}
						node = node.NextSibling;
					}
					while (node2 != null && node2.Role != role)
					{
						node2 = node2.NextSibling;
					}
					if (node == null)
					{
						break;
					}
					flag = node.DoMatchCollection(role, node2, match, backtrackingInfo);
					while (backtrackingStack.Count > stack.Count)
					{
						stack.Push(node.NextSibling);
					}
					node = node.NextSibling;
					if (node2 != null)
					{
						node2 = node2.NextSibling;
					}
				}
				while (node2 != null && node2.Role != role)
				{
					node2 = node2.NextSibling;
				}
				if (flag && node2 == null)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Gets the string that matches any string.
		/// </summary>
		// Token: 0x0400003F RID: 63
		public static readonly string AnyString = "$any$";

		// Token: 0x02000024 RID: 36
		internal struct PossibleMatch
		{
			// Token: 0x06000149 RID: 329 RVA: 0x00004839 File Offset: 0x00003839
			public PossibleMatch(INode nextOther, int checkpoint)
			{
				this.NextOther = nextOther;
				this.Checkpoint = checkpoint;
			}

			// Token: 0x04000040 RID: 64
			public readonly INode NextOther;

			// Token: 0x04000041 RID: 65
			public readonly int Checkpoint;
		}
	}
}
