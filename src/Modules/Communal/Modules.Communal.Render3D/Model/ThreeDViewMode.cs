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
	[Extension(typeof(IViewMode))]
	public class ThreeDViewMode : BaseViewMode
	{
		public override bool CanHandle(CocosItem cocosItem)
		{
			return cocosItem.Is3DFile();
		}

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

		public override IObjectContextMenu GetContextMenu()
		{
			return this.contextMenu;
		}

		public override void Activated(CocosItem cocosItem)
		{
			base.Activated(cocosItem);
			GameWindow.Current.SetSceneMode(false);
			GameWindow.Current.GetCanvasObject().Scale = new ScaleValue(1f);
			Services.GetService<IComToolPad>().ControlsViewFilter = this._controlToolViewFilter;
		}

		public override void OnDocumentChanged(CocosItem cocosItem)
		{
			base.OnDocumentChanged(cocosItem);
			GameNode3DObject gameNode3DObject = cocosItem.GetRootNode() as GameNode3DObject;
			if (gameNode3DObject != null)
			{
				gameNode3DObject.ResetDefaultCameraBrush();
			}
		}

		public override void ResetView()
		{
			this.MoveCanvasToZeroPoint();
		}

		private void MoveCanvasToZeroPoint()
		{
			if (GameWindow.Current == null)
			{
				return;
			}
			GameWindow.Current.GetCanvasObject().Position = PointF.Empty;
		}

		public override Widget GetToolbar()
		{
			if (this.toolbar == null)
			{
				this.toolbar = new Toolbar3D(this.toolGroup);
			}
			return this.toolbar;
		}

		private IObjectContextMenu contextMenu;

		private Toolbar3D toolbar;

		private IControlsViewFilter _controlToolViewFilter = new ThreeDControlsViewFilter();
	}
}
