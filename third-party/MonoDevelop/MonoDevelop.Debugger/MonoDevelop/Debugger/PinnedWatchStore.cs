using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Debugging.Client;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Ide;

namespace MonoDevelop.Debugger
{
	public class PinnedWatchStore
	{
		[ItemProperty("Watch")]
		[ExpandedCollection]
		private List<PinnedWatch> watches = new List<PinnedWatch>();

		private Dictionary<Breakpoint, PinnedWatch> liveWatches = new Dictionary<Breakpoint, PinnedWatch>();

		private List<PinnedWatch> batchAdded;

		private List<PinnedWatch> batchRemoved;

		private List<PinnedWatch> batchChanged;

		private bool changedFlag;

		private int batchUpdate;

		public event EventHandler Changed;

		public event EventHandler<PinnedWatchEventArgs> WatchAdded;

		public event EventHandler<PinnedWatchEventArgs> WatchRemoved;

		public event EventHandler<PinnedWatchEventArgs> WatchChanged;

		public void Add(PinnedWatch watch)
		{
			lock (watches)
			{
				if (watch.Store != null)
				{
					throw new InvalidOperationException("Watch already belongs to another store");
				}
				watch.Store = this;
				watches.Add(watch);
			}
			OnWatchAdded(watch);
			OnChanged();
		}

		public void Remove(PinnedWatch watch)
		{
			lock (watches)
			{
				if (watch.Store != this)
				{
					return;
				}
				watch.Store = null;
				watches.Remove(watch);
			}
			OnWatchRemoved(watch);
			OnChanged();
		}

		public bool IsWatcherBreakpoint(Breakpoint bp)
		{
			lock (watches)
			{
				return liveWatches.ContainsKey(bp);
			}
		}

		internal void Bind(PinnedWatch watch, Breakpoint be)
		{
			lock (watches)
			{
				if (be == null)
				{
					if (watch.BoundTracer != null)
					{
						liveWatches.Remove(watch.BoundTracer);
					}
					watch.LiveUpdate = false;
				}
				else
				{
					watch.BoundTracer = be;
					liveWatches[be] = watch;
					watch.LiveUpdate = true;
				}
			}
		}

		internal void BindAll(BreakpointStore bps)
		{
			lock (watches)
			{
				foreach (PinnedWatch watch in watches)
				{
					foreach (Breakpoint breakpoint in bps.GetBreakpoints())
					{
						if ((breakpoint.HitAction & HitAction.PrintExpression) != HitAction.None && breakpoint.TraceExpression == "{" + watch.Expression + "}" && (FilePath)breakpoint.FileName == watch.File && breakpoint.Line == watch.Line)
						{
							Bind(watch, breakpoint);
						}
					}
				}
			}
		}

		internal bool UpdateLiveWatch(Breakpoint bp, string trace)
		{
			lock (watches)
			{
				if (!liveWatches.TryGetValue(bp, out var value))
				{
					return false;
				}
				value.UpdateFromTrace(trace);
				return true;
			}
		}

		internal void LoadFrom(PinnedWatchStore store)
		{
			try
			{
				BeginBatchUpdate();
				lock (watches)
				{
					List<PinnedWatch> list = new List<PinnedWatch>(watches);
					watches.Clear();
					foreach (PinnedWatch item in list)
					{
						item.Store = null;
						OnWatchRemoved(item);
					}
					foreach (PinnedWatch watch in store.watches)
					{
						watch.Store = this;
						watches.Add(watch);
						OnWatchAdded(watch);
					}
				}
				OnChanged();
			}
			finally
			{
				EndBatchUpdate();
			}
		}

		internal void InvalidateAll()
		{
			try
			{
				lock (watches)
				{
					BeginBatchUpdate();
					foreach (PinnedWatch watch in watches)
					{
						watch.Invalidate();
					}
				}
			}
			finally
			{
				EndBatchUpdate();
			}
		}

		public IEnumerable<PinnedWatch> GetWatchesForFile(FilePath file)
		{
			List<PinnedWatch> list = new List<PinnedWatch>();
			lock (watches)
			{
				list.AddRange(watches.Where((PinnedWatch w) => w.File == file));
			}
			BatchEnsureEvaluated(list);
			return list;
		}

