using System;
using System.Drawing;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Render3D.Model
{
	public class BoxSelectedNode3D : DrawNodeObject, IDrawRect
	{
		public BoxSelectedNode3D(CameraObject cameraObject)
		{
			this.camera = cameraObject;
			this.camera.SetRectDrawNode(this);
		}

		public void DrawRectangle(CocoStudio.Model.PointF leftTop, CocoStudio.Model.PointF rightButtom)
		{
			CocoStudio.Model.PointF pointF = new CocoStudio.Model.PointF(leftTop.X, leftTop.Y);
			CocoStudio.Model.PointF pointF2 = new CocoStudio.Model.PointF(rightButtom.X, rightButtom.Y);
			pointF = GameWindow.Current.ConvertControlToScene(pointF);
			pointF2 = GameWindow.Current.ConvertControlToScene(pointF2);
			base.DrawRectangle(new CocoStudio.Model.PointF(pointF.X, pointF2.Y), new CocoStudio.Model.PointF(pointF2.X, pointF.Y), BoxSelectedNode3D.lineColor, 0.5f, true);
			if (this.camera != null)
			{
				this.camera.UpdateSelectFrustum(new RectF(leftTop.X, leftTop.Y, rightButtom.X - leftTop.X, rightButtom.Y - leftTop.Y));
			}
		}

		void IDrawRect.Clear()
		{
			base.Clear();
		}

		private const float lineWidth = 0.5f;

		private static readonly Color lineColor = Color.White;

		private CameraObject camera;
	}
}
