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
	[DisplayName("Display_Component_Entity")]
	[EngineClassName("Node")]
	public class FileNodeObject : NodeObject, IPlayControl, IInnerActoinNode, IStretchSize
	{
		[ResourceIgnore]
		public CocosItem Project { get; private set; }

		private CSProjectNode GetInnerObject()
		{
			return (CSProjectNode)this.innerNode;
		}

		public FileNodeObject()
		{
		}

		public FileNodeObject(CocosItem project)
		{
			this.FileData = project;
		}

		public FileNodeObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSProjectNode();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			base.InitIcon("Object.png");
		}

		public override void InitOperation()
		{
			this.OperationFlag = (OperationMask)65527;
		}

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

		protected override void OnMouseDoubleClick(MouseEventArgs args)
		{
			if (this.Project != null)
			{
				Services.Workbench.OpenDocument(this.Project);
			}
		}

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

		private void Instance_SpeedChangedEvent(object sender, SpeedChangedArgs e)
		{
			this.InnerActionSpeed = e.Speed;
		}

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

		protected override void OnBindingRecorder()
		{
			this.GetInnerObject().RefreshLayout();
			ExtenderFactory.Binding(this, new BaseExtender[]
			{
				new LayoutExtender(this),
				new ResourceExtender(this)
			});
		}

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

		public FileNodeObject Instance
		{
			get
			{
				return this;
			}
		}

		public bool HasData()
		{
			return this.innerTimelineAction != null && this.innerTimelineAction.Duration > 0;
		}

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

		private AbstractNodeObject rootObject;

		private TimelineAction innerTimelineAction = null;

		private DateTime? lastWriteTime = null;

		private SizeF projectSize = SizeF.Empty;

		private ResourceFile filePath = null;

		private InnerActionValue actionValue;

		private bool isPreviewPlaying;

		private bool iseventbinded = false;

		private bool isInnerLoop = true;

		private bool isSpeedSynchr = true;

		private float innerActionSpeed = 0f;

		private bool canStretch = true;
	}
}
