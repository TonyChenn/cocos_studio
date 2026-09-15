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
	internal class CreateBoneTool : BaseTool
	{
		public IEnumerable<VisualObject> _selectedObjects { get; protected set; }

		public CreateBoneTool()
		{
			this._boneRackPen = new CSBoneRackDrawPen();
			this._canvasObject = GameWindow.Current.GetCanvasObject();
			this._boneRackPen.SetDrawNodePen(BoneControlObject.Instance.GetCSVisual() as CSDrawNode);
		}

		public override bool HasSeparator
		{
			get
			{
				return true;
			}
		}

		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.CreateBone.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.Skeleton_CreateFlagGradeBone;
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.C;
			}
		}

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

		public void OnRefreshControlDraw()
		{
			if (SelectService.Instance.SelectedObjectList.Count == 0)
			{
				return;
			}
			this._boneRackPen.ClearDraw();
			this.RefreshDraw();
		}

		public void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args)
		{
			if (this._boneRackPen == null)
			{
				return;
			}
			this.SetSelectedObjects(args.SelectedObject, args.SelectedParentObject);
		}

		internal void ReCalculatePoints()
		{
			this._boneParentCenterPoint = BoneControlObject.Instance.TransformToSelf(CreateBoneTool.GetCenterOfNodeToScene(this._parentBone));
			this.RefreshDraw();
		}

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

		public override void OnKeyUp(KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Escape)
			{
				this.EndDrawBoneRack(false);
			}
			base.OnKeyUp(args);
		}

		private void StartDrawBoneRack(PointF startPoint)
		{
			this._boneParentCenterPoint = this.GetCenterOfNodeToDraw(this._parentBone);
			this._drawNewBonePos = this._canvasObject.TransformToSelf(startPoint);
			this._isDrawingNewBoneRack = true;
		}

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

		protected static PointF GetCenterOfNodeToScene(BoneObject bone)
		{
			SizeF boxSize = bone.BoxSize;
			return bone.TransformToScene(new PointF((bone is SkeletonObject) ? 0f : (boxSize.Width / 2f), 0f));
		}

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

		protected PointF GetCanvasPointToDraw(PointF point)
		{
			ScaleValue scale = this._canvasObject.Scale;
			return new PointF(point.X * scale.ScaleX, point.Y * scale.ScaleY);
		}

		protected PointF _boneParentCenterPoint;

		protected BoneObject _parentBone;

		protected bool _isDrawingNewBoneRack;

		protected PointF _drawNewBonePos;

		protected CanvasObject _canvasObject;

		protected PointF _drawNewBoneEndPos = new PointF();

		protected CSBoneRackDrawPen _boneRackPen;

		private PointF _lastClickWinPoint = new PointF();
	}
}
