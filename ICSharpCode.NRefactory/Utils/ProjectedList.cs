using System;
using System.Collections;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.Utils
{
	// Token: 0x02000123 RID: 291
	public sealed class ProjectedList<TInput, TOutput> : IList<TOutput>, ICollection<TOutput>, IEnumerable<TOutput>, IEnumerable where TOutput : class
	{
		// Token: 0x06000A3B RID: 2619 RVA: 0x0001E714 File Offset: 0x0001D714
		public ProjectedList(IList<TInput> input, Func<TInput, TOutput> projection)
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
			this.projection = projection;
			this.items = new TOutput[input.Count];
		}

		// Token: 0x170003EA RID: 1002
		public TOutput this[int index]
		{
			get
			{
				TOutput toutput = LazyInit.VolatileRead<TOutput>(ref this.items[index]);
				if (toutput != null)
				{
					return toutput;
				}
				return LazyInit.GetOrSet<TOutput>(ref this.items[index], this.projection(this.input[index]));
			}
		}

		// Token: 0x170003EB RID: 1003
		TOutput IList<TOutput>.this[int index]
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

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0001E7C5 File Offset: 0x0001D7C5
		public int Count
		{
			get
			{
				return this.items.Length;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x0001E7CF File Offset: 0x0001D7CF
		bool ICollection<TOutput>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0001E7D4 File Offset: 0x0001D7D4
		int IList<TOutput>.IndexOf(TOutput item)
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

		// Token: 0x06000A42 RID: 2626 RVA: 0x0001E80B File Offset: 0x0001D80B
		void IList<TOutput>.Insert(int index, TOutput item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0001E812 File Offset: 0x0001D812
		void IList<TOutput>.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0001E819 File Offset: 0x0001D819
		void ICollection<TOutput>.Add(TOutput item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0001E820 File Offset: 0x0001D820
		void ICollection<TOutput>.Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0001E828 File Offset: 0x0001D828
		bool ICollection<TOutput>.Contains(TOutput item)
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

		// Token: 0x06000A47 RID: 2631 RVA: 0x0001E860 File Offset: 0x0001D860
		void ICollection<TOutput>.CopyTo(TOutput[] array, int arrayIndex)
		{
			for (int i = 0; i < this.items.Length; i++)
			{
				array[arrayIndex + i] = this[i];
			}
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0001E890 File Offset: 0x0001D890
		bool ICollection<TOutput>.Remove(TOutput item)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0001E944 File Offset: 0x0001D944
		public IEnumerator<TOutput> GetEnumerator()
		{
			for (int i = 0; i < this.Count; i++)
			{
				yield return this[i];
			}
			yield break;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0001E960 File Offset: 0x0001D960
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400037B RID: 891
		private readonly IList<TInput> input;

		// Token: 0x0400037C RID: 892
		private readonly Func<TInput, TOutput> projection;

		// Token: 0x0400037D RID: 893
		private readonly TOutput[] items;
	}
}
