using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.Events;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel.HitTest;
using CocoStudio.Model.Visiter;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render.Model;

namespace CocoStudio.Model.ViewModel
{
	public class ControlNode : VisualObject, IDisposable
	{
		private void InitContentSizeEvent()
		{
			GlobalCommand.ContentSizeCmd.Execute += this.ContentSizeCmd_Execute;
			GlobalCommand.ContentSizeCmd.Update += this.ContentSizeCmd_Update;
			GlobalCommand.ContentScaleCmd.Execute += this.ContentScaleCmd_Execute;
			GlobalCommand.ContentScaleCmd.Update += this.ContentScaleCmd_Update;
		}

		private void ContentSizeCmd_Update(object sender, CommandUpdateArgs e)
		{
			if (Services.Workbench.ActiveDocument.File != null && Services.Workbench.ActiveDocument.File.Is2DFile())
			{
				e.Info.Enabled = true;
				e.Info.Checked = Option.UserConfig.IsDragChangeSize;
			}
			else
			{
				e.Info.Enabled = false;
				e.Info.Checked = false;
			}
		}

		private void ContentSizeCmd_Execute(object sender, CommandRunArgs e)
		{
			this.SetIsDragChangeSize(true);
		}

		private void ContentScaleCmd_Update(object sender, CommandUpdateArgs e)
		{
			if (Services.Workbench.ActiveDocument.File != null && Services.Workbench.ActiveDocument.File.Is2DFile())
			{
				e.Info.Enabled = true;
				e.Info.Checked = !Option.UserConfig.IsDragChangeSize;
			}
			else
			{
				e.Info.Enabled = false;
				e.Info.Checked = false;
			}
		}

		private void ContentScaleCmd_Execute(object sender, CommandRunArgs e)
		{
			this.SetIsDragChangeSize(false);
		}

		private void SetIsDragChangeSize(bool isDragChangeSize)
		{
			bool isDragChangeSize2 = Option.UserConfig.IsDragChangeSize;
			if (isDragChangeSize2 != isDragChangeSize)
			{
				Option.UserConfig.IsDragChangeSize = isDragChangeSize;
				this.Init();
				Option.UserConfig.Save();
			}
			if (isDragChangeSize)
			{
				LogConfig.Output.Info(LanguageInfo.Output_DragChangeSize, true);
			}
			else
			{
				LogConfig.Output.Info(LanguageInfo.Output_DragChangeScale, true);
			}
		}

		public List<VisualObject> SelectedObjects
		{
			get
			{
				return this._selectedObjects;
			}
			private set
			{
				if (value == null)
				{
					this._selectedObjects.Clear();
				}
				else
				{
					this._selectedObjects = value;
				}
				this.RefreshOperation();
			}
		}

		public List<VisualObject> SelectedParentObjects
		{
			get
			{
				return this._selectedParentObjects;
			}
			private set
			{
				if (value == null)
				{
					this._selectedParentObjects.Clear();
				}
				else
				{
					this._selectedParentObjects = value;
				}
			}
		}

		public bool Enabled
		{
			get
			{
				return this.innerControlNode.IsEnable();
			}
			private set
			{
				this.innerControlNode.SetEnable(value);
			}
		}

		public ControlNode(CanvasObject canvasObject)
		{
			this.canvasObject = canvasObject;
			this.innerControlNode = new CSComControlNode();
			this.innerControlNode.SetCanvasObject(canvasObject.GetCSVisual());
			this.Visible = (this.VisibleForFrame = true);
			TimelineActionManager.Instance.CurrentFrameIndexChangedEvent += this.OnFrameIndexChanged;
			Services.EventsService.GetEvent<AlignedObjectsEvent>().Subscribe(new Action<AlignedObjectsArgs>(this.OnAlignedObjects));
			Services.EventsService.GetEvent<ScaleLockedChangeEvent>().Subscribe(new Action<bool>(this.OnScaleLockedChange));
			Services.EventsService.GetEvent<CanvasSizeChangeEvent>().Subscribe(new Action<CanvasSizeChangeEventArgs>(this.OnCanvasSizeChange));
			this.InitAnchorPointEvent();
			this.InitContentSizeEvent();
		}

		internal override CSVisualObject GetCSVisual()
		{
			return this.innerControlNode;
		}

		private void Init()
		{
			this.innerControlNode.SetAnchorPointVisible(Option.UserConfig.IsShowAnchorPoint);
			this.innerControlNode.SetControlPointVisible(true);
			this.InitProperty();
		}

