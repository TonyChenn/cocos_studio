using System;
using System.Collections.Generic;
using Cairo;
using GLib;
using Gdk;
using Gtk;
using ICSharpCode.NRefactory.Refactoring;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;
using Mono.TextEditor.Theatrics;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Debugger;
using MonoDevelop.Ide.FindInFiles;
using Xwt.Drawing;

namespace MonoDevelop.SourceEditor.QuickTasks
{
	public class QuickTaskOverviewMode : DrawingArea
	{
		private class PreviewPopup
		{
			private QuickTaskOverviewMode strip;

			private TextSegment segment;

			private int w;

			private int y;

			public PreviewPopup(QuickTaskOverviewMode strip, TextSegment segment, int w, int y)
			{
				this.strip = strip;
				this.segment = segment;
				this.w = w;
				this.y = y;
			}

			public bool Run()
			{
				strip.previewWindow = new CodeSegmentPreviewWindow(strip.TextEditor, hideCodeSegmentPreviewInformString: true, segment, w, -1, removeIndent: false);
				strip.previewWindow.WidthRequest = w;
				strip.previewWindow.Show();
				strip.PositionPreviewWindow(y);
				return false;
			}
		}

		private const uint FadeDuration = 90u;

		private const double barAlphaMax = 0.5;

		private const double barAlphaMin = 0.22;

		private static Xwt.Drawing.Image searchImage = Xwt.Drawing.Image.FromResource("issues-busy-16.png");

		private static Xwt.Drawing.Image okImage = Xwt.Drawing.Image.FromResource("issues-ok-16.png");

		private static Xwt.Drawing.Image warningImage = Xwt.Drawing.Image.FromResource("issues-warning-16.png");

		private static Xwt.Drawing.Image errorImage = Xwt.Drawing.Image.FromResource("issues-error-16.png");

		private static Xwt.Drawing.Image suggestionImage = Xwt.Drawing.Image.FromResource("issues-suggestion-16.png");

		private static readonly Cairo.Color win81Background = new Cairo.Color(0.9411764705882353, 0.9411764705882353, 0.9411764705882353);

		private static readonly Cairo.Color win81Slider = new Cairo.Color(41.0 / 51.0, 41.0 / 51.0, 41.0 / 51.0);

		private static readonly Cairo.Color win81SliderPrelight = new Cairo.Color(166.0 / 255.0, 166.0 / 255.0, 166.0 / 255.0);

		private static readonly Cairo.Color win81SliderActive = new Cairo.Color(32.0 / 85.0, 32.0 / 85.0, 32.0 / 85.0);

		private readonly int barPadding = (Platform.IsWindows ? 1 : 3);

		private readonly QuickTaskStrip parentStrip;

		protected readonly Adjustment vadjustment;

		private int caretLine = -1;

		internal CodeSegmentPreviewWindow previewWindow;

		private Stage<QuickTaskOverviewMode> fadeInStage = new Stage<QuickTaskOverviewMode>();

		private Stage<QuickTaskOverviewMode> fadeOutStage = new Stage<QuickTaskOverviewMode>();

		private uint previewPopupTimeout;

		private bool isPointerInside;

		private uint fadeTimeOutHandler;

		private double grabY = -1.0;

		private double grabCenter;

		private Dictionary<int, double> yPositionCache = new Dictionary<int, double>();

		private double barColorValue;

		public static Xwt.Drawing.Image SuggestionImage => suggestionImage;

		public static Xwt.Drawing.Image ErrorImage => errorImage;

		public static Xwt.Drawing.Image WarningImage => warningImage;

		public static Xwt.Drawing.Image OkImage => okImage;

		public TextEditor TextEditor { get; private set; }

		public IEnumerable<QuickTask> AllTasks => parentStrip.AllTasks;

		public IEnumerable<Usage> AllUsages => parentStrip.AllUsages;

		protected virtual double IndicatorHeight => Platform.IsWindows ? base.Allocation.Width : 14;

