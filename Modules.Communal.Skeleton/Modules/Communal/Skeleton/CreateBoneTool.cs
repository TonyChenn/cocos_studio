using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Lib.Prism;
using CocoStudio.Model;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render.Model;
using Xwt.Drawing;

namespace Modules.Communal.Skeleton
{
	// Token: 0x02000003 RID: 3
	internal class CreateBoneTool : BaseTool
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000006 RID: 6 RVA: 0x0000217F File Offset: 0x0000037F
		// (set) Token: 0x06000007 RID: 7 RVA: 0x00002187 File Offset: 0x00000387
		public IEnumerable<VisualObject> _selectedObjects { get; protected set; }

		// Token: 0x06000008 RID: 8 RVA: 0x00002190 File Offset: 0x00000390
		public CreateBoneTool()
		{
			this._boneRackPen = new CSBoneRackDrawPen();
			this._canvasObject = GameWindow.Current.GetCanvasObject();
			this._boneRackPen.SetDrawNodePen(BoneControlObject.Instance.GetCSVisual() as CSDrawNode);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021EE File Offset: 0x000003EE
		public override bool HasSeparator
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000021F1 File Offset: 0x000003F1
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.CreateBone.png");
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000021FD File Offset: 0x000003FD
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.Skeleton_CreateFlagGradeBone;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002204 File Offset: 0x00000404
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.C;
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002208 File Offset: 0x00000408
		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			if (base.IsSelected)
			{
				BoneControlObject.Instance.ResetAsOrigin();
				this.SetSelectedObjects(SelectService.Instance.SelectedObjectList, SelectService.Instance.SelectedParentObjectList);
			}
			else
			{
				this.SetSelectedObjects(null, null);
				this._isDrawingNewBoneRack = false;
			}
			this.RefreshDraw();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002260 File Offset: 0x00000460
		public void OnRefreshControlDraw()
		{
			if (SelectService.Instance.SelectedObjectList.Count == 0)
			{
				return;
			}
			this._boneRackPen.ClearDraw();
			this.RefreshDraw();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002292 File Offset: 0x00000492
		public void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args)
		{
			if (this._boneRackPen == null)
			{
				return;
			}
			this.SetSelectedObjects(args.SelectedObject, args.SelectedParentObject);
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022AF File Offset: 0x000004AF
		internal void ReCalculatePoints()
		{
			this._boneParentCenterPoint = BoneControlObject.Instance.TransformToSelf(CreateBoneTool.GetCenterOfNodeToScene(this._parentBone));
			this.RefreshDraw();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000022D2 File Offset: 0x000004D2
		protected virtual void SetSelectedObjects(IEnumerable<VisualObject> selectedObject, IEnumerable<VisualObject> parentSelectedObject)
		{
			this._selectedObjects = selectedObject;
			if (this._selectedObjects == null)
			{
				this._parentBone = null;
				return;
			}
			if (this._parentBone == null)
			{
				this._parentBone = (Services.ProjectOperations.CurrentSelectedProject.GetRootNode() as SkeletonObject);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002310 File Offset: 0x00000510
		public override void OnMouseDown(ButtonPressEventArgs args)
		{
			if (args.Event.GetMouseButton() == MouseButton.Left)
			{
				args.RetVal = true;
			}
			else if (args.Event.GetMouseButton() == MouseButton.Right)
			{
				args.RetVal = true;
				if (!this._isDrawingNewBoneRack)
				{
					this.Group.Current = this.Group.GetTool<RotateTool>();
				}
			}
			this._lastClickWinPoint.X = (float)args.Event.X;
			this._lastClickWinPoint.Y = (float)args.Event.Y;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000023A0 File Offset: 0x000005A0
		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			args.RetVal = true;
			if (args.Event.GetMouseButton() == MouseButton.Left)
			{
				PointF pointF = this.ConvertCoordinate(new PointF((float)args.Event.X, (float)args.Event.Y));
				if (this._isDrawingNewBoneRack)
				{
					this.EndDrawBoneRack(true);
				}
				else
				{
					this._drawNewBoneEndPos.X = pointF.X + (float)BoneObject.MINLENGHT;
					this._drawNewBoneEndPos.Y = pointF.Y + (float)BoneObject.MINLENGHT;
				}
				this.StartDrawBoneRack(pointF);
				return;
			}
			if (args.Event.GetMouseButton() == MouseButton.Right)
			{
				if (this._isDrawingNewBoneRack)
				{
					this.EndDrawBoneRack(false);
					return;
				}
				args.RetVal = false;
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002460 File Offset: 0x00000660
		public override void OnMouseMove(MotionNotifyEventArgs args)
		{
			PointF pointF = new PointF((float)args.Event.X, (float)args.Event.Y);
			if (this._isDrawingNewBoneRack)
			{
				PointF pointF2 = this.ConvertCoordinate(pointF);
				this._drawNewBoneEndPos.X = pointF2.X;
				this._drawNewBoneEndPos.Y = pointF2.Y;
				this.RefreshDraw();
			}
			else if (args.Event.GetMouseButton() == MouseButton.Left && BaseTool.IsMouseMoved(this._lastClickWinPoint, pointF))
			{
				this.StartDrawBoneRack(this.ConvertCoordinate(pointF));
			}
			args.RetVal = true;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000024FA File Offset: 0x000006FA
		public override void OnKeyUp(KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Escape)
			{
				this.EndDrawBoneRack(false);
			}
			base.OnKeyUp(args);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000251C File Offset: 0x0000071C
		private void StartDrawBoneRack(PointF startPoint)
		{
			this._boneParentCenterPoint = this.GetCenterOfNodeToDraw(this._parentBone);
			this._drawNewBonePos = this._canvasObject.TransformToSelf(startPoint);
			this._isDrawingNewBoneRack = true;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000254C File Offset: 0x0000074C
		private void EndDrawBoneRack(bool create)
		{
			this._isDrawingNewBoneRack = false;
			this._boneRackPen.ClearDraw();
			if (create)
			{
				using (CompositeTask.Run("binding nodes", SelectService.Instance))
				{
					CocosItem file = Services.Workbench.ActiveDocument.File;
					BoneObject boneObject = new BoneObject();
					PointF pointF = this._canvasObject.TransformToSelf(this._drawNewBoneEndPos);
					float num = pointF.X - this._drawNewBonePos.X;
					float num2 = pointF.Y - this._drawNewBonePos.Y;
					float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
					boneObject.Length = ((num3 < (float)BoneObject.MINLENGHT) ? ((float)BoneObject.MINLENGHT) : num3);
					boneObject.Name = file.CreateObjectName(boneObject, "");
					this._parentBone.Children.Add(boneObject);
					boneObject.ZOrder = (file.GetRootNode() as SkeletonObject).SubBonesMaxZOrder + 1;
					(boneObject.GetCSVisual() as CSBoneNode).ResetBoneScaledWidth();
					PointF pointF2 = boneObject.TransformToParent(this._canvasObject.TransformToScene(this._drawNewBonePos));
					PointF pointF3 = boneObject.TransformToParent(this._drawNewBoneEndPos);
					num2 = pointF3.Y - pointF2.Y;
					num = pointF3.X - pointF2.X;
					boneObject.Rotation = -(float)Math.Atan2((double)num2, (double)num) * 57.29578f;
					boneObject.Position = pointF2;
					List<VisualObject> list = new List<VisualObject>
					{
						boneObject
					};
					SelectedVisualObjectsChangeEvent @event = EventAggregator.Instance.GetEvent<SelectedVisualObjectsChangeEvent>();
					@event.Publish(new SelectedVisualObjectsChangeEventArgs(list, list, false));
				}
			}
			this.RefreshDraw();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002710 File Offset: 0x00000910
		protected virtual void RefreshDraw()
		{
			this._boneRackPen.ClearDraw();
			if (this._isDrawingNewBoneRack)
			{
				PointF end = BoneControlObject.Instance.TransformToSelf(this._drawNewBoneEndPos);
				PointF canvasPointToDraw = this.GetCanvasPointToDraw(this._drawNewBonePos);
				this._boneRackPen.DrawBoneRack(canvasPointToDraw, end, 10f);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002760 File Offset: 0x00000960
		protected static PointF GetCenterOfNodeToScene(BoneObject bone)
		{
			SizeF boxSize = bone.BoxSize;
			return bone.TransformToScene(new PointF((bone is SkeletonObject) ? 0f : (boxSize.Width / 2f), 0f));
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000027A0 File Offset: 0x000009A0
		protected PointF GetCenterOfNodeToDraw(VisualObject node)
		{
			PointF pointF = new PointF();
			NodeObject nodeObject = node as NodeObject;
			if (nodeObject != null)
			{
				if (nodeObject.Size.Height - 0f > 0.001f && nodeObject.Size.Height - 0f > 0.001f)
				{
					SizeF boxSize = nodeObject.BoxSize;
					pointF.X = boxSize.Width / 2f;
					pointF.Y = boxSize.Height / 2f;
				}
			}
			else
			{
				BoneObject boneObject = node as BoneObject;
				if (boneObject == null)
				{
					return pointF;
				}
				SizeF boxSize2 = boneObject.BoxSize;
				pointF.X = ((boneObject is SkeletonObject) ? 0f : (boxSize2.Width / 2f));
				pointF.Y = 0f;
			}
			pointF = node.TransformToScene(pointF);
			return BoneControlObject.Instance.TransformToSelf(pointF);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002874 File Offset: 0x00000A74
		protected PointF GetCanvasPointToDraw(PointF point)
		{
			ScaleValue scale = this._canvasObject.Scale;
			return new PointF(point.X * scale.ScaleX, point.Y * scale.ScaleY);
		}

		// Token: 0x04000001 RID: 1
		protected PointF _boneParentCenterPoint;

		// Token: 0x04000002 RID: 2
		protected BoneObject _parentBone;

		// Token: 0x04000003 RID: 3
		protected bool _isDrawingNewBoneRack;

		// Token: 0x04000004 RID: 4
		protected PointF _drawNewBonePos;

		// Token: 0x04000005 RID: 5
		protected CanvasObject _canvasObject;

		// Token: 0x04000006 RID: 6
		protected PointF _drawNewBoneEndPos = new PointF();

		// Token: 0x04000007 RID: 7
		protected CSBoneRackDrawPen _boneRackPen;

		// Token: 0x04000008 RID: 8
		private PointF _lastClickWinPoint = new PointF();
	}
}
