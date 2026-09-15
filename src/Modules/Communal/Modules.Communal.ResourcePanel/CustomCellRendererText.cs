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
	internal class CustomCellRendererText : CellRendererText
	{
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

		public CustomCellRendererText(ResourceTreeView parent)
		{
			this.parent = parent;
		}

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

		private Xwt.Drawing.Image GetImage()
		{
			if (this.nodeInfo == null)
			{
				return CustomCellRendererText.NullImage;
			}
			return (!base.IsExpanded) ? this.nodeInfo.IconInfo.ExpandIcon : this.nodeInfo.IconInfo.UnExpandIcon;
		}

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

		private const int StatusIconSpacing = 4;

		private double zoom;

		private Pango.Layout layout;

		private FontDescription scaledFont;

		private FontDescription customFont;

		private bool bound;

		private ResourceTreeView parent;

		private string markup;

		private NodeInfo nodeInfo;

		public static readonly Xwt.Drawing.Image NullImage = ImageService.GetIcon("md-empty");
	}
}
