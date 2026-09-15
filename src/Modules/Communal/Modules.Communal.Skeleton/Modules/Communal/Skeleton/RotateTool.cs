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
	internal class RotateTool : BoneObjectTool
	{
		public RotateTool()
		{
			this._controlNodeDrawPen = new CSRotationDrawPen();
		}

		public override ToolType Type
		{
			get
			{
				return ToolType.Radio;
			}
		}

		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Rotation.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool_Rotate + " (E)";
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.E;
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
				PointF originPoint = instance.TransfCanvasToSence(instance.Position);
				float pointToRotation = BoneObjectTool.GetPointToRotation(point, originPoint);
				float pointToRotation2 = BoneObjectTool.GetPointToRotation(instance._lastMousePressedPos, originPoint);
				float num = pointToRotation - pointToRotation2;
				if (num > 0f)
				{
					operateState2 = CSControlNodeDrawPen.OperateState.XLINE;
				}
				else
				{
					operateState2 = CSControlNodeDrawPen.OperateState.YLINE;
				}
				this.UpdateRotateAxis(num);
				this.SetAttachedRotateDelta(num);
				instance._lastMousePressedPos = point;
			}
			if (operateState != operateState2)
			{
				this._controlNodeDrawPen.SetOperateState(operateState2);
				this._controlNodeDrawPen.ClearDraw();
				this._controlNodeDrawPen.DrawControl();
			}
		}

		protected override void ResetControl()
		{
			BoneControlObject.Instance.ResetAxis(true, true);
		}

		private void SetAttachedRotateDelta(float deltap)
		{
			IEnumerable<VisualObject> attachedObjects = BoneControlObject.Instance._attachedObjects;
			foreach (VisualObject visualObject in attachedObjects)
			{
				visualObject.Rotation -= deltap;
			}
		}

		private void UpdateRotateAxis(float deltaRotation)
		{
			BoneControlObject.Instance.Rotation -= deltaRotation;
		}

		protected override void OnSelectObjectsChanged(IEnumerable<VisualObject> selectedObject, IEnumerable<VisualObject> selectedParentObject)
		{
			base.OnSelectObjectsChanged(selectedObject, selectedParentObject, OperationMask.RotationFlag);
		}
	}
}
