using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cairo;
using GLib;
using Gdk;
using Gtk;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Refactoring;
using ICSharpCode.NRefactory.Semantics;
using Mono.TextEditor;
using MonoDevelop.AnalysisCore.Fixes;
using MonoDevelop.CodeIssues;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Debugger;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;
using MonoDevelop.Refactoring;
using MonoDevelop.SourceEditor.QuickTasks;
using Pango;

namespace MonoDevelop.CodeActions
{
	internal class CodeActionEditorExtension : TextEditorExtension
	{
		private class FixMenuEntry
		{
			public static readonly FixMenuEntry Separator = new FixMenuEntry("-", null);

			public readonly string Label;

			public readonly System.Action Action;

			public FixMenuEntry(string label, System.Action action)
			{
				Label = label;
				Action = action;
			}
		}

		private class FixMenuDescriptor : FixMenuEntry
		{
			private readonly List<FixMenuEntry> items = new List<FixMenuEntry>();

			public IReadOnlyList<FixMenuEntry> Items => items;

			public object MotionNotifyEvent { get; set; }

			public FixMenuDescriptor()
				: base(null, null)
			{
			}

			public FixMenuDescriptor(string label)
				: base(label, null)
			{
			}

			public void Add(FixMenuEntry entry)
			{
				items.Add(entry);
			}
		}

		private class ContextActionRunner
		{
			private CodeAction act;

			private Document document;

			private TextLocation loc;

			public ContextActionRunner(CodeAction act, Document document, TextLocation loc)
			{
				this.act = act;
				this.document = document;
				this.loc = loc;
			}

			public void Run(object sender, EventArgs e)
			{
				IRefactoringContext context = document.ParsedDocument.CreateRefactoringContext(document, CancellationToken.None);
				RefactoringService.ApplyFix(act, context);
			}

			public void BatchRun(object sender, EventArgs e)
			{
				act.BatchRun(document, loc);
			}
		}

		private class SmartTagMarker : TextSegmentMarker, IActionTextLineMarker
		{
			private const double tagMarkerWidth = 8.0;

			private const double tagMarkerHeight = 2.0;

			private CodeActionEditorExtension codeActionEditorExtension;

			internal List<CodeAction> fixes;

			private DocumentLocation loc;

			public SmartTagMarker(int offset, CodeActionEditorExtension codeActionEditorExtension, List<CodeAction> fixes, DocumentLocation loc)
				: base(offset, 0)
			{
				this.codeActionEditorExtension = codeActionEditorExtension;
				this.fixes = fixes;
				this.loc = loc;
			}

			public SmartTagMarker(int offset)
				: base(offset, 0)
			{
			}

			public override void Draw(TextEditor editor, Cairo.Context cr, Pango.Layout layout, bool selected, int startOffset, int endOffset, double y, double startXPos, double endXPos)
			{
				DocumentLine line = editor.GetLine(loc.Line);
				double d = editor.ColumnToX(line, loc.Column) - editor.HAdjustment.Value + editor.TextViewMargin.XOffset + (double)editor.TextViewMargin.TextStartPosition;
				cr.Rectangle(Math.Floor(d) + 0.5, Math.Floor(y) + 0.5 + ((line == editor.GetLineByOffset(startOffset)) ? (editor.LineHeight - 2.0 - 1.0) : 0.0), 8.0 * cr.LineWidth, 2.0 * cr.LineWidth);
				if (HslColor.Brightness(editor.ColorStyle.PlainText.Background) < 0.5)
				{
					cr.SetSourceRGBA(0.8, 0.8, 1.0, 0.9);
				}
				else
				{
					cr.SetSourceRGBA(0.2, 0.2, 1.0, 0.9);
				}
				cr.Stroke();
			}

			bool IActionTextLineMarker.MousePressed(TextEditor editor, MarginMouseEventArgs args)
			{
				return false;
			}

