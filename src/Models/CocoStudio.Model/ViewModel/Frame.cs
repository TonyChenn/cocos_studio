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
	// Token: 0x020000BC RID: 188
	[TypeExtensionPoint(ExtensionAttributeType = typeof(FrameExtensionAttribute), NodeType = typeof(FrameExtensionNode))]
	public abstract class Frame : BaseObject, ICloneable
	{
		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060005D8 RID: 1496 RVA: 0x00018A08 File Offset: 0x00016C08
		// (remove) Token: 0x060005D9 RID: 1497 RVA: 0x00018A44 File Offset: 0x00016C44
		public event EventHandler ParentChanged;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060005DA RID: 1498 RVA: 0x00018A80 File Offset: 0x00016C80
		// (remove) Token: 0x060005DB RID: 1499 RVA: 0x00018ABC File Offset: 0x00016CBC
		public event Action<Frame, int> FrameIndexChangedEvent;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060005DC RID: 1500 RVA: 0x00018AF8 File Offset: 0x00016CF8
		// (remove) Token: 0x060005DD RID: 1501 RVA: 0x00018B34 File Offset: 0x00016D34
		public event EventHandler<FrameChangeTimelineRenderArgs> FrameRenderChangedEvent;

		// Token: 0x060005DE RID: 1502 RVA: 0x00018B70 File Offset: 0x00016D70
		protected virtual void RaiseFrameIndexChangedEvent(int oldIndex)
		{
			if (this.FrameIndex != oldIndex && this.FrameIndexChangedEvent != null)
			{
				this.FrameIndexChangedEvent(this, oldIndex);
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x00018BA8 File Offset: 0x00016DA8
		// (set) Token: 0x060005E0 RID: 1504 RVA: 0x00018BC8 File Offset: 0x00016DC8
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

		// Token: 0x060005E1 RID: 1505 RVA: 0x00018C6C File Offset: 0x00016E6C
		internal virtual void SetInnerFrameIndex(int frameIndex)
		{
			this.innerClass.SetFrameIndex(frameIndex);
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x00018C7C File Offset: 0x00016E7C
		// (set) Token: 0x060005E3 RID: 1507 RVA: 0x00018C9C File Offset: 0x00016E9C
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

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x00018D20 File Offset: 0x00016F20
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x00018D70 File Offset: 0x00016F70
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

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00018EB0 File Offset: 0x000170B0
		public CSTimelineFrame InnerClass
		{
			get
			{
				return this.innerClass;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x00018EC8 File Offset: 0x000170C8
		// (set) Token: 0x060005E8 RID: 1512 RVA: 0x00018EDF File Offset: 0x000170DF
		public virtual ITimeline Timeline { get; set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x00018EE8 File Offset: 0x000170E8
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

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x00018F30 File Offset: 0x00017130
		// (set) Token: 0x060005EB RID: 1515 RVA: 0x00018F48 File Offset: 0x00017148
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

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x00018F54 File Offset: 0x00017154
		// (set) Token: 0x060005ED RID: 1517 RVA: 0x00018F6C File Offset: 0x0001716C
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

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x00018F78 File Offset: 0x00017178
		// (set) Token: 0x060005EF RID: 1519 RVA: 0x00018F90 File Offset: 0x00017190
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

		// Token: 0x060005F0 RID: 1520 RVA: 0x00018F9C File Offset: 0x0001719C
		public virtual bool HitTest(Point point)
		{
			return this.HitRect.Contains(point);
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00018FC0 File Offset: 0x000171C0
		public Frame()
		{
			this.innerClass = new CSExtensionFrame();
			this.enterCallback = new CSExtensionFrame.FrameEnterCallBack(this.Enter);
			this.applyCallback = new CSExtensionFrame.FrameApplyCallBack(this.Apply);
			this.RegisterCallBack();
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00019021 File Offset: 0x00017221
		private void RegisterCallBack()
		{
			(this.innerClass as CSExtensionFrame).SetFrameEnterCallBack(this.enterCallback);
			(this.innerClass as CSExtensionFrame).SetFrameApplyCallBack(this.applyCallback);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00019052 File Offset: 0x00017252
		private void UnRegisterCallBack()
		{
			(this.innerClass as CSExtensionFrame).SetFrameEnterCallBack(null);
			(this.innerClass as CSExtensionFrame).SetFrameApplyCallBack(null);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001907C File Offset: 0x0001727C
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

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001912F File Offset: 0x0001732F
		protected virtual void OnEnter(int nextFrameIndex, bool isChangeState)
		{
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00019134 File Offset: 0x00017334
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

		// Token: 0x060005F7 RID: 1527 RVA: 0x00019198 File Offset: 0x00017398
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

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001924D File Offset: 0x0001744D
		protected virtual void OnApply(float percent, bool isChangeState)
		{
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00019250 File Offset: 0x00017450
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

		// Token: 0x060005FA RID: 1530 RVA: 0x000192F4 File Offset: 0x000174F4
		protected virtual void OnUpdateProperty(AbstractNodeObject node)
		{
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000192F8 File Offset: 0x000174F8
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

		// Token: 0x060005FC RID: 1532 RVA: 0x0001934C File Offset: 0x0001754C
		public virtual object Clone()
		{
			Type type = base.GetType();
			Frame frame = Activator.CreateInstance(type) as Frame;
			this.SetValue(frame);
			return frame;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0001937C File Offset: 0x0001757C
		protected virtual void SetValue(Frame frame)
		{
			frame.FrameIndex = this.FrameIndex;
			frame.Tween = this.Tween;
			frame.forceApply = this.forceApply;
			frame.Timeline = this.Timeline;
			frame.PropertyHandler = this.PropertyHandler;
			frame.EasingData = (this.EasingData.Clone() as EasingValue);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000193E0 File Offset: 0x000175E0
		public void RemoveFromTimeline()
		{
			Timeline timeline = this.Timeline as Timeline;
			if (timeline != null)
			{
				timeline.Frames.Remove(this);
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00019411 File Offset: 0x00017611
		internal virtual void UpdateValue(object deltaValue)
		{
		}

		// Token: 0x040002B0 RID: 688
		private EasingValue easingValue = null;

		// Token: 0x040002B1 RID: 689
		public PropertyAccessorHandler PropertyHandler;

		// Token: 0x040002B2 RID: 690
		protected CSTimelineFrame innerClass;

		// Token: 0x040002B3 RID: 691
		protected bool select;

		// Token: 0x040002B4 RID: 692
		private Rectangle rect;

		// Token: 0x040002B5 RID: 693
		private Rectangle hitRect;

		// Token: 0x040002B6 RID: 694
		private CSExtensionFrame.FrameEnterCallBack enterCallback;

		// Token: 0x040002B7 RID: 695
		private CSExtensionFrame.FrameApplyCallBack applyCallback;

		// Token: 0x040002B8 RID: 696
		protected Frame nextFrame = null;

		// Token: 0x040002B9 RID: 697
		protected bool forceApply = false;
	}
}
