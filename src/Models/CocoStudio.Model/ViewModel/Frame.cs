using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ExtensionModel;
using CocoStudio.Model.Interface;
using CocoStudio.UndoManager;
using Gdk;
using Mono.Addins;

namespace CocoStudio.Model.ViewModel
{
	[TypeExtensionPoint(ExtensionAttributeType = typeof(FrameExtensionAttribute), NodeType = typeof(FrameExtensionNode))]
	public abstract class Frame : BaseObject, ICloneable
	{
		public event EventHandler ParentChanged;

		public event Action<Frame, int> FrameIndexChangedEvent;

		public event EventHandler<FrameChangeTimelineRenderArgs> FrameRenderChangedEvent;

		protected virtual void RaiseFrameIndexChangedEvent(int oldIndex)
		{
			if (this.FrameIndex != oldIndex && this.FrameIndexChangedEvent != null)
			{
				this.FrameIndexChangedEvent(this, oldIndex);
			}
		}

		[UndoProperty]
		public virtual int FrameIndex
		{
			get
			{
				return this.innerClass.GetFrameIndex();
			}
			set
			{
				int frameIndex = this.FrameIndex;
				if (frameIndex != value)
				{
					using (CompositeTask.Run(" ChangedFrameIndex", null))
					{
						this.innerClass.SetFrameIndex(value);
						this.RaiseFrameIndexChangedEvent(frameIndex);
						this.RaisePropertyChanged<int>(() => this.FrameIndex);
					}
				}
			}
		}

		internal virtual void SetInnerFrameIndex(int frameIndex)
		{
			this.innerClass.SetFrameIndex(frameIndex);
		}

		[UndoProperty]
		public virtual bool Tween
		{
			get
			{
				return this.innerClass.IsTween();
			}
			set
			{
				this.innerClass.SetTween(value);
				if (this.FrameRenderChangedEvent != null)
				{
					this.FrameRenderChangedEvent(this, new FrameChangeTimelineRenderArgs(Services.TaskService.IsUndoing, false));
				}
				this.RaisePropertyChanged<bool>(() => this.Tween);
			}
		}

		[UndoProperty]
		public virtual EasingValue EasingData
		{
			get
			{
				if (null == this.easingValue)
				{
					this.easingValue = new EasingValue((TweenType)this.innerClass.GetTweenType(), new List<float>(this.innerClass.GetEasingParam()));
				}
				return this.easingValue;
			}
			set
			{
				this.easingValue = value;
				this.innerClass.SetTweenType((int)this.easingValue.TweenType);
				List<float> list = new List<float>();
				foreach (PointF pointF in this.easingValue.PropPoints)
				{
					list.Add(pointF.X);
					list.Add(pointF.Y);
				}
				this.innerClass.SetEasingParam(new CSVectorFloat(list));
				if (this.easingValue != null && this.FrameRenderChangedEvent != null && (value.TweenType == TweenType.Linear || this.easingValue.TweenType == TweenType.Linear))
				{
					bool isUndoing = Services.TaskService.IsUndoing;
					this.FrameRenderChangedEvent(this, new FrameChangeTimelineRenderArgs(isUndoing, isUndoing));
				}
				this.RaisePropertyChanged<EasingValue>(() => this.EasingData);
			}
		}

		public CSTimelineFrame InnerClass
		{
			get
			{
				return this.innerClass;
			}
		}

		public virtual ITimeline Timeline { get; set; }

		public virtual AbstractNodeObject Node
		{
			get
			{
				AbstractNodeObject result;
				if (this.Timeline == null)
				{
					result = null;
				}
				else
				{
					Timeline timeline = this.Timeline as Timeline;
					if (timeline == null)
					{
						result = null;
					}
					else
					{
						result = timeline.Node;
					}
				}
				return result;
			}
		}

		public virtual bool Select
		{
			get
			{
				return this.select;
			}
			set
			{
				this.select = value;
			}
		}

		public Rectangle Rect
		{
			get
			{
				return this.rect;
			}
			set
			{
				this.rect = value;
			}
		}

		public Rectangle HitRect
		{
			get
			{
				return this.hitRect;
			}
			set
			{
				this.hitRect = value;
			}
		}

		public virtual bool HitTest(Point point)
		{
			return this.HitRect.Contains(point);
		}

		public Frame()
		{
			this.innerClass = new CSExtensionFrame();
			this.enterCallback = new CSExtensionFrame.FrameEnterCallBack(this.Enter);
			this.applyCallback = new CSExtensionFrame.FrameApplyCallBack(this.Apply);
			this.RegisterCallBack();
		}

