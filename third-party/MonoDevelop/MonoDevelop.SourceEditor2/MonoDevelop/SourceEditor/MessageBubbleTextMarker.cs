using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cairo;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;
using MonoDevelop.Components;
using MonoDevelop.Debugger;
using MonoDevelop.Ide.Tasks;
using Pango;

namespace MonoDevelop.SourceEditor
{
	internal class MessageBubbleTextMarker : MarginMarker, IDisposable, IActionTextLineMarker
	{
		internal const int border = 4;

		private const int LIGHT = 0;

		private const int DARK = 1;

		private const int LINE = 2;

		private const int TOP = 0;

		private const int BOTTOM = 1;

		private readonly MessageBubbleCache cache;

		private List<ErrorText> errors = new List<ErrorText>();

		private Task task;

		private DocumentLine lineSegment;

		internal double lastHeight;

		private string initialText;

		private bool isError;

		private static System.Text.RegularExpressions.Regex mcsErrorFormat = new System.Text.RegularExpressions.Regex("(.+)\\(CS\\d+\\)\\Z");

		internal Layout errorCountLayout;

		private List<MessageBubbleCache.LayoutDescriptor> layouts;

		private bool ShowIconsInBubble;

		private double bubbleDrawX;

		private double bubbleDrawY;

		private double bubbleWidth;

		private bool bubbleIsReduced;

		private TextEditor editor => cache.editor;

		public override bool IsVisible
		{
			get
			{
				return !task.Completed;
			}
			set
			{
				task.Completed = !value;
			}
		}

		public bool UseVirtualLines { get; set; }

		internal IList<ErrorText> Errors => errors;

		public override TextLineMarkerFlags Flags
		{
			get
			{
				if (lineSegment != null && lineSegment.Markers.Any((TextLineMarker m) => m is DebugTextMarker))
				{
					return TextLineMarkerFlags.None;
				}
				return TextLineMarkerFlags.DrawsSelection;
			}
		}

		internal AmbientColor MarkerColor
		{
			get
			{
				if (!isError)
				{
					return editor.ColorStyle.MessageBubbleWarningMarker;
				}
				return editor.ColorStyle.MessageBubbleErrorMarker;
			}
		}

		internal AmbientColor TagColor
		{
			get
			{
				if (!isError)
				{
					return editor.ColorStyle.MessageBubbleWarningTag;
				}
				return editor.ColorStyle.MessageBubbleErrorTag;
			}
		}

		internal AmbientColor TooltipColor
		{
			get
			{
				if (!isError)
				{
					return editor.ColorStyle.MessageBubbleWarningTooltip;
				}
				return editor.ColorStyle.MessageBubbleErrorTooltip;
			}
		}

		internal AmbientColor LineColor
		{
			get
			{
				if (!isError)
				{
					return editor.ColorStyle.MessageBubbleWarningLine;
				}
				return editor.ColorStyle.MessageBubbleErrorLine;
			}
		}

		internal AmbientColor CounterColor
		{
			get
			{
				if (!isError)
				{
					return editor.ColorStyle.MessageBubbleWarningCounter;
				}
				return editor.ColorStyle.MessageBubbleErrorCounter;
			}
		}

		internal AmbientColor IconMarginColor
		{
			get
			{
				if (!isError)
				{
					return editor.ColorStyle.MessageBubbleWarningIconMargin;
				}
				return editor.ColorStyle.MessageBubbleErrorIconMargin;
			}
		}

		internal IList<MessageBubbleCache.LayoutDescriptor> Layouts => layouts;

		internal int LayoutWidth
		{
			get
			{
				if (layouts == null)
				{
					return 0;
				}
				return layouts[0].Width;
			}
		}

		public double GetLineHeight(TextEditor editor)
		{
			return editor.LineHeight;
		}

		public void SetPrimaryError(string text)
		{
			EnsureLayoutCreated(editor);
			System.Text.RegularExpressions.Match match = mcsErrorFormat.Match(text);
			if (match.Success)
			{
				text = match.Groups[1].Value;
			}
			int num = -1;
			for (int i = 0; i < errors.Count; i++)
			{
				if (errors[i].ErrorMessage == text)
				{
					num = i;
					break;
				}
			}
			if (num > 0)
			{
				ErrorText item = errors[num];
				errors.RemoveAt(num);
				errors.Insert(0, item);
				MessageBubbleCache.LayoutDescriptor item2 = layouts[num];
				layouts.RemoveAt(num);
				layouts.Insert(0, item2);
			}
		}

