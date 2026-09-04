using System;
using System.Collections.Generic;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render.Model;
using Xwt.Drawing;

namespace Modules.Communal.Skeleton
{
	// Token: 0x0200001C RID: 28
	internal class TranslateTool : BoneObjectTool
	{
		// Token: 0x0600010B RID: 267 RVA: 0x00006251 File Offset: 0x00004451
		public TranslateTool()
		{
			this._controlNodeDrawPen = new CSTranslateNodeDrawPen();
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00006264 File Offset: 0x00004464
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Translate.png");
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00006270 File Offset: 0x00004470
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool_Translate + " (W)";
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00006281 File Offset: 0x00004481
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.W;
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00006285 File Offset: 0x00004485
		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			if (base.IsSelected && this.controlNode != null)
			{
				this.controlNode.Rotation = 0f;
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000062B0 File Offset: 0x000044B0
		public override void OnMouseMove(MotionNotifyEventArgs args)
		{
			BoneControlObject instance = BoneControlObject.Instance;
			if (!instance.Visible)
			{
				return;
			}
			MouseEventArgs mouseEventArgs = EventArgsConvert.ToMouseEvent(args.Event, this);
			PointF point = mouseEventArgs.Point;
			CSControlNodeDrawPen.OperateState operateState = this._controlNodeDrawPen.GetOperateState();
			CSControlNodeDrawPen.OperateState operateState2 = operateState;
			if (args.Event.GetMouseButton() != MouseButton.Left)
			{
				operateState2 = instance.HitTestControlType(point);
			}
			else if (operateState != CSControlNodeDrawPen.OperateState.NONE)
			{
				instance._isOperating = true;
				PointF pointF = new PointF(point.X - instance._lastMousePressedPos.X, point.Y - instance._lastMousePressedPos.Y);
				if (operateState == CSControlNodeDrawPen.OperateState.XLINE)
				{
					pointF.Y = 0f;
				}
				else if (operateState == CSControlNodeDrawPen.OperateState.YLINE)
				{
					pointF.X = 0f;
				}
				this.UpdateAxisTranslate(pointF);
				this.SetAttachedPosDelta(pointF);
				instance._lastMousePressedPos = point;
			}
			if (operateState != operateState2)
			{
				this._controlNodeDrawPen.SetOperateState(operateState2);
				this._controlNodeDrawPen.ClearDraw();
				this._controlNodeDrawPen.DrawControl();
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000063A0 File Offset: 0x000045A0
		protected override void RefreshControlAxis()
		{
			if (SelectService.Instance.SelectedObjectList.Count == 0)
			{
				return;
			}
			BoneControlObject.Instance.UpdateControlAxisPos();
			BoneControlObject.Instance.ResetAxis(false, true);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000063D8 File Offset: 0x000045D8
		private void SetAttachedPosDelta(PointF deltap)
		{
			IEnumerable<VisualObject> attachedObjects = BoneControlObject.Instance._attachedObjects;
			if (attachedObjects == null)
			{
				return;
			}
			foreach (VisualObject visualObject in attachedObjects)
			{
				PointF anchorPointInPoints = visualObject.GetCSVisual().GetAnchorPointInPoints();
				PointF pointF = visualObject.TransformToScene(anchorPointInPoints);
				pointF.X += deltap.X;
				pointF.Y += deltap.Y;
				visualObject.Position = visualObject.TransformToParent(pointF);
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00006474 File Offset: 0x00004674
		private void UpdateAxisTranslate(PointF deltaPos)
		{
			CanvasObject canvasObject = BoneControlObject.Instance._canvasObject;
			PointF pointF = canvasObject.TransformToScene(BoneControlObject.Instance.Position);
			pointF.X += deltaPos.X;
			pointF.Y += deltaPos.Y;
			pointF = canvasObject.TransformToSelf(pointF);
			BoneControlObject.Instance.Position = pointF;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000064D6 File Offset: 0x000046D6
		protected override void OnSelectObjectsChanged(IEnumerable<VisualObject> selectedObject, IEnumerable<VisualObject> selectedParentObject)
		{
			base.OnSelectObjectsChanged(selectedObject, selectedParentObject, OperationMask.MoveFlag);
		}
	}
}