			void IActionTextLineMarker.MouseHover(TextEditor editor, MarginMouseEventArgs args, TextLineMarkerHoverResult result)
			{
				if (args.Button != 0)
				{
					return;
				}
				DocumentLine line = editor.GetLine(loc.Line);
				if (line != null)
				{
					double num = editor.ColumnToX(line, loc.Column) - editor.HAdjustment.Value + (double)editor.TextViewMargin.TextStartPosition;
					editor.LineToY(line.LineNumber + 1);
					_ = editor.VAdjustment.Value;
					if (args.X - num >= -8.0 * editor.Options.Zoom && args.X - num < 16.0 * editor.Options.Zoom)
					{
						result.Cursor = null;
						Popup();
					}
					else
					{
						codeActionEditorExtension.CancelSmartTagPopupTimeout();
					}
				}
			}

			public void Popup()
			{
				codeActionEditorExtension.smartTagPopupTimeoutId = GLib.Timeout.Add(250u, delegate
				{
					codeActionEditorExtension.PopupQuickFixMenu(null, delegate(FixMenuDescriptor menu)
					{
						codeActionEditorExtension.codeActionMenu = menu;
					});
					codeActionEditorExtension.smartTagPopupTimeoutId = 0u;
					return false;
				});
			}
		}

		private const int menuTimeout = 250;

		private uint quickFixTimeout;

		private uint smartTagPopupTimeoutId;

		private uint menuCloseTimeoutId;

		private FixMenuDescriptor codeActionMenu;

		private static readonly Dictionary<string, int> CodeActionUsages;

		private CancellationTokenSource quickFixCancellationTokenSource;

		private SmartTagMarker currentSmartTag;

		private DocumentLocation currentSmartTagBegin;

		private static readonly List<CodeAction> emptyList;

		public IEnumerable<CodeAction> Fixes { get; private set; }

		static CodeActionEditorExtension()
		{
			CodeActionUsages = new Dictionary<string, int>();
			emptyList = new List<CodeAction>();
			Properties properties = PropertyService.Get("CodeActionUsages", new Properties());
			foreach (string key in properties.Keys)
			{
				CodeActionUsages[key] = properties.Get<int>(key);
			}
		}

		private void CancelSmartTagPopupTimeout()
		{
			if (smartTagPopupTimeoutId != 0)
			{
				Source.Remove(smartTagPopupTimeoutId);
				smartTagPopupTimeoutId = 0u;
			}
		}

		private void CancelMenuCloseTimer()
		{
			if (menuCloseTimeoutId != 0)
			{
				Source.Remove(menuCloseTimeoutId);
				menuCloseTimeoutId = 0u;
			}
		}

		private void RemoveWidget()
		{
			if (currentSmartTag != null)
			{
				document.Editor.Document.RemoveMarker(currentSmartTag);
				currentSmartTag = null;
				currentSmartTagBegin = DocumentLocation.Empty;
			}
			CancelSmartTagPopupTimeout();
		}

		public override void Dispose()
		{
			CancelMenuCloseTimer();
			CancelQuickFixTimer();
			document.Editor.SelectionChanged -= HandleSelectionChanged;
			document.DocumentParsed -= HandleDocumentDocumentParsed;
			document.Editor.Parent.BeginHover -= HandleBeginHover;
			RemoveWidget();
			Fixes = null;
			base.Dispose();
		}

		private static void ConfirmUsage(string id)
		{
			if (!CodeActionUsages.ContainsKey(id))
			{
				CodeActionUsages[id] = 1;
			}
			else
			{
				CodeActionUsages[id]++;
			}
			Properties properties = PropertyService.Get("CodeActionUsages", new Properties());
			properties.Set(id, CodeActionUsages[id]);
		}

		internal static int GetUsage(string id)
		{
			if (!CodeActionUsages.TryGetValue(id, out var value))
			{
				return 0;
			}
			return value;
		}

		public void CancelQuickFixTimer()
		{
			if (quickFixCancellationTokenSource != null)
			{
				quickFixCancellationTokenSource.Cancel();
			}
			if (quickFixTimeout != 0)
			{
				Source.Remove(quickFixTimeout);
				quickFixTimeout = 0u;
			}
		}