		public QuickTaskOverviewMode(QuickTaskStrip parent)
		{
			parentStrip = parent;
			base.Events |= EventMask.PointerMotionMask | EventMask.ButtonMotionMask | EventMask.ButtonPressMask | EventMask.ButtonReleaseMask | EventMask.EnterNotifyMask | EventMask.LeaveNotifyMask;
			vadjustment = parentStrip.VAdjustment;
			vadjustment.ValueChanged += RedrawOnVAdjustmentChange;
			vadjustment.Changed += RedrawOnVAdjustmentChange;
			parentStrip.TaskProviderUpdated += RedrawOnUpdate;
			TextEditor = parent.TextEditor;
			TextEditor.HighlightSearchPatternChanged += RedrawOnUpdate;
			TextEditor.TextViewMargin.SearchRegionsUpdated += RedrawOnUpdate;
			TextEditor.TextViewMargin.MainSearchResultChanged += RedrawOnUpdate;
			TextEditor.GetTextEditorData().HeightTree.LineUpdateFrom += HandleLineUpdateFrom;
			TextEditor.HighlightSearchPatternChanged += HandleHighlightSearchPatternChanged;
			base.HasTooltip = true;
			fadeInStage.ActorStep += delegate(Actor<QuickTaskOverviewMode> actor)
			{
				barColorValue = actor.Percent;
				return true;
			};
			fadeInStage.Iteration += delegate
			{
				QueueDraw();
			};
			fadeOutStage.ActorStep += delegate(Actor<QuickTaskOverviewMode> actor)
			{
				barColorValue = 1.0 - actor.Percent;
				return true;
			};
			fadeOutStage.Iteration += delegate
			{
				QueueDraw();
			};
			fadeInStage.UpdateFrequency = (fadeOutStage.UpdateFrequency = 10u);
		}

		private void HandleHighlightSearchPatternChanged(object sender, EventArgs e)
		{
			yPositionCache.Clear();
		}

		private void HandleLineUpdateFrom(object sender, HeightTree.HeightChangedEventArgs e)
		{
			yPositionCache.Clear();
		}

		private void CaretPositionChanged(object sender, EventArgs e)
		{
			int line = TextEditor.Caret.Line;
			if (caretLine != line)
			{
				caretLine = line;
				QueueDraw();
			}
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();
			CancelFadeInTimeout();
			RemovePreviewPopupTimeout();
			DestroyPreviewWindow();
			TextEditor.Caret.PositionChanged -= CaretPositionChanged;
			TextEditor.HighlightSearchPatternChanged -= HandleHighlightSearchPatternChanged;
			TextEditor.GetTextEditorData().HeightTree.LineUpdateFrom -= HandleLineUpdateFrom;
			TextEditor.HighlightSearchPatternChanged -= RedrawOnUpdate;
			TextEditor.TextViewMargin.SearchRegionsUpdated -= RedrawOnUpdate;
			TextEditor.TextViewMargin.MainSearchResultChanged -= RedrawOnUpdate;
			parentStrip.TaskProviderUpdated -= RedrawOnUpdate;
			vadjustment.ValueChanged -= RedrawOnVAdjustmentChange;
			vadjustment.Changed -= RedrawOnVAdjustmentChange;
		}

		private void RedrawOnUpdate(object sender, EventArgs e)
		{
			QueueDraw();
		}

		private void RedrawOnVAdjustmentChange(object sender, EventArgs e)
		{
			if (QuickTaskStrip.MergeScrollBarAndQuickTasks)
			{
				QueueDraw();
			}
		}

		private bool IsOverIndicator(double y)
		{
			return y < IndicatorHeight;
		}

