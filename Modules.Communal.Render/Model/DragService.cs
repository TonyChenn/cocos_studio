using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.Lib.Prism;
using CocoStudio.Model;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.ViewModel.HitTest;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gdk;
using Gtk;
using Modules.Communal.Render.ExtensionModel;
using Mono.Addins;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000023 RID: 35
	internal class DragService : IDragEventHandler
	{
		// Token: 0x06000125 RID: 293 RVA: 0x00007AF4 File Offset: 0x00005CF4
		public DragService(IGLView glView, IEventAggregator eventAggregator)
		{
			if (eventAggregator != null)
			{
				this.glView = glView;
				this.eventAggregator = Services.EventsService;
				this.rootVisualObject = glView.GameWindow.GetCanvasObject();
				this.dragMenu = this.CreateDragMenu();
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00007B48 File Offset: 0x00005D48
		private IObjectDragMenu CreateDragMenu()
		{
			ExtensionNodeList extensionNodes = AddinManager.GetExtensionNodes(typeof(IObjectDragMenu));
			IObjectDragMenu result;
			if (extensionNodes == null || extensionNodes.Count != 1)
			{
				result = null;
			}
			else
			{
				foreach (TypeExtensionNode typeExtensionNode in extensionNodes)
				{
					return Activator.CreateInstance(typeExtensionNode.Type) as IObjectDragMenu;
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00007BEC File Offset: 0x00005DEC
		public void OnDragLeave(DragLeaveArgs e)
		{
			if (this.lastDragOverObject != null && this.lastDragMotionArgs != null)
			{
				this.lastDragOverObject.DragLeave(this.lastDragMotionArgs);
				this.lastDragMotionArgs = null;
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007C30 File Offset: 0x00005E30
		public void OnDragDrop(DragDropArgs e)
		{
			if (this.IsDropingFromOutside(e.Context))
			{
				this.lastDragDropArgs = e;
				Gtk.Drag.GetData(this.glView as Widget, e.Context, null, e.Time);
			}
			else
			{
				this.DispatchDragDropEvent(e);
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00007C84 File Offset: 0x00005E84
		public void OnDragOver(DragMotionArgs e)
		{
			this.CreateFileDropData(e);
			VisualObject currentDragOverObject = this.GetCurrentDragOverObject(e.X, e.Y);
			if (this.lastDragMotionArgs != e)
			{
				this.lastDragMotionArgs = e;
			}
			this.ChangeCurrentDragOverObject(currentDragOverObject, e);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00007CCC File Offset: 0x00005ECC
		public void DragDataReceived(DragDataReceivedArgs args)
		{
			if (this.IsDropingFromOutside(args.Context) && this.lastDragDropArgs != null)
			{
				FileDropInfo fileArray = args.SelectionData.GetFileArray();
				this.lastDragDropArgs.Context.SetDragData(fileArray);
				this.DispatchDragDropEvent(this.lastDragDropArgs);
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00007D28 File Offset: 0x00005F28
		private VisualObject GetCurrentDragOverObject(int x, int y)
		{
			HitTestResult hitVisual = HitTestService.Current.GetHitVisual(this.rootVisualObject, new PointF((float)x, (float)y));
			VisualObject hitVisual2 = this.rootVisualObject;
			if (hitVisual != null)
			{
				hitVisual2 = hitVisual.HitVisual;
			}
			return hitVisual2;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00007D70 File Offset: 0x00005F70
		private void CreateFileDropData(DragMotionArgs e)
		{
			if (e != this.lastDragMotionArgs)
			{
				if (this.IsDropingFromOutside(e.Context))
				{
					e.Context.SetDragData(new FileDropInfo(string.Empty));
				}
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00007DBC File Offset: 0x00005FBC
		private bool IsDropingFromOutside(DragContext e)
		{
			return e.DragProtocol == DragProtocol.Win32Dropfiles || e.GetSourceWidget() == null;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00007DFC File Offset: 0x00005FFC
		private void ChangeCurrentDragOverObject(VisualObject currentDragOverObject, DragMotionArgs e)
		{
			if (this.lastDragOverObject != null && this.lastDragOverObject.Equals(currentDragOverObject))
			{
				this.lastDragOverObject.DragOver(e);
			}
			else
			{
				if (this.lastDragOverObject != null)
				{
					this.lastDragOverObject.DragLeave(e);
				}
				this.lastDragOverObject = currentDragOverObject;
				if (this.lastDragOverObject != null)
				{
					this.lastDragOverObject.DragEnter(e);
				}
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00007E7C File Offset: 0x0000607C
		private void DispatchDragDropEvent(DragDropArgs e)
		{
			if (this.lastDragOverObject == null)
			{
				this.lastDragOverObject = this.GetCurrentDragOverObject(e.X, e.Y);
			}
			if (this.lastDragOverObject != null)
			{
				using (CompositeTask.Run("DragService.SendDragDropEvent", SelectService.Instance))
				{
					this.lastDragOverObject.DragDrop(e);
					this.lastDragOverObject = null;
					e.RetVal = true;
				}
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00007F1C File Offset: 0x0000611C
		private bool DropResourceToReplace(DragDropArgs e)
		{
			object dragData = e.Context.GetDragData();
			ResourceInfoDragData resourceInfoDragData = dragData as ResourceInfoDragData;
			if (resourceInfoDragData != null && resourceInfoDragData.Items.Count > 0 && !(this.lastDragOverObject is CanvasObject))
			{
				ResourceItem dragResourceItem = null;
				if (resourceInfoDragData.Items.Count == 1)
				{
					dragResourceItem = resourceInfoDragData.Items.FirstOrDefault<ResourceItem>();
				}
				DragMenuShowingArgs dragMenuShowingArgs = new DragMenuShowingArgs(this.lastDragOverObject, dragResourceItem);
				using (CompositeTask.Run("Drag drop", null))
				{
					if (dragMenuShowingArgs.DragResourceItem != null && this.dragMenu.CanShow(dragMenuShowingArgs))
					{
						Menu popupMenu = this.dragMenu.GetPopupMenu();
						if (popupMenu != null)
						{
							popupMenu.Popup();
							popupMenu.ShowAll();
						}
						List<VisualObject> list = new List<VisualObject>();
						list.Add(this.lastDragOverObject);
						SelectedVisualObjectsChangeEventArgs payload = new SelectedVisualObjectsChangeEventArgs(list, list, false);
						this.eventAggregator.GetEvent<SelectedVisualObjectsChangeEvent>().Publish(payload);
						this.lastDragOverObject = null;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04000049 RID: 73
		private IEventAggregator eventAggregator;

		// Token: 0x0400004A RID: 74
		private VisualObject rootVisualObject;

		// Token: 0x0400004B RID: 75
		private VisualObject lastDragOverObject;

		// Token: 0x0400004C RID: 76
		private IGLView glView;

		// Token: 0x0400004D RID: 77
		private DragMotionArgs lastDragMotionArgs;

		// Token: 0x0400004E RID: 78
		private DragDropArgs lastDragDropArgs;

		// Token: 0x0400004F RID: 79
		private IObjectDragMenu dragMenu;
	}
}
