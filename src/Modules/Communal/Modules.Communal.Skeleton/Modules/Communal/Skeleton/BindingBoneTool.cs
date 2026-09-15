using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.ViewModel.HitTest;
using CocoStudio.Model.Visiter;
using CocoStudio.UndoManager;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render.Model;
using Xwt.Drawing;

namespace Modules.Communal.Skeleton
{
	internal class BindingBoneTool : BaseTool, ISkeletonTool
	{
		public IEnumerable<VisualObject> _selectedObjects { get; private set; }

		private bool CanBinding
		{
			get
			{
				return this._isBinding && this._canBinding;
			}
			set
			{
				this._canBinding = value;
				if (this._canBinding)
				{
					BaseTool.SetCursor(Cursors.ArrowMove);
					return;
				}
				BaseTool.SetCursor(Cursors.Arrow);
			}
		}

		public BindingBoneTool()
		{
			this._boneRackPen = new CSBoneRackDrawPen();
			this._canvasObject = GameWindow.Current.GetCanvasObject();
			this._boneRackPen.SetDrawNodePen(BoneControlObject.Instance.GetCSVisual() as CSDrawNode);
		}

		public override bool HasSeparator
		{
			get
			{
				return true;
			}
		}

		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.Link.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.Skeleton_Binding + " (B)";
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.B;
			}
		}

		public override void OnKeyUp(KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Escape)
			{
				this.CancelBinding();
			}
			base.OnKeyUp(args);
		}

		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			if (base.IsSelected)
			{
				BoneControlObject.Instance.ResetAsOrigin();
				this.SetSelectedObjects(SelectService.Instance.SelectedObjectList, SelectService.Instance.SelectedParentObjectList);
				return;
			}
			this.SetSelectedObjects(null, null);
		}

		protected void SetSelectedObjects(IEnumerable<VisualObject> selectedObject, IEnumerable<VisualObject> parentSeletedObject)
		{
			if (this._attactedObjects != null && this._attactedObjects.Count<VisualObject>() > 0)
			{
				this._attactedObjects.Last<VisualObject>().PropertyChanged -= this.Selected_PropertyChanged;
			}
			this._attactedObjects = parentSeletedObject;
			this._selectedObjects = selectedObject;
			if (this._selectedObjects == null)
			{
				this._boneRackPen.ClearDraw();
				return;
			}
			int num = this._selectedObjects.Count<VisualObject>();
			if (num != 0)
			{
				this.ChangedParentBone(null);
				this.CancelBinding();
			}
			else
			{
				this._boneRackPen.ClearDraw();
			}
			if (this._attactedObjects != null && this._attactedObjects.Count<VisualObject>() > 0)
			{
				this._attactedObjects.Last<VisualObject>().PropertyChanged += this.Selected_PropertyChanged;
			}
			this.RefreshDraw();
		}

		private void Selected_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			string propertyName = e.PropertyName;
			if (this._attactedObjects == null || this._attactedObjects.Count<VisualObject>() == 0 || propertyName == "IsSelected")
			{
				return;
			}
			this.RefreshDraw();
		}

		protected virtual void RefreshDraw()
		{
			this._boneRackPen.ClearDraw();
			this.DrawConnectLines();
			if (this._isBinding && null != this._lastMousePos && this._isBinding)
			{
				foreach (VisualObject visualObject in this._selectedObjects)
				{
					AbstractNodeObject abstractNodeObject = visualObject as AbstractNodeObject;
					if (abstractNodeObject != null)
					{
						abstractNodeObject = abstractNodeObject.Parent;
					}
					if (abstractNodeObject != null)
					{
						PointF centerOfNodeToDraw = this.GetCenterOfNodeToDraw(visualObject);
						PointF end = BoneControlObject.Instance.TransformToSelf(this._lastMousePos);
						this._boneRackPen.DrawArrowLine(centerOfNodeToDraw, end, Color4F.YELLOW);
					}
				}
			}
		}

		public void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args)
		{
			if (this._boneRackPen == null)
			{
				return;
			}
			if (this.selectChangedFromEndBinding)
			{
				this.CancelBinding();
				return;
			}
			this.SetSelectedObjects(args.SelectedObject, args.SelectedParentObject);
		}

		public void OnRefreshControlDraw()
		{
			if (SelectService.Instance.SelectedObjectList.Count == 0)
			{
				return;
			}
			this._boneRackPen.ClearDraw();
			this.DrawConnectLines();
		}

		public void OnCanvasZoomedChangedEvent()
		{
			this.ReCalculatePoints();
		}

		private void ReCalculatePoints()
		{
			this.RefreshDraw();
		}

		public override void OnMouseDown(ButtonPressEventArgs args)
		{
			this.isMouseUpToOtherTool = false;
			args.RetVal = true;
			if (args.Event.GetMouseButton() == MouseButton.Left)
			{
				if (!this._isBinding)
				{
					PointF point = this.ConvertCoordinate(new PointF((float)args.Event.X, (float)args.Event.Y));
					HitTestResult hoverVisualBetweenSelected = HitTestMode.GetHoverVisualBetweenSelected(point, this._selectedObjects);
					if (hoverVisualBetweenSelected != null && hoverVisualBetweenSelected.HitVisual != null)
					{
						this.StartBinding();
						this._canBinding = false;
						return;
					}
					this.isMouseUpToOtherTool = true;
					args.RetVal = false;
					return;
				}
			}
			else if (args.Event.GetMouseButton() == MouseButton.Right)
			{
				if (this._isBinding)
				{
					this.CancelBinding();
					return;
				}
				this.isMouseUpToOtherTool = true;
				args.RetVal = false;
			}
		}

		public override void OnMouseMove(MotionNotifyEventArgs args)
		{
			if (!this._isBinding)
			{
				return;
			}
			bool canBinding = true;
			PointF point = new PointF((float)args.Event.X, (float)args.Event.Y);
			PointF lastMousePos = this.ConvertCoordinate(point);
			this._lastMousePos = lastMousePos;
			bool flag = false;
			SkeletonObject skeletonObject = Services.Workbench.ActiveDocument.File.GetRootNode() as SkeletonObject;
			HitTestResult hitVisualFirstSelected = HitTestService.Current.GetHitVisualFirstSelected(skeletonObject, point, this._selectedObjects);
			if (hitVisualFirstSelected != null && hitVisualFirstSelected.HitVisual != null)
			{
				if (hitVisualFirstSelected.HitVisual.IsSelected)
				{
					canBinding = false;
				}
				else
				{
					BoneObject boneObject = hitVisualFirstSelected.HitVisual as BoneObject;
					if (boneObject == null)
					{
						flag = this.ChangedParentBone(null);
						canBinding = false;
					}
					else if (this._selectedObjects.Count<VisualObject>() == 1 && boneObject.IsAncestor(this._selectedObjects.First<VisualObject>() as AbstractNodeObject))
					{
						flag = this.ChangedParentBone(null);
						canBinding = false;
					}
					else
					{
						flag = this.ChangedParentBone(boneObject);
					}
				}
			}
			else
			{
				flag = this.ChangedParentBone(skeletonObject);
			}
			if (flag && this._parentBone != null)
			{
				this._parentBone.ObjectBoudingState = CSVisualObject.ObjectState.DragOver;
				LogConfig.TipWithOutConsole.Info(this._parentBone.Name, false);
			}
			this.CanBinding = canBinding;
			this.RefreshDraw();
			args.RetVal = true;
		}

		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			base.OnMouseUp(args);
			if (this.CanBinding)
			{
				this.EndBinding();
			}
			args.RetVal = true;
			if (this.isMouseUpToOtherTool)
			{
				args.RetVal = false;
			}
		}

		private bool ChangedParentBone(AbstractNodeObject node)
		{
			BoneObject boneObject = node as BoneObject;
			if (this._parentBone != boneObject)
			{
				if (this._parentBone != null && !this._parentBone.IsSelected)
				{
					this._parentBone.ObjectBoudingState = CSVisualObject.ObjectState.Default;
				}
				this._parentBone = boneObject;
				return true;
			}
			return false;
		}

		private void CancelBinding()
		{
			this._isBinding = false;
			this.CanBinding = false;
			if (this._parentBone != null && this._parentBone.ObjectBoudingState == CSVisualObject.ObjectState.DragOver)
			{
				this._parentBone.ObjectBoudingState = CSVisualObject.ObjectState.Default;
			}
			this._boneRackPen.ClearDraw();
			this.DrawConnectLines();
		}

		private void StartBinding()
		{
			this._isBinding = true;
		}

		private void EndBinding()
		{
			using (CompositeTask.Run("binding nodes", null))
			{
				IReadOnlyList<VisualObject> selectedParentObjectList = SelectService.Instance.SelectedParentObjectList;
				foreach (VisualObject visualObject in this._selectedObjects)
				{
					AbstractNodeObject abstractNodeObject = visualObject as AbstractNodeObject;
					AbstractNodeObject parent = abstractNodeObject.Parent;
					if (abstractNodeObject != null && parent != null && parent != this._parentBone && !this._parentBone.IsAncestor(abstractNodeObject))
					{
						parent.Children.Remove(abstractNodeObject);
						this._parentBone.Children.Add(abstractNodeObject);
					}
				}
				this._parentBone.ObjectBoudingState = CSVisualObject.ObjectState.Default;
				this._isBinding = false;
				this.selectChangedFromEndBinding = true;
				Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>().Publish(new SelectedVisualObjectsChangeEventArgs(this._selectedObjects, selectedParentObjectList, false));
				this.selectChangedFromEndBinding = false;
			}
		}

		private void DrawConnectLines()
		{
			AbstractNodeObject rootNode = Services.Workbench.ActiveDocument.File.GetRootNode();
			foreach (VisualObject visualObject in this._selectedObjects)
			{
				AbstractNodeObject abstractNodeObject = visualObject as AbstractNodeObject;
				if (abstractNodeObject != null)
				{
					abstractNodeObject = abstractNodeObject.Parent;
				}
				if (visualObject.Visible && abstractNodeObject != null && abstractNodeObject != rootNode && visualObject != rootNode)
				{
					PointF centerOfNodeToDraw = this.GetCenterOfNodeToDraw(visualObject);
					PointF centerOfNodeToDraw2 = this.GetCenterOfNodeToDraw(abstractNodeObject);
					this._boneRackPen.DrawArrowLine(centerOfNodeToDraw, centerOfNodeToDraw2);
				}
			}
		}

		protected PointF GetCenterOfNodeToDraw(VisualObject node)
		{
			PointF pointF = new PointF();
			NodeObject nodeObject = node as NodeObject;
			if (nodeObject != null)
			{
				if (nodeObject.Size.Height - 0f > 0.001f && nodeObject.Size.Height - 0f > 0.001f)
				{
					SizeF boxSize = nodeObject.BoxSize;
					pointF.X = boxSize.Width / 2f;
					pointF.Y = boxSize.Height / 2f;
				}
			}
			else
			{
				BoneObject boneObject = node as BoneObject;
				if (boneObject == null)
				{
					return pointF;
				}
				SizeF boxSize2 = boneObject.BoxSize;
				pointF.X = boxSize2.Width / 2f;
				pointF.Y = 0f;
			}
			pointF = node.TransformToScene(pointF);
			return BoneControlObject.Instance.TransformToSelf(pointF);
		}

		private bool IsObjectBone(VisualObject node)
		{
			return null != node as BoneObject;
		}

		public static void BindingNodeToBone(AbstractNodeObject childNode, BoneObject boneToBeParent)
		{
			if (childNode == boneToBeParent || boneToBeParent.IsAncestor(childNode))
			{
				return;
			}
			IReadOnlyList<VisualObject> selectedObjectList = SelectService.Instance.SelectedObjectList;
			IReadOnlyList<VisualObject> selectedParentObjectList = SelectService.Instance.SelectedParentObjectList;
			using (CompositeTask.Run("BindingBoneTool.BindingBones", SelectService.Instance))
			{
				AbstractNodeObject parent = childNode.Parent;
				if (parent != null)
				{
					parent.Children.Remove(childNode);
				}
				boneToBeParent.Children.Add(childNode);
				SelectedVisualObjectsChangeEvent @event = Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>();
				@event.Publish(new SelectedVisualObjectsChangeEventArgs(selectedObjectList, selectedParentObjectList, false));
			}
		}

		private CSBoneRackDrawPen _boneRackPen;

		private CanvasObject _canvasObject;

		private bool _isBinding;

		private bool _canBinding;

		private BoneObject _parentBone;

		private PointF _lastMousePos;

		private IEnumerable<VisualObject> _attactedObjects;

		private bool isMouseUpToOtherTool;

		private bool selectChangedFromEndBinding;
	}
}