		private void InitProperty()
		{
			if (this._selectedObjects.Count == 1)
			{
				NodeObject nodeObject = this._selectedParentObjects.FirstOrDefault<VisualObject>() as NodeObject;
				if (nodeObject == null || nodeObject.Parent == null)
				{
					this.Enabled = false;
				}
				else
				{
					this.Size = nodeObject.BoxSize;
					this.Position = nodeObject.Position;
					this.AnchorPoint = nodeObject.BoxAnchorPoint;
					this.RotationSkew = nodeObject.RotationSkew;
					IFlipped flipped = nodeObject as IFlipped;
					if (flipped != null && flipped.IsReverse)
					{
						float num = (float)(flipped.FlipX ? -1 : 1);
						float num2 = (float)(flipped.FlipY ? -1 : 1);
						this.Scale = new ScaleValue(nodeObject.Scale.ScaleX * num, nodeObject.Scale.ScaleY * num2, 0.1, -99999999.0, 99999999.0);
					}
					else
					{
						this.Scale = nodeObject.Scale;
					}
					if (!nodeObject.OperationFlag.HasFlag(OperationMask.ScaleFlag) && !nodeObject.OperationFlag.HasFlag(OperationMask.RotationFlag))
					{
						this.innerControlNode.SetControlPointVisible(false);
					}
					this.innerControlNode.SetAnchorPointVisible(nodeObject.OperationFlag.HasFlag(OperationMask.AnchorMoveFlag) && Option.UserConfig.IsShowAnchorPoint);
					this.verticalMove = true;
					this.horizontalMove = true;
					if (nodeObject.Parent.Size.Width == 0f)
					{
						if (nodeObject.PositionPercentXEnabled || nodeObject.HorizontalEdge == HorizontalBerthEdge.BothEdge)
						{
							this.horizontalMove = false;
						}
					}
					if (nodeObject.Parent.Size.Height == 0f)
					{
						if (nodeObject.PositionPercentYEnabled || nodeObject.VerticalEdge == VerticalBerthEdge.BothEdge)
						{
							this.verticalMove = false;
						}
					}
				}
			}
			else
			{
				float num4;
				float num3 = num4 = float.MaxValue;
				float num6;
				float num5 = num6 = float.MinValue;
				foreach (VisualObject visualObject in this.SelectedObjects)
				{
					NodeObject nodeObject2 = visualObject as NodeObject;
					if (nodeObject2 != null)
					{
						RectF boundingRect = this.GetBoundingRect(nodeObject2, nodeObject2.BoxSize);
						num4 = Math.Min(num4, boundingRect.Left);
						num3 = Math.Min(num3, boundingRect.Bottom);
						num5 = Math.Max(num5, boundingRect.Right);
						num6 = Math.Max(num6, boundingRect.Top);
					}
				}
				this.Scale = new ScaleValue(1f, 1f, 0.1, -99999999.0, 99999999.0);
				this.RotationSkew = new ScaleValue(0f, 0f, 0.1, -99999999.0, 99999999.0);
				this.AnchorPoint = new ScaleValue(0.5f, 0.5f, 0.1, -99999999.0, 99999999.0);
				this.Size = new SizeF(num5 - num4, num6 - num3);
				this.Position = new PointF(num4 + this.Size.Width * 0.5f, num3 + this.Size.Height * 0.5f);
			}
		}

		private RectF GetBoundingRect(NodeObject node, SizeF size)
		{
			RectF rect = new RectF(PointF.Empty, size);
			return this.innerControlNode.RectApplyTransform(rect, node.GetCSVisual().ConvertToNodeMatrix(this.canvasObject.GetCSVisual()));
		}

		private void RefreshOperation()
		{
			this.OperationFlag &= ~OperationMask.MoveFlag;
			foreach (VisualObject visualObject in this._selectedObjects)
			{
				if (visualObject.OperationFlag == OperationMask.NoneFlag)
				{
					OperationMask operationFlag = this.OperationFlag;
					this.OperationFlag = OperationMask.NoneFlag;
					break;
				}
				if (visualObject.OperationFlag.HasFlag(OperationMask.MoveFlag))
				{
					this.OperationFlag |= OperationMask.MoveFlag;
					break;
				}
			}
		}

		private void InitAnchorPointEvent()
		{
			GlobalCommand.AnchorPointCmd.Execute += this.AnchorPointCmd_Execute;
			GlobalCommand.AnchorPointCmd.Update += this.AnchorPointCmd_Update;
		}

		private void AnchorPointCmd_Update(object sender, CommandUpdateArgs e)
		{
			if (Services.Workbench.ActiveDocument.File != null && Services.Workbench.ActiveDocument.File.Is2DFile())
			{
				e.Info.Enabled = true;
			}
			else
			{
				e.Info.Enabled = false;
			}
			e.Info.Checked = Option.UserConfig.IsShowAnchorPoint;
		}

		private void AnchorPointCmd_Execute(object sender, CommandRunArgs e)
		{
			Option.UserConfig.IsShowAnchorPoint = !Option.UserConfig.IsShowAnchorPoint;
			this.Init();
			Option.UserConfig.Save();
			string message = string.Empty;
			if (Option.UserConfig.IsShowAnchorPoint)
			{
				message = LanguageInfo.Menu_View_ShowAnchorPoint;
			}
			else
			{
				message = LanguageInfo.Menu_View_HideAnchorPoint;
			}
			LogConfig.Output.Info(message, true);
		}

		private void OnAlignedObjects(AlignedObjectsArgs obj)
		{
			this.Init();
		}

		private void OnScaleLockedChange(bool locked)
		{
			this.isScaleLocked = locked;
		}

		private void OnCanvasSizeChange(CanvasSizeChangeEventArgs obj)
		{
			this.Init();
		}

		private void OnFrameIndexChanged()
		{
			if (!this.isMouseDown && !Services.TaskService.IsUndoing)
			{
				this.Init();
			}
		}

		public void SelectedObjectsChanged(IEnumerable<VisualObject> selectedObjectList, IEnumerable<VisualObject> selectedParentObjectList)
		{
			if (this.SelectedObjects.Count > 0)
			{
				this._selectedObjects.LastOrDefault<VisualObject>().PropertyChanged -= this.ComControlNode_PropertyChanged;
			}
			this.SelectedObjects = selectedObjectList.ToList<VisualObject>();
			this.SelectedParentObjects = selectedParentObjectList.ToList<VisualObject>();
			if (this.SelectedObjects.Count > 0)
			{
				this._selectedObjects.LastOrDefault<VisualObject>().PropertyChanged += this.ComControlNode_PropertyChanged;
			}
			if (this.SelectedObjects.Count <= 0 || this.SelectedParentObjects.Count <= 0)
			{
				this.Enabled = false;
			}
			else
			{
				this.Enabled = true;
			}
			this.Init();
			if (this.SelectedObjects.Count == 1)
			{
				this.innerControlNode.SetAttachNode(this.SelectedObjects.FirstOrDefault<VisualObject>().GetCSVisual() as CSNode);
			}
			else if (this.SelectedObjects.Count > 0)
			{
				this.innerControlNode.SetAttachNode(new CSNode(IntPtr.Zero, true));
			}
			this.isScaleLocked = false;
			this.mouseOperationType = MouseOperationType.OPERATION_POSITION;
		}

