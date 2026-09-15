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
	internal class TranslateTool : BoneObjectTool
	{
		public TranslateTool()
		{
			this._controlNodeDrawPen = new CSTranslateNodeDrawPen();
		}

		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Translate.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool_Translate + " (W)";
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.W;
			}
		}

		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			if (base.IsSelected && this.controlNode != null)
			{
				this.controlNode.Rotation = 0f;
			}
		}

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

		protected override void RefreshControlAxis()
		{
			if (SelectService.Instance.SelectedObjectList.Count == 0)
			{
				return;
			}
			BoneControlObject.Instance.UpdateControlAxisPos();
			BoneControlObject.Instance.ResetAxis(false, true);
		}

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

		private void UpdateAxisTranslate(PointF deltaPos)
		{
			CanvasObject canvasObject = BoneControlObject.Instance._canvasObject;
			PointF pointF = canvasObject.TransformToScene(BoneControlObject.Instance.Position);
			pointF.X += deltaPos.X;
			pointF.Y += deltaPos.Y;
			pointF = canvasObject.TransformToSelf(pointF);
			BoneControlObject.Instance.Position = pointF;
		}

		protected override void OnSelectObjectsChanged(IEnumerable<VisualObject> selectedObject, IEnumerable<VisualObject> selectedParentObject)
		{
			base.OnSelectObjectsChanged(selectedObject, selectedParentObject, OperationMask.MoveFlag);
		}
	}
}