		private void BatchEnsureEvaluated(List<PinnedWatch> ws)
		{
			if (DebuggingService.CurrentFrame == null)
			{
				return;
			}
			try
			{
				BeginBatchUpdate();
				List<string> list = new List<string>();
				foreach (PinnedWatch w in ws)
				{
					list.Add(w.Expression);
				}
				if (list.Count > 0)
				{
					ObjectValue[] expressionValues = DebuggingService.CurrentFrame.GetExpressionValues(list.ToArray(), evaluateMethods: true);
					for (int i = 0; i < expressionValues.Length; i++)
					{
						ws[i].LoadValue(expressionValues[i]);
					}
				}
			}
			finally
			{
				EndBatchUpdate();
			}
		}

		internal void BatchUpdate(IEnumerable<FilePath> fileNames)
		{
			BeginBatchUpdate();
			List<PinnedWatch> list = new List<PinnedWatch>();
			lock (watches)
			{
				list.AddRange(watches.Where((PinnedWatch w) => fileNames.Contains(w.File)));
			}
			BatchEnsureEvaluated(list);
		}

		public void BeginBatchUpdate()
		{
			lock (watches)
			{
				batchUpdate++;
			}
		}

		public void EndBatchUpdate()
		{
			int num;
			List<PinnedWatch> list;
			List<PinnedWatch> list2;
			List<PinnedWatch> list3;
			bool flag;
			lock (watches)
			{
				num = --batchUpdate;
				list = batchAdded;
				list2 = batchRemoved;
				list3 = batchChanged;
				flag = changedFlag;
				batchAdded = null;
				batchRemoved = null;
				batchChanged = null;
				changedFlag = false;
			}
			if (num != 0)
			{
				return;
			}
			if (list != null)
			{
				foreach (PinnedWatch item in list)
				{
					OnWatchAdded(item);
				}
			}
			if (list3 != null)
			{
				foreach (PinnedWatch item2 in list3)
				{
					OnWatchChanged(item2);
				}
			}
			if (list2 != null)
			{
				foreach (PinnedWatch item3 in list2)
				{
					OnWatchRemoved(item3);
				}
			}
			if (flag)
			{
				OnChanged();
			}
		}

		private void OnWatchAdded(PinnedWatch watch)
		{
			if (batchUpdate > 0)
			{
				if (batchAdded == null)
				{
					batchAdded = new List<PinnedWatch>();
				}
				if (!batchAdded.Contains(watch))
				{
					batchAdded.Add(watch);
				}
			}
			else if (WatchAdded != null)
			{
				WatchAdded(this, new PinnedWatchEventArgs(watch));
			}
		}

		private void OnWatchRemoved(PinnedWatch watch)
		{
			if (batchUpdate > 0)
			{
				if (batchRemoved == null)
				{
					batchRemoved = new List<PinnedWatch>();
				}
				if (!batchRemoved.Contains(watch))
				{
					batchRemoved.Add(watch);
				}
			}
			else if (WatchRemoved != null)
			{
				WatchRemoved(this, new PinnedWatchEventArgs(watch));
			}
		}

		private void OnWatchChanged(PinnedWatch watch)
		{
			if (batchUpdate > 0)
			{
				if (batchChanged == null)
				{
					batchChanged = new List<PinnedWatch>();
				}
				if (!batchChanged.Contains(watch))
				{
					batchChanged.Add(watch);
				}
				return;
			}
			DispatchService.GuiDispatch(delegate
			{
				if (WatchChanged != null)
				{
					WatchChanged(this, new PinnedWatchEventArgs(watch));
				}
			});
		}

		private void OnChanged()
		{
			if (batchUpdate > 0)
			{
				changedFlag = true;
			}
			else if (Changed != null)
			{
				Changed(this, EventArgs.Empty);
			}
		}

		internal void NotifyWatchChanged(PinnedWatch watch)
		{
			OnWatchChanged(watch);
			OnChanged();
		}
	}
}
