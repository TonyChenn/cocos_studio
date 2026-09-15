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
	[Serializable]
	public sealed class ImmutableStack<T> : IEnumerable<T>, IEnumerable
	{
		private ImmutableStack()
		{
		}

		private ImmutableStack(T value, ImmutableStack<T> next)
		{
			this.value = value;
			this.next = next;
		}

		/// <summary>
		/// Pushes an item on the stack. This does not modify the stack itself, but returns a new
		/// one with the value pushed.
		/// </summary>
		public ImmutableStack<T> Push(T item)
		{
			return new ImmutableStack<T>(item, this);
		}

		/// <summary>
		/// Gets the item on the top of the stack.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The stack is empty.</exception>
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
		public T PeekOrDefault()
		{
			return this.value;
		}

		/// <summary>
		/// Gets the stack with the top item removed.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The stack is empty.</exception>
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

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <inheritdoc />
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
		public static readonly ImmutableStack<T> Empty = new ImmutableStack<T>();

		private readonly T value;

		private readonly ImmutableStack<T> next;
	}
}
