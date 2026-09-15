using System;
using System.Collections.ObjectModel;
using CocoStudio.Core;
using CocoStudio.Model.Visiter;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	public class TimelineActionManager : BaseObject
	{
		public event CurrentFrameIndexChangedHandler CurrentFrameIndexChangedEvent;

		public event EventHandler<SpeedChangedArgs> SpeedChangedEvent;

		public event AnimationPlayHandler AnimationPlayEvent;

		public event EventHandler<AnimateStatesArgs> AnimateStatesChangedEvent;

		public bool CanAutoKey
		{
			get
			{
				return this.canAutoKey;
			}
			set
			{
				this.canAutoKey = value;
			}
		}

		public bool CanGotoFrame { get; set; }

		[UndoProperty]
		public int CurrentFrameIndex
		{
			get
			{
				int result;
				if (this.CurrentTimelineAction != null)
				{
					result = this.CurrentTimelineAction.CurrentFrameIndex;
				}
				else
				{
					result = 0;
				}
				return result;
			}
			set
			{
				if (this.CurrentTimelineAction != null && this.CanGotoFrame)
				{
					bool flag = this.CanAutoKey;
					if (flag)
					{
						this.CanAutoKey = false;
					}
					int currentFrameIndex = this.CurrentFrameIndex;
					if (value < 0)
					{
						value = 0;
					}
					if (this.IsPlaying)
					{
						if (this.AnimationPlayEvent != null)
						{
							this.AnimationPlayEvent(false);
						}
					}
					this.CurrentTimelineAction.CurrentFrameIndex = value;
					this.RaisePropertyChanged<int>(() => this.CurrentFrameIndex);
					if (this.CurrentFrameIndexChangedEvent != null)
					{
						this.CurrentFrameIndexChangedEvent();
					}
					if (flag)
					{
						this.CanAutoKey = true;
					}
				}
			}
		}

		public bool NeedRefreshAnimate
		{
			get
			{
				bool result;
				if (this.CurrentTimelineAction != null)
				{
					bool flag = Services.Workbench.ActiveDocument.File.IsSkeletonFile() && this.CurrentTimelineAction.NeedRefreshAnimate;
					result = flag;
				}
				else
				{
					result = false;
				}
				return result;
			}
			set
			{
				if (this.CurrentTimelineAction != null)
				{
					this.CurrentTimelineAction.NeedRefreshAnimate = value;
				}
				this.AnimateStatesChangedEvent(this, new AnimateStatesArgs());
			}
		}

		public void RefreshCurrentFrame()
		{
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.CurrentFrameIndex = this.CurrentFrameIndex;
			}
		}

		public int Duration
		{
			get
			{
				int result;
				if (this.CurrentTimelineAction != null)
				{
					result = this.CurrentTimelineAction.Duration;
				}
				else
				{
					result = 0;
				}
				return result;
			}
			set
			{
				if (this.CurrentTimelineAction != null)
				{
					this.CurrentTimelineAction.Duration = value;
				}
			}
		}

		public bool AutoKey
		{
			get
			{
				return this.CurrentTimelineAction != null && this.CurrentTimelineAction.AutoKey;
			}
			set
			{
				if (this.CurrentTimelineAction != null)
				{
					this.CurrentTimelineAction.AutoKey = value;
				}
				if (this.AnimateStatesChangedEvent != null)
				{
					this.AnimateStatesChangedEvent(this, new AnimateStatesArgs());
				}
			}
		}

		public int AutoCreateFrameDuraton
		{
			get
			{
				int result;
				if (this.CurrentTimelineAction != null)
				{
					result = this.CurrentTimelineAction.AutoCreateFrameDuraton;
				}
				else
				{
					result = 5;
				}
				return result;
			}
			set
			{
				if (this.CurrentTimelineAction != null)
				{
					this.CurrentTimelineAction.AutoCreateFrameDuraton = value;
				}
			}
		}

		public bool Loop
		{
			get
			{
				return this.CurrentTimelineAction == null || this.CurrentTimelineAction.Loop;
			}
			set
			{
				if (this.CurrentTimelineAction != null)
				{
					this.CurrentTimelineAction.Loop = value;
				}
			}
		}

		public float Speed
		{
			get
			{
				float result;
				if (this.CurrentTimelineAction != null)
				{
					result = this.CurrentTimelineAction.Speed;
				}
				else
				{
					result = 1f;
				}
				return result;
			}
			set
			{
				if (this.CurrentTimelineAction != null)
				{
					this.CurrentTimelineAction.Speed = value;
					if (this.SpeedChangedEvent != null)
					{
						this.SpeedChangedEvent(this, new SpeedChangedArgs(this.CurrentTimelineAction.Speed));
					}
				}
			}
		}

		public bool IsPlaying
		{
			get
			{
				return this.CurrentTimelineAction != null && this.CurrentTimelineAction.IsPlaying;
			}
		}

		public static TimelineActionManager Instance
		{
			get
			{
				if (TimelineActionManager.timelineAction == null)
				{
					TimelineActionManager.timelineAction = new TimelineActionManager();
				}
				return TimelineActionManager.timelineAction;
			}
		}

		private TimelineActionManager()
		{
			this.CanGotoFrame = true;
			base.BindingRecorder(null);
		}

		public void AddTimeline(Timeline timeline)
		{
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.AddTimeline(timeline);
			}
		}

		public void RemoveTimeline(Timeline timeline)
		{
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.RemoveTimeline(timeline);
			}
		}

		public void ReCalcDuration()
		{
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.ReCalcDuration();
			}
		}

		public void Init(AbstractNodeObject node, TimelineAction action)
		{
			this.CurrentTimelineAction = action;
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.InitWithRootNode(node);
				this.CurrentTimelineAction.ActiveAction();
			}
		}

		public void Clear()
		{
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.Clear();
			}
		}

		public void Play()
		{
			this.CanAutoKey = false;
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.Play();
			}
			if (this.AnimationPlayEvent != null)
			{
				this.AnimationPlayEvent(true);
			}
		}

		public void Pause(bool isAtCurrentAnimationStart = false)
		{
			this.CanAutoKey = true;
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.Pause(isAtCurrentAnimationStart);
				if (!isAtCurrentAnimationStart)
				{
					this.RaisePropertyChanged<int>(() => this.CurrentFrameIndex, true);
				}
			}
			if (this.AnimationPlayEvent != null)
			{
				this.AnimationPlayEvent(false);
			}
		}

		public bool OnionSkinEnable
		{
			get
			{
				return this.CurrentTimelineAction != null && this.CurrentTimelineAction.OnionSkinEnable;
			}
			set
			{
				if (this.CurrentTimelineAction != null)
				{
					this.CurrentTimelineAction.OnionSkinEnable = value;
				}
				if (this.AnimateStatesChangedEvent != null)
				{
					this.AnimateStatesChangedEvent(this, new AnimateStatesArgs());
				}
			}
		}

		public int OnionPreSkinNum
		{
			get
			{
				int result;
				if (this.CurrentTimelineAction != null)
				{
					result = this.CurrentTimelineAction.OnionPreSkinNum;
				}
				else
				{
					result = 0;
				}
				return result;
			}
			set
			{
				if (this.CurrentTimelineAction != null)
				{
					this.CurrentTimelineAction.OnionPreSkinNum = value;
				}
			}
		}

		public int OnionSuffSkinNum
		{
			get
			{
				int result;
				if (this.CurrentTimelineAction != null)
				{
					result = this.CurrentTimelineAction.OnionSuffSkinNum;
				}
				else
				{
					result = 0;
				}
				return result;
			}
			set
			{
				if (this.CurrentTimelineAction != null)
				{
					this.CurrentTimelineAction.OnionSuffSkinNum = value;
				}
			}
		}

		public bool IsFrameOnionKey(int frameindex = -1)
		{
			return this.CurrentTimelineAction != null && this.CurrentTimelineAction.IsOnionKeyFrame(frameindex);
		}

		public void AddOnionKeyFrame(int frameIndex)
		{
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.AddOnionKeyFrame(frameIndex);
			}
		}

		public void RemoveOnionKeyFrame(int frameIndex)
		{
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.RemoveOnionKeyFrame(frameIndex);
			}
		}

		public bool IsNeedChangeState()
		{
			return this.CurrentTimelineAction != null && this.CurrentTimelineAction.IsNeedChangeState();
		}

		public AnimationInfo ActivedAnimationInfo
		{
			get
			{
				AnimationInfo result;
				if (this.CurrentTimelineAction == null)
				{
					result = null;
				}
				else
				{
					result = this.CurrentTimelineAction.ActivedAnimationInfo;
				}
				return result;
			}
			set
			{
				this.CurrentTimelineAction.ActivedAnimationInfo = value;
			}
		}

		public ObservableCollection<AnimationInfo> GetAnimationInfoList()
		{
			ObservableCollection<AnimationInfo> result;
			if (this.CurrentTimelineAction == null)
			{
				result = null;
			}
			else
			{
				result = this.CurrentTimelineAction.AnimationInfoList;
			}
			return result;
		}

		private bool canAutoKey = false;

		private TimelineAction CurrentTimelineAction;

		private static TimelineActionManager timelineAction = null;
	}
}
