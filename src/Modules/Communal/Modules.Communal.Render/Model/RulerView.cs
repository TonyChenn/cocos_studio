using System;
using System.ComponentModel;
using System.Drawing;
using Cairo;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using Gdk;
using Gtk;
using MonoDevelop.Core;

namespace Modules.Communal.Render.Model
{
	public class RulerView : DrawingArea
	{
		public double Zoom { get; set; }

		public double ZeroValue { get; set; }

		public Orientation RulerOrientation { get; set; }

		public RulerView()
		{
			this.ZeroValue = 200.0;
			this.Zoom = 1.0;
			base.CanFocus = true;
			base.Events = EventMask.AllEventsMask;
			base.ButtonPressEvent += this.RulerView_ButtonPressEvent;
			base.ButtonReleaseEvent += this.RulerView_ButtonReleaseEvent;
			base.MotionNotifyEvent += this.RulerView_MotionNotifyEvent;
			base.KeyPressEvent += this.RulerView_KeyPressEvent;
			base.KeyReleaseEvent += this.RulerView_KeyReleaseEvent;
		}

		public void Initialize()
		{
			GameWindow.Current.GetCanvasObject().PropertyChanged += this.CanvasPropertyChangedHandle;
		}

		protected override bool OnExposeEvent(EventExpose evnt)
		{
			using (Context context = CairoHelper.Create(base.GdkWindow))
			{
				Gdk.Rectangle area = evnt.Area;
				area.Width = base.Allocation.Width;
				area.Height = base.Allocation.Height;
				this.context = context;
				this.context.Antialias = Antialias.None;
				this.context.LineWidth = 1.0;
				this.RenderBackground(area);
				this.RenderBottomLine(area);
				this.RenderLine(area);
			}
			return base.OnExposeEvent(evnt);
		}

		private void RenderBackground(Gdk.Rectangle area)
		{
			System.Drawing.Color color_Background = RulerConsts.Color_Background;
			this.context.SetSourceRGBA((double)color_Background.R / 255.0, (double)color_Background.G / 255.0, (double)color_Background.B / 255.0, 1.0);
			this.context.Rectangle((double)area.X, (double)area.Y, (double)area.Width, (double)area.Height);
			this.context.Fill();
		}

		private void RenderBottomLine(Gdk.Rectangle area)
		{
			this.context.SetColor(RulerConsts.Color_Line);
			if (this.RulerOrientation == Orientation.Horizontal)
			{
				this.context.MoveTo(0.0, 1.0);
				this.context.LineTo((double)area.Width, 1.0);
			}
			else
			{
				double x = 18.0;
				if (Platform.IsMac)
				{
					x = 17.0;
				}
				this.context.MoveTo(x, 0.0);
				this.context.LineTo(x, (double)area.Height);
			}
			this.context.Stroke();
		}

		private void RenderLine(Gdk.Rectangle area)
		{
			this.context.SetFontSize(10.0);
			this.forPixelUnit = ((this.Zoom >= 0.32) ? 100 : 1000);
			double num = this.ZeroValue / (double)this.forPixelUnit;
			this.rulerLength = (double)((this.RulerOrientation == Orientation.Horizontal) ? area.Width : area.Height);
			double num2 = this.rulerLength / (double)this.forPixelUnit / this.Zoom - num;
			this.DrawLine(this.ZeroValue * this.Zoom, 10.0);
			this.DrawText(this.ZeroValue * this.Zoom, 0);
			this.isLeftZero = true;
			for (double num3 = 0.0; num3 < num; num3 += 1.0)
			{
				this.DrawThinLine(num3);
			}
			this.isLeftZero = false;
			for (double num4 = 0.0; num4 < num2; num4 += 1.0)
			{
				this.DrawThinLine(num4);
			}
		}

