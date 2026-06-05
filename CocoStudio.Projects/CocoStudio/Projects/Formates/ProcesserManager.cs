using System;
using System.Linq;
using CocoStudio.Model;
using Mono.Addins;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000037 RID: 55
	internal class ProcesserManager
	{
		// Token: 0x06000145 RID: 325 RVA: 0x00005E49 File Offset: 0x00004049
		public ProcesserManager()
		{
			this.publishProcessers = this.CollectProcesser<IPublishProcesser>();
			this.pairProcessers = this.CollectProcesser<ICompositeResourceProcesser>();
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00005E6C File Offset: 0x0000406C
		private T[] CollectProcesser<T>()
		{
			return AddinManager.GetExtensionObjects<T>();
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005E98 File Offset: 0x00004098
		internal IPublishProcesser GetPublishProcesser(ResourceData resourceData)
		{
			return this.publishProcessers.FirstOrDefault((IPublishProcesser a) => a.CanProcess(resourceData));
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005EE0 File Offset: 0x000040E0
		internal ICompositeResourceProcesser GetCompositeResourceProcesser(string filePath)
		{
			return this.pairProcessers.FirstOrDefault((ICompositeResourceProcesser a) => a.CanProcess(filePath));
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00005F28 File Offset: 0x00004128
		internal IPublishProcesser GetPublishProcesser<T>()
		{
			return this.publishProcessers.FirstOrDefault((IPublishProcesser a) => a.GetType() == typeof(T));
		}

		// Token: 0x04000051 RID: 81
		private IPublishProcesser[] publishProcessers;

		// Token: 0x04000052 RID: 82
		private ICompositeResourceProcesser[] pairProcessers;
	}
}
