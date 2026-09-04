using System;
using System.Text;
using GLib;
using Gdk;
using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Components;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Components;
using Pango;
using Xwt.Drawing;

namespace MonoDevelop.Debugger
{
	public class StackTracePad : ScrolledWindow, IPadContent, IDisposable
	{
		private const int IconColumn = 0;

		private const int MethodColumn = 1;

		private const int FileColumn = 2;

		private const int LangColumn = 3;

		private const int AddrColumn = 4;

		private const int ForegroundColumn = 5;

		private const int StyleColumn = 6;

		private const int FrameColumn = 7;

		private const int FrameIndexColumn = 8;

		private const int CanRefreshColumn = 9;

		private readonly CellRendererImage refresh;

		private readonly CommandEntrySet menuSet;

		private readonly PadTreeView tree;

		private readonly ListStore store;

		private IPadWindow window;

		private bool needsUpdate;

		private static Xwt.Drawing.Image pointerImage = Xwt.Drawing.Image.FromResource("stack-pointer-16.png");

		public Widget Control => this;

		public string Id => "MonoDevelop.Debugger.StackTracePad";

		public string DefaultPlacement => "Bottom";

		public StackTracePad()
		{
			base.ShadowType = ShadowType.None;
			ActionCommand cmd = new ActionCommand("StackTracePad.EvaluateMethodParams", GettextCatalog.GetString("Evaluate Method Parameters"));
			ActionCommand cmd2 = new ActionCommand("StackTracePad.ActivateFrame", GettextCatalog.GetString("Activate Stack Frame"));
			menuSet = new CommandEntrySet();
			menuSet.Add(cmd);
			menuSet.Add(cmd2);
			menuSet.AddSeparator();
			menuSet.AddItem(EditCommands.SelectAll);
			menuSet.AddItem(EditCommands.Copy);
			store = new ListStore(typeof(bool), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(Pango.Style), typeof(object), typeof(int), typeof(bool));
			tree = new PadTreeView(store);
			tree.RulesHint = true;
			tree.HeadersVisible = true;
			tree.Selection.Mode = SelectionMode.Multiple;
			tree.SearchEqualFunc = Search;
			tree.EnableSearch = true;
			tree.SearchColumn = 1;
			tree.ButtonPressEvent += HandleButtonPressEvent;
			tree.DoPopupMenu = ShowPopup;
			TreeViewColumn treeViewColumn = new TreeViewColumn();
			CellRendererImage cellRendererImage = new CellRendererImage();
			treeViewColumn.PackStart(cellRendererImage, expand: false);
			cellRendererImage.Image = pointerImage;
			treeViewColumn.AddAttribute(cellRendererImage, "visible", 0);
			tree.AppendColumn(treeViewColumn);
			treeViewColumn = new TreeViewColumn
			{
				Title = GettextCatalog.GetString("Name")
			};
			refresh = new CellRendererImage();
			refresh.Image = ImageService.GetIcon(Gtk.Stock.Refresh).WithSize(12.0, 12.0);
			treeViewColumn.PackStart(refresh, expand: false);
			treeViewColumn.AddAttribute(refresh, "visible", 9);
			treeViewColumn.PackStart(tree.TextRenderer, expand: true);
			treeViewColumn.AddAttribute(tree.TextRenderer, "text", 1);
			treeViewColumn.AddAttribute(tree.TextRenderer, "foreground", 5);
			treeViewColumn.AddAttribute(tree.TextRenderer, "style", 6);
			treeViewColumn.Resizable = true;
			treeViewColumn.Alignment = 0f;
			tree.AppendColumn(treeViewColumn);
			treeViewColumn = new TreeViewColumn();
			treeViewColumn.Title = GettextCatalog.GetString("File");
			treeViewColumn.PackStart(tree.TextRenderer, expand: false);
			treeViewColumn.AddAttribute(tree.TextRenderer, "text", 2);
			treeViewColumn.AddAttribute(tree.TextRenderer, "foreground", 5);
			tree.AppendColumn(treeViewColumn);
			treeViewColumn = new TreeViewColumn();
			treeViewColumn.Title = GettextCatalog.GetString("Language");
			treeViewColumn.PackStart(tree.TextRenderer, expand: false);
			treeViewColumn.AddAttribute(tree.TextRenderer, "text", 3);
			treeViewColumn.AddAttribute(tree.TextRenderer, "foreground", 5);
			tree.AppendColumn(treeViewColumn);
			treeViewColumn = new TreeViewColumn();
			treeViewColumn.Title = GettextCatalog.GetString("Address");
			treeViewColumn.PackStart(tree.TextRenderer, expand: false);
			treeViewColumn.AddAttribute(tree.TextRenderer, "text", 4);
			treeViewColumn.AddAttribute(tree.TextRenderer, "foreground", 5);
			tree.AppendColumn(treeViewColumn);
			Add(tree);
			ShowAll();
			UpdateDisplay();
			DebuggingService.CallStackChanged += OnClassStackChanged;
			DebuggingService.CurrentFrameChanged += OnFrameChanged;
			DebuggingService.StoppedEvent += OnDebuggingServiceStopped;
			tree.RowActivated += OnRowActivated;
		}

