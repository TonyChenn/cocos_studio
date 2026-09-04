using System;
using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Components;
using Pango;

namespace MonoDevelop.Debugger
{
	public class ThreadsPad : ScrolledWindow, IPadContent, IDisposable
	{
		private enum Columns
		{
			Icon,
			Id,
			Name,
			Object,
			Weight,
			Location
		}

		private TreeViewState treeViewState;

		private PadTreeView tree;

		private TreeStore store;

		private bool needsUpdate;

		private IPadWindow window;

		public Widget Control => this;

		public string Id => "MonoDevelop.Debugger.ThreadsPad";

		public string DefaultPlacement => "Bottom";

		public ThreadsPad()
		{
			base.ShadowType = ShadowType.None;
			store = new TreeStore(typeof(string), typeof(string), typeof(string), typeof(object), typeof(int), typeof(string));
			tree = new PadTreeView(store);
			tree.RulesHint = true;
			tree.HeadersVisible = true;
			treeViewState = new TreeViewState(tree, 3);
			TreeViewColumn treeViewColumn = new TreeViewColumn();
			CellRenderer cell = new CellRendererImage();
			treeViewColumn.PackStart(cell, expand: false);
			treeViewColumn.AddAttribute(cell, "stock_id", 0);
			tree.AppendColumn(treeViewColumn);
			TreeViewColumn treeViewColumn2 = new TreeViewColumn();
			treeViewColumn2.Title = GettextCatalog.GetString("Id");
			treeViewColumn2.PackStart(tree.TextRenderer, expand: true);
			treeViewColumn2.AddAttribute(tree.TextRenderer, "text", 1);
			treeViewColumn2.AddAttribute(tree.TextRenderer, "weight", 4);
			treeViewColumn2.Resizable = true;
			treeViewColumn2.Alignment = 0f;
			tree.AppendColumn(treeViewColumn2);
			treeViewColumn = new TreeViewColumn();
			treeViewColumn.Title = GettextCatalog.GetString("Name");
			treeViewColumn.Resizable = true;
			treeViewColumn.PackStart(tree.TextRenderer, expand: false);
			treeViewColumn.AddAttribute(tree.TextRenderer, "text", 2);
			treeViewColumn.AddAttribute(tree.TextRenderer, "weight", 4);
			tree.AppendColumn(treeViewColumn);
			treeViewColumn = new TreeViewColumn();
			treeViewColumn.Title = GettextCatalog.GetString("Location");
			treeViewColumn.Resizable = true;
			treeViewColumn.PackStart(tree.TextRenderer, expand: false);
			treeViewColumn.AddAttribute(tree.TextRenderer, "text", 5);
			treeViewColumn.AddAttribute(tree.TextRenderer, "weight", 4);
			tree.AppendColumn(treeViewColumn);
			Add(tree);
			ShowAll();
			UpdateDisplay();
			tree.RowActivated += OnRowActivated;
			DebuggingService.CallStackChanged += OnStackChanged;
			DebuggingService.PausedEvent += OnDebuggerPaused;
			DebuggingService.ResumedEvent += OnDebuggerResumed;
			DebuggingService.StoppedEvent += OnDebuggerStopped;
		}

		public override void Dispose()
		{
			base.Dispose();
			DebuggingService.CallStackChanged -= OnStackChanged;
			DebuggingService.PausedEvent -= OnDebuggerPaused;
			DebuggingService.ResumedEvent -= OnDebuggerResumed;
			DebuggingService.StoppedEvent -= OnDebuggerStopped;
		}

		private void OnStackChanged(object s, EventArgs a)
		{
			UpdateDisplay();
		}

		void IPadContent.Initialize(IPadWindow window)
		{
			this.window = window;
			window.PadContentShown += delegate
			{
				if (needsUpdate)
				{
					Update();
				}
			};
		}

		public void UpdateDisplay()
		{
			if (window != null && window.ContentVisible)
			{
				Update();
			}
			else
			{
				needsUpdate = true;
			}
		}