		private void DrawThinLine(double dUnit)
		{
			if (this.Zoom >= 0.09 && this.Zoom < 0.16)
			{
				this.DrawContent(dUnit, 20, 2, 10);
			}
			else if (this.Zoom >= 0.16 && this.Zoom < 0.25)
			{
				this.DrawContent(dUnit, 20, 2, 4);
			}
			else if (this.Zoom >= 0.25 && this.Zoom < 0.32)
			{
				this.DrawContent(dUnit, 50, 5, 10);
			}
			else if (this.Zoom >= 0.32 && this.Zoom < 0.5)
			{
				this.DrawContent(dUnit, 5, 6, 5);
			}
			else if (this.Zoom >= 0.5 && this.Zoom < 0.63)
			{
				this.DrawContent(dUnit, 10, 5, 10);
			}
			else if (this.Zoom >= 0.63 && this.Zoom < 0.98)
			{
				this.DrawContent(dUnit, 10, 11, 5);
			}
			else if (this.Zoom >= 0.98 && this.Zoom < 1.6)
			{
				this.DrawContent(dUnit, 20, 2, 10);
			}
			else if (this.Zoom >= 1.6 && this.Zoom < 2.5)
			{
				this.DrawContent(dUnit, 20, 2, 4);
			}
			else if (this.Zoom >= 2.5 && this.Zoom < 3.2)
			{
				this.DrawContent(dUnit, 50, 5, 10);
			}
			else
			{
				this.DrawContent(dUnit, 50, 51, 5);
			}
		}

		private void DrawContent(double dUnit, int forCount, int centerLineCount, int longLineCount)
		{
			double num = dUnit * (double)this.forPixelUnit;
			double num2 = (double)(this.forPixelUnit / forCount) * this.Zoom;
			double num3;
			if (this.isLeftZero)
			{
				num2 = -num2;
				num3 = (this.ZeroValue - num) * this.Zoom;
			}
			else
			{
				num3 = (this.ZeroValue + num) * this.Zoom;
			}
			for (int i = 1; i <= forCount; i++)
			{
				double num4 = num3 + (double)i * num2;
				if (i % longLineCount == 0)
				{
					this.DrawLine(num4, 10.0);
					int num5 = (int)num + i * (this.forPixelUnit / forCount);
					num5 = (this.isLeftZero ? (-num5) : num5);
					this.DrawText(num4, num5);
				}
				else if (i % centerLineCount == 0)
				{
					this.DrawLine(num4, 7.0);
				}
				else
				{
					this.DrawLine(num4, 5.0);
				}
			}
		}

		private void DrawText(double point_x, int text)
		{
			this.context.SetColor(RulerConsts.Color_Text);
			if (this.RulerOrientation == Orientation.Horizontal)
			{
				double num = (double)text.ToString().Length * 2.65;
				this.context.MoveTo(point_x - num, 18.0);
				this.context.ShowText(text.ToString());
			}
			else
			{
				double num = (double)text.ToString().Length * 2.65;
				this.context.MoveTo(0.0, this.rulerLength - point_x - num);
				this.context.Rotate(1.5707963267948966);
				this.context.ShowText(text.ToString());
				this.context.Rotate(-1.5707963267948966);
			}
		}

		private void DrawLine(double x, double h)
		{
			this.context.SetColor(RulerConsts.Color_Line);
			if (this.RulerOrientation == Orientation.Horizontal)
			{
				this.context.MoveTo(x, 1.0);
				this.context.LineTo(x, h);
			}
			else
			{
				this.context.MoveTo(17.0, this.rulerLength - x + 1.0);
				this.context.LineTo(18.0 - h, this.rulerLength - x + 1.0);
			}
			this.context.Stroke();
		}

		private void DragNewLine(MotionNotifyEventArgs args)
		{
			if (!this.hasCreateNewLine)
			{
				this.AddGuides(args);
				this.hasCreateNewLine = true;
			}
			GuidesService.Instance.OnMouseMove(args);
		}

