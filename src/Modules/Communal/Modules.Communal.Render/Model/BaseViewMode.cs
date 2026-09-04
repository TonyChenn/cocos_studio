using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Gdk;
using Gtk;
using Modules.Communal.Render.ExtensionModel;
using Mono.Addins;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000017 RID: 23
	public abstract class BaseViewMode : IViewMode, IActivateControl, IDocumentEventHandler, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IDragEventHandler
	{
		// Token: 0x060000BF RID: 191 RVA: 0x00005FC4 File Offset: 0x000041C4
		protected static IObjectContextMenu LoadContextMenu(string projectType)
		{
			IObjectContextMenu[] extensionObjects = AddinManager.GetExtensionObjects<IObjectContextMenu>(true);
			IObjectContextMenu result;
			if (extensionObjects == null)
			{
				result = null;
			}
			else
			{
				foreach (IObjectContextMenu objectContextMenu in extensionObjects)
				{
					if (objectContextMenu.Type == projectType)
					{
						return objectContextMenu;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060000C0 RID: 192
		public abstract bool CanHandle(CocosItem cocosItem);

		// Token: 0x060000C1 RID: 193 RVA: 0x00006028 File Offset: 0x00004228
		public virtual void Initialize(IGLView glView)
		{
			if (!BaseViewMode.isInitialized)
			{
				BaseViewMode.isInitialized = true;
				BaseViewMode.dragService = new DragService(glView, Services.EventsService);
				BaseViewMode.commandService = new CommandServcie();
				BaseViewMode.commandService.Initialize(glView);
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006070 File Offset: 0x00004270
		public virtual IObjectContextMenu GetContextMenu()
		{
			return null;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006084 File Offset: 0x00004284
		public virtual bool CanShowContextMenu(PointF scenePoint)
		{
			return false;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006097 File Offset: 0x00004297
		public virtual void OnCanvasSizeChanged()
		{
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000609A File Offset: 0x0000429A
		public virtual void ResetView()
		{
		}

		// Token: 0x060000C6 RID: 198
		public abstract Widget GetToolbar();

		// Token: 0x060000C7 RID: 199 RVA: 0x000060A0 File Offset: 0x000042A0
		public virtual ICommandService GetCommandService()
		{
			return BaseViewMode.commandService;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000060B8 File Offset: 0x000042B8
		public virtual void Activated(CocosItem cocosItem)
		{
			IObjectContextMenu contextMenu = this.GetContextMenu();
			if (contextMenu != null)
			{
				contextMenu.Activated(cocosItem);
			}
			foreach (IOperateModule operateModule in this.operateList)
			{
				operateModule.Activated(cocosItem);
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006130 File Offset: 0x00004330
		public virtual void Deactivated()
		{
			IObjectContextMenu contextMenu = this.GetContextMenu();
			if (contextMenu != null)
			{
				contextMenu.Deactivated();
			}
			foreach (IOperateModule operateModule in this.operateList)
			{
				operateModule.Deactivated();
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000061A4 File Offset: 0x000043A4
		public virtual void OnMouseMove(MotionNotifyEventArgs args)
		{
			if (KeyboardExtend.IsMousePressed(args.Event.State))
			{
				this.currentOperate.OnMouseMove(args);
			}
			else
			{
				foreach (IOperateModule operateModule in this.operateList)
				{
					operateModule.OnMouseMove(args);
					if (args.CheckRetval())
					{
						break;
					}
				}
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000623C File Offset: 0x0000443C
		public virtual void OnMouseUp(ButtonReleaseEventArgs args)
		{
			this.currentOperate.OnMouseUp(args);
			this.currentOperate = this.toolGroup;
			if (!args.CheckRetval())
			{
				int count = this.operateList.Count;
				if (count > 0 && args.Event.GetMouseButton() == MouseButton.Left)
				{
					this.operateList[count - 1].OnMouseUp(args);
				}
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000062B4 File Offset: 0x000044B4
		public virtual void OnMouseDown(ButtonPressEventArgs args)
		{
			foreach (IOperateModule operateModule in this.operateList)
			{
				operateModule.OnMouseDown(args);
				if (args.CheckRetval())
				{
					this.currentOperate = operateModule;
					break;
				}
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00006328 File Offset: 0x00004528
		public virtual void OnMouseEnter(EnterNotifyEventArgs args)
		{
			this.currentOperate.OnMouseEnter(args);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00006338 File Offset: 0x00004538
		public virtual void OnMouseLeave(LeaveNotifyEventArgs args)
		{
			this.currentOperate.OnMouseLeave(args);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006348 File Offset: 0x00004548
		public virtual void OnMouseWheel(ScrollEventArgs args)
		{
			if (KeyboardExtend.IsMousePressed(args.Event.State))
			{
				this.currentOperate.OnMouseWheel(args);
			}
			else
			{
				foreach (IOperateModule operateModule in this.operateList)
				{
					operateModule.OnMouseWheel(args);
					if (args.CheckRetval())
					{
						break;
					}
				}
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000063DC File Offset: 0x000045DC
		public virtual void OnMouseDoubleClick(ButtonPressEventArgs args)
		{
			this.currentOperate.OnMouseDoubleClick(args);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000063EC File Offset: 0x000045EC
		public virtual void OnMouseGestures(MouseGesturesEventArgs args)
		{
			foreach (IOperateModule operateModule in this.operateList)
			{
				operateModule.OnMouseGestures(args);
				if (args.CheckRetval())
				{
					break;
				}
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00006458 File Offset: 0x00004658
		public virtual void OnKeyDown(KeyPressEventArgs args)
		{
			if (KeyboardExtend.IsMousePressed(args.Event.State))
			{
				this.currentOperate.OnKeyDown(args);
			}
			else
			{
				foreach (IOperateModule operateModule in this.operateList)
				{
					operateModule.OnKeyDown(args);
					if (args.CheckRetval())
					{
						break;
					}
				}
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000064EC File Offset: 0x000046EC
		public virtual void OnKeyUp(KeyReleaseEventArgs args)
		{
			if (KeyboardExtend.IsMousePressed(args.Event.State))
			{
				this.currentOperate.OnKeyUp(args);
			}
			else
			{
				foreach (IOperateModule operateModule in this.operateList)
				{
					operateModule.OnKeyUp(args);
					if (args.CheckRetval())
					{
						break;
					}
				}
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00006580 File Offset: 0x00004780
		public virtual void OnDocumentChanged(CocosItem cocosItem)
		{
			if (GameWindow.Current != null)
			{
				CanvasObject canvasObject = GameWindow.Current.GetCanvasObject();
				IProjectFileRenderView renderView = cocosItem.GetRenderView();
				renderView.ChangeView(canvasObject, cocosItem);
				this.ResetView();
				foreach (IOperateModule operateModule in this.operateList)
				{
					IDocumentEventHandler documentEventHandler = operateModule as IDocumentEventHandler;
					if (documentEventHandler != null)
					{
						documentEventHandler.OnDocumentChanged(cocosItem);
					}
				}
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00006628 File Offset: 0x00004828
		public virtual void OnDocumentSaved(CocosItem cocosItem)
		{
			foreach (IOperateModule operateModule in this.operateList)
			{
				IDocumentEventHandler documentEventHandler = operateModule as IDocumentEventHandler;
				if (documentEventHandler != null)
				{
					documentEventHandler.OnDocumentSaved(cocosItem);
				}
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00006698 File Offset: 0x00004898
		public virtual void OnDocumentBeforeSave(CocosItem cocosItem)
		{
			foreach (IOperateModule operateModule in this.operateList)
			{
				IDocumentEventHandler documentEventHandler = operateModule as IDocumentEventHandler;
				if (documentEventHandler != null)
				{
					documentEventHandler.OnDocumentBeforeSave(cocosItem);
				}
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00006708 File Offset: 0x00004908
		public virtual void OnDocumentClosed(CocosItem cocosItem)
		{
			foreach (IOperateModule operateModule in this.operateList)
			{
				IDocumentEventHandler documentEventHandler = operateModule as IDocumentEventHandler;
				if (documentEventHandler != null)
				{
					documentEventHandler.OnDocumentClosed(cocosItem);
				}
			}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00006778 File Offset: 0x00004978
		public virtual void OnDragOver(DragMotionArgs args)
		{
			BaseViewMode.dragService.OnDragOver(args);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00006787 File Offset: 0x00004987
		public virtual void OnDragLeave(DragLeaveArgs args)
		{
			BaseViewMode.dragService.OnDragLeave(args);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00006796 File Offset: 0x00004996
		public virtual void OnDragDrop(DragDropArgs args)
		{
			BaseViewMode.dragService.OnDragDrop(args);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x000067A5 File Offset: 0x000049A5
		public virtual void DragDataReceived(DragDataReceivedArgs args)
		{
			BaseViewMode.dragService.DragDataReceived(args);
		}

		// Token: 0x04000025 RID: 37
		protected IOperateModule currentOperate;

		// Token: 0x04000026 RID: 38
		protected List<IOperateModule> operateList;

		// Token: 0x04000027 RID: 39
		protected ToolGroup toolGroup;

		// Token: 0x04000028 RID: 40
		private static DragService dragService;

		// Token: 0x04000029 RID: 41
		private static CommandServcie commandService;

		// Token: 0x0400002A RID: 42
		private static bool isInitialized;
	}
}