		public override void CursorPositionChanged()
		{
			CancelQuickFixTimer();
			if ((bool)QuickTaskStrip.EnableFancyFeatures && base.Document.ParsedDocument != null && !DebuggingService.IsDebugging)
			{
				quickFixCancellationTokenSource = new CancellationTokenSource();
				CancellationToken token = quickFixCancellationTokenSource.Token;
				quickFixTimeout = GLib.Timeout.Add(100u, delegate
				{
					DocumentLocation loc = base.Document.Editor.Caret.Location;
					RefactoringService.QueueQuickFixAnalysis(base.Document, loc, token, delegate(List<CodeAction> fixes)
					{
						if (!fixes.Any())
						{
							if (!ResolveCommandHandler.ResolveAt(document, out var resolveResult, out var node, token))
							{
								if (currentSmartTag != null)
								{
									Application.Invoke(delegate
									{
										RemoveWidget();
									});
								}
								return;
							}
							List<ResolveCommandHandler.PossibleNamespace> possibleNamespaces = ResolveCommandHandler.GetPossibleNamespaces(document, node, ref resolveResult);
							if (!possibleNamespaces.Any())
							{
								if (currentSmartTag != null)
								{
									Application.Invoke(delegate
									{
										RemoveWidget();
									});
								}
								return;
							}
						}
						Application.Invoke(delegate
						{
							if (!token.IsCancellationRequested)
							{
								CreateSmartTag(fixes, loc);
							}
						});
					});
					quickFixTimeout = 0u;
					return false;
				});
			}
			else
			{
				RemoveWidget();
			}
			base.CursorPositionChanged();
		}

		internal static bool IsAnalysisOrErrorFix(CodeAction act)
		{
			if (!(act is AnalysisContextActionProvider.AnalysisCodeAction))
			{
				return act.Severity == Severity.Error;
			}
			return true;
		}

		private void PopupQuickFixMenu(EventButton evt, Action<FixMenuDescriptor> menuAction)
		{
			FixMenuDescriptor fixMenuDescriptor = new FixMenuDescriptor();
			FixMenuDescriptor fixMenuDescriptor2 = fixMenuDescriptor;
			int items = 0;
			if (ResolveCommandHandler.ResolveAt(document, out var resolveResult, out var node))
			{
				List<ResolveCommandHandler.PossibleNamespace> possibleNamespaces = ResolveCommandHandler.GetPossibleNamespaces(document, node, ref resolveResult);
				foreach (ResolveCommandHandler.PossibleNamespace t in possibleNamespaces.Where((ResolveCommandHandler.PossibleNamespace tp) => tp.OnlyAddReference))
				{
					string importText = t.GetImportText();
					System.Action action = delegate
					{
						new ResolveCommandHandler.AddImport(document, resolveResult, null, t.Reference, addUsing: true, node).Run();
					};
					fixMenuDescriptor.Add(new FixMenuEntry(importText, action));
					items++;
				}
				if (!(resolveResult is AmbiguousTypeResolveResult))
				{
					foreach (ResolveCommandHandler.PossibleNamespace item in possibleNamespaces.Where((ResolveCommandHandler.PossibleNamespace tp) => tp.IsAccessibleWithGlobalUsing))
					{
						string ns = item.Namespace;
						ProjectReference reference = item.Reference;
						fixMenuDescriptor.Add(new FixMenuEntry(item.GetImportText(), delegate
						{
							new ResolveCommandHandler.AddImport(document, resolveResult, ns, reference, addUsing: true, node).Run();
						}));
						items++;
					}
				}
				if (!(resolveResult is UnknownMemberResolveResult))
				{
					foreach (ResolveCommandHandler.PossibleNamespace item2 in possibleNamespaces)
					{
						string ns2 = item2.Namespace;
						ProjectReference reference2 = item2.Reference;
						fixMenuDescriptor.Add(new FixMenuEntry(item2.GetInsertNamespaceText(document.Editor.GetTextBetween(node.StartLocation, node.EndLocation)), delegate
						{
							new ResolveCommandHandler.AddImport(document, resolveResult, ns2, reference2, addUsing: false, node).Run();
						}));
						items++;
					}
				}
				if (fixMenuDescriptor.Items.Any() && Fixes.Any())
				{
					fixMenuDescriptor2 = new FixMenuDescriptor(GettextCatalog.GetString("Quick Fixes"));
					fixMenuDescriptor.Add(fixMenuDescriptor2);
					items++;
				}
			}
			PopulateFixes(fixMenuDescriptor2, ref items);
			if (items != 0)
			{
				document.Editor.SuppressTooltips = true;
				document.Editor.Parent.HideTooltip();
				menuAction?.Invoke(fixMenuDescriptor);
				TextEditor parent = document.Editor.Parent;
				Cairo.Point point = parent.LocationToPoint(currentSmartTagBegin);
				ShowFixesMenu(evt: new Gdk.Rectangle(point.X + parent.Allocation.X, point.Y + (int)document.Editor.LineHeight + parent.Allocation.Y, 0, 0), parent: document.Editor.Parent, entrySet: fixMenuDescriptor);
			}
		}

