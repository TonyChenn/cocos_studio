using System;
using Cairo;
using Gdk;
using GLib;
using Gtk;
using MonoDevelop.Components;
using MonoDevelop.Ide;
using Pango;
using Xwt.Drawing;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000004 RID: 4
	internal class CustomCellRendererText : CellRendererText
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002244 File Offset: 0x00000444
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000224C File Offset: 0x0000044C
		public FontDescription CustomFont
		{
			get
			{
				return this.customFont;
			}
			set
			{
				if (this.scaledFont != null)
				{
					this.scaledFont.Dispose();
					this.scaledFont = null;
				}
				this.customFont = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000226F File Offset: 0x0000046F
		// (set) Token: 0x0600000E RID: 14 RVA: 0x00002277 File Offset: 0x00000477
		[Property("node-info")]
		public NodeInfo NodeInfo
		{
			get
			{
				return this.nodeInfo;
			}
			set
			{
				this.nodeInfo = value;
				if (value != null)
				{
					this.TextMarkup = value.Name;
				}
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000022A0 File Offset: 0x000004A0
		// (set) Token: 0x06000011 RID: 17 RVA: 0x000022A8 File Offset: 0x000004A8
		[Property("text-markup")]
		public string TextMarkup
		{
			get
			{
				return this.markup;
			}
			set
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					value = value.Replace("&", "&amp;");
				}
				base.Markup = (this.markup = value);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022DF File Offset: 0x000004DF
		public CustomCellRendererText(ResourceTreeView parent)
		{
			this.parent = parent;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000022F0 File Offset: 0x000004F0
		protected override void Render(Drawable window, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
		{
			if (this.nodeInfo == null)
			{
				base.Render(window, widget, background_area, cell_area, expose_area, flags);
			}
			StateType stateType = this.GetStateType(widget, flags);
			Xwt.Drawing.Image image = this.GetImage();
			double width = image.Width;
			this.SetLayout(widget);
			CellRenderHelper.Render(this.nodeInfo.DataItem, window, widget, background_area, cell_area, expose_area, flags);
			int num;
			int num2;
			this.layout.GetPixelSize(out num, out num2);
			num = ((num == 0) ? 30 : num);
			num2 = ((num2 == 0) ? 16 : num2);
			int x = cell_area.X + (int)base.Xpad + (int)width;
			int y = cell_area.Y + (cell_area.Height - num2) / 2;
			window.DrawLayout(widget.Style.TextGC(stateType), x, y, this.layout);
			this.DrawableIcon(window, widget, cell_area, image, 0);
			Xwt.Drawing.Image statusIconInternal = this.nodeInfo.IconInfo.StatusIconInternal;
			bool flag = this.nodeInfo.IconInfo.StatusIconInternal != CellRendererImage.NullImage && statusIconInternal != null;
			if (flag)
			{
				double num3 = (double)(num + 4) + statusIconInternal.Width;
				this.DrawableIcon(window, widget, cell_area, statusIconInternal, (int)num3);
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002418 File Offset: 0x00000618
		private void DrawableIcon(Drawable window, Widget widget, Gdk.Rectangle cell_area, Xwt.Drawing.Image img, int offSetX = 0)
		{
			if (img == null)
			{
				return;
			}
			using (Cairo.Context context = Gdk.CairoHelper.Create(window))
			{
				context.DrawImage(widget, img, (double)(cell_area.Left + offSetX), (double)cell_area.Top);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000246C File Offset: 0x0000066C
		private void SetLayout(Widget widget)
		{
			if (this.scaledFont == null)
			{
				if (this.scaledFont != null)
				{
					this.scaledFont.Dispose();
				}
				this.scaledFont = (this.customFont ?? this.parent.Style.FontDesc).Copy();
				this.scaledFont.Size = (int)((double)this.customFont.Size * this.Zoom);
				if (this.layout != null)
				{
					this.layout.FontDescription = this.scaledFont;
				}
			}
			if (this.layout == null || this.layout.Context != widget.PangoContext)
			{
				if (this.layout != null)
				{
					this.layout.Dispose();
				}
				this.layout = new Pango.Layout(widget.PangoContext);
				this.layout.FontDescription = this.scaledFont;
			}
			this.layout.SetMarkup(this.TextMarkup);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002554 File Offset: 0x00000754
		private StateType GetStateType(Widget widget, CellRendererState flags)
		{
			StateType result = StateType.Normal;
			if ((flags & CellRendererState.Prelit) != (CellRendererState)0)
			{
				result = StateType.Prelight;
			}
			if ((flags & CellRendererState.Focused) != (CellRendererState)0)
			{
				result = StateType.Normal;
			}
			if ((flags & CellRendererState.Insensitive) != (CellRendererState)0)
			{
				result = StateType.Insensitive;
			}
			if ((flags & CellRendererState.Selected) != (CellRendererState)0)
			{
				result = (widget.HasFocus ? StateType.Selected : StateType.Active);
			}
			return result;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000258C File Offset: 0x0000078C
		private Xwt.Drawing.Image GetImage()
		{
			if (this.nodeInfo == null)
			{
				return CustomCellRendererText.NullImage;
			}
			return (!base.IsExpanded) ? this.nodeInfo.IconInfo.ExpandIcon : this.nodeInfo.IconInfo.UnExpandIcon;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000025D3 File Offset: 0x000007D3
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000025DB File Offset: 0x000007DB
		public double Zoom
		{
			get
			{
				return this.zoom;
			}
			set
			{
				if (this.scaledFont != null)
				{
					this.scaledFont.Dispose();
					this.scaledFont = null;
				}
				this.zoom = value;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000025FE File Offset: 0x000007FE
		protected override void OnDestroyed()
		{
			base.OnDestroyed();
			if (this.scaledFont != null)
			{
				this.scaledFont.Dispose();
			}
			if (this.layout != null)
			{
				this.layout.Dispose();
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000262C File Offset: 0x0000082C
		public Gdk.Rectangle GetStatusIconArea(Widget widget, Gdk.Rectangle cell_area)
		{
			Xwt.Drawing.Image statusIconInternal = this.nodeInfo.IconInfo.StatusIconInternal;
			this.SetupLayout(widget);
			int num;
			int num2;
			this.layout.GetPixelSize(out num, out num2);
			int num3 = cell_area.X + (int)base.Xpad;
			double num4 = (double)(num3 + num + 4) + statusIconInternal.Width;
			return new Gdk.Rectangle((int)num4, cell_area.Y, (int)statusIconInternal.Width, cell_area.Height);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000269C File Offset: 0x0000089C
		private void SetupLayout(Widget widget)
		{
			if (this.scaledFont == null)
			{
				if (this.scaledFont != null)
				{
					this.scaledFont.Dispose();
				}
				this.scaledFont = (this.customFont ?? this.parent.Style.FontDesc).Copy();
				this.scaledFont.Size = this.customFont.Size;
				if (this.layout != null)
				{
					this.layout.FontDescription = this.scaledFont;
				}
			}
			if (this.layout == null || this.layout.Context != widget.PangoContext)
			{
				if (this.layout != null)
				{
					this.layout.Dispose();
				}
				this.layout = new Pango.Layout(widget.PangoContext);
				this.layout.FontDescription = this.scaledFont;
			}
			this.layout.SetMarkup(this.TextMarkup);
		}

		// Token: 0x04000007 RID: 7
		private const int StatusIconSpacing = 4;

		// Token: 0x04000008 RID: 8
		private double zoom;

		// Token: 0x04000009 RID: 9
		private Pango.Layout layout;

		// Token: 0x0400000A RID: 10
		private FontDescription scaledFont;

		// Token: 0x0400000B RID: 11
		private FontDescription customFont;

		// Token: 0x0400000C RID: 12
		private bool bound;

		// Token: 0x0400000D RID: 13
		private ResourceTreeView parent;

		// Token: 0x0400000E RID: 14
		private string markup;

		// Token: 0x0400000F RID: 15
		private NodeInfo nodeInfo;

		// Token: 0x04000010 RID: 16
		public static readonly Xwt.Drawing.Image NullImage = ImageService.GetIcon("md-empty");
	}
}