		protected override bool OnMotionNotifyEvent(EventMotion evnt)
		{
			RemovePreviewPopupTimeout();
			if (IsInGrab())
			{
				double num = evnt.Y - grabY;
				MovePosition(grabCenter + num);
			}
			else
			{
				UpdatePrelightState(evnt.X, evnt.Y);
			}
			if ((evnt.State & (ModifierType.Button1Mask | ModifierType.Button2Mask | ModifierType.Button3Mask | ModifierType.Button4Mask | ModifierType.Button5Mask) & ModifierType.ShiftMask) == ModifierType.ShiftMask)
			{
				int num2 = YToLine(evnt.Y);
				num2 = Math.Max(1, num2 - 2);
				int lineNumber = Math.Min(TextEditor.LineCount, num2 + 5);
				DocumentLine line = TextEditor.GetLine(num2);
				DocumentLine line2 = TextEditor.GetLine(lineNumber);
				if (line == null || line2 == null)
				{
					return base.OnMotionNotifyEvent(evnt);
				}
				TextSegment segment = new TextSegment(line.Offset, line2.Offset + line2.Length - line.Offset);
				if (previewWindow != null)
				{
					previewWindow.SetSegment(segment, removeIndent: false);
					PositionPreviewWindow((int)evnt.Y);
				}
				else
				{
					PreviewPopup previewPopup = new PreviewPopup(this, segment, TextEditor.Allocation.Width * 4 / 7, (int)evnt.Y);
					previewPopupTimeout = GLib.Timeout.Add(450u, previewPopup.Run);
				}
			}
			else
			{
				RemovePreviewPopupTimeout();
				DestroyPreviewWindow();
			}
			return base.OnMotionNotifyEvent(evnt);
		}

		private bool IsInGrab()
		{
			return grabY >= 0.0;
		}

		private void UpdatePrelightState(double x, double y)
		{
			StateType state = StateType.Normal;
			if (IsInsideBar(x, y))
			{
				state = StateType.Prelight;
			}
			UpdateState(state);
		}

		private bool IsInsideBar(double x, double y)
		{
			GetBarDimensions(out var x2, out var y2, out var w, out var h);
			return x >= x2 && x <= x2 + w && y >= y2 && y <= y2 + h;
		}

		protected override bool OnQueryTooltip(int x, int y, bool keyboard_tooltip, Tooltip tooltip)
		{
			if (TextEditor.HighlightSearchPattern)
			{
				if (IsOverIndicator(y))
				{
					int searchResultMatchCount = TextEditor.TextViewMargin.SearchResultMatchCount;
					tooltip.Text = GettextCatalog.GetPluralString("{0} match", "{0} matches", searchResultMatchCount, searchResultMatchCount);
					return true;
				}
				return false;
			}
			if (IsOverIndicator(y))
			{
				CountTasks(out var errors, out var warnings, out var hints, out var _);
				string text = null;
				text = ((errors == 0 && warnings == 0) ? GettextCatalog.GetString("No errors or warnings") : ((errors == 0) ? GettextCatalog.GetPluralString("{0} warning", "{0} warnings", warnings, warnings) : ((warnings != 0) ? GettextCatalog.GetString("{0} errors and {1} warnings", errors, warnings) : GettextCatalog.GetPluralString("{0} error", "{0} errors", errors, errors))));
				if (errors > 0)
				{
					text = text + Environment.NewLine + GettextCatalog.GetString("Click to navigate to the next error");
				}
				else if (warnings > 0)
				{
					text = text + Environment.NewLine + GettextCatalog.GetString("Click to navigate to the next warning");
				}
				else if (warnings + hints > 0)
				{
					text = text + Environment.NewLine + GettextCatalog.GetString("Click to navigate to the next message");
				}
				tooltip.Text = text;
				return true;
			}
			QuickTask hoverTask = GetHoverTask(y);
			if (hoverTask != null)
			{
				tooltip.Text = hoverTask.Description;
				return true;
			}
			return false;
		}

		private void CountTasks(out int errors, out int warnings, out int hints, out int suggestions)
		{
			errors = (warnings = (hints = (suggestions = 0)));
			foreach (QuickTask allTask in AllTasks)
			{
				switch (allTask.Severity)
				{
				case Severity.Error:
					errors++;
					break;
				case Severity.Warning:
					warnings++;
					break;
				case Severity.Hint:
					hints++;
					break;
				case Severity.Suggestion:
					suggestions++;
					break;
				}
			}
		}

