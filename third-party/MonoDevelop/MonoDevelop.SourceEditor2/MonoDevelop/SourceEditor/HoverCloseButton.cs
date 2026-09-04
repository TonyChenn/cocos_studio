using System;
using Cairo;
using Gdk;
using Gtk;

namespace MonoDevelop.SourceEditor
{
	public class HoverCloseButton : EventBox
	{
		private bool hovered;

		private bool click;

		public event EventHandler Clicked;

		public HoverCloseButton()
		{
			base.VisibleWindow = false;
			base.Events |= EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | EventMask.EnterNotifyMask | EventMask.LeaveNotifyMask;
		}

		protected override void OnSizeRequested(ref Requisition requisition)
		{
			base.OnSizeRequested(ref requisition);
			requisition.Width = (requisition.Height = 16);
		}

		protected override bool OnEnterNotifyEvent(EventCrossing evnt)
		{
			hovered = true;
			QueueDraw();
			return base.OnEnterNotifyEvent(evnt);
		}

		protected override bool OnLeaveNotifyEvent(EventCrossing evnt)
		{
			hovered = false;
			QueueDraw();
			return base.OnLeaveNotifyEvent(evnt);
		}

		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			if (evnt.Button == 1 && hovered)
			{
				click = true;
			}
			return base.OnButtonPressEvent(evnt);
		}

		protected override bool OnButtonReleaseEvent(EventButton evnt)
		{
			if (click && hovered)
			{
				OnClicked(EventArgs.Empty);
			}
			click = false;
			return base.OnButtonReleaseEvent(evnt);
		}

		protected virtual void OnClicked(EventArgs e)
		{
			Clicked?.Invoke(this, e);
		}

		protected override bool OnExposeEvent(EventExpose evnt)
		{
			using (Context context = CairoHelper.Create(evnt.Window))
			{
				DrawCloseButton(context, new Gdk.Point(base.Allocation.X + base.Allocation.Width / 2, base.Allocation.Y + base.Allocation.Height / 2), hovered, 1.0, 0.0);
			}
			return base.OnExposeEvent(evnt);
		}

		private static void DrawCloseButton(Context context, Gdk.Point center, bool hovered, double opacity, double animationProgress)
		{
			if (hovered)
			{
				double radius = 6.0;
				context.Arc(center.X, center.Y, radius, 0.0, Math.PI * 2.0);
				context.SetSourceRGBA(0.6, 0.6, 0.6, opacity);
				context.Fill();
				context.SetSourceRGBA(0.95, 0.95, 0.95, opacity);
				context.LineWidth = 2.0;
				context.MoveTo(center.X - 3, center.Y - 3);
				context.LineTo(center.X + 3, center.Y + 3);
				context.MoveTo(center.X - 3, center.Y + 3);
				context.LineTo(center.X + 3, center.Y - 3);
				context.Stroke();
				return;
			}
			double num = 0.63 - 0.1 * animationProgress;
			double num2 = 0.74;
			double num3 = Math.Max(0.0, 1.0 - animationProgress * 2.0);
			context.MoveTo(center.X - 3, (double)center.Y - 3.0 * num3);
			context.LineTo(center.X + 3, (double)center.Y + 3.0 * num3);
			context.MoveTo(center.X - 3, (double)center.Y + 3.0 * num3);
			context.LineTo(center.X + 3, (double)center.Y - 3.0 * num3);
			context.LineWidth = 2.0;
			context.SetSourceRGBA(num, num, num, opacity);
			context.Stroke();
			if (animationProgress > 0.5)
			{
				double num4 = (animationProgress - 0.5) * 2.0;
				context.MoveTo(center.X - 3, center.Y);
				context.LineTo(center.X + 3, center.Y);
				context.LineWidth = 2.0 - num4;
				context.SetSourceRGBA(num, num, num, opacity);
				context.Stroke();
				double radius2 = num4 * 3.5;
				context.Arc(center.X, center.Y, radius2, 0.0, Math.PI * 2.0);
				context.SetSourceRGBA(num2, num2, num2, opacity);
				context.Fill();
				using (LinearGradient linearGradient = new LinearGradient(0.0, center.Y - 5, 0.0, center.Y))
				{
					context.Arc(center.X, center.Y + 1, radius2, 0.0, Math.PI * 2.0);
					linearGradient.AddColorStop(0.0, new Cairo.Color(0.0, 0.0, 0.0, 0.2 * opacity));
					linearGradient.AddColorStop(1.0, new Cairo.Color(0.0, 0.0, 0.0, 0.0));
					context.SetSource(linearGradient);
					context.Stroke();
				}
				context.Arc(center.X, center.Y, radius2, 0.0, Math.PI * 2.0);
				context.SetSourceRGBA(num, num, num, opacity);
				context.Stroke();
			}
		}
	}
}
