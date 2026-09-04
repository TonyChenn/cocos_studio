using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GLib;
using Gdk;
using Gtk;
using ICSharpCode.NRefactory.Refactoring;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.TextEditor;
using Mono.TextEditor.Highlighting;
using Mono.TextEditor.Theatrics;
using MonoDevelop.Components;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.FindInFiles;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;
using MonoDevelop.SourceEditor.QuickTasks;
using Pango;

namespace MonoDevelop.SourceEditor
{
	internal class SourceEditorWidget : ITextEditorExtension, IDisposable, IQuickTaskProvider
	{
		public class Border : DrawingArea
		{
			protected override bool OnExposeEvent(EventExpose evnt)
			{
				evnt.Window.DrawRectangle(base.Style.DarkGC(base.State), filled: true, evnt.Area);
				return true;
			}
		}

		private class DecoratedScrolledWindow : HBox
		{
			private SourceEditorWidget parent;

			private ScrolledWindow scrolledWindow;

			private QuickTaskStrip strip;

			private bool suppressScrollbar;

			public Adjustment Hadjustment => scrolledWindow.Hadjustment;

			public Adjustment Vadjustment => scrolledWindow.Vadjustment;

			public QuickTaskStrip Strip => strip;

			public DecoratedScrolledWindow(SourceEditorWidget parent)
			{
				this.parent = parent;
				strip = new QuickTaskStrip();
				scrolledWindow = new CompactScrolledWindow();
				scrolledWindow.ButtonPressEvent += PrepareEvent;
				PackStart(scrolledWindow, expand: true, fill: true, 0u);
				strip.VAdjustment = scrolledWindow.Vadjustment;
				PackEnd(strip, expand: false, fill: true, 0u);
				parent.quickTaskProvider.ForEach(AddQuickTaskProvider);
				QuickTaskStrip.EnableFancyFeatures.Changed += FancyFeaturesChanged;
				FancyFeaturesChanged(null, null);
			}

			private void FancyFeaturesChanged(object sender, EventArgs e)
			{
				if (QuickTaskStrip.MergeScrollBarAndQuickTasks)
				{
					if ((bool)QuickTaskStrip.EnableFancyFeatures)
					{
						GtkWorkarounds.SetOverlayScrollbarPolicy(scrolledWindow, PolicyType.Automatic, PolicyType.Never);
						SetSuppressScrollbar(value: true);
					}
					else
					{
						GtkWorkarounds.SetOverlayScrollbarPolicy(scrolledWindow, PolicyType.Automatic, PolicyType.Automatic);
						SetSuppressScrollbar(value: false);
					}
					QueueResize();
				}
			}

			private void SetSuppressScrollbar(bool value)
			{
				if (suppressScrollbar != value)
				{
					suppressScrollbar = value;
					if (suppressScrollbar)
					{
						scrolledWindow.VScrollbar.SizeRequested += SuppressSize;
						scrolledWindow.VScrollbar.ExposeEvent += SuppressExpose;
					}
					else
					{
						scrolledWindow.VScrollbar.SizeRequested -= SuppressSize;
						scrolledWindow.VScrollbar.ExposeEvent -= SuppressExpose;
					}
				}
			}

			[ConnectBefore]
			private static void SuppressExpose(object o, ExposeEventArgs args)
			{
				args.RetVal = true;
			}

			[ConnectBefore]
			private static void SuppressSize(object o, SizeRequestedArgs args)
			{
				args.Requisition = Requisition.Zero;
				args.RetVal = true;
			}

			public void AddQuickTaskProvider(IQuickTaskProvider p)
			{
				p.TasksUpdated += HandleTasksUpdated;
			}

			private void HandleTasksUpdated(object sender, EventArgs e)
			{
				strip.Update((IQuickTaskProvider)sender);
			}

			public void RemoveQuickTaskProvider(IQuickTaskProvider provider)
			{
				if (provider != null)
				{
					provider.TasksUpdated -= HandleTasksUpdated;
				}
			}

			public void AddUsageProvider(IUsageProvider p)
			{
				p.UsagesUpdated += delegate
				{
					strip.Update(p);
				};
			}

			protected override void OnDestroyed()
			{
				if (scrolledWindow.Child != null)
				{
					RemoveEvents();
				}
				SetSuppressScrollbar(value: false);
				QuickTaskStrip.EnableFancyFeatures.Changed -= FancyFeaturesChanged;
				scrolledWindow.ButtonPressEvent -= PrepareEvent;
				base.OnDestroyed();
			}

			private void PrepareEvent(object sender, ButtonPressEventArgs args)
			{
				args.RetVal = true;
			}

			public void SetTextEditor(TextEditor container)
			{
				scrolledWindow.Child = container;
				strip.TextEditor = container;
				container.Caret.ModeChanged += parent.UpdateLineColOnEventHandler;
				container.Caret.PositionChanged += parent.CaretPositionChanged;
				container.SelectionChanged += parent.UpdateLineColOnEventHandler;
			}

			private void OptionsChanged(object sender, EventArgs e)
			{
				TextEditor textEditor = (TextEditor)sender;
				scrolledWindow.ModifyBg(StateType.Normal, (HslColor)textEditor.ColorStyle.PlainText.Background);
			}

			private void RemoveEvents()
			{
				if (!(scrolledWindow.Child is TextEditor textEditor))
				{
					LoggingService.LogError("can't remove events from text editor container.");
					return;
				}
				textEditor.Caret.ModeChanged -= parent.UpdateLineColOnEventHandler;
				textEditor.Caret.PositionChanged -= parent.CaretPositionChanged;
				textEditor.SelectionChanged -= parent.UpdateLineColOnEventHandler;
			}

			public TextEditor RemoveTextEditor()
			{
				if (!(scrolledWindow.Child is TextEditor textEditor))
				{
					return null;
				}
				RemoveEvents();
				scrolledWindow.Remove(textEditor);
				textEditor.Unparent();
				return textEditor;
			}
		}

		private const uint CHILD_PADDING = 0u;

		private SourceEditorView view;

		private DecoratedScrolledWindow mainsw;

		private TextEditorData textEditorData;

		private bool isDisposed;

		private ParsedDocument parsedDocument;

		private readonly ExtensibleTextEditor textEditor;

		private ExtensibleTextEditor splittedTextEditor;

		private ExtensibleTextEditor lastActiveEditor;

		private List<IQuickTaskProvider> quickTaskProvider = new List<IQuickTaskProvider>();

		private List<IUsageProvider> usageProvider = new List<IUsageProvider>();

		private VBox vbox = new VBox();

		private HashSet<string> symbols = new HashSet<string>();

		private bool reloadSettings;

		private CancellationTokenSource parserInformationUpdateSrc = new CancellationTokenSource();

		private List<ErrorMarker> errors = new List<ErrorMarker>();

		private uint resetTimerId;

		private Paned splitContainer;

		private DecoratedScrolledWindow secondsw;

		private InfoBar messageBar;

		private OverlayMessageWindow messageOverlayWindow;

		private MessageBubbleTextMarker oldExpandedMarker;