		private void ComControlNode_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (!this.isMouseMoved)
			{
				if (this._selectedObjects.Count != 0 && !(e.PropertyName == "IsSelected"))
				{
					if (e.PropertyName == "Parent" || e.PropertyName == "OperationFlag")
					{
						this.RefreshOperation();
					}
					this.Init();
				}
			}
		}

		protected override void OnMouseDown(MouseEventArgs args)
		{
			base.OnMouseDown(args);
			this.lastRotation = 0f;
			this.isMouseDown = true;
			this.isShiftDown = KeyboardExtend.IsModifyKeyPressed(ModifierType.ShiftMask);
			this.controlPointType = (ControlPointType)this.innerControlNode.GetControlPointType();
			this.lastMousePoint = args.Point;
			this.movingRect = this.GetBoundingBox();
			this.movingPosition = this.GetCanvasPosition();
			LayoutExtender.LayoutEnabled = false;
			foreach (VisualObject visualObject in this.SelectedObjects)
			{
				if (visualObject.Recorder.IsAutoRecord)
				{
					visualObject.Recorder.Stop(true);
				}
				else
				{
					LogConfig.Logger.Error("don't try to start a auto record object's record");
				}
			}
		}

		private RectF GetBoundingBox()
		{
			RectF result;
			if (this.SelectedObjects == null)
			{
				result = RectF.Empty;
			}
			else if (this.SelectedObjects.Count == 1)
			{
				NodeObject nodeObject = this.SelectedObjects[0] as NodeObject;
				if (nodeObject == null)
				{
					result = RectF.Empty;
				}
				else
				{
					result = this.GetBoundingRect(nodeObject, this.Size);
				}
			}
			else
			{
				result = this.innerControlNode.GetBoundingBox();
			}
			return result;
		}

		private PointF GetCanvasPosition()
		{
			PointF result;
			if (this.SelectedObjects.Count == 1)
			{
				NodeObject nodeObject = this.SelectedObjects[0] as NodeObject;
				if (nodeObject == null)
				{
					result = PointF.Empty;
				}
				else
				{
					ScaleValue anchorPoint = nodeObject.AnchorPoint;
					SizeF size = this.Size;
					PointF selfPoint = new PointF(this.Size.Width * anchorPoint.ScaleX, this.Size.Height * anchorPoint.ScaleY);
					PointF sencePoint = nodeObject.TransformToScene(selfPoint);
					result = this.canvasObject.TransformToSelf(sencePoint);
				}
			}
			else
			{
				result = this.Position;
			}
			return result;
		}

		protected override void OnMouseMove(MouseEventArgs args)
		{
			base.OnMouseMove(args);
			if (this.OperationFlag.HasFlag(OperationMask.MoveFlag) && !(base.lastClickPoint == null) && this.mouseOperationType != MouseOperationType.OPERATION_NONE && this._selectedObjects.Count != 0)
			{
				this.HandleMouseMove(args.Point);
			}
		}

		protected override void OnMouseUp(MouseEventArgs args)
		{
			base.OnMouseUp(args);
			if (TimelineActionManager.Instance.AutoKey)
			{
				TimelineActionManager.Instance.RefreshCurrentFrame();
			}
			this.isMouseDown = false;
			this.isMouseMoved = false;
			if (this.SelectedObjects != null)
			{
				LayoutExtender.LayoutEnabled = true;
				foreach (VisualObject visualObject in this.SelectedObjects)
				{
					if (!visualObject.Recorder.IsAutoRecord)
					{
						visualObject.Recorder.Start(true, false);
					}
					else
					{
						LogConfig.Logger.Error("don't try to start a auto record object's record");
					}
					LayoutExtender.RefreshLayout(visualObject);
				}
			}
		}

		protected PointF GetVectorByKey(Gdk.Key key)
		{
			PointF pointF = new PointF(0f, 0f);
			switch (key)
			{
			case Gdk.Key.Left:
				pointF.X = -1f;
				break;
			case Gdk.Key.Up:
				pointF.Y = 1f;
				break;
			case Gdk.Key.Right:
				pointF.X = 1f;
				break;
			case Gdk.Key.Down:
				pointF.Y = -1f;
				break;
			}
			PointF result;
			if (pointF.X == 0f && pointF.Y == 0f)
			{
				result = null;
			}
			else
			{
				result = pointF;
			}
			return result;
		}

