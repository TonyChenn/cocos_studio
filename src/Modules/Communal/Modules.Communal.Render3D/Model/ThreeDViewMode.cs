using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.Render.ExtensionModel;
using Modules.Communal.Render.Model;
using Modules.Communal.Render3D.View;
using Modules.UI.ComTool;
using Modules.UI.ComTool.Model;
using Mono.Addins;

namespace Modules.Communal.Render3D.Model
{
	// Token: 0x02000006 RID: 6
	[Extension(typeof(IViewMode))]
	public class ThreeDViewMode : BaseViewMode
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00002823 File Offset: 0x00000A23
		public override bool CanHandle(CocosItem cocosItem)
		{
			return cocosItem.Is3DFile();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000282C File Offset: 0x00000A2C
		public override void Initialize(IGLView glView)
		{
			base.Initialize(glView);
			this.contextMenu = BaseViewMode.LoadContextMenu(NodeType.Scene3D.ToString());
			List<BaseTool> list = new List<BaseTool>();
			CameraTool item = new CameraTool();
			list.Add(item);
			TranslateTool translateTool = new TranslateTool();
			list.Add(translateTool);
			RotateTool item2 = new RotateTool();
			list.Add(item2);
			ScaleTool item3 = new ScaleTool();
			list.Add(item3);
			this.toolGroup = new ToolGroup(list);
			this.toolGroup.Current = translateTool;
			this.operateList = new List<IOperateModule>();
			this.operateList.Add(item);
			this.operateList.Add(this.toolGroup);
			Light3DTool item4 = new Light3DTool();
			this.operateList.Add(item4);
			BoxSelectedNode3D drawRect = new BoxSelectedNode3D(GameWindow.Current.GetSceneObject().GetCamera());
			HitTestMode hitTestMode = new HitTestMode3D();
			SelectTool item5 = new SelectTool(drawRect, hitTestMode, this.toolGroup);
			this.operateList.Add(item5);
			foreach (IOperateModule operateModule in this.operateList)
			{
				operateModule.Initialize(glView);
			}
			this.currentOperate = this.toolGroup;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002978 File Offset: 0x00000B78
		public override IObjectContextMenu GetContextMenu()
		{
			return this.contextMenu;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002980 File Offset: 0x00000B80
		public override void Activated(CocosItem cocosItem)
		{
			base.Activated(cocosItem);
			GameWindow.Current.SetSceneMode(false);
			GameWindow.Current.GetCanvasObject().Scale = new ScaleValue(1f);
			Services.GetService<IComToolPad>().ControlsViewFilter = this._controlToolViewFilter;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000029C0 File Offset: 0x00000BC0
		public override void OnDocumentChanged(CocosItem cocosItem)
		{
			base.OnDocumentChanged(cocosItem);
			GameNode3DObject gameNode3DObject = cocosItem.GetRootNode() as GameNode3DObject;
			if (gameNode3DObject != null)
			{
				gameNode3DObject.ResetDefaultCameraBrush();
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000029E9 File Offset: 0x00000BE9
		public override void ResetView()
		{
			this.MoveCanvasToZeroPoint();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000029F1 File Offset: 0x00000BF1
		private void MoveCanvasToZeroPoint()
		{
			if (GameWindow.Current == null)
			{
				return;
			}
			GameWindow.Current.GetCanvasObject().Position = PointF.Empty;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A0F File Offset: 0x00000C0F
		public override Widget GetToolbar()
		{
			if (this.toolbar == null)
			{
				this.toolbar = new Toolbar3D(this.toolGroup);
			}
			return this.toolbar;
		}

		// Token: 0x0400000B RID: 11
		private IObjectContextMenu contextMenu;

		// Token: 0x0400000C RID: 12
		private Toolbar3D toolbar;

		// Token: 0x0400000D RID: 13
		private IControlsViewFilter _controlToolViewFilter = new ThreeDControlsViewFilter();
	}
}
