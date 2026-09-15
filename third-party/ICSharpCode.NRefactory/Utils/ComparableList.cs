using System;
using System.Collections;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// A list that can be compared to other ComparableLists for equality.
	/// Can not be used to store null values.
	/// </summary>
	public sealed class ComparableList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IEquatable<ComparableList<T>>
	{
		public ComparableList()
		{
			this.elements = new List<T>();
		}

		public ComparableList(IEnumerable<T> values)
		{
			this.elements = new List<T>(values);
		}

		public int IndexOf(T item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return this.elements.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			this.elements.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			this.elements.RemoveAt(index);
		}

		public T this[int index]
		{
			get
			{
				return this.elements[index];
			}
			set
			{
				this.elements[index] = value;
			}
		}

		public void Add(T item)
		{
			this.elements.Add(item);
		}

		public void Clear()
		{
			this.elements.Clear();
		}

		public bool Contains(T item)
		{
			return this.elements.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			this.elements.CopyTo(array, arrayIndex);
		}

		public bool Remove(T item)
		{
			return this.elements.Remove(item);
		}

		public int Count
		{
			get
			{
				return this.elements.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.elements.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as ComparableList<T>);
		}

		public bool Equals(ComparableList<T> obj)
		{
			if (obj == null || this.Count != obj.Count)
			{
				return false;
			}
			for (int i = 0; i < this.Count; i++)
			{
				T t = this[i];
				if (!t.Equals(obj[i]))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			int num = 19;
			foreach (T t in this)
			{
				num *= 31;
				num += t.GetHashCode();
			}
			return num;
		}

		public static bool operator ==(ComparableList<T> item1, ComparableList<T> item2)
		{
			if (object.ReferenceEquals(item1, null))
			{
				return object.ReferenceEquals(item2, null);
			}
			return item1.Equals(item2);
		}

		public static bool operator !=(ComparableList<T> item1, ComparableList<T> item2)
		{
			return !(item1 == item2);
		}

		private List<T> elements;
	}
}
