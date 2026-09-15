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
	public class BoneControlObject : VisualObject
	{
		public CanvasObject _canvasObject { get; private set; }

		public IEnumerable<VisualObject> _selectedObjects { get; private set; }

		public IEnumerable<VisualObject> _attachedObjects { get; private set; }

		internal override CSVisualObject GetCSVisual()
		{
			return this._axisDrawNode.GetCSVisual();
		}

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

		public void RedrawControl()
		{
			this._controlNodeDrawPen.ClearDraw();
			this._controlNodeDrawPen.DrawControl();
		}

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

		protected override void OnMouseMove(MouseEventArgs args)
		{
			base.OnMouseMove(args);
		}

		protected override void OnMouseUp(MouseEventArgs args)
		{
			base.OnMouseUp(args);
			this._controlNodeDrawPen.SetOperateState(CSControlNodeDrawPen.OperateState.NONE);
			this.RedrawControl();
		}

		public PointF TransfSceneToCanvas(PointF scenePoint)
		{
			return this._canvasObject.TransformToSelf(scenePoint);
		}

		public PointF TransfCanvasToSence(PointF canvasPoint)
		{
			return this._canvasObject.TransformToScene(canvasPoint);
		}

		protected CSControlNodeDrawPen.OperateState PointOnControl(PointF point)
		{
			return this._controlNodeDrawPen.PointAtControl(point);
		}

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

		public CSControlNodeDrawPen.OperateState HitTestControlType(PointF point)
		{
			CSControlNodeDrawPen.OperateState operateState = this.PointOnControl(point);
			if (operateState == CSControlNodeDrawPen.OperateState.NONE && this.IsPointAtSelectedObjects(point))
			{
				operateState = CSControlNodeDrawPen.OperateState.XYLINE;
			}
			return operateState;
		}

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

		public void ResetAsOrigin()
		{
			this.Position = BoneControlObject.OriginAxisPosition;
			this.Visible = true;
			this.Rotation = 0f;
		}

		public const int OriginAxisXLength = 100;

		public const int OriginAxisYLength = 100;

		public const float OriginAxisRotation = 0f;

		public static readonly PointF OriginAxisPosition = new PointF(0f, 0f);

		public static readonly ScaleValue OriginAxisScale = new ScaleValue(1f, 1f, 0.1, -99999999.0, 99999999.0);

		private DrawNodeObject _axisDrawNode;

		private CSControlNodeDrawPen _controlNodeDrawPen;

		private CSControlNodeDrawPen.OperateState _operateState;

		public bool _isOperating;

		public PointF _lastMousePressedPos;

		private static BoneControlObject _instance = null;
	}
}