		private QuickTask GetHoverTask(double y)
		{
			QuickTask result = null;
			foreach (QuickTask allTask in AllTasks)
			{
				double yPosition = GetYPosition(allTask.Location.Line);
				if (Math.Abs(yPosition - y) < 3.0)
				{
					result = allTask;
				}
			}
			return result;
		}

		private void UpdateState(StateType state)
		{
			if (base.State != state)
			{
				base.State = state;
				QueueDraw();
			}
		}

		private void PositionPreviewWindow(int my)
		{
			base.GdkWindow.GetOrigin(out var x, out var y);
			Gdk.Rectangle monitorGeometry = base.Screen.GetMonitorGeometry(base.Screen.GetMonitorAtPoint(x, y));
			Gdk.Rectangle allocation = previewWindow.Allocation;
			int num = x - 4 - allocation.Width;
			if (num < monitorGeometry.Left)
			{
				num = x + parentStrip.Allocation.Width + 4;
			}
			int val = y + my - allocation.Height / 2;
			val = Math.Max(monitorGeometry.Top, Math.Min(val, monitorGeometry.Bottom));
			previewWindow.Move(num, val);
		}

		private void RemovePreviewPopupTimeout()
		{
			if (previewPopupTimeout != 0)
			{
				Source.Remove(previewPopupTimeout);
				previewPopupTimeout = 0u;
			}
		}

		private void DestroyPreviewWindow()
		{
			if (previewWindow != null)
			{
				previewWindow.Destroy();
				previewWindow = null;
			}
		}

		private void CancelFadeInTimeout()
		{
			if (fadeTimeOutHandler != 0)
			{
				Source.Remove(fadeTimeOutHandler);
				fadeTimeOutHandler = 0u;
			}
		}

		protected override bool OnEnterNotifyEvent(EventCrossing evnt)
		{
			isPointerInside = true;
			if (!IsInGrab())
			{
				CancelFadeInTimeout();
				fadeTimeOutHandler = GLib.Timeout.Add(250u, delegate
				{
					StartFadeInAnimation();
					fadeTimeOutHandler = 0u;
					return false;
				});
			}
			return base.OnEnterNotifyEvent(evnt);
		}

		private void StartFadeInAnimation()
		{
			fadeOutStage.Pause();
			fadeInStage.AddOrReset(this, 90u);
			fadeInStage.Play();
		}

		private void StartFadeOutAnimation()
		{
			CancelFadeInTimeout();
			UpdateState(StateType.Normal);
			if (barColorValue != 0.0)
			{
				fadeInStage.Pause();
				fadeOutStage.AddOrReset(this, 90u);
				fadeOutStage.Play();
			}
		}

		protected override bool OnLeaveNotifyEvent(EventCrossing evnt)
		{
			isPointerInside = false;
			if (!IsInGrab())
			{
				StartFadeOutAnimation();
			}
			RemovePreviewPopupTimeout();
			DestroyPreviewWindow();
			return base.OnLeaveNotifyEvent(evnt);
		}

