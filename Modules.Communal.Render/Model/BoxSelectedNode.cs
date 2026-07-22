using System;
using System.Drawing;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000006 RID: 6
	public class BoxSelectedNode : DrawNodeObject, IDrawRect
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00005470 File Offset: 0x00003670
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00005486 File Offset: 0x00003686
		public static BoxSelectedNode Instance { get; private set; } = new BoxSelectedNode();

		// Token: 0x0600004F RID: 79 RVA: 0x000054A7 File Offset: 0x000036A7
		private BoxSelectedNode()
		{
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000054B4 File Offset: 0x000036B4
		public void Initialize()
		{
			if (!this.isInitialized)
			{
				this.isInitialized = true;
				this.ZOrder = 100000000;
				GameWindow.Current.GetSceneObject().AddChild(this);
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000054F8 File Offset: 0x000036F8
		public void DrawRectangle(CocoStudio.Model.PointF leftTop, CocoStudio.Model.PointF rightButtom)
		{
			CocoStudio.Model.PointF pointF = new CocoStudio.Model.PointF(leftTop.X, leftTop.Y);
			CocoStudio.Model.PointF pointF2 = new CocoStudio.Model.PointF(rightButtom.X, rightButtom.Y);
			pointF = GameWindow.Current.ConvertControlToScene(pointF);
			pointF2 = GameWindow.Current.ConvertControlToScene(pointF2);
			base.DrawRectangle(new CocoStudio.Model.PointF(pointF.X, pointF2.Y), new CocoStudio.Model.PointF(pointF2.X, pointF.Y), BoxSelectedNode.lineColor, 0.5f, false);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00005576 File Offset: 0x00003776
		void IDrawRect.Clear()
		{
			base.Clear();
		}

		// Token: 0x04000012 RID: 18
		private const float lineWidth = 0.5f;

		// Token: 0x04000013 RID: 19
		private static readonly Color lineColor = Color.White;

		// Token: 0x04000014 RID: 20
		private bool isInitialized;
	}
}