		private void OnDebuggingServiceStopped(object sender, EventArgs e)
		{
			if (store != null)
			{
				store.Clear();
			}
		}

		private static bool Search(TreeModel model, int column, string key, TreeIter iter)
		{
			string text = (string)model.GetValue(iter, column);
			return !text.Contains(key);
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

		private static string EvaluateMethodName(StackFrame frame, EvaluationOptions options)
		{
			StringBuilder stringBuilder = new StringBuilder(frame.SourceLocation.MethodName);
			ObjectValue[] parameters = frame.GetParameters(options);
			if (parameters.Length != 0 || !frame.SourceLocation.MethodName.StartsWith("[", StringComparison.Ordinal))
			{
				stringBuilder.Append(" (");
				for (int i = 0; i < parameters.Length; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(parameters[i].Name).Append("=").Append(parameters[i].Value);
				}
				stringBuilder.Append(")");
			}
			return stringBuilder.ToString();
		}

		private void Update()
		{
			if (tree.IsRealized)
			{
				tree.ScrollToPoint(0, 0);
			}
			needsUpdate = false;
			store.Clear();
			if (!DebuggingService.IsPaused)
			{
				return;
			}
			EvaluationOptions evaluationOptions = DebuggingService.DebuggerSession.Options.EvaluationOptions;
			Backtrace currentCallStack = DebuggingService.CurrentCallStack;
			for (int i = 0; i < currentCallStack.FrameCount; i++)
			{
				bool flag = i == DebuggingService.CurrentFrameIndex;
				StackFrame frame = currentCallStack.GetFrame(i);
				if (frame.IsDebuggerHidden)
				{
					continue;
				}
				string text = EvaluateMethodName(frame, evaluationOptions);
				string text2;
				if (!string.IsNullOrEmpty(frame.SourceLocation.FileName))
				{
					text2 = frame.SourceLocation.FileName;
					if (frame.SourceLocation.Line != -1)
					{
						text2 = text2 + ":" + frame.SourceLocation.Line;
					}
				}
				else
				{
					text2 = string.Empty;
				}
				Pango.Style style = (frame.IsExternalCode ? Pango.Style.Italic : Pango.Style.Normal);
				store.AppendValues(flag, text, text2, frame.Language, "0x" + frame.Address.ToString("x"), null, style, frame, i, !evaluationOptions.AllowDisplayStringEvaluation);
			}
		}

		private bool GetCellAtPos(int x, int y, out TreePath path, out TreeViewColumn col, out CellRenderer cellRenderer)
		{
			if (tree.GetPathAtPos(x, y, out path, out col, out var cell_x, out var _))
			{
				tree.GetCellArea(path, col);
				CellRenderer[] cellRenderers = col.CellRenderers;
				foreach (CellRenderer cellRenderer2 in cellRenderers)
				{
					col.CellGetPosition(cellRenderer2, out var start_pos, out var width);
					if (cellRenderer2.Visible && cell_x >= start_pos && cell_x < start_pos + width)
					{
						cellRenderer = cellRenderer2;
						return true;
					}
				}
			}
			cellRenderer = null;
			return false;
		}

		[ConnectBefore]
		private void HandleButtonPressEvent(object sender, ButtonPressEventArgs args)
		{
			if (args.Event.Button == 1 && GetCellAtPos((int)args.Event.X, (int)args.Event.Y, out var path, out var _, out var cellRenderer) && store.GetIter(out var iter, path) && cellRenderer == refresh)
			{
				EvaluationOptions evaluationOptions = DebuggingService.DebuggerSession.Options.EvaluationOptions.Clone();
				evaluationOptions.AllowMethodEvaluation = true;
				evaluationOptions.AllowToStringCalls = true;
				StackFrame frame = (StackFrame)store.GetValue(iter, 7);
				string value = EvaluateMethodName(frame, evaluationOptions);
				store.SetValue(iter, 1, value);
				store.SetValue(iter, 9, value: false);
			}
		}

		public void UpdateCurrentFrame()
		{
			if (!store.GetIterFirst(out var iter))
			{
				return;
			}
			do
			{
				int num = (int)store.GetValue(iter, 8);
				if (num == DebuggingService.CurrentFrameIndex)
				{
					store.SetValue(iter, 0, value: true);
				}
				else
				{
					store.SetValue(iter, 0, value: false);
				}
			}
			while (store.IterNext(ref iter));
		}

		protected void OnFrameChanged(object o, EventArgs args)
		{
			UpdateCurrentFrame();
		}

		protected void OnClassStackChanged(object o, EventArgs args)
		{
			UpdateDisplay();
		}

		private void OnRowActivated(object o, RowActivatedArgs args)
		{
			ActivateFrame();
		}

		public void RedrawContent()
		{
			UpdateDisplay();
		}

		private void ShowPopup(EventButton evt)
		{
			IdeApp.CommandService.ShowContextMenu(tree, evt, menuSet, tree);
		}

		[CommandHandler("StackTracePad.EvaluateMethodParams")]
		private void EvaluateMethodParams()
		{
			if (!store.GetIterFirst(out var iter))
			{
				return;
			}
			EvaluationOptions evaluationOptions = DebuggingService.DebuggerSession.Options.EvaluationOptions.Clone();
			evaluationOptions.AllowMethodEvaluation = true;
			evaluationOptions.AllowToStringCalls = true;
			evaluationOptions.AllowTargetInvoke = true;
			do
			{
				if ((bool)store.GetValue(iter, 9))
				{
					StackFrame frame = (StackFrame)store.GetValue(iter, 7);
					string value = EvaluateMethodName(frame, evaluationOptions);
					store.SetValue(iter, 1, value);
					store.SetValue(iter, 9, value: false);
				}
			}
			while (store.IterNext(ref iter));
		}

		[CommandHandler("StackTracePad.ActivateFrame")]
		private void ActivateFrame()
		{
			TreePath[] selectedRows = tree.Selection.GetSelectedRows();
			if (selectedRows.Length > 0 && store.GetIter(out var iter, selectedRows[0]))
			{
				DebuggingService.CurrentFrameIndex = (int)store.GetValue(iter, 8);
			}
		}

		[CommandHandler(EditCommands.SelectAll)]
		internal void OnSelectAll()
		{
			tree.Selection.SelectAll();
		}

		[CommandHandler(EditCommands.Copy)]
		internal void OnCopy()
		{
			StringBuilder stringBuilder = new StringBuilder();
			TreePath[] selectedRows = tree.Selection.GetSelectedRows(out var model);
			foreach (TreePath path in selectedRows)
			{
				if (model.GetIter(out var iter, path))
				{
					string arg = (string)model.GetValue(iter, 1);
					string arg2 = (string)model.GetValue(iter, 2);
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append('\n');
					}
					stringBuilder.AppendFormat("{0} in {1}", arg, arg2);
				}
			}
			Clipboard clipboard = Clipboard.Get(Atom.Intern("CLIPBOARD", only_if_exists: false));
			clipboard.Text = stringBuilder.ToString();
			clipboard = Clipboard.Get(Atom.Intern("PRIMARY", only_if_exists: false));
			clipboard.Text = stringBuilder.ToString();
		}

		protected override void OnDestroyed()
		{
			DebuggingService.CallStackChanged -= OnClassStackChanged;
			DebuggingService.CurrentFrameChanged -= OnFrameChanged;
			DebuggingService.StoppedEvent -= OnDebuggingServiceStopped;
			base.OnDestroyed();
		}
	}
}
