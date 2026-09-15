using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Skeleton
{
	internal class SkeletonToolGroup : ToolGroup
	{
		public SkeletonToolGroup(IEnumerable<BaseTool> toolList) : base(toolList)
		{
		}

		public override void Activated(CocosItem cocosItem)
		{
			base.Activated(cocosItem);
			SelectedVisualObjectsChangeEvent @event = Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>();
			@event.Subscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.OnSelectObjectsChangeEvent));
			Services.EventsService.GetEvent<CanvasZoomChangeEvent>().Subscribe(new Action<CanvasZoomChangeEventArgs>(this.OnCanvasZoomedChangedEvent));
			TimelineActionManager.Instance.AnimateStatesChangedEvent += this.Instance_AnimateStatesChangedEvent;
			TimelineActionManager.Instance.CurrentFrameIndexChangedEvent += this.Instance_CurrentFrameIndexChangedEvent;
		}

		private void Instance_CurrentFrameIndexChangedEvent()
		{
			ISkeletonTool skeletonTool = base.Current as ISkeletonTool;
			if (skeletonTool != null)
			{
				skeletonTool.OnRefreshControlDraw();
			}
		}

		public override void Deactivated()
		{
			base.Deactivated();
			SelectedVisualObjectsChangeEvent @event = Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>();
			@event.Unsubscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.OnSelectObjectsChangeEvent));
			Services.EventsService.GetEvent<CanvasZoomChangeEvent>().Unsubscribe(new Action<CanvasZoomChangeEventArgs>(this.OnCanvasZoomedChangedEvent));
			TimelineActionManager.Instance.AnimateStatesChangedEvent -= this.Instance_AnimateStatesChangedEvent;
			TimelineActionManager.Instance.CurrentFrameIndexChangedEvent -= this.Instance_CurrentFrameIndexChangedEvent;
		}

		private void OnCanvasZoomedChangedEvent(CanvasZoomChangeEventArgs args)
		{
			ISkeletonTool skeletonTool = base.Current as ISkeletonTool;
			if (skeletonTool != null)
			{
				BoneControlObject.Instance.ResetAxis(false, true);
				skeletonTool.OnCanvasZoomedChangedEvent();
				if (Services.Workbench.ActiveDocument != null && Services.Workbench.ActiveDocument.File != null)
				{
					SkeletonObject skeletonObject = Services.Workbench.ActiveDocument.File.GetRootNode() as SkeletonObject;
					if (skeletonObject != null)
					{
						skeletonObject.ResetScaleWidth();
					}
				}
			}
		}

		private void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args)
		{
			ISkeletonTool skeletonTool = base.Current as ISkeletonTool;
			if (skeletonTool != null)
			{
				skeletonTool.OnSelectObjectsChangeEvent(args);
			}
		}

		private void Instance_AnimateStatesChangedEvent(object sender, AnimateStatesArgs e)
		{
			BindingBoneTool tool = base.GetTool<BindingBoneTool>();
			UnBindingBoneTool tool2 = base.GetTool<UnBindingBoneTool>();
			CreateBoneGroupTool tool3 = base.GetTool<CreateBoneGroupTool>();
			SchematicTool tool4 = base.GetTool<SchematicTool>();
			bool flag = TimelineActionManager.Instance.AutoKey || TimelineActionManager.Instance.OnionSkinEnable || !TimelineActionManager.Instance.NeedRefreshAnimate;
			if (flag && (base.Current == tool || base.Current == tool2 || base.Current == tool3))
			{
				base.Current = base.GetTool<RotateTool>();
			}
			bool enabled = !flag;
			tool.Enabled = enabled;
			tool2.Enabled = enabled;
			tool3.Enabled = enabled;
			tool4.Enabled = enabled;
		}
	}
}
