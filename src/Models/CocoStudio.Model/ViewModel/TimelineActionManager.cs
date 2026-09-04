using System;
using System.Collections.ObjectModel;
using CocoStudio.Core;
using CocoStudio.Model.Visiter;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000EA RID: 234
	public class TimelineActionManager : BaseObject
	{
		// Token: 0x14000010 RID: 16
		// (add) Token: 0x0600078F RID: 1935 RVA: 0x0001E590 File Offset: 0x0001C790
		// (remove) Token: 0x06000790 RID: 1936 RVA: 0x0001E5CC File Offset: 0x0001C7CC
		public event CurrentFrameIndexChangedHandler CurrentFrameIndexChangedEvent;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000791 RID: 1937 RVA: 0x0001E608 File Offset: 0x0001C808
		// (remove) Token: 0x06000792 RID: 1938 RVA: 0x0001E644 File Offset: 0x0001C844
		public event EventHandler<SpeedChangedArgs> SpeedChangedEvent;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000793 RID: 1939 RVA: 0x0001E680 File Offset: 0x0001C880
		// (remove) Token: 0x06000794 RID: 1940 RVA: 0x0001E6BC File Offset: 0x0001C8BC
		public event AnimationPlayHandler AnimationPlayEvent;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000795 RID: 1941 RVA: 0x0001E6F8 File Offset: 0x0001C8F8
		// (remove) Token: 0x06000796 RID: 1942 RVA: 0x0001E734 File Offset: 0x0001C934
		public event EventHandler<AnimateStatesArgs> AnimateStatesChangedEvent;

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x0001E770 File Offset: 0x0001C970
		// (set) Token: 0x06000798 RID: 1944 RVA: 0x0001E788 File Offset: 0x0001C988
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

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x0001E794 File Offset: 0x0001C994
		// (set) Token: 0x0600079A RID: 1946 RVA: 0x0001E7AB File Offset: 0x0001C9AB
		public bool CanGotoFrame { get; set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x0001E7B4 File Offset: 0x0001C9B4
		// (set) Token: 0x0600079C RID: 1948 RVA: 0x0001E7E4 File Offset: 0x0001C9E4
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

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x0001E8D8 File Offset: 0x0001CAD8
		// (set) Token: 0x0600079E RID: 1950 RVA: 0x0001E924 File Offset: 0x0001CB24
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

		// Token: 0x0600079F RID: 1951 RVA: 0x0001E960 File Offset: 0x0001CB60
		public void RefreshCurrentFrame()
		{
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.CurrentFrameIndex = this.CurrentFrameIndex;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0001E994 File Offset: 0x0001CB94
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x0001E9C4 File Offset: 0x0001CBC4
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

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x0001E9EC File Offset: 0x0001CBEC
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x0001EA1C File Offset: 0x0001CC1C
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

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0001EA68 File Offset: 0x0001CC68
		// (set) Token: 0x060007A5 RID: 1957 RVA: 0x0001EA98 File Offset: 0x0001CC98
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

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0001EAC0 File Offset: 0x0001CCC0
		// (set) Token: 0x060007A7 RID: 1959 RVA: 0x0001EAF0 File Offset: 0x0001CCF0
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

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0001EB18 File Offset: 0x0001CD18
		// (set) Token: 0x060007A9 RID: 1961 RVA: 0x0001EB4C File Offset: 0x0001CD4C
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

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x0001EBA4 File Offset: 0x0001CDA4
		public bool IsPlaying
		{
			get
			{
				return this.CurrentTimelineAction != null && this.CurrentTimelineAction.IsPlaying;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x0001EBD4 File Offset: 0x0001CDD4
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

		// Token: 0x060007AC RID: 1964 RVA: 0x0001EC06 File Offset: 0x0001CE06
		private TimelineActionManager()
		{
			this.CanGotoFrame = true;
			base.BindingRecorder(null);
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0001EC28 File Offset: 0x0001CE28
		public void AddTimeline(Timeline timeline)
		{
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.AddTimeline(timeline);
			}
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0001EC50 File Offset: 0x0001CE50
		public void RemoveTimeline(Timeline timeline)
		{
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.RemoveTimeline(timeline);
			}
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0001EC78 File Offset: 0x0001CE78
		public void ReCalcDuration()
		{
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.ReCalcDuration();
			}
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0001ECA4 File Offset: 0x0001CEA4
		public void Init(AbstractNodeObject node, TimelineAction action)
		{
			this.CurrentTimelineAction = action;
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.InitWithRootNode(node);
				this.CurrentTimelineAction.ActiveAction();
			}
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0001ECE4 File Offset: 0x0001CEE4
		public void Clear()
		{
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.Clear();
			}
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0001ED0C File Offset: 0x0001CF0C
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

		// Token: 0x060007B3 RID: 1971 RVA: 0x0001ED58 File Offset: 0x0001CF58
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

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x0001EDE8 File Offset: 0x0001CFE8
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x0001EE18 File Offset: 0x0001D018
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

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0001EE64 File Offset: 0x0001D064
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x0001EE94 File Offset: 0x0001D094
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

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x0001EEBC File Offset: 0x0001D0BC
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x0001EEEC File Offset: 0x0001D0EC
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

		// Token: 0x060007BA RID: 1978 RVA: 0x0001EF14 File Offset: 0x0001D114
		public bool IsFrameOnionKey(int frameindex = -1)
		{
			return this.CurrentTimelineAction != null && this.CurrentTimelineAction.IsOnionKeyFrame(frameindex);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0001EF44 File Offset: 0x0001D144
		public void AddOnionKeyFrame(int frameIndex)
		{
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.AddOnionKeyFrame(frameIndex);
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0001EF6C File Offset: 0x0001D16C
		public void RemoveOnionKeyFrame(int frameIndex)
		{
			if (this.CurrentTimelineAction != null)
			{
				this.CurrentTimelineAction.RemoveOnionKeyFrame(frameIndex);
			}
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0001EF94 File Offset: 0x0001D194
		public bool IsNeedChangeState()
		{
			return this.CurrentTimelineAction != null && this.CurrentTimelineAction.IsNeedChangeState();
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x0001EFD0 File Offset: 0x0001D1D0
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x0001EFBD File Offset: 0x0001D1BD
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

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001F004 File Offset: 0x0001D204
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

		// Token: 0x04000315 RID: 789
		private bool canAutoKey = false;

		// Token: 0x04000316 RID: 790
		private TimelineAction CurrentTimelineAction;

		// Token: 0x04000317 RID: 791
		private static TimelineActionManager timelineAction = null;
	}
}
