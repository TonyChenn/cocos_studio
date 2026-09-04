using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cairo;
using GLib;
using Gtk;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide.FindInFiles;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.SourceEditor.QuickTasks;
using Pango;

namespace MonoDevelop.SourceEditor
{
	public abstract class AbstractUsagesExtension<T> : TextEditorExtension, IUsageProvider
	{
		public class UsageMarker : TextLineMarker
		{
			private List<UsageSegment> usages = new List<UsageSegment>();

			public List<UsageSegment> Usages => usages;

			public bool Contains(int offset)
			{
				return usages.Any((UsageSegment u) => u.TextSegment.Offset <= offset && offset <= u.TextSegment.EndOffset);
			}

			public override bool DrawBackground(TextEditor editor, Cairo.Context cr, double y, LineMetrics metrics)
			{
				if (metrics.SelectionStart >= 0 || editor.CurrentMode is TextLinkEditMode || editor.TextViewMargin.SearchResultMatchCount > 0)
				{
					return false;
				}
				foreach (UsageSegment usage in Usages)
				{
					int offset = usage.TextSegment.Offset;
					int endOffset = usage.TextSegment.EndOffset;
					if (endOffset < metrics.TextStartOffset || offset > metrics.TextEndOffset)
					{
						return false;
					}
					double val;
					double val2;
					if (offset < metrics.TextStartOffset && metrics.TextEndOffset < endOffset)
					{
						val = metrics.TextRenderStartPosition;
						val2 = metrics.TextRenderEndPosition;
					}
					else
					{
						int num = ((metrics.TextStartOffset < offset) ? offset : metrics.TextStartOffset);
						int num2 = ((metrics.TextEndOffset < endOffset) ? metrics.TextEndOffset : endOffset);
						uint curIndex = 0u;
						uint byteIndex = 0u;
						TextViewMargin.TranslateToUTF8Index(metrics.Layout.LineChars, (uint)(num - metrics.TextStartOffset), ref curIndex, ref byteIndex);
						int x = metrics.Layout.Layout.IndexToPos((int)byteIndex).X;
						val = metrics.TextRenderStartPosition + (double)(int)((double)x / Pango.Scale.PangoScale);
						TextViewMargin.TranslateToUTF8Index(metrics.Layout.LineChars, (uint)(num2 - metrics.TextStartOffset), ref curIndex, ref byteIndex);
						x = metrics.Layout.Layout.IndexToPos((int)byteIndex).X;
						val2 = metrics.TextRenderStartPosition + (double)(int)((double)x / Pango.Scale.PangoScale);
					}
					val = Math.Max(val, editor.TextViewMargin.XOffset);
					val2 = Math.Max(val2, editor.TextViewMargin.XOffset);
					if (val < val2)
					{
						AmbientColor ambientColor = (((usage.UsageType & ReferenceUsageType.Write) != ReferenceUsageType.Write) ? editor.ColorStyle.UsagesRectangle : editor.ColorStyle.ChangingUsagesRectangle);
						using (LinearGradient linearGradient = new LinearGradient(val + 1.0, y + 1.0, val2, y + editor.LineHeight))
						{
							linearGradient.AddColorStop(0.0, ambientColor.Color);
							linearGradient.AddColorStop(1.0, ambientColor.SecondColor);
							cr.SetSource(linearGradient);
							cr.RoundedRectangle(val + 0.5, y + 1.5, val2 - val - 1.0, editor.LineHeight - 2.0, editor.LineHeight / 4.0);
							cr.FillPreserve();
						}
						cr.SetSourceColor(ambientColor.BorderColor);
						cr.Stroke();
					}
				}
				return true;
			}
		}

		public class UsageSegment
		{
			public readonly ReferenceUsageType UsageType;

			public readonly TextSegment TextSegment;

			public UsageSegment(ReferenceUsageType usageType, int offset, int length)
			{
				UsageType = usageType;
				TextSegment = new TextSegment(offset, length);
			}

			public static implicit operator TextSegment(UsageSegment usage)
			{
				return usage.TextSegment;
			}
		}

		protected static readonly List<MemberReference> EmptyList = new List<MemberReference>();

		public readonly List<UsageSegment> UsagesSegments = new List<UsageSegment>();

		protected TextEditorData TextEditorData;

		private CancellationTokenSource tooltipCancelSrc = new CancellationTokenSource();

		private Dictionary<int, UsageMarker> markers = new Dictionary<int, UsageMarker>();

		private uint popupTimer;

		private readonly List<Usage> usages = new List<Usage>();

		public Dictionary<int, UsageMarker> Markers => markers;

		public bool IsTimerOnQueue => popupTimer != 0;

		IEnumerable<Usage> IUsageProvider.Usages => usages;

		public event EventHandler UsagesUpdated;

		public override void Initialize()
		{
			base.Initialize();
			TextEditorData = base.Document.Editor;
			TextEditorData.Caret.PositionChanged += HandleTextEditorDataCaretPositionChanged;
			TextEditorData.Document.TextReplaced += HandleTextEditorDataDocumentTextReplaced;
			TextEditorData.SelectionChanged += HandleTextEditorDataSelectionChanged;
			PropertyService.PropertyChanged += PropertyService_PropertyChanged;
		}

		private void PropertyService_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (!(e.Key != "EnableHighlightUsages"))
			{
				HandleTextEditorDataCaretPositionChanged(null, null);
			}
		}

