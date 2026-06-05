using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoDevelop.Projects
{
	// Token: 0x0200013F RID: 319
	public class ProjectItemCollection<T> : ItemCollection<T>, IItemListHandler where T : ProjectItem
	{
		// Token: 0x06000BF2 RID: 3058 RVA: 0x0002C6B0 File Offset: 0x0002A8B0
		internal ProjectItemCollection(SolutionEntityItem parent)
		{
			this.parent = parent;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x0002C6BF File Offset: 0x0002A8BF
		public ProjectItemCollection()
		{
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0002C6C7 File Offset: 0x0002A8C7
		protected virtual void AddItem(T item)
		{
			base.Items.Add(item);
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0002C6D8 File Offset: 0x0002A8D8
		public void AddRange(IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				this.AddItem(item);
			}
			this.NotifyAdded(items, false);
			this.NotifyAdded(items, true);
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0002C730 File Offset: 0x0002A930
		protected virtual void RemoveItem(T item)
		{
			base.Items.Remove(item);
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0002C740 File Offset: 0x0002A940
		public void RemoveRange(IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				this.RemoveItem(item);
			}
			this.NotifyRemoved(items, false);
			this.NotifyRemoved(items, true);
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0002C7B4 File Offset: 0x0002A9B4
		public void Bind<U>(ProjectItemCollection<U> subCollection) where U : T
		{
			if (this.subCollections == null)
			{
				this.subCollections = new List<IItemListHandler>();
			}
			this.subCollections.Add(subCollection);
			subCollection.parentCollection = this;
			IItemListHandler list = subCollection;
			list.InternalAdd(from ob in this
			where list.CanHandle(ob)
			select ob, true);
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0002C812 File Offset: 0x0002AA12
		public void Unbind<U>(ProjectItemCollection<U> subCollection) where U : T
		{
			if (this.subCollections != null)
			{
				this.subCollections.Remove(subCollection);
				subCollection.parentCollection = null;
			}
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0002C9D4 File Offset: 0x0002ABD4
		public IEnumerable<U> GetAll<U>() where U : T
		{
			foreach (T it in this)
			{
				if (it is U)
				{
					yield return (U)((object)it);
				}
			}
			yield break;
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0002C9F4 File Offset: 0x0002ABF4
		protected override void OnItemAdded(T item)
		{
			T[] array = new T[]
			{
				item
			};
			this.NotifyAdded((IEnumerable<ProjectItem>)array, true);
			this.NotifyAdded((IEnumerable<ProjectItem>)array, false);
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0002CA2C File Offset: 0x0002AC2C
		protected override void OnItemRemoved(T item)
		{
			T[] array = new T[]
			{
				item
			};
			this.NotifyRemoved((IEnumerable<ProjectItem>)array, true);
			this.NotifyRemoved((IEnumerable<ProjectItem>)array, false);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x0002CA64 File Offset: 0x0002AC64
		void IItemListHandler.InternalAdd(IEnumerable<ProjectItem> items, bool comesFromParent)
		{
			foreach (ProjectItem projectItem in items)
			{
				this.AddItem((T)((object)projectItem));
			}
			this.NotifyAdded(items, comesFromParent);
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0002CABC File Offset: 0x0002ACBC
		void IItemListHandler.InternalRemove(IEnumerable<ProjectItem> items, bool comesFromParent)
		{
			foreach (ProjectItem projectItem in items)
			{
				this.RemoveItem((T)((object)projectItem));
			}
			this.NotifyRemoved(items, comesFromParent);
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x0002CB14 File Offset: 0x0002AD14
		bool IItemListHandler.CanHandle(ProjectItem obj)
		{
			return obj is T;
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x0002CB38 File Offset: 0x0002AD38
		private void NotifyAdded(IEnumerable<ProjectItem> items, bool comesFromParent)
		{
			if (comesFromParent)
			{
				if (this.subCollections == null)
				{
					return;
				}
				using (List<IItemListHandler>.Enumerator enumerator = this.subCollections.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IItemListHandler col = enumerator.Current;
						col.InternalAdd(from i in items
						where col.CanHandle(i)
						select i, true);
					}
					return;
				}
			}
			if (this.parentCollection != null)
			{
				this.parentCollection.InternalAdd(items, false);
			}
			if (this.parent != null)
			{
				this.parent.OnItemsAdded(items);
			}
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x0002CBFC File Offset: 0x0002ADFC
		private void NotifyRemoved(IEnumerable<ProjectItem> items, bool comesFromParent)
		{
			if (comesFromParent)
			{
				if (this.subCollections == null)
				{
					return;
				}
				using (List<IItemListHandler>.Enumerator enumerator = this.subCollections.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IItemListHandler col = enumerator.Current;
						col.InternalRemove(from i in items
						where col.CanHandle(i)
						select i, true);
					}
					return;
				}
			}
			if (this.parentCollection != null)
			{
				this.parentCollection.InternalRemove(items, false);
			}
			if (this.parent != null)
			{
				this.parent.OnItemsRemoved(items);
			}
		}

		// Token: 0x04000397 RID: 919
		private SolutionEntityItem parent;

		// Token: 0x04000398 RID: 920
		private IItemListHandler parentCollection;

		// Token: 0x04000399 RID: 921
		private List<IItemListHandler> subCollections;
	}
}
