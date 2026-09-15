using System;
using System.Linq;
using CocoStudio.Model;
using Mono.Addins;

namespace CocoStudio.Projects.Formates
{
	internal class ProcesserManager
	{
		public ProcesserManager()
		{
			this.publishProcessers = this.CollectProcesser<IPublishProcesser>();
			this.pairProcessers = this.CollectProcesser<ICompositeResourceProcesser>();
		}

		private T[] CollectProcesser<T>()
		{
			return AddinManager.GetExtensionObjects<T>();
		}

		internal IPublishProcesser GetPublishProcesser(ResourceData resourceData)
		{
			return this.publishProcessers.FirstOrDefault((IPublishProcesser a) => a.CanProcess(resourceData));
		}

		internal ICompositeResourceProcesser GetCompositeResourceProcesser(string filePath)
		{
			return this.pairProcessers.FirstOrDefault((ICompositeResourceProcesser a) => a.CanProcess(filePath));
		}

		internal IPublishProcesser GetPublishProcesser<T>()
		{
			return this.publishProcessers.FirstOrDefault((IPublishProcesser a) => a.GetType() == typeof(T));
		}

		private IPublishProcesser[] publishProcessers;

		private ICompositeResourceProcesser[] pairProcessers;
	}
}