		private bool ShowFixesMenu(Widget parent, Gdk.Rectangle evt, FixMenuDescriptor entrySet)
		{
			if (parent == null || parent.GdkWindow == null)
			{
				return true;
			}
			try
			{
				Menu menu = CreateGtkMenu(entrySet);
				menu.Events |= EventMask.AllEventsMask;
				menu.SelectFirst(search_sensitive: true);
				menu.Hidden += delegate
				{
					document.Editor.SuppressTooltips = false;
				};
				menu.ShowAll();
				menu.SelectFirst(search_sensitive: true);
				menu.MotionNotifyEvent += delegate(object o, MotionNotifyEventArgs args)
				{
					if (args.Event.Window == base.Editor.Parent.TextArea.GdkWindow)
					{
						StartMenuCloseTimer();
					}
					else
					{
						CancelMenuCloseTimer();
					}
				};
				GtkWorkarounds.ShowContextMenu(menu, parent, null, evt);
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Error while context menu popup.", ex);
			}
			return true;
		}

		private static Menu CreateGtkMenu(FixMenuDescriptor entrySet)
		{
			Menu menu = new Menu();
			foreach (FixMenuEntry item in entrySet.Items)
			{
				if (item == FixMenuEntry.Separator)
				{
					menu.Add(new SeparatorMenuItem());
					continue;
				}
				if (item is FixMenuDescriptor entrySet2)
				{
					MenuItem menuItem = new MenuItem(item.Label);
					menuItem.Submenu = CreateGtkMenu(entrySet2);
					menu.Add(menuItem);
					continue;
				}
				MenuItem menuItem2 = new MenuItem(item.Label);
				menuItem2.Activated += delegate
				{
					item.Action();
				};
				menu.Add(menuItem2);
			}
			return menu;
		}