		internal MessageBubbleTextMarker(MessageBubbleCache cache, Task task, DocumentLine lineSegment, bool isError, string errorMessage)
		{
			if (cache == null)
			{
				throw new ArgumentNullException("cache");
			}
			this.cache = cache;
			this.task = task;
			IsVisible = true;
			this.lineSegment = lineSegment;
			initialText = editor.Document.GetTextAt(lineSegment);
			this.isError = isError;
			AddError(task, isError, errorMessage);
		}

		public void AddError(Task task, bool isError, string errorMessage)
		{
			System.Text.RegularExpressions.Match match = mcsErrorFormat.Match(errorMessage);
			if (match.Success)
			{
				errorMessage = match.Groups[1].Value;
			}
			errors.Add(new ErrorText(task, isError, errorMessage));
			DisposeLayout();
		}

		public void DisposeLayout()
		{
			layouts = null;
			if (errorCountLayout != null)
			{
				errorCountLayout.Dispose();
				errorCountLayout = null;
			}
		}

		public void Dispose()
		{
			DisposeLayout();
			cache.DestroyPopoverWindow();
		}

		private Cairo.Color BlendSelection(Cairo.Color color, bool selected)
		{
			if (!selected)
			{
				return color;
			}
			Cairo.Color background = editor.ColorStyle.SelectedText.Background;
			return new Cairo.Color(color.R * 0.1 + background.R * 0.9, color.G * 0.1 + background.G * 0.9, color.B * 0.1 + background.B * 0.9);
		}

		private Cairo.Color Highlight(Cairo.Color color, bool highlighted)
		{
			if (!highlighted)
			{
				return color;
			}
			Cairo.Color background = editor.ColorStyle.PlainText.Background;
			return new Cairo.Color(color.R * 0.7 + background.R * 0.30000000000000004, color.G * 0.7 + background.G * 0.30000000000000004, color.B * 0.7 + background.B * 0.30000000000000004);
		}

		private Cairo.Color GetLineColor(bool highlighted, bool selected)
		{
			return BlendSelection(Highlight(LineColor.Color, highlighted), selected);
		}

		private Cairo.Color GetMarkerColor(bool highlighted, bool selected)
		{
			return BlendSelection(Highlight(MarkerColor.Color, highlighted), selected);
		}

		private Cairo.Color GetLineColorBottom(bool highlighted, bool selected)
		{
			return BlendSelection(Highlight(LineColor.SecondColor, highlighted), selected);
		}

		private Cairo.Color GetLineColorBorder(bool highlighted, bool selected)
		{
			return BlendSelection(Highlight(LineColor.BorderColor, highlighted), selected);
		}

		internal void EnsureLayoutCreated(TextEditor editor)
		{
			if (layouts != null)
			{
				return;
			}
			layouts = new List<MessageBubbleCache.LayoutDescriptor>();
			foreach (ErrorText error in errors)
			{
				layouts.Add(cache.CreateLayoutDescriptor(error));
			}
			if (errorCountLayout == null && errors.Count > 1)
			{
				errorCountLayout = new Layout(editor.PangoContext);
				errorCountLayout.FontDescription = cache.errorCountFontDescription;
				errorCountLayout.SetText(errors.Count.ToString());
			}
		}

		private Tuple<int, int> GetErrorCountBounds(LineMetrics metrics)
		{
			EnsureLayoutCreated(editor);
			double num = editor.TextViewMargin.XOffset + metrics.TextRenderEndPosition;
			if ((errors.Count > 1 && errorCountLayout != null) || (double)editor.Allocation.Width < num + (double)layouts[0].Width)
			{
				int width = 0;
				int height = 0;
				if (errorCountLayout != null)
				{
					errorCountLayout.GetPixelSize(out width, out height);
				}
				else
				{
					width = 10;
				}
				return Tuple.Create(width + 10, height);
			}
			return Tuple.Create(0, 0);
		}

		private static void DrawRectangle(Cairo.Context g, double x, double y, double width, double height)
		{
			double x2 = x + width;
			double y2 = y + height;
			g.MoveTo(new PointD(x, y));
			g.LineTo(new PointD(x2, y));
			g.LineTo(new PointD(x2, y2));
			g.LineTo(new PointD(x, y2));
			g.LineTo(new PointD(x, y));
			g.ClosePath();
		}

