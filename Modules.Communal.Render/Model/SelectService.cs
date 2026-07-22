using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.Lib.Prism;
using CocoStudio.Model;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.ViewModel.HitTest;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using CocoStudio.UndoManager.TaskModel;
using Gdk;
using Gtk;
using Modules.Communal.PropertyGrid;
using Modules.Communal.Render.View;
using MonoDevelop.Core;

namespace Modules.Communal.Render.Model
{
	// Token: 0x0200002D RID: 45
	public class SelectService : BaseObject, ITaskAction
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00009D6C File Offset: 0x00007F6C
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x00009D84 File Offset: 0x00007F84
		public VisualObject CurrentObject
		{
			get
			{
				return this.currentObject;
			}
			private set
			{
				this.currentObject = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00009D90 File Offset: 0x00007F90
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00009DB0 File Offset: 0x00007FB0
		[UndoProperty]
		public IReadOnlyList<VisualObject> SelectedObjectList
		{
			get
			{
				return this.selectedObjectList.ToList<VisualObject>();
			}
			private set
			{
				this.ClearSelectedObject();
				this.selectedObjectList.AddRange(value);
				foreach (VisualObject visualObject in this.selectedObjectList)
				{
					visualObject.IsSelected = true;
				}
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00009E20 File Offset: 0x00008020
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x00009E3D File Offset: 0x0000803D
		public IReadOnlyList<VisualObject> SelectedParentObjectList
		{
			get
			{
				return this.selectedParentObjectList.ToList<VisualObject>();
			}
			private set
			{
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00009E40 File Offset: 0x00008040
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x00009E57 File Offset: 0x00008057
		public IDrawRect RectNode { get; set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00009E60 File Offset: 0x00008060
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00009E78 File Offset: 0x00008078
		public bool IsActived
		{
			get
			{
				return this.isActived;
			}
			set
			{
				this.isActived = value;
				if (this.isActived)
				{
					Services.TaskService.Undone += this.TaskService_Undone;
					Services.TaskService.Redone += this.TaskService_Undone;
				}
				else
				{
					Services.TaskService.Undone -= this.TaskService_Undone;
					Services.TaskService.Redone -= this.TaskService_Undone;
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00009EFC File Offset: 0x000080FC
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00009F12 File Offset: 0x00008112
		public static SelectService Instance { get; private set; } = new SelectService();

		// Token: 0x060001BC RID: 444 RVA: 0x00009F28 File Offset: 0x00008128
		private SelectService()
		{
			this.eventAggregator = Services.EventsService;
			this.selectedObjectList = new List<VisualObject>();
			this.selectedParentObjectList = new List<VisualObject>();
			SelectService.Instance = this;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00009F84 File Offset: 0x00008184
		public void OnMouseDown(ButtonPressEventArgs e)
		{
			this.isDrawSelectRect = false;
			this.RefreshSelectedObject(e.Event);
			if (!this.isDrawSelectRect)
			{
				this.RaiseSelectedObjectsChanged();
			}
			else
			{
				e.RetVal = true;
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00009FCC File Offset: 0x000081CC
		public void OnMouseUp(ButtonReleaseEventArgs e)
		{
			HitTestService.Current.FilterCoveredChildren(this.rootObject, this.selectedParentObjectList, this.selectedObjectList);
			if (this.isDrawSelectRect)
			{
				this.EndRectSelectObject();
				this.RaiseSelectedObjectsChanged();
			}
			else if (!this.IsCtrlPressed())
			{
				this.RefreshSelectedObject(e.Event);
				this.RaiseSelectedObjectsChanged();
			}
			e.RetVal = true;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000A044 File Offset: 0x00008244
		public void OnMouseMove(MotionNotifyEventArgs e)
		{
			if (this.isDrawSelectRect)
			{
				if (e.Event.State.HasFlag(ModifierType.Button1Mask))
				{
					PointF point = e.Event.GetPoint();
					this.isRenderSelecting = true;
					this.DrawRectSelectObject(point);
					this.isRenderSelecting = false;
				}
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000A0AC File Offset: 0x000082AC
		public void OnKeyDown(KeyPressEventArgs e)
		{
			if (e.Event.Key == Gdk.Key.Escape && e.Event.State == ModifierType.None)
			{
				using (CompositeTask.Run("Select Service Esc key down.", null))
				{
					this.ClearSelectedObject();
					this.UpdateSelectedParentObject(null);
					this.RaiseSelectedObjectsChanged();
				}
			}
			else if (e.Event.Key == Gdk.Key.a || e.Event.Key == Gdk.Key.A)
			{
				ModifierType state = e.Event.State;
				if ((Platform.IsMac && state.HasFlag(ModifierType.Mod2Mask | ModifierType.MetaMask)) || state.HasFlag(ModifierType.ControlMask))
				{
					this.SelectAll();
					e.RetVal = true;
				}
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000A1BC File Offset: 0x000083BC
		private void RefreshSelectedObject(EventButton e)
		{
			this.clickPoint = e.GetPoint();
			this.lastHitTestResult = HitTestService.Current.GetHitVisual(this.rootObject, this.clickPoint);
			this.isRenderSelecting = true;
			if (this.lastHitTestResult == null && e.GetMouseButton() == MouseButton.Left)
			{
				this.StartRectSelectObject(this.lastHitTestResult);
			}
			this.ClickSelectObject(this.lastHitTestResult);
			HitTestService.Current.FilterCoveredChildren(this.rootObject, this.selectedParentObjectList, this.selectedObjectList);
			this.isRenderSelecting = false;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000A258 File Offset: 0x00008458
		private void ClearSelectedObject()
		{
			foreach (VisualObject visualObject in this.selectedObjectList)
			{
				if (visualObject != null)
				{
					visualObject.IsSelected = false;
				}
			}
			this.selectedObjectList.Clear();
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000A2C8 File Offset: 0x000084C8
		private void StartRectSelectObject(HitTestResult result)
		{
			if (result == null || result.HitVisual == null || result.HitVisual is CanvasObject)
			{
				this.ChangeRectArea(true);
				this.ResetRectAera();
				this.tempInRectSelectedObjectList = this.selectedObjectList.ToList<VisualObject>();
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000A31C File Offset: 0x0000851C
		private void EndRectSelectObject()
		{
			this.ChangeRectArea(false);
			this.tempInRectSelectedObjectList = null;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000A32E File Offset: 0x0000852E
		private void ChangeRectArea(bool isVisible)
		{
			this.isDrawSelectRect = isVisible;
			this.RectNode.Visible = isVisible;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000A348 File Offset: 0x00008548
		private void UpdataRectAera(PointF currentPoint)
		{
			this.rectAera.X = Math.Min(this.clickPoint.X, currentPoint.X);
			this.rectAera.Y = Math.Min(this.clickPoint.Y, currentPoint.Y);
			this.rectAera.Width = Math.Abs(currentPoint.X - this.clickPoint.X);
			this.rectAera.Height = Math.Abs(currentPoint.Y - this.clickPoint.Y);
			this.RectNode.Clear();
			this.RectNode.DrawRectangle(this.clickPoint, currentPoint);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000A400 File Offset: 0x00008600
		private void DrawRectSelectObject(PointF movePoint)
		{
			this.UpdataRectAera(movePoint);
			if ((Platform.IsMac && KeyboardExtend.IsModifyKeyPressed(ModifierType.Mod2Mask)) || (!Platform.IsMac && KeyboardExtend.IsModifyKeyPressed(ModifierType.ControlMask)))
			{
				this.RectSelectObjectWithCtrl();
			}
			else
			{
				this.RectSelectObject();
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000A458 File Offset: 0x00008658
		private void RectSelectObject()
		{
			this.ClearSelectedObject();
			this.selectedObjectList = HitTestService.Current.GetVisualInRect(this.rootObject, this.rectAera);
			foreach (VisualObject visualObject in this.selectedObjectList)
			{
				visualObject.IsSelected = true;
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000A4D8 File Offset: 0x000086D8
		private void RectSelectObjectWithCtrl()
		{
			this.ClearSelectedObject();
			List<VisualObject> visualInRect = HitTestService.Current.GetVisualInRect(this.rootObject, this.rectAera);
			foreach (VisualObject visualObject in this.tempInRectSelectedObjectList)
			{
				visualObject.IsSelected = true;
				this.selectedObjectList.Add(visualObject);
			}
			foreach (VisualObject visualObject in visualInRect)
			{
				if (this.tempInRectSelectedObjectList != null && this.tempInRectSelectedObjectList.Contains(visualObject))
				{
					visualObject.IsSelected = false;
					this.selectedObjectList.Remove(visualObject);
				}
				else
				{
					visualObject.IsSelected = true;
					this.selectedObjectList.Add(visualObject);
				}
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000A5EC File Offset: 0x000087EC
		private void ResetRectAera()
		{
			this.rectAera.X = 0f;
			this.rectAera.Y = 0f;
			this.rectAera.Width = 0f;
			this.rectAera.Height = 0f;
			this.RectNode.Clear();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000A64C File Offset: 0x0000884C
		private bool IsCtrlPressed()
		{
			ModifierType modifierKey = ModifierType.ControlMask;
			if (Platform.IsMac)
			{
				modifierKey = ModifierType.Mod2Mask;
			}
			return KeyboardExtend.IsModifyKeyPressed(modifierKey);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000A678 File Offset: 0x00008878
		private void ClickSelectObject(HitTestResult result)
		{
			if (this.IsCtrlPressed())
			{
				this.SelectObjectWithCtrl(result);
			}
			else
			{
				this.SelectObject(result);
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000A6A8 File Offset: 0x000088A8
		private void SelectObjectWithCtrl(HitTestResult result)
		{
			if (result == null || result.HitVisual == null)
			{
				this.currentObject = null;
				this.UpdateSelectedParentObject(null);
			}
			else
			{
				if (result.HitVisual.IsSelected)
				{
					this.selectedObjectList.Remove(result.HitVisual);
					this.selectedParentObjectList.Remove(result.HitVisual);
					result.HitVisual.IsSelected = false;
				}
				else
				{
					this.selectedObjectList.Add(result.HitVisual);
					result.HitVisual.IsSelected = true;
				}
				this.currentObject = result.HitVisual;
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000A758 File Offset: 0x00008958
		private void SelectObject(HitTestResult result)
		{
			if (result == null || result.HitVisual == null)
			{
				this.ChangeCurrentObject(null);
				this.ClearSelectedObject();
				this.UpdateSelectedParentObject(null);
			}
			else if (!this.selectedObjectList.Contains(result.HitVisual))
			{
				this.ClearSelectedObject();
				this.ChangeCurrentObject(result.HitVisual);
				this.selectedObjectList.Add(this.currentObject);
				this.UpdateSelectedParentObject(result.HitVisual);
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000A7E4 File Offset: 0x000089E4
		private void ChangeCurrentObject(VisualObject newObject)
		{
			if (this.currentObject != null)
			{
				this.currentObject.IsSelected = false;
			}
			this.currentObject = newObject;
			if (this.currentObject != null)
			{
				this.currentObject.IsSelected = true;
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000A834 File Offset: 0x00008A34
		private void UpdateSelectedParentObject(VisualObject selectedObject)
		{
			this.selectedParentObjectList.Clear();
			if (selectedObject != null)
			{
				this.selectedParentObjectList.Add(selectedObject);
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000A864 File Offset: 0x00008A64
		private void RaiseSelectedObjectsChanged()
		{
			if (this.eventAggregator != null)
			{
				SelectedVisualObjectsChangeEvent @event = this.eventAggregator.GetEvent<SelectedVisualObjectsChangeEvent>();
				@event.Unsubscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.OnSelectObjectsChangeEvent));
				@event.Publish(new SelectedVisualObjectsChangeEventArgs(this.selectedObjectList, this.selectedParentObjectList, false)
				{
					IsDoInNow = true
				});
				IPropertyGrid service = Services.GetService<IPropertyGrid>();
				service.SelectedObjects = this.ConvertToObjectList(this.selectedObjectList);
				@event.Subscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.OnSelectObjectsChangeEvent));
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000A8F4 File Offset: 0x00008AF4
		private void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args)
		{
			if (!this.isRenderSelecting)
			{
				this.selectedObjectList.Clear();
				this.selectedParentObjectList.Clear();
				this.selectedObjectList.AddRange(args.SelectedObject);
				this.selectedParentObjectList.AddRange(args.SelectedParentObject);
				this.currentObject = this.selectedObjectList.FirstOrDefault<VisualObject>();
				IPropertyGrid service = Services.GetService<IPropertyGrid>();
				service.SelectedObjects = this.ConvertToObjectList(this.selectedObjectList);
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000A974 File Offset: 0x00008B74
		private List<object> ConvertToObjectList(List<VisualObject> visualObjList)
		{
			List<object> list = new List<object>();
			foreach (VisualObject item in visualObjList)
			{
				list.Add(item);
			}
			return list;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000A9D8 File Offset: 0x00008BD8
		private void TaskService_Undone(object sender, TaskServiceEventArgs e)
		{
			if (this.IsActived)
			{
				HitTestService.Current.FilterCoveredChildren(this.rootObject, this.selectedParentObjectList, this.selectedObjectList);
				this.SelectedParentObjectList = this.selectedParentObjectList;
				this.RaiseSelectedObjectsChanged();
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000AA28 File Offset: 0x00008C28
		public void Initialize()
		{
			GameWindow gameWindow = GameWindow.Current;
			this.rootObject = gameWindow.GetCanvasObject();
			this.eventAggregator.GetEvent<SelectedVisualObjectsChangeEvent>().Subscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.OnSelectObjectsChangeEvent));
			base.BindingRecorder(null);
			if (base.Recorder.IsAutoRecord)
			{
				base.Recorder.Stop(false);
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000AA8C File Offset: 0x00008C8C
		public void SelectAll()
		{
			if (this.rootObject != null)
			{
				using (CompositeTask.Run("SelectService.SelectAll().", null))
				{
					CocosItem file = Services.Workbench.ActiveDocument.File;
					HitTestService.Current.SelectAllObjects(file.GetRootNode(), this.selectedObjectList, this.selectedParentObjectList);
					this.RaiseSelectedObjectsChanged();
				}
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000AB10 File Offset: 0x00008D10
		public void Clear()
		{
			using (CompositeTask.Run("SelectService.Clear().", null))
			{
				this.ClearSelectedObject();
				this.selectedParentObjectList.Clear();
				this.RaiseSelectedObjectsChanged();
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000AB6C File Offset: 0x00008D6C
		internal void OnDocumentChanged(GameCanvasContent canvasContent)
		{
			this.selectedObjectList.Clear();
			this.selectedParentObjectList.Clear();
			if (canvasContent.SelectedObject != null)
			{
				this.selectedObjectList.AddRange(canvasContent.SelectedObject);
			}
			if (canvasContent.SelectedParentObject != null)
			{
				this.selectedParentObjectList.AddRange(canvasContent.SelectedParentObject);
			}
			this.RaiseSelectedObjectsChanged();
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000ABD8 File Offset: 0x00008DD8
		public void BeginTask()
		{
			if (!base.Recorder.IsAutoRecord)
			{
				base.Recorder.Start(false, false);
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000AC08 File Offset: 0x00008E08
		public void EndTask()
		{
			this.RaisePropertyChanged<IReadOnlyList<VisualObject>>(() => this.SelectedObjectList);
			if (base.Recorder.IsAutoRecord)
			{
				base.Recorder.Stop(false);
			}
		}

		// Token: 0x0400006F RID: 111
		private IEventAggregator eventAggregator;

		// Token: 0x04000070 RID: 112
		private VisualObject rootObject;

		// Token: 0x04000071 RID: 113
		private VisualObject currentObject;

		// Token: 0x04000072 RID: 114
		private List<VisualObject> selectedObjectList;

		// Token: 0x04000073 RID: 115
		private List<VisualObject> selectedParentObjectList;

		// Token: 0x04000074 RID: 116
		private List<VisualObject> tempInRectSelectedObjectList;

		// Token: 0x04000075 RID: 117
		private bool isRenderSelecting;

		// Token: 0x04000076 RID: 118
		private PointF clickPoint = new PointF();

		// Token: 0x04000077 RID: 119
		private bool isDrawSelectRect = false;

		// Token: 0x04000078 RID: 120
		private RectF rectAera = RectF.Empty;

		// Token: 0x04000079 RID: 121
		private HitTestResult lastHitTestResult;

		// Token: 0x0400007A RID: 122
		private bool isActived;
	}
}
