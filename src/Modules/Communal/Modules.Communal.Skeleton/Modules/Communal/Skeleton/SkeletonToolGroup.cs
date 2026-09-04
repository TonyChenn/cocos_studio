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
	// Token: 0x02000007 RID: 7
	internal class SkeletonToolGroup : ToolGroup
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002ED1 File Offset: 0x000010D1
		public SkeletonToolGroup(IEnumerable<BaseTool> toolList) : base(toolList)
		{
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002EDC File Offset: 0x000010DC
		public override void Activated(CocosItem cocosItem)
		{
			base.Activated(cocosItem);
			SelectedVisualObjectsChangeEvent @event = Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>();
			@event.Subscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.OnSelectObjectsChangeEvent));
			Services.EventsService.GetEvent<CanvasZoomChangeEvent>().Subscribe(new Action<CanvasZoomChangeEventArgs>(this.OnCanvasZoomedChangedEvent));
			TimelineActionManager.Instance.AnimateStatesChangedEvent += this.Instance_AnimateStatesChangedEvent;
			TimelineActionManager.Instance.CurrentFrameIndexChangedEvent += this.Instance_CurrentFrameIndexChangedEvent;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002F58 File Offset: 0x00001158
		private void Instance_CurrentFrameIndexChangedEvent()
		{
			ISkeletonTool skeletonTool = base.Current as ISkeletonTool;
			if (skeletonTool != null)
			{
				skeletonTool.OnRefreshControlDraw();
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002F7C File Offset: 0x0000117C
		public override void Deactivated()
		{
			base.Deactivated();
			SelectedVisualObjectsChangeEvent @event = Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>();
			@event.Unsubscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.OnSelectObjectsChangeEvent));
			Services.EventsService.GetEvent<CanvasZoomChangeEvent>().Unsubscribe(new Action<CanvasZoomChangeEventArgs>(this.OnCanvasZoomedChangedEvent));
			TimelineActionManager.Instance.AnimateStatesChangedEvent -= this.Instance_AnimateStatesChangedEvent;
			TimelineActionManager.Instance.CurrentFrameIndexChangedEvent -= this.Instance_CurrentFrameIndexChangedEvent;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002FF4 File Offset: 0x000011F4
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

		// Token: 0x06000041 RID: 65 RVA: 0x00003064 File Offset: 0x00001264
		private void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args)
		{
			ISkeletonTool skeletonTool = base.Current as ISkeletonTool;
			if (skeletonTool != null)
			{
				skeletonTool.OnSelectObjectsChangeEvent(args);
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003088 File Offset: 0x00001288
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
