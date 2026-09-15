using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Xwt.Drawing;

namespace Modules.Communal.Skeleton
{
	internal class CreateBindingBoneTool : CreateBoneTool
	{
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
				return ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.CreateBindingBone.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.Skeleton_CreateBone;
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.C;
			}
		}

		protected override void SetSelectedObjects(IEnumerable<VisualObject> selectedObject, IEnumerable<VisualObject> parentSeletedObject)
		{
			base._selectedObjects = selectedObject;
			if (this._attactedObjects != null && this._attactedObjects.Count<VisualObject>() > 0)
			{
				this._attactedObjects.Last<VisualObject>().PropertyChanged -= this.Selected_PropertyChanged;
			}
			this._attactedObjects = parentSeletedObject;
			if (base._selectedObjects != null)
			{
				this._parentBone = (selectedObject.LastOrDefault<VisualObject>() as BoneObject);
				if (this._parentBone == null)
				{
					this._parentBone = (Services.Workbench.ActiveDocument.File.GetRootNode() as SkeletonObject);
				}
				if (this._attactedObjects != null && this._attactedObjects.Count<VisualObject>() > 0)
				{
					this._attactedObjects.Last<VisualObject>().PropertyChanged += this.Selected_PropertyChanged;
				}
				this.RefreshDraw();
				return;
			}
			this._parentBone = null;
		}

		private void Selected_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			string propertyName = e.PropertyName;
			if (this._parentBone == null || propertyName == "IsSelected")
			{
				return;
			}
			this._boneParentCenterPoint = BoneControlObject.Instance.TransformToSelf(CreateBoneTool.GetCenterOfNodeToScene(this._parentBone));
			this.RefreshDraw();
		}

		protected override void RefreshDraw()
		{
			this._boneRackPen.ClearDraw();
			this.DrawConnectLines();
			if (this._isDrawingNewBoneRack)
			{
				PointF pointF = BoneControlObject.Instance.TransformToSelf(this._drawNewBoneEndPos);
				PointF canvasPointToDraw = base.GetCanvasPointToDraw(this._drawNewBonePos);
				PointF start = new PointF((pointF.X + canvasPointToDraw.X) / 2f, (pointF.Y + canvasPointToDraw.Y) / 2f);
				this._boneRackPen.DrawArrowLine(start, this._boneParentCenterPoint, Color4F.YELLOW);
				this._boneRackPen.DrawBoneRack(canvasPointToDraw, pointF, 10f);
			}
		}

		protected void DrawConnectLines()
		{
			if (base._selectedObjects == null)
			{
				return;
			}
			foreach (VisualObject visualObject in base._selectedObjects)
			{
				if (visualObject.Visible && visualObject is BoneObject)
				{
					AbstractNodeObject abstractNodeObject = visualObject as AbstractNodeObject;
					if (abstractNodeObject != null)
					{
						abstractNodeObject = abstractNodeObject.Parent;
					}
					if (abstractNodeObject != null)
					{
						PointF centerOfNodeToDraw = base.GetCenterOfNodeToDraw(visualObject);
						PointF centerOfNodeToDraw2 = base.GetCenterOfNodeToDraw(abstractNodeObject);
						this._boneRackPen.DrawArrowLine(centerOfNodeToDraw, centerOfNodeToDraw2);
					}
				}
			}
		}

		private IEnumerable<VisualObject> _attactedObjects;
	}
}