		public bool MousePressed(TextEditor editor, MarginMouseEventArgs args)
		{
			return false;
		}

		public void MouseHover(TextEditor editor, MarginMouseEventArgs args, TextLineMarkerHoverResult result)
		{
			if (IsVisible && base.LineSegment != null && bubbleDrawX < args.X && args.X < bubbleDrawX + bubbleWidth)
			{
				editor.HideTooltip();
				result.Cursor = null;
				cache.StartHover(this, bubbleDrawX, bubbleDrawY, bubbleWidth, bubbleIsReduced);
			}
		}

		public override void Draw(TextEditor editor, Cairo.Context g, double y, LineMetrics metrics)
		{
		}

		public override void DrawAfterEol(TextEditor textEditor, Cairo.Context g, double y, EndOfLineMetrics metrics)
		{
			if (!IsVisible)
			{
				return;
			}
			EnsureLayoutCreated(editor);
			int width = 0;
			int height = 0;
			if (errorCountLayout != null)
			{
				errorCountLayout.GetPixelSize(out width, out height);
				width = Math.Max(15, Math.Max(width + 3, (int)(editor.LineHeight * 3.0 / 4.0)));
			}
			double num = metrics.TextRenderEndPosition;
			double num2 = (double)(LayoutWidth + width) + editor.LineHeight;
			Layout layout = layouts[0].Layout;
			bool flag = true;
			bool flag2 = false;
			bubbleIsReduced = flag;
			bool flag3 = width > 0 && errorCountLayout != null;
			double r = editor.LineHeight / 2.0 - 1.0;
			if (flag)
			{
				num2 = (double)editor.Allocation.Width - num;
				string text = layouts[0].Layout.Text;
				layout = new Layout(editor.PangoContext);
				layout.FontDescription = cache.fontDescription;
				double num3 = num2 - (double)width - editor.LineHeight + 4.0;
				double num4 = (double)Math.Max(25, width) * editor.Options.Zoom;
				if (num3 < num4)
				{
					flag2 = true;
					flag3 = false;
					num2 = num4;
					num = Math.Min(num, (double)editor.Allocation.Width - num2);
				}
				else
				{
					layout.Ellipsize = EllipsizeMode.End;
					layout.Width = (int)(num3 * Scale.PangoScale);
					layout.SetText(text);
					layout.GetPixelSize(out var width2, out var _);
					num2 = (double)(width2 + width) + editor.LineHeight - 2.0;
				}
			}
			bubbleDrawX = num - editor.TextViewMargin.XOffset;
			bubbleDrawY = y + 2.0;
			bubbleWidth = num2;
			double lineHeight = editor.LineHeight;
			g.RoundedRectangle(num, y, num2, lineHeight, r);
			g.SetSourceColor(TagColor.Color);
			g.Fill();
			if (flag3)
			{
				double num5 = lineHeight - 2.0;
				double num6 = num + num2 - (double)width - 1.0;
				double num7 = Math.Round(y + (lineHeight - num5) / 2.0);
				g.RoundedRectangle(num6, num7, width, num5, editor.LineHeight / 2.0 - 2.0);
				using (LinearGradient linearGradient = new LinearGradient(num6, num7, num6, num7 + num5))
				{
					linearGradient.AddColorStop(0.0, CounterColor.Color);
					linearGradient.AddColorStop(1.0, CounterColor.Color.AddLight(-0.1));
					g.SetSource(linearGradient);
					g.Fill();
				}
				g.Save();
				errorCountLayout.GetPixelSize(out var width3, out height);
				double tx = Math.Round(num6 + (double)((2 + width - width3) / 2));
				double ty = Math.Round(num7 + (-1.0 + num5 - (double)height) / 2.0);
				g.Translate(tx, ty);
				g.SetSourceColor(CounterColor.SecondColor);
				g.ShowLayout(errorCountLayout);
				g.Restore();
			}
			if (flag2)
			{
				double num8 = 2.0 * editor.Options.Zoom;
				double num9 = 1.0 * editor.Options.Zoom;
				num += 1.0 * editor.Options.Zoom + Math.Ceiling((bubbleWidth - 3.0 * (num8 * 2.0) - 2.0 * num9) / 2.0);
				for (int i = 0; i < 3; i++)
				{
					g.Arc(num, y + lineHeight / 2.0, num8, 0.0, Math.PI * 2.0);
					g.SetSourceColor(TagColor.SecondColor);
					g.Fill();
					num += num8 * 2.0 + num9;
				}
			}
			else
			{
				double tx2 = Math.Round(num + editor.LineHeight / 2.0);
				double ty2 = Math.Round(y + (editor.LineHeight - (double)layouts[0].Height) / 2.0) - 1.0;
				g.Save();
				g.Translate(tx2, ty2);
				g.SetSourceColor(TagColor.SecondColor);
				g.ShowLayout(layout);
				g.Restore();
			}
			if (flag)
			{
				layout.Dispose();
			}
		}