		private void HandleTextEditorDataSelectionChanged(object sender, EventArgs e)
		{
			if (TextEditorData.IsSomethingSelected)
			{
				RemoveMarkers();
			}
		}

		private void HandleTextEditorDataDocumentTextReplaced(object sender, DocumentChangeEventArgs e)
		{
			RemoveMarkers();
		}

		public override void Dispose()
		{
			CancelTooltip();
			PropertyService.PropertyChanged -= PropertyService_PropertyChanged;
			TextEditorData.SelectionChanged -= HandleTextEditorDataSelectionChanged;
			TextEditorData.Caret.PositionChanged -= HandleTextEditorDataCaretPositionChanged;
			TextEditorData.Document.TextReplaced -= HandleTextEditorDataDocumentTextReplaced;
			base.Dispose();
			RemoveTimer();
		}

		public void ForceUpdate()
		{
			RemoveTimer();
			DelayedTooltipShow();
		}

		protected abstract bool TryResolve(out T resolveResult);

		protected abstract IEnumerable<MemberReference> GetReferences(T resolveResult, CancellationToken token);

		private bool DelayedTooltipShow()
		{
			try
			{
				if (!TryResolve(out var result))
				{
					ClearQuickTasks();
					return false;
				}
				CancelTooltip();
				CancellationToken token = tooltipCancelSrc.Token;
				Task.Factory.StartNew(delegate
				{
					List<MemberReference> list = GetReferences(result, token).ToList();
					if (!token.IsCancellationRequested)
					{
						Application.Invoke(delegate
						{
							if (!token.IsCancellationRequested)
							{
								ShowReferences(list);
							}
						});
					}
				});
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Unhandled Exception in HighlightingUsagesExtension", ex);
			}
			finally
			{
				popupTimer = 0u;
			}
			return false;
		}

		private void RemoveTimer()
		{
			if (popupTimer != 0)
			{
				Source.Remove(popupTimer);
				popupTimer = 0u;
			}
		}

		private void HandleTextEditorDataCaretPositionChanged(object sender, DocumentLocationEventArgs e)
		{
			if (!DefaultSourceEditorOptions.Instance.EnableHighlightUsages)
			{
				RemoveMarkers();
				RemoveTimer();
			}
			else if (TextEditorData.IsSomethingSelected || !markers.Values.Any((UsageMarker m) => m.Contains(TextEditorData.Caret.Offset)))
			{
				RemoveMarkers();
				RemoveTimer();
				if (!TextEditorData.IsSomethingSelected)
				{
					popupTimer = GLib.Timeout.Add(1000u, DelayedTooltipShow);
				}
			}
		}

		private void ClearQuickTasks()
		{
			UsagesSegments.Clear();
			if (usages.Count > 0)
			{
				usages.Clear();
				OnUsagesUpdated(EventArgs.Empty);
			}
		}

		private void CancelTooltip()
		{
			tooltipCancelSrc.Cancel();
			tooltipCancelSrc = new CancellationTokenSource();
		}

		private UsageMarker GetMarker(int line)
		{
			if (!markers.TryGetValue(line, out var value))
			{
				value = new UsageMarker();
				TextEditorData.Document.AddMarker(line, value);
				markers.Add(line, value);
			}
			return value;
		}

		private void RemoveMarkers()
		{
			if (markers.Count == 0)
			{
				return;
			}
			TextEditorData.Parent.TextViewMargin.AlphaBlendSearchResults = false;
			foreach (KeyValuePair<int, UsageMarker> marker in markers)
			{
				TextEditorData.Document.RemoveMarker(marker.Value, updateLine: true);
			}
			markers.Clear();
		}

		private void ShowReferences(IEnumerable<MemberReference> references)
		{
			RemoveMarkers();
			HashSet<int> hashSet = new HashSet<int>();
			usages.Clear();
			UsagesSegments.Clear();
			TextEditor parent = TextEditorData.Parent;
			if (parent != null && parent.TextViewMargin != null)
			{
				if (references != null)
				{
					bool flag = false;
					foreach (MemberReference reference in references)
					{
						if (reference != null)
						{
							UsageMarker marker = GetMarker(reference.Region.BeginLine);
							usages.Add(new Usage(reference.Region.Begin, reference.ReferenceUsageType));
							int offset = reference.Offset;
							int endOffset = offset + reference.Length;
							if (!flag && parent.TextViewMargin.SearchResults.Any((TextSegment sr) => sr.Contains(offset) || sr.Contains(endOffset) || (offset < sr.Offset && sr.EndOffset < endOffset)))
							{
								flag = (parent.TextViewMargin.AlphaBlendSearchResults = true);
							}
							UsagesSegments.Add(new UsageSegment(reference.ReferenceUsageType, offset, endOffset - offset));
							marker.Usages.Add(new UsageSegment(reference.ReferenceUsageType, offset, endOffset - offset));
							hashSet.Add(reference.Region.BeginLine);
						}
					}
				}
				foreach (int item in hashSet)
				{
					TextEditorData.Document.CommitLineUpdate(item);
				}
				UsagesSegments.Sort((UsageSegment x, UsageSegment y) => x.TextSegment.Offset.CompareTo(y.TextSegment.Offset));
			}
			OnUsagesUpdated(EventArgs.Empty);
		}

		private void OnUsagesUpdated(EventArgs e)
		{
			UsagesUpdated?.Invoke(this, e);
		}
	}
}
