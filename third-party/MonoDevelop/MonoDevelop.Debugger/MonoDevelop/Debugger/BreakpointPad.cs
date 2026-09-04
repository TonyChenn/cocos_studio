using System;
using GLib;
using Gdk;
using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Components;
using MonoDevelop.Components.Commands;
using MonoDevelop.Components.Docking;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Components;

namespace MonoDevelop.Debugger
{
	public class BreakpointPad : IPadContent, IDisposable
	{
		private enum Columns
		{
			Icon,
			Selected,
			FileName,
			Breakpoint,
			Condition,
			TraceExp,
			HitCount,
			LastTrace
		}

		private enum LocalCommands
		{
			GoToFile,
			Properties
		}

		private BreakpointStore breakpoints;

		private PadTreeView tree;

		private TreeStore store;

		private Widget control;

		private ScrolledWindow sw;

		private CommandEntrySet menuSet;

		private TreeViewState treeState;

		private EventHandler<BreakpointEventArgs> breakpointUpdatedHandler;

		private EventHandler<BreakpointEventArgs> breakpointRemovedHandler;

		private EventHandler<BreakpointEventArgs> breakpointAddedHandler;

		private EventHandler breakpointChangedHandler;

		public Widget Control => control;

		public string Id => "MonoDevelop.Debugger.BreakpointPad";

		public string DefaultPlacement => "Bottom";

		public void Initialize(IPadWindow window)
		{
			ActionCommand cmd = new ActionCommand(LocalCommands.GoToFile, GettextCatalog.GetString("Go to File"));
			ActionCommand cmd2 = new ActionCommand(LocalCommands.Properties, GettextCatalog.GetString("Properties"), Gtk.Stock.Properties);
			menuSet = new CommandEntrySet();
			menuSet.Add(cmd);
			menuSet.AddSeparator();
			menuSet.AddItem(DebugCommands.EnableDisableBreakpoint);
			menuSet.AddItem(DebugCommands.ClearAllBreakpoints);
			menuSet.AddItem(DebugCommands.DisableAllBreakpoints);
			menuSet.AddItem(EditCommands.DeleteKey);
			menuSet.AddSeparator();
			menuSet.Add(cmd2);
			CommandEntrySet commandEntrySet = new CommandEntrySet();
			commandEntrySet.AddItem(DebugCommands.EnableDisableBreakpoint);
			commandEntrySet.AddItem(DebugCommands.ClearAllBreakpoints);
			commandEntrySet.AddItem(DebugCommands.DisableAllBreakpoints);
			commandEntrySet.AddItem(EditCommands.Delete);
			commandEntrySet.AddSeparator();
			commandEntrySet.Add(cmd2);
			commandEntrySet.AddSeparator();
			commandEntrySet.Add(new CommandEntry(DebugCommands.NewFunctionBreakpoint)
			{
				DispayType = CommandEntryDisplayType.IconAndText
			});
			commandEntrySet.Add(new CommandEntry(DebugCommands.NewCatchpoint)
			{
				DispayType = CommandEntryDisplayType.IconAndText
			});
			store = new TreeStore(typeof(string), typeof(bool), typeof(string), typeof(object), typeof(string), typeof(string), typeof(string), typeof(string));
			tree = new PadTreeView();
			tree.Model = store;
			tree.RulesHint = true;
			tree.HeadersVisible = true;
			tree.DoPopupMenu = ShowPopup;
			tree.KeyPressEvent += OnKeyPressEvent;
			tree.Selection.Mode = SelectionMode.Multiple;
			treeState = new TreeViewState(tree, 3);
			TreeViewColumn treeViewColumn = new TreeViewColumn();
			CellRenderer cell = new CellRendererImage();
			treeViewColumn.PackStart(cell, expand: false);
			treeViewColumn.AddAttribute(cell, "stock_id", 0);
			tree.AppendColumn(treeViewColumn);
			CellRendererToggle cellRendererToggle = new CellRendererToggle();
			cellRendererToggle.Toggled += ItemToggled;
			treeViewColumn = new TreeViewColumn();
			treeViewColumn.PackStart(cellRendererToggle, expand: false);
			treeViewColumn.AddAttribute(cellRendererToggle, "active", 1);
			tree.AppendColumn(treeViewColumn);
			TreeViewColumn treeViewColumn2 = new TreeViewColumn();
			CellRenderer textRenderer = tree.TextRenderer;
			treeViewColumn2.Title = GettextCatalog.GetString("Name");
			treeViewColumn2.PackStart(textRenderer, expand: true);
			treeViewColumn2.AddAttribute(textRenderer, "text", 2);
			treeViewColumn2.Resizable = true;
			treeViewColumn2.Alignment = 0f;
			tree.AppendColumn(treeViewColumn2);
			treeViewColumn = tree.AppendColumn(GettextCatalog.GetString("Condition"), textRenderer, "text", 4);
			treeViewColumn.Resizable = true;
			treeViewColumn = tree.AppendColumn(GettextCatalog.GetString("Trace Expression"), textRenderer, "text", 5);
			treeViewColumn.Resizable = true;
			treeViewColumn = tree.AppendColumn(GettextCatalog.GetString("Hit Count"), textRenderer, "text", 6);
			treeViewColumn.Resizable = true;
			treeViewColumn = tree.AppendColumn(GettextCatalog.GetString("Last Trace"), textRenderer, "text", 7);
			treeViewColumn.Resizable = true;
			sw = new ScrolledWindow();
			sw.ShadowType = ShadowType.None;
			sw.Add(tree);
			control = sw;
			control.ShowAll();
			breakpoints = DebuggingService.Breakpoints;
			UpdateDisplay();
			breakpointUpdatedHandler = DispatchService.GuiDispatch<EventHandler<BreakpointEventArgs>>(OnBreakpointUpdated);
			breakpointRemovedHandler = DispatchService.GuiDispatch<EventHandler<BreakpointEventArgs>>(OnBreakpointRemoved);
			breakpointAddedHandler = DispatchService.GuiDispatch<EventHandler<BreakpointEventArgs>>(OnBreakpointAdded);
			breakpointChangedHandler = DispatchService.GuiDispatch<EventHandler>(OnBreakpointChanged);
			breakpoints.BreakpointAdded += breakpointAddedHandler;
			breakpoints.BreakpointRemoved += breakpointRemovedHandler;
			breakpoints.Changed += breakpointChangedHandler;
			breakpoints.BreakpointUpdated += breakpointUpdatedHandler;
			DebuggingService.PausedEvent += OnDebuggerStatusCheck;
			DebuggingService.ResumedEvent += OnDebuggerStatusCheck;
			DebuggingService.StoppedEvent += OnDebuggerStatusCheck;
			tree.RowActivated += OnRowActivated;
			DockItemToolbar toolbar = window.GetToolbar(PositionType.Top);
			toolbar.Add(commandEntrySet, sw);
			toolbar.ShowAll();
		}

