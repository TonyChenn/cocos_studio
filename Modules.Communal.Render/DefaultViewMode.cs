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
using Modules.Communal.Render.Model.ViewMode;
using Modules.Communal.Render.View;
using Modules.UI.ComTool;
using Modules.UI.ComTool.Model;
using Mono.Addins;

namespace Modules.Communal.Render
{
	// Token: 0x02000031 RID: 49
	[Extension(typeof(IViewMode))]
	public class DefaultViewMode : BaseViewMode
	{
		// Token: 0x06000206 RID: 518 RVA: 0x0000BC60 File Offset: 0x00009E60
		public override bool CanHandle(CocosItem cocosItem)
		{
			return cocosItem.Is2DFile();
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000BC78 File Offset: 0x00009E78
		public override void Initialize(IGLView glView)
		{
			base.Initialize(glView);
			this.OnInitialize(glView);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000BC8C File Offset: 0x00009E8C
		protected virtual void OnInitialize(IGLView glView)
		{
			CanvasTool canvasTool = new CanvasTool();
			DefaultObjectTool defaultObjectTool = new DefaultObjectTool();
			this.toolGroup = new ToolGroup(new BaseTool[]
			{
				canvasTool,
				defaultObjectTool
			});
			this.toolGroup.Current = defaultObjectTool;
			this.operateList = new List<IOperateModule>();
			this.operateList.Add(canvasTool);
			this.operateList.Add(this.toolGroup);
			this.operateList.Add(GuidesService.Instance);
			BoxSelectedNode instance = BoxSelectedNode.Instance;
			instance.Initialize();
			HitTestMode hitTestMode = new HitTestMode();
			SelectTool item = new SelectTool(instance, hitTestMode, this.toolGroup);
			this.operateList.Add(item);
			foreach (IOperateModule operateModule in this.operateList)
			{
				operateModule.Initialize(glView);
			}
			this.currentOperate = this.toolGroup;
			this.contextMenu = BaseViewMode.LoadContextMenu(NodeType.Scene.ToString());
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000BDB4 File Offset: 0x00009FB4
		public override bool CanShowContextMenu(PointF scenePoint)
		{
			ContextMenuShowingArgs contextMenuShowingArgs = new ContextMenuShowingArgs(SelectService.Instance.SelectedParentObjectList, scenePoint, true);
			this.contextMenu.CanShow(contextMenuShowingArgs);
			return contextMenuShowingArgs.Enable;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000BDEC File Offset: 0x00009FEC
		public override IObjectContextMenu GetContextMenu()
		{
			return this.contextMenu;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000BE04 File Offset: 0x0000A004
		public override void OnCanvasSizeChanged()
		{
			this.MoveCanvasToCenter();
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000BE0E File Offset: 0x0000A00E
		public override void ResetView()
		{
			this.MoveCanvasToCenter();
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000BE18 File Offset: 0x0000A018
		public override void Activated(CocosItem cocosItem)
		{
			base.Activated(cocosItem);
			if (GameWindow.Current != null)
			{
				GameWindow.Current.SetSceneMode(true);
				if (this.canvasScale != null)
				{
					this.toolGroup.GetTool<CanvasTool>().ZoomCanvasObject(this.canvasScale);
				}
				this.MoveCanvasToCenter();
				Services.GetService<IComToolPad>().ControlsViewFilter = this._controlToolViewFilter;
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000BE89 File Offset: 0x0000A089
		public override void Deactivated()
		{
			base.Deactivated();
			this.canvasScale = GameWindow.Current.GetCanvasObject().Scale;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000BEA8 File Offset: 0x0000A0A8
		private void MoveCanvasToCenter()
		{
			if (GameWindow.Current != null && GameWindow.Current.GetCanvasObject() != null)
			{
				GameWindow gameWindow = GameWindow.Current;
				CanvasObject canvasObject = gameWindow.GetCanvasObject();
				float num = ((float)gameWindow.Width - canvasObject.Size.Width * canvasObject.Scale.ScaleX) / 2f;
				float num2 = ((float)gameWindow.Height - canvasObject.Size.Height * canvasObject.Scale.ScaleX) / 2f;
				num = (float)((int)Math.Floor((double)num));
				num2 = (float)((int)Math.Floor((double)num2));
				canvasObject.Position = new PointF(num, num2);
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000BF54 File Offset: 0x0000A154
		public override Widget GetToolbar()
		{
			if (this.toolbar == null)
			{
				this.toolbar = new Toolbar2D(this.toolGroup);
			}
			return this.toolbar;
		}

		// Token: 0x04000092 RID: 146
		private ScaleValue canvasScale;

		// Token: 0x04000093 RID: 147
		protected IObjectContextMenu contextMenu;

		// Token: 0x04000094 RID: 148
		protected Widget toolbar;

		// Token: 0x04000095 RID: 149
		private IControlsViewFilter _controlToolViewFilter = new Default2DContolsViewFilter();
	}
}