		private void PopulateFixes(FixMenuDescriptor menu, ref int items)
		{
			int num = 1;
			bool flag = false;
			bool flag2 = false;
			List<string> list = new List<string>();
			foreach (CodeAction fix_ in Fixes.OrderByDescending((CodeAction i) => Tuple.Create(IsAnalysisOrErrorFix(i), (int)i.Severity, GetUsage(i.IdString))))
			{
				Func<string, bool> predicate = (string f) => fix_.IdString.IndexOf(f, StringComparison.Ordinal) >= 0;
				if (!list.Any(predicate))
				{
					list.Add(fix_.IdString);
					if (IsAnalysisOrErrorFix(fix_))
					{
						flag = true;
					}
					if (!flag2 && flag && !IsAnalysisOrErrorFix(fix_))
					{
						menu.Add(FixMenuEntry.Separator);
						flag2 = true;
					}
					CodeAction fix = fix_;
					string text = fix.Title.Replace("_", "__");
					string label = ((num <= 10) ? ("_" + num++ % 10 + " " + text) : ("  " + text));
					FixMenuEntry entry = new FixMenuEntry(label, delegate
					{
						new ContextActionRunner(fix, document, currentSmartTagBegin).Run(null, EventArgs.Empty);
						ConfirmUsage(fix.IdString);
					});
					menu.Add(entry);
					items++;
				}
			}
			bool flag3 = true;
			IEnumerable<IGrouping<BaseCodeIssueProvider, AnalysisContextActionProvider.AnalysisCodeAction>> enumerable = from f in Fixes.OfType<AnalysisContextActionProvider.AnalysisCodeAction>()
				where f.Result is InspectorResults
				group f by ((InspectorResults)f.Result).Inspector;
			foreach (IGrouping<BaseCodeIssueProvider, AnalysisContextActionProvider.AnalysisCodeAction> item in enumerable)
			{
				IGrouping<BaseCodeIssueProvider, AnalysisContextActionProvider.AnalysisCodeAction> grouping = item;
				AnalysisContextActionProvider.AnalysisCodeAction arbitraryFixInGroup = grouping.First();
				InspectorResults inspectorResults = (InspectorResults)arbitraryFixInGroup.Result;
				if (flag3)
				{
					menu.Add(FixMenuEntry.Separator);
					flag3 = false;
				}
				FixMenuDescriptor fixMenuDescriptor = new FixMenuDescriptor();
				foreach (AnalysisContextActionProvider.AnalysisCodeAction item2 in grouping)
				{
					AnalysisContextActionProvider.AnalysisCodeAction analysisFix = item2;
					if (analysisFix.SupportsBatchRunning)
					{
						FixMenuEntry entry2 = new FixMenuEntry(string.Format(GettextCatalog.GetString("Apply in file: {0}"), analysisFix.Title), delegate
						{
							ConfirmUsage(analysisFix.IdString);
							new ContextActionRunner(analysisFix, document, currentSmartTagBegin).BatchRun(null, EventArgs.Empty);
						});
						fixMenuDescriptor.Add(entry2);
						fixMenuDescriptor.Add(FixMenuEntry.Separator);
					}
				}
				BaseCodeIssueProvider inspector = inspectorResults.Inspector;
				if (inspector.CanSuppressWithAttribute)
				{
					FixMenuEntry entry3 = new FixMenuEntry(GettextCatalog.GetString("_Suppress with attribute"), delegate
					{
						inspector.SuppressWithAttribute(document, arbitraryFixInGroup.DocumentRegion);
					});
					fixMenuDescriptor.Add(entry3);
				}
				if (inspector.CanDisableWithPragma)
				{
					FixMenuEntry entry4 = new FixMenuEntry(GettextCatalog.GetString("_Suppress with #pragma"), delegate
					{
						inspector.DisableWithPragma(document, arbitraryFixInGroup.DocumentRegion);
					});
					fixMenuDescriptor.Add(entry4);
				}
				if (inspector.CanDisableOnce)
				{
					FixMenuEntry entry5 = new FixMenuEntry(GettextCatalog.GetString("_Disable Once"), delegate
					{
						inspector.DisableOnce(document, arbitraryFixInGroup.DocumentRegion);
					});
					fixMenuDescriptor.Add(entry5);
				}
				if (inspector.CanDisableAndRestore)
				{
					FixMenuEntry entry6 = new FixMenuEntry(GettextCatalog.GetString("Disable _and Restore"), delegate
					{
						inspector.DisableAndRestore(document, arbitraryFixInGroup.DocumentRegion);
					});
					fixMenuDescriptor.Add(entry6);
				}
				string label2 = GettextCatalog.GetString("_Options for \"{0}\"", InspectorResults.GetTitle(inspectorResults.Inspector));
				FixMenuDescriptor fixMenuDescriptor2 = new FixMenuDescriptor(label2);
				FixMenuEntry entry7 = new FixMenuEntry(GettextCatalog.GetString("_Configure Rule"), delegate
				{
					arbitraryFixInGroup.ShowOptions(null, EventArgs.Empty);
				});
				fixMenuDescriptor2.Add(entry7);
				menu.Add(fixMenuDescriptor2);
				items++;
			}
		}