		private RoundedFrame searchAndReplaceWidgetFrame;

		private SearchAndReplaceWidget searchAndReplaceWidget;

		private RoundedFrame gotoLineNumberWidgetFrame;

		private GotoLineNumberWidget gotoLineNumberWidget;

		private List<QuickTask> tasks = new List<QuickTask>();

		private ISourceEditorOptions options => textEditor.Options;

		internal QuickTaskStrip QuickTaskStrip => mainsw.Strip;

		public ExtensibleTextEditor TextEditor => lastActiveEditor ?? textEditor;

		public Ambience Ambience
		{
			get
			{
				string fileName = (view.IsUntitled ? view.UntitledName : view.ContentName);
				return AmbienceService.GetAmbienceForFile(fileName);
			}
		}

		ITextEditorExtension ITextEditorExtension.Next => null;

		public bool HasMessageBar => messageBar != null;

		public VBox Vbox => vbox;

		public bool SearchWidgetHasFocus
		{
			get
			{
				if (HasAnyFocusedChild(searchAndReplaceWidget) || HasAnyFocusedChild(gotoLineNumberWidget))
				{
					return true;
				}
				return false;
			}
		}

		public ParsedDocument ParsedDocument
		{
			get
			{
				return parsedDocument;
			}
			set
			{
				SetParsedDocument(value, runInThread: true);
			}
		}

		public bool IsSplitted => splitContainer != null;

		public bool EditorHasFocus => TextEditor.TextArea.HasFocus;

		public SourceEditorView View
		{
			get
			{
				return view;
			}
			set
			{
				view = value;
			}
		}

		internal bool UseIncorrectMarkers { get; set; }

		internal bool HasIncorrectEolMarker
		{
			get
			{
				TextDocument document = Document;
				if (document == null)
				{
					return false;
				}
				if (document.HasLineEndingMismatchOnTextSet)
				{
					return true;
				}
				string detectedEolMarker = DetectedEolMarker;
				if (detectedEolMarker == null)
				{
					return false;
				}
				return detectedEolMarker != textEditor.Options.DefaultEolMarker;
			}
		}

		private string DetectedEolMarker
		{
			get
			{
				if (Document.HasLineEndingMismatchOnTextSet)
				{
					return "?";
				}
				if (textEditor.IsDisposed)
				{
					LoggingService.LogWarning("SourceEditorWidget.cs: HasIncorrectEolMarker was called on disposed source editor widget." + Environment.NewLine + Environment.StackTrace);
					return null;
				}
				DocumentLine line = Document.GetLine(1);
				if (line != null && line.DelimiterLength > 0)
				{
					return Document.GetTextAt(line.Length, line.DelimiterLength);
				}
				return null;
			}
		}

		public TextDocument Document => TextEditor?.Document;

		public IEnumerable<QuickTask> QuickTasks => tasks;

		public event EventHandler TasksUpdated;

		public void AddQuickTaskProvider(IQuickTaskProvider provider)
		{
			quickTaskProvider.Add(provider);
			mainsw.AddQuickTaskProvider(provider);
			if (secondsw != null)
			{
				secondsw.AddQuickTaskProvider(provider);
			}
		}

		public void RemoveQuickTaskProvider(IQuickTaskProvider provider)
		{
			quickTaskProvider.Remove(provider);
			mainsw.RemoveQuickTaskProvider(provider);
			if (secondsw != null)
			{
				secondsw.RemoveQuickTaskProvider(provider);
			}
		}

		public void AddUsageTaskProvider(IUsageProvider provider)
		{
			usageProvider.Add(provider);
			mainsw.AddUsageProvider(provider);
			if (secondsw != null)
			{
				secondsw.AddUsageProvider(provider);
			}
		}

		object ITextEditorExtension.GetExtensionCommandTarget()
		{
			return null;
		}

		void ITextEditorExtension.TextChanged(int startIndex, int endIndex)
		{
		}

		void ITextEditorExtension.CursorPositionChanged()
		{
		}

		bool ITextEditorExtension.KeyPress(Gdk.Key key, char keyChar, ModifierType modifier)
		{
			TextEditor.SimulateKeyPress(key, keyChar, modifier);
			if (key == Gdk.Key.Escape)
			{
				return true;
			}
			return false;
		}

		private static bool HasAnyFocusedChild(Widget widget)
		{
			if (widget == null)
			{
				return false;
			}
			Stack<Widget> stack = new Stack<Widget>();
			stack.Push(widget);
			while (stack.Count > 0)
			{
				Widget widget2 = stack.Pop();
				if (widget2.HasFocus)
				{
					return true;
				}
				if (widget2 is Container container)
				{
					Widget[] children = container.Children;
					foreach (Widget item in children)
					{
						stack.Push(item);
					}
				}
			}
			return false;
		}

		public SourceEditorWidget(SourceEditorView view)
		{
			SourceEditorWidget sourceEditorWidget = this;
			this.view = view;
			vbox.SetSizeRequest(32, 32);
			lastActiveEditor = (textEditor = new ExtensibleTextEditor(view));
			TextArea textArea = textEditor.TextArea;
			FocusInEventHandler value = delegate(object o, FocusInEventArgs s)
			{
				sourceEditorWidget.lastActiveEditor = (ExtensibleTextEditor)((TextArea)o).GetTextEditorData().Parent;
				view.FireCompletionContextChanged();
			};
			textArea.FocusInEvent += value;
			textEditor.TextArea.FocusOutEvent += delegate
			{
				if (splittedTextEditor == null || !splittedTextEditor.TextArea.HasFocus)
				{
					OnLostFocus();
				}
			};
			IdeApp.FocusOut += delegate
			{
				textEditor.TextArea.HideTooltip(checkMouseOver: false);
			};
			mainsw = new DecoratedScrolledWindow(this);
			mainsw.SetTextEditor(textEditor);
			vbox.PackStart(mainsw, expand: true, fill: true, 0u);
			textEditorData = textEditor.GetTextEditorData();
			textEditorData.EditModeChanged += delegate
			{
				KillWidgets();
			};
			ResetFocusChain();
			UpdateLineCol();
			vbox.BorderWidth = 0u;
			vbox.Spacing = 0;
			vbox.Focused += delegate
			{
				UpdateLineCol();
			};
			vbox.Destroyed += delegate
			{
				sourceEditorWidget.isDisposed = true;
				sourceEditorWidget.RemoveErrorUndelinesResetTimerId();
				sourceEditorWidget.StopParseInfoThread();
				sourceEditorWidget.KillWidgets();
				IQuickTaskProvider[] array = sourceEditorWidget.quickTaskProvider.ToArray();
				foreach (IQuickTaskProvider provider in array)
				{
					sourceEditorWidget.RemoveQuickTaskProvider(provider);
				}
				sourceEditorWidget.lastActiveEditor = null;
				sourceEditorWidget.splittedTextEditor = null;
				view = null;
				sourceEditorWidget.parsedDocument = null;
			};
			vbox.ShowAll();
		}

		private void OnLostFocus()
		{
		}

