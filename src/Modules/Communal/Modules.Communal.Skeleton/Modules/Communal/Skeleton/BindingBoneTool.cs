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
	// Token: 0x02000012 RID: 18
	internal class BindingBoneTool : BaseTool, ISkeletonTool
	{
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00004828 File Offset: 0x00002A28
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00004830 File Offset: 0x00002A30
		public IEnumerable<VisualObject> _selectedObjects { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00004839 File Offset: 0x00002A39
		// (set) Token: 0x0600008E RID: 142 RVA: 0x0000484B File Offset: 0x00002A4B
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

		// Token: 0x0600008F RID: 143 RVA: 0x00004871 File Offset: 0x00002A71
		public BindingBoneTool()
		{
			this._boneRackPen = new CSBoneRackDrawPen();
			this._canvasObject = GameWindow.Current.GetCanvasObject();
			this._boneRackPen.SetDrawNodePen(BoneControlObject.Instance.GetCSVisual() as CSDrawNode);
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000048AE File Offset: 0x00002AAE
		public override bool HasSeparator
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000048B1 File Offset: 0x00002AB1
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.Link.png");
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000092 RID: 146 RVA: 0x000048BD File Offset: 0x00002ABD
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.Skeleton_Binding + " (B)";
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000048CE File Offset: 0x00002ACE
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.B;
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000048D2 File Offset: 0x00002AD2
		public override void OnKeyUp(KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Escape)
			{
				this.CancelBinding();
			}
			base.OnKeyUp(args);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000048F3 File Offset: 0x00002AF3
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

		// Token: 0x06000096 RID: 150 RVA: 0x00004930 File Offset: 0x00002B30
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

		// Token: 0x06000097 RID: 151 RVA: 0x000049F4 File Offset: 0x00002BF4
		private void Selected_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			string propertyName = e.PropertyName;
			if (this._attactedObjects == null || this._attactedObjects.Count<VisualObject>() == 0 || propertyName == "IsSelected")
			{
				return;
			}
			this.RefreshDraw();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004A34 File Offset: 0x00002C34
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

		// Token: 0x06000099 RID: 153 RVA: 0x00004AF0 File Offset: 0x00002CF0
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

		// Token: 0x0600009A RID: 154 RVA: 0x00004B1C File Offset: 0x00002D1C
		public void OnRefreshControlDraw()
		{
			if (SelectService.Instance.SelectedObjectList.Count == 0)
			{
				return;
			}
			this._boneRackPen.ClearDraw();
			this.DrawConnectLines();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004B4E File Offset: 0x00002D4E
		public void OnCanvasZoomedChangedEvent()
		{
			this.ReCalculatePoints();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004B56 File Offset: 0x00002D56
		private void ReCalculatePoints()
		{
			this.RefreshDraw();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004B60 File Offset: 0x00002D60
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

		// Token: 0x0600009E RID: 158 RVA: 0x00004C28 File Offset: 0x00002E28
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

		// Token: 0x0600009F RID: 159 RVA: 0x00004D69 File Offset: 0x00002F69
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

		// Token: 0x060000A0 RID: 160 RVA: 0x00004DA0 File Offset: 0x00002FA0
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

		// Token: 0x060000A1 RID: 161 RVA: 0x00004DE8 File Offset: 0x00002FE8
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

		// Token: 0x060000A2 RID: 162 RVA: 0x00004E36 File Offset: 0x00003036
		private void StartBinding()
		{
			this._isBinding = true;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004E40 File Offset: 0x00003040
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

		// Token: 0x060000A4 RID: 164 RVA: 0x00004F44 File Offset: 0x00003144
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

		// Token: 0x060000A5 RID: 165 RVA: 0x00004FE8 File Offset: 0x000031E8
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

		// Token: 0x060000A6 RID: 166 RVA: 0x000050A9 File Offset: 0x000032A9
		private bool IsObjectBone(VisualObject node)
		{
			return null != node as BoneObject;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000050B8 File Offset: 0x000032B8
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

		// Token: 0x0400002D RID: 45
		private CSBoneRackDrawPen _boneRackPen;

		// Token: 0x0400002E RID: 46
		private CanvasObject _canvasObject;

		// Token: 0x0400002F RID: 47
		private bool _isBinding;

		// Token: 0x04000030 RID: 48
		private bool _canBinding;

		// Token: 0x04000031 RID: 49
		private BoneObject _parentBone;

		// Token: 0x04000032 RID: 50
		private PointF _lastMousePos;

		// Token: 0x04000033 RID: 51
		private IEnumerable<VisualObject> _attactedObjects;

		// Token: 0x04000034 RID: 52
		private bool isMouseUpToOtherTool;

		// Token: 0x04000035 RID: 53
		private bool selectChangedFromEndBinding;
	}
}
