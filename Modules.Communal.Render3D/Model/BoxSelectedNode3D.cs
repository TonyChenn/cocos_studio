using System;
using System.Drawing;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Render3D.Model
{
	// Token: 0x02000002 RID: 2
	public class BoxSelectedNode3D : DrawNodeObject, IDrawRect
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public BoxSelectedNode3D(CameraObject cameraObject)
		{
			this.camera = cameraObject;
			this.camera.SetRectDrawNode(this);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000206C File Offset: 0x0000026C
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

		// Token: 0x06000003 RID: 3 RVA: 0x00002132 File Offset: 0x00000332
		void IDrawRect.Clear()
		{
			base.Clear();
		}

		// Token: 0x04000001 RID: 1
		private const float lineWidth = 0.5f;

		// Token: 0x04000002 RID: 2
		private static readonly Color lineColor = Color.White;

		// Token: 0x04000003 RID: 3
		private CameraObject camera;
	}
}
