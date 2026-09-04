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
	// Token: 0x02000014 RID: 20
	internal abstract class BoneObjectTool : BaseObjectTool, ISkeletonTool
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x000057D6 File Offset: 0x000039D6
		protected BoneObjectTool()
		{
			this.controlNode = BoneControlObject.Instance;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000057EC File Offset: 0x000039EC
		protected virtual void RefreshControlAxis()
		{
			if (SelectService.Instance.SelectedObjectList.Count == 0)
			{
				return;
			}
			BoneControlObject.Instance.UpdateControlAxisPos();
			BoneControlObject.Instance.ResetAxis(true, true);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00005823 File Offset: 0x00003A23
		protected void InitControl()
		{
			BoneControlObject.Instance.InitControl(this._controlNodeDrawPen);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00005835 File Offset: 0x00003A35
		public override void Load()
		{
			base.Load();
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000583D File Offset: 0x00003A3D
		public override void UnLoad()
		{
			base.UnLoad();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00005848 File Offset: 0x00003A48
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

		// Token: 0x060000C8 RID: 200 RVA: 0x000058C3 File Offset: 0x00003AC3
		private void RefreshControlSelected()
		{
			this.OnSelectObjectsChanged(SelectService.Instance.SelectedObjectList, SelectService.Instance.SelectedParentObjectList);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000058DF File Offset: 0x00003ADF
		public override void OnMouseMove(MotionNotifyEventArgs args)
		{
			if (this.controlNode == null || !this.controlNode.Visible || this._controlNodeDrawPen == null)
			{
				return;
			}
			base.OnMouseMove(args);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005906 File Offset: 0x00003B06
		public override void OnMouseDown(ButtonPressEventArgs args)
		{
			base.OnMouseDown(args);
			if (args.Event.GetMouseButton() == MouseButton.Right)
			{
				args.RetVal = this.HitTest(this.clickPoint);
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00005934 File Offset: 0x00003B34
		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			if (args.Event.GetMouseButton() == MouseButton.Left)
			{
				this.ResetControl();
				BoneControlObject.Instance._isOperating = false;
			}
			base.OnMouseUp(args);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000595C File Offset: 0x00003B5C
		protected virtual void ResetControl()
		{
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000595E File Offset: 0x00003B5E
		public static float GetPointToRotation(PointF point, PointF originPoint)
		{
			return BoneObjectTool.CC_RADIANS_TO_DEGREES((float)Math.Atan2((double)(point.Y - originPoint.Y), (double)(point.X - originPoint.X)));
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00005987 File Offset: 0x00003B87
		public static float CC_RADIANS_TO_DEGREES(float v)
		{
			return v * 57.29578f;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00005990 File Offset: 0x00003B90
		public static float CC_DEGREES_TO_RADIANS(float v)
		{
			return v * 0.017453292f;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00005999 File Offset: 0x00003B99
		public void OnCanvasZoomedChangedEvent()
		{
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000599B File Offset: 0x00003B9B
		public virtual void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args)
		{
			this.OnSelectObjectsChanged(args.SelectedObject, args.SelectedParentObject);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000059AF File Offset: 0x00003BAF
		protected virtual void OnSelectObjectsChanged(IEnumerable<VisualObject> selectedObject, IEnumerable<VisualObject> selectedParentObject)
		{
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000059B4 File Offset: 0x00003BB4
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

		// Token: 0x060000D4 RID: 212 RVA: 0x00005A2C File Offset: 0x00003C2C
		public virtual void OnRefreshControlDraw()
		{
			if (!BoneControlObject.Instance._isOperating && !Services.TaskService.IsUndoing)
			{
				this.RefreshControlAxis();
			}
		}

		// Token: 0x04000045 RID: 69
		protected CSControlNodeDrawPen _controlNodeDrawPen;
	}
}