		public void Dispose()
		{
			breakpoints.BreakpointAdded -= breakpointAddedHandler;
			breakpoints.BreakpointRemoved -= breakpointRemovedHandler;
			breakpoints.Changed -= breakpointChangedHandler;
			breakpoints.BreakpointUpdated -= breakpointUpdatedHandler;
			DebuggingService.PausedEvent -= OnDebuggerStatusCheck;
			DebuggingService.ResumedEvent -= OnDebuggerStatusCheck;
			DebuggingService.StoppedEvent -= OnDebuggerStatusCheck;
		}

		private void ShowPopup(EventButton evt)
		{
			IdeApp.CommandService.ShowContextMenu(tree, evt, menuSet, tree);
		}

		[CommandHandler(LocalCommands.Properties)]
		protected void OnProperties()
		{
			TreePath[] selectedRows = tree.Selection.GetSelectedRows();
			if (selectedRows.Length == 1 && store.GetIter(out var iter, selectedRows[0]))
			{
				BreakEvent bp = (BreakEvent)store.GetValue(iter, 3);
				if (DebuggingService.ShowBreakpointProperties(ref bp))
				{
					UpdateDisplay();
				}
			}
		}

		private string GetIconId(BreakEvent bp)
		{
			if (bp is Catchpoint)
			{
				if (!bp.Enabled)
				{
					return "md-catchpoint-disabled";
				}
				return "md-catchpoint";
			}
			if (!bp.Enabled)
			{
				return "md-breakpoint-disabled";
			}
			return "md-breakpoint";
		}

