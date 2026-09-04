using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using CocoStudio.EngineAdapterWrap;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000132 RID: 306
	public class GuidesList : List<GuidesObject>, INotifyCollectionChanged
	{
		// Token: 0x06000B4A RID: 2890 RVA: 0x0002CB74 File Offset: 0x0002AD74
		public GuidesList(CSGuidesService innerService)
		{
			if (GuidesList.innerService == null)
			{
				GuidesList.innerService = innerService;
			}
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0002CBA0 File Offset: 0x0002ADA0
		public new void Add(GuidesObject line)
		{
			base.Add(line);
			GuidesList.innerService.Add(line.GetNode());
			base.Sort();
			NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, line, 0);
			this.RaiseChanged(args);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0002CBE0 File Offset: 0x0002ADE0
		public new void Remove(GuidesObject line)
		{
			base.Remove(line);
			GuidesList.innerService.Remove(line.GetNode());
			NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, line, 0);
			this.RaiseChanged(args);
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0002CC18 File Offset: 0x0002AE18
		public new void Clear()
		{
			base.Clear();
			GuidesList.innerService.Clear();
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0002CC30 File Offset: 0x0002AE30
		public new void AddRange(IEnumerable<GuidesObject> collection)
		{
			base.AddRange(collection);
			foreach (GuidesObject guidesObject in collection)
			{
				GuidesList.innerService.Add(guidesObject.GetNode());
				guidesObject.BindingRecorder(null);
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000B4F RID: 2895 RVA: 0x0002CCA0 File Offset: 0x0002AEA0
		// (remove) Token: 0x06000B50 RID: 2896 RVA: 0x0002CCDC File Offset: 0x0002AEDC
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x06000B51 RID: 2897 RVA: 0x0002CD18 File Offset: 0x0002AF18
		private void RaiseChanged(NotifyCollectionChangedEventArgs args)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, args);
			}
		}

		// Token: 0x040004D3 RID: 1235
		private static CSGuidesService innerService;
	}
}