		private void UpdateLineColOnEventHandler(object sender, EventArgs e)
		{
			UpdateLineCol();
		}

		private void ResetFocusChain()
		{
			List<Widget> list = new List<Widget>();
			list.Add(textEditor.TextArea);
			if (searchAndReplaceWidget != null)
			{
				list.Add(searchAndReplaceWidget);
			}
			if (gotoLineNumberWidget != null)
			{
				list.Add(gotoLineNumberWidget);
			}
			vbox.FocusChain = list.ToArray();
		}

		public void Dispose()
		{
			RemoveErrorUndelinesResetTimerId();
		}

		private FoldSegment AddMarker(List<FoldSegment> foldSegments, string text, DomRegion region, FoldingType type)
		{
			TextDocument document = textEditorData.Document;
			if (document == null || region.BeginLine <= 0 || region.EndLine <= 0 || region.BeginLine > document.LineCount || region.EndLine > document.LineCount)
			{
				return null;
			}
			int num = document.LocationToOffset(region.BeginLine, region.BeginColumn);
			int num2 = document.LocationToOffset(region.EndLine, region.EndColumn);
			FoldSegment foldSegment = new FoldSegment(document, text, num, num2 - num, type);
			foldSegments.Add(foldSegment);
			return foldSegment;
		}

