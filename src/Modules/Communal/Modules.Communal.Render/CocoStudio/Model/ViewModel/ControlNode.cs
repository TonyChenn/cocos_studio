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
	// Token: 0x02000002 RID: 2
	public class ControlNode : VisualObject, IDisposable
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private void InitContentSizeEvent()
		{
			GlobalCommand.ContentSizeCmd.Execute += this.ContentSizeCmd_Execute;
			GlobalCommand.ContentSizeCmd.Update += this.ContentSizeCmd_Update;
			GlobalCommand.ContentScaleCmd.Execute += this.ContentScaleCmd_Execute;
			GlobalCommand.ContentScaleCmd.Update += this.ContentScaleCmd_Update;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020BC File Offset: 0x000002BC
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

		// Token: 0x06000003 RID: 3 RVA: 0x0000213D File Offset: 0x0000033D
		private void ContentSizeCmd_Execute(object sender, CommandRunArgs e)
		{
			this.SetIsDragChangeSize(true);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002148 File Offset: 0x00000348
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

		// Token: 0x06000005 RID: 5 RVA: 0x000021CC File Offset: 0x000003CC
		private void ContentScaleCmd_Execute(object sender, CommandRunArgs e)
		{
			this.SetIsDragChangeSize(false);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021D8 File Offset: 0x000003D8
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

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002248 File Offset: 0x00000448
		// (set) Token: 0x06000008 RID: 8 RVA: 0x00002260 File Offset: 0x00000460
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

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002298 File Offset: 0x00000498
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000022B0 File Offset: 0x000004B0
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

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000022E0 File Offset: 0x000004E0
		// (set) Token: 0x0600000C RID: 12 RVA: 0x000022FD File Offset: 0x000004FD
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

		// Token: 0x0600000D RID: 13 RVA: 0x00002310 File Offset: 0x00000510
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

		// Token: 0x0600000E RID: 14 RVA: 0x0000243C File Offset: 0x0000063C
		internal override CSVisualObject GetCSVisual()
		{
			return this.innerControlNode;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002454 File Offset: 0x00000654
		private void Init()
		{
			this.innerControlNode.SetAnchorPointVisible(Option.UserConfig.IsShowAnchorPoint);
			this.innerControlNode.SetControlPointVisible(true);
			this.InitProperty();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002484 File Offset: 0x00000684
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

		// Token: 0x06000011 RID: 17 RVA: 0x0000286C File Offset: 0x00000A6C
		private RectF GetBoundingRect(NodeObject node, SizeF size)
		{
			RectF rect = new RectF(PointF.Empty, size);
			return this.innerControlNode.RectApplyTransform(rect, node.GetCSVisual().ConvertToNodeMatrix(this.canvasObject.GetCSVisual()));
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000028B0 File Offset: 0x00000AB0
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

		// Token: 0x06000013 RID: 19 RVA: 0x00002968 File Offset: 0x00000B68
		private void InitAnchorPointEvent()
		{
			GlobalCommand.AnchorPointCmd.Execute += this.AnchorPointCmd_Execute;
			GlobalCommand.AnchorPointCmd.Update += this.AnchorPointCmd_Update;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000299C File Offset: 0x00000B9C
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

		// Token: 0x06000015 RID: 21 RVA: 0x00002A10 File Offset: 0x00000C10
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

		// Token: 0x06000016 RID: 22 RVA: 0x00002A7E File Offset: 0x00000C7E
		private void OnAlignedObjects(AlignedObjectsArgs obj)
		{
			this.Init();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002A88 File Offset: 0x00000C88
		private void OnScaleLockedChange(bool locked)
		{
			this.isScaleLocked = locked;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002A92 File Offset: 0x00000C92
		private void OnCanvasSizeChange(CanvasSizeChangeEventArgs obj)
		{
			this.Init();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002A9C File Offset: 0x00000C9C
		private void OnFrameIndexChanged()
		{
			if (!this.isMouseDown && !Services.TaskService.IsUndoing)
			{
				this.Init();
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002ACC File Offset: 0x00000CCC
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

		// Token: 0x0600001B RID: 27 RVA: 0x00002C08 File Offset: 0x00000E08
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

		// Token: 0x0600001C RID: 28 RVA: 0x00002C8C File Offset: 0x00000E8C
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

		// Token: 0x0600001D RID: 29 RVA: 0x00002D74 File Offset: 0x00000F74
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

		// Token: 0x0600001E RID: 30 RVA: 0x00002DF8 File Offset: 0x00000FF8
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

		// Token: 0x0600001F RID: 31 RVA: 0x00002EA8 File Offset: 0x000010A8
		protected override void OnMouseMove(MouseEventArgs args)
		{
			base.OnMouseMove(args);
			if (this.OperationFlag.HasFlag(OperationMask.MoveFlag) && !(base.lastClickPoint == null) && this.mouseOperationType != MouseOperationType.OPERATION_NONE && this._selectedObjects.Count != 0)
			{
				this.HandleMouseMove(args.Point);
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002F18 File Offset: 0x00001118
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

		// Token: 0x06000021 RID: 33 RVA: 0x00002FF0 File Offset: 0x000011F0
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

		// Token: 0x06000022 RID: 34 RVA: 0x00003098 File Offset: 0x00001298
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

		// Token: 0x06000023 RID: 35 RVA: 0x000033AC File Offset: 0x000015AC
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

		// Token: 0x06000024 RID: 36 RVA: 0x00003434 File Offset: 0x00001634
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

		// Token: 0x06000025 RID: 37 RVA: 0x000039F4 File Offset: 0x00001BF4
		private void HandleSkew(PointF point)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000039FC File Offset: 0x00001BFC
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

		// Token: 0x06000027 RID: 39 RVA: 0x00003CF8 File Offset: 0x00001EF8
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

		// Token: 0x06000028 RID: 40 RVA: 0x00004084 File Offset: 0x00002284
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

		// Token: 0x06000029 RID: 41 RVA: 0x000045A8 File Offset: 0x000027A8
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

		// Token: 0x0600002A RID: 42 RVA: 0x00004750 File Offset: 0x00002950
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

		// Token: 0x0600002B RID: 43 RVA: 0x000049D8 File Offset: 0x00002BD8
		private bool TestFloatEqual(float v1, float v2, float threshold = 0.0001f)
		{
			return Math.Abs(v1 - v2) <= threshold;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00004A04 File Offset: 0x00002C04
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

		// Token: 0x0600002D RID: 45 RVA: 0x00004A50 File Offset: 0x00002C50
		private bool CheckFloatEqual(float v1, float v2, float threshold = 0.0001f)
		{
			return Math.Abs(v1 - v2) <= threshold;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00004A7C File Offset: 0x00002C7C
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

		// Token: 0x0600002F RID: 47 RVA: 0x00004AC8 File Offset: 0x00002CC8
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

		// Token: 0x06000030 RID: 48 RVA: 0x00004B0B File Offset: 0x00002D0B
		private void Print(CSMatrix m)
		{
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00004B10 File Offset: 0x00002D10
		private static float CC_RADIANS_TO_DEGREES(float v)
		{
			return v * 57.29578f;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00004B2C File Offset: 0x00002D2C
		private static float CC_DEGREES_TO_RADIANS(float v)
		{
			return v * 0.017453292f;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00004B48 File Offset: 0x00002D48
		private static PointF PointSub(PointF p1, PointF p2)
		{
			return new PointF(p1.X - p2.X, p1.Y - p2.Y);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00004B7C File Offset: 0x00002D7C
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

		// Token: 0x06000035 RID: 53 RVA: 0x00004D5E File Offset: 0x00002F5E
		public void Dispose()
		{
			TimelineActionManager.Instance.CurrentFrameIndexChangedEvent -= this.OnFrameIndexChanged;
		}

		// Token: 0x04000001 RID: 1
		private CanvasObject canvasObject;

		// Token: 0x04000002 RID: 2
		private CSComControlNode innerControlNode;

		// Token: 0x04000003 RID: 3
		private bool horizontalMove = true;

		// Token: 0x04000004 RID: 4
		private bool verticalMove = true;

		// Token: 0x04000005 RID: 5
		private List<VisualObject> _selectedObjects = new List<VisualObject>();

		// Token: 0x04000006 RID: 6
		private List<VisualObject> _selectedParentObjects = new List<VisualObject>();

		// Token: 0x04000007 RID: 7
		private bool isScaleLocked = false;

		// Token: 0x04000008 RID: 8
		private bool isShiftDown = false;

		// Token: 0x04000009 RID: 9
		private bool isMouseMoved = false;

		// Token: 0x0400000A RID: 10
		private bool isMouseDown = false;

		// Token: 0x0400000B RID: 11
		private RectF movingRect = RectF.Empty;

		// Token: 0x0400000C RID: 12
		private PointF movingPosition;

		// Token: 0x0400000D RID: 13
		private float lastRotation = 0f;

		// Token: 0x0400000E RID: 14
		private PointF lastMousePoint = null;

		// Token: 0x0400000F RID: 15
		private PointF keyVector = null;

		// Token: 0x04000010 RID: 16
		private MouseOperationType mouseOperationType;

		// Token: 0x04000011 RID: 17
		private ControlPointType controlPointType;
	}
}
