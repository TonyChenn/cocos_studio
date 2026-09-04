using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.EngineAdapterWrap;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000EF RID: 239
	[DisplayName("DrawNode")]
	public class DrawNodeObject : VisualObject
	{
		// Token: 0x060007E5 RID: 2021 RVA: 0x0001F910 File Offset: 0x0001DB10
		private CSDrawNode GetCom()
		{
			return this.innerNode;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0001F928 File Offset: 0x0001DB28
		internal override CSVisualObject GetCSVisual()
		{
			return this.innerNode;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0001F940 File Offset: 0x0001DB40
		public DrawNodeObject()
		{
			this.innerNode = new CSDrawNode();
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0001F956 File Offset: 0x0001DB56
		public void SetVisible(bool visible)
		{
			this.innerNode.SetVisible(visible);
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0001F968 File Offset: 0x0001DB68
		public void DrawDot(PointF pos, float radius, Color color)
		{
			Color4F color2 = new Color4F((float)(color.R / byte.MaxValue), (float)(color.G / byte.MaxValue), (float)(color.B / byte.MaxValue), (float)(color.A / byte.MaxValue));
			this.GetCom().DrawDot(pos, radius, color2);
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0001F9C4 File Offset: 0x0001DBC4
		public void DrawSegment(PointF from, PointF to, float radius, Color color)
		{
			Color4F color2 = new Color4F((float)(color.R / byte.MaxValue), (float)(color.G / byte.MaxValue), (float)(color.B / byte.MaxValue), (float)(color.A / byte.MaxValue));
			this.GetCom().DrawSegment(from, to, radius, color2);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001FA20 File Offset: 0x0001DC20
		public void DrawRectangle(PointF leftop, PointF rightbuttom, Color color, float radius = 1f, bool isfill = false)
		{
			Color4F color2 = new Color4F((float)(color.R / byte.MaxValue), (float)(color.G / byte.MaxValue), (float)(color.B / byte.MaxValue), (float)(color.A / byte.MaxValue));
			this.GetCom().DrawRectangle(leftop, rightbuttom, radius, color2, isfill);
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0001FA80 File Offset: 0x0001DC80
		public void DrawQuadraticBezier(PointF from, PointF control, PointF to, int segments, Color color)
		{
			Color4F color2 = new Color4F((float)(color.R / byte.MaxValue), (float)(color.G / byte.MaxValue), (float)(color.B / byte.MaxValue), (float)(color.A / byte.MaxValue));
			this.GetCom().DrawQuadraticBezier(from, control, to, segments, color2);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0001FADE File Offset: 0x0001DCDE
		public void Clear()
		{
			this.GetCom().Clear();
		}

		// Token: 0x04000327 RID: 807
		private CSDrawNode innerNode;
	}
}