		public override bool CanDrawBackground(Margin margin)
		{
			if (!IsVisible)
			{
				return false;
			}
			if (!(margin is FoldMarkerMargin) && !(margin is GutterMargin) && !(margin is IconMargin))
			{
				return margin is ActionMargin;
			}
			return true;
		}

		public override bool CanDrawForeground(Margin margin)
		{
			if (!IsVisible)
			{
				return false;
			}
			return margin is IconMargin;
		}

		private void DrawIconMarginBackground(TextEditor ed, Cairo.Context cr, MarginDrawMetrics metrics)
		{
			cr.Rectangle(metrics.X, metrics.Y, metrics.Width, metrics.Height);
			cr.SetSourceColor(IconMarginColor.Color);
			cr.Fill();
			cr.MoveTo(metrics.Right - 0.5, metrics.Y);
			cr.LineTo(metrics.Right - 0.5, metrics.Bottom);
			cr.SetSourceColor(IconMarginColor.BorderColor);
			cr.Stroke();
			if (cache.CurrentSelectedTextMarker != null && cache.CurrentSelectedTextMarker != this)
			{
				cr.Rectangle(metrics.X, metrics.Y, metrics.Width, metrics.Height);
				cr.SetSourceRGBA(ed.ColorStyle.IndicatorMargin.Color.R, ed.ColorStyle.IndicatorMargin.Color.G, ed.ColorStyle.IndicatorMargin.Color.B, 0.5);
				cr.Fill();
			}
		}

		public override void DrawForeground(TextEditor editor, Cairo.Context cr, MarginDrawMetrics metrics)
		{
			double tx = Math.Round(metrics.X + (metrics.Width - cache.errorPixbuf.Width) / 2.0) - 1.0;
			double ty = Math.Floor(metrics.Y + (metrics.Height - cache.errorPixbuf.Height) / 2.0);
			cr.Save();
			cr.Translate(tx, ty);
			cr.DrawImage(editor, errors.Any((ErrorText e) => e.IsError) ? cache.errorPixbuf : cache.warningPixbuf, 0.0, 0.0);
			cr.Restore();
		}

		public override bool DrawBackground(TextEditor editor, Cairo.Context cr, MarginDrawMetrics metrics)
		{
			if (metrics.Margin is FoldMarkerMargin || metrics.Margin is GutterMargin || metrics.Margin is ActionMargin)
			{
				return DrawMarginBackground(editor, metrics.Margin, cr, metrics.Area, lineSegment, metrics.LineNumber, metrics.X, metrics.Y, metrics.Height);
			}
			if (metrics.Margin is IconMargin)
			{
				DrawIconMarginBackground(editor, cr, metrics);
				return true;
			}
			return false;
		}

		private bool DrawMarginBackground(TextEditor e, Margin margin, Cairo.Context cr, Cairo.Rectangle area, DocumentLine documentLine, long line, double x, double y, double lineHeight)
		{
			if (cache.CurrentSelectedTextMarker != null && cache.CurrentSelectedTextMarker != this)
			{
				return false;
			}
			cr.Rectangle(x, y, margin.Width, lineHeight);
			cr.SetSourceColor(LineColor.Color);
			cr.Fill();
			return true;
		}