		protected override void OnKeyDown(KeyPressEventArgs e)
		{
			if (!this.isMouseMoved && !this.isMouseDown && this.OperationFlag.HasFlag(OperationMask.MoveFlag))
			{
				if (this.SelectedObjects != null)
				{
					this.keyVector = this.GetVectorByKey(e.Event.Key);
					if (this.keyVector != null)
					{
						if (!Services.TaskService.IsRunningCompositeTask)
						{
							Services.TaskService.BeginCompositeTask("Com control node key down.");
						}
						if (KeyboardExtend.IsModifyKeyPressed(ModifierType.ShiftMask))
						{
							this.keyVector.X *= 10f;
							this.keyVector.Y *= 10f;
						}
						List<CSMatrix> list = new List<CSMatrix>();
						CSMatrix anchorWorldMatrix = this.innerControlNode.GetAnchorWorldMatrix();
						this.Print(anchorWorldMatrix);
						CSMatrix csmatrix = this.innerControlNode.Mat4Inverse(anchorWorldMatrix);
						this.Print(csmatrix);
						for (int i = 0; i < this._selectedParentObjects.Count<VisualObject>(); i++)
						{
							VisualObject visualObject = this._selectedParentObjects.ElementAt(i);
							CSMatrix anchorWorldMatrix2 = visualObject.GetCSVisual().GetAnchorWorldMatrix();
							this.Print(anchorWorldMatrix2);
							CSMatrix csmatrix2 = this.innerControlNode.Mat4Multiply(csmatrix, anchorWorldMatrix2);
							this.Print(csmatrix2);
							list.Add(csmatrix2);
						}
						PointF position = this.Position;
						this.Position = new PointF(position.X + this.keyVector.X, position.Y + this.keyVector.Y);
						CSMatrix anchorWorldMatrix3 = this.innerControlNode.GetAnchorWorldMatrix();
						this.Print(anchorWorldMatrix3);
						for (int i = 0; i < this._selectedParentObjects.Count<VisualObject>(); i++)
						{
							VisualObject visualObject = this._selectedParentObjects.ElementAt(i);
							VisualObject parent = (visualObject as AbstractNodeObject).Parent;
							if (parent == null)
							{
								parent = this.canvasObject;
							}
							CSMatrix csmatrix3 = visualObject.GetCSVisual().GetParentWorldMatrix();
							this.Print(csmatrix3);
							csmatrix3 = this.innerControlNode.Mat4Inverse(csmatrix3);
							this.Print(csmatrix3);
							CSMatrix csmatrix4 = this.innerControlNode.Mat4Multiply(anchorWorldMatrix3, list[i]);
							this.Print(csmatrix4);
							CSMatrix csmatrix2 = this.innerControlNode.Mat4Multiply(csmatrix3, csmatrix4);
							this.Print(csmatrix2);
							MatrixNode matrixNode = this.innerControlNode.Mat4ToMatrixNode(csmatrix2);
							if (!this.CheckFloatEqual(visualObject.Position.X, matrixNode.CX, 0.0001f) || !this.CheckFloatEqual(visualObject.Position.Y, matrixNode.CY, 0.0001f))
							{
								visualObject.Position = new PointF(matrixNode.CX, matrixNode.CY);
							}
						}
						e.RetVal = true;
					}
				}
			}
		}

		protected override void OnKeyUp(KeyReleaseEventArgs e)
		{
			if (!this.isMouseMoved && !this.isMouseDown)
			{
				PointF vectorByKey = this.GetVectorByKey(e.Event.Key);
				if (this.keyVector != null && vectorByKey != null && this.keyVector.Equals(vectorByKey))
				{
					if (Services.TaskService.IsRunningCompositeTask)
					{
						Services.TaskService.EndCompositeTask();
					}
				}
			}
		}

