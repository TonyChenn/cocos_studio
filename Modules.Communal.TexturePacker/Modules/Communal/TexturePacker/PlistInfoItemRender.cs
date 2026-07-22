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
	// Token: 0x0200000A RID: 10
	public class PlistInfoItemRender : DrawingArea
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003530 File Offset: 0x00001730
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00003538 File Offset: 0x00001738
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

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600006D RID: 109 RVA: 0x000035A8 File Offset: 0x000017A8
		// (set) Token: 0x0600006E RID: 110 RVA: 0x000035B0 File Offset: 0x000017B0
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

		// Token: 0x0600006F RID: 111 RVA: 0x000035C0 File Offset: 0x000017C0
		public PlistInfoItemRender(PlistInfoModel model)
		{
			this.Model = model;
			base.WidthRequest = this.Model.SizeRequest.Width;
			base.HeightRequest = this.Model.SizeRequest.Height;
			this.Scale = 1.0;
			base.CanFocus = true;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003628 File Offset: 0x00001828
		protected override bool OnExposeEvent(EventExpose evnt)
		{
			this.DrawBackground();
			this.DrawSprites();
			this.DrawSelectAndMaxRect();
			return base.OnExposeEvent(evnt);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003644 File Offset: 0x00001844
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

		// Token: 0x06000072 RID: 114 RVA: 0x00003758 File Offset: 0x00001958
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

		// Token: 0x06000073 RID: 115 RVA: 0x000037B8 File Offset: 0x000019B8
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

		// Token: 0x06000074 RID: 116 RVA: 0x000038D0 File Offset: 0x00001AD0
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

		// Token: 0x06000075 RID: 117 RVA: 0x00003924 File Offset: 0x00001B24
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

		// Token: 0x06000076 RID: 118 RVA: 0x00003B4C File Offset: 0x00001D4C
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

		// Token: 0x06000077 RID: 119 RVA: 0x00003C28 File Offset: 0x00001E28
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

		// Token: 0x06000078 RID: 120 RVA: 0x00003E18 File Offset: 0x00002018
		internal void OnButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			this.selectedRect.Width = (this.selectedRect.Height = 0);
			this.pressed = false;
			base.QueueDraw();
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003E4C File Offset: 0x0000204C
		private Gdk.Rectangle ConvertItemRect(PlistInfoItem item)
		{
			Gdk.Rectangle renderRect = item.RenderRect;
			return new Gdk.Rectangle(renderRect.X + this.Position.X, renderRect.Y + this.Position.Y, renderRect.Width, renderRect.Height);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003E99 File Offset: 0x00002099
		internal void OnKeyPressEvent(object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.space)
			{
				this.spacePressed = true;
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003EB1 File Offset: 0x000020B1
		internal void OnKeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.space)
			{
				this.spacePressed = false;
			}
		}

		// Token: 0x04000025 RID: 37
		private const double colordoubleper = 0.0039215686274;

		// Token: 0x04000026 RID: 38
		private double scale;

		// Token: 0x04000027 RID: 39
		private Gdk.Color bgColor;

		// Token: 0x04000028 RID: 40
		public Gdk.Point Position;

		// Token: 0x04000029 RID: 41
		private PlistInfoModel Model;

		// Token: 0x0400002A RID: 42
		private bool pressed;

		// Token: 0x0400002B RID: 43
		private bool clickedOnItem;

		// Token: 0x0400002C RID: 44
		private Gdk.Point pressedPoint;

		// Token: 0x0400002D RID: 45
		private Gdk.Point lastPoint;

		// Token: 0x0400002E RID: 46
		private Gdk.Rectangle selectedRect;

		// Token: 0x0400002F RID: 47
		private Gdk.Rectangle motionNodifiedItemRect = default(Gdk.Rectangle);

		// Token: 0x04000030 RID: 48
		private bool spacePressed;
	}
}
