using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.ViewModel.HitTest;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Skeleton
{
	// Token: 0x02000013 RID: 19
	public class BoneControlObject : VisualObject
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00005154 File Offset: 0x00003354
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x0000515C File Offset: 0x0000335C
		public CanvasObject _canvasObject { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00005165 File Offset: 0x00003365
		// (set) Token: 0x060000AB RID: 171 RVA: 0x0000516D File Offset: 0x0000336D
		public IEnumerable<VisualObject> _selectedObjects { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00005176 File Offset: 0x00003376
		// (set) Token: 0x060000AD RID: 173 RVA: 0x0000517E File Offset: 0x0000337E
		public IEnumerable<VisualObject> _attachedObjects { get; private set; }

		// Token: 0x060000AE RID: 174 RVA: 0x00005187 File Offset: 0x00003387
		internal override CSVisualObject GetCSVisual()
		{
			return this._axisDrawNode.GetCSVisual();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005194 File Offset: 0x00003394
		protected override HitTestResult HitTestCore(PointF point)
		{
			if (this._controlNodeDrawPen == null)
			{
				return null;
			}
			this._operateState = this.HitTestControlType(point);
			if (this._operateState != CSControlNodeDrawPen.OperateState.NONE)
			{
				return new HitTestResult(this, point, MouseOperationType.OPERATION_NONE, ControlPointType.POINT_NONE, false);
			}
			return null;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000051C4 File Offset: 0x000033C4
		protected BoneControlObject()
		{
			this._canvasObject = GameWindow.Current.GetCanvasObject();
			if (this._axisDrawNode == null)
			{
				this._axisDrawNode = new DrawNodeObject();
				this._axisDrawNode.ZOrder = 100000000;
				this._canvasObject.GetCSVisual().AddChild(this._axisDrawNode.GetCSVisual());
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00005225 File Offset: 0x00003425
		public static BoneControlObject Instance
		{
			get
			{
				if (BoneControlObject._instance == null)
				{
					BoneControlObject._instance = new BoneControlObject();
				}
				return BoneControlObject._instance;
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000523D File Offset: 0x0000343D
		public void InitControl(CSControlNodeDrawPen controlPen)
		{
			if (controlPen == null)
			{
				this.Visible = false;
				return;
			}
			this._controlNodeDrawPen = controlPen;
			this._controlNodeDrawPen.SetDrawNodePen(this._axisDrawNode.GetCSVisual() as CSDrawNode);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000526C File Offset: 0x0000346C
		public void SetSelectObjectsChanged(IEnumerable<VisualObject> selectedObjects, IEnumerable<VisualObject> selectedParentObjects)
		{
			if (this._attachedObjects != null && this._attachedObjects.Count<VisualObject>() > 0)
			{
				this._attachedObjects.LastOrDefault<VisualObject>().PropertyChanged -= this.Selected_PropertyChanged;
			}
			this._selectedObjects = selectedObjects;
			this._attachedObjects = selectedParentObjects;
			if (this._attachedObjects != null && this._attachedObjects.Count<VisualObject>() > 0)
			{
				this._attachedObjects.LastOrDefault<VisualObject>().PropertyChanged += this.Selected_PropertyChanged;
			}
			if (this._attachedObjects == null || this._selectedObjects == null)
			{
				this.Visible = false;
				return;
			}
			foreach (VisualObject visualObject in this._attachedObjects)
			{
				if (visualObject.OperationFlag == OperationMask.NoneFlag)
				{
					this.Visible = false;
					return;
				}
			}
			bool flag = this._attachedObjects != null && this._attachedObjects.Count<VisualObject>() != 0;
			this.Visible = flag;
			if (flag)
			{
				this.UpdateControlAxisPos();
				if (this._controlNodeDrawPen != null)
				{
					this._controlNodeDrawPen.ClearDraw();
					this.RedrawControl();
				}
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005394 File Offset: 0x00003594
		private void Selected_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this._isOperating)
			{
				return;
			}
			string propertyName = e.PropertyName;
			if (this._selectedObjects.Count<VisualObject>() == 0 || propertyName == "IsSelected")
			{
				return;
			}
			if (propertyName == "Position")
			{
				this.UpdateControlAxisPos();
				return;
			}
			if ((this._controlNodeDrawPen is CSRotationDrawPen || this._controlNodeDrawPen is CSScaleNodeDrawPen) && this._attachedObjects != null && this._attachedObjects.Count<VisualObject>() == 1)
			{
				this._controlNodeDrawPen.GetNodeRotateToPen(this._attachedObjects.First<VisualObject>().GetCSVisual());
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000542C File Offset: 0x0000362C
		public void UpdateControlAxisPos()
		{
			float num4;
			float num3;
			float num2;
			float num = num2 = (num3 = (num4 = 0f));
			bool flag = false;
			PointF pointF = new PointF();
			if (this._attachedObjects.Count<VisualObject>() == 1)
			{
				VisualObject visualObject = this._attachedObjects.First<VisualObject>();
				if (visualObject.OperationFlag == OperationMask.SizeFlag)
				{
					this._axisDrawNode.Visible = false;
				}
				PointF pointF2 = visualObject.TransformToScene(visualObject.GetCSVisual().GetAnchorPointInPoints());
				pointF.X = pointF2.X;
				pointF.Y = pointF2.Y;
			}
			else
			{
				foreach (VisualObject visualObject2 in this._attachedObjects)
				{
					SizeF sizeF = new SizeF();
					NodeObject nodeObject = visualObject2 as NodeObject;
					if (nodeObject == null)
					{
						BoneObject boneObject = visualObject2 as BoneObject;
						if (boneObject == null)
						{
							continue;
						}
						sizeF = boneObject.BoxSize;
					}
					else
					{
						sizeF = nodeObject.BoxSize;
					}
					if (sizeF.Height - 0f >= 0.001f && sizeF.Width - 0f >= 0.001f)
					{
						CSRect sizeRectToWorldTransfrom = this._controlNodeDrawPen.GetSizeRectToWorldTransfrom(visualObject2.GetCSVisual());
						float num5 = sizeRectToWorldTransfrom.MinX();
						float num6 = sizeRectToWorldTransfrom.MinY();
						float num7 = sizeRectToWorldTransfrom.MaxX();
						float num8 = sizeRectToWorldTransfrom.MaxY();
						if (!flag)
						{
							num2 = num5;
							num4 = num6;
							num = num7;
							num3 = num8;
							flag = true;
						}
						else
						{
							if (num2 > num5)
							{
								num2 = num5;
							}
							if (num4 > num6)
							{
								num4 = num6;
							}
							if (num < num7)
							{
								num = num7;
							}
							if (num3 < num8)
							{
								num3 = num8;
							}
						}
					}
				}
				pointF.X = num2 + (num - num2) / 2f;
				pointF.Y = num4 + (num3 - num4) / 2f;
			}
			this._axisDrawNode.Position = this.TransfSceneToCanvas(pointF);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005604 File Offset: 0x00003804
		public void RedrawControl()
		{
			this._controlNodeDrawPen.ClearDraw();
			this._controlNodeDrawPen.DrawControl();
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000561C File Offset: 0x0000381C
		protected override void OnMouseDown(MouseEventArgs args)
		{
			base.OnMouseDown(args);
			this._lastMousePressedPos = args.Point;
			if (this._operateState != this._controlNodeDrawPen.GetOperateState())
			{
				this._controlNodeDrawPen.SetOperateState(this._operateState);
				this.RedrawControl();
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000565B File Offset: 0x0000385B
		protected override void OnMouseMove(MouseEventArgs args)
		{
			base.OnMouseMove(args);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005664 File Offset: 0x00003864
		protected override void OnMouseUp(MouseEventArgs args)
		{
			base.OnMouseUp(args);
			this._controlNodeDrawPen.SetOperateState(CSControlNodeDrawPen.OperateState.NONE);
			this.RedrawControl();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000567F File Offset: 0x0000387F
		public PointF TransfSceneToCanvas(PointF scenePoint)
		{
			return this._canvasObject.TransformToSelf(scenePoint);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000568D File Offset: 0x0000388D
		public PointF TransfCanvasToSence(PointF canvasPoint)
		{
			return this._canvasObject.TransformToScene(canvasPoint);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000569B File Offset: 0x0000389B
		protected CSControlNodeDrawPen.OperateState PointOnControl(PointF point)
		{
			return this._controlNodeDrawPen.PointAtControl(point);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000056AC File Offset: 0x000038AC
		private bool IsPointAtSelectedObjects(PointF point)
		{
			if (this._selectedObjects != null)
			{
				HitTestResult hoverVisualBetweenSelected = HitTestMode.GetHoverVisualBetweenSelected(point, this._selectedObjects);
				if (hoverVisualBetweenSelected != null && hoverVisualBetweenSelected.HitVisual != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000056DC File Offset: 0x000038DC
		public CSControlNodeDrawPen.OperateState HitTestControlType(PointF point)
		{
			CSControlNodeDrawPen.OperateState operateState = this.PointOnControl(point);
			if (operateState == CSControlNodeDrawPen.OperateState.NONE && this.IsPointAtSelectedObjects(point))
			{
				operateState = CSControlNodeDrawPen.OperateState.XYLINE;
			}
			return operateState;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00005700 File Offset: 0x00003900
		public void ResetAxis(bool updateRotation, bool updatescale = true)
		{
			if (updateRotation && this._attachedObjects != null)
			{
				if (this._attachedObjects.Count<VisualObject>() == 1)
				{
					this._controlNodeDrawPen.GetNodeRotateToPen(this._attachedObjects.First<VisualObject>().GetCSVisual());
				}
				else
				{
					this._axisDrawNode.Rotation = 0f;
				}
			}
			if (updatescale)
			{
				this._controlNodeDrawPen.ResetDrawPenScale();
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00005761 File Offset: 0x00003961
		public void ResetAsOrigin()
		{
			this.Position = BoneControlObject.OriginAxisPosition;
			this.Visible = true;
			this.Rotation = 0f;
		}

		// Token: 0x04000037 RID: 55
		public const int OriginAxisXLength = 100;

		// Token: 0x04000038 RID: 56
		public const int OriginAxisYLength = 100;

		// Token: 0x04000039 RID: 57
		public const float OriginAxisRotation = 0f;

		// Token: 0x0400003A RID: 58
		public static readonly PointF OriginAxisPosition = new PointF(0f, 0f);

		// Token: 0x0400003B RID: 59
		public static readonly ScaleValue OriginAxisScale = new ScaleValue(1f, 1f, 0.1, -99999999.0, 99999999.0);

		// Token: 0x0400003C RID: 60
		private DrawNodeObject _axisDrawNode;

		// Token: 0x0400003D RID: 61
		private CSControlNodeDrawPen _controlNodeDrawPen;

		// Token: 0x0400003E RID: 62
		private CSControlNodeDrawPen.OperateState _operateState;

		// Token: 0x0400003F RID: 63
		public bool _isOperating;

		// Token: 0x04000040 RID: 64
		public PointF _lastMousePressedPos;

		// Token: 0x04000041 RID: 65
		private static BoneControlObject _instance = null;
	}
}