		private void HandleMouseMove(PointF point)
		{
			bool flag = true;
			this.isMouseMoved = true;
			float scaleX = this.Scale.ScaleX;
			float scaleY = this.Scale.ScaleY;
			List<CSMatrix> list = new List<CSMatrix>();
			CSMatrix anchorWorldMatrix = this.innerControlNode.GetAnchorWorldMatrix();
			this.Print(anchorWorldMatrix);
			CSMatrix csmatrix = this.innerControlNode.Mat4Inverse(anchorWorldMatrix);
			this.Print(csmatrix);
			for (int i = 0; i < this._selectedParentObjects.Count; i++)
			{
				VisualObject visualObject = this._selectedParentObjects[i];
				CSMatrix anchorWorldMatrix2 = visualObject.GetCSVisual().GetAnchorWorldMatrix();
				this.Print(anchorWorldMatrix2);
				CSMatrix csmatrix2 = this.innerControlNode.Mat4Multiply(csmatrix, anchorWorldMatrix2);
				this.Print(csmatrix2);
				list.Add(csmatrix2);
			}
			if (this.mouseOperationType == MouseOperationType.OPERATION_ANCHOR_POINT)
			{
				this.HandleAnchorPoint(point);
				if (this.SelectedObjects.Count > 1)
				{
					return;
				}
			}
			else if (this.mouseOperationType == MouseOperationType.OPERATION_POSITION)
			{
				this.HandlePosition(point);
			}
			else if (this.mouseOperationType == MouseOperationType.OPERATION_ROTATION)
			{
				this.HandleRotation(point);
			}
			else if (this.mouseOperationType == MouseOperationType.OPERATION_SCALE)
			{
				bool flag2 = this.HandleScale(point);
				flag = flag2;
				if (!flag2)
				{
					return;
				}
			}
			else if (this.mouseOperationType == MouseOperationType.OPERATION_SIZE)
			{
				bool flag2 = this.HandleSize(point);
				flag = flag2;
				if (!flag2)
				{
					return;
				}
			}
			else if (this.mouseOperationType == MouseOperationType.OPERATION_SKEW)
			{
				this.HandleSkew(point);
			}
			CSMatrix anchorWorldMatrix3 = this.innerControlNode.GetAnchorWorldMatrix();
			this.Print(anchorWorldMatrix3);
			for (int i = 0; i < this._selectedParentObjects.Count<VisualObject>(); i++)
			{
				VisualObject visualObject = this._selectedParentObjects.ElementAt(i);
				VisualObject parent = (visualObject as AbstractNodeObject).Parent;
				if (parent == null)
				{
					parent = this.canvasObject;
				}
				CSMatrix csmatrix3 = visualObject.GetCSVisual().GetParentWorldMatrix();
				this.Print(csmatrix3);
				csmatrix3 = this.innerControlNode.Mat4Inverse(csmatrix3);
				this.Print(csmatrix3);
				CSMatrix csmatrix4 = this.innerControlNode.Mat4Multiply(anchorWorldMatrix3, list[i]);
				this.Print(csmatrix4);
				CSMatrix csmatrix2 = this.innerControlNode.Mat4Multiply(csmatrix3, csmatrix4);
				this.Print(csmatrix2);
				MatrixNode matrixNode = this.innerControlNode.Mat4ToMatrixNode(csmatrix2);
				if (!this.CheckFloatEqual(visualObject.Position.X, matrixNode.CX, 0.0001f) || !this.CheckFloatEqual(visualObject.Position.Y, matrixNode.CY, 0.0001f))
				{
					visualObject.Position = new PointF(matrixNode.CX, matrixNode.CY);
				}
				float num = (float)((scaleX * this.Scale.ScaleX > 0f) ? 1 : -1);
				float num2 = (float)((scaleY * this.Scale.ScaleY > 0f) ? 1 : -1);
				float num3 = (float)((visualObject.Scale.ScaleX < 0f) ? -1 : 1);
				float num4 = (float)((visualObject.Scale.ScaleY < 0f) ? -1 : 1);
				if (num == -1f || num2 == -1f || !this.TestFloatEqual(visualObject.Scale.ScaleX, matrixNode.CScaleX * num3, 0.0001f) || !this.TestFloatEqual(visualObject.Scale.ScaleY, matrixNode.CScaleY * num4, 0.0001f))
				{
					visualObject.Scale = new ScaleValue(matrixNode.CScaleX * num3 * num, matrixNode.CScaleY * num4 * num2, 0.1, -99999999.0, 99999999.0);
					num3 = (float)((visualObject.Scale.ScaleX < 0f) ? -1 : 1);
					num4 = (float)((visualObject.Scale.ScaleY < 0f) ? -1 : 1);
				}
				IFlipped flipped = visualObject as IFlipped;
				bool flag3 = false;
				bool flag4 = false;
				if (flipped != null && flipped.IsReverse)
				{
					flag3 = flipped.FlipX;
					flag4 = flipped.FlipY;
				}
				float num5 = (float)((num3 == -1f == flag3) ? 0 : 1);
				float num6 = (float)((num4 == -1f == flag4) ? 0 : 1);
				float num7 = this.SimplifyRotation(ControlNode.CC_RADIANS_TO_DEGREES(-matrixNode.CSkewX - 3.1415927f * num6));
				float num8 = this.SimplifyRotation(ControlNode.CC_RADIANS_TO_DEGREES(-matrixNode.CSkewY - 3.1415927f * num5));
				ScaleValue rotationSkew = visualObject.RotationSkew;
				float num9 = this.SimplifyRotation(rotationSkew.ScaleX);
				float num10 = this.SimplifyRotation(rotationSkew.ScaleY);
				if (!this.CheckFloatEqual(rotationSkew.ScaleX, num7, 0.0001f) || !this.CheckFloatEqual(rotationSkew.ScaleY, num8, 0.0001f))
				{
					float num11 = this.SimplifyRotationDif(num7 - num9);
					float num12 = this.SimplifyRotationDif(num8 - num10);
					rotationSkew.ScaleX += num11;
					rotationSkew.ScaleY += num12;
					visualObject.RotationSkew = rotationSkew;
				}
			}
			if (flag)
			{
				this.lastMousePoint = point;
			}
		}

		private void HandleSkew(PointF point)
		{
			throw new NotImplementedException();
		}

		private void HandleAnchorPoint(PointF point)
		{
			if (this.horizontalMove && this.verticalMove)
			{
				PointF pointF = this.TransformToSelf(point);
				float x;
				float num = x = pointF.X;
				float y;
				float num2 = y = pointF.Y;
				float scaleX;
				float num3 = scaleX = pointF.X / this.Size.Width;
				float scaleY;
				float num4 = scaleY = pointF.Y / this.Size.Height;
				this.AdsorbAnchorPoint(ref num3, this.AnchorPoint.ScaleX, 0f, ref num, 0f);
				this.AdsorbAnchorPoint(ref num3, this.AnchorPoint.ScaleX, 0.5f, ref num, this.Size.Width / 2f);
				this.AdsorbAnchorPoint(ref num3, this.AnchorPoint.ScaleX, 1f, ref num, this.Size.Width);
				this.AdsorbAnchorPoint(ref num4, this.AnchorPoint.ScaleY, 0f, ref num2, 0f);
				this.AdsorbAnchorPoint(ref num4, this.AnchorPoint.ScaleY, 0.5f, ref num2, this.Size.Height / 2f);
				this.AdsorbAnchorPoint(ref num4, this.AnchorPoint.ScaleY, 1f, ref num2, this.Size.Height);
				bool flag = false;
				bool flag2 = false;
				if ((num3 == 0f || num3 == 1f) && num4 >= -0.1f && num4 <= 1.1f)
				{
					flag = true;
				}
				else if (num3 == 0.5f && ((num4 >= -0.1f && num4 <= 0.1f) || (num4 >= 0.9f && num4 <= 1.1f)))
				{
					flag = true;
				}
				if (flag)
				{
					scaleX = num3;
					x = num;
				}
				if ((num4 == 0f || num4 == 1f) && num3 >= -0.1f && num3 <= 1.1f)
				{
					flag2 = true;
				}
				else if (num4 == 0.5f && ((num3 >= -0.1f && num3 <= 0.1f) || (num3 >= 0.9f && num3 <= 1.1f)))
				{
					flag2 = true;
				}
				if (flag2)
				{
					scaleY = num4;
					y = num2;
				}
				ScaleValue anchorPoint = new ScaleValue(scaleX, scaleY, 0.1, -99999999.0, 99999999.0);
				pointF = new PointF(x, y);
				pointF = this.innerControlNode.TransformPoint(pointF, this.innerControlNode.GetMatrixWithoutReCalculate());
				this.AnchorPoint = anchorPoint;
				this.Position = pointF;
				if (this.SelectedObjects.Count == 1)
				{
					this.SelectedObjects.FirstOrDefault<VisualObject>().AnchorPoint = anchorPoint;
				}
			}
		}

