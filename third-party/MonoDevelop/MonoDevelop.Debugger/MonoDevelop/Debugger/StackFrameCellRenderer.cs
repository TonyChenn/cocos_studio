using System;
using Cairo;
using GLib;
using Gdk;
using Gtk;
using Mono.Debugging.Client;
using Mono.TextEditor;
using MonoDevelop.Components;
using MonoDevelop.Ide.Fonts;
using Pango;

namespace MonoDevelop.Debugger
{
	internal class StackFrameCellRenderer : CellRenderer
	{
		private const int RoundedRectangleRadius = 2;

		private const int RoundedRectangleHeight = 14;

		private const int RoundedRectangleWidth = 28;

		private const int Padding = 6;

		private static readonly FontDescription LineNumberFont = FontService.MonospaceFont.CopyModified(0.9);

		public readonly Pango.Context Context;

		public ExceptionStackFrame Frame;

		public bool IsUserCode;

		public string Markup;

		private int MaxMarkupWidth
		{
			get
			{
				if (base.Width < 0)
				{
					return base.Width;
				}
				return base.Width - 52;
			}
		}

		public StackFrameCellRenderer(Pango.Context ctx)
		{
			Context = ctx;
		}

		public override void GetSize(Widget widget, ref Gdk.Rectangle cell_area, out int x_offset, out int y_offset, out int width, out int height)
		{
			using (Pango.Layout layout = new Pango.Layout(Context))
			{
				layout.Width = (int)((double)MaxMarkupWidth * Pango.Scale.PangoScale);
				layout.SetMarkup(GetMarkup(selected: false));
				layout.GetPixelExtents(out var _, out var logical_rect);
				width = 46 + logical_rect.Width + 6;
				height = 6 + Math.Max(14, logical_rect.Height) + 6;
				x_offset = 0;
				y_offset = 0;
			}
		}

		private void RenderLineNumberIcon(Widget widget, Cairo.Context cr, Gdk.Rectangle cell_area, int markupHeight, int yOffset)
		{
			if (Frame != null)
			{
				cr.Save();
				cr.Translate(cell_area.X + 6, cell_area.Y + 6 + yOffset);
				cr.Antialias = Antialias.Subpixel;
				cr.RoundedRectangle(0.0, 0.0, 28.0, 14.0, 2.0);
				cr.Clip();
				if (IsUserCode)
				{
					cr.SetSourceRGBA(0.9, 0.6, 0.87, 1.0);
				}
				else
				{
					cr.SetSourceRGBA(0.77, 0.77, 0.77, 1.0);
				}
				cr.RoundedRectangle(0.0, 0.0, 28.0, 14.0, 2.0);
				cr.Fill();
				cr.SetSourceRGBA(0.0, 0.0, 0.0, 0.11);
				cr.RoundedRectangle(0.0, 0.0, 28.0, 14.0, 2.0);
				cr.LineWidth = 2.0;
				cr.Stroke();
				int num = ((!string.IsNullOrEmpty(Frame.File)) ? Frame.Line : (-1));
				using (Pango.Layout layout = PangoUtil.CreateLayout(widget, (num != -1) ? num.ToString() : "???"))
				{
					layout.Alignment = Pango.Alignment.Left;
					layout.FontDescription = LineNumberFont;
					layout.GetPixelSize(out var width, out var height);
					double num2 = (double)(14 - height) / 2.0;
					double tx = (double)(28 - width) / 2.0;
					cr.Save();
					cr.SetSourceRGBA(0.0, 0.0, 0.0, 0.34);
					cr.Translate(tx, num2 + 1.0);
					cr.ShowLayout(layout);
					cr.Restore();
					cr.SetSourceRGBA(1.0, 1.0, 1.0, 1.0);
					cr.Translate(tx, num2);
					cr.ShowLayout(layout);
				}
				cr.Restore();
			}
		}

		private string GetMarkup(bool selected)
		{
			if (Markup != null)
			{
				return Markup;
			}
			string text = $"<b>{GLib.Markup.EscapeText(Frame.DisplayText)}</b>";
			if (selected)
			{
				text = "<span foreground='#FFFFFF'>" + text + "</span>";
			}
			if (!string.IsNullOrEmpty(Frame.File))
			{
				text += string.Format("\n<span size='smaller' foreground='{0}'>{1}", selected ? "#FFFFFF" : "#777777", GLib.Markup.EscapeText(Frame.File));
				if (Frame.Line > 0)
				{
					text = text + ":" + Frame.Line;
					if (Frame.Column > 0)
					{
						text = text + "," + Frame.Column;
					}
				}
				text += "</span>";
			}
			return text;
		}

		protected override void Render(Drawable window, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
		{
			using (Cairo.Context context = Gdk.CairoHelper.Create(window))
			{
				using (Pango.Layout layout = new Pango.Layout(Context))
				{
					layout.Width = (int)((double)MaxMarkupWidth * Pango.Scale.PangoScale);
					layout.SetMarkup(GetMarkup((flags & CellRendererState.Selected) != 0));
					layout.GetPixelExtents(out var ink_rect, out var logical_rect);
					RenderLineNumberIcon(widget, context, cell_area, logical_rect.Height, ink_rect.Y);
					context.Rectangle(expose_area.X, expose_area.Y, expose_area.Width, expose_area.Height);
					context.Clip();
					context.Translate(cell_area.X + 6 + 28 + 6 + 6, cell_area.Y + 6);
					context.ShowLayout(layout);
				}
			}
		}
	}
}
