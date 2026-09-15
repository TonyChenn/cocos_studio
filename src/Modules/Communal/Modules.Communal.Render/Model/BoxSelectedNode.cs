using System;
using System.Drawing;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;

namespace Modules.Communal.Render.Model
{
	public class BoxSelectedNode : DrawNodeObject, IDrawRect
	{
		public static BoxSelectedNode Instance { get; private set; } = new BoxSelectedNode();

		private BoxSelectedNode()
		{
		}

		public void Initialize()
		{
			if (!this.isInitialized)
			{
				this.isInitialized = true;
				this.ZOrder = 100000000;
				GameWindow.Current.GetSceneObject().AddChild(this);
			}
		}

		public void DrawRectangle(CocoStudio.Model.PointF leftTop, CocoStudio.Model.PointF rightButtom)
		{
			CocoStudio.Model.PointF pointF = new CocoStudio.Model.PointF(leftTop.X, leftTop.Y);
			CocoStudio.Model.PointF pointF2 = new CocoStudio.Model.PointF(rightButtom.X, rightButtom.Y);
			pointF = GameWindow.Current.ConvertControlToScene(pointF);
			pointF2 = GameWindow.Current.ConvertControlToScene(pointF2);
			base.DrawRectangle(new CocoStudio.Model.PointF(pointF.X, pointF2.Y), new CocoStudio.Model.PointF(pointF2.X, pointF.Y), BoxSelectedNode.lineColor, 0.5f, false);
		}

		void IDrawRect.Clear()
		{
			base.Clear();
		}

		private const float lineWidth = 0.5f;

		private static readonly Color lineColor = Color.White;

		private bool isInitialized;
	}
}
