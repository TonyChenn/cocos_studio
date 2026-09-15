using System;
using CocoStudio.Projects;
using Gdk;
using Gtk;
using Xwt.Drawing;
using Xwt.GtkBackend;

namespace Modules.Communal.Render.Model
{
	public class SelectTool : BaseTool, IOperateModule, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IActivateControl
	{
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override string Tooltip
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public SelectTool(IDrawRect drawRect, HitTestMode hitTestMode, ToolGroup toolGroup)
		{
			this.drawRect = drawRect;
			this.hitTestMode = hitTestMode;
			this.Group = toolGroup;
		}

		public void Initialize(IGLView glView)
		{
			if (!SelectTool.isInitialized)
			{
				SelectTool.isInitialized = true;
				SelectTool.selectServcie = SelectService.Instance;
				SelectTool.selectServcie.Initialize();
			}
		}

		public override void OnMouseDown(ButtonPressEventArgs args)
		{
			SelectTool.selectServcie.OnMouseDown(args);
			if (!args.CheckRetval() && SelectTool.selectServcie.SelectedObjectList.Count > 0)
			{
				this.Group.Current.OnMouseDown(args);
				args.RetVal = null;
			}
		}

		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			if (!args.Event.IsContextMenuButton())
			{
				SelectTool.selectServcie.OnMouseUp(args);
				args.RetVal = true;
			}
		}

		public override void OnMouseMove(MotionNotifyEventArgs args)
		{
			SelectTool.selectServcie.OnMouseMove(args);
		}

		public override void OnKeyDown(KeyPressEventArgs args)
		{
			SelectTool.selectServcie.OnKeyDown(args);
		}

		public void Activated(CocosItem cocosItem)
		{
			this.drawRect.Visible = true;
			SelectTool.selectServcie.RectNode = this.drawRect;
			HitTestService.Current = this.hitTestMode;
		}

		public void Deactivated()
		{
			this.drawRect.Visible = false;
			this.drawRect.Clear();
		}

		private static SelectService selectServcie;

		private IDrawRect drawRect;

		private HitTestMode hitTestMode;

		private static bool isInitialized = false;
	}
}