		public override bool DrawBackground(TextEditor editor, Cairo.Context g, double y, LineMetrics metrics)
		{
			if (!IsVisible)
			{
				return false;
			}
			bool flag = cache.CurrentSelectedTextMarker != null && cache.CurrentSelectedTextMarker != this;
			if (metrics.LineSegment.Markers.Any((TextLineMarker m) => m is DebugTextMarker))
			{
				return false;
			}
			EnsureLayoutCreated(editor);
			double xOffset = editor.TextViewMargin.XOffset;
			int width = editor.Allocation.Width;
			bool flag2 = metrics.TextStartOffset <= editor.Caret.Offset && editor.Caret.Offset <= metrics.TextEndOffset;
			int item = GetErrorCountBounds(metrics).Item1;
			double val = (double)(width - LayoutWidth - 4) - (ShowIconsInBubble ? cache.errorPixbuf.Width : 0.0) - (double)item;
			double val2 = Math.Round(editor.TextViewMargin.XOffset + editor.LineHeight / 2.0);
			double num = Math.Max(val, val2);
			bool flag3 = editor.IsSomethingSelected && editor.SelectionMode != SelectionMode.Block && editor.SelectionRange.Contains(lineSegment.Offset + lineSegment.Length);
			bool highlighted = (editor.Document.GetTextAt(lineSegment) == initialText || 1 == 0) && flag2;
			if (!flag)
			{
				DrawRectangle(g, xOffset, y, width, editor.LineHeight);
				g.SetSourceColor(LineColor.Color);
				g.Fill();
				if (metrics.Layout.StartSet || metrics.SelectionStart == metrics.TextEndOffset)
				{
					double num3;
					double num2;
					if (metrics.SelectionStart != metrics.TextEndOffset)
					{
						num2 = (int)((double)metrics.Layout.Layout.IndexToPos(metrics.Layout.SelectionStartIndex).X / Scale.PangoScale);
						num3 = (int)((double)metrics.Layout.Layout.IndexToPos(metrics.Layout.SelectionEndIndex).X / Scale.PangoScale);
					}
					else
					{
						num2 = num;
						num3 = num2;
					}
					if (editor.MainSelection.SelectionMode == SelectionMode.Block && num2 == num3)
					{
						num3 = num2 + 2.0;
					}
					num2 += metrics.TextRenderStartPosition;
					num3 += metrics.TextRenderStartPosition;
					num2 = Math.Max(editor.TextViewMargin.XOffset, num2);
					if (flag3)
					{
						num3 = editor.Allocation.Width + (int)editor.HAdjustment.Value;
					}
					if (num2 < num3)
					{
						DrawRectangle(g, num2, y, num3 - num2, editor.LineHeight);
						g.SetSourceColor(GetLineColor(highlighted, selected: true));
						g.Fill();
					}
				}
				DrawErrorMarkers(editor, g, metrics, y);
			}
			double num4 = y + 0.5;
			double y2 = num4 + editor.LineHeight - 1.0;
			bool selected = flag3;
			double num5 = editor.TextViewMargin.XOffset + (double)editor.TextViewMargin.TextStartPosition + metrics.Layout.Width;
			if (num < num5)
			{
				num = num5;
			}
			if (editor.Options.ShowRuler)
			{
				double num6 = Math.Max(editor.TextViewMargin.XOffset, xOffset + editor.TextViewMargin.RulerX);
				if (num6 >= num)
				{
					g.MoveTo(new PointD(num6 + 0.5, num4));
					g.LineTo(new PointD(num6 + 0.5, y2));
					g.SetSourceColor(GetLineColorBorder(highlighted, selected));
					g.Stroke();
				}
			}
			return true;
		}

		private void DrawErrorMarkers(TextEditor editor, Cairo.Context g, LineMetrics metrics, double y)
		{
			uint curIndex = 0u;
			uint byteIndex = 0u;
			int offset = metrics.LineSegment.Offset;
			foreach (Task item in errors.Select((ErrorText t) => t.Task))
			{
				uint textIndex = (uint)Math.Min(Math.Max(0, item.Column - 1), metrics.Layout.LineChars.Length);
				int index_ = (int)metrics.Layout.TranslateToUTF8Index(textIndex, ref curIndex, ref byteIndex);
				Pango.Rectangle rectangle = metrics.Layout.Layout.IndexToPos(index_);
				int num = offset + item.Column - 1;
				g.SetSourceColor(GetMarkerColor(highlighted: false, metrics.SelectionStart <= num && num < metrics.SelectionEnd));
				g.MoveTo(metrics.TextRenderStartPosition + (double)editor.TextViewMargin.TextStartPosition + (double)rectangle.X / Scale.PangoScale, y + editor.LineHeight - 3.0);
				g.RelLineTo(3.0, 3.0);
				g.RelLineTo(-6.0, 0.0);
				g.ClosePath();
				g.Fill();
			}
		}
	}
}