		[CommandHandler(DebugCommands.EnableDisableBreakpoint)]
		protected void OnEnableDisable()
		{
			breakpoints.Changed -= breakpointChangedHandler;
			try
			{
				bool flag = false;
				TreePath[] selectedRows = tree.Selection.GetSelectedRows();
				foreach (TreePath path in selectedRows)
				{
					if (store.GetIter(out var iter, path))
					{
						BreakEvent breakEvent = (BreakEvent)store.GetValue(iter, 3);
						if (!breakEvent.Enabled)
						{
							flag = true;
							break;
						}
					}
				}
				TreePath[] selectedRows2 = tree.Selection.GetSelectedRows();
				foreach (TreePath path2 in selectedRows2)
				{
					if (store.GetIter(out var iter2, path2))
					{
						BreakEvent breakEvent2 = (BreakEvent)store.GetValue(iter2, 3);
						breakEvent2.Enabled = flag;
						store.SetValue(iter2, 0, GetIconId(breakEvent2));
						store.SetValue(iter2, 1, flag);
					}
				}
			}
			finally
			{
				breakpoints.Changed += breakpointChangedHandler;
			}
		}

		[CommandHandler(LocalCommands.GoToFile)]
		protected void OnBpJumpTo()
		{
			TreePath[] selectedRows = tree.Selection.GetSelectedRows();
			if (selectedRows.Length == 1 && store.GetIter(out var iter, selectedRows[0]))
			{
				BreakEvent breakEvent = (BreakEvent)store.GetValue(iter, 3);
				if (breakEvent is Breakpoint breakpoint && !string.IsNullOrEmpty(breakpoint.FileName))
				{
					IdeApp.Workbench.OpenDocument(breakpoint.FileName, breakpoint.Line, 1);
				}
			}
		}

		private bool DeleteSelectedBreakpoints()
		{
			bool result = false;
			breakpoints.BreakpointRemoved -= breakpointRemovedHandler;
			try
			{
				TreePath[] selectedRows = tree.Selection.GetSelectedRows();
				Array.Sort(selectedRows, new TreePathComparer(reversed: true));
				TreePath[] array = selectedRows;
				foreach (TreePath path in array)
				{
					if (store.GetIter(out var iter, path))
					{
						BreakEvent bp = (BreakEvent)store.GetValue(iter, 3);
						lock (breakpoints)
						{
							breakpoints.Remove(bp);
						}
						result = true;
					}
				}
				return result;
			}
			finally
			{
				breakpoints.BreakpointRemoved += breakpointRemovedHandler;
			}
		}

		[CommandUpdateHandler(EditCommands.SelectAll)]
		protected void UpdateSelectAll(CommandInfo cmd)
		{
			cmd.Enabled = store.GetIterFirst(out var _);
		}

		[CommandHandler(EditCommands.SelectAll)]
		protected void OnSelectAll()
		{
			tree.Selection.SelectAll();
		}

		[CommandHandler(EditCommands.DeleteKey)]
		[CommandHandler(EditCommands.Delete)]
		protected void OnDeleted()
		{
			if (DeleteSelectedBreakpoints())
			{
				UpdateDisplay();
			}
		}

		[CommandUpdateHandler(LocalCommands.Properties)]
		[CommandUpdateHandler(LocalCommands.GoToFile)]
		protected void UpdateBpCommand(CommandInfo cmd)
		{
			cmd.Enabled = tree.Selection.CountSelectedRows() == 1;
		}

		[CommandUpdateHandler(DebugCommands.EnableDisableBreakpoint)]
		[CommandUpdateHandler(EditCommands.DeleteKey)]
		[CommandUpdateHandler(EditCommands.Delete)]
		protected void UpdateMultiBpCommand(CommandInfo cmd)
		{
			cmd.Enabled = tree.Selection.CountSelectedRows() > 0;
		}

