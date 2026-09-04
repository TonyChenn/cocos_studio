using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// Static helper methods for traversing trees.
	/// </summary>
	// Token: 0x02000126 RID: 294
	public static class TreeTraversal
	{
		/// <summary>
		/// Converts a tree data structure into a flat list by traversing it in pre-order.
		/// </summary>
		/// <param name="root">The root element of the tree.</param>
		/// <param name="recursion">The function that gets the children of an element.</param>
		/// <returns>Iterator that enumerates the tree structure in pre-order.</returns>
		// Token: 0x06000A5F RID: 2655 RVA: 0x0001EBEC File Offset: 0x0001DBEC
		public static IEnumerable<T> PreOrder<T>(T root, Func<T, IEnumerable<T>> recursion)
		{
			return TreeTraversal.PreOrder<T>(new T[]
			{
				root
			}, recursion);
		}

		/// <summary>
		/// Converts a tree data structure into a flat list by traversing it in pre-order.
		/// </summary>
		/// <param name="input">The root elements of the forest.</param>
		/// <param name="recursion">The function that gets the children of an element.</param>
		/// <returns>Iterator that enumerates the tree structure in pre-order.</returns>
		// Token: 0x06000A60 RID: 2656 RVA: 0x0001EE30 File Offset: 0x0001DE30
		public static IEnumerable<T> PreOrder<T>(IEnumerable<T> input, Func<T, IEnumerable<T>> recursion)
		{
			Stack<IEnumerator<T>> stack = new Stack<IEnumerator<T>>();
			try
			{
				stack.Push(input.GetEnumerator());
				while (stack.Count > 0)
				{
					while (stack.Peek().MoveNext())
					{
						T element = stack.Peek().Current;
						yield return element;
						IEnumerable<T> children = recursion(element);
						if (children != null)
						{
							stack.Push(children.GetEnumerator());
						}
					}
					stack.Pop().Dispose();
				}
			}
			finally
			{
				while (stack.Count > 0)
				{
					stack.Pop().Dispose();
				}
			}
			yield break;
		}

		/// <summary>
		/// Converts a tree data structure into a flat list by traversing it in post-order.
		/// </summary>
		/// <param name="root">The root element of the tree.</param>
		/// <param name="recursion">The function that gets the children of an element.</param>
		/// <returns>Iterator that enumerates the tree structure in post-order.</returns>
		// Token: 0x06000A61 RID: 2657 RVA: 0x0001EE54 File Offset: 0x0001DE54
		public static IEnumerable<T> PostOrder<T>(T root, Func<T, IEnumerable<T>> recursion)
		{
			return TreeTraversal.PostOrder<T>(new T[]
			{
				root
			}, recursion);
		}

		/// <summary>
		/// Converts a tree data structure into a flat list by traversing it in post-order.
		/// </summary>
		/// <param name="input">The root elements of the forest.</param>
		/// <param name="recursion">The function that gets the children of an element.</param>
		/// <returns>Iterator that enumerates the tree structure in post-order.</returns>
		// Token: 0x06000A62 RID: 2658 RVA: 0x0001F0E4 File Offset: 0x0001E0E4
		public static IEnumerable<T> PostOrder<T>(IEnumerable<T> input, Func<T, IEnumerable<T>> recursion)
		{
			Stack<IEnumerator<T>> stack = new Stack<IEnumerator<T>>();
			try
			{
				stack.Push(input.GetEnumerator());
				while (stack.Count > 0)
				{
					while (stack.Peek().MoveNext())
					{
						T element = stack.Peek().Current;
						IEnumerable<T> children = recursion(element);
						if (children != null)
						{
							stack.Push(children.GetEnumerator());
						}
						else
						{
							yield return element;
						}
					}
					stack.Pop().Dispose();
					if (stack.Count > 0)
					{
						yield return stack.Peek().Current;
					}
				}
			}
			finally
			{
				while (stack.Count > 0)
				{
					stack.Pop().Dispose();
				}
			}
			yield break;
		}
	}
}