		private Cairo.Color GetBarColor(Severity severity)
		{
			ColorScheme colorStyle = TextEditor.ColorStyle;
			if (colorStyle == null)
			{
				return new Cairo.Color(0.0, 0.0, 0.0);
			}
			switch (severity)
			{
			case Severity.Error:
				return colorStyle.UnderlineError.Color;
			case Severity.Warning:
				return colorStyle.UnderlineWarning.Color;
			case Severity.Suggestion:
				return colorStyle.UnderlineSuggestion.Color;
			case Severity.Hint:
				return colorStyle.UnderlineHint.Color;
			case Severity.None:
				return colorStyle.PlainText.Background;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		protected virtual void MovePosition(double y)
		{
			double val = (y - IndicatorHeight) / ((double)base.Allocation.Height - IndicatorHeight) * vadjustment.Upper - vadjustment.PageSize / 2.0;
			val = Math.Max(vadjustment.Lower, Math.Min(val, vadjustment.Upper - vadjustment.PageSize));
			vadjustment.Value = val;
		}

		private double GetSliderCenter()
		{
			double num = (double)base.Allocation.Height - IndicatorHeight;
			double num2 = (vadjustment.Value + vadjustment.PageSize / 2.0) / (vadjustment.Upper - vadjustment.Lower);
			return IndicatorHeight + num * num2;
		}

		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			if (evnt.Button != 1 || evnt.IsContextMenuButton())
			{
				return base.OnButtonPressEvent(evnt);
			}
			if (IsOverIndicator(evnt.Y))
			{
				parentStrip.GotoTask(parentStrip.SearchNextTask(GetHoverMode()));
				return base.OnButtonPressEvent(evnt);
			}
			QuickTask hoverTask = GetHoverTask(evnt.Y);
			if (hoverTask != null)
			{
				MoveToTask(hoverTask);
			}
			if (IsInsideBar(evnt.X, evnt.Y))
			{
				Grab.Add(this);
				grabCenter = GetSliderCenter();
				grabY = evnt.Y;
			}
			else
			{
				MovePosition(evnt.Y);
			}
			return base.OnButtonPressEvent(evnt);
		}

		private void ClearGrab()
		{
			if (IsInGrab())
			{
				Grab.Remove(this);
				grabY = -1.0;
			}
		}

		protected override bool OnButtonReleaseEvent(EventButton evnt)
		{
			ClearGrab();
			if (!isPointerInside)
			{
				StartFadeOutAnimation();
			}
			return base.OnButtonReleaseEvent(evnt);
		}

		protected override bool OnGrabBrokenEvent(EventGrabBroken evnt)
		{
			ClearGrab();
			return base.OnGrabBrokenEvent(evnt);
		}

		private void MoveToTask(QuickTask task)
		{
			if (task.Location.IsEmpty)
			{
				Console.WriteLine("empty:" + task.Description);
			}
			DocumentLocation location = new DocumentLocation(Math.Max(1, task.Location.Line), Math.Max(1, task.Location.Column));
			TextEditor.Caret.Location = location;
			TextEditor.CenterToCaret();
			TextEditor.StartCaretPulseAnimation();
			TextEditor.GrabFocus();
		}

		private QuickTaskStrip.HoverMode GetHoverMode()
		{
			CountTasks(out var errors, out var warnings, out var _, out var _);
			if (errors > 0)
			{
				return QuickTaskStrip.HoverMode.NextError;
			}
			if (warnings > 0)
			{
				return QuickTaskStrip.HoverMode.NextWarning;
			}
			return QuickTaskStrip.HoverMode.NextMessage;
		}

		protected void DrawIndicator(Cairo.Context cr, Severity severity)
		{
			Xwt.Drawing.Image img;
			switch (severity)
			{
			case Severity.Error:
				img = errorImage;
				break;
			case Severity.Warning:
				img = warningImage;
				break;
			case Severity.Suggestion:
			case Severity.Hint:
				img = suggestionImage;
				break;
			default:
				img = okImage;
				break;
			}
			DrawIndicator(cr, img);
		}

		protected void DrawSearchIndicator(Cairo.Context cr)
		{
			DrawIndicator(cr, searchImage);
		}

		private void DrawIndicator(Cairo.Context cr, Xwt.Drawing.Image img)
		{
			cr.DrawImage(this, img, Math.Round(((double)base.Allocation.Width - img.Width) / 2.0), -1.0);
		}

		protected override void OnSizeRequested(ref Requisition requisition)
		{
			base.OnSizeRequested(ref requisition);
			requisition.Width = (Platform.IsWindows ? 17 : 15);
		}

		private double LineToY(int logicalLine)
		{
			double num = (double)base.Allocation.Height - IndicatorHeight;
			int y = TextEditor.LocationToPoint(logicalLine, 1, useAbsoluteCoordinates: true).Y;
			double num2 = Math.Max(TextEditor.GetTextEditorData().TotalHeight, TextEditor.Allocation.Height) + (double)TextEditor.Allocation.Height - TextEditor.LineHeight;
			return IndicatorHeight + num * (double)y / num2;
		}

		private int YToLine(double y)
		{
			double num = 0.5 + (y - IndicatorHeight) / ((double)base.Allocation.Height - IndicatorHeight) * (double)TextEditor.GetTextEditorData().VisibleLineCount;
			return TextEditor.GetTextEditorData().VisualToLogicalLine((int)num);
		}

		protected void DrawCaret(Cairo.Context cr)
		{
			if (TextEditor.ColorStyle != null && caretLine >= 0)
			{
				double yPosition = GetYPosition(caretLine);
				cr.MoveTo(0.0, yPosition - 4.0);
				cr.LineTo(7.0, yPosition);
				cr.LineTo(0.0, yPosition + 4.0);
				cr.ClosePath();
				cr.SetSourceColor(TextEditor.ColorStyle.PlainText.Foreground);
				cr.Fill();
			}
		}

		private double GetYPosition(int logicalLine)
		{
			if (!yPositionCache.TryGetValue(logicalLine, out var value))
			{
				value = (yPositionCache[logicalLine] = LineToY(logicalLine));
			}
			return value;
		}

		protected Severity DrawQuickTasks(Cairo.Context cr)
		{
			Severity severity = Severity.None;
			foreach (Usage allUsage in AllUsages)
			{
				DocumentLocation location = allUsage.Location;
				double yPosition = GetYPosition(location.Line);
				Cairo.Color foreground = TextEditor.ColorStyle.PlainText.Foreground;
				foreground.A = 0.4;
				HslColor hslColor = (((allUsage.UsageType & ReferenceUsageType.Write) == 0) ? (((allUsage.UsageType & ReferenceUsageType.Read) == 0) ? ((HslColor)foreground) : ((HslColor)TextEditor.ColorStyle.UsagesRectangle.Color)) : ((HslColor)TextEditor.ColorStyle.ChangingUsagesRectangle.Color));
				hslColor.L = 0.5;
				cr.Color = hslColor;
				cr.MoveTo(0.0, yPosition - 3.0);
				cr.LineTo(5.0, yPosition);
				cr.LineTo(0.0, yPosition + 3.0);
				cr.LineTo(0.0, yPosition - 3.0);
				cr.ClosePath();
				cr.Fill();
			}
			foreach (QuickTask allTask in AllTasks)
			{
				double yPosition2 = GetYPosition(allTask.Location.Line);
				cr.SetSourceColor(GetBarColor(allTask.Severity));
				cr.Rectangle(0.0, Math.Round(yPosition2) - 1.0, base.Allocation.Width, 2.0);
				cr.Fill();
				switch (allTask.Severity)
				{
				case Severity.Error:
					severity = Severity.Error;
					break;
				case Severity.Warning:
					if (severity == Severity.None)
					{
						severity = Severity.Warning;
					}
					break;
				}
			}
			return severity;
		}

		protected void DrawLeftBorder(Cairo.Context cr)
		{
			cr.MoveTo(0.5, 0.0);
			cr.LineTo(0.5, base.Allocation.Height);
			if (TextEditor.ColorStyle != null)
			{
				Xwt.Drawing.Color color = TextEditor.ColorStyle.PlainText.Background.ToXwtColor();
				if (!Platform.IsWindows)
				{
					color.Light *= 0.88;
				}
				cr.SetSourceColor(color.ToCairoColor());
			}
			cr.Stroke();
		}

		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{
			yPositionCache.Clear();
			base.OnSizeAllocated(allocation);
		}

		private void GetBarDimensions(out double x, out double y, out double w, out double h)
		{
			Gdk.Rectangle allocation = base.Allocation;
			x = ((!Platform.IsWindows) ? (1 + barPadding) : 0);
			double upper = vadjustment.Upper;
			int num = allocation.Height - (int)IndicatorHeight;
			y = IndicatorHeight + Math.Round((double)num * vadjustment.Value / upper);
			w = (Platform.IsWindows ? allocation.Width : 8);
			h = Math.Max(16.0, Math.Round((double)num * (vadjustment.PageSize / upper)) - (double)barPadding - (double)barPadding);
		}

		protected virtual void DrawBar(Cairo.Context cr)
		{
			if (vadjustment != null && !(vadjustment.Upper <= vadjustment.PageSize))
			{
				GetBarDimensions(out var x, out var y, out var w, out var h);
				if (Platform.IsWindows)
				{
					cr.Rectangle(x, y, w, h);
				}
				else
				{
					cr.RoundedRectangle(x, y, w, h, 4.0);
				}
				bool flag = base.State == StateType.Prelight;
				Cairo.Color final;
				if (Platform.IsWindows)
				{
					final = (flag ? win81SliderPrelight : win81Slider);
					final = AddAlpha(win81Background, final, 0.5);
				}
				else
				{
					double num = HslColor.Brightness(TextEditor.ColorStyle.PlainText.Background);
					final = new Cairo.Color(1.0 - num, 1.0 - num, 1.0 - num, barColorValue * 0.28 + 0.22);
				}
				cr.SetSourceColor(final);
				cr.Fill();
			}
		}

		private static Cairo.Color AddAlpha(Cairo.Color bg, Cairo.Color final, double alpha)
		{
			return new Cairo.Color(ReverseAlpha(final.R, final.A, bg.R, bg.A, alpha), ReverseAlpha(final.G, final.A, bg.G, bg.A, alpha), ReverseAlpha(final.B, final.A, bg.B, bg.A, alpha), alpha);
		}

		private static double ReverseAlpha(double c0, double a0, double cb, double ab, double aa)
		{
			return (c0 * a0 - cb * ab * (1.0 - aa)) / aa;
		}

		protected void DrawSearchResults(Cairo.Context cr)
		{
			foreach (TextSegment searchResult in TextEditor.TextViewMargin.SearchResults)
			{
				int logicalLine = TextEditor.OffsetToLineNumber(searchResult.Offset);
				double yPosition = GetYPosition(logicalLine);
				bool flag = false;
				if (!TextEditor.TextViewMargin.MainSearchResult.IsInvalid)
				{
					flag = searchResult.Offset == TextEditor.TextViewMargin.MainSearchResult.Offset;
				}
				cr.SetSourceColor(flag ? TextEditor.ColorStyle.SearchResultMain.Color : TextEditor.ColorStyle.SearchResult.Color);
				cr.Rectangle(barPadding, Math.Round(yPosition) - 1.0, base.Allocation.Width - barPadding * 2, 2.0);
				cr.Fill();
			}
		}

		protected override bool OnExposeEvent(EventExpose e)
		{
			if (TextEditor == null)
			{
				return true;
			}
			using (Cairo.Context context = CairoHelper.Create(e.Window))
			{
				context.LineWidth = 1.0;
				context.Rectangle(0.0, 0.0, base.Allocation.Width, base.Allocation.Height);
				if (TextEditor.ColorStyle != null)
				{
					if (Platform.IsWindows)
					{
						using (SolidPattern source = new SolidPattern(win81Background))
						{
							context.SetSource(source);
						}
					}
					else
					{
						Xwt.Drawing.Color color = TextEditor.ColorStyle.PlainText.Background.ToXwtColor();
						color.Light *= 0.948;
						using (Cairo.LinearGradient linearGradient = new Cairo.LinearGradient(0.0, 0.0, base.Allocation.Width, 0.0))
						{
							linearGradient.AddColorStop(0.0, color.ToCairoColor());
							linearGradient.AddColorStop(0.7, TextEditor.ColorStyle.PlainText.Background);
							linearGradient.AddColorStop(1.0, color.ToCairoColor());
							context.SetSource(linearGradient);
						}
					}
				}
				context.Fill();
				if (TextEditor == null)
				{
					return true;
				}
				if (TextEditor.HighlightSearchPattern)
				{
					DrawSearchResults(context);
					DrawSearchIndicator(context);
				}
				else if (!DebuggingService.IsDebugging)
				{
					Severity severity = DrawQuickTasks(context);
					DrawIndicator(context, severity);
				}
				DrawCaret(context);
				if (QuickTaskStrip.MergeScrollBarAndQuickTasks)
				{
					DrawBar(context);
				}
				DrawLeftBorder(context);
			}
			return true;
		}
	}
}
