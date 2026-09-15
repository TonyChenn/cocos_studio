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
	public class CanvasTool : BaseTool, IOperateModule, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IActivateControl
	{
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Hand.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool2DWidget_CanvasOperation + " (Q)";
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.q;
			}
		}

		public override void Initialize()
		{
			if (!CanvasTool.isInitialized)
			{
				CanvasTool.isInitialized = true;
				CanvasTool.canvasObject = GameWindow.Current.GetCanvasObject();
			}
		}

		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			if (base.IsSelected)
			{
				BaseTool.SetCursor(Cursors.Hand);
			}
		}

		public override void OnMouseEnter(EnterNotifyEventArgs args)
		{
			if (base.IsSelected || KeyboardExtend.IsKeyDown(Gdk.Key.space))
			{
				BaseTool.SetCursor(Cursors.Hand);
				args.RetVal = true;
			}
		}

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

		public override void OnMouseMove(MotionNotifyEventArgs args)
		{
			if (base.IsSelected || KeyboardExtend.IsKeyDown(Gdk.Key.space) || args.Event.GetMouseButton() != MouseButton.None)
			{
				MouseEventArgs args2 = EventArgsConvert.ToMouseEvent(args.Event, this);
				CanvasTool.canvasObject.MouseMove(args2);
				args.RetVal = true;
			}
		}

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

		public override void OnMouseGestures(MouseGesturesEventArgs args)
		{
			PointF mousePoint = GameWindow.Current.ConvertControlToScene(args.GetPoint());
			CanvasZoomChangeEventArgs args2 = new CanvasZoomChangeEventArgs((float)args.Zoom, mousePoint);
			this.ZoomCanvasObject(args2);
			this.RaiseCanvasZoomChanged(args2);
			args.Retval = true;
		}

		public override void OnKeyDown(KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.space && args.Event.State == ModifierType.None)
			{
				BaseTool.SetCursor(Cursors.Hand);
				args.RetVal = true;
			}
		}

		public override void OnKeyUp(KeyReleaseEventArgs args)
		{
			if (!base.IsSelected && args.Event.Key == Gdk.Key.space && args.Event.State == ModifierType.None)
			{
				BaseTool.SetCursor(Cursors.Arrow);
				args.RetVal = true;
			}
		}

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

		private PointF GetPageScrollPixelDeltas(ScrollEventArgs e)
		{
			GameWindow gameWindow = GameWindow.Current;
			double num;
			double num2;
			e.Event.GetPageScrollPixelDeltas((double)gameWindow.Width, (double)gameWindow.Height, out num, out num2);
			return new PointF((float)num, (float)num2);
		}

		private void RaiseCanvasZoomChanged(CanvasZoomChangeEventArgs args)
		{
			IEventAggregator eventsService = Services.EventsService;
			eventsService.GetEvent<CanvasZoomChangeEvent>().Unsubscribe(new Action<CanvasZoomChangeEventArgs>(this.ZoomCanvasObject));
			eventsService.GetEvent<CanvasZoomChangeEvent>().Publish(args);
			eventsService.GetEvent<CanvasZoomChangeEvent>().Subscribe(new Action<CanvasZoomChangeEventArgs>(this.ZoomCanvasObject));
		}

		internal void ZoomCanvasObject(ScaleValue newScale)
		{
			float zoomDelta = newScale.ScaleX - CanvasTool.canvasObject.Scale.ScaleX;
			CanvasZoomChangeEventArgs args = new CanvasZoomChangeEventArgs(zoomDelta);
			this.ZoomCanvasObject(args);
			this.RaiseCanvasZoomChanged(args);
		}

		public void Initialize(IGLView glView)
		{
		}

		public void Activated(CocosItem cocosItem)
		{
			Services.EventsService.GetEvent<CanvasZoomChangeEvent>().Subscribe(new Action<CanvasZoomChangeEventArgs>(this.ZoomCanvasObject));
		}

		public void Deactivated()
		{
			Services.EventsService.GetEvent<CanvasZoomChangeEvent>().Unsubscribe(new Action<CanvasZoomChangeEventArgs>(this.ZoomCanvasObject));
		}

		private const Gdk.Key controlKey = Gdk.Key.space;

		private static bool isInitialized;

		private static CanvasObject canvasObject;
	}
}
