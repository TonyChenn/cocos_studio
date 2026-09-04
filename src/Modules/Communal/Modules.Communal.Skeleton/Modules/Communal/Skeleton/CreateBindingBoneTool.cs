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
	// Token: 0x02000004 RID: 4
	internal class CreateBindingBoneTool : CreateBoneTool
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000028B4 File Offset: 0x00000AB4
		public override bool HasSeparator
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000028B7 File Offset: 0x00000AB7
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.CreateBindingBone.png");
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000028C3 File Offset: 0x00000AC3
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.Skeleton_CreateBone;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000028CA File Offset: 0x00000ACA
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.C;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000028D0 File Offset: 0x00000AD0
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

		// Token: 0x06000022 RID: 34 RVA: 0x000029A0 File Offset: 0x00000BA0
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

		// Token: 0x06000023 RID: 35 RVA: 0x000029EC File Offset: 0x00000BEC
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

		// Token: 0x06000024 RID: 36 RVA: 0x00002A88 File Offset: 0x00000C88
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

		// Token: 0x0400000A RID: 10
		private IEnumerable<VisualObject> _attactedObjects;
	}
}
