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
	// Token: 0x02000019 RID: 25
	internal class ScaleTool : BoneObjectTool
	{
		// Token: 0x060000EE RID: 238 RVA: 0x00005D2F File Offset: 0x00003F2F
		public ScaleTool()
		{
			this._controlNodeDrawPen = new CSScaleNodeDrawPen();
			this.DefaultControlLine = this._controlNodeDrawPen.GetControlXLength();
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00005D53 File Offset: 0x00003F53
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Scale.png");
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00005D5F File Offset: 0x00003F5F
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool_Scale + " (R)";
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00005D70 File Offset: 0x00003F70
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.R;
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005D74 File Offset: 0x00003F74
		protected override void ResetControl()
		{
			if (BoneControlObject.Instance.Visible)
			{
				this._controlNodeDrawPen.ClearDraw();
				(this._controlNodeDrawPen as CSScaleNodeDrawPen).ResetLine();
				this._controlNodeDrawPen.DrawControl();
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005DA8 File Offset: 0x00003FA8
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

		// Token: 0x060000F4 RID: 244 RVA: 0x00005F10 File Offset: 0x00004110
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

		// Token: 0x060000F5 RID: 245 RVA: 0x00006044 File Offset: 0x00004244
		protected override void OnSelectObjectsChanged(IEnumerable<VisualObject> selectedObject, IEnumerable<VisualObject> selectedParentObject)
		{
			base.OnSelectObjectsChanged(selectedObject, selectedParentObject, OperationMask.ScaleFlag);
		}

		// Token: 0x04000048 RID: 72
		private float DefaultControlLine;
	}
}
