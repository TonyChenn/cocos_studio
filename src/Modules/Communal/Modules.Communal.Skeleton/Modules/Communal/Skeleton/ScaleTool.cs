using System;
using System.Collections.Generic;
using System.Linq;
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
	internal class ScaleTool : BoneObjectTool
	{
		public ScaleTool()
		{
			this._controlNodeDrawPen = new CSScaleNodeDrawPen();
			this.DefaultControlLine = this._controlNodeDrawPen.GetControlXLength();
		}

		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Scale.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool_Scale + " (R)";
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.R;
			}
		}

		protected override void ResetControl()
		{
			if (BoneControlObject.Instance.Visible)
			{
				this._controlNodeDrawPen.ClearDraw();
				(this._controlNodeDrawPen as CSScaleNodeDrawPen).ResetLine();
				this._controlNodeDrawPen.DrawControl();
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
			bool flag = false;
			if (args.Event.GetMouseButton() != MouseButton.Left)
			{
				operateState2 = instance.HitTestControlType(point);
			}
			else if (operateState2 != CSControlNodeDrawPen.OperateState.NONE)
			{
				instance._isOperating = true;
				float num = point.X - instance._lastMousePressedPos.X;
				float num2 = point.Y - instance._lastMousePressedPos.Y;
				PointF pointF = new PointF(num, num2);
				float rotation = instance.Rotation;
				if (rotation % 360f != 0f)
				{
					float num3 = -BoneObjectTool.CC_DEGREES_TO_RADIANS(rotation);
					float num4 = (float)Math.Cos((double)num3);
					float num5 = (float)Math.Sin((double)num3);
					pointF.X = num * num4 + num2 * num5;
					pointF.Y = num2 * num4 - num * num5;
				}
				pointF.X /= this.DefaultControlLine;
				pointF.Y /= this.DefaultControlLine;
				flag = true;
				this.SetAttachedScaleDelta(pointF, operateState2);
				instance._lastMousePressedPos = point;
			}
			if (operateState != operateState2)
			{
				this._controlNodeDrawPen.SetOperateState(operateState2);
				flag = true;
			}
			if (flag)
			{
				this._controlNodeDrawPen.ClearDraw();
				this._controlNodeDrawPen.DrawControl();
			}
		}

		private void SetAttachedScaleDelta(PointF deltap, CSControlNodeDrawPen.OperateState state)
		{
			IEnumerable<VisualObject> attachedObjects = BoneControlObject.Instance._attachedObjects;
			if (attachedObjects.Count<VisualObject>() == 1 && attachedObjects.First<VisualObject>().UniformScale)
			{
				deltap.Y = deltap.X;
			}
			else if (state == CSControlNodeDrawPen.OperateState.XLINE)
			{
				deltap.Y = 0f;
			}
			else if (state == CSControlNodeDrawPen.OperateState.YLINE)
			{
				deltap.X = 0f;
			}
			(this._controlNodeDrawPen as CSScaleNodeDrawPen).StretchLine(deltap);
			foreach (VisualObject visualObject in attachedObjects)
			{
				ScaleValue scale = visualObject.Scale;
				scale.ScaleX += deltap.X * scale.ScaleX;
				if (visualObject.UniformScale)
				{
					scale.ScaleY += deltap.X * scale.ScaleY;
				}
				else
				{
					scale.ScaleY += deltap.Y * scale.ScaleY;
				}
				if (Math.Abs(scale.ScaleX) < 0.01f || Math.Abs(scale.ScaleY) < 0.01f)
				{
					break;
				}
				visualObject.Scale = scale;
			}
		}

		protected override void OnSelectObjectsChanged(IEnumerable<VisualObject> selectedObject, IEnumerable<VisualObject> selectedParentObject)
		{
			base.OnSelectObjectsChanged(selectedObject, selectedParentObject, OperationMask.ScaleFlag);
		}

		private float DefaultControlLine;
	}
}
