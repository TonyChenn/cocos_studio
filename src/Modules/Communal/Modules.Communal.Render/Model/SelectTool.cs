using System;
using CocoStudio.Projects;
using Gdk;
using Gtk;
using Xwt.Drawing;
using Xwt.GtkBackend;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000030 RID: 48
	public class SelectTool : BaseTool, IOperateModule, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IActivateControl
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000BAE4 File Offset: 0x00009CE4
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001FB RID: 507 RVA: 0x0000BAEC File Offset: 0x00009CEC
		public override string Tooltip
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000BAF4 File Offset: 0x00009CF4
		public override Gdk.Key ShortcutKey
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000BAFC File Offset: 0x00009CFC
		public SelectTool(IDrawRect drawRect, HitTestMode hitTestMode, ToolGroup toolGroup)
		{
			this.drawRect = drawRect;
			this.hitTestMode = hitTestMode;
			this.Group = toolGroup;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000BB20 File Offset: 0x00009D20
		public void Initialize(IGLView glView)
		{
			if (!SelectTool.isInitialized)
			{
				SelectTool.isInitialized = true;
				SelectTool.selectServcie = SelectService.Instance;
				SelectTool.selectServcie.Initialize();
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000BB58 File Offset: 0x00009D58
		public override void OnMouseDown(ButtonPressEventArgs args)
		{
			SelectTool.selectServcie.OnMouseDown(args);
			if (!args.CheckRetval() && SelectTool.selectServcie.SelectedObjectList.Count > 0)
			{
				this.Group.Current.OnMouseDown(args);
				args.RetVal = null;
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000BBB4 File Offset: 0x00009DB4
		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			if (!args.Event.IsContextMenuButton())
			{
				SelectTool.selectServcie.OnMouseUp(args);
				args.RetVal = true;
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000BBEF File Offset: 0x00009DEF
		public override void OnMouseMove(MotionNotifyEventArgs args)
		{
			SelectTool.selectServcie.OnMouseMove(args);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000BBFE File Offset: 0x00009DFE
		public override void OnKeyDown(KeyPressEventArgs args)
		{
			SelectTool.selectServcie.OnKeyDown(args);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000BC0D File Offset: 0x00009E0D
		public void Activated(CocosItem cocosItem)
		{
			this.drawRect.Visible = true;
			SelectTool.selectServcie.RectNode = this.drawRect;
			HitTestService.Current = this.hitTestMode;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000BC3A File Offset: 0x00009E3A
		public void Deactivated()
		{
			this.drawRect.Visible = false;
			this.drawRect.Clear();
		}

		// Token: 0x0400008E RID: 142
		private static SelectService selectServcie;

		// Token: 0x0400008F RID: 143
		private IDrawRect drawRect;

		// Token: 0x04000090 RID: 144
		private HitTestMode hitTestMode;

		// Token: 0x04000091 RID: 145
		private static bool isInitialized = false;
	}
}
