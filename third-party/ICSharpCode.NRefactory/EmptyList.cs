using System;
using System.Collections;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory
{
	// Token: 0x0200010C RID: 268
	[Serializable]
	public sealed class EmptyList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IEnumerator<T>, IDisposable, IEnumerator
	{
		// Token: 0x06000997 RID: 2455 RVA: 0x0001AF41 File Offset: 0x00019F41
		private EmptyList()
		{
		}

		// Token: 0x170003DA RID: 986
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

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x0001AF61 File Offset: 0x00019F61
		public int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x0600099B RID: 2459 RVA: 0x0001AF64 File Offset: 0x00019F64
		bool ICollection<T>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0001AF67 File Offset: 0x00019F67
		int IList<T>.IndexOf(T item)
		{
			return -1;
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0001AF6A File Offset: 0x00019F6A
		void IList<T>.Insert(int index, T item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0001AF71 File Offset: 0x00019F71
		void IList<T>.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0001AF78 File Offset: 0x00019F78
		void ICollection<T>.Add(T item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0001AF7F File Offset: 0x00019F7F
		void ICollection<T>.Clear()
		{
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0001AF81 File Offset: 0x00019F81
		bool ICollection<T>.Contains(T item)
		{
			return false;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0001AF84 File Offset: 0x00019F84
		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0001AF86 File Offset: 0x00019F86
		bool ICollection<T>.Remove(T item)
		{
			return false;
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0001AF89 File Offset: 0x00019F89
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this;
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0001AF8C File Offset: 0x00019F8C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x0001AF90 File Offset: 0x00019F90
		T IEnumerator<T>.Current
		{
			get
			{
				return default(T);
			}
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0001AFA8 File Offset: 0x00019FA8
		object IEnumerator.Current
		{
			get
			{
				return default(T);
			}
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0001AFC3 File Offset: 0x00019FC3
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0001AFC5 File Offset: 0x00019FC5
		bool IEnumerator.MoveNext()
		{
			return false;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0001AFC8 File Offset: 0x00019FC8
		void IEnumerator.Reset()
		{
		}

		// Token: 0x04000318 RID: 792
		public static readonly EmptyList<T> Instance = new EmptyList<T>();
	}
}
