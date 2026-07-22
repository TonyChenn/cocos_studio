using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.ExtensionModel;
using CocoStudio.Model.Interface;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000E5 RID: 229
	public class Timeline : BaseObject, ITimeline
	{
		// Token: 0x0600073E RID: 1854 RVA: 0x0001D3FC File Offset: 0x0001B5FC
		public static Timeline CreateTimeline(PropertyInfo propertyInfo, AbstractNodeObject node)
		{
			Timeline timeline = new Timeline(node);
			timeline.PropertyInfo = propertyInfo;
			node.Timelines.Add(timeline);
			return timeline;
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x0001D42C File Offset: 0x0001B62C
		// (set) Token: 0x06000740 RID: 1856 RVA: 0x0001D443 File Offset: 0x0001B643
		public PropertyInfo PropertyInfo { get; private set; }

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000741 RID: 1857 RVA: 0x0001D44C File Offset: 0x0001B64C
		// (remove) Token: 0x06000742 RID: 1858 RVA: 0x0001D488 File Offset: 0x0001B688
		public event EventHandler<TimelineDurationChangedEventArgs> DurationChangedEvent;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000743 RID: 1859 RVA: 0x0001D4C4 File Offset: 0x0001B6C4
		// (remove) Token: 0x06000744 RID: 1860 RVA: 0x0001D500 File Offset: 0x0001B700
		public event EventHandler<FrameChangeTimelineRenderArgs> TimelineChangeRenderEvent;

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000745 RID: 1861 RVA: 0x0001D53C File Offset: 0x0001B73C
		public CSTimeline InnerClass
		{
			get
			{
				return this.innerClass;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x0001D554 File Offset: 0x0001B754
		// (set) Token: 0x06000747 RID: 1863 RVA: 0x0001D56C File Offset: 0x0001B76C
		public AbstractNodeObject Node
		{
			get
			{
				return this.node;
			}
			private set
			{
				this.node = value;
				this.innerClass.SetActionTag(this.node.ActionTag);
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x0001D590 File Offset: 0x0001B790
		[UndoProperty]
		public FrameCollection Frames
		{
			get
			{
				return this.frames;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x0001D5A8 File Offset: 0x0001B7A8
		// (set) Token: 0x0600074A RID: 1866 RVA: 0x0001D5BF File Offset: 0x0001B7BF
		public int Duration { get; private set; }

		// Token: 0x0600074B RID: 1867 RVA: 0x0001D5C8 File Offset: 0x0001B7C8
		public Timeline(AbstractNodeObject node)
		{
			this.innerClass = new CSTimeline();
			this.Node = node;
			this.frames = new FrameCollection(this);
			this.Frames.CollectionChanged += this.OnFrameCollectionChanged;
			base.BindingRecorder(null);
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0001D620 File Offset: 0x0001B820
		private HandlerFrame GetNodeHandlerFrameAtIndex(int index, bool createNotExists = true)
		{
			HandlerFrame handlerFrame = null;
			foreach (Frame frame in this.Node.Frames)
			{
				if (frame.FrameIndex == index)
				{
					handlerFrame = (frame as HandlerFrame);
					break;
				}
			}
			if (handlerFrame == null && createNotExists)
			{
				handlerFrame = new HandlerFrame();
				handlerFrame.SetInnerFrameIndex(index);
				this.Node.Frames.Add(handlerFrame);
			}
			return handlerFrame;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001D6D4 File Offset: 0x0001B8D4
		private void AddFrameToHandler(Frame frame)
		{
			int frameIndex = frame.FrameIndex;
			HandlerFrame nodeHandlerFrameAtIndex = this.GetNodeHandlerFrameAtIndex(frameIndex, true);
			nodeHandlerFrameAtIndex.Frames.Add(frame);
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0001D700 File Offset: 0x0001B900
		private void RemoveFrameFromHandler(Frame frame, int handlerIndex)
		{
			FrameCollection frameCollection = this.Node.Frames;
			HandlerFrame handlerFrame = null;
			foreach (Frame frame2 in frameCollection)
			{
				if (frame2.FrameIndex == handlerIndex)
				{
					handlerFrame = (frame2 as HandlerFrame);
					break;
				}
			}
			if (handlerFrame != null)
			{
				handlerFrame.Frames.Remove(frame);
				if (handlerFrame.Frames.Count == 0)
				{
					this.Node.Frames.Remove(handlerFrame);
				}
			}
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0001D7C0 File Offset: 0x0001B9C0
		private void UpdateDuration()
		{
			int num = 0;
			Frame frame = this.Frames.LastOrDefault<Frame>();
			if (frame != null)
			{
				num = frame.FrameIndex;
			}
			if (num != this.Duration)
			{
				this.Duration = num;
				this.RaiseDurationChangedEvent();
			}
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0001D80C File Offset: 0x0001BA0C
		private void RaiseDurationChangedEvent()
		{
			if (this.DurationChangedEvent != null)
			{
				this.DurationChangedEvent(this, new TimelineDurationChangedEventArgs());
			}
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0001D83C File Offset: 0x0001BA3C
		private void Update(bool needUpdateDuration = true)
		{
			if (needUpdateDuration)
			{
				this.UpdateDuration();
			}
			this.InnerClass.ClearSearchState();
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001D868 File Offset: 0x0001BA68
		public Frame GetTimelineFrameAtIndex(int frameIndex, bool autoCreate = true)
		{
			Frame frame = this.Frames.FirstOrDefault((Frame a) => a.FrameIndex == frameIndex);
			if (frame == null && autoCreate)
			{
				frame = FrameTypeManager.Instance.CreateFrame(this.PropertyInfo);
				frame.SetInnerFrameIndex(frameIndex);
				frame.UpdateProperty(this.node);
				frame.BindingRecorder(null);
				this.Frames.Add(frame);
			}
			return frame;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001D8F1 File Offset: 0x0001BAF1
		private void OnFrameIndexChangedEvent(Frame frame, int oldFrameIndex)
		{
			this.RemoveFrameFromHandler(frame, oldFrameIndex);
			this.Frames.Remove(frame);
			this.Frames.Add(frame);
			this.Update(true);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001D920 File Offset: 0x0001BB20
		private void OnFrameCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (object obj in e.NewItems)
				{
					Frame frame = obj as Frame;
					this.AddFrameToHandler(frame);
					frame.AncestorObjectChanged(this, e.Action);
					this.InnerClass.InsertFrame(this.Frames.IndexOf(frame), frame.InnerClass);
					frame.FrameIndexChangedEvent += this.OnFrameIndexChangedEvent;
					frame.FrameRenderChangedEvent += this.frame_FrameRenderChangedEvent;
				}
			}
			if (e.OldItems != null)
			{
				foreach (object obj in e.OldItems)
				{
					Frame frame = obj as Frame;
					this.RemoveFrameFromHandler(frame, frame.FrameIndex);
					this.InnerClass.RemoveFrame(frame.InnerClass);
					frame.AncestorObjectChanged(this, e.Action);
					frame.FrameIndexChangedEvent -= this.OnFrameIndexChangedEvent;
					frame.FrameRenderChangedEvent -= this.frame_FrameRenderChangedEvent;
					this.RefreshCurrentActionFrame();
				}
			}
			this.Update(true);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0001DAB8 File Offset: 0x0001BCB8
		private void RefreshCurrentActionFrame()
		{
			TimelineActionManager.Instance.RefreshCurrentFrame();
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0001DAC8 File Offset: 0x0001BCC8
		private void frame_FrameRenderChangedEvent(object sender, FrameChangeTimelineRenderArgs e)
		{
			if (this.TimelineChangeRenderEvent != null)
			{
				this.TimelineChangeRenderEvent(this, e);
			}
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0001DAF4 File Offset: 0x0001BCF4
		internal void AncestorObjectChanged(BaseObject sourceObj, NotifyCollectionChangedAction action)
		{
			foreach (Frame frame in this.Frames)
			{
				frame.AncestorObjectChanged(sourceObj, action);
			}
		}

		// Token: 0x04000302 RID: 770
		private CSTimeline innerClass;

		// Token: 0x04000303 RID: 771
		private AbstractNodeObject node;

		// Token: 0x04000304 RID: 772
		private FrameCollection frames;
	}
}