		[ConnectBefore]
		private void OnKeyPressEvent(object sender, KeyPressEventArgs args)
		{
			switch (args.Event.Key)
			{
			case Gdk.Key.BackSpace:
			case Gdk.Key.KP_Delete:
			case Gdk.Key.Delete:
				if (DeleteSelectedBreakpoints())
				{
					args.RetVal = true;
					UpdateDisplay();
				}
				break;
			case Gdk.Key.space:
				if (tree.Selection.CountSelectedRows() > 0)
				{
					OnEnableDisable();
					args.RetVal = true;
				}
				break;
			}
		}

		private void ItemToggled(object o, ToggledArgs args)
		{
			breakpoints.Changed -= breakpointChangedHandler;
			try
			{
				if (store.GetIterFromString(out var iter, args.Path))
				{
					BreakEvent breakEvent = (BreakEvent)store.GetValue(iter, 3);
					breakEvent.Enabled = !breakEvent.Enabled;
					store.SetValue(iter, 0, GetIconId(breakEvent));
					store.SetValue(iter, 1, breakEvent.Enabled);
				}
			}
			finally
			{
				breakpoints.Changed += breakpointChangedHandler;
			}
		}

		public void UpdateDisplay()
		{
			if (tree.IsRealized)
			{
				tree.ScrollToPoint(0, 0);
			}
			treeState.Save();
			store.Clear();
			if (breakpoints != null)
			{
				lock (breakpoints)
				{
					foreach (BreakEvent breakevent in breakpoints.GetBreakevents())
					{
						string text = ((breakevent.HitCountMode != HitCountMode.None) ? breakevent.CurrentHitCount.ToString() : "");
						string text2 = (((breakevent.HitAction & HitAction.PrintExpression) != HitAction.None) ? breakevent.TraceExpression : "");
						string text3 = (((breakevent.HitAction & HitAction.PrintExpression) != HitAction.None) ? breakevent.LastTraceValue : "");
						FunctionBreakpoint functionBreakpoint = breakevent as FunctionBreakpoint;
						Breakpoint breakpoint = breakevent as Breakpoint;
						Catchpoint catchpoint = breakevent as Catchpoint;
						string text4 = ((functionBreakpoint == null) ? ((breakpoint == null) ? ((catchpoint == null) ? "" : catchpoint.ExceptionName) : $"{breakpoint.FileName}:{breakpoint.Line},{breakpoint.Column}") : ((functionBreakpoint.ParamTypes == null) ? functionBreakpoint.FunctionName : (functionBreakpoint.FunctionName + "(" + string.Join(", ", functionBreakpoint.ParamTypes) + ")")));
						store.AppendValues(GetIconId(breakevent), breakevent.Enabled, text4, breakevent, breakpoint?.ConditionExpression, text2, text, text3);
					}
				}
			}
			treeState.Load();
		}

		private void OnBreakpointUpdated(object s, BreakpointEventArgs args)
		{
			if (!store.GetIterFirst(out var iter))
			{
				return;
			}
			do
			{
				BreakEvent breakEvent = (BreakEvent)store.GetValue(iter, 3);
				if (breakEvent == args.Breakpoint)
				{
					string value = ((breakEvent.HitCountMode != HitCountMode.None) ? breakEvent.CurrentHitCount.ToString() : "");
					string value2 = (((breakEvent.HitAction & HitAction.PrintExpression) != HitAction.None) ? breakEvent.LastTraceValue : "");
					store.SetValue(iter, 6, value);
					store.SetValue(iter, 7, value2);
					break;
				}
			}
			while (store.IterNext(ref iter));
		}

		protected void OnBreakpointAdded(object o, EventArgs args)
		{
			UpdateDisplay();
		}

		protected void OnBreakpointRemoved(object o, EventArgs args)
		{
			UpdateDisplay();
		}

		protected void OnBreakpointChanged(object o, EventArgs args)
		{
			UpdateDisplay();
		}

		private void OnDebuggerStatusCheck(object s, EventArgs a)
		{
			if (control != null)
			{
				control.Sensitive = !breakpoints.IsReadOnly;
			}
		}

		private void OnRowActivated(object o, RowActivatedArgs args)
		{
			OnBpJumpTo();
		}

		public void RedrawContent()
		{
			UpdateDisplay();
		}

		protected void OnDeleteClicked(object o, EventArgs args)
		{
			OnDeleted();
		}
	}
}