		private void CreateSmartTag(List<CodeAction> fixes, DocumentLocation loc)
		{
			Fixes = fixes;
			if (!QuickTaskStrip.EnableFancyFeatures)
			{
				RemoveWidget();
				return;
			}
			TextEditorData editor = document.Editor;
			if (editor == null || editor.Parent == null || !editor.Parent.IsRealized)
			{
				RemoveWidget();
				return;
			}
			if (document.ParsedDocument == null || document.ParsedDocument.IsInvalid)
			{
				RemoveWidget();
				return;
			}
			TextEditor parent = editor.Parent;
			if (parent == null)
			{
				RemoveWidget();
				return;
			}
			bool flag = true;
			DocumentLocation documentLocation = loc;
			foreach (CodeAction fix in fixes)
			{
				if (!fix.DocumentRegion.IsEmpty)
				{
					if (flag || loc < fix.DocumentRegion.Begin)
					{
						documentLocation = fix.DocumentRegion.Begin;
					}
					flag = false;
				}
			}
			if (documentLocation.Line != loc.Line)
			{
				documentLocation = new DocumentLocation(loc.Line, 1);
			}
			if (flag)
			{
				int num;
				for (num = document.Editor.LocationToOffset(documentLocation); num > 0; num--)
				{
					char charAt = document.Editor.GetCharAt(num - 1);
					if (!char.IsLetterOrDigit(charAt) && charAt != '_')
					{
						break;
					}
				}
				documentLocation = document.Editor.OffsetToLocation(num);
			}
			if (currentSmartTag != null && currentSmartTagBegin == documentLocation)
			{
				currentSmartTag.fixes = fixes;
				return;
			}
			RemoveWidget();
			currentSmartTagBegin = documentLocation;
			DocumentLine line = document.Editor.GetLine(documentLocation.Line);
			currentSmartTag = new SmartTagMarker((line.NextLine ?? line).Offset, this, fixes, documentLocation);
			document.Editor.Document.AddMarker(currentSmartTag);
		}

		public override void Initialize()
		{
			base.Initialize();
			document.DocumentParsed += HandleDocumentDocumentParsed;
			document.Editor.SelectionChanged += HandleSelectionChanged;
			document.Editor.Parent.BeginHover += HandleBeginHover;
		}

		private void HandleBeginHover(object sender, EventArgs e)
		{
			CancelSmartTagPopupTimeout();
			CancelMenuCloseTimer();
		}

		private void StartMenuCloseTimer()
		{
			CancelMenuCloseTimer();
			menuCloseTimeoutId = GLib.Timeout.Add(250u, delegate
			{
				menuCloseTimeoutId = 0u;
				return false;
			});
		}

		private void HandleSelectionChanged(object sender, EventArgs e)
		{
			CursorPositionChanged();
		}

		private void HandleDocumentDocumentParsed(object sender, EventArgs e)
		{
			CursorPositionChanged();
		}

		[CommandUpdateHandler(RefactoryCommands.QuickFix)]
		public void UpdateQuickFixCommand(CommandInfo ci)
		{
			if ((bool)QuickTaskStrip.EnableFancyFeatures)
			{
				ci.Enabled = currentSmartTag != null;
			}
			else
			{
				ci.Enabled = true;
			}
		}

		[CommandHandler(RefactoryCommands.QuickFix)]
		private void OnQuickFixCommand()
		{
			if (!QuickTaskStrip.EnableFancyFeatures)
			{
				Fixes = RefactoringService.GetValidActions(base.Document, base.Document.Editor.Caret.Location);
				currentSmartTagBegin = base.Document.Editor.Caret.Location;
				PopupQuickFixMenu(null, null);
			}
			else if (currentSmartTag != null)
			{
				currentSmartTag.Popup();
			}
		}

		internal List<CodeAction> GetCurrentFixes()
		{
			if (currentSmartTag != null)
			{
				return currentSmartTag.fixes;
			}
			return emptyList;
		}
	}
}
