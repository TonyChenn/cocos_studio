using System;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Render3D.Model.Tool
{
	// Token: 0x02000008 RID: 8
	internal abstract class Object3DTool : BaseObjectTool
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002CA8 File Offset: 0x00000EA8
		public override void Initialize()
		{
			this.controlObject = ControlNode3D.Instance;
			this.controlNode = this.controlObject;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002CC1 File Offset: 0x00000EC1
		protected override bool HitTest(PointF widgetPoint)
		{
			return this.controlObject.HitTest(widgetPoint) != null;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002CD4 File Offset: 0x00000ED4
		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			base.OnMouseUp(args);
			if (!BaseTool.IsMouseMoved(this.clickPoint, args.Event.GetPoint()))
			{
				args.RetVal = true;
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002D01 File Offset: 0x00000F01
		public override PointF ConvertCoordinate(PointF point)
		{
			return point;
		}

		// Token: 0x04000010 RID: 16
		protected ControlNode3D controlObject;
	}
}
