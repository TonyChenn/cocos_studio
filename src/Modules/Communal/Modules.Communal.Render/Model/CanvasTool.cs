using System;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Lib.Prism;
using CocoStudio.Model;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.TextEditor;
using MonoDevelop.Core;
using Xwt.Drawing;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000019 RID: 25
	public class CanvasTool : BaseTool, IOperateModule, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IActivateControl
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000067BC File Offset: 0x000049BC
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Hand.png");
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000067D8 File Offset: 0x000049D8
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool2DWidget_CanvasOperation + " (Q)";
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000067FC File Offset: 0x000049FC
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.q;
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006810 File Offset: 0x00004A10
		public override void Initialize()
		{
			if (!CanvasTool.isInitialized)
			{
				CanvasTool.isInitialized = true;
				CanvasTool.canvasObject = GameWindow.Current.GetCanvasObject();
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006844 File Offset: 0x00004A44
		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			if (base.IsSelected)
			{
				BaseTool.SetCursor(Cursors.Hand);
			}
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006874 File Offset: 0x00004A74
		public override void OnMouseEnter(EnterNotifyEventArgs args)
		{
			if (base.IsSelected || KeyboardExtend.IsKeyDown(Gdk.Key.space))
			{
				BaseTool.SetCursor(Cursors.Hand);
				args.RetVal = true;
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000068B8 File Offset: 0x00004AB8
		public override void OnMouseDown(ButtonPressEventArgs args)
		{
			if (base.IsSelected || KeyboardExtend.IsKeyDown(Gdk.Key.space) || args.Event.GetMouseButton() == MouseButton.Middle)
			{
				BaseTool.SetCursor(Cursors.Fist);
				MouseEventArgs args2 = EventArgsConvert.ToMouseEvent(args.Event, this);
				CanvasTool.canvasObject.MouseDown(args2);
				args.RetVal = true;
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00006924 File Offset: 0x00004B24
		public override void OnMouseMove(MotionNotifyEventArgs args)
		{
			if (base.IsSelected || KeyboardExtend.IsKeyDown(Gdk.Key.space) || args.Event.GetMouseButton() != MouseButton.None)
			{
				MouseEventArgs args2 = EventArgsConvert.ToMouseEvent(args.Event, this);
				CanvasTool.canvasObject.MouseMove(args2);
				args.RetVal = true;
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00006984 File Offset: 0x00004B84
		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			if (base.IsSelected || KeyboardExtend.IsKeyDown(Gdk.Key.space))
			{
				BaseTool.SetCursor(Cursors.Hand);
			}
			else
			{
				BaseTool.SetCursor(Cursors.Arrow);
			}
			MouseEventArgs args2 = EventArgsConvert.ToMouseEvent(args.Event, this);
			CanvasTool.canvasObject.MouseUp(args2);
			args.RetVal = true;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000069EC File Offset: 0x00004BEC
		public override void OnMouseWheel(ScrollEventArgs args)
		{
			if (Option.UserConfig.IsUseMouseWheel || KeyboardExtend.IsKeyDown(Gdk.Key.Alt_L) || KeyboardExtend.IsKeyDown(Gdk.Key.Alt_R))
			{
				PointF pageScrollPixelDeltas = this.GetPageScrollPixelDeltas(args);
				PointF mousePoint = GameWindow.Current.ConvertControlToScene(args.Event.GetPoint());
				CanvasZoomChangeEventArgs args2 = new CanvasZoomChangeEventArgs(-pageScrollPixelDeltas.Y / 96f * 0.1f, mousePoint);
				this.ZoomCanvasObject(args2);
				this.RaiseCanvasZoomChanged(args2);
				args.RetVal = true;
			}
			else if (Platform.IsMac)
			{
				this.MoveCanvasObject(args);
				args.RetVal = true;
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00006AA8 File Offset: 0x00004CA8
		public override void OnMouseGestures(MouseGesturesEventArgs args)
		{
			PointF mousePoint = GameWindow.Current.ConvertControlToScene(args.GetPoint());
			CanvasZoomChangeEventArgs args2 = new CanvasZoomChangeEventArgs((float)args.Zoom, mousePoint);
			this.ZoomCanvasObject(args2);
			this.RaiseCanvasZoomChanged(args2);
			args.Retval = true;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00006AF0 File Offset: 0x00004CF0
		public override void OnKeyDown(KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.space && args.Event.State == ModifierType.None)
			{
				BaseTool.SetCursor(Cursors.Hand);
				args.RetVal = true;
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00006B40 File Offset: 0x00004D40
		public override void OnKeyUp(KeyReleaseEventArgs args)
		{
			if (!base.IsSelected && args.Event.Key == Gdk.Key.space && args.Event.State == ModifierType.None)
			{
				BaseTool.SetCursor(Cursors.Arrow);
				args.RetVal = true;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00006B98 File Offset: 0x00004D98
		private void ZoomCanvasObject(CanvasZoomChangeEventArgs args)
		{
			CanvasObject canvasObject = CanvasTool.canvasObject;
			ScaleValue scaleValue = canvasObject.ComputeScaleValue(args.ZoomDelta);
			if (scaleValue != null)
			{
				ScaleValue scaleValue2 = scaleValue;
				ScaleValue scale = canvasObject.Scale;
				PointF position = canvasObject.Position;
				canvasObject.Scale = scaleValue2;
				if (args.MousePoint != null)
				{
					PointF mousePoint = args.MousePoint;
					float num = canvasObject.Size.Width * scale.ScaleX;
					float num2 = canvasObject.Size.Height * scale.ScaleY;
					float num3 = position.X + num / 2f;
					float num4 = position.Y + num2 / 2f;
					float num5 = (num3 - mousePoint.X) / scale.ScaleX * (scaleValue2.ScaleX - scale.ScaleX);
					float num6 = (num4 - mousePoint.Y) / scale.ScaleY * (scaleValue2.ScaleY - scale.ScaleY);
					canvasObject.Position = new PointF(canvasObject.Position.X + num5, canvasObject.Position.Y + num6);
				}
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00006CBC File Offset: 0x00004EBC
		private void MoveCanvasObject(ScrollEventArgs e)
		{
			PointF pageScrollPixelDeltas = this.GetPageScrollPixelDeltas(e);
			if (pageScrollPixelDeltas.X != 0f || pageScrollPixelDeltas.Y != 0f)
			{
				PointF position = CanvasTool.canvasObject.Position;
				position.X -= pageScrollPixelDeltas.X / 2f;
				position.Y += pageScrollPixelDeltas.Y / 2f;
				CanvasTool.canvasObject.Position = position;
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00006D48 File Offset: 0x00004F48
		private PointF GetPageScrollPixelDeltas(ScrollEventArgs e)
		{
			GameWindow gameWindow = GameWindow.Current;
			double num;
			double num2;
			e.Event.GetPageScrollPixelDeltas((double)gameWindow.Width, (double)gameWindow.Height, out num, out num2);
			return new PointF((float)num, (float)num2);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00006D88 File Offset: 0x00004F88
		private void RaiseCanvasZoomChanged(CanvasZoomChangeEventArgs args)
		{
			IEventAggregator eventsService = Services.EventsService;
			eventsService.GetEvent<CanvasZoomChangeEvent>().Unsubscribe(new Action<CanvasZoomChangeEventArgs>(this.ZoomCanvasObject));
			eventsService.GetEvent<CanvasZoomChangeEvent>().Publish(args);
			eventsService.GetEvent<CanvasZoomChangeEvent>().Subscribe(new Action<CanvasZoomChangeEventArgs>(this.ZoomCanvasObject));
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00006DDC File Offset: 0x00004FDC
		internal void ZoomCanvasObject(ScaleValue newScale)
		{
			float zoomDelta = newScale.ScaleX - CanvasTool.canvasObject.Scale.ScaleX;
			CanvasZoomChangeEventArgs args = new CanvasZoomChangeEventArgs(zoomDelta);
			this.ZoomCanvasObject(args);
			this.RaiseCanvasZoomChanged(args);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006E18 File Offset: 0x00005018
		public void Initialize(IGLView glView)
		{
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006E1B File Offset: 0x0000501B
		public void Activated(CocosItem cocosItem)
		{
			Services.EventsService.GetEvent<CanvasZoomChangeEvent>().Subscribe(new Action<CanvasZoomChangeEventArgs>(this.ZoomCanvasObject));
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006E3A File Offset: 0x0000503A
		public void Deactivated()
		{
			Services.EventsService.GetEvent<CanvasZoomChangeEvent>().Unsubscribe(new Action<CanvasZoomChangeEventArgs>(this.ZoomCanvasObject));
		}

		// Token: 0x0400002B RID: 43
		private const Gdk.Key controlKey = Gdk.Key.space;

		// Token: 0x0400002C RID: 44
		private static bool isInitialized;

		// Token: 0x0400002D RID: 45
		private static CanvasObject canvasObject;
	}
}
