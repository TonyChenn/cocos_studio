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
	internal class DragService : IDragEventHandler
	{
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

		public void OnDragLeave(DragLeaveArgs e)
		{
			if (this.lastDragOverObject != null && this.lastDragMotionArgs != null)
			{
				this.lastDragOverObject.DragLeave(this.lastDragMotionArgs);
				this.lastDragMotionArgs = null;
			}
		}

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

		public void DragDataReceived(DragDataReceivedArgs args)
		{
			if (this.IsDropingFromOutside(args.Context) && this.lastDragDropArgs != null)
			{
				FileDropInfo fileArray = args.SelectionData.GetFileArray();
				this.lastDragDropArgs.Context.SetDragData(fileArray);
				this.DispatchDragDropEvent(this.lastDragDropArgs);
			}
		}

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

		private bool IsDropingFromOutside(DragContext e)
		{
			return e.DragProtocol == DragProtocol.Win32Dropfiles || e.GetSourceWidget() == null;
		}

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

		private IEventAggregator eventAggregator;

		private VisualObject rootVisualObject;

		private VisualObject lastDragOverObject;

		private IGLView glView;

		private DragMotionArgs lastDragMotionArgs;

		private DragDropArgs lastDragDropArgs;

		private IObjectDragMenu dragMenu;
	}
}