		private bool HandleScale(PointF point)
		{
			float scaleX = this.Scale.ScaleX;
			float scaleY = this.Scale.ScaleY;
			PointF anchorPointInPoints = this.innerControlNode.GetAnchorPointInPoints();
			PointF p = this.TransformToSelf(ControlNode.PointSub(point, anchorPointInPoints));
			PointF p2 = this.TransformToSelf(ControlNode.PointSub(this.lastMousePoint, anchorPointInPoints));
			PointF pointF = ControlNode.PointSub(p, p2);
			float num = pointF.X * scaleX;
			float num2 = pointF.Y * scaleY;
			float num3 = -num / anchorPointInPoints.X;
			float num4 = num / (this.Size.Width - anchorPointInPoints.X);
			float num5 = -num2 / anchorPointInPoints.Y;
			float num6 = num2 / (this.Size.Height - anchorPointInPoints.Y);
			float num7 = 0f;
			float num8 = 0f;
			switch (this.controlPointType)
			{
			case ControlPointType.POINT_LEFT_TOP:
				if (this.AnchorPoint.ScaleX != 0f)
				{
					num7 = num3;
				}
				if (this.AnchorPoint.ScaleY != 1f)
				{
					num8 = num6;
				}
				break;
			case ControlPointType.POINT_LEFT_BOTTOM:
				if (this.AnchorPoint.ScaleX != 0f)
				{
					num7 = num3;
				}
				if (this.AnchorPoint.ScaleY != 0f)
				{
					num8 = num5;
				}
				break;
			case ControlPointType.POINT_RIGHT_BOTTOM:
				if (this.AnchorPoint.ScaleX != 1f)
				{
					num7 = num4;
				}
				if (this.AnchorPoint.ScaleY != 0f)
				{
					num8 = num5;
				}
				break;
			case ControlPointType.POINT_RIGHT_TOP:
				if (this.AnchorPoint.ScaleX != 1f)
				{
					num7 = num4;
				}
				if (this.AnchorPoint.ScaleY != 1f)
				{
					num8 = num6;
				}
				break;
			case ControlPointType.POINT_LEFT_MIDDLE:
				if (this.AnchorPoint.ScaleX != 0f)
				{
					num7 = num3;
				}
				break;
			case ControlPointType.POINT_MIDDLE_BOTTOM:
				if (this.AnchorPoint.ScaleY != 0f)
				{
					num8 = num5;
				}
				break;
			case ControlPointType.POINT_RIGHT_MIDDLE:
				if (this.AnchorPoint.ScaleX != 1f)
				{
					num7 = num4;
				}
				break;
			case ControlPointType.POINT_MIDDLE_TOP:
				if (this.AnchorPoint.ScaleY != 1f)
				{
					num8 = num6;
				}
				break;
			}
			bool result;
			if ((double)Math.Abs(this.Scale.ScaleX + num7) < 0.01)
			{
				result = false;
			}
			else if ((double)Math.Abs(this.Scale.ScaleY + num8) < 0.01)
			{
				result = false;
			}
			else
			{
				if (this.isShiftDown || this.isScaleLocked)
				{
					float num9 = this.Scale.ScaleY / this.Scale.ScaleX;
					float num10 = this.Scale.ScaleX + num7;
					this.Scale = new ScaleValue(num10, num10 * num9, 0.1, -99999999.0, 99999999.0);
				}
				else
				{
					this.Scale = new ScaleValue(this.Scale.ScaleX + num7, this.Scale.ScaleY + num8, 0.1, -99999999.0, 99999999.0);
				}
				result = true;
			}
			return result;
		}

