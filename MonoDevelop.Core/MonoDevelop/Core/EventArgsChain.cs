using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoDevelop.Core
{
	// Token: 0x0200003A RID: 58
	public class EventArgsChain<T> : EventArgs, ICollection<T>, IEnumerable<T>, IEnumerable, IEventArgsChain
	{
		// Token: 0x060001EE RID: 494 RVA: 0x000087D8 File Offset: 0x000069D8
		public EventArgsChain()
		{
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000087EB File Offset: 0x000069EB
		public EventArgsChain(IEnumerable<T> args)
		{
			this.events.AddRange(args);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000880A File Offset: 0x00006A0A
		void IEventArgsChain.MergeWith(IEventArgsChain chain)
		{
			this.events.AddRange(((EventArgsChain<T>)chain).events);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00008822 File Offset: 0x00006A22
		public void MergeWith(EventArgsChain<T> chain)
		{
			this.events.AddRange(chain.events);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00008835 File Offset: 0x00006A35
		public void AddRange(IEnumerable<T> args)
		{
			this.events.AddRange(args);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00008843 File Offset: 0x00006A43
		public void Add(T eventArgs)
		{
			this.events.Add(eventArgs);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00008851 File Offset: 0x00006A51
		public void Clear()
		{
			this.events.Clear();
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000885E File Offset: 0x00006A5E
		public bool Contains(T item)
		{
			return this.events.Contains(item);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000886C File Offset: 0x00006A6C
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.events.CopyTo(array, arrayIndex);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000887B File Offset: 0x00006A7B
		public bool Remove(T item)
		{
			return this.events.Remove(item);
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00008889 File Offset: 0x00006A89
		public int Count
		{
			get
			{
				return this.events.Count;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00008896 File Offset: 0x00006A96
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00008899 File Offset: 0x00006A99
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.events.GetEnumerator();
		}

		// Token: 0x060001FB RID: 507 RVA: 0x000088AB File Offset: 0x00006AAB
		public IEnumerator<T> GetEnumerator()
		{
			return this.events.GetEnumerator();
		}

		// Token: 0x040000BB RID: 187
		private List<T> events = new List<T>();
	}
}
