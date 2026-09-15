using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.PatternMatching
{
	/// <summary>
	/// Base class for all patterns.
	/// </summary>
	public abstract class Pattern : INode
	{
		public static bool MatchString(string pattern, string text)
		{
			return pattern == Pattern.AnyString || pattern == text;
		}

		bool INode.IsNull
		{
			get
			{
				return false;
			}
		}

		Role INode.Role
		{
			get
			{
				return null;
			}
		}

		INode INode.NextSibling
		{
			get
			{
				return null;
			}
		}

		INode INode.FirstChild
		{
			get
			{
				return null;
			}
		}

		public abstract bool DoMatch(INode other, Match match);

		public virtual bool DoMatchCollection(Role role, INode pos, Match match, BacktrackingInfo backtrackingInfo)
		{
			return this.DoMatch(pos, match);
		}

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
		public static readonly string AnyString = "$any$";

		internal struct PossibleMatch
		{
			public PossibleMatch(INode nextOther, int checkpoint)
			{
				this.NextOther = nextOther;
				this.Checkpoint = checkpoint;
			}

			public readonly INode NextOther;

			public readonly int Checkpoint;
		}
	}
}
