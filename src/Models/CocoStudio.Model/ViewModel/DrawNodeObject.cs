using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.EngineAdapterWrap;

namespace CocoStudio.Model.ViewModel
{
	[DisplayName("DrawNode")]
	public class DrawNodeObject : VisualObject
	{
		private CSDrawNode GetCom()
		{
			return this.innerNode;
		}

		internal override CSVisualObject GetCSVisual()
		{
			return this.innerNode;
		}

		public DrawNodeObject()
		{
			this.innerNode = new CSDrawNode();
		}

		public void SetVisible(bool visible)
		{
			this.innerNode.SetVisible(visible);
		}

		public void DrawDot(PointF pos, float radius, Color color)
		{
			Color4F color2 = new Color4F((float)(color.R / byte.MaxValue), (float)(color.G / byte.MaxValue), (float)(color.B / byte.MaxValue), (float)(color.A / byte.MaxValue));
			this.GetCom().DrawDot(pos, radius, color2);
		}

		public void DrawSegment(PointF from, PointF to, float radius, Color color)
		{
			Color4F color2 = new Color4F((float)(color.R / byte.MaxValue), (float)(color.G / byte.MaxValue), (float)(color.B / byte.MaxValue), (float)(color.A / byte.MaxValue));
			this.GetCom().DrawSegment(from, to, radius, color2);
		}

		public void DrawRectangle(PointF leftop, PointF rightbuttom, Color color, float radius = 1f, bool isfill = false)
		{
			Color4F color2 = new Color4F((float)(color.R / byte.MaxValue), (float)(color.G / byte.MaxValue), (float)(color.B / byte.MaxValue), (float)(color.A / byte.MaxValue));
			this.GetCom().DrawRectangle(leftop, rightbuttom, radius, color2, isfill);
		}

		public void DrawQuadraticBezier(PointF from, PointF control, PointF to, int segments, Color color)
		{
			Color4F color2 = new Color4F((float)(color.R / byte.MaxValue), (float)(color.G / byte.MaxValue), (float)(color.B / byte.MaxValue), (float)(color.A / byte.MaxValue));
			this.GetCom().DrawQuadraticBezier(from, control, to, segments, color2);
		}

		public void Clear()
		{
			this.GetCom().Clear();
		}

		private CSDrawNode innerNode;
	}
}