		private void HandleParseInformationUpdaterWorkerThreadDoWork(bool firstTime, ParsedDocument parsedDocument, CancellationToken token = default(CancellationToken))
		{
			TextDocument document = Document;
			if (document == null || parsedDocument == null)
			{
				return;
			}
			UpdateErrorUndelines(parsedDocument);
			if (!options.ShowFoldMargin || parsedDocument.HasErrors)
			{
				return;
			}
			try
			{
				List<FoldSegment> list = new List<FoldSegment>();
				bool flag = parsedDocument.Defines.Count != symbols.Count;
				if (!flag)
				{
					foreach (PreProcessorDefine define in parsedDocument.Defines)
					{
						if (token.IsCancellationRequested)
						{
							return;
						}
						if (!symbols.Contains(define.Define))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					symbols.Clear();
					foreach (PreProcessorDefine define2 in parsedDocument.Defines)
					{
						symbols.Add(define2.Define);
					}
				}
				foreach (FoldingRegion folding in parsedDocument.Foldings)
				{
					if (token.IsCancellationRequested)
					{
						return;
					}
					FoldingType type = FoldingType.None;
					bool flag2 = false;
					bool isFolded = false;
					switch (folding.Type)
					{
					case FoldType.Member:
						type = FoldingType.TypeMember;
						break;
					case FoldType.Type:
						type = FoldingType.TypeDefinition;
						break;
					case FoldType.UserRegion:
						type = FoldingType.Region;
						flag2 = options.DefaultRegionsFolding;
						isFolded = true;
						break;
					case FoldType.Comment:
						type = FoldingType.Comment;
						flag2 = options.DefaultCommentFolding;
						isFolded = true;
						break;
					case FoldType.CommentInsideMember:
						type = FoldingType.Comment;
						flag2 = options.DefaultCommentFolding;
						isFolded = false;
						break;
					case FoldType.Undefined:
						flag2 = true;
						isFolded = folding.IsFoldedByDefault;
						break;
					}
					FoldSegment foldSegment = AddMarker(list, folding.Name, folding.Region, type);
					if (foldSegment != null && flag2 && firstTime)
					{
						foldSegment.IsFolded = isFolded;
					}
					else if (foldSegment != null && folding.Region.IsInside(textEditorData.Caret.Line, textEditorData.Caret.Column))
					{
						foldSegment.IsFolded = false;
					}
				}
				document.UpdateFoldSegments(list, startTask: false, useApplicationInvoke: true, token);
				if (!reloadSettings)
				{
					return;
				}
				reloadSettings = false;
				Application.Invoke(delegate
				{
					if (!isDisposed)
					{
						view.LoadSettings();
						mainsw.QueueDraw();
					}
				});
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Unhandled exception in ParseInformationUpdaterWorkerThread", ex);
			}
		}

		internal void UpdateParsedDocument(ParsedDocument document)
		{
			if (!isDisposed && document != null && view != null)
			{
				SetParsedDocument(document, parsedDocument != null);
			}
		}

		internal void SetParsedDocument(ParsedDocument newDocument, bool runInThread)
		{
			parsedDocument = newDocument;
			if (parsedDocument == null)
			{
				return;
			}
			StopParseInfoThread();
			if (runInThread)
			{
				CancellationToken token = parserInformationUpdateSrc.Token;
				Task.Factory.StartNew(delegate
				{
					HandleParseInformationUpdaterWorkerThreadDoWork(firstTime: false, parsedDocument, token);
				});
			}
			else
			{
				HandleParseInformationUpdaterWorkerThreadDoWork(firstTime: true, parsedDocument);
			}
		}

		private void StopParseInfoThread()
		{
			parserInformationUpdateSrc.Cancel();
			parserInformationUpdateSrc = new CancellationTokenSource();
		}

		internal void SetLastActiveEditor(ExtensibleTextEditor editor)
		{
			lastActiveEditor = editor;
		}

		private void RemoveErrorUndelinesResetTimerId()
		{
			if (resetTimerId != 0)
			{
				Source.Remove(resetTimerId);
				resetTimerId = 0u;
			}
		}

		private void UpdateErrorUndelines(ParsedDocument parsedDocument)
		{
			if (!options.UnderlineErrors || parsedDocument == null)
			{
				Application.Invoke(delegate
				{
					TextDocument textDocument = ((TextEditor != null) ? TextEditor.Document : null);
					if (textDocument != null)
					{
						RemoveErrorUnderlines(textDocument);
						UpdateQuickTasks(parsedDocument);
					}
				});
				return;
			}
			Application.Invoke(delegate
			{
				if (!quickTaskProvider.Contains(this))
				{
					AddQuickTaskProvider(this);
				}
				RemoveErrorUndelinesResetTimerId();
				resetTimerId = GLib.Timeout.Add(500u, delegate
				{
					if (!isDisposed)
					{
						TextDocument textDocument = ((TextEditor != null) ? TextEditor.Document : null);
						if (textDocument != null)
						{
							RemoveErrorUnderlines(textDocument);
							if (parsedDocument.Errors != null)
							{
								foreach (ICSharpCode.NRefactory.TypeSystem.Error error in parsedDocument.Errors)
								{
									UnderLineError(textDocument, error);
								}
							}
						}
					}
					resetTimerId = 0u;
					return false;
				});
				UpdateQuickTasks(parsedDocument);
			});
		}

		private void RemoveErrorUnderlines(TextDocument doc)
		{
			errors.ForEach(delegate(ErrorMarker err)
			{
				doc.RemoveMarker(err);
			});
			errors.Clear();
		}

		private void UnderLineError(TextDocument doc, ICSharpCode.NRefactory.TypeSystem.Error info)
		{
			DocumentLine line = doc.GetLine(info.Region.BeginLine);
			if (!errors.Any((ErrorMarker em) => em.LineSegment == line))
			{
				ErrorMarker errorMarker = new ErrorMarker(textEditor.Document, info, line);
				errors.Add(errorMarker);
				doc.AddMarker(line, errorMarker);
			}
		}

		public void Unsplit()
		{
			if (splitContainer != null)
			{
				double value = mainsw.Vadjustment.Value;
				double value2 = mainsw.Hadjustment.Value;
				splitContainer.Remove(mainsw);
				secondsw.Destroy();
				secondsw = null;
				splittedTextEditor = null;
				vbox.Remove(splitContainer);
				splitContainer.Destroy();
				splitContainer = null;
				RecreateMainSw();
				vbox.PackStart(mainsw, expand: true, fill: true, 0u);
				vbox.ShowAll();
				mainsw.Vadjustment.Value = value;
				mainsw.Hadjustment.Value = value2;
			}
		}

		public void SwitchWindow()
		{
			if (splittedTextEditor.HasFocus)
			{
				textEditor.GrabFocus();
			}
			else
			{
				splittedTextEditor.GrabFocus();
			}
		}

		public void Split(bool vSplit)
		{
			double value = mainsw.Vadjustment.Value;
			double value2 = mainsw.Hadjustment.Value;
			if (splitContainer != null)
			{
				Unsplit();
			}
			vbox.Remove(mainsw);
			RecreateMainSw();
			splitContainer = (vSplit ? ((Paned)new VPaned()) : ((Paned)new HPaned()));
			splitContainer.Add1(mainsw);
			splitContainer.ButtonPressEvent += delegate(object sender, ButtonPressEventArgs args)
			{
				if (args.Event.Type == EventType.TwoButtonPress && args.RetVal == null)
				{
					Unsplit();
				}
			};
			secondsw = new DecoratedScrolledWindow(this);
			splittedTextEditor = new ExtensibleTextEditor(view, textEditor.Options, textEditor.Document);
			splittedTextEditor.TextArea.FocusInEvent += delegate(object o, FocusInEventArgs s)
			{
				lastActiveEditor = (ExtensibleTextEditor)((TextArea)o).GetTextEditorData().Parent;
				view.FireCompletionContextChanged();
			};
			splittedTextEditor.TextArea.FocusOutEvent += delegate
			{
				if (!textEditor.TextArea.HasFocus)
				{
					OnLostFocus();
				}
			};
			splittedTextEditor.Extension = textEditor.Extension;
			if (textEditor.GetTextEditorData().HasIndentationTracker)
			{
				splittedTextEditor.GetTextEditorData().IndentationTracker = textEditor.GetTextEditorData().IndentationTracker;
			}
			splittedTextEditor.Document.BracketMatcher = textEditor.Document.BracketMatcher;
			secondsw.SetTextEditor(splittedTextEditor);
			splitContainer.Add2(secondsw);
			vbox.PackStart(splitContainer, expand: true, fill: true, 0u);
			splitContainer.Position = (vSplit ? vbox.Allocation.Height : vbox.Allocation.Width) / 2 - 1;
			vbox.ShowAll();
			Adjustment vadjustment = secondsw.Vadjustment;
			double value3 = (mainsw.Vadjustment.Value = value);
			vadjustment.Value = value3;
			Adjustment hadjustment = secondsw.Hadjustment;
			double value4 = (mainsw.Hadjustment.Value = value2);
			hadjustment.Value = value4;
		}

		private void RecreateMainSw()
		{
			double value = mainsw.Vadjustment.Value;
			double value2 = mainsw.Hadjustment.Value;
			TextEditor textEditor = mainsw.RemoveTextEditor();
			mainsw.Destroy();
			mainsw = new DecoratedScrolledWindow(this);
			mainsw.SetTextEditor(textEditor);
			mainsw.Vadjustment.Value = value;
			mainsw.Hadjustment.Value = value2;
			lastActiveEditor = this.textEditor;
		}

		internal static string EllipsizeMiddle(string str, int truncLen)
		{
			if (str == null)
			{
				return "";
			}
			if (str.Length <= truncLen)
			{
				return str;
			}
			string text = "...";
			int num = (truncLen - text.Length) / 2;
			int startIndex = str.Length - truncLen + num + text.Length;
			return str.Substring(0, num) + text + str.Substring(startIndex);
		}

		public void ShowFileChangedWarning(bool multiple)
		{
			RemoveMessageBar();
			if (messageBar == null)
			{
				messageBar = new InfoBar(MessageType.Warning);
				messageBar.SetMessageLabel(GettextCatalog.GetString("<b>The file \"{0}\" has been changed outside of {1}.</b>\nDo you want to keep your changes, or reload the file from disk?", EllipsizeMiddle(Document.FileName, 50), BrandingService.ApplicationName));
				Button button = new Button(GettextCatalog.GetString("_Reload from disk"));
				button.Image = ImageService.GetImage(Gtk.Stock.Refresh, IconSize.Button);
				button.Clicked += delegate
				{
					Reload();
					view.TextEditor.GrabFocus();
				};
				messageBar.ActionArea.Add(button);
				Button button2 = new Button(GettextCatalog.GetString("_Keep changes"));
				button2.Image = ImageService.GetImage(Gtk.Stock.Cancel, IconSize.Button);
				button2.Clicked += delegate
				{
					RemoveMessageBar();
					view.LastSaveTimeUtc = File.GetLastWriteTimeUtc(view.ContentName);
					view.WorkbenchWindow.ShowNotification = false;
				};
				messageBar.ActionArea.Add(button2);
				if (multiple)
				{
					Button button3 = new Button(GettextCatalog.GetString("_Reload all"));
					button3.Image = ImageService.GetImage(Gtk.Stock.Cancel, IconSize.Button);
					button3.Clicked += delegate
					{
						FileRegistry.ReloadAllChangedFiles();
					};
					messageBar.ActionArea.Add(button3);
					Button button4 = new Button(GettextCatalog.GetString("_Ignore all"));
					button4.Image = ImageService.GetImage(Gtk.Stock.Cancel, IconSize.Button);
					button4.Clicked += delegate
					{
						FileRegistry.IgnoreAllChangedFiles();
					};
					messageBar.ActionArea.Add(button4);
				}
			}
			view.IsDirty = true;
			view.WarnOverwrite = true;
			vbox.PackStart(messageBar, expand: false, fill: false, 0u);
			vbox.ReorderChild(messageBar, 0);
			messageBar.ShowAll();
			messageBar.QueueDraw();
			view.WorkbenchWindow.ShowNotification = true;
		}

		internal void UpdateEolMarkerMessage(bool multiple)
		{
			if (!UseIncorrectMarkers && DefaultSourceEditorOptions.Instance.LineEndingConversion != LineEndingConversion.LeaveAsIs)
			{
				ShowIncorretEolMarkers(Document.FileName, multiple);
			}
		}

		internal bool EnsureCorrectEolMarker(string fileName)
		{
			if (UseIncorrectMarkers || DefaultSourceEditorOptions.Instance.LineEndingConversion == LineEndingConversion.LeaveAsIs)
			{
				return true;
			}
			if (HasIncorrectEolMarker)
			{
				switch (DefaultSourceEditorOptions.Instance.LineEndingConversion)
				{
				case LineEndingConversion.Ask:
				{
					bool hasMultipleIncorretEolMarkers = FileRegistry.HasMultipleIncorretEolMarkers;
					ShowIncorretEolMarkers(fileName, hasMultipleIncorretEolMarkers);
					if (hasMultipleIncorretEolMarkers)
					{
						FileRegistry.UpdateEolMessages();
					}
					return false;
				}
				case LineEndingConversion.ConvertAlways:
					ConvertLineEndings();
					return true;
				default:
					return true;
				}
			}
			return true;
		}

		internal void ConvertLineEndings()
		{
			string defaultEolMarker = TextEditor.Options.DefaultEolMarker;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (DocumentLine line in Document.Lines)
			{
				stringBuilder.Append(TextEditor.GetTextAt(num, line.Length));
				num += line.LengthIncludingDelimiter;
				if (line.DelimiterLength > 0)
				{
					stringBuilder.Append(defaultEolMarker);
				}
			}
			view.StoreSettings();
			view.ReplaceContent(Document.FileName, stringBuilder.ToString(), view.SourceEncoding);
			Document.HasLineEndingMismatchOnTextSet = false;
			view.LoadSettings();
		}

		private static string GetEolString(string detectedEol)
		{
			switch (detectedEol)
			{
			case "\n":
				return "UNIX";
			case "\r\n":
				return "Windows";
			case "\r":
				return "Mac";
			case "?":
				return "mixed";
			default:
				return "Unknown";
			}
		}

		private void ShowIncorretEolMarkers(string fileName, bool multiple)
		{
			RemoveMessageBar();
			messageOverlayWindow = new OverlayMessageWindow();
			HBox hbox = new HBox();
			hbox.Spacing = 8;
			HoverCloseButton image = new HoverCloseButton();
			hbox.PackStart(image, expand: false, fill: false, 0u);
			Label label = new Label($"This file has line endings ({GetEolString(DetectedEolMarker)}) which differ from the policy settings ({GetEolString(textEditor.Options.DefaultEolMarker)}).");
			HslColor hslColor = textEditor.ColorStyle.NotificationText.Foreground;
			label.ModifyFg(StateType.Normal, hslColor);
			label.Layout.GetPixelSize(out var w, out var _);
			label.Ellipsize = EllipsizeMode.End;
			hbox.PackStart(label, expand: true, fill: true, 0u);
			Button okButton = new Button(Gtk.Stock.Ok);
			okButton.WidthRequest = 60;
			VBox vBox = new VBox();
			vBox.PackEnd(okButton, expand: true, fill: true, 2u);
			hbox.PackEnd(vBox, expand: false, fill: false, 0u);
			List<string> list = new List<string>();
			list.Add($"Convert to {GetEolString(textEditor.Options.DefaultEolMarker)} line endings");
			list.Add($"Convert all files to {GetEolString(textEditor.Options.DefaultEolMarker)} line endings");
			list.Add($"Keep {GetEolString(DetectedEolMarker)} line endings");
			list.Add($"Keep {GetEolString(DetectedEolMarker)} line endings in all files");
			ComboBox combo = new ComboBox(list.ToArray());
			combo.Active = 0;
			hbox.PackEnd(combo, expand: false, fill: false, 0u);
			HBox hBox = new HBox();
			hBox.PackStart(hbox, expand: true, fill: true, 8u);
			messageOverlayWindow.Child = hBox;
			messageOverlayWindow.ShowOverlay(TextEditor);
			GLib.Timeout.Add(100u, delegate
			{
				combo.QueueResize();
				return false;
			});
			messageOverlayWindow.SizeFunc = () => okButton.SizeRequest().Width + combo.SizeRequest().Width + image.SizeRequest().Width + w + hbox.Spacing * 4 + 16;
			image.Clicked += delegate
			{
				UseIncorrectMarkers = true;
				view.WorkbenchWindow.ShowNotification = false;
				RemoveMessageBar();
			};
			okButton.Clicked += delegate
			{
				switch (combo.Active)
				{
				case 0:
					ConvertLineEndings();
					view.WorkbenchWindow.ShowNotification = false;
					view.Save(fileName, view.SourceEncoding);
					break;
				case 1:
					FileRegistry.ConvertLineEndingsInAllFiles();
					break;
				case 2:
					UseIncorrectMarkers = true;
					view.WorkbenchWindow.ShowNotification = false;
					break;
				case 3:
					FileRegistry.IgnoreLineEndingsInAllFiles();
					break;
				}
				RemoveMessageBar();
			};
		}

		public void ShowAutoSaveWarning(string fileName)
		{
			RemoveMessageBar();
			TextEditor.Visible = false;
			if (messageBar == null)
			{
				messageBar = new InfoBar(MessageType.Warning);
				messageBar.SetMessageLabel(BrandingService.BrandApplicationName(GettextCatalog.GetString("<b>An autosave file has been found for this file.</b>\nThis could mean that another instance of MonoDevelop is editing this file, or that MonoDevelop crashed with unsaved changes.\n\nDo you want to use the original file, or load from the autosave file?")));
				Button button = new Button(GettextCatalog.GetString("_Use original file"));
				button.Image = ImageService.GetImage(Gtk.Stock.Refresh, IconSize.Button);
				button.Clicked += delegate
				{
					try
					{
						AutoSave.RemoveAutoSaveFile(fileName);
						TextEditor.GrabFocus();
						view.Load(fileName);
						view.WorkbenchWindow.Document.ReparseDocument();
					}
					catch (Exception ex)
					{
						LoggingService.LogError("Could not remove the autosave file.", ex);
					}
					finally
					{
						RemoveMessageBar();
					}
				};
				messageBar.ActionArea.Add(button);
				Button button2 = new Button(GettextCatalog.GetString("_Load from autosave"));
				button2.Image = ImageService.GetImage(Gtk.Stock.RevertToSaved, IconSize.Button);
				button2.Clicked += delegate
				{
					try
					{
						string content = AutoSave.LoadAutoSave(fileName);
						AutoSave.RemoveAutoSaveFile(fileName);
						TextEditor.GrabFocus();
						view.Load(fileName);
						view.ReplaceContent(fileName, content, view.SourceEncoding);
						view.WorkbenchWindow.Document.ReparseDocument();
						view.IsDirty = true;
					}
					catch (Exception ex)
					{
						LoggingService.LogError("Could not remove the autosave file.", ex);
					}
					finally
					{
						RemoveMessageBar();
					}
				};
				messageBar.ActionArea.Add(button2);
			}
			view.IsDirty = true;
			view.WarnOverwrite = true;
			vbox.PackStart(messageBar, expand: false, fill: false, 0u);
			vbox.ReorderChild(messageBar, 0);
			messageBar.ShowAll();
			messageBar.QueueDraw();
		}

		public void RemoveMessageBar()
		{
			if (messageBar != null)
			{
				if (messageBar.Parent == vbox)
				{
					vbox.Remove(messageBar);
				}
				messageBar.Destroy();
				messageBar = null;
			}
			if (!TextEditor.Visible)
			{
				TextEditor.Visible = true;
			}
			if (messageOverlayWindow != null)
			{
				messageOverlayWindow.Destroy();
				messageOverlayWindow = null;
			}
		}

		public void Reload()
		{
			try
			{
				if (File.Exists(view.ContentName))
				{
					view.StoreSettings();
					reloadSettings = true;
					view.Load(view.ContentName, view.SourceEncoding, reload: true);
					view.WorkbenchWindow.ShowNotification = false;
				}
			}
			catch (Exception ex)
			{
				MessageService.ShowError("Could not reload the file.", ex);
			}
			finally
			{
				RemoveMessageBar();
			}
		}

		private void CaretPositionChanged(object o, DocumentLocationEventArgs args)
		{
			UpdateLineCol();
			DocumentLine line = TextEditor.Document.GetLine(TextEditor.Caret.Line);
			MessageBubbleTextMarker messageBubbleTextMarker = null;
			if (line != null && line.Markers.Any((TextLineMarker m) => m is MessageBubbleTextMarker))
			{
				messageBubbleTextMarker = (MessageBubbleTextMarker)line.Markers.First((TextLineMarker m) => m is MessageBubbleTextMarker);
			}
			if (oldExpandedMarker != null)
			{
				_ = oldExpandedMarker;
			}
			oldExpandedMarker = messageBubbleTextMarker;
		}

		internal void UpdateLineCol()
		{
		}

		private bool KillWidgets()
		{
			bool result = false;
			if (searchAndReplaceWidgetFrame != null)
			{
				searchAndReplaceWidgetFrame.Destroy();
				searchAndReplaceWidgetFrame = null;
				searchAndReplaceWidget = null;
				result = true;
				IdeApp.Workbench.StatusBar.ShowReady();
			}
			if (gotoLineNumberWidgetFrame != null)
			{
				gotoLineNumberWidgetFrame.Destroy();
				gotoLineNumberWidgetFrame = null;
				gotoLineNumberWidget = null;
				result = true;
			}
			if (textEditor != null)
			{
				textEditor.HighlightSearchPattern = false;
			}
			if (splittedTextEditor != null)
			{
				splittedTextEditor.HighlightSearchPattern = false;
			}
			if (!isDisposed)
			{
				ResetFocusChain();
			}
			return result;
		}

		internal bool RemoveSearchWidget()
		{
			bool result = KillWidgets();
			if (!isDisposed)
			{
				TextEditor.GrabFocus();
			}
			return result;
		}

		public void EmacsFindNext()
		{
			if (searchAndReplaceWidget == null)
			{
				ShowSearchWidget();
			}
			else
			{
				FindNext();
			}
		}

		public void EmacsFindPrevious()
		{
			if (searchAndReplaceWidget == null)
			{
				ShowSearchWidget();
			}
			else
			{
				FindPrevious();
			}
		}

		public void ShowSearchWidget()
		{
			ShowSearchReplaceWidget(replace: false);
		}

		public void ShowReplaceWidget()
		{
			ShowSearchReplaceWidget(replace: true);
		}

		internal void OnUpdateUseSelectionForFind(CommandInfo info)
		{
			info.Enabled = searchAndReplaceWidget != null && TextEditor.IsSomethingSelected;
		}

		public void UseSelectionForFind()
		{
			SetSearchPatternToSelection();
		}

		internal void OnUpdateUseSelectionForReplace(CommandInfo info)
		{
			info.Enabled = searchAndReplaceWidget != null && TextEditor.IsSomethingSelected;
		}

		public void UseSelectionForReplace()
		{
			SetReplacePatternToSelection();
		}

		private void ShowSearchReplaceWidget(bool replace)
		{
			if (searchAndReplaceWidget == null)
			{
				KillWidgets();
				searchAndReplaceWidgetFrame = new RoundedFrame();
				searchAndReplaceWidgetFrame.SetFillColor(CairoExtensions.GdkColorToCairoColor(vbox.Style.Background(StateType.Normal)));
				searchAndReplaceWidgetFrame.Child = (searchAndReplaceWidget = new SearchAndReplaceWidget(TextEditor, searchAndReplaceWidgetFrame));
				searchAndReplaceWidget.Destroyed += delegate
				{
					RemoveSearchWidget();
				};
				searchAndReplaceWidgetFrame.ShowAll();
				TextEditor.AddAnimatedWidget(searchAndReplaceWidgetFrame, 300u, Easing.ExponentialInOut, Blocking.Downstage, TextEditor.Allocation.Width - 400, -searchAndReplaceWidget.Allocation.Height);
				if (splittedTextEditor != null)
				{
					splittedTextEditor.HighlightSearchPattern = true;
					splittedTextEditor.TextViewMargin.RefreshSearchMarker();
				}
				ResetFocusChain();
			}
			else if (TextEditor.IsSomethingSelected)
			{
				searchAndReplaceWidget.SetSearchPattern();
			}
			searchAndReplaceWidget.UpdateSearchPattern();
			searchAndReplaceWidget.IsReplaceMode = replace;
			if (searchAndReplaceWidget.SearchFocused)
			{
				if (replace)
				{
					searchAndReplaceWidget.Replace();
				}
				else
				{
					FindNext();
				}
			}
			searchAndReplaceWidget.Focus();
		}

		public void ShowGotoLineNumberWidget()
		{
			if (gotoLineNumberWidget == null)
			{
				KillWidgets();
				gotoLineNumberWidgetFrame = new RoundedFrame();
				gotoLineNumberWidgetFrame.SetFillColor(CairoExtensions.GdkColorToCairoColor(vbox.Style.Background(StateType.Normal)));
				gotoLineNumberWidgetFrame.Child = (gotoLineNumberWidget = new GotoLineNumberWidget(textEditor, gotoLineNumberWidgetFrame));
				gotoLineNumberWidget.Destroyed += delegate
				{
					RemoveSearchWidget();
				};
				gotoLineNumberWidgetFrame.ShowAll();
				TextEditor.AddAnimatedWidget(gotoLineNumberWidgetFrame, 300u, Easing.ExponentialInOut, Blocking.Downstage, TextEditor.Allocation.Width - 400, -gotoLineNumberWidget.Allocation.Height);
				ResetFocusChain();
			}
			gotoLineNumberWidget.Focus();
		}

		public Mono.TextEditor.SearchResult FindNext()
		{
			return FindNext(focus: true);
		}

		public Mono.TextEditor.SearchResult FindNext(bool focus)
		{
			return SearchAndReplaceWidget.FindNext(TextEditor);
		}

		public Mono.TextEditor.SearchResult FindPrevious()
		{
			return FindPrevious(focus: true);
		}

		public Mono.TextEditor.SearchResult FindPrevious(bool focus)
		{
			return SearchAndReplaceWidget.FindPrevious(TextEditor);
		}

		internal static string FormatPatternToSelectionOption(string pattern)
		{
			return FindInFilesDialog.FormatPatternToSelectionOption(pattern, SearchAndReplaceWidget.SearchEngine == "regex");
		}

		private void SetSearchPatternToSelection()
		{
			if (!TextEditor.IsSomethingSelected)
			{
				int num = textEditor.Caret.Offset;
				int i = num;
				while (num - 1 >= 0 && DynamicAbbrevHandler.IsIdentifierPart(textEditor.GetCharAt(num - 1)))
				{
					num--;
				}
				for (; i < textEditor.Length && DynamicAbbrevHandler.IsIdentifierPart(textEditor.GetCharAt(i)); i++)
				{
				}
				textEditor.Caret.Offset = i;
				TextEditor.SetSelection(num, i);
			}
			if (TextEditor.IsSomethingSelected)
			{
				string searchPattern = FormatPatternToSelectionOption(TextEditor.SelectedText);
				SearchAndReplaceOptions.SearchPattern = searchPattern;
				SearchAndReplaceWidget.UpdateSearchHistory(TextEditor.SearchPattern);
			}
			if (searchAndReplaceWidget != null)
			{
				searchAndReplaceWidget.UpdateSearchPattern();
			}
		}

		private void SetReplacePatternToSelection()
		{
			if (searchAndReplaceWidget != null && TextEditor.IsSomethingSelected)
			{
				searchAndReplaceWidget.ReplacePattern = TextEditor.SelectedText;
			}
		}

		public Mono.TextEditor.SearchResult FindNextSelection()
		{
			SetSearchPatternToSelection();
			TextEditor.GrabFocus();
			return FindNext();
		}

		public Mono.TextEditor.SearchResult FindPreviousSelection()
		{
			SetSearchPatternToSelection();
			TextEditor.GrabFocus();
			return FindPrevious();
		}

		internal void MonodocResolver()
		{
			ResolveResult languageItem = TextEditor.GetLanguageItem(TextEditor.Caret.Offset, out var _);
			if (languageItem is UnknownIdentifierResolveResult)
			{
				UnknownIdentifierResolveResult unknownIdentifierResolveResult = (UnknownIdentifierResolveResult)languageItem;
				IdeApp.HelpOperations.SearchHelpFor(unknownIdentifierResolveResult.Identifier);
				return;
			}
			if (languageItem is UnknownMemberResolveResult)
			{
				UnknownMemberResolveResult unknownMemberResolveResult = (UnknownMemberResolveResult)languageItem;
				IdeApp.HelpOperations.SearchHelpFor(unknownMemberResolveResult.MemberName);
				return;
			}
			string monoDocHelpUrl = HelpService.GetMonoDocHelpUrl(languageItem);
			if (monoDocHelpUrl != null)
			{
				IdeApp.HelpOperations.ShowHelp(monoDocHelpUrl);
			}
		}

		internal void MonodocResolverUpdate(CommandInfo cinfo)
		{
			ResolveResult languageItem = TextEditor.GetLanguageItem(TextEditor.Caret.Offset, out var _);
			if (languageItem == null || (!IdeApp.HelpOperations.CanShowHelp(languageItem) && !(languageItem is UnknownIdentifierResolveResult) && !(languageItem is UnknownMemberResolveResult)))
			{
				cinfo.Bypass = true;
			}
		}

		internal void OnUpdateToggleComment(CommandInfo info)
		{
			List<string> value;
			List<string> value2;
			List<string> value3;
			if (!(Document.SyntaxMode is SyntaxMode syntaxMode))
			{
				info.Visible = false;
			}
			else if (syntaxMode.Properties.TryGetValue("LineComment", out value))
			{
				info.Visible = value.Count > 0;
			}
			else if (syntaxMode.Properties.TryGetValue("BlockCommentStart", out value2) && syntaxMode.Properties.TryGetValue("BlockCommentEnd", out value3))
			{
				info.Visible = value2.Count > 0 && value3.Count > 0;
			}
		}

		private void ToggleCodeCommentWithBlockComments()
		{
			if (!(Document.SyntaxMode is SyntaxMode syntaxMode) || !syntaxMode.Properties.TryGetValue("BlockCommentStart", out var value) || value.Count == 0 || !syntaxMode.Properties.TryGetValue("BlockCommentEnd", out var value2) || value2.Count == 0)
			{
				return;
			}
			string text = value[0];
			string text2 = value2[0];
			using (Document.OpenUndoGroup())
			{
				DocumentLine documentLine;
				DocumentLine documentLine2;
				if (TextEditor.IsSomethingSelected)
				{
					documentLine = Document.GetLineByOffset(textEditor.SelectionRange.Offset);
					documentLine2 = Document.GetLineByOffset(textEditor.SelectionRange.EndOffset);
				}
				else
				{
					documentLine = (documentLine2 = Document.GetLine(textEditor.Caret.Line));
				}
				string textAt = Document.GetTextAt(documentLine.Offset, documentLine.Length);
				string textAt2 = Document.GetTextAt(documentLine2.Offset, documentLine2.Length);
				if (textAt.StartsWith(text) && textAt2.EndsWith(text2, StringComparison.Ordinal))
				{
					textEditor.Remove(documentLine2.Offset + documentLine2.Length - text2.Length, text2.Length);
					textEditor.Remove(documentLine.Offset, text.Length);
					if (TextEditor.IsSomethingSelected)
					{
						TextEditor.SelectionAnchor -= text2.Length;
					}
				}
				else
				{
					textEditor.Insert(documentLine2.Offset + documentLine2.Length, text2);
					textEditor.Insert(documentLine.Offset, text);
					if (TextEditor.IsSomethingSelected)
					{
						TextEditor.SelectionAnchor += text2.Length;
					}
				}
			}
		}

		private bool TryGetLineCommentTag(out string commentTag)
		{
			if (!(Document.SyntaxMode is SyntaxMode syntaxMode) || !syntaxMode.Properties.TryGetValue("LineComment", out var value) || value.Count == 0)
			{
				commentTag = null;
				return false;
			}
			commentTag = value[0];
			return true;
		}

		public void ToggleCodeComment()
		{
			if (!TryGetLineCommentTag(out var commentTag))
			{
				ToggleCodeCommentWithBlockComments();
				return;
			}
			bool flag = false;
			foreach (DocumentLine selectedLine in textEditor.SelectedLines)
			{
				if (selectedLine.GetIndentation(TextEditor.Document).Length != selectedLine.Length)
				{
					string textAt = Document.GetTextAt(selectedLine);
					string text = textAt.TrimStart();
					if (!text.StartsWith(commentTag, StringComparison.Ordinal))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				CommentSelectedLines(commentTag);
			}
			else
			{
				UncommentSelectedLines(commentTag);
			}
		}

		public void AddCodeComment()
		{
			if (TryGetLineCommentTag(out var commentTag))
			{
				CommentSelectedLines(commentTag);
			}
		}

		public void RemoveCodeComment()
		{
			if (TryGetLineCommentTag(out var commentTag))
			{
				UncommentSelectedLines(commentTag);
			}
		}

		public void OnUpdateToggleErrorTextMarker(CommandInfo info)
		{
			DocumentLine line = TextEditor.Document.GetLine(TextEditor.Caret.Line);
			if (line == null)
			{
				info.Visible = false;
				return;
			}
			MessageBubbleTextMarker messageBubbleTextMarker = (MessageBubbleTextMarker)line.Markers.FirstOrDefault((TextLineMarker m) => m is MessageBubbleTextMarker);
			info.Visible = messageBubbleTextMarker != null;
		}

		public void OnToggleErrorTextMarker()
		{
			DocumentLine line = TextEditor.Document.GetLine(TextEditor.Caret.Line);
			if (line != null)
			{
				MessageBubbleTextMarker messageBubbleTextMarker = (MessageBubbleTextMarker)line.Markers.FirstOrDefault((TextLineMarker m) => m is MessageBubbleTextMarker);
				if (messageBubbleTextMarker != null)
				{
					messageBubbleTextMarker.IsVisible = !messageBubbleTextMarker.IsVisible;
					TextEditor.QueueDraw();
				}
			}
		}

		private void CommentSelectedLines(string commentTag)
		{
			int start = (TextEditor.IsSomethingSelected ? Document.OffsetToLineNumber(TextEditor.SelectionRange.Offset) : TextEditor.Caret.Line);
			int num = (TextEditor.IsSomethingSelected ? Document.OffsetToLineNumber(TextEditor.SelectionRange.EndOffset) : TextEditor.Caret.Line);
			if (num < 0)
			{
				num = Document.LineCount;
			}
			DocumentLine documentLine = (TextEditor.IsSomethingSelected ? TextEditor.Document.GetLineByOffset(TextEditor.SelectionAnchor) : null);
			int num2 = (TextEditor.IsSomethingSelected ? (TextEditor.SelectionAnchor - documentLine.Offset) : (-1));
			using (Document.OpenUndoGroup())
			{
				foreach (DocumentLine selectedLine in TextEditor.SelectedLines)
				{
					TextEditor.Insert(selectedLine.Offset, commentTag);
				}
				if (TextEditor.IsSomethingSelected)
				{
					if (TextEditor.SelectionAnchor < TextEditor.Caret.Offset)
					{
						if (num2 != 0)
						{
							TextEditor.SelectionAnchor = Math.Min(documentLine.Offset + documentLine.Length, Math.Max(documentLine.Offset, TextEditor.SelectionAnchor + commentTag.Length));
						}
					}
					else if (num2 != 0)
					{
						TextEditor.SelectionAnchor = Math.Min(documentLine.Offset + documentLine.Length, Math.Max(documentLine.Offset, documentLine.Offset + num2 + commentTag.Length));
					}
				}
				if (TextEditor.IsSomethingSelected)
				{
					TextEditor.ExtendSelectionTo(TextEditor.Caret.Offset);
				}
			}
			Document.CommitMultipleLineUpdate(start, num);
		}

		private void UncommentSelectedLines(string commentTag)
		{
			int start = (TextEditor.IsSomethingSelected ? Document.OffsetToLineNumber(TextEditor.SelectionRange.Offset) : TextEditor.Caret.Line);
			int num = (TextEditor.IsSomethingSelected ? Document.OffsetToLineNumber(TextEditor.SelectionRange.EndOffset) : TextEditor.Caret.Line);
			if (num < 0)
			{
				num = Document.LineCount;
			}
			DocumentLine documentLine = (TextEditor.IsSomethingSelected ? TextEditor.Document.GetLineByOffset(TextEditor.SelectionAnchor) : null);
			int num2 = (TextEditor.IsSomethingSelected ? (TextEditor.SelectionAnchor - documentLine.Offset) : (-1));
			using (Document.OpenUndoGroup())
			{
				int num3 = -1;
				int num4 = 0;
				foreach (DocumentLine selectedLine in TextEditor.SelectedLines)
				{
					string textAt = Document.GetTextAt(selectedLine);
					string text = textAt.TrimStart();
					int num5 = 0;
					if (text.StartsWith(commentTag))
					{
						TextEditor.Remove(selectedLine.Offset + (textAt.Length - text.Length), commentTag.Length);
						num5 = commentTag.Length;
					}
					num4 = num5;
					if (num3 < 0)
					{
						num3 = num4;
					}
				}
				if (TextEditor.IsSomethingSelected)
				{
					if (TextEditor.SelectionAnchor < TextEditor.Caret.Offset)
					{
						TextEditor.SelectionAnchor = Math.Min(documentLine.Offset + documentLine.Length, Math.Max(documentLine.Offset, TextEditor.SelectionAnchor - num3));
					}
					else
					{
						TextEditor.SelectionAnchor = Math.Min(documentLine.Offset + documentLine.Length, Math.Max(documentLine.Offset, documentLine.Offset + num2 - num4));
					}
				}
				if (TextEditor.IsSomethingSelected)
				{
					TextEditor.ExtendSelectionTo(TextEditor.Caret.Offset);
				}
			}
			Document.CommitMultipleLineUpdate(start, num);
		}

		protected virtual void OnTasksUpdated(EventArgs e)
		{
			TasksUpdated?.Invoke(this, e);
		}

		private void UpdateQuickTasks(ParsedDocument doc)
		{
			tasks.Clear();
			if (doc != null)
			{
				foreach (Tag tagComment in doc.TagComments)
				{
					QuickTask item = new QuickTask(tagComment.Text, tagComment.Region.Begin, Severity.Hint);
					tasks.Add(item);
				}
				foreach (ICSharpCode.NRefactory.TypeSystem.Error error in doc.Errors)
				{
					QuickTask item2 = new QuickTask(error.Message, error.Region.Begin, (error.ErrorType == ErrorType.Error) ? Severity.Error : Severity.Warning);
					tasks.Add(item2);
				}
			}
			OnTasksUpdated(EventArgs.Empty);
		}

		internal void NextIssue()
		{
			if ((bool)QuickTaskStrip.EnableFancyFeatures)
			{
				mainsw.Strip.GotoTask(mainsw.Strip.SearchNextTask(QuickTaskStrip.HoverMode.NextMessage));
			}
		}

		internal void PrevIssue()
		{
			if ((bool)QuickTaskStrip.EnableFancyFeatures)
			{
				mainsw.Strip.GotoTask(mainsw.Strip.SearchPrevTask(QuickTaskStrip.HoverMode.NextMessage));
			}
		}

		internal void NextIssueError()
		{
			if ((bool)QuickTaskStrip.EnableFancyFeatures)
			{
				mainsw.Strip.GotoTask(mainsw.Strip.SearchNextTask(QuickTaskStrip.HoverMode.NextError));
			}
		}

		internal void PrevIssueError()
		{
			if ((bool)QuickTaskStrip.EnableFancyFeatures)
			{
				mainsw.Strip.GotoTask(mainsw.Strip.SearchPrevTask(QuickTaskStrip.HoverMode.NextError));
			}
		}
	}
}
