using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.Render;
using Modules.Communal.Render.Model;
using Modules.Communal.Skeleton.ViewModel;
using Modules.UI.ComTool;
using Mono.Addins;

namespace Modules.Communal.Skeleton
{
	// Token: 0x02000029 RID: 41
	[Extension(typeof(IViewMode))]
	public class SkeletonViewMode : DefaultViewMode
	{
		// Token: 0x060001D5 RID: 469 RVA: 0x0000969C File Offset: 0x0000789C
		public override bool CanHandle(CocosItem cocosItem)
		{
			return cocosItem.ContentType == NodeType.Skeleton.ToString();
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000096B4 File Offset: 0x000078B4
		protected override void OnInitialize(IGLView glView)
		{
			List<BaseTool> list = new List<BaseTool>();
			CanvasTool item = new CanvasTool();
			list.Add(item);
			TranslateTool item2 = new TranslateTool();
			list.Add(item2);
			RotateTool rotateTool = new RotateTool();
			list.Add(rotateTool);
			ScaleTool item3 = new ScaleTool();
			list.Add(item3);
			CreateBoneGroupTool item4 = new CreateBoneGroupTool();
			list.Add(item4);
			BindingBoneTool item5 = new BindingBoneTool();
			list.Add(item5);
			UnBindingBoneTool item6 = new UnBindingBoneTool();
			list.Add(item6);
			HideBoneTool item7 = new HideBoneTool();
			list.Add(item7);
			HideSkinTool item8 = new HideSkinTool();
			list.Add(item8);
			SchematicTool item9 = new SchematicTool();
			list.Add(item9);
			this.toolGroup = new SkeletonToolGroup(list);
			this.toolGroup.Current = rotateTool;
			this.operateList = new List<IOperateModule>();
			this.operateList.Add(item);
			this.operateList.Add(this.toolGroup);
			this.operateList.Add(GuidesService.Instance);
			BoxSelectedNode instance = BoxSelectedNode.Instance;
			instance.Initialize();
			HitTestModeSkeleton hitTestMode = new HitTestModeSkeleton();
			SelectTool item10 = new SelectTool(instance, hitTestMode, this.toolGroup);
			this.operateList.Add(item10);
			foreach (IOperateModule operateModule in this.operateList)
			{
				operateModule.Initialize(glView);
			}
			this.currentOperate = this.toolGroup;
			this.contextMenu = BaseViewMode.LoadContextMenu(NodeType.Skeleton.ToString());
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00009848 File Offset: 0x00007A48
		public override Widget GetToolbar()
		{
			if (this.toolbar == null)
			{
				this.toolbar = new ToolbarSkeleton(this.toolGroup);
			}
			return this.toolbar;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000986C File Offset: 0x00007A6C
		public override void OnDocumentChanged(CocosItem cocosItem)
		{
			base.OnDocumentChanged(cocosItem);
			SkeletonObject skeletonObject = cocosItem.GetRootNode() as SkeletonObject;
			if (skeletonObject != null)
			{
				skeletonObject.ReCaculateMaxZorder();
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00009895 File Offset: 0x00007A95
		public override void Activated(CocosItem cocosItem)
		{
			base.Activated(cocosItem);
			Services.GetService<IComToolPad>().ControlsViewFilter = this._controlToolViewFilter;
		}

		// Token: 0x04000087 RID: 135
		private SkeletonControlsViewFilter _controlToolViewFilter = new SkeletonControlsViewFilter();
	}
}
