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
	[Extension(typeof(IViewMode))]
	public class DefaultViewMode : BaseViewMode
	{
		public override bool CanHandle(CocosItem cocosItem)
		{
			return cocosItem.Is2DFile();
		}

		public override void Initialize(IGLView glView)
		{
			base.Initialize(glView);
			this.OnInitialize(glView);
		}

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

		public override bool CanShowContextMenu(PointF scenePoint)
		{
			ContextMenuShowingArgs contextMenuShowingArgs = new ContextMenuShowingArgs(SelectService.Instance.SelectedParentObjectList, scenePoint, true);
			this.contextMenu.CanShow(contextMenuShowingArgs);
			return contextMenuShowingArgs.Enable;
		}

		public override IObjectContextMenu GetContextMenu()
		{
			return this.contextMenu;
		}

		public override void OnCanvasSizeChanged()
		{
			this.MoveCanvasToCenter();
		}

		public override void ResetView()
		{
			this.MoveCanvasToCenter();
		}

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

		public override void Deactivated()
		{
			base.Deactivated();
			this.canvasScale = GameWindow.Current.GetCanvasObject().Scale;
		}

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

		public override Widget GetToolbar()
		{
			if (this.toolbar == null)
			{
				this.toolbar = new Toolbar2D(this.toolGroup);
			}
			return this.toolbar;
		}

		private ScaleValue canvasScale;

		protected IObjectContextMenu contextMenu;

		protected Widget toolbar;

		private IControlsViewFilter _controlToolViewFilter = new Default2DContolsViewFilter();
	}
}