		private void Update()
		{
			if (tree.IsRealized)
			{
				tree.ScrollToPoint(0, 0);
			}
			treeViewState.Save();
			store.Clear();
			if (!DebuggingService.IsPaused)
			{
				return;
			}
			try
			{
				ProcessInfo[] processes = DebuggingService.DebuggerSession.GetProcesses();
				if (processes.Length == 1)
				{
					AppendThreads(TreeIter.Zero, processes[0]);
				}
				else
				{
					ProcessInfo[] array = processes;
					foreach (ProcessInfo processInfo in array)
					{
						TreeIter iter = store.AppendValues(null, processInfo.Id.ToString(), processInfo.Name, processInfo, 400, "");
						AppendThreads(iter, processInfo);
					}
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogInternalError(ex);
			}
			tree.ExpandAll();
			treeViewState.Load();
		}

		private void AppendThreads(TreeIter iter, ProcessInfo process)
		{
			ThreadInfo[] threads = process.GetThreads();
			Array.Sort(threads, (ThreadInfo t1, ThreadInfo t2) => t1.Id.CompareTo(t2.Id));
			DebuggingService.DebuggerSession.FetchFrames(threads);
			ThreadInfo[] array = threads;
			foreach (ThreadInfo threadInfo in array)
			{
				ThreadInfo activeThread = DebuggingService.DebuggerSession.ActiveThread;
				string text = ((threadInfo.Name == null && threadInfo.Id == 1) ? "Main Thread" : threadInfo.Name);
				Weight weight = ((threadInfo == activeThread) ? Weight.Bold : Weight.Normal);
				string text2 = ((threadInfo == activeThread) ? Gtk.Stock.GoForward : null);
				if (iter.Equals(TreeIter.Zero))
				{
					store.AppendValues(text2, threadInfo.Id.ToString(), text, threadInfo, (int)weight, threadInfo.Location);
				}
				else
				{
					store.AppendValues(iter, text2, threadInfo.Id.ToString(), text, threadInfo, (int)weight, threadInfo.Location);
				}
			}
		}

		private void UpdateThread(TreeIter iter, ThreadInfo thread, ThreadInfo activeThread)
		{
			Weight value = ((thread == activeThread) ? Weight.Bold : Weight.Normal);
			string value2 = ((thread == activeThread) ? Gtk.Stock.GoForward : null);
			store.SetValue(iter, 4, (int)value);
			store.SetValue(iter, 0, value2);
		}

		private void UpdateThreads(ThreadInfo activeThread)
		{
			if (!store.GetIterFirst(out var iter))
			{
				return;
			}
			do
			{
				ThreadInfo threadInfo = store.GetValue(iter, 3) as ThreadInfo;
				if (threadInfo == null)
				{
					if (store.IterChildren(out var iter2))
					{
						do
						{
							threadInfo = store.GetValue(iter, 3) as ThreadInfo;
							UpdateThread(iter2, threadInfo, activeThread);
						}
						while (store.IterNext(ref iter2));
					}
				}
				else
				{
					UpdateThread(iter, threadInfo, activeThread);
				}
			}
			while (store.IterNext(ref iter));
		}

		private void OnRowActivated(object s, RowActivatedArgs args)
		{
			if (!tree.Selection.GetSelected(out var iter))
			{
				return;
			}
			ThreadInfo threadInfo = store.GetValue(iter, 3) as ThreadInfo;
			if (!(threadInfo != null))
			{
				return;
			}
			DebuggingService.CallStackChanged -= OnStackChanged;
			try
			{
				DebuggingService.ActiveThread = threadInfo;
				UpdateThreads(threadInfo);
			}
			finally
			{
				DebuggingService.CallStackChanged += OnStackChanged;
			}
		}

		public void RedrawContent()
		{
			UpdateDisplay();
		}

		private void OnDebuggerPaused(object s, EventArgs a)
		{
			UpdateDisplay();
		}

		private void OnDebuggerResumed(object s, EventArgs a)
		{
			UpdateDisplay();
		}

		private void OnDebuggerStopped(object s, EventArgs a)
		{
			UpdateDisplay();
		}
	}
}
