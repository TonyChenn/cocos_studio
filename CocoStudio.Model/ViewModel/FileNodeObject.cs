using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using CocoStudio.UndoManager.Recorder;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000F4 RID: 244
	[DisplayName("Display_Component_Entity")]
	[EngineClassName("Node")]
	public class FileNodeObject : NodeObject, IPlayControl, IInnerActoinNode, IStretchSize
	{
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x000217B4 File Offset: 0x0001F9B4
		// (set) Token: 0x0600086C RID: 2156 RVA: 0x000217CB File Offset: 0x0001F9CB
		[ResourceIgnore]
		public CocosItem Project { get; private set; }

		// Token: 0x0600086D RID: 2157 RVA: 0x000217D4 File Offset: 0x0001F9D4
		private CSProjectNode GetInnerObject()
		{
			return (CSProjectNode)this.innerNode;
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x000217F4 File Offset: 0x0001F9F4
		public FileNodeObject()
		{
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00021858 File Offset: 0x0001FA58
		public FileNodeObject(CocosItem project)
		{
			this.FileData = project;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x000218C4 File Offset: 0x0001FAC4
		public FileNodeObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00021927 File Offset: 0x0001FB27
		protected override void CreateCSObject()
		{
			this.innerNode = new CSProjectNode();
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00021935 File Offset: 0x0001FB35
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			base.InitIcon("Object.png");
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0002194C File Offset: 0x0001FB4C
		public override void InitOperation()
		{
			this.OperationFlag = (OperationMask)65527;
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0002195C File Offset: 0x0001FB5C
		private void InitInnerAction()
		{
			if (this.innerTimelineAction == null)
			{
				this.actionValue = null;
				this.actionValue = new InnerActionValue();
			}
			else
			{
				this.InnerActionSpeed = this.innerActionSpeed;
				InnerActionType actionType = InnerActionType.SingleFrame;
				int singleFrameIndex = 0;
				List<string> list = new List<string>();
				list.Add("-- ALL --");
				string activeAnimationName = "-- ALL --";
				foreach (AnimationInfo animationInfo in this.innerTimelineAction.AnimationInfoList)
				{
					list.Add(animationInfo.Name);
				}
				if (this.actionValue != null)
				{
					actionType = this.actionValue.ActionType;
					singleFrameIndex = this.actionValue.SingleFrameIndex;
					if (list.Contains(this.actionValue.ActivedAnimationName))
					{
						activeAnimationName = this.actionValue.ActivedAnimationName;
					}
				}
				this.actionValue = new InnerActionValue(actionType, list, activeAnimationName, singleFrameIndex);
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x00021A80 File Offset: 0x0001FC80
		// (set) Token: 0x06000876 RID: 2166 RVA: 0x00021AD0 File Offset: 0x0001FCD0
		public bool CustomSizeEnabled
		{
			get
			{
				return this.projectSize.Width != this.Size.Width || this.projectSize.Height != this.Size.Height;
			}
			set
			{
				if (!value)
				{
					this.Size = this.projectSize;
				}
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x00021AF4 File Offset: 0x0001FCF4
		// (set) Token: 0x06000878 RID: 2168 RVA: 0x00021B0C File Offset: 0x0001FD0C
		[PropertyOrder(7)]
		[UndoProperty]
		[Browsable(false)]
		[DisplayName("Display_AnchorPoint")]
		[Editor(typeof(AnchorPointEditor), typeof(AnchorPointEditor))]
		[Category("Group_Routine")]
		public override ScaleValue AnchorPoint
		{
			get
			{
				return base.AnchorPoint;
			}
			set
			{
				base.AnchorPoint = value;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x00021B18 File Offset: 0x0001FD18
		// (set) Token: 0x0600087A RID: 2170 RVA: 0x00021B30 File Offset: 0x0001FD30
		[DisplayName("Display_File")]
		[Editor(typeof(ResourceFileEditor), typeof(ResourceFileEditor))]
		[Category("Group_Feature")]
		[ResourceFilter(new string[]
		{
			"csd"
		})]
		[Browsable(true)]
		[DefaultValue(null)]
		[PropertyOrder(112)]
		[UndoProperty]
		[IgnoreResize]
		public ResourceFile FileData
		{
			get
			{
				return this.filePath;
			}
			set
			{
				this.filePath = value;
				this.Project = (value as CocosItem);
				if (value == ResourceFile.DefaultMarker)
				{
					this.filePath = null;
				}
				this.ReloadProject(true);
				this.InitInnerAction();
				string taskName = base.GetType().Name + "FileData";
				using (CompositeTask.Run(taskName, null))
				{
					if (this.Project != null)
					{
						string fileType = this.Project.GetFileType();
						if (fileType != "Layer")
						{
							this.StretchWidthEnable = false;
							this.StretchHeightEnable = false;
						}
					}
					else
					{
						this.StretchWidthEnable = false;
						this.StretchHeightEnable = false;
					}
					this.RaisePropertyChanged<InnerActionValue>(() => this.ActionValue);
					this.RaisePropertyChanged<ResourceFile>(() => this.FileData);
				}
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x00021C88 File Offset: 0x0001FE88
		// (set) Token: 0x0600087C RID: 2172 RVA: 0x00021CBC File Offset: 0x0001FEBC
		[DisplayName("Animation_InnerAction")]
		[UndoProperty]
		[Browsable(true)]
		[PropertyOrder(113)]
		[Editor(typeof(AnimateListEditor), typeof(AnimateListEditor))]
		[DefaultValue(null)]
		[FrameProperty(true)]
		[IgnoreResize]
		[Category("Group_Feature")]
		public InnerActionValue ActionValue
		{
			get
			{
				if (this.actionValue == null)
				{
					this.actionValue = new InnerActionValue();
				}
				return this.actionValue;
			}
			set
			{
				this.actionValue = value;
				this.IsPlaying = this.isPreviewPlaying;
				string taskName = base.GetType().Name + "ActionValue";
				using (CompositeTask.Run(taskName, null))
				{
					this.RaisePropertyChanged<InnerActionValue>(() => this.ActionValue);
				}
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x00021E50 File Offset: 0x00020050
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x00021D5C File Offset: 0x0001FF5C
		[PropertyOrder(114)]
		[Editor(typeof(PlayControlEditor), typeof(PlayControlEditor))]
		[DisplayName("MainTool_Preview")]
		[Category("Group_Feature")]
		[Browsable(true)]
		public bool IsPlaying
		{
			get
			{
				return this.isPreviewPlaying;
			}
			set
			{
				if (this.HasData())
				{
					this.isPreviewPlaying = value;
					this.isInnerLoop = (this.innerTimelineAction.Loop = (this.actionValue.ActionType == InnerActionType.LoopAction));
					if (this.actionValue.ActionType == InnerActionType.SingleFrame)
					{
						this.innerTimelineAction.CurrentFrameIndex = this.actionValue.SingleFrameIndex;
					}
					else if (this.isPreviewPlaying)
					{
						this.innerTimelineAction.Loop = this.isInnerLoop;
						this.innerTimelineAction.Play(this.actionValue.ActivedAnimationName);
					}
					else if (this.actionValue.ActionType == InnerActionType.SingleFrame)
					{
						this.innerTimelineAction.CurrentFrameIndex = this.actionValue.SingleFrameIndex;
					}
					else
					{
						this.innerTimelineAction.Pause(true);
					}
				}
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00021E68 File Offset: 0x00020068
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			FileNodeObject fileNodeObject = cObject as FileNodeObject;
			if (fileNodeObject != null)
			{
				fileNodeObject.FileData = this.FileData;
				fileNodeObject.ActionValue = this.ActionValue;
				fileNodeObject.IsSpeedSynchr = this.IsSpeedSynchr;
				fileNodeObject.InnerActionSpeed = this.InnerActionSpeed;
				fileNodeObject.StretchWidthEnable = this.StretchWidthEnable;
				fileNodeObject.StretchHeightEnable = this.StretchHeightEnable;
			}
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00021EE0 File Offset: 0x000200E0
		protected override void OnMouseDoubleClick(MouseEventArgs args)
		{
			if (this.Project != null)
			{
				Services.Workbench.OpenDocument(this.Project);
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00021F14 File Offset: 0x00020114
		internal override void AncestorObjectChanged(BaseObject sourceObj, NotifyCollectionChangedAction action)
		{
			base.AncestorObjectChanged(sourceObj, action);
			if (this.rootObject != null)
			{
				this.rootObject.AncestorObjectChanged(this.rootObject, action);
			}
			if (!this.iseventbinded && action == NotifyCollectionChangedAction.Add)
			{
				TimelineActionManager.Instance.AnimationPlayEvent += this.Instance_AnimationPlayEvent;
				if (this.IsSpeedSynchr)
				{
					TimelineActionManager.Instance.SpeedChangedEvent += this.Instance_SpeedChangedEvent;
				}
				if (this.InnerActionSpeed == 0f)
				{
					if (this.IsSpeedSynchr)
					{
						this.InnerActionSpeed = TimelineActionManager.Instance.Speed;
					}
					else if (this.innerTimelineAction != null)
					{
						this.innerActionSpeed = this.innerTimelineAction.Speed;
					}
				}
				this.iseventbinded = true;
			}
			else if (this.iseventbinded && action == NotifyCollectionChangedAction.Remove)
			{
				TimelineActionManager.Instance.AnimationPlayEvent -= this.Instance_AnimationPlayEvent;
				if (this.IsSpeedSynchr)
				{
					this.innerActionSpeed = 0f;
					TimelineActionManager.Instance.SpeedChangedEvent -= this.Instance_SpeedChangedEvent;
				}
				this.iseventbinded = false;
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0002206C File Offset: 0x0002026C
		private void Instance_SpeedChangedEvent(object sender, SpeedChangedArgs e)
		{
			this.InnerActionSpeed = e.Speed;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0002207C File Offset: 0x0002027C
		private void Instance_AnimationPlayEvent(bool playing)
		{
			if (this.innerTimelineAction != null)
			{
				this.isPreviewPlaying = false;
				if (playing)
				{
					this.ApplyActionValue(null);
				}
				else
				{
					this.innerTimelineAction.Pause(false);
				}
			}
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x000220C4 File Offset: 0x000202C4
		private void LoadProject(CocosItem project)
		{
			if (project != null && project.DataError == null)
			{
				bool isCreateDefaultRecorder = BaseRecorder.IsCreateDefaultRecorder;
				BaseRecorder.IsCreateDefaultRecorder = false;
				if (!project.IsLoaded)
				{
					project.Load(ProjectsService.Instance.DefaultMonitor);
					if (project.DataError == null)
					{
						this.rootObject = project.GetRootNode();
						this.innerTimelineAction = project.GetTimelineAction();
					}
					project.UnLoad(ProjectsService.Instance.DefaultMonitor);
				}
				else
				{
					this.rootObject = (project.GetRootNode().Clone() as AbstractNodeObject);
					this.innerTimelineAction = new TimelineAction();
					this.innerTimelineAction.InitWithRootNode(this.rootObject);
					ObservableCollection<AnimationInfo> animationInfoList = project.GetTimelineAction().AnimationInfoList;
					foreach (AnimationInfo animationInfo in animationInfoList)
					{
						this.innerTimelineAction.AnimationInfoList.Add(animationInfo.Clone() as AnimationInfo);
					}
				}
				BaseRecorder.IsCreateDefaultRecorder = isCreateDefaultRecorder;
				if (this.rootObject != null)
				{
					this.rootObject.GetCSVisual().SetZOrder(-1);
				}
				this.rootObject.GetCSVisual().SetVisible(true);
				this.GetInnerObject().SetProjectNode(this.rootObject.GetCSVisual());
			}
			this.InitInnerAction();
			if (this.innerTimelineAction != null)
			{
				this.ApplyActionValue(null);
				TimelineActionManager.Instance.RefreshCurrentFrame();
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00022274 File Offset: 0x00020474
		private void ReloadProject(bool refreshSize)
		{
			if (this.rootObject != null)
			{
				this.rootObject.AncestorObjectChanged(this.rootObject, NotifyCollectionChangedAction.Remove);
				this.innerNode.RemoveChild(this.rootObject.GetCSVisual());
				this.rootObject = null;
			}
			if (this.innerTimelineAction != null)
			{
				this.innerTimelineAction.Clear();
				this.innerTimelineAction = null;
			}
			this.LoadProject(this.Project);
			if (this.rootObject == null)
			{
				this.Size = SizeF.Empty;
			}
			else if (refreshSize)
			{
				this.projectSize = this.rootObject.Size;
				this.Size = this.projectSize;
			}
			else if (this.projectSize.Width != this.Size.Width || this.projectSize.Height != this.Size.Height)
			{
				this.GetInnerObject().RefreshLayout();
			}
			else
			{
				this.projectSize = this.rootObject.Size;
				this.Size = this.projectSize;
			}
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x000223AC File Offset: 0x000205AC
		internal void Reload()
		{
			if (this.Project != null)
			{
				DateTime? dateTime = ResourceItem.GetLastWriteTime(this.Project.FullPath);
				if (this.Project.IsLoaded && this.lastWriteTime == dateTime)
				{
					GameFileLoader.FindProjectNodeToReload(this.rootObject);
				}
				else
				{
					this.lastWriteTime = dateTime;
					this.ReloadProject(false);
				}
			}
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00022454 File Offset: 0x00020654
		protected override void OnBindingRecorder()
		{
			this.GetInnerObject().RefreshLayout();
			ExtenderFactory.Binding(this, new BaseExtender[]
			{
				new LayoutExtender(this),
				new ResourceExtender(this)
			});
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00022490 File Offset: 0x00020690
		public void ApplyActionValue(InnerActionValue argsActionValue = null)
		{
			if (this.HasData())
			{
				if (argsActionValue != null)
				{
					this.actionValue = argsActionValue;
				}
				InnerActionType actionType = this.actionValue.ActionType;
				this.isInnerLoop = (actionType == InnerActionType.LoopAction);
				this.innerTimelineAction.Loop = this.isInnerLoop;
				InnerActionType actionType2 = this.actionValue.ActionType;
				if (InnerActionType.SingleFrame == actionType2)
				{
					this.innerTimelineAction.CurrentFrameIndex = this.actionValue.SingleFrameIndex;
				}
				else
				{
					int currentFrameIndex = 0;
					string activedAnimationName = this.actionValue.ActivedAnimationName;
					bool flag = false;
					foreach (AnimationInfo animationInfo in this.innerTimelineAction.AnimationInfoList)
					{
						if (animationInfo.Name == activedAnimationName)
						{
							this.innerTimelineAction.ActivedAnimationInfo = animationInfo;
							currentFrameIndex = animationInfo.StartIndex;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.innerTimelineAction.ActivedAnimationInfo = null;
					}
					this.innerTimelineAction.CurrentFrameIndex = currentFrameIndex;
				}
			}
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x000225DC File Offset: 0x000207DC
		public void ApplyStep(InnerActionValue innerActionValue, int latestKeyIndex)
		{
			if (!innerActionValue.Equals(this.actionValue))
			{
				this.actionValue = innerActionValue;
			}
			InnerActionType actionType = this.ActionValue.ActionType;
			if (this.HasData() && actionType != InnerActionType.SingleFrame)
			{
				int num = TimelineActionManager.Instance.CurrentFrameIndex;
				int duration = TimelineActionManager.Instance.Duration;
				if (num > duration)
				{
					num = duration;
				}
				int num2 = num - latestKeyIndex;
				if (!this.isSpeedSynchr)
				{
					float speed = TimelineActionManager.Instance.Speed;
					num2 = (int)((float)num2 * this.innerActionSpeed / speed);
				}
				AnimationInfo activedAnimationInfo = this.innerTimelineAction.ActivedAnimationInfo;
				int num3 = 0;
				int num4 = this.innerTimelineAction.Duration;
				if (activedAnimationInfo != null)
				{
					num3 = activedAnimationInfo.StartIndex;
					num4 = activedAnimationInfo.EndIndex;
				}
				int num5 = num4 - num3;
				if (num5 != -1)
				{
					if (actionType == InnerActionType.LoopAction)
					{
						this.innerTimelineAction.Pause(true);
						num2 %= num5 + 1;
						this.innerTimelineAction.CurrentFrameIndex += num2;
					}
					else if (actionType == InnerActionType.NoLoopAction)
					{
						if (num2 <= num5)
						{
							this.innerTimelineAction.Pause(true);
							this.innerTimelineAction.CurrentFrameIndex += num2;
						}
					}
				}
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x0002274C File Offset: 0x0002094C
		// (set) Token: 0x0600088B RID: 2187 RVA: 0x00022764 File Offset: 0x00020964
		public bool IsSpeedSynchr
		{
			get
			{
				return this.isSpeedSynchr;
			}
			private set
			{
				this.isSpeedSynchr = value;
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x00022770 File Offset: 0x00020970
		// (set) Token: 0x0600088D RID: 2189 RVA: 0x00022788 File Offset: 0x00020988
		[UndoProperty]
		public float InnerActionSpeed
		{
			get
			{
				return this.innerActionSpeed;
			}
			set
			{
				this.innerActionSpeed = value;
				if (this.innerTimelineAction == null)
				{
					this.innerActionSpeed = 0f;
				}
				else
				{
					this.innerTimelineAction.Speed = this.innerActionSpeed;
				}
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x000227D0 File Offset: 0x000209D0
		public FileNodeObject Instance
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x000227E4 File Offset: 0x000209E4
		public bool HasData()
		{
			return this.innerTimelineAction != null && this.innerTimelineAction.Duration > 0;
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x00022810 File Offset: 0x00020A10
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x00022830 File Offset: 0x00020A30
		[UndoProperty]
		public override bool PercentWidthEnable
		{
			get
			{
				return this.GetInnerObject().GetPercentWidthEnable();
			}
			set
			{
				this.GetInnerObject().SetPercentWidthEnable(value);
				string taskName = base.GetType().Name + "PercentWidthEnable";
				using (CompositeTask.Run(taskName, null))
				{
					if (value)
					{
						this.StretchWidthEnable = false;
					}
					this.RaisePropertyChanged<bool>(() => this.PercentWidthEnable);
				}
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x000228DC File Offset: 0x00020ADC
		// (set) Token: 0x06000893 RID: 2195 RVA: 0x000228FC File Offset: 0x00020AFC
		[UndoProperty]
		public override bool PercentHeightEnable
		{
			get
			{
				return this.GetInnerObject().GetPercentHeightEnable();
			}
			set
			{
				this.GetInnerObject().SetPercentHeightEnable(value);
				this.RaisePropertyChanged<bool>(() => this.PercentHeightEnable);
				string taskName = base.GetType().Name + "PercentHeightEnable";
				using (CompositeTask.Run(taskName, null))
				{
					if (value)
					{
						this.StretchHeightEnable = false;
					}
					this.RaisePropertyChanged<bool>(() => this.PercentHeightEnable);
				}
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x000229E0 File Offset: 0x00020BE0
		// (set) Token: 0x06000895 RID: 2197 RVA: 0x00022A00 File Offset: 0x00020C00
		[UndoProperty]
		[LayoutRefresh]
		public virtual bool StretchWidthEnable
		{
			get
			{
				return this.GetInnerObject().GetStretchWidthEnable();
			}
			set
			{
				this.GetInnerObject().SetStretchWidthEnable(value);
				string taskName = base.GetType().Name + "StretchWidthEnable";
				using (CompositeTask.Run(taskName, null))
				{
					if (value)
					{
						this.PercentWidthEnable = false;
					}
					this.RaisePropertyChanged<bool>(() => this.StretchWidthEnable);
				}
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x00022AAC File Offset: 0x00020CAC
		// (set) Token: 0x06000897 RID: 2199 RVA: 0x00022ACC File Offset: 0x00020CCC
		[UndoProperty]
		[LayoutRefresh]
		public virtual bool StretchHeightEnable
		{
			get
			{
				return this.GetInnerObject().GetStretchHeightEnable();
			}
			set
			{
				this.GetInnerObject().SetStretchHeightEnable(value);
				string taskName = base.GetType().Name + "StretchHeightEnable";
				using (CompositeTask.Run(taskName, null))
				{
					if (value)
					{
						this.PercentHeightEnable = false;
					}
					this.RaisePropertyChanged<bool>(() => this.StretchHeightEnable);
				}
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x00022B78 File Offset: 0x00020D78
		// (set) Token: 0x06000899 RID: 2201 RVA: 0x00022B90 File Offset: 0x00020D90
		public virtual bool CanShowStretch
		{
			get
			{
				return this.canStretch;
			}
			set
			{
				this.canStretch = value;
			}
		}

		// Token: 0x0400032E RID: 814
		private AbstractNodeObject rootObject;

		// Token: 0x0400032F RID: 815
		private TimelineAction innerTimelineAction = null;

		// Token: 0x04000330 RID: 816
		private DateTime? lastWriteTime = null;

		// Token: 0x04000331 RID: 817
		private SizeF projectSize = SizeF.Empty;

		// Token: 0x04000332 RID: 818
		private ResourceFile filePath = null;

		// Token: 0x04000333 RID: 819
		private InnerActionValue actionValue;

		// Token: 0x04000334 RID: 820
		private bool isPreviewPlaying;

		// Token: 0x04000335 RID: 821
		private bool iseventbinded = false;

		// Token: 0x04000336 RID: 822
		private bool isInnerLoop = true;

		// Token: 0x04000337 RID: 823
		private bool isSpeedSynchr = true;

		// Token: 0x04000338 RID: 824
		private float innerActionSpeed = 0f;

		// Token: 0x04000339 RID: 825
		private bool canStretch = true;
	}
}
