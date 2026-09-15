using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Skeleton
{
	internal abstract class BoneObjectTool : BaseObjectTool, ISkeletonTool
	{
		protected BoneObjectTool()
		{
			this.controlNode = BoneControlObject.Instance;
		}

		protected virtual void RefreshControlAxis()
		{
			if (SelectService.Instance.SelectedObjectList.Count == 0)
			{
				return;
			}
			BoneControlObject.Instance.UpdateControlAxisPos();
			BoneControlObject.Instance.ResetAxis(true, true);
		}

		protected void InitControl()
		{
			BoneControlObject.Instance.InitControl(this._controlNodeDrawPen);
		}

		public override void Load()
		{
			base.Load();
		}

		public override void UnLoad()
		{
			base.UnLoad();
		}

		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			this.controlNode.Visible = base.IsSelected;
			if (this._controlNodeDrawPen == null)
			{
				return;
			}
			if (base.IsSelected)
			{
				this.InitControl();
				this.RefreshControlSelected();
				this.RefreshControlAxis();
				BoneControlObject.Instance.ResetAxis(true, true);
				return;
			}
			this._controlNodeDrawPen.ClearDraw();
			this.controlNode.Rotation = 0f;
			BoneControlObject.Instance.SetSelectObjectsChanged(null, null);
		}

		private void RefreshControlSelected()
		{
			this.OnSelectObjectsChanged(SelectService.Instance.SelectedObjectList, SelectService.Instance.SelectedParentObjectList);
		}

		public override void OnMouseMove(MotionNotifyEventArgs args)
		{
			if (this.controlNode == null || !this.controlNode.Visible || this._controlNodeDrawPen == null)
			{
				return;
			}
			base.OnMouseMove(args);
		}

		public override void OnMouseDown(ButtonPressEventArgs args)
		{
			base.OnMouseDown(args);
			if (args.Event.GetMouseButton() == MouseButton.Right)
			{
				args.RetVal = this.HitTest(this.clickPoint);
			}
		}

		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			if (args.Event.GetMouseButton() == MouseButton.Left)
			{
				this.ResetControl();
				BoneControlObject.Instance._isOperating = false;
			}
			base.OnMouseUp(args);
		}

		protected virtual void ResetControl()
		{
		}

		public static float GetPointToRotation(PointF point, PointF originPoint)
		{
			return BoneObjectTool.CC_RADIANS_TO_DEGREES((float)Math.Atan2((double)(point.Y - originPoint.Y), (double)(point.X - originPoint.X)));
		}

		public static float CC_RADIANS_TO_DEGREES(float v)
		{
			return v * 57.29578f;
		}

		public static float CC_DEGREES_TO_RADIANS(float v)
		{
			return v * 0.017453292f;
		}

		public void OnCanvasZoomedChangedEvent()
		{
		}

		public virtual void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args)
		{
			this.OnSelectObjectsChanged(args.SelectedObject, args.SelectedParentObject);
		}

		protected virtual void OnSelectObjectsChanged(IEnumerable<VisualObject> selectedObject, IEnumerable<VisualObject> selectedParentObject)
		{
		}

		protected void OnSelectObjectsChanged(IEnumerable<VisualObject> selectedObject, IEnumerable<VisualObject> selectedParentObject, OperationMask mask)
		{
			List<VisualObject> list = new List<VisualObject>();
			foreach (VisualObject visualObject in selectedParentObject)
			{
				if (visualObject.OperationFlag.HasFlag(mask))
				{
					list.Add(visualObject);
				}
			}
			BoneControlObject.Instance.SetSelectObjectsChanged(selectedObject, list);
			this.RefreshControlAxis();
		}

		public virtual void OnRefreshControlDraw()
		{
			if (!BoneControlObject.Instance._isOperating && !Services.TaskService.IsUndoing)
			{
				this.RefreshControlAxis();
			}
		}

		protected CSControlNodeDrawPen _controlNodeDrawPen;
	}
}
