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
	public class Timeline : BaseObject, ITimeline
	{
		public static Timeline CreateTimeline(PropertyInfo propertyInfo, AbstractNodeObject node)
		{
			Timeline timeline = new Timeline(node);
			timeline.PropertyInfo = propertyInfo;
			node.Timelines.Add(timeline);
			return timeline;
		}

		public PropertyInfo PropertyInfo { get; private set; }

		public event EventHandler<TimelineDurationChangedEventArgs> DurationChangedEvent;

		public event EventHandler<FrameChangeTimelineRenderArgs> TimelineChangeRenderEvent;

		public CSTimeline InnerClass
		{
			get
			{
				return this.innerClass;
			}
		}

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

		[UndoProperty]
		public FrameCollection Frames
		{
			get
			{
				return this.frames;
			}
		}

		public int Duration { get; private set; }

		public Timeline(AbstractNodeObject node)
		{
			this.innerClass = new CSTimeline();
			this.Node = node;
			this.frames = new FrameCollection(this);
			this.Frames.CollectionChanged += this.OnFrameCollectionChanged;
			base.BindingRecorder(null);
		}

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

		private void AddFrameToHandler(Frame frame)
		{
			int frameIndex = frame.FrameIndex;
			HandlerFrame nodeHandlerFrameAtIndex = this.GetNodeHandlerFrameAtIndex(frameIndex, true);
			nodeHandlerFrameAtIndex.Frames.Add(frame);
		}

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

		private void RaiseDurationChangedEvent()
		{
			if (this.DurationChangedEvent != null)
			{
				this.DurationChangedEvent(this, new TimelineDurationChangedEventArgs());
			}
		}

		private void Update(bool needUpdateDuration = true)
		{
			if (needUpdateDuration)
			{
				this.UpdateDuration();
			}
			this.InnerClass.ClearSearchState();
		}

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

		private void OnFrameIndexChangedEvent(Frame frame, int oldFrameIndex)
		{
			this.RemoveFrameFromHandler(frame, oldFrameIndex);
			this.Frames.Remove(frame);
			this.Frames.Add(frame);
			this.Update(true);
		}

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

		private void RefreshCurrentActionFrame()
		{
			TimelineActionManager.Instance.RefreshCurrentFrame();
		}

		private void frame_FrameRenderChangedEvent(object sender, FrameChangeTimelineRenderArgs e)
		{
			if (this.TimelineChangeRenderEvent != null)
			{
				this.TimelineChangeRenderEvent(this, e);
			}
		}

		internal void AncestorObjectChanged(BaseObject sourceObj, NotifyCollectionChangedAction action)
		{
			foreach (Frame frame in this.Frames)
			{
				frame.AncestorObjectChanged(sourceObj, action);
			}
		}

		private CSTimeline innerClass;

		private AbstractNodeObject node;

		private FrameCollection frames;
	}
}