		private void AddGuides(MotionNotifyEventArgs args)
		{
			CocoStudio.Model.PointF pointF = this.GetToCanvasPosition(args);
			GuidesObject guidesObject = new GuidesObject(this.RulerOrientation);
			pointF = GameWindow.Current.ConvertControlToScene(pointF);
			pointF = GameWindow.Current.GetCanvasObject().TransformToSelf(pointF);
			switch (this.RulerOrientation)
			{
			case Orientation.Horizontal:
				guidesObject.Position = pointF.Y;
				break;
			case Orientation.Vertical:
				guidesObject.Position = pointF.X;
				break;
			}
			GuidesService.Instance.DragNewGuides(guidesObject);
		}

		private CocoStudio.Model.PointF GetToCanvasPosition(MotionNotifyEventArgs args)
		{
			CocoStudio.Model.PointF pointF = this.ConvertMovePosition(args.Event.X, args.Event.Y);
			pointF = GameWindow.Current.ConvertControlToScene(pointF);
			return GameWindow.Current.GetCanvasObject().TransformToSelf(pointF);
		}

		private CocoStudio.Model.PointF ConvertMovePosition(double x, double y)
		{
			CocoStudio.Model.PointF pointF = new CocoStudio.Model.PointF((float)x, (float)y);
			switch (this.RulerOrientation)
			{
			case Orientation.Horizontal:
				pointF.Y = (float)(GameWindow.Current.Height + (int)y);
				break;
			case Orientation.Vertical:
				pointF.X -= (float)base.Allocation.Width;
				break;
			}
			return pointF;
		}

		private void CanvasPropertyChangedHandle(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			CanvasObject canvas = sender as CanvasObject;
			if (e.PropertyName == "Position" || e.PropertyName == "Scale")
			{
				this.QueueDrawRuler(canvas);
			}
		}

		private void QueueDrawRuler(CanvasObject canvas)
		{
			CocoStudio.Model.PointF position = canvas.Position;
			if (this.RulerOrientation == Orientation.Horizontal)
			{
				this.ZeroValue = (double)(position.X / canvas.Scale.ScaleX);
			}
			else
			{
				this.ZeroValue = (double)(position.Y / canvas.Scale.ScaleX);
			}
			this.Zoom = (double)canvas.Scale.ScaleX;
			base.QueueDraw();
		}

		private void RulerView_ButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			if (args.Event.GetMouseButton() == MouseButton.Left)
			{
				base.HasFocus = true;
				this.mousePress_x = args.Event.X;
				this.mousePress_y = args.Event.Y;
			}
		}

		private void RulerView_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			base.GdkWindow.Cursor = null;
			this.hasCreateNewLine = false;
			CocoStudio.Model.PointF pointF = this.ConvertMovePosition(args.Event.X, args.Event.Y);
			GameWindow gameWindow = GameWindow.Current;
			bool isDeleted = pointF.X < 0f || pointF.X > (float)gameWindow.Width || pointF.Y < 0f || pointF.Y > (float)gameWindow.Height;
			GuidesService.Instance.DragFinished(isDeleted);
		}

		private void RulerView_MotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			if (KeyboardExtend.IsMousePressed(args.Event.State))
			{
				this.SetCursor();
				double num = args.Event.X - this.mousePress_x;
				double num2 = args.Event.Y - this.mousePress_y;
				double num3 = Math.Sqrt(num * num + num2 * num2);
				if (num3 >= 5.0)
				{
					this.DragNewLine(args);
				}
			}
		}

		private void SetCursor()
		{
			if (this.RulerOrientation == Orientation.Horizontal)
			{
				base.GdkWindow.Cursor = Cursors.HorizontalCursor;
			}
			else
			{
				base.GdkWindow.Cursor = Cursors.VerticalCursor;
			}
		}

		private void RulerView_KeyPressEvent(object o, KeyPressEventArgs args)
		{
			GuidesService.Instance.OnKeyDown(args);
		}

		private void RulerView_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			GuidesService.Instance.OnKeyUp(args);
		}

		private double rulerLength;

		private int forPixelUnit;

		private Context context;

		private bool isLeftZero = false;

		private double mousePress_x;

		private double mousePress_y;

		private bool hasCreateNewLine = false;
	}
}
