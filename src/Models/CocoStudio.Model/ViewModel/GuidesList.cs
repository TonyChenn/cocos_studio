using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using CocoStudio.EngineAdapterWrap;

namespace CocoStudio.Model.ViewModel
{
	public class GuidesList : List<GuidesObject>, INotifyCollectionChanged
	{
		public GuidesList(CSGuidesService innerService)
		{
			if (GuidesList.innerService == null)
			{
				GuidesList.innerService = innerService;
			}
		}

		public new void Add(GuidesObject line)
		{
			base.Add(line);
			GuidesList.innerService.Add(line.GetNode());
			base.Sort();
			NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, line, 0);
			this.RaiseChanged(args);
		}

		public new void Remove(GuidesObject line)
		{
			base.Remove(line);
			GuidesList.innerService.Remove(line.GetNode());
			NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, line, 0);
			this.RaiseChanged(args);
		}

		public new void Clear()
		{
			base.Clear();
			GuidesList.innerService.Clear();
		}

		public new void AddRange(IEnumerable<GuidesObject> collection)
		{
			base.AddRange(collection);
			foreach (GuidesObject guidesObject in collection)
			{
				GuidesList.innerService.Add(guidesObject.GetNode());
				guidesObject.BindingRecorder(null);
			}
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		private void RaiseChanged(NotifyCollectionChangedEventArgs args)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, args);
			}
		}

		private static CSGuidesService innerService;
	}
}
