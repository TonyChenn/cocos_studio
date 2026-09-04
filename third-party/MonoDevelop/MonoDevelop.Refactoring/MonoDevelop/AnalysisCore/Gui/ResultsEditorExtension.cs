using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GLib;
using ICSharpCode.NRefactory.Refactoring;
using Mono.TextEditor;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.SourceEditor.QuickTasks;

namespace MonoDevelop.AnalysisCore.Gui
{
	public class ResultsEditorExtension : TextEditorExtension, IQuickTaskProvider
	{
		private class ResultsUpdater
		{
			private readonly ResultsEditorExtension ext;

			private readonly CancellationToken cancellationToken;

			private int oldMarkers;

			private IEnumerator<Result> enumerator;

			public ResultsUpdater(ResultsEditorExtension ext, IEnumerable<Result> results, CancellationToken cancellationToken)
			{
				if (ext == null)
				{
					throw new ArgumentNullException("ext");
				}
				if (results == null)
				{
					throw new ArgumentNullException("results");
				}
				this.ext = ext;
				this.cancellationToken = cancellationToken;
				oldMarkers = ext.markers.Count;
				enumerator = results.GetEnumerator();
			}

			public void Update()
			{
				if ((bool)QuickTaskStrip.EnableFancyFeatures && !cancellationToken.IsCancellationRequested)
				{
					ext.tasks.Clear();
					Idle.Add(IdleHandler);
				}
			}

			private bool IdleHandler()
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return false;
				}
				TextEditorData editor = ext.Editor;
				if (editor == null || editor.Document == null)
				{
					return false;
				}
				int num = 0;
				while (oldMarkers > 0 && num < 20)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						return false;
					}
					editor.Document.RemoveMarker(ext.markers.Dequeue());
					oldMarkers--;
					num++;
				}
				for (int i = 0; i < 20; i++)
				{
					if (!enumerator.MoveNext())
					{
						ext.OnTasksUpdated(EventArgs.Empty);
						return false;
					}
					if (cancellationToken.IsCancellationRequested)
					{
						return false;
					}
					Result current = enumerator.Current;
					if (current.InspectionMark != IssueMarker.None)
					{
						int num2 = editor.LocationToOffset(current.Region.Begin);
						int num3 = editor.LocationToOffset(current.Region.End);
						if (num2 >= num3)
						{
							continue;
						}
						if (current.InspectionMark == IssueMarker.GrayOut)
						{
							GrayOutMarker grayOutMarker = new GrayOutMarker(current, TextSegment.FromBounds(num2, num3));
							grayOutMarker.IsVisible = current.Underline;
							editor.Document.AddMarker(grayOutMarker);
							ext.markers.Enqueue(grayOutMarker);
							editor.Parent.TextViewMargin.RemoveCachedLine(editor.GetLineByOffset(num2));
							editor.Parent.QueueDraw();
						}
						else
						{
							ResultMarker resultMarker = new ResultMarker(current, TextSegment.FromBounds(num2, num3));
							resultMarker.IsVisible = current.Underline;
							editor.Document.AddMarker(resultMarker);
							ext.markers.Enqueue(resultMarker);
						}
					}
					ext.tasks.Add(new QuickTask(current.Message, current.Region.Begin, current.Level));
				}
				return true;
			}
		}

		private const int UPDATE_COUNT = 20;

		private bool disposed;

		private bool enabled;

		private Task oldTask;

		private CancellationTokenSource src;

		private Queue<ResultMarker> markers = new Queue<ResultMarker>();

		private List<QuickTask> tasks = new List<QuickTask>();

		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				if (enabled != value)
				{
					if (value)
					{
						Enable();
					}
					else
					{
						Disable();
					}
				}
			}
		}

		public IEnumerable<QuickTask> QuickTasks => tasks;

		public event EventHandler TasksUpdated;

		public override void Initialize()
		{
			base.Initialize();
			AnalysisOptions.AnalysisEnabled.Changed += AnalysisOptionsChanged;
			AnalysisOptionsChanged(null, null);
		}

		private void AnalysisOptionsChanged(object sender, EventArgs e)
		{
			Enabled = AnalysisOptions.AnalysisEnabled;
		}

		public override void Dispose()
		{
			if (!disposed)
			{
				enabled = false;
				base.Document.DocumentParsed -= OnDocumentParsed;
				CancelTask();
				AnalysisOptions.AnalysisEnabled.Changed -= AnalysisOptionsChanged;
				while (markers.Count > 0)
				{
					base.Document.Editor.Document.RemoveMarker(markers.Dequeue());
				}
				tasks.Clear();
				disposed = true;
			}
		}

		private void Enable()
		{
			if (!enabled)
			{
				enabled = true;
				base.Document.DocumentParsed += OnDocumentParsed;
				if (base.Document.ParsedDocument != null)
				{
					OnDocumentParsed(null, null);
				}
			}
		}

		private void CancelTask()
		{
			if (src == null)
			{
				return;
			}
			src.Cancel();
			try
			{
				oldTask.Wait();
			}
			catch (TaskCanceledException)
			{
			}
			catch (AggregateException ex2)
			{
				ex2.Handle((Exception e) => e is TaskCanceledException);
			}
		}

		private void Disable()
		{
			if (enabled)
			{
				enabled = false;
				base.Document.DocumentParsed -= OnDocumentParsed;
				CancelTask();
				new ResultsUpdater(this, new Result[0], CancellationToken.None).Update();
			}
		}

		private void OnDocumentParsed(object sender, EventArgs args)
		{
			if (!QuickTaskStrip.EnableFancyFeatures)
			{
				return;
			}
			ParsedDocument parsedDocument = base.Document.ParsedDocument;
			if (parsedDocument == null)
			{
				return;
			}
			lock (this)
			{
				CancelTask();
				src = new CancellationTokenSource();
				RuleTreeType treeType = new RuleTreeType("Document", Path.GetExtension(parsedDocument.FileName));
				Task<IEnumerable<Result>> task = AnalysisService.QueueAnalysis(base.Document, treeType, src.Token);
				oldTask = task.ContinueWith(delegate(Task<IEnumerable<Result>> t)
				{
					new ResultsUpdater(this, t.Result, src.Token).Update();
				}, src.Token);
			}
		}

		public IList<Result> GetResultsAtOffset(int offset, CancellationToken token = default(CancellationToken))
		{
			List<Result> list = new List<Result>();
			foreach (TextSegmentMarker item in base.Editor.Document.GetTextSegmentMarkersAt(offset))
			{
				if (!token.IsCancellationRequested)
				{
					if (item is ResultMarker resultMarker)
					{
						list.Add(resultMarker.Result);
					}
					continue;
				}
				break;
			}
			return list;
		}

		public IEnumerable<Result> GetResults()
		{
			return markers.Select((ResultMarker m) => m.Result);
		}

		protected virtual void OnTasksUpdated(EventArgs e)
		{
			TasksUpdated?.Invoke(this, e);
		}
	}
}
