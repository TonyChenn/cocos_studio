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
	// Token: 0x02000018 RID: 24
	internal class RotateTool : BoneObjectTool
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x00005B8C File Offset: 0x00003D8C
		public RotateTool()
		{
			this._controlNodeDrawPen = new CSRotationDrawPen();
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00005B9F File Offset: 0x00003D9F
		public override ToolType Type
		{
			get
			{
				return ToolType.Radio;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00005BA2 File Offset: 0x00003DA2
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Rotation.png");
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00005BAE File Offset: 0x00003DAE
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool_Rotate + " (E)";
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00005BBF File Offset: 0x00003DBF
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.E;
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005BC4 File Offset: 0x00003DC4
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

		// Token: 0x060000EA RID: 234 RVA: 0x00005CA4 File Offset: 0x00003EA4
		protected override void ResetControl()
		{
			BoneControlObject.Instance.ResetAxis(true, true);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005CB4 File Offset: 0x00003EB4
		private void SetAttachedRotateDelta(float deltap)
		{
			IEnumerable<VisualObject> attachedObjects = BoneControlObject.Instance._attachedObjects;
			foreach (VisualObject visualObject in attachedObjects)
			{
				visualObject.Rotation -= deltap;
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005D10 File Offset: 0x00003F10
		private void UpdateRotateAxis(float deltaRotation)
		{
			BoneControlObject.Instance.Rotation -= deltaRotation;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005D24 File Offset: 0x00003F24
		protected override void OnSelectObjectsChanged(IEnumerable<VisualObject> selectedObject, IEnumerable<VisualObject> selectedParentObject)
		{
			base.OnSelectObjectsChanged(selectedObject, selectedParentObject, OperationMask.RotationFlag);
		}
	}
}