		private bool HandleSize(PointF point)
		{
			PointF anchorPointInPoints = this.innerControlNode.GetAnchorPointInPoints();
			PointF p = this.TransformToSelf(ControlNode.PointSub(point, anchorPointInPoints));
			PointF p2 = this.TransformToSelf(ControlNode.PointSub(this.lastMousePoint, anchorPointInPoints));
			PointF pointF = ControlNode.PointSub(p, p2);
			float x = pointF.X;
			float y = pointF.Y;
			float num;
			if (anchorPointInPoints.X == 0f)
			{
				num = -x;
			}
			else
			{
				num = -x / anchorPointInPoints.X;
			}
			float num2;
			if (this.Size.Width == anchorPointInPoints.X)
			{
				num2 = x;
			}
			else
			{
				num2 = x / (this.Size.Width - anchorPointInPoints.X);
			}
			float num3;
			if (anchorPointInPoints.Y == 0f)
			{
				num3 = -y;
			}
			else
			{
				num3 = -y / anchorPointInPoints.Y;
			}
			float num4;
			if (this.Size.Height == anchorPointInPoints.Y)
			{
				num4 = y;
			}
			else
			{
				num4 = y / (this.Size.Height - anchorPointInPoints.Y);
			}
			float num5 = 0f;
			float num6 = 0f;
			switch (this.controlPointType)
			{
			case ControlPointType.POINT_LEFT_TOP:
				if (this.AnchorPoint.ScaleX != 0f)
				{
					num5 = num;
				}
				if (this.AnchorPoint.ScaleY != 1f)
				{
					num6 = num4;
				}
				break;
			case ControlPointType.POINT_LEFT_BOTTOM:
				if (this.AnchorPoint.ScaleX != 0f)
				{
					num5 = num;
				}
				if (this.AnchorPoint.ScaleY != 0f)
				{
					num6 = num3;
				}
				break;
			case ControlPointType.POINT_RIGHT_BOTTOM:
				if (this.AnchorPoint.ScaleX != 1f)
				{
					num5 = num2;
				}
				if (this.AnchorPoint.ScaleY != 0f)
				{
					num6 = num3;
				}
				break;
			case ControlPointType.POINT_RIGHT_TOP:
				if (this.AnchorPoint.ScaleX != 1f)
				{
					num5 = num2;
				}
				if (this.AnchorPoint.ScaleY != 1f)
				{
					num6 = num4;
				}
				break;
			case ControlPointType.POINT_LEFT_MIDDLE:
				if (this.AnchorPoint.ScaleX != 0f)
				{
					num5 = num;
				}
				break;
			case ControlPointType.POINT_MIDDLE_BOTTOM:
				if (this.AnchorPoint.ScaleY != 0f)
				{
					num6 = num3;
				}
				break;
			case ControlPointType.POINT_RIGHT_MIDDLE:
				if (this.AnchorPoint.ScaleX != 1f)
				{
					num5 = num2;
				}
				break;
			case ControlPointType.POINT_MIDDLE_TOP:
				if (this.AnchorPoint.ScaleY != 1f)
				{
					num6 = num4;
				}
				break;
			}
			bool result;
			if ((double)Math.Abs(this.Size.Width + this.Size.Width * num5) < 0.01)
			{
				result = false;
			}
			else if ((double)Math.Abs(this.Size.Height + this.Size.Width * num6) < 0.01)
			{
				result = false;
			}
			else
			{
				float num7 = 0f;
				float num8 = 0f;
				float num9 = 1f;
				float num10 = 1f;
				if (this.isShiftDown || this.isScaleLocked)
				{
					if (this.Size.Width != 0f)
					{
						float num11 = this.Size.Height / this.Size.Width;
						num7 = this.Size.Width + this.Size.Width * num5;
						num8 = num7 * num11;
					}
				}
				else
				{
					num7 = ((this.Size.Width != 0f) ? (this.Size.Width * (1f + num5)) : num5);
					num8 = ((this.Size.Height != 0f) ? (this.Size.Height * (1f + num6)) : num6);
				}
				if (num7 < 0f)
				{
					num9 = -1f;
				}
				if (num8 < 0f)
				{
					num10 = -1f;
				}
				this.Scale = new ScaleValue(this.Scale.ScaleX * num9, this.Scale.ScaleY * num10, 0.1, -99999999.0, 99999999.0);
				this.Size = new SizeF(Math.Abs(num7), Math.Abs(num8));
				NodeObject nodeObject = this._selectedParentObjects.FirstOrDefault<VisualObject>() as NodeObject;
				if (nodeObject == null)
				{
					result = false;
				}
				else
				{
					nodeObject.Size = new SizeF(this.Size.Width, this.Size.Height);
					result = true;
				}
			}
			return result;
		}

		private void HandleRotation(PointF point)
		{
			PointF position = this.Position;
			PointF pointF;
			PointF pointF2;
			if (this._selectedParentObjects.Count<VisualObject>() == 1)
			{
				pointF = this._selectedParentObjects.FirstOrDefault<VisualObject>().TransformToParent(this.lastMousePoint);
				pointF2 = this._selectedParentObjects.FirstOrDefault<VisualObject>().TransformToParent(point);
			}
			else
			{
				pointF = this.canvasObject.TransformToSelf(this.lastMousePoint);
				pointF2 = this.canvasObject.TransformToSelf(point);
			}
			float num = ControlNode.CC_DEGREES_TO_RADIANS(this.RotationSkew.ScaleX);
			float num2 = ControlNode.CC_DEGREES_TO_RADIANS(this.RotationSkew.ScaleY);
			float num3 = (float)(-(float)Math.Atan2((double)(pointF.Y - position.Y), (double)(pointF.X - position.X)));
			float num4 = (float)(-(float)Math.Atan2((double)(pointF2.Y - position.Y), (double)(pointF2.X - position.X)));
			if (this.isShiftDown)
			{
				num4 = ((num4 * num3 < 0f) ? (-num4) : num4);
				float num5 = ControlNode.CC_RADIANS_TO_DEGREES(num4 - num3);
				this.lastRotation += num5;
				if (Math.Abs(this.lastRotation) >= 15f)
				{
					this.RotationSkew = new ScaleValue();
					this.lastRotation = 0f;
				}
			}
			else
			{
				num2 = ControlNode.CC_RADIANS_TO_DEGREES(num2 + num4 - num3);
				num = ControlNode.CC_RADIANS_TO_DEGREES(num + num4 - num3);
				this.RotationSkew = new ScaleValue(num, num2, 0.1, -99999999.0, 99999999.0);
			}
		}

