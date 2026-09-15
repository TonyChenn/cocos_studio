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
	public abstract class BaseViewMode : IViewMode, IActivateControl, IDocumentEventHandler, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IDragEventHandler
	{
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

		public abstract bool CanHandle(CocosItem cocosItem);

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

		public virtual IObjectContextMenu GetContextMenu()
		{
			return null;
		}

		public virtual bool CanShowContextMenu(PointF scenePoint)
		{
			return false;
		}

		public virtual void OnCanvasSizeChanged()
		{
		}

		public virtual void ResetView()
		{
		}

		public abstract Widget GetToolbar();

		public virtual ICommandService GetCommandService()
		{
			return BaseViewMode.commandService;
		}

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

		public virtual void OnMouseEnter(EnterNotifyEventArgs args)
		{
			this.currentOperate.OnMouseEnter(args);
		}

		public virtual void OnMouseLeave(LeaveNotifyEventArgs args)
		{
			this.currentOperate.OnMouseLeave(args);
		}

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

		public virtual void OnMouseDoubleClick(ButtonPressEventArgs args)
		{
			this.currentOperate.OnMouseDoubleClick(args);
		}

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

		public virtual void OnDragOver(DragMotionArgs args)
		{
			BaseViewMode.dragService.OnDragOver(args);
		}

		public virtual void OnDragLeave(DragLeaveArgs args)
		{
			BaseViewMode.dragService.OnDragLeave(args);
		}

		public virtual void OnDragDrop(DragDropArgs args)
		{
			BaseViewMode.dragService.OnDragDrop(args);
		}

		public virtual void DragDataReceived(DragDataReceivedArgs args)
		{
			BaseViewMode.dragService.DragDataReceived(args);
		}

		protected IOperateModule currentOperate;

		protected List<IOperateModule> operateList;

		protected ToolGroup toolGroup;

		private static DragService dragService;

		private static CommandServcie commandService;

		private static bool isInitialized;
	}
}
