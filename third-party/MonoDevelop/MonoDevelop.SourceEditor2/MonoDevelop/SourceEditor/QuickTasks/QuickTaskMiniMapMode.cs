using System;
using Cairo;
using GLib;
using Gdk;
using Gtk;
using Mono.TextEditor;
using MonoDevelop.Core;

namespace MonoDevelop.SourceEditor.QuickTasks
{
	public class QuickTaskMiniMapMode : HBox
	{
		public class Minimpap : QuickTaskOverviewMode
		{
			private class BgBufferUpdate
			{
				private int maxLine;

				private double sx;

				private double sy;

				private uint handler;

				private Context cr;

				private Minimpap mode;

				private int curLine = 1;

				public BgBufferUpdate(Minimpap mode)
				{
					this.mode = mode;
					cr = CairoHelper.Create(mode.backgroundBuffer);
					cr.LineWidth = 1.0;
					int width = mode.backgroundBuffer.ClipRegion.Clipbox.Width;
					int height = mode.backgroundBuffer.ClipRegion.Clipbox.Height;
					cr.Rectangle(0.0, 0.0, width, height);
					if (mode.TextEditor.ColorStyle != null)
					{
						cr.SetSourceColor(mode.TextEditor.ColorStyle.PlainText.Background);
					}
					cr.Fill();
					maxLine = mode.TextEditor.GetTextEditorData().VisibleLineCount;
					sx = (double)width / (double)mode.TextEditor.Allocation.Width;
					sy = Math.Min(1.0, 2.0 * (double)maxLine / mode.TextEditor.GetTextEditorData().TotalHeight);
					cr.Scale(sx, sy);
					handler = Idle.Add(BgBufferUpdater);
				}

				public void RemoveHandler()
				{
					if (cr != null)
					{
						Source.Remove(handler);
						handler = 0u;
						((IDisposable)cr).Dispose();
						cr = null;
						mode.curUpdate = null;
					}
				}

				private bool BgBufferUpdater()
				{
					if (mode.TextEditor.Document == null || handler == 0)
					{
						return false;
					}
					try
					{
						for (int i = 0; i < 25; i++)
						{
							if (curLine >= maxLine)
							{
								break;
							}
							int lineNumber = mode.TextEditor.GetTextEditorData().VisualToLogicalLine(curLine);
							DocumentLine line = mode.TextEditor.GetLine(lineNumber);
							if (line != null)
							{
								TextViewMargin.LayoutWrapper layout = mode.TextEditor.TextViewMargin.GetLayout(line);
								cr.MoveTo(0.0, (double)(curLine - 1) * mode.TextEditor.LineHeight);
								cr.ShowLayout(layout.Layout);
								if (layout.IsUncached)
								{
									layout.Dispose();
								}
							}
							curLine++;
						}
						if (curLine >= maxLine)
						{
							mode.SwapBuffer();
							((IDisposable)cr).Dispose();
							cr = null;
							mode.curUpdate = null;
							mode.QueueDraw();
							return false;
						}
					}
					catch (Exception ex)
					{
						LoggingService.LogError("Error in background buffer drawer.", ex);
						return false;
					}
					return true;
				}
			}

			private const double lineHeight = 2.0;

			private Pixmap backgroundPixbuf;

			private Pixmap backgroundBuffer;

			private uint redrawTimeout;

			private TextDocument doc;

			private BgBufferUpdate curUpdate;

			private int curWidth = -1;

			private int curHeight = -1;

			public Minimpap(QuickTaskStrip parent)
				: base(parent)
			{
				doc = parent.TextEditor.Document;
				doc.TextReplaced += TextReplaced;
				doc.Folded += HandleFolded;
			}

			private void HandleFolded(object sender, FoldSegmentEventArgs e)
			{
				RequestRedraw();
			}

			private void TextReplaced(object sender, DocumentChangeEventArgs args)
			{
				RequestRedraw();
			}

			public void RemoveRedrawTimer()
			{
				if (redrawTimeout != 0)
				{
					Source.Remove(redrawTimeout);
					redrawTimeout = 0u;
				}
			}

			private void RequestRedraw()
			{
				RemoveRedrawTimer();
				redrawTimeout = GLib.Timeout.Add(450u, delegate
				{
					if (curUpdate != null)
					{
						curUpdate.RemoveHandler();
						curUpdate = null;
					}
					if (backgroundPixbuf != null)
					{
						curUpdate = new BgBufferUpdate(this);
					}
					redrawTimeout = 0u;
					return false;
				});
			}

			protected override void DrawBar(Context cr)
			{
			}

			protected override void MovePosition(double y)
			{
				int num = (int)(2.0 * (double)base.TextEditor.GetTextEditorData().VisibleLineCount);
				double val = vadjustment.Upper * (Math.Min((double)GetBufferYOffset() + y, num) / (double)num) - vadjustment.PageSize / 2.0;
				val = Math.Max(vadjustment.Lower, Math.Min(val, vadjustment.Upper - vadjustment.PageSize));
				vadjustment.Value = val;
			}

