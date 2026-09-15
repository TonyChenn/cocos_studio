using System;
using System.Collections;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory
{
	[Serializable]
	public sealed class EmptyList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IEnumerator<T>, IDisposable, IEnumerator
	{
		private EmptyList()
		{
		}

		public T this[int index]
		{
			get
			{
				throw new ArgumentOutOfRangeException("index");
			}
			set
			{
				throw new ArgumentOutOfRangeException("index");
			}
		}

		public int Count
		{
			get
			{
				return 0;
			}
		}

		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		int IList<T>.IndexOf(T item)
		{
			return -1;
		}

		void IList<T>.Insert(int index, T item)
		{
			throw new NotSupportedException();
		}

		void IList<T>.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		void ICollection<T>.Add(T item)
		{
			throw new NotSupportedException();
		}

		void ICollection<T>.Clear()
		{
		}

		bool ICollection<T>.Contains(T item)
		{
			return false;
		}

		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
		}

		bool ICollection<T>.Remove(T item)
		{
			return false;
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		T IEnumerator<T>.Current
		{
			get
			{
				return default(T);
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return default(T);
			}
		}

		void IDisposable.Dispose()
		{
		}

		bool IEnumerator.MoveNext()
		{
			return false;
		}

		void IEnumerator.Reset()
		{
		}

		public static readonly EmptyList<T> Instance = new EmptyList<T>();
	}
}
