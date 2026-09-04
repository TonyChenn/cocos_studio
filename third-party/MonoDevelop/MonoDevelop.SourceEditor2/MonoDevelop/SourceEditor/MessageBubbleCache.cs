using System;
using System.Collections.Generic;
using Cairo;
using GLib;
using Gdk;
using Gtk;
using Mono.TextEditor;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Fonts;
using Pango;
using Xwt.Drawing;

namespace MonoDevelop.SourceEditor
{
	internal class MessageBubbleCache : IDisposable
	{
		private class MessageBubblePopoverWindow : PopoverWindow
		{
			private const int verticalTextBorder = 10;

			private const int verticalTextSpace = 7;

			private const int textBorder = 12;

			private const int iconTextSpacing = 8;

			private readonly MessageBubbleCache cache;

			private readonly MessageBubbleTextMarker marker;

			private readonly int maxTextWidth = (int)(260.0 * Pango.Scale.PangoScale);

			public MessageBubblePopoverWindow(MessageBubbleCache cache, MessageBubbleTextMarker marker)
			{
				this.cache = cache;
				this.marker = marker;
				base.ShowArrow = true;
				base.Theme.ArrowLength = 7;
				base.TransientFor = IdeApp.Workbench.RootWindow;
			}

			protected override void OnSizeRequested(ref Requisition requisition)
			{
				base.OnSizeRequested(ref requisition);
				double num = 13 + (Platform.IsWindows ? 10 : 2);
				using (Pango.Layout layout = new Pango.Layout(base.PangoContext))
				{
					layout.FontDescription = cache.tooltipFontDescription;
					foreach (ErrorText error in marker.Errors)
					{
						if (marker.Layouts.Count == 1)
						{
							layout.Width = maxTextWidth;
						}
						layout.SetText(GetFirstLine(error));
						layout.GetPixelSize(out var width, out var height);
						if (marker.Layouts.Count > 1)
						{
							width += (int)cache.warningPixbuf.Width + 8;
						}
						requisition.Width = Math.Max(width + 24, requisition.Width);
						num += (double)(height + 7);
					}
				}
				requisition.Height = (int)num;
			}

			protected override bool OnEnterNotifyEvent(EventCrossing evnt)
			{
				cache.CancelLeaveDestroyTimeout();
				return base.OnEnterNotifyEvent(evnt);
			}

			protected override void OnDrawContent(EventExpose evnt, Cairo.Context g)
			{
				base.Theme.BorderColor = marker.TooltipColor.Color;
				g.Rectangle(0.0, 0.0, base.Allocation.Width, base.Allocation.Height);
				g.SetSourceColor(marker.TooltipColor.Color);
				g.Fill();
				using (Pango.Layout layout = new Pango.Layout(base.PangoContext))
				{
					layout.FontDescription = cache.tooltipFontDescription;
					double num = 10.0;
					bool flag = marker.Errors.Count > 1;
					foreach (ErrorText error in marker.Errors)
					{
						Xwt.Drawing.Image image = (error.IsError ? cache.errorPixbuf : cache.warningPixbuf);
						if (!flag)
						{
							layout.Width = maxTextWidth;
						}
						layout.SetText(GetFirstLine(error));
						layout.GetPixelSize(out var _, out var height);
						if (flag)
						{
							g.Save();
							g.Translate(12.0, num + 3.0 + Math.Max(0.0, ((double)height - image.Height) / 2.0));
							g.DrawImage(this, image, 0.0, 0.0);
							g.Restore();
						}
						g.Save();
						g.Translate(flag ? (20.0 + image.Width) : 12.0, num + 3.0);
						g.SetSourceColor(marker.TagColor.SecondColor);
						g.ShowLayout(layout);
						g.Restore();
						num += (double)(height + 7);
					}
				}
			}
		}

		internal class LayoutDescriptor
		{
			public Pango.Layout Layout { get; set; }

			public int Width { get; set; }

			public int Height { get; set; }

			public LayoutDescriptor(Pango.Layout layout, int width, int height)
			{
				Layout = layout;
				Width = width;
				Height = height;
			}
		}

		internal Xwt.Drawing.Image errorPixbuf;

		internal Xwt.Drawing.Image warningPixbuf;

		internal Dictionary<string, LayoutDescriptor> textWidthDictionary = new Dictionary<string, LayoutDescriptor>();

		internal Dictionary<DocumentLine, double> lineWidthDictionary = new Dictionary<DocumentLine, double>();

		internal TextEditor editor;

		internal FontDescription fontDescription;

		internal FontDescription tooltipFontDescription;

		internal FontDescription errorCountFontDescription;

		public MessageBubbleTextMarker CurrentSelectedTextMarker;

		private uint hoverTimeout;

		private MessageBubblePopoverWindow popoverWindow;

		private MessageBubbleTextMarker removedMarker;

		private uint leaveDestroyTimeout;

		public event EventHandler Changed;