		private void HandlePosition(PointF point)
		{
			if (this.horizontalMove || this.verticalMove)
			{
				if (!TimelineActionManager.Instance.OnionSkinEnable || TimelineActionManager.Instance.AutoKey)
				{
					NodeObject nodeObject = this._selectedParentObjects.FirstOrDefault<VisualObject>() as NodeObject;
					if (nodeObject != null && !(this.movingPosition == null))
					{
						PointF pointF = this.canvasObject.TransformToSelf(this.lastMousePoint);
						PointF pointF2 = this.canvasObject.TransformToSelf(point);
						float num = pointF2.X - pointF.X;
						float num2 = pointF2.Y - pointF.Y;
						if (KeyboardExtend.IsModifyKeyPressed(ModifierType.ShiftMask))
						{
							if (Math.Abs(num) > Math.Abs(num2))
							{
								num2 = 0f;
							}
							else
							{
								num = 0f;
							}
						}
						this.movingPosition.X = this.movingPosition.X + num;
						this.movingPosition.Y = this.movingPosition.Y + num2;
						PointF pointF3 = this.movingPosition.Clone() as PointF;
						if (GuidesService.Instance.Visible)
						{
							this.movingRect.Offset(num, num2);
							List<DockGuidesResult> dockGuides = GuidesService.Instance.GetDockGuides(this.movingRect);
							foreach (DockGuidesResult dockGuidesResult in dockGuides)
							{
								switch (dockGuidesResult.Guides.Direction)
								{
								case Orientation.Horizontal:
									pointF3.Y += dockGuidesResult.Offset;
									break;
								case Orientation.Vertical:
									pointF3.X += dockGuidesResult.Offset;
									break;
								}
							}
							if (this.SelectedObjects.Count == 1)
							{
								PointF sencePoint = this.canvasObject.TransformToScene(pointF3);
								pointF3 = nodeObject.TransformToParent(sencePoint);
							}
						}
						if (!this.horizontalMove)
						{
							pointF3.X = this.Position.X;
						}
						if (!this.verticalMove)
						{
							pointF3.Y = this.Position.Y;
						}
						this.Position = pointF3;
					}
				}
			}
		}

		private bool TestFloatEqual(float v1, float v2, float threshold = 0.0001f)
		{
			return Math.Abs(v1 - v2) <= threshold;
		}

		private void AdsorbAnchorPoint(ref float newAnchorValue, float oldAnchorValue, float dstAnchorValue, ref float positionValue, float dstPositionValue)
		{
			if (this.CheckFloatEqual(newAnchorValue, dstAnchorValue, 0.1f))
			{
				if (Math.Abs(dstAnchorValue - newAnchorValue) < Math.Abs(dstAnchorValue - oldAnchorValue))
				{
					newAnchorValue = dstAnchorValue;
				}
				else
				{
					newAnchorValue = oldAnchorValue;
				}
				positionValue = dstPositionValue;
			}
		}

		private bool CheckFloatEqual(float v1, float v2, float threshold = 0.0001f)
		{
			return Math.Abs(v1 - v2) <= threshold;
		}

		private float SimplifyRotation(float r)
		{
			r %= 360f;
			if (r < 0f)
			{
				r += 360f;
			}
			if (r > 180f)
			{
				r -= 360f;
			}
			return r;
		}

		private float SimplifyRotationDif(float r)
		{
			if (r < -180f)
			{
				r += 360f;
			}
			if (r > 180f)
			{
				r -= 360f;
			}
			return r;
		}

		private void Print(CSMatrix m)
		{
		}

		private static float CC_RADIANS_TO_DEGREES(float v)
		{
			return v * 57.29578f;
		}

		private static float CC_DEGREES_TO_RADIANS(float v)
		{
			return v * 0.017453292f;
		}

		private static PointF PointSub(PointF p1, PointF p2)
		{
			return new PointF(p1.X - p2.X, p1.Y - p2.Y);
		}

		protected override HitTestResult HitTestCore(PointF point)
		{
			HitTestResult result;
			if (!this.Enabled)
			{
				result = null;
			}
			else
			{
				if (!this.isMouseDown)
				{
					this.mouseOperationType = (MouseOperationType)this.innerControlNode.HitTest(point);
					if (Option.UserConfig.IsDragChangeSize && this.mouseOperationType == MouseOperationType.OPERATION_SCALE)
					{
						if (this._selectedObjects != null && this._selectedObjects.Count == 1)
						{
							NodeObject nodeObject = this._selectedParentObjects.FirstOrDefault<VisualObject>() as NodeObject;
							if (nodeObject == null)
							{
								return null;
							}
							if (nodeObject.IsCanChangeSize())
							{
								this.mouseOperationType = MouseOperationType.OPERATION_SIZE;
							}
						}
					}
					if (this.mouseOperationType == MouseOperationType.OPERATION_NONE)
					{
						return null;
					}
					this.controlPointType = (ControlPointType)this.innerControlNode.GetControlPointType();
					if (this.Size.Width == 0f || this.Size.Height == 0f || this.Scale.ScaleX == 0f || this.Scale.ScaleY == 0f)
					{
						this.mouseOperationType = MouseOperationType.OPERATION_POSITION;
					}
					if (!this.OperationFlag.HasFlag(OperationMask.MoveFlag) && this.controlPointType == ControlPointType.POINT_NONE)
					{
						return null;
					}
				}
				CSMatrix anchorWorldMatrix = this.innerControlNode.GetAnchorWorldMatrix();
				PointF pointF = new PointF(anchorWorldMatrix.CX, anchorWorldMatrix.CY);
				float rotate = (float)(-(float)Math.Atan2((double)(point.Y - pointF.Y), (double)(point.X - pointF.X)));
				result = new HitTestResult(this, point, this.mouseOperationType, this.controlPointType, rotate);
			}
			return result;
		}

		public void Dispose()
		{
			TimelineActionManager.Instance.CurrentFrameIndexChangedEvent -= this.OnFrameIndexChanged;
		}

		private CanvasObject canvasObject;

		private CSComControlNode innerControlNode;

		private bool horizontalMove = true;

		private bool verticalMove = true;

		private List<VisualObject> _selectedObjects = new List<VisualObject>();

		private List<VisualObject> _selectedParentObjects = new List<VisualObject>();

		private bool isScaleLocked = false;

		private bool isShiftDown = false;

		private bool isMouseMoved = false;

		private bool isMouseDown = false;

		private RectF movingRect = RectF.Empty;

		private PointF movingPosition;

		private float lastRotation = 0f;

		private PointF lastMousePoint = null;

		private PointF keyVector = null;

		private MouseOperationType mouseOperationType;

		private ControlPointType controlPointType;
	}
}
