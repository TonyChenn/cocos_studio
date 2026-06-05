using System;
using System.Collections;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.Utils
{
	// Token: 0x02000124 RID: 292
	public sealed class ProjectedList<TContext, TInput, TOutput> : IList<TOutput>, ICollection<TOutput>, IEnumerable<TOutput>, IEnumerable where TOutput : class
	{
		// Token: 0x06000A4B RID: 2635 RVA: 0x0001E968 File Offset: 0x0001D968
		public ProjectedList(TContext context, IList<TInput> input, Func<TContext, TInput, TOutput> projection)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (projection == null)
			{
				throw new ArgumentNullException("projection");
			}
			this.input = input;
			this.context = context;
			this.projection = projection;
			this.items = new TOutput[input.Count];
		}

		// Token: 0x170003EE RID: 1006
		public TOutput this[int index]
		{
			get
			{
				TOutput toutput = LazyInit.VolatileRead<TOutput>(ref this.items[index]);
				if (toutput != null)
				{
					return toutput;
				}
				return LazyInit.GetOrSet<TOutput>(ref this.items[index], this.projection(this.context, this.input[index]));
			}
		}

		// Token: 0x170003EF RID: 1007
		TOutput IList<!2>.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x0001EA27 File Offset: 0x0001DA27
		public int Count
		{
			get
			{
				return this.items.Length;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0001EA31 File Offset: 0x0001DA31
		bool ICollection<!2>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0001EA34 File Offset: 0x0001DA34
		int IList<!2>.IndexOf(TOutput item)
		{
			EqualityComparer<TOutput> @default = EqualityComparer<TOutput>.Default;
			for (int i = 0; i < this.Count; i++)
			{
				if (@default.Equals(this[i], item))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0001EA6B File Offset: 0x0001DA6B
		void IList<!2>.Insert(int index, TOutput item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0001EA72 File Offset: 0x0001DA72
		void IList<!2>.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0001EA79 File Offset: 0x0001DA79
		void ICollection<!2>.Add(TOutput item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0001EA80 File Offset: 0x0001DA80
		void ICollection<!2>.Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0001EA88 File Offset: 0x0001DA88
		bool ICollection<!2>.Contains(TOutput item)
		{
			EqualityComparer<TOutput> @default = EqualityComparer<TOutput>.Default;
			for (int i = 0; i < this.Count; i++)
			{
				if (@default.Equals(this[i], item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0001EAC0 File Offset: 0x0001DAC0
		void ICollection<!2>.CopyTo(TOutput[] array, int arrayIndex)
		{
			for (int i = 0; i < this.items.Length; i++)
			{
				array[arrayIndex + i] = this[i];
			}
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0001EAF0 File Offset: 0x0001DAF0
		bool ICollection<!2>.Remove(TOutput item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0001EBA4 File Offset: 0x0001DBA4
		public IEnumerator<TOutput> GetEnumerator()
		{
			for (int i = 0; i < this.Count; i++)
			{
				yield return this[i];
			}
			yield break;
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0001EBC0 File Offset: 0x0001DBC0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400037E RID: 894
		private readonly IList<TInput> input;

		// Token: 0x0400037F RID: 895
		private readonly TContext context;

		// Token: 0x04000380 RID: 896
		private readonly Func<TContext, TInput, TOutput> projection;

		// Token: 0x04000381 RID: 897
		private readonly TOutput[] items;
	}
}
