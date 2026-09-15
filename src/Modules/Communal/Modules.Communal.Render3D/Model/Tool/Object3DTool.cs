using System;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Render3D.Model.Tool
{
	internal abstract class Object3DTool : BaseObjectTool
	{
		public override void Initialize()
		{
			this.controlObject = ControlNode3D.Instance;
			this.controlNode = this.controlObject;
		}

		protected override bool HitTest(PointF widgetPoint)
		{
			return this.controlObject.HitTest(widgetPoint) != null;
		}

		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			base.OnMouseUp(args);
			if (!BaseTool.IsMouseMoved(this.clickPoint, args.Event.GetPoint()))
			{
				args.RetVal = true;
			}
		}

		public override PointF ConvertCoordinate(PointF point)
		{
			return point;
		}

		protected ControlNode3D controlObject;
	}
}