			protected override void OnSizeRequested(ref Requisition requisition)
			{
				base.OnSizeRequested(ref requisition);
				requisition.Width = 150;
			}

			private void DestroyBgBuffer()
			{
				if (curUpdate != null)
				{
					curUpdate.RemoveHandler();
				}
				if (backgroundPixbuf != null)
				{
					backgroundPixbuf.Dispose();
					backgroundBuffer.Dispose();
					backgroundPixbuf = (backgroundBuffer = null);
					curWidth = (curHeight = -1);
				}
			}

			protected override void OnDestroyed()
			{
				base.OnDestroyed();
				doc.Folded -= HandleFolded;
				doc.TextReplaced -= TextReplaced;
				RemoveRedrawTimer();
				DestroyBgBuffer();
			}

			protected override void OnSizeAllocated(Gdk.Rectangle allocation)
			{
				base.OnSizeAllocated(allocation);
				if (allocation.Width > 1 && (allocation.Width != curWidth || allocation.Height != curHeight))
				{
					CreateBgBuffer();
				}
			}

			protected override void OnMapped()
			{
				if (backgroundPixbuf == null && base.Allocation.Width > 1)
				{
					CreateBgBuffer();
				}
				base.OnMapped();
			}

			protected override void OnUnmapped()
			{
				DestroyBgBuffer();
				base.OnUnmapped();
			}

			private void SwapBuffer()
			{
				Pixmap pixmap = backgroundPixbuf;
				backgroundPixbuf = backgroundBuffer;
				backgroundBuffer = pixmap;
			}

			private void CreateBgBuffer()
			{
				DestroyBgBuffer();
				curWidth = base.Allocation.Width;
				curHeight = Math.Max(base.Allocation.Height, (int)(2.0 * (double)base.TextEditor.GetTextEditorData().VisibleLineCount));
				if (base.GdkWindow == null || curWidth < 1 || curHeight < 1)
				{
					return;
				}
				backgroundPixbuf = new Pixmap(base.GdkWindow, curWidth, curHeight);
				backgroundBuffer = new Pixmap(base.GdkWindow, curWidth, curHeight);
				if (base.TextEditor.ColorStyle != null)
				{
					using (Context context = CairoHelper.Create(backgroundPixbuf))
					{
						context.Rectangle(0.0, 0.0, curWidth, curHeight);
						context.SetSourceColor(base.TextEditor.ColorStyle.PlainText.Background);
						context.Fill();
					}
				}
				curUpdate = new BgBufferUpdate(this);
			}

			private int GetBufferYOffset()
			{
				int num = backgroundPixbuf.ClipRegion.Clipbox.Height - base.Allocation.Height;
				if (num < 0)
				{
					return 0;
				}
				return Math.Max(0, (int)((double)num * vadjustment.Value / (vadjustment.Upper - vadjustment.Lower - vadjustment.PageSize)));
			}

			protected override bool OnExposeEvent(EventExpose e)
			{
				if (base.TextEditor == null)
				{
					return true;
				}
				using (Context context = CairoHelper.Create(e.Window))
				{
					context.LineWidth = 1.0;
					if (backgroundPixbuf != null)
					{
						e.Window.DrawDrawable(base.Style.BlackGC, backgroundPixbuf, 0, GetBufferYOffset(), 0, 0, base.Allocation.Width, base.Allocation.Height);
					}
					else
					{
						context.Rectangle(0.0, 0.0, base.Allocation.Width, base.Allocation.Height);
						if (base.TextEditor.ColorStyle != null)
						{
							context.SetSourceColor(base.TextEditor.ColorStyle.PlainText.Background);
						}
						context.Fill();
					}
					if (backgroundPixbuf != null)
					{
						int bufferYOffset = GetBufferYOffset();
						int line = base.TextEditor.YToLine(vadjustment.Value);
						double num = (double)base.TextEditor.LogicalToVisualLocation(line, 1).Line * 2.0;
						context.Rectangle(0.0, num - (double)bufferYOffset, base.Allocation.Width, 2.0 * vadjustment.PageSize / base.TextEditor.LineHeight);
						Cairo.Color color = (HslColor)base.Style.Dark(base.State);
						color.A = 0.2;
						context.SetSourceColor(color);
						context.Fill();
					}
					DrawLeftBorder(context);
				}
				return true;
			}
		}

		private QuickTaskOverviewMode rightMap;

		public QuickTaskMiniMapMode(QuickTaskStrip parent)
		{
			Minimpap child = new Minimpap(parent);
			PackStart(child, expand: true, fill: true, 0u);
			rightMap = new QuickTaskOverviewMode(parent);
			PackStart(rightMap, expand: true, fill: true, 0u);
		}
	}
}