		public MessageBubbleCache(TextEditor editor)
		{
			this.editor = editor;
			errorPixbuf = Xwt.Drawing.Image.FromResource("gutter-error-15.png");
			warningPixbuf = Xwt.Drawing.Image.FromResource("gutter-warning-15.png");
			editor.EditorOptionsChanged += HandleEditorEditorOptionsChanged;
			editor.TextArea.LeaveNotifyEvent += HandleLeaveNotifyEvent;
			editor.TextArea.MotionNotifyEvent += HandleMotionNotifyEvent;
			editor.TextArea.BeginHover += HandleBeginHover;
			editor.VAdjustment.ValueChanged += HandleValueChanged;
			editor.HAdjustment.ValueChanged += HandleValueChanged;
			fontDescription = FontService.GetFontDescription("Pad");
			tooltipFontDescription = FontService.GetFontDescription("Pad").CopyModified(null, Weight.Bold);
			errorCountFontDescription = FontService.GetFontDescription("Pad").CopyModified(null, Weight.Bold);
		}

		private void HandleValueChanged(object sender, EventArgs e)
		{
			DestroyPopoverWindow();
		}

		private void HandleMotionNotifyEvent(object o, MotionNotifyEventArgs args)
		{
			if (CurrentSelectedTextMarker == null)
			{
				DestroyPopoverWindow();
			}
		}

		private void CancelHoverTimeout()
		{
			if (hoverTimeout != 0)
			{
				Source.Remove(hoverTimeout);
				hoverTimeout = 0u;
			}
		}

		public void StartHover(MessageBubbleTextMarker marker, double bubbleX, double bubbleY, double bubbleWidth, bool isReduced)
		{
			CancelHoverTimeout();
			if (removedMarker == marker)
			{
				CurrentSelectedTextMarker = marker;
				return;
			}
			hoverTimeout = GLib.Timeout.Add(200u, delegate
			{
				CurrentSelectedTextMarker = marker;
				editor.QueueDraw();
				DestroyPopoverWindow();
				if (marker.Layouts == null || (marker.Layouts.Count < 2 && !isReduced))
				{
					return false;
				}
				popoverWindow = new MessageBubblePopoverWindow(this, marker);
				popoverWindow.ShowWindowShadow = false;
				popoverWindow.ShowPopup(editor, new Gdk.Rectangle((int)(bubbleX + editor.TextViewMargin.XOffset), (int)bubbleY, (int)bubbleWidth, (int)editor.LineHeight), PopupPosition.Top);
				return false;
			});
		}

		private void HandleBeginHover(object sender, EventArgs e)
		{
			CancelHoverTimeout();
			removedMarker = CurrentSelectedTextMarker;
			if (CurrentSelectedTextMarker != null)
			{
				CurrentSelectedTextMarker = null;
				editor.QueueDraw();
			}
		}

		private void CancelLeaveDestroyTimeout()
		{
			if (leaveDestroyTimeout != 0)
			{
				Source.Remove(leaveDestroyTimeout);
				leaveDestroyTimeout = 0u;
			}
		}

		private void HandleLeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
		{
			CancelLeaveDestroyTimeout();
			leaveDestroyTimeout = GLib.Timeout.Add(100u, delegate
			{
				DestroyPopoverWindow();
				leaveDestroyTimeout = 0u;
				return false;
			});
			CancelHoverTimeout();
			if (CurrentSelectedTextMarker != null)
			{
				CurrentSelectedTextMarker = null;
				editor.QueueDraw();
			}
		}

		public bool RemoveLine(DocumentLine line)
		{
			if (!lineWidthDictionary.ContainsKey(line))
			{
				return false;
			}
			lineWidthDictionary.Remove(line);
			return true;
		}

		internal void DestroyPopoverWindow()
		{
			if (popoverWindow != null)
			{
				popoverWindow.Destroy();
				popoverWindow = null;
			}
		}

		public void Dispose()
		{
			CancelLeaveDestroyTimeout();
			CancelHoverTimeout();
			DestroyPopoverWindow();
			editor.VAdjustment.ValueChanged -= HandleValueChanged;
			editor.HAdjustment.ValueChanged -= HandleValueChanged;
			editor.TextArea.BeginHover -= HandleBeginHover;
			editor.TextArea.LeaveNotifyEvent -= HandleLeaveNotifyEvent;
			editor.TextArea.MotionNotifyEvent -= HandleMotionNotifyEvent;
			editor.EditorOptionsChanged -= HandleEditorEditorOptionsChanged;
			if (textWidthDictionary == null)
			{
				return;
			}
			foreach (LayoutDescriptor value in textWidthDictionary.Values)
			{
				value.Layout.Dispose();
			}
		}

		private static string GetFirstLine(ErrorText errorText)
		{
			string text = errorText.ErrorMessage ?? "";
			int num = text.IndexOfAny(new char[2] { '\n', '\r' });
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
			return text;
		}

		internal LayoutDescriptor CreateLayoutDescriptor(ErrorText errorText)
		{
			if (!textWidthDictionary.TryGetValue(errorText.ErrorMessage, out var value))
			{
				Pango.Layout layout = new Pango.Layout(editor.PangoContext);
				layout.FontDescription = fontDescription;
				layout.SetText(GetFirstLine(errorText));
				layout.GetPixelSize(out var width, out var height);
				value = (textWidthDictionary[errorText.ErrorMessage] = new LayoutDescriptor(layout, width, height));
			}
			return value;
		}

		private void HandleEditorEditorOptionsChanged(object sender, EventArgs e)
		{
			lineWidthDictionary.Clear();
			OnChanged(EventArgs.Empty);
		}

		protected virtual void OnChanged(EventArgs e)
		{
			Changed?.Invoke(this, e);
		}
	}
}
