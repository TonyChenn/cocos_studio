using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// An immutable stack.
	///
	/// Using 'foreach' on the stack will return the items from top to bottom (in the order they would be popped).
	/// </summary>
	// Token: 0x0200011C RID: 284
	[Serializable]
	public sealed class ImmutableStack<T> : IEnumerable<!0>, IEnumerable
	{
		// Token: 0x06000A12 RID: 2578 RVA: 0x0001E0EA File Offset: 0x0001D0EA
		private ImmutableStack()
		{
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0001E0F2 File Offset: 0x0001D0F2
		private ImmutableStack(T value, ImmutableStack<T> next)
		{
			this.value = value;
			this.next = next;
		}

		/// <summary>
		/// Pushes an item on the stack. This does not modify the stack itself, but returns a new
		/// one with the value pushed.
		/// </summary>
		// Token: 0x06000A14 RID: 2580 RVA: 0x0001E108 File Offset: 0x0001D108
		public ImmutableStack<T> Push(T item)
		{
			return new ImmutableStack<T>(item, this);
		}

		/// <summary>
		/// Gets the item on the top of the stack.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The stack is empty.</exception>
		// Token: 0x06000A15 RID: 2581 RVA: 0x0001E111 File Offset: 0x0001D111
		public T Peek()
		{
			if (this.IsEmpty)
			{
				throw new InvalidOperationException("Operation not valid on empty stack.");
			}
			return this.value;
		}

		/// <summary>
		/// Gets the item on the top of the stack.
		/// Returns <c>default(T)</c> if the stack is empty.
		/// </summary>
		// Token: 0x06000A16 RID: 2582 RVA: 0x0001E12C File Offset: 0x0001D12C
		public T PeekOrDefault()
		{
			return this.value;
		}

		/// <summary>
		/// Gets the stack with the top item removed.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The stack is empty.</exception>
		// Token: 0x06000A17 RID: 2583 RVA: 0x0001E134 File Offset: 0x0001D134
		public ImmutableStack<T> Pop()
		{
			if (this.IsEmpty)
			{
				throw new InvalidOperationException("Operation not valid on empty stack.");
			}
			return this.next;
		}

		/// <summary>
		/// Gets if this stack is empty.
		/// </summary>
		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x0001E14F File Offset: 0x0001D14F
		public bool IsEmpty
		{
			get
			{
				return this.next == null;
			}
		}

		/// <summary>
		/// Gets an enumerator that iterates through the stack top-to-bottom.
		/// </summary>
		// Token: 0x06000A19 RID: 2585 RVA: 0x0001E204 File Offset: 0x0001D204
		public IEnumerator<T> GetEnumerator()
		{
			ImmutableStack<T> t = this;
			while (!t.IsEmpty)
			{
				yield return t.value;
				t = t.next;
			}
			yield break;
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0001E220 File Offset: 0x0001D220
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <inheritdoc />
		// Token: 0x06000A1B RID: 2587 RVA: 0x0001E228 File Offset: 0x0001D228
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("[Stack");
			foreach (T t in this)
			{
				stringBuilder.Append(' ');
				stringBuilder.Append(t);
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		/// <summary>
		/// Gets the empty stack instance.
		/// </summary>
		// Token: 0x04000371 RID: 881
		public static readonly ImmutableStack<T> Empty = new ImmutableStack<T>();

		// Token: 0x04000372 RID: 882
		private readonly T value;

		// Token: 0x04000373 RID: 883
		private readonly ImmutableStack<T> next;
	}
}
