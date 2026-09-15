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
	public class SelectService : BaseObject, ITaskAction
	{
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

		public IDrawRect RectNode { get; set; }

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

		public static SelectService Instance { get; private set; } = new SelectService();

		private SelectService()
		{
			this.eventAggregator = Services.EventsService;
			this.selectedObjectList = new List<VisualObject>();
			this.selectedParentObjectList = new List<VisualObject>();
			SelectService.Instance = this;
		}

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

		private void StartRectSelectObject(HitTestResult result)
		{
			if (result == null || result.HitVisual == null || result.HitVisual is CanvasObject)
			{
				this.ChangeRectArea(true);
				this.ResetRectAera();
				this.tempInRectSelectedObjectList = this.selectedObjectList.ToList<VisualObject>();
			}
		}

		private void EndRectSelectObject()
		{
			this.ChangeRectArea(false);
			this.tempInRectSelectedObjectList = null;
		}

		private void ChangeRectArea(bool isVisible)
		{
			this.isDrawSelectRect = isVisible;
			this.RectNode.Visible = isVisible;
		}

		private void UpdataRectAera(PointF currentPoint)
		{
			this.rectAera.X = Math.Min(this.clickPoint.X, currentPoint.X);
			this.rectAera.Y = Math.Min(this.clickPoint.Y, currentPoint.Y);
			this.rectAera.Width = Math.Abs(currentPoint.X - this.clickPoint.X);
			this.rectAera.Height = Math.Abs(currentPoint.Y - this.clickPoint.Y);
			this.RectNode.Clear();
			this.RectNode.DrawRectangle(this.clickPoint, currentPoint);
		}

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

		private void RectSelectObject()
		{
			this.ClearSelectedObject();
			this.selectedObjectList = HitTestService.Current.GetVisualInRect(this.rootObject, this.rectAera);
			foreach (VisualObject visualObject in this.selectedObjectList)
			{
				visualObject.IsSelected = true;
			}
		}

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

		private void ResetRectAera()
		{
			this.rectAera.X = 0f;
			this.rectAera.Y = 0f;
			this.rectAera.Width = 0f;
			this.rectAera.Height = 0f;
			this.RectNode.Clear();
		}

		private bool IsCtrlPressed()
		{
			ModifierType modifierKey = ModifierType.ControlMask;
			if (Platform.IsMac)
			{
				modifierKey = ModifierType.Mod2Mask;
			}
			return KeyboardExtend.IsModifyKeyPressed(modifierKey);
		}

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

		private void UpdateSelectedParentObject(VisualObject selectedObject)
		{
			this.selectedParentObjectList.Clear();
			if (selectedObject != null)
			{
				this.selectedParentObjectList.Add(selectedObject);
			}
		}

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

		private List<object> ConvertToObjectList(List<VisualObject> visualObjList)
		{
			List<object> list = new List<object>();
			foreach (VisualObject item in visualObjList)
			{
				list.Add(item);
			}
			return list;
		}

		private void TaskService_Undone(object sender, TaskServiceEventArgs e)
		{
			if (this.IsActived)
			{
				HitTestService.Current.FilterCoveredChildren(this.rootObject, this.selectedParentObjectList, this.selectedObjectList);
				this.SelectedParentObjectList = this.selectedParentObjectList;
				this.RaiseSelectedObjectsChanged();
			}
		}

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

		public void Clear()
		{
			using (CompositeTask.Run("SelectService.Clear().", null))
			{
				this.ClearSelectedObject();
				this.selectedParentObjectList.Clear();
				this.RaiseSelectedObjectsChanged();
			}
		}

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

		public void BeginTask()
		{
			if (!base.Recorder.IsAutoRecord)
			{
				base.Recorder.Start(false, false);
			}
		}

		public void EndTask()
		{
			this.RaisePropertyChanged<IReadOnlyList<VisualObject>>(() => this.SelectedObjectList);
			if (base.Recorder.IsAutoRecord)
			{
				base.Recorder.Stop(false);
			}
		}

		private IEventAggregator eventAggregator;

		private VisualObject rootObject;

		private VisualObject currentObject;

		private List<VisualObject> selectedObjectList;

		private List<VisualObject> selectedParentObjectList;

		private List<VisualObject> tempInRectSelectedObjectList;

		private bool isRenderSelecting;

		private PointF clickPoint = new PointF();

		private bool isDrawSelectRect = false;

		private RectF rectAera = RectF.Empty;

		private HitTestResult lastHitTestResult;

		private bool isActived;
	}
}
