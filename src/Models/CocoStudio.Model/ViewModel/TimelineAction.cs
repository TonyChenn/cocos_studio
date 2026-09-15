using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using CocoStudio.Basic;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.ExtensionModel;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	public class TimelineAction
	{
		[UndoProperty]
		public int CurrentFrameIndex
		{
			get
			{
				return this.innerClass.GetCurrentFrame();
			}
			set
			{
				this.innerClass.GotoFrame(value);
			}
		}

		public int Duration
		{
			get
			{
				return this.innerClass.GetDuration();
			}
			set
			{
				this.innerClass.SetDuration(value);
			}
		}

		public bool AutoKey { get; set; }

		public int AutoCreateFrameDuraton { get; set; }

		public bool Loop { get; set; }

		public float Speed
		{
			get
			{
				return this.innerClass.GetTimeSpeed();
			}
			set
			{
				this.innerClass.SetTimeSpeed(value);
			}
		}

		public bool IsPlaying
		{
			get
			{
				return this.innerClass.IsPlaying();
			}
		}

		public int TimelineZoom
		{
			get
			{
				return this._panelIndexSpace;
			}
			set
			{
				this._panelIndexSpace = value;
			}
		}

		public bool NeedRefreshAnimate
		{
			get
			{
				return this._needRefreshAnimate;
			}
			set
			{
				this._needRefreshAnimate = value;
			}
		}

		public TimelineAction()
		{
			this.innerClass = new CSTimelineAction();
			this.Speed = 1f;
			this.AutoCreateFrameDuraton = 5;
			this.AutoKey = false;
			this.Loop = true;
		}

		public static Timeline GetNodeTimeline(AbstractNodeObject node, PropertyInfo propertyInfo, bool autoCreate = true)
		{
			Timeline timeline = null;
			foreach (Timeline timeline2 in node.Timelines)
			{
				if (timeline2.PropertyInfo.Name == propertyInfo.Name)
				{
					timeline = timeline2;
					break;
				}
			}
			if (node.Parent != null && timeline == null && autoCreate)
			{
				timeline = Timeline.CreateTimeline(propertyInfo, node);
			}
			return timeline;
		}

		public void AddTimeline(Timeline timeline)
		{
			if (timeline.PropertyInfo.Name == "AnchorPoint")
			{
				CSTimeline cstimeline = null;
				foreach (Timeline timeline2 in timeline.Node.Timelines)
				{
					if (timeline2.PropertyInfo.Name == "Position")
					{
						cstimeline = timeline2.InnerClass;
						break;
					}
				}
				if (cstimeline != null)
				{
					this.innerClass.RemoveTimeline(cstimeline);
				}
				this.innerClass.AddTimeline(timeline.InnerClass);
				if (cstimeline != null)
				{
					this.innerClass.AddTimeline(cstimeline);
				}
			}
			else
			{
				this.innerClass.AddTimeline(timeline.InnerClass);
			}
			timeline.AncestorObjectChanged(timeline, NotifyCollectionChangedAction.Add);
			timeline.DurationChangedEvent += this.TimelineDurationChangedHandle;
		}

		private void TimelineDurationChangedHandle(object sender, TimelineDurationChangedEventArgs e)
		{
			this.ReCalcDuration();
		}

		public void RemoveTimeline(Timeline timeline)
		{
			this.innerClass.RemoveTimeline(timeline.InnerClass);
			timeline.DurationChangedEvent -= this.TimelineDurationChangedHandle;
			timeline.AncestorObjectChanged(timeline, NotifyCollectionChangedAction.Remove);
		}

		public void ReCalcDuration()
		{
			if (this.rootNode == null)
			{
				this.Duration = 0;
			}
			else
			{
				List<int> list = new List<int>();
				list.Add(0);
				Stack<AbstractNodeObject> stack = new Stack<AbstractNodeObject>();
				foreach (AbstractNodeObject item in this.rootNode.Children)
				{
					stack.Push(item);
				}
				while (stack.Count > 0)
				{
					AbstractNodeObject abstractNodeObject = stack.Pop();
					int num = 0;
					foreach (Timeline timeline in abstractNodeObject.Timelines)
					{
						if (timeline.Duration > num)
						{
							num = timeline.Duration;
						}
					}
					list.Add(num);
					foreach (AbstractNodeObject item2 in abstractNodeObject.Children)
					{
						stack.Push(item2);
					}
				}
				this.Duration = list.Max();
			}
		}

		public void InitWithRootNode(AbstractNodeObject node)
		{
			this.rootNode = node;
			foreach (AbstractNodeObject node2 in node.Children)
			{
				this.InitNodeItem(node2);
			}
			this.innerClass.InitWithRootNode(this.rootNode.GetCSVisual());
			this.ReCalcDuration();
		}

		public void ActiveAction()
		{
			if (this.rootNode != null)
			{
				this.innerClass.ActiveAction(this.rootNode.GetCSVisual());
			}
			else
			{
				LogConfig.Logger.Error("can not active action with null");
			}
		}

		private void InitNodeItem(AbstractNodeObject node)
		{
			Type type = node.GetType();
			TimelineAction.CreateDefaultTimeline(node);
			foreach (AbstractNodeObject node2 in node.Children)
			{
				this.InitNodeItem(node2);
			}
			foreach (Timeline timeline in node.Timelines)
			{
				this.AddTimeline(timeline);
			}
		}

		public void Clear()
		{
			this.rootNode = null;
			this.ClearAnimationInfos();
		}

		public static void CreateDefaultTimeline(AbstractNodeObject node)
		{
			PropertyInfo[] properties = node.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (FrameTypeManager.Instance.IsAutoCreateFrame(propertyInfo))
				{
					TimelineAction.GetNodeTimeline(node, propertyInfo, true);
				}
			}
		}

		public void Play()
		{
			int num = 0;
			int num2 = this.Duration;
			if (this.ActivedAnimationInfo != null)
			{
				num = this.ActivedAnimationInfo.StartIndex;
				num2 = this.ActivedAnimationInfo.EndIndex;
				if (this.CurrentFrameIndex > num2 || this.CurrentFrameIndex < num)
				{
					this.CurrentFrameIndex = num;
				}
			}
			if (this.CurrentFrameIndex == num2 && !this.Loop)
			{
				this.CurrentFrameIndex = num;
			}
			this.innerClass.Play(num, num2, this.CurrentFrameIndex, this.Loop);
		}

		public void Pause(bool isAtCurrentAnimationStart = false)
		{
			if (isAtCurrentAnimationStart)
			{
				this.CurrentFrameIndex = ((this.ActivedAnimationInfo == null) ? 0 : this.ActivedAnimationInfo.StartIndex);
			}
			else
			{
				this.innerClass.Pause();
			}
		}

		public void Play(string animationName)
		{
			if ("-- ALL --" == animationName)
			{
				this.ActivedAnimationInfo = new AnimationInfo("-- ALL --", 0, this.Duration);
			}
			else if (this.ActivedAnimationInfo == null || this.ActivedAnimationInfo.Name != animationName)
			{
				foreach (AnimationInfo animationInfo in this.AnimationInfoList)
				{
					if (animationInfo.Name == animationName)
					{
						this.ActivedAnimationInfo = animationInfo;
						break;
					}
				}
			}
			this.Play();
		}

		[UndoProperty]
		public ObservableCollection<AnimationInfo> AnimationInfoList
		{
			get
			{
				return this.animationInfoList;
			}
		}

		public AnimationInfo ActivedAnimationInfo { get; set; }

		internal void ClearAnimationInfos()
		{
			this.animationInfoList.Clear();
		}

		public bool OnionSkinEnable
		{
			get
			{
				return this.innerClass.IsOnionSkinEnable();
			}
			set
			{
				this.innerClass.SetOnionSkinEnable(value);
			}
		}

		public int OnionPreSkinNum
		{
			get
			{
				return this.innerClass.GetOnionSkinPreNum();
			}
			set
			{
				this.innerClass.SetOnionSkinPreNum(value);
			}
		}

		public int OnionSuffSkinNum
		{
			get
			{
				return this.innerClass.GetOnionSkinSuffNum();
			}
			set
			{
				this.innerClass.SetOnionSkinSuffNum(value);
			}
		}

		public bool IsOnionKeyFrame(int frameindex = -1)
		{
			bool result;
			if (-1 == frameindex)
			{
				result = this.innerClass.IsOnionKeyFrame(this.CurrentFrameIndex);
			}
			else
			{
				result = this.innerClass.IsOnionKeyFrame(frameindex);
			}
			return result;
		}

		public void AddOnionKeyFrame(int frameIndex)
		{
			this.innerClass.AddOnionSkinKey(frameIndex);
		}

		public void RemoveOnionKeyFrame(int frameIndex)
		{
			this.innerClass.RemoveOnionSkinKey(frameIndex);
		}

		public bool IsNeedChangeState()
		{
			return this.innerClass.IsNeedChangeState();
		}

		private CSTimelineAction innerClass;

		private int _panelIndexSpace = 12;

		private bool _needRefreshAnimate = true;

		private AbstractNodeObject rootNode = null;

		private ObservableCollection<AnimationInfo> animationInfoList = new ObservableCollection<AnimationInfo>();
	}
}
