using System;
using System.Collections.Generic;
using Cairo;
using CocoStudio.Basic;
using Gdk;
using GLib;
using Gtk;
using MonoDevelop.Core;

namespace Modules.Communal.TexturePacker
{
	public class PlistInfoItemRender : DrawingArea
	{
		public double Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
				List<PlistInfoItem> items = this.Model.Items;
				if (items != null)
				{
					foreach (PlistInfoItem plistInfoItem in items)
					{
						plistInfoItem.RenderScale = this.scale;
					}
				}
				base.QueueDraw();
			}
		}

		public Gdk.Color BgColor
		{
			get
			{
				return this.bgColor;
			}
			set
			{
				this.bgColor = value;
				base.QueueDraw();
			}
		}

		public PlistInfoItemRender(PlistInfoModel model)
		{
			this.Model = model;
			base.WidthRequest = this.Model.SizeRequest.Width;
			base.HeightRequest = this.Model.SizeRequest.Height;
			this.Scale = 1.0;
			base.CanFocus = true;
		}

		protected override bool OnExposeEvent(EventExpose evnt)
		{
			this.DrawBackground();
			this.DrawSprites();
			this.DrawSelectAndMaxRect();
			return base.OnExposeEvent(evnt);
		}

		private void DrawBackground()
		{
			using (Context context = CairoHelper.Create(base.GdkWindow))
			{
				int num;
				int num2;
				base.GdkWindow.GetSize(out num, out num2);
				context.SetSourceRGB(0.254901960781, 0.254901960781, 0.274509803918);
				context.Rectangle(0.0, 0.0, (double)num, (double)num2);
				context.Fill();
				context.Stroke();
				int num3 = (int)((double)this.Model.RealSize.Width * this.Scale);
				int num4 = (int)((double)this.Model.RealSize.Height * this.Scale);
				context.SetSourceRGB(0.19607843137000003, 0.19607843137000003, 0.2117647058796);
				context.Rectangle((double)this.Position.X, (double)this.Position.Y, (double)num3, (double)num4);
				context.Fill();
				context.Stroke();
			}
		}

		private void DrawSprites()
		{
			List<PlistInfoItem> items = this.Model.Items;
			if (items == null)
			{
				return;
			}
			foreach (PlistInfoItem plistInfoItem in items)
			{
				plistInfoItem.Draw(this);
			}
		}

		private void DrawSelectAndMaxRect()
		{
			using (Context context = CairoHelper.Create(base.GdkWindow))
			{
				context.SetSourceRGBA(0.0, 0.0, 0.7, 0.5);
				context.Rectangle((double)this.selectedRect.X, (double)this.selectedRect.Y, (double)this.selectedRect.Width, (double)this.selectedRect.Height);
				context.Fill();
				context.Stroke();
				context.LineWidth = 0.6;
				context.SetSourceRGB(1.0, 1.0, 1.0);
				context.Rectangle((double)this.Position.X, (double)this.Position.Y, (double)this.Model.MaxSize.Width, (double)this.Model.MaxSize.Height);
				context.Stroke();
			}
		}

		private bool IsControlPressed(EventButton evnt)
		{
			bool result = false;
			if (Platform.IsMac && evnt.State.HasFlag(ModifierType.Mod2Mask | ModifierType.MetaMask))
			{
				result = true;
			}
			else if (evnt.State.HasFlag(ModifierType.ControlMask))
			{
				result = true;
			}
			return result;
		}

		internal void OnButtonPressEvent(object o, ButtonPressEventArgs args)
		{
			base.GrabFocus();
			bool flag = args.Event.GetMouseButton() == MouseButton.Right;
			this.pressed = true;
			this.clickedOnItem = false;
			this.lastPoint = (this.pressedPoint = new Gdk.Point((int)args.Event.X, (int)args.Event.Y));
			bool flag2 = this.IsControlPressed(args.Event);
			if (!flag2)
			{
				foreach (PlistInfoItem plistInfoItem in this.Model.SelectedItems)
				{
					if (flag && this.ConvertItemRect(plistInfoItem).Contains(this.pressedPoint))
					{
						foreach (PlistInfoItem plistInfoItem2 in this.Model.SelectedItems)
						{
							plistInfoItem2.Select = true;
						}
						return;
					}
					plistInfoItem.Select = false;
				}
				this.Model.SelectedItems.Clear();
			}
			List<PlistInfoItem> items = this.Model.Items;
			foreach (PlistInfoItem plistInfoItem3 in items)
			{
				if (this.ConvertItemRect(plistInfoItem3).Contains(this.pressedPoint))
				{
					this.clickedOnItem = true;
					if (flag2)
					{
						if (!plistInfoItem3.Select)
						{
							plistInfoItem3.Select = true;
							this.Model.SelectedItems.Add(plistInfoItem3);
						}
						else
						{
							plistInfoItem3.Select = false;
							this.Model.SelectedItems.Remove(plistInfoItem3);
						}
						this.pressed = false;
						break;
					}
					plistInfoItem3.Select = true;
					this.Model.SelectedItems.Add(plistInfoItem3);
					break;
				}
			}
			base.QueueDraw();
		}

		public void ShowToolTip(int x, int y)
		{
			if (this.motionNodifiedItemRect.Contains(x, y))
			{
				return;
			}
			base.TooltipText = null;
			List<PlistInfoItem> items = this.Model.Items;
			if (items == null)
			{
				return;
			}
			try
			{
				foreach (PlistInfoItem plistInfoItem in items)
				{
					Gdk.Rectangle rectangle = this.ConvertItemRect(plistInfoItem);
					if (rectangle.Contains(x, y))
					{
						string tipText = plistInfoItem.RelativeFile;
						GLib.Timeout.Add(50U, delegate
						{
							this.TooltipText = tipText;
							return false;
						});
						this.motionNodifiedItemRect = rectangle;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				LogConfig.OutputWithoutTip.Error(ex.Message);
			}
		}

		internal void OnMotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			Gdk.Point point = new Gdk.Point((int)args.Event.X, (int)args.Event.Y);
			if (!this.pressed)
			{
				this.ShowToolTip((int)args.Event.X, (int)args.Event.Y);
				return;
			}
			base.QueueDraw();
			if (this.spacePressed)
			{
				this.Position.X = this.Position.X + (point.X - this.lastPoint.X);
				this.Position.Y = this.Position.Y + (point.Y - this.lastPoint.Y);
			}
			else if (!this.clickedOnItem)
			{
				int num = Math.Min(this.pressedPoint.X, point.X);
				int num2 = Math.Min(this.pressedPoint.Y, point.Y);
				int num3 = Math.Max(this.pressedPoint.X, point.X);
				int num4 = Math.Max(this.pressedPoint.Y, point.Y);
				this.selectedRect.X = num;
				this.selectedRect.Y = num2;
				this.selectedRect.Width = num3 - num;
				this.selectedRect.Height = num4 - num2;
				this.Model.SelectedItems.Clear();
				List<PlistInfoItem> items = this.Model.Items;
				foreach (PlistInfoItem plistInfoItem in items)
				{
					if (this.ConvertItemRect(plistInfoItem).IntersectsWith(this.selectedRect))
					{
						plistInfoItem.Select = true;
						this.Model.SelectedItems.Add(plistInfoItem);
					}
					else
					{
						plistInfoItem.Select = false;
					}
				}
			}
			this.lastPoint = point;
		}

		internal void OnButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			this.selectedRect.Width = (this.selectedRect.Height = 0);
			this.pressed = false;
			base.QueueDraw();
		}

		private Gdk.Rectangle ConvertItemRect(PlistInfoItem item)
		{
			Gdk.Rectangle renderRect = item.RenderRect;
			return new Gdk.Rectangle(renderRect.X + this.Position.X, renderRect.Y + this.Position.Y, renderRect.Width, renderRect.Height);
		}

		internal void OnKeyPressEvent(object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.space)
			{
				this.spacePressed = true;
			}
		}

		internal void OnKeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.space)
			{
				this.spacePressed = false;
			}
		}

		private const double colordoubleper = 0.0039215686274;

		private double scale;

		private Gdk.Color bgColor;

		public Gdk.Point Position;

		private PlistInfoModel Model;

		private bool pressed;

		private bool clickedOnItem;

		private Gdk.Point pressedPoint;

		private Gdk.Point lastPoint;

		private Gdk.Rectangle selectedRect;

		private Gdk.Rectangle motionNodifiedItemRect = default(Gdk.Rectangle);

		private bool spacePressed;
	}
}
