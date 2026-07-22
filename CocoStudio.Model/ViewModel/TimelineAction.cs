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
	// Token: 0x020000E7 RID: 231
	public class TimelineAction
	{
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x0001DB50 File Offset: 0x0001BD50
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x0001DB6D File Offset: 0x0001BD6D
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

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x0001DB80 File Offset: 0x0001BD80
		// (set) Token: 0x0600075D RID: 1885 RVA: 0x0001DB9D File Offset: 0x0001BD9D
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

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0001DBB0 File Offset: 0x0001BDB0
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x0001DBC7 File Offset: 0x0001BDC7
		public bool AutoKey { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0001DBD0 File Offset: 0x0001BDD0
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x0001DBE7 File Offset: 0x0001BDE7
		public int AutoCreateFrameDuraton { get; set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x0001DBF0 File Offset: 0x0001BDF0
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x0001DC07 File Offset: 0x0001BE07
		public bool Loop { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0001DC10 File Offset: 0x0001BE10
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x0001DC2D File Offset: 0x0001BE2D
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

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x0001DC40 File Offset: 0x0001BE40
		public bool IsPlaying
		{
			get
			{
				return this.innerClass.IsPlaying();
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x0001DC60 File Offset: 0x0001BE60
		// (set) Token: 0x06000768 RID: 1896 RVA: 0x0001DC78 File Offset: 0x0001BE78
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

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x0001DC84 File Offset: 0x0001BE84
		// (set) Token: 0x0600076A RID: 1898 RVA: 0x0001DC9C File Offset: 0x0001BE9C
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

		// Token: 0x0600076B RID: 1899 RVA: 0x0001DCA8 File Offset: 0x0001BEA8
		public TimelineAction()
		{
			this.innerClass = new CSTimelineAction();
			this.Speed = 1f;
			this.AutoCreateFrameDuraton = 5;
			this.AutoKey = false;
			this.Loop = true;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0001DD10 File Offset: 0x0001BF10
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

		// Token: 0x0600076D RID: 1901 RVA: 0x0001DDB4 File Offset: 0x0001BFB4
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

		// Token: 0x0600076E RID: 1902 RVA: 0x0001DEC4 File Offset: 0x0001C0C4
		private void TimelineDurationChangedHandle(object sender, TimelineDurationChangedEventArgs e)
		{
			this.ReCalcDuration();
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0001DECE File Offset: 0x0001C0CE
		public void RemoveTimeline(Timeline timeline)
		{
			this.innerClass.RemoveTimeline(timeline.InnerClass);
			timeline.DurationChangedEvent -= this.TimelineDurationChangedHandle;
			timeline.AncestorObjectChanged(timeline, NotifyCollectionChangedAction.Remove);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0001DF00 File Offset: 0x0001C100
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

		// Token: 0x06000771 RID: 1905 RVA: 0x0001E090 File Offset: 0x0001C290
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

		// Token: 0x06000772 RID: 1906 RVA: 0x0001E110 File Offset: 0x0001C310
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

		// Token: 0x06000773 RID: 1907 RVA: 0x0001E158 File Offset: 0x0001C358
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

		// Token: 0x06000774 RID: 1908 RVA: 0x0001E218 File Offset: 0x0001C418
		public void Clear()
		{
			this.rootNode = null;
			this.ClearAnimationInfos();
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0001E22C File Offset: 0x0001C42C
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

		// Token: 0x06000776 RID: 1910 RVA: 0x0001E284 File Offset: 0x0001C484
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

		// Token: 0x06000777 RID: 1911 RVA: 0x0001E320 File Offset: 0x0001C520
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

		// Token: 0x06000778 RID: 1912 RVA: 0x0001E364 File Offset: 0x0001C564
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

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x0001E434 File Offset: 0x0001C634
		[UndoProperty]
		public ObservableCollection<AnimationInfo> AnimationInfoList
		{
			get
			{
				return this.animationInfoList;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x0001E458 File Offset: 0x0001C658
		// (set) Token: 0x0600077A RID: 1914 RVA: 0x0001E44C File Offset: 0x0001C64C
		public AnimationInfo ActivedAnimationInfo { get; set; }

		// Token: 0x0600077C RID: 1916 RVA: 0x0001E46F File Offset: 0x0001C66F
		internal void ClearAnimationInfos()
		{
			this.animationInfoList.Clear();
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x0001E480 File Offset: 0x0001C680
		// (set) Token: 0x0600077E RID: 1918 RVA: 0x0001E49D File Offset: 0x0001C69D
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

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x0001E4B0 File Offset: 0x0001C6B0
		// (set) Token: 0x06000780 RID: 1920 RVA: 0x0001E4CD File Offset: 0x0001C6CD
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

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x0001E4E0 File Offset: 0x0001C6E0
		// (set) Token: 0x06000782 RID: 1922 RVA: 0x0001E4FD File Offset: 0x0001C6FD
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

		// Token: 0x06000783 RID: 1923 RVA: 0x0001E510 File Offset: 0x0001C710
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

		// Token: 0x06000784 RID: 1924 RVA: 0x0001E54D File Offset: 0x0001C74D
		public void AddOnionKeyFrame(int frameIndex)
		{
			this.innerClass.AddOnionSkinKey(frameIndex);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0001E55D File Offset: 0x0001C75D
		public void RemoveOnionKeyFrame(int frameIndex)
		{
			this.innerClass.RemoveOnionSkinKey(frameIndex);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0001E570 File Offset: 0x0001C770
		public bool IsNeedChangeState()
		{
			return this.innerClass.IsNeedChangeState();
		}

		// Token: 0x04000308 RID: 776
		private CSTimelineAction innerClass;

		// Token: 0x04000309 RID: 777
		private int _panelIndexSpace = 12;

		// Token: 0x0400030A RID: 778
		private bool _needRefreshAnimate = true;

		// Token: 0x0400030B RID: 779
		private AbstractNodeObject rootNode = null;

		// Token: 0x0400030C RID: 780
		private ObservableCollection<AnimationInfo> animationInfoList = new ObservableCollection<AnimationInfo>();
	}
}
