using System;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;

namespace Modules.UI.RenderContextMenu
{
	internal class ObjectRectangle
	{
		public VisualObject vObject { get; set; }

		public float AbsoluteLeft { get; set; }

		public float AbsoluteRight { get; set; }

		public float AbsoluteTop { get; set; }

		public float AbsoluteBottom { get; set; }

		public float AbsoluteCenterX { get; set; }

		public float AbsoluteCenterY { get; set; }

		public ObjectRectangle(VisualObject vObj)
		{
			this.vObject = vObj;
			this.RefreshProperty();
		}

		public ObjectRectangle(VisualObject vObj, float left, float right, float top, float bottom)
		{
			this.vObject = vObj;
			this.AbsoluteLeft = left;
			this.AbsoluteRight = right;
			this.AbsoluteTop = top;
			this.AbsoluteBottom = bottom;
		}

		public void RefreshProperty()
		{
			SizeF size = this.vObject.Size;
			float width = size.Width;
			float height = size.Height;
			PointF selfPoint = new PointF(0f, 0f);
			PointF selfPoint2 = new PointF(0f, height);
			PointF selfPoint3 = new PointF(width, height);
			PointF selfPoint4 = new PointF(width, 0f);
			float num = 10000f;
			float num2 = -10000f;
			float num3 = -10000f;
			float num4 = 10000f;
			PointF pointF = Services.ProjectOperations.CurrentSelectedProject.GetRootNode().TransformToScene(new PointF(0f, 0f));
			PointF pointF2 = this.vObject.TransformToScene(selfPoint);
			PointF pointF3 = this.vObject.TransformToScene(selfPoint2);
			PointF pointF4 = this.vObject.TransformToScene(selfPoint3);
			PointF pointF5 = this.vObject.TransformToScene(selfPoint4);
			pointF2.X -= pointF.X;
			pointF3.X -= pointF.X;
			pointF4.X -= pointF.X;
			pointF5.X -= pointF.X;
			pointF2.Y -= pointF.Y;
			pointF3.Y -= pointF.Y;
			pointF4.Y -= pointF.Y;
			pointF5.Y -= pointF.Y;
			num = Math.Min(num, pointF2.X);
			num = Math.Min(num, pointF3.X);
			num = Math.Min(num, pointF4.X);
			num = Math.Min(num, pointF5.X);
			num2 = Math.Max(num2, pointF2.X);
			num2 = Math.Max(num2, pointF3.X);
			num2 = Math.Max(num2, pointF4.X);
			num2 = Math.Max(num2, pointF5.X);
			num3 = Math.Max(num3, pointF2.Y);
			num3 = Math.Max(num3, pointF3.Y);
			num3 = Math.Max(num3, pointF4.Y);
			num3 = Math.Max(num3, pointF5.Y);
			num4 = Math.Min(num4, pointF2.Y);
			num4 = Math.Min(num4, pointF3.Y);
			num4 = Math.Min(num4, pointF4.Y);
			num4 = Math.Min(num4, pointF5.Y);
			this.AbsoluteLeft = num;
			this.AbsoluteRight = num2;
			this.AbsoluteTop = num3;
			this.AbsoluteBottom = num4;
			this.AbsoluteCenterX = (num2 + num) * 0.5f;
			this.AbsoluteCenterY = (num3 + num4) * 0.5f;
		}
	}
}