		private void RegisterCallBack()
		{
			(this.innerClass as CSExtensionFrame).SetFrameEnterCallBack(this.enterCallback);
			(this.innerClass as CSExtensionFrame).SetFrameApplyCallBack(this.applyCallback);
		}

		private void UnRegisterCallBack()
		{
			(this.innerClass as CSExtensionFrame).SetFrameEnterCallBack(null);
			(this.innerClass as CSExtensionFrame).SetFrameApplyCallBack(null);
		}

		private void Enter(int nextFrameIndex)
		{
			if (this.Node != null)
			{
				bool canAutoKey = TimelineActionManager.Instance.CanAutoKey;
				if (canAutoKey)
				{
					TimelineActionManager.Instance.CanAutoKey = false;
				}
				bool flag = TimelineActionManager.Instance.IsNeedChangeState();
				this.Node.IsRaisePropertyChanged = (flag && !TimelineActionManager.Instance.IsPlaying);
				this.nextFrame = (this.Timeline as Timeline).Frames[nextFrameIndex];
				this.OnEnter(nextFrameIndex, flag);
				this.Node.IsRaisePropertyChanged = true;
				if (canAutoKey)
				{
					TimelineActionManager.Instance.CanAutoKey = true;
				}
			}
		}

		protected virtual void OnEnter(int nextFrameIndex, bool isChangeState)
		{
		}

		public void ForceEnterWhenApply(int currentIndex)
		{
			if (!this.Tween && this.FrameIndex < currentIndex && this.nextFrame != null && currentIndex < this.nextFrame.FrameIndex)
			{
				bool flag = this.forceApply;
				this.forceApply = true;
				this.Apply(0f);
				this.forceApply = flag;
			}
		}

		private void Apply(float percent)
		{
			if (this.Node != null)
			{
				if (this.Tween || this.forceApply)
				{
					bool canAutoKey = TimelineActionManager.Instance.CanAutoKey;
					if (canAutoKey)
					{
						TimelineActionManager.Instance.CanAutoKey = false;
					}
					bool flag = TimelineActionManager.Instance.IsNeedChangeState();
					this.Node.IsRaisePropertyChanged = (flag && !TimelineActionManager.Instance.IsPlaying);
					this.OnApply(percent, flag);
					this.Node.IsRaisePropertyChanged = true;
					if (canAutoKey)
					{
						TimelineActionManager.Instance.CanAutoKey = true;
					}
				}
			}
		}

		protected virtual void OnApply(float percent, bool isChangeState)
		{
		}

		public void UpdateProperty(AbstractNodeObject node)
		{
			this.OnUpdateProperty(node);
			if (this.Timeline != null)
			{
				Frame frame = this;
				foreach (Frame frame2 in this.Timeline.Frames)
				{
					if (frame2.FrameIndex > this.FrameIndex)
					{
						frame = frame2;
						break;
					}
				}
				this.InnerClass.OnEnter(frame.InnerClass);
			}
		}

		protected virtual void OnUpdateProperty(AbstractNodeObject node)
		{
		}

		internal virtual void AncestorObjectChanged(BaseObject sourceObj, NotifyCollectionChangedAction action)
		{
			if (this.ParentChanged != null)
			{
				this.ParentChanged(this, null);
			}
			if (action == NotifyCollectionChangedAction.Add)
			{
				this.RegisterCallBack();
			}
			else if (action == NotifyCollectionChangedAction.Remove)
			{
				this.UnRegisterCallBack();
			}
		}

		public virtual object Clone()
		{
			Type type = base.GetType();
			Frame frame = Activator.CreateInstance(type) as Frame;
			this.SetValue(frame);
			return frame;
		}

		protected virtual void SetValue(Frame frame)
		{
			frame.FrameIndex = this.FrameIndex;
			frame.Tween = this.Tween;
			frame.forceApply = this.forceApply;
			frame.Timeline = this.Timeline;
			frame.PropertyHandler = this.PropertyHandler;
			frame.EasingData = (this.EasingData.Clone() as EasingValue);
		}

		public void RemoveFromTimeline()
		{
			Timeline timeline = this.Timeline as Timeline;
			if (timeline != null)
			{
				timeline.Frames.Remove(this);
			}
		}

		internal virtual void UpdateValue(object deltaValue)
		{
		}

		private EasingValue easingValue = null;

		public PropertyAccessorHandler PropertyHandler;

		protected CSTimelineFrame innerClass;

		protected bool select;

		private Rectangle rect;

		private Rectangle hitRect;

		private CSExtensionFrame.FrameEnterCallBack enterCallback;

		private CSExtensionFrame.FrameApplyCallBack applyCallback;

		protected Frame nextFrame = null;

		protected bool forceApply = false;
	}
}
