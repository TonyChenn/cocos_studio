using System;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x0200001E RID: 30
	internal class ObjectRectangle
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x0000599C File Offset: 0x00003B9C
		// (set) Token: 0x060000DA RID: 218 RVA: 0x000059B3 File Offset: 0x00003BB3
		public VisualObject vObject { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000DB RID: 219 RVA: 0x000059BC File Offset: 0x00003BBC
		// (set) Token: 0x060000DC RID: 220 RVA: 0x000059D3 File Offset: 0x00003BD3
		public float AbsoluteLeft { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000059DC File Offset: 0x00003BDC
		// (set) Token: 0x060000DE RID: 222 RVA: 0x000059F3 File Offset: 0x00003BF3
		public float AbsoluteRight { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000059FC File Offset: 0x00003BFC
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00005A13 File Offset: 0x00003C13
		public float AbsoluteTop { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00005A1C File Offset: 0x00003C1C
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00005A33 File Offset: 0x00003C33
		public float AbsoluteBottom { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00005A3C File Offset: 0x00003C3C
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00005A53 File Offset: 0x00003C53
		public float AbsoluteCenterX { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00005A5C File Offset: 0x00003C5C
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00005A73 File Offset: 0x00003C73
		public float AbsoluteCenterY { get; set; }

		// Token: 0x060000E7 RID: 231 RVA: 0x00005A7C File Offset: 0x00003C7C
		public ObjectRectangle(VisualObject vObj)
		{
			this.vObject = vObj;
			this.RefreshProperty();
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005A96 File Offset: 0x00003C96
		public ObjectRectangle(VisualObject vObj, float left, float right, float top, float bottom)
		{
			this.vObject = vObj;
			this.AbsoluteLeft = left;
			this.AbsoluteRight = right;
			this.AbsoluteTop = top;
			this.AbsoluteBottom = bottom;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005ACC File Offset: 0x00003CCC
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
